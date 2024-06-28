using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Client.Differences.Graphic;
using UnityEngine;

namespace MortiseFrame.Compass {

    public static class PathFindingCore {

        static List<Vector2> openList = new List<Vector2>();
        static List<Vector2> closedList = new List<Vector2>();
        static List<Vector2> path = new List<Vector2>();
        static Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>();
        static Dictionary<Vector2, float> fMap = new Dictionary<Vector2, float>();
        static Vector2[] neighbours = new Vector2[8];

        public static int FindPath(Vector2 startGrid, Vector2 endGrid, Func<int, int, bool> walkable, int mapWidth, int mapHeight, PathFindingDirection directionMode, bool cornerWalkable, Vector2[] path) {
            // 初始化
            openList.Clear();
            closedList.Clear();
            parentMap.Clear();
            fMap.Clear();

            if (walkable((int)endGrid.x, (int)endGrid.y) == false) {
                return 0;
            }

            // 添加起始点到openList
            openList.Add(startGrid);

            // 设置当前点
            var current = startGrid;

            // 计算 F
            CalculateF(startGrid, endGrid);

            // OpenList不为空时循环
            while (openList.Count > 0) {

                // 从openList中找到F值最小的点，作为当前点
                current = GetMinFGrid();

                // 如果当前点是终点，结束
                if (current == endGrid) {
                    break;
                }

                // 将当前点从openList中移除
                openList.Remove(current);

                // 将当前点加入closedList
                closedList.Add(current);

                // 找到当前点附近的点
                int neighboursLen = GetNeighbours(current, walkable, mapWidth, mapHeight, directionMode, cornerWalkable);
                for (int i = 0; i < neighboursLen; i++) {

                    Vector2 neighbour = neighbours[i];
                    if (closedList.Contains(neighbour)) {
                        continue;
                    }

                    // 如果不在openList中，加入openList，并设置父节点，计算F值
                    if (!openList.Contains(neighbour)) {
                        openList.Add(neighbour);
                        parentMap[neighbour] = current;
                        fMap[neighbour] = GetF(startGrid, endGrid, neighbour);
                    } else {
                        // 如果在openList中，计算新的G值，如果比原来的小，更新F值，更新父节点
                        if (GetG(startGrid, current) + 1 < GetG(startGrid, neighbour)) {
                            fMap[neighbour] = GetF(startGrid, endGrid, neighbour);
                            parentMap[neighbour] = current;
                        }
                    }
                }

            }
            // 从目标开始回溯父节点，直到父节点==起始点
            var index = 0;
            while (current != startGrid && index < path.Length) {
                path[index] = current;
                current = parentMap[current];
                index++;
            }
            if (index < path.Length) {
                path[index] = startGrid;
                index++;
            }
            Array.Reverse(path);
            return index;
        }

        static Vector2 GetMinFGrid() {
            Vector2 minFGrid = openList[0];
            float minF = fMap[minFGrid];
            foreach (Vector2 point in openList) {
                if (fMap[point] < minF) {
                    minF = fMap[point];
                    minFGrid = point;
                }
            }
            return minFGrid;
        }

        static void CalculateF(Vector2 start, Vector2 end) {
            foreach (Vector2 point in openList) {
                fMap[point] = GetF(start, end, point);
            }
        }

        static float GetF(Vector2 start, Vector2 end, Vector2 point) {
            return GetG(start, point) + GetH(point, end);
        }

        // 从起始点到此点的路径长度
        static float GetG(Vector2 start, Vector2 point) {
            if (parentMap.ContainsKey(point)) {
                return GetG(start, parentMap[point]) + 1;
            } else {
                return (int)(Mathf.Abs(start.x - point.x) + Mathf.Abs(start.y - point.y));
            }
        }

        // 从此点到终点的直线距离
        static float GetH(Vector2 point, Vector2 end) {
            return Vector2.Distance(point, end);
        }

        static int[,] directions = new int[,] {
            {-1,  0},
            { 1,  0},
            { 0, -1},
            { 0,  1},
            {-1, -1},
            {-1,  1},
            { 1, -1},
            { 1,  1}
        };

        static int GetNeighbours(Vector2 point,
                                           Func<int, int, bool> walkable,
                                           int mapWidth,
                                           int mapHeight,
                                           PathFindingDirection directionMode,
                                           bool cornerWalkable) {
            int x = (int)point.x;
            int y = (int)point.y;

            int directionsLength = directionMode == PathFindingDirection.FourDirections ? 4 : 8;
            int index = 0;
            for (int i = 0; i < directionsLength; i++) {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                if (newX >= 0 && newX < mapWidth && newY >= 0 && newY < mapHeight && walkable(newX, newY)) {

                    if (i > 3 && cornerWalkable == false) {
                        if (i == 4 && (!walkable(x - 1, y) || !walkable(x, y - 1))) {
                            continue;
                        }
                        if (i == 5 && (!walkable(x - 1, y) || !walkable(x, y + 1))) {
                            continue;
                        }
                        if (i == 6 && (!walkable(x + 1, y) || !walkable(x, y - 1))) {
                            continue;
                        }
                        if (i == 7 && (!walkable(x + 1, y) || !walkable(x, y + 1))) {
                            continue;
                        }
                    }
                    neighbours[index] = new Vector2(newX, newY);
                    index++;
                }
            }
            return index;
        }
    }
}
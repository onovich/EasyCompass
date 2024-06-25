using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Compass {

    public class PathFindingCore {

        List<Vector2> openList;
        List<Vector2> closedList;
        List<Vector2> path;
        Dictionary<Vector2, Vector2> parentMap;
        Dictionary<Vector2, float> fMap;

        public PathFindingCore() {
            openList = new List<Vector2>();
            closedList = new List<Vector2>();
            path = new List<Vector2>();
            parentMap = new Dictionary<Vector2, Vector2>();
            fMap = new Dictionary<Vector2, float>();
        }

        public List<Vector2> FindPath(Vector2 startGrid, Vector2 endGrid, bool[,] map) {
            // 初始化
            openList.Clear();
            closedList.Clear();
            path.Clear();
            parentMap.Clear();
            fMap.Clear();

            if (map[(int)endGrid.x, (int)endGrid.y] == false) {
                return path;
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
                List<Vector2> neighbours = GetNeighbours(current, map);
                foreach (Vector2 neighbour in neighbours) {

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
            while (current != startGrid) {
                path.Add(current);
                current = parentMap[current];
            }
            path.Add(startGrid);
            return path;
        }

        Vector2 GetMinFGrid() {
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

        void CalculateF(Vector2 start, Vector2 end) {
            foreach (Vector2 point in openList) {
                fMap[point] = GetF(start, end, point);
            }
        }

        float GetF(Vector2 start, Vector2 end, Vector2 point) {
            return GetG(start, point) + GetH(point, end);
        }

        // 从起始点到此点的路径长度
        float GetG(Vector2 start, Vector2 point) {
            if (parentMap.ContainsKey(point)) {
                return GetG(start, parentMap[point]) + 1;
            } else {
                return (int)(Mathf.Abs(start.x - point.x) + Mathf.Abs(start.y - point.y));
            }
        }

        // 从此点到终点的直线距离
        float GetH(Vector2 point, Vector2 end) {
            return Vector2.Distance(point, end);
        }

        List<Vector2> GetNeighbours(Vector2 point, bool[,] map) {
            List<Vector2> neighbours = new List<Vector2>();

            int x = (int)point.x;
            int y = (int)point.y;

            if (x - 1 >= 0 && map[x - 1, y] == true) {
                neighbours.Add(new Vector2(x - 1, y));
            }

            if (x + 1 < map.GetLength(0) && map[x + 1, y] == true) {
                neighbours.Add(new Vector2(x + 1, y));
            }

            if (y - 1 >= 0 && map[x, y - 1] == true) {
                neighbours.Add(new Vector2(x, y - 1));
            }

            if (y + 1 < map.GetLength(1) && map[x, y + 1] == true) {
                neighbours.Add(new Vector2(x, y + 1));
            }

            return neighbours;
        }
    }
}
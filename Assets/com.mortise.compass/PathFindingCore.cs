using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Compass {

    public class PathFindingCore {

        List<Vector2> openList;
        List<Vector2> closedList;

        public PathFindingCore() {
            openList = new List<Vector2>();
            closedList = new List<Vector2>();
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 end, int[,] map) {

            //1. 将起点加入 closedList
            closedList.Add(start);

            // 2. 重复以下过程
            while (true) {
                // 2.1 获取 closedList 中的第一个点 S
                Vector2 s = closedList[0];
                // 2.2 获取 S 相邻点中符合条件的点，加入 openList
                List<Vector2> neighbours = GetNeighbours(s, map);
                foreach (Vector2 neighbour in neighbours) {
                    if (!openList.Contains(neighbour) && !closedList.Contains(neighbour)) {
                        openList.Add(neighbour);
                    }
                }
                // 2.3 计算 openList 中 F 值最小的点 T
                Vector2 t = openList[0];
                foreach (Vector2 point in openList) {
                    if (GetF(start, end, point) < GetF(start, end, t)) {
                        t = point;
                    }
                }
                // 2.4 将 T 从 openList 移除，加入 closedList
                openList.Remove(t);
                closedList.Add(t);
                // 2.5 直到 T 为终点，或者 openList 为空
                if (t == end || openList.Count == 0) {
                    break;
                }
            }
            return closedList;
        }

        public int GetF(Vector2 start, Vector2 end, Vector2 point) {
            return GetG(start, point) + GetH(point, end);
        }

        public int GetG(Vector2 start, Vector2 point) {
            return (int)(Mathf.Abs(start.x - point.x) + Mathf.Abs(start.y - point.y));
        }

        public int GetH(Vector2 point, Vector2 end) {
            return (int)(Mathf.Abs(point.x - end.x) + Mathf.Abs(point.y - end.y));
        }

        public List<Vector2> GetNeighbours(Vector2 point, int[,] map) {
            List<Vector2> neighbours = new List<Vector2>();

            int x = (int)point.x;
            int y = (int)point.y;

            if (x - 1 >= 0 && map[x - 1, y] == 0) {
                neighbours.Add(new Vector2(x - 1, y));
            }

            if (x + 1 < map.GetLength(0) && map[x + 1, y] == 0) {
                neighbours.Add(new Vector2(x + 1, y));
            }

            if (y - 1 >= 0 && map[x, y - 1] == 0) {
                neighbours.Add(new Vector2(x, y - 1));
            }

            if (y + 1 < map.GetLength(1) && map[x, y + 1] == 0) {
                neighbours.Add(new Vector2(x, y + 1));
            }

            return neighbours;
        }
    }

}
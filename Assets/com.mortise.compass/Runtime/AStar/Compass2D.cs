using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Compass {
    public class Compass2D {

        readonly PriorityQueue<Node2D> openList = new PriorityQueue<Node2D>();
        readonly HashSet<Node2D> closedList = new HashSet<Node2D>();
        readonly int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
        readonly int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };
        readonly int[] cost = { 10, 10, 10, 10, 14, 14, 14, 14 };
        readonly Node2DPool nodePool;

        // 回调
        public Action<bool> OnReach;

        // 启发式函数
        readonly Func<Node2D, Node2D, float> heuristicFunc;

        public Compass2D(Node2DPool nodePool, HeuristicType type = HeuristicType.Euclidean) {
            this.heuristicFunc = HeuristicUtil.GetHeuristic(type);
            this.nodePool = nodePool;
        }

        public List<Vector2> FindPath(Map2D map, Vector2 startPos, Vector2 endPos, float agentsize) {
            openList.Clear();
            closedList.Clear();

            var end = MathUtil.Pos2Node(endPos, map);
            var start = MathUtil.Pos2Node(startPos, map);

            if (start == null || end == null) {
                Debug.LogError($"start or end is null: {startPos}, {endPos}");
                return null;
            }

            // 如果起点或终点不可通过，寻找最近的可通过点
            Node2D closestNodeToTarget = start;
            float closestDistanceToTarget = Vector2.Distance(startPos, endPos);

            start.SetG(0);
            start.SetH(heuristicFunc(start, end));
            // start.SetF(start.G + start.H);
            start.SetF(0);
            start.SetParent(null);

            openList.Enqueue(start, start.F);

            while (openList.Count > 0) {
                var currentNode = openList.Dequeue();
                if (currentNode == null) {
                    Debug.LogError("currentNode is null");
                    return null;
                }

                if (currentNode == end) {
                    OnReach?.Invoke(true); // 告知上层抵达终点
                    return GetPathFromNode(currentNode, start);
                }

                closedList.Add(currentNode);

                for (int i = 0; i < 8; i++) {
                    int nx = currentNode.X + dx[i];
                    int ny = currentNode.Y + dy[i];

                    if (nx < 0 || nx >= map.Width || ny < 0 || ny >= map.Height) {
                        continue;
                    }

                    var neighbour = map.Nodes[nx, ny];

                    // 对于对角线移动，检查两个相邻的方格是否都是通行的
                    if (i >= 4) { // i >= 4 表示现在是对角线移动
                        var neighbour1 = map.Nodes[currentNode.X + dx[i], currentNode.Y];
                        var neighbour2 = map.Nodes[currentNode.X, currentNode.Y + dy[i]];
                        if (neighbour1.Capacity < agentsize || neighbour2.Capacity < agentsize) {
                            continue;
                        }
                    }

                    // 更新最近的可通过点
                    var distanceToTarget = Vector2.Distance(new Vector2(neighbour.X, neighbour.Y), endPos);
                    if (distanceToTarget < closestDistanceToTarget) {
                        closestDistanceToTarget = distanceToTarget;
                        closestNodeToTarget = neighbour;
                    }

                    if (closedList.Contains(neighbour) || neighbour.Capacity < agentsize) {
                        continue;
                    }

                    float tentativeG = currentNode.G + cost[i];

                    if (tentativeG < neighbour.G || !openList.Contains(neighbour)) {
                        neighbour.SetG(tentativeG);
                        neighbour.SetH(heuristicFunc(neighbour, end));
                        neighbour.SetF(neighbour.G + neighbour.H);
                        neighbour.SetParent(currentNode);

                        if (openList.Contains(neighbour)) {
                            openList.UpdatePriority(neighbour, neighbour.F);
                        } else {
                            openList.Enqueue(neighbour, neighbour.F);
                        }
                    }

                }

            }
            OnReach?.Invoke(false); // 告知上层无法抵达终点
            return GetPathFromNode(closestNodeToTarget, start);
        }

        private List<Vector2> GetPathFromNode(Node2D endNode, Node2D startNode) {
            var path = new List<Vector2>();
            var currentNode = endNode;

            while (currentNode != null) {
                path.Add(currentNode.GetPos());
                currentNode = currentNode.Parent;
            }
            path.Remove(startNode.GetPos());
            path.Reverse();
            return path;
        }
    }
}
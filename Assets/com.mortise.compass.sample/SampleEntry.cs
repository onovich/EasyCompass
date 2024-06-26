using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MortiseFrame.Compass {

    public class SampleEntry : MonoBehaviour {

        [SerializeField] Vector2 minPos;
        [SerializeField] Vector2 maxPos;
        [SerializeField] float gridUnit = 1;
        [SerializeField] Transform obstacleRoot;

        [SerializeField] GameObject startPoint;
        [SerializeField] GameObject endPoint;

        List<Vector2> path;
        bool[,] map;
        PathFindingCore pathFindingCore;

        void Update() {
            var axis = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                axis += new Vector3(0, 1, 0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                axis += new Vector3(0, -1, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                axis += new Vector3(-1, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                axis += new Vector3(1, 0, 0);
            }

            var targetPos = endPoint.transform.position + axis;
            if (targetPos.x < minPos.x || targetPos.x > maxPos.x || targetPos.y < minPos.y || targetPos.y > maxPos.y) {
                return;
            }

            endPoint.transform.position = targetPos;
            if (axis != Vector3.zero) {
                RefreshPath(pathFindingCore);
                if (path.Count > 1) {
                    startPoint.transform.position = GridUtil.GridToPos(path[1], minPos, gridUnit);
                    RefreshPath(pathFindingCore);
                }
            }
        }

        void Start() {
            pathFindingCore = new PathFindingCore();
            path = new List<Vector2>();
            InitMap();
            BakeObstacle();
            RefreshPath(pathFindingCore);
        }

        void InitMap() {
            var minGrid = GridUtil.PosToGrid(minPos, minPos, gridUnit);
            var maxGrid = GridUtil.PosToGrid(maxPos, minPos, gridUnit);
            var size = maxGrid - minGrid + Vector2.one;
            var xCount = Mathf.RoundToInt(size.x / gridUnit);
            var yCount = Mathf.RoundToInt(size.y / gridUnit);

            map = new bool[xCount, yCount];
            for (int i = 0; i < xCount; i++) {
                for (int j = 0; j < yCount; j++) {
                    map[i, j] = true;
                }
            }
        }

        void BakeObstacle() {
            if (obstacleRoot == null) {
                return;
            }
            var count = obstacleRoot.childCount;
            for (int i = 0; i < count; i++) {
                var child = obstacleRoot.GetChild(i);
                var pos = child.position;
                var grid = GridUtil.PosToGrid(pos, minPos, gridUnit);
                map[(int)grid.x, (int)grid.y] = false;
            }
        }

        void RefreshPath(PathFindingCore pathFindingCore) {
            var start = startPoint.transform.position;
            var end = endPoint.transform.position;

            var startGrid = GridUtil.PosToGrid(start, minPos, gridUnit);
            var endGrid = GridUtil.PosToGrid(end, minPos, gridUnit);

            path.Clear();
            path = pathFindingCore.FindPath(startGrid, endGrid, map);
            if (path == null || path.Count == 0) {
                Debug.Log("No path found");
            }
        }

        void OnDrawGizmos() {
            var minGrid = GridUtil.PosToGrid(minPos, minPos, gridUnit);
            var maxGrid = GridUtil.PosToGrid(maxPos, minPos, gridUnit);
            if (minGrid == maxGrid) {
                return;
            }
            Gizmos.color = Color.red;
            var size = maxGrid - minGrid;
            var centerPos = minPos + size / 2;

            var xCount = Mathf.RoundToInt(size.x / gridUnit);
            var yCount = Mathf.RoundToInt(size.y / gridUnit);
            Gizmos.color = new Color(0, 0, 1, 0.2f);
            for (int i = 0; i <= xCount; i++) {
                for (int j = 0; j <= yCount; j++) {
                    var x = minPos.x + i * gridUnit;
                    var y = minPos.y + j * gridUnit;
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), new Vector3(gridUnit, gridUnit, 0));
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(centerPos, size + new Vector2(gridUnit, gridUnit));

            if (path == null) {
                return;
            }
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++) {
                var current = GridUtil.GridToPos(path[i], minPos, gridUnit);
                var next = GridUtil.GridToPos(path[i + 1], minPos, gridUnit);
                Gizmos.DrawLine(current, next);
            }
        }

    }

}
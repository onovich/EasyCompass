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
        [SerializeField] bool[,] map;
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
                    startPoint.transform.position = GridUtil.GridCenterToWorld(path[1], minPos, gridUnit);
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
            var minGrid = GridUtil.WorldToGrid(minPos, minPos, gridUnit);
            var maxGrid = GridUtil.WorldToGrid(maxPos, minPos, gridUnit);
            var size = maxGrid - minGrid + Vector2.one;
            // var xCount = Mathf.CeilToInt(size.x / gridUnit);
            // var yCount = Mathf.CeilToInt(size.y / gridUnit);
            var xCount = Mathf.CeilToInt(size.x);
            var yCount = Mathf.CeilToInt(size.y);
            Debug.Log("minGrid = " + minGrid + " maxGrid = " + maxGrid);
            Debug.Log("size.x = " + size.x + " size.y = " + size.y + " gridUnit = " + gridUnit);
            Debug.Log("xCount = " + xCount + " yCount = " + yCount);

            map = new bool[xCount, yCount];
            for (int i = 0; i < xCount; i++) {
                for (int j = 0; j < yCount; j++) {
                    map[i, j] = true;
                }
            }
        }

        bool OutOfMap(Vector2 grid) {
            var xBelowZero = grid.x < 0;
            if (xBelowZero) {
                Debug.Log("xBelowZero" + grid.x);
            }
            var outOfWidth = grid.x >= map.GetLength(0);
            if (outOfWidth) {
                Debug.Log("outOfWidth" + grid.x + " w = " + map.GetLength(0));
            }
            var yBelowZero = grid.y < 0;
            if (yBelowZero) {
                Debug.Log("yBelowZero" + grid.y);
            }
            var outOfHeight = grid.y >= map.GetLength(1);
            if (outOfHeight) {
                Debug.Log("outOfHeight " + grid.y + " h = " + map.GetLength(1));
            }
            var isOut = xBelowZero || outOfWidth || yBelowZero || outOfHeight;
            return isOut;
        }

        void BakeObstacle() {
            if (obstacleRoot == null) {
                return;
            }
            var count = obstacleRoot.childCount;
            for (int i = 0; i < count; i++) {
                var child = obstacleRoot.GetChild(i).GetComponent<ObstacleEditorEntity>();
                child.GetArea(gridUnit, (pos) => {
                    var grid = GridUtil.WorldToGrid(pos, minPos, gridUnit);
                    if (OutOfMap(grid)) {
                    }
                    map[(int)grid.x, (int)grid.y] = false;
                });
            }
        }

        void RefreshPath(PathFindingCore pathFindingCore) {
            var start = startPoint.transform.position;
            var end = endPoint.transform.position;

            var startGrid = GridUtil.WorldToGrid(start, minPos, gridUnit);
            var endGrid = GridUtil.WorldToGrid(end, minPos, gridUnit);

            path.Clear();
            path = pathFindingCore.FindPath(startGrid, endGrid, map);
            if (path == null || path.Count == 0) {
                Debug.Log("No path found");
            }
        }

        void OnDrawGizmos() {
            if (gridUnit == 0) {
                return;
            }

            var size = maxPos - minPos;
            var centerPos = minPos + size / 2;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(centerPos, size);

            Gizmos.color = Color.red;
            Gizmos.color = new Color(1, 1, 1, 0.2f);
            for (float i = 0; i < size.x; i += gridUnit) {
                for (float j = 0; j < size.y; j += gridUnit) {
                    var x = minPos.x + i + gridUnit / 2;
                    var y = minPos.y + j + gridUnit / 2;
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), new Vector3(gridUnit, gridUnit, 0));
                }
            }

            if (path == null) {
                return;
            }
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++) {
                var current = GridUtil.GridCenterToWorld(path[i], minPos, gridUnit);
                var next = GridUtil.GridCenterToWorld(path[i + 1], minPos, gridUnit);
                Gizmos.DrawLine(current, next);
            }

            if (map == null) {
                return;
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(1); j++) {
                    if (map[i, j] == false) {
                        var pos = GridUtil.GridCenterToWorld(new Vector2(i, j), minPos, gridUnit);
                        Gizmos.DrawCube(pos, new Vector3(gridUnit, gridUnit, 0));
                    }
                }
            }
        }

    }

}
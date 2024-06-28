using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

namespace MortiseFrame.Compass.Sample {

    public class SampleEntry : MonoBehaviour {

        [SerializeField] Vector2 gridGridCornerLD;
        [SerializeField] Vector2 gridGridConderRT;
        [SerializeField] float gridUnit = 1;
        [SerializeField] Transform obstacleRoot;

        [SerializeField] GameObject startPoint;
        [SerializeField] GameObject endPoint;

        List<Vector2> path;
        [SerializeField] bool[] map;
        [SerializeField] int mapWidth;
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
            if (targetPos.x < gridGridCornerLD.x || targetPos.x > gridGridConderRT.x - gridUnit || targetPos.y < gridGridCornerLD.y || targetPos.y > gridGridConderRT.y - gridUnit) {
                return;
            }

            endPoint.transform.position = targetPos;
            if (axis != Vector3.zero) {
                var oldPos = GridUtil.GridCenterToWorld(path[0], gridGridCornerLD, gridUnit);
                RefreshPath(pathFindingCore);
                if (path.Count > 1) {
                    var offset = GridUtil.GridCenterToWorld(path[1], gridGridCornerLD, gridUnit) - oldPos;
                    startPoint.transform.position += new Vector3(offset.x, offset.y, 0);
                    RefreshPath(pathFindingCore);
                }
            }
        }

        void Start() {
            pathFindingCore = new PathFindingCore();
            path = new List<Vector2>();
            RefreshPath(pathFindingCore);
        }

        [ContextMenu("Bake")]
        void Bake() {
            InitMap();
            BakeObstacle();
        }

        void InitMap() {
            var size = gridGridConderRT - gridGridCornerLD;
            var xCount = Mathf.CeilToInt(size.x);
            var yCount = Mathf.CeilToInt(size.y);

            map = new bool[xCount * yCount];
            mapWidth = xCount;
            for (int i = 0; i < xCount; i++) {
                for (int j = 0; j < yCount; j++) {
                    MapUtil.TrySetMapWalkable(map, xCount, i, j, true);
                }
            }
        }

        bool OutOfMap(Vector2 grid) {
            var xBelowZero = grid.x < 0;
            if (xBelowZero) {
                Debug.Log("xBelowZero" + grid.x);
            }
            var outOfWidth = grid.x >= mapWidth;
            if (outOfWidth) {
                Debug.Log("outOfWidth" + grid.x + " w = " + mapWidth);
            }
            var yBelowZero = grid.y < 0;
            if (yBelowZero) {
                Debug.Log("yBelowZero" + grid.y);
            }
            var mapHeight = MapUtil.GetMapHeight(map, mapWidth);
            var outOfHeight = grid.y >= mapHeight;
            if (outOfHeight) {
                Debug.Log("outOfHeight " + grid.y + " h = " + mapHeight);
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
                child.GetArea(gridUnit, gridGridCornerLD, (grid) => {
                    if (OutOfMap(grid)) {
                    }
                    MapUtil.TrySetMapWalkable(map, mapWidth, (int)grid.x, (int)grid.y, false);
                });
            }
        }

        void RefreshPath(PathFindingCore pathFindingCore) {
            var start = startPoint.transform.position;
            var end = endPoint.transform.position;

            var startGrid = GridUtil.WorldToGrid(start, gridGridCornerLD, gridUnit);
            var endGrid = GridUtil.WorldToGrid(end, gridGridCornerLD, gridUnit);

            path.Clear();
            var mapHeight = MapUtil.GetMapHeight(map, mapWidth);
            path = pathFindingCore.FindPath(startGrid, endGrid, (x, y) => {
                return MapUtil.IsMapWalkable(map, mapWidth, x, y);
            }, mapWidth, mapHeight);
            if (path == null || path.Count == 0) {
                Debug.Log("No path found");
            }
        }

        void OnDrawPath() {
            if (path == null) {
                return;
            }
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++) {
                var current = GridUtil.GridCenterToWorld(path[i], gridGridCornerLD, gridUnit);
                var next = GridUtil.GridCenterToWorld(path[i + 1], gridGridCornerLD, gridUnit);
                Gizmos.DrawLine(current, next);
            }
        }

        void OnDrawGrid() {
            if (gridUnit == 0) {
                return;
            }

            var size = gridGridConderRT - gridGridCornerLD;
            var centerPos = gridGridCornerLD + size / 2;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(centerPos, size);

            Gizmos.color = Color.red;
            Gizmos.color = new Color(1, 1, 1, 0.2f);
            for (float i = 0; i < size.x; i += gridUnit) {
                for (float j = 0; j < size.y; j += gridUnit) {
                    var x = gridGridCornerLD.x + i + gridUnit / 2;
                    var y = gridGridCornerLD.y + j + gridUnit / 2;
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), new Vector3(gridUnit, gridUnit, 0));
                }
            }
        }

        void OnDrawObstacle() {
            if (map == null || map.Length == 0) {
                return;
            }
            Gizmos.color = Color.red;
            for (int index = 0; index < map.Length; index++) {
                var grid = MapUtil.IndexToGridCenter(mapWidth, index);
                var awalkable = MapUtil.IsMapWalkable(map, mapWidth, grid.x, grid.y);
                if (awalkable == false) {
                    var pos = GridUtil.GridCenterToWorld(new Vector2(grid.x, grid.y), gridGridCornerLD, gridUnit);
                    Gizmos.DrawCube(pos, new Vector3(gridUnit, gridUnit, 0));
                }
            }
        }

        void OnDrawGizmos() {
            OnDrawGrid();
            OnDrawPath();
            OnDrawObstacle();
        }
    }

}
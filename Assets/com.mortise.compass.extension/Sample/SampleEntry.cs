using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

namespace MortiseFrame.Compass.Extension.Sample {

    public class SampleEntry : MonoBehaviour {

        [SerializeField] Vector2 gridGridCornerLD;
        [SerializeField] Vector2 gridGridConderRT;
        [SerializeField] float gridUnit = 1;
        [SerializeField] Transform obstacleRoot;

        [SerializeField] GameObject startPoint;
        [SerializeField] GameObject endPoint;

        [SerializeField] PathFindingDirection directionMode;

        List<Vector2> path;
        [SerializeField] bool[] map;
        [SerializeField] int mapWidth;
        PathFindingCore pathFindingCore;

        void Update() {
            var axis = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                axis += new Vector3(0, gridUnit, 0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                axis += new Vector3(0, -gridUnit, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                axis += new Vector3(-gridUnit, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                axis += new Vector3(gridUnit, 0, 0);
            }

            if (axis != Vector3.zero) {
                if (path.Count > 0) {
                    MoveRole(axis, endPoint.transform);
                    RefreshPath(pathFindingCore);
                }
                if (path.Count > 1) {
                    var oldPos = GridUtil.GridToWorld_Center(path[0], gridGridCornerLD, gridUnit);
                    var offset = GridUtil.GridToWorld_Center(path[1], gridGridCornerLD, gridUnit) - oldPos;
                    MoveRole(offset, startPoint.transform);
                    RefreshPath(pathFindingCore);
                }
            }
        }

        void MoveRole(Vector3 axis, Transform role) {
            var targetPos = role.position + axis;
            // if (targetPos.x < gridGridCornerLD.x || targetPos.x > gridGridConderRT.x - gridUnit || targetPos.y < gridGridCornerLD.y || targetPos.y > gridGridConderRT.y - gridUnit) {
            if (targetPos.x < gridGridCornerLD.x || targetPos.x > gridGridConderRT.x || targetPos.y < gridGridCornerLD.y || targetPos.y > gridGridConderRT.y) {
                return;
            }
            role.position = targetPos;
        }

        void Start() {
            pathFindingCore = new PathFindingCore();
            path = new List<Vector2>();
            RefreshPath(pathFindingCore);
        }

        [ContextMenu("Bake")]
        void Bake() {
            ToasterHelper.Toast(obstacleRoot, gridGridCornerLD, gridGridConderRT, gridUnit, out map, out mapWidth);
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
            }, mapWidth, mapHeight, directionMode);
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
                var current = GridUtil.GridToWorld_Center(path[i], gridGridCornerLD, gridUnit);
                var next = GridUtil.GridToWorld_Center(path[i + 1], gridGridCornerLD, gridUnit);
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
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            for (int index = 0; index < map.Length; index++) {
                var grid = MapUtil.IndexToGridCenter(mapWidth, index);
                var awalkable = MapUtil.IsMapWalkable(map, mapWidth, grid.x, grid.y);
                if (awalkable == false) {
                    var pos = GridUtil.GridToWorld_Center(new Vector2(grid.x, grid.y), gridGridCornerLD, gridUnit);
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
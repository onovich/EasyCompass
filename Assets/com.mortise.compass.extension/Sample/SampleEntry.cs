using System;
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
        [SerializeField] bool cornerWalkable;

        Vector2[] path;
        int pathLen;
        [SerializeField] bool[] map;
        [SerializeField] int mapWidth;
        [SerializeField] int pathLenExpected = 100;

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
                if (path.Length > 0) {
                    MoveRole(axis, endPoint.transform);
                    RefreshPath();
                }
                if (path.Length > 1) {
                    var oldPos = PathFindingGridUtil.GridToWorld_Center(path[0], gridGridCornerLD, gridUnit);
                    var offset = PathFindingGridUtil.GridToWorld_Center(path[1], gridGridCornerLD, gridUnit) - oldPos;
                    MoveRole(offset, startPoint.transform);
                    RefreshPath();
                }
            }
        }

        void MoveRole(Vector3 axis, Transform role) {
            var targetPos = role.position + axis;
            if (targetPos.x < gridGridCornerLD.x || targetPos.x > gridGridConderRT.x || targetPos.y < gridGridCornerLD.y || targetPos.y > gridGridConderRT.y) {
                return;
            }
            role.position = targetPos;
        }

        void Start() {
            path = new Vector2[pathLenExpected];
            RefreshPath();
            CLog.Log = Debug.Log;
            CLog.LogError = Debug.LogError;
            CLog.LogWarning = Debug.LogWarning;
        }

        [ContextMenu("Bake")]
        void Bake() {
            PathFindingBakerHelper.Bake(obstacleRoot, gridGridCornerLD, gridGridConderRT, gridUnit, 0.0001f, out map, out mapWidth);
        }

        void RefreshPath() {
            var start = startPoint.transform.position;
            var end = endPoint.transform.position;

            var startGrid = PathFindingGridUtil.WorldToGrid(start, gridGridCornerLD, gridUnit);
            var endGrid = PathFindingGridUtil.WorldToGrid(end, gridGridCornerLD, gridUnit);
            Array.Clear(path, 0, path.Length);

            var mapHeight = PathFindingMapUtil.GetMapHeight(map, mapWidth);
            pathLen = PathFindingCore.FindPath(startGrid, endGrid, (x, y) => {
                return PathFindingMapUtil.IsMapWalkable(map, mapWidth, x, y);
            }, mapWidth, mapHeight, directionMode, cornerWalkable, path);
            if (path == null || pathLen == 0) {
                CLog.Log("No path found");
            }
        }

        void OnDrawPath() {
            PathFindingGizmosHelper.OnDrawPath(pathLen, path, gridGridCornerLD, gridUnit);
        }

        void OnDrawGrid() {
            PathFindingGizmosHelper.OnDrawGrid(gridUnit, gridGridCornerLD, gridGridConderRT);
        }

        void OnDrawObstacle() {
            PathFindingGizmosHelper.OnDrawObstacle(map, mapWidth, gridGridCornerLD, gridUnit);
        }

        void OnDrawGizmos() {
            OnDrawGrid();
            OnDrawPath();
            OnDrawObstacle();
        }
    }

}
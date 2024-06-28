using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public static class PathFindingGizmosHelper {

        public static void OnDrawPath(int pathLen, Vector2[] path, Vector2 gridGridCornerLD, float gridUnit) {
            if (pathLen == 0) {
                return;
            }
            Gizmos.color = Color.yellow;
            for (int i = 0; i < pathLen - 1; i++) {
                var current = PathFindingGridUtil.GridToWorld_Center(path[i], gridGridCornerLD, gridUnit);
                var next = PathFindingGridUtil.GridToWorld_Center(path[i + 1], gridGridCornerLD, gridUnit);
                Gizmos.DrawLine(current, next);
            }
        }

        public static void OnDrawGrid(float gridUnit, Vector2 gridGridCornerLD, Vector2 gridGridConderRT) {
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

        public static void OnDrawObstacle(bool[] map, int mapWidth, Vector2 gridGridCornerLD, float gridUnit) {
            if (map == null || map.Length == 0) {
                return;
            }
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            for (int index = 0; index < map.Length; index++) {
                var grid = PathFindingMapUtil.IndexToGridCenter(mapWidth, index);
                var awalkable = PathFindingMapUtil.IsMapWalkable(map, mapWidth, grid.x, grid.y);
                if (awalkable == false) {
                    var pos = PathFindingGridUtil.GridToWorld_Center(new Vector2(grid.x, grid.y), gridGridCornerLD, gridUnit);
                    Gizmos.DrawCube(pos, new Vector3(gridUnit, gridUnit, 0));
                }
            }
        }

    }

}
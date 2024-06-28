using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public static class PathFindingGridUtil {

        public static Vector2 WorldToGrid(Vector2 worldPoint,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = Mathf.Floor((worldPoint.x - gridCornerLD.x) / gridUnit);
            var y = Mathf.Floor((worldPoint.y - gridCornerLD.y) / gridUnit);
            return new Vector2(x, y);
        }

        public static Vector2 GridToWorld_Center(Vector2 grid,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = gridCornerLD.x + grid.x * gridUnit + gridUnit / 2;
            var y = gridCornerLD.y + grid.y * gridUnit + gridUnit / 2;
            return new Vector2(x, y);
        }

        public static Vector2 GridToWorld_LD(Vector2 grid,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = gridCornerLD.x + grid.x * gridUnit;
            var y = gridCornerLD.y + grid.y * gridUnit;
            return new Vector2(x, y);
        }

        public static Vector2 GridToWorld_RT(Vector2 grid,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = gridCornerLD.x + grid.x * gridUnit + gridUnit;
            var y = gridCornerLD.y + grid.y * gridUnit + gridUnit;
            return new Vector2(x, y);
        }

        public static bool IsContain(Vector2 a, Vector2 bMin, Vector2 bMax) {
            return a.x >= bMin.x && a.x < bMax.x && a.y >= bMin.y && a.y < bMax.y;
        }

        public static void SizeToGridArea(Vector2 size,
                                          Vector2 obstacleMinPos,
                                          Vector2 gridCornerLD,
                                          float gridUnit,
                                          float epsilon,
                                          System.Action<Vector2> action) {

            var obstacleMinGrid = WorldToGrid(obstacleMinPos, gridCornerLD, gridUnit);
            var obstacleMaxPos = obstacleMinPos + size - new Vector2(epsilon, epsilon);
            var obstacleMaxGrid = WorldToGrid(obstacleMaxPos, gridCornerLD, gridUnit);
            for (int i = (int)obstacleMinGrid.x; i <= obstacleMaxGrid.x; i++) {
                for (int j = (int)obstacleMinGrid.y; j <= obstacleMaxGrid.y; j++) {
                    action(new Vector2(i, j));
                }
            }

        }
    }

}
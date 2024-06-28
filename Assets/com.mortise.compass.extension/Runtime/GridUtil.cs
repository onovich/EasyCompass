using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public static class GridUtil {

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

        public static bool IsContain(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax) {
            return aMin.x <= bMax.x && aMax.x >= bMin.x && aMin.y <= bMax.y && aMax.y >= bMin.y;
        }

        public static void SizeToGridArea(Vector2 size,
                                          Vector2 obstacleMinPos,
                                          Vector2 gridCornerLD,
                                          float gridUnit,
                                          System.Action<Vector2> action) {

            var obstacleMaxPos = obstacleMinPos + size;
            for (float x = obstacleMinPos.x; x < obstacleMaxPos.x; x += gridUnit) {
                for (float y = obstacleMinPos.y; y < obstacleMaxPos.y; y += gridUnit) {
                    var grid = WorldToGrid(new Vector2(x, y), gridCornerLD, gridUnit);
                    var gridMinPos = GridToWorld_LD(grid, gridCornerLD, gridUnit);
                    var gridMaxPos = GridToWorld_RT(grid, gridCornerLD, gridUnit);

                    var minPos = new Vector2(x, y);
                    var maxPos = new Vector2(x + gridUnit, y + gridUnit);
                    if (IsContain(minPos, maxPos, gridMinPos, gridMaxPos)) {
                        action(grid);
                    }
                }
            }
        }
    }
}
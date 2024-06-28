using UnityEngine;

namespace MortiseFrame.Compass {

    public static class GridUtil {

        public static Vector2 WorldToGrid(Vector2 worldPoint,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = Mathf.Floor((worldPoint.x - gridCornerLD.x) / gridUnit);
            var y = Mathf.Floor((worldPoint.y - gridCornerLD.y) / gridUnit);
            // Debug.Log("World To Grid: " + worldPoint.x + " - " + gridCornerLD.x + " / " + gridUnit + " = " + x);
            return new Vector2(x, y);
        }

        public static Vector2 GridCenterToWorld(Vector2 gridCenter,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = gridCornerLD.x + gridCenter.x * gridUnit + gridUnit / 2;
            var y = gridCornerLD.y + gridCenter.y * gridUnit + gridUnit / 2;
            return new Vector2(x, y);
        }

        public static void SizeToGridArea(Vector2 size,
                                          Vector2 obstacleMinPos,
                                          Vector2 gridCornerLD,
                                          float gridUnit,
                                          System.Action<Vector2> action) {
            var obstacleMinGrid = WorldToGrid(obstacleMinPos, gridCornerLD, gridUnit);
            for (float x = 0; x < size.x; x += gridUnit) {
                for (float y = 0; y < size.y; y += gridUnit) {
                    var pos = new Vector2(x, y) + obstacleMinPos;
                    var grid = WorldToGrid(pos, gridCornerLD, gridUnit);
                    Debug.Log("GetArea, grid = " + grid + " pos = " + pos);
                    action(grid);
                    // Debug.Log("GetArea, grid = " + grid + " pos = " + pos
                    // + " x = " + x + " y = " + y + " minPos = " + obstacleMinPos + " gridUnit = " + gridUnit);
                }
            }
        }
    }
}
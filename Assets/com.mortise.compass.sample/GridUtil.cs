using UnityEngine;

namespace MortiseFrame.Compass {

    public static class GridUtil {

        public static Vector2 WorldToGrid(Vector2 worldPoint,
                                          Vector2 gridCornerLD,
                                          float gridUnit) {
            var x = Mathf.Floor((worldPoint.x - gridCornerLD.x) / gridUnit);
            var y = Mathf.Floor((worldPoint.y - gridCornerLD.y) / gridUnit);
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
            // Debug.Log("SizeToGridArea, obstacleMinGrid = " + obstacleMinGrid + " obstacleMinPos = " + obstacleMinPos);
            for (float x = 0; x < size.x; x += gridUnit) {
                for (float y = 0; y < size.y; y += gridUnit) {
                    var pos = new Vector2(x, y) + obstacleMinPos + gridCornerLD;
                    var grid = WorldToGrid(pos, obstacleMinPos, gridUnit) + obstacleMinGrid;
                    action(grid);
                    // Debug.Log("GetArea, grid = " + grid + " pos = " + pos
                    // + " x = " + x + " y = " + y + " minPos = " + obstacleMinPos + " gridUnit = " + gridUnit);
                }
            }
        }
    }
}
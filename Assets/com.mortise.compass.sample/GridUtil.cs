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
                                          Vector2 minPos,
                                          float gridUnit,
                                          System.Action<Vector2> action) {
            for (float x = 0; x < size.x; x += gridUnit) {
                for (float y = 0; y < size.y; y += gridUnit) {
                    var pos = new Vector2(x, y) + minPos;
                    var grid = WorldToGrid(pos, minPos, gridUnit);
                    action(grid);
                }
            }
        }
    }
}
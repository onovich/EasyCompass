using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public static class ToasterHelper {

        public static bool Toast(Transform obstacleRoot,
                                 Vector2 gridCornerLD,
                                 Vector2 gridCornerRT,
                                 float gridUnit,
                                 out bool[] map,
                                 out int mapWidth) {
            InitMap(gridCornerLD, gridCornerRT, gridUnit, out map, out mapWidth);
            return BakeObstacle(obstacleRoot, gridCornerLD, gridUnit, map, mapWidth);
        }

        static void InitMap(Vector2 gridCornerLD, Vector2 gridCornerRT, float gridUnit, out bool[] map, out int mapWidth) {
            var gridLD = GridUtil.WorldToGrid(gridCornerLD, gridCornerLD, gridUnit);
            var gridRT = GridUtil.WorldToGrid(gridCornerRT, gridCornerLD, gridUnit);
            var xCount = (int)(gridRT.x - gridLD.x);
            var yCount = (int)(gridRT.y - gridLD.y);

            map = new bool[xCount * yCount];
            mapWidth = xCount;
            for (int i = 0; i < xCount; i++) {
                for (int j = 0; j < yCount; j++) {
                    MapUtil.TrySetMapWalkable(map, xCount, i, j, true);
                }
            }
        }

        static bool BakeObstacle(Transform obstacleRoot, Vector2 gridCornerLD, float gridUnit, bool[] map, int mapWidth) {
            if (obstacleRoot == null) {
                return false;
            }
            var count = obstacleRoot.childCount;
            var succ = count > 0;
            for (int i = 0; i < count; i++) {
                var child = obstacleRoot.GetChild(i).GetComponent<ObstacleEditorEntity>();
                child.GetArea(gridUnit, gridCornerLD, (grid) => {
                    if (OutOfMap(grid, map, mapWidth)) {
                        succ = false;
                        return;
                    }
                    MapUtil.TrySetMapWalkable(map, mapWidth, (int)grid.x, (int)grid.y, false);
                });
            }
            return succ;
        }

        static bool OutOfMap(Vector2 grid, bool[] map, int mapWidth) {
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

    }

}
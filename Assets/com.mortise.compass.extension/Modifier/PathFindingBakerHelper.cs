using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public static class PathFindingBakerHelper {

        public static bool Bake(Transform obstacleRoot,
                                 Vector2 gridCornerLD,
                                 Vector2 gridCornerRT,
                                 float gridUnit,
                                 float epsilon,
                                 out bool[] map,
                                 out int mapWidth) {
            InitMap(gridCornerLD, gridCornerRT, gridUnit, out map, out mapWidth);
            return BakeObstacle(obstacleRoot, gridCornerLD, gridUnit, epsilon, map, mapWidth);
        }

        static void InitMap(Vector2 gridCornerLD, Vector2 gridCornerRT, float gridUnit, out bool[] map, out int mapWidth) {
            var gridLD = PathFindingGridUtil.WorldToGrid(gridCornerLD, gridCornerLD, gridUnit);
            var gridRT = PathFindingGridUtil.WorldToGrid(gridCornerRT, gridCornerLD, gridUnit);
            var xCount = (int)(gridRT.x - gridLD.x);
            var yCount = (int)(gridRT.y - gridLD.y);

            map = new bool[xCount * yCount];
            mapWidth = xCount;
            for (int i = 0; i < xCount; i++) {
                for (int j = 0; j < yCount; j++) {
                    PathFindingMapUtil.TrySetMapWalkable(map, xCount, i, j, true);
                }
            }
        }

        static bool BakeObstacle(Transform obstacleRoot, Vector2 gridCornerLD, float gridUnit, float epsilon, bool[] map, int mapWidth) {
            if (obstacleRoot == null) {
                return false;
            }
            var count = obstacleRoot.childCount;
            var succ = count > 0;
            for (int i = 0; i < count; i++) {
                var child = obstacleRoot.GetChild(i).GetComponent<PathFindingObstacleEditorEntity>();
                child.GetArea(gridUnit, gridCornerLD, epsilon, (grid) => {
                    if (OutOfMap(grid, map, mapWidth)) {
                        succ = false;
                        return;
                    }
                    PathFindingMapUtil.TrySetMapWalkable(map, mapWidth, (int)grid.x, (int)grid.y, false);
                });
            }
            return succ;
        }

        static bool OutOfMap(Vector2 grid, bool[] map, int mapWidth) {
            var xBelowZero = grid.x < 0;
            if (xBelowZero) {
                CLog.Log("xBelowZero" + grid.x);
            }
            var outOfWidth = grid.x >= mapWidth;
            if (outOfWidth) {
                CLog.Log("outOfWidth" + grid.x + " w = " + mapWidth);
            }
            var yBelowZero = grid.y < 0;
            if (yBelowZero) {
                CLog.Log("yBelowZero" + grid.y);
            }
            var mapHeight = PathFindingMapUtil.GetMapHeight(map, mapWidth);
            var outOfHeight = grid.y >= mapHeight;
            if (outOfHeight) {
                CLog.Log("outOfHeight " + grid.y + " h = " + mapHeight);
            }
            var isOut = xBelowZero || outOfWidth || yBelowZero || outOfHeight;
            return isOut;
        }

    }

}
using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public static class PathFindingMapUtil {

        public static bool IsMapWalkable(bool[] map, int width, int gridX, int gridY) {
            int index = GetGridIndex(width, gridX, gridY);
            var mapHeight = GetMapHeight(map, width);
            if (index < 0 || index >= map.Length) {
                CLog.LogError("Index out of range: " + index + " map length: " + map.Length
                + " width: " + width + " height: " + mapHeight + " gridX: " + gridX + " gridY: " + gridY);
                return false;
            }
            return map[index];
        }

        static int GetGridIndex(int width, int gridX, int gridY) {
            return gridY * width + gridX;
        }

        public static Vector2Int IndexToGridCenter(int width, int index) {
            int gridX = index % width;
            int gridY = index / width;
            return new Vector2Int(gridX, gridY);
        }

        public static int GetMapHeight(bool[] map, int width) {
            if (width == 0) {
                return 0;
            }
            return map.Length / width;
        }

        public static bool TrySetMapWalkable(bool[] map, int width, int gridX, int gridY, bool walkable) {
            if (gridX < 0 || gridX >= width) {
                return false;
            }
            if (gridY < 0 || gridY >= map.Length / width) {
                return false;
            }
            int index = GetGridIndex(width, gridX, gridY);
            map[index] = walkable;
            return true;
        }

    }

}
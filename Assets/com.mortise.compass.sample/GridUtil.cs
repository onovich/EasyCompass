using UnityEngine;

namespace MortiseFrame.Compass {

    public static class GridUtil {

        public static Vector2 PosToGrid(Vector2 pos, Vector2 min, float gridUnit) {
            return new Vector2((int)((pos.x - min.x) / gridUnit), (int)((pos.y - min.y) / gridUnit));
        }

        public static Vector2 GridToPos(Vector2 grid, Vector2 min, float gridUnit) {
            return new Vector2(min.x + grid.x * gridUnit, min.y + grid.y * gridUnit);
        }
    }
}
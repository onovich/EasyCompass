using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Compass {

    public class SampleEntry : MonoBehaviour {

        [SerializeField] Vector2 min;
        [SerializeField] Vector2 max;
        [SerializeField] float gridUnit = 1;

        void OnDrawGizmos() {
            if (min == max) {
                return;
            }
            Gizmos.color = Color.red;
            var size = max - min;
            var center = min + size / 2;

            var xCount = (int)(size.x / gridUnit);
            var yCount = (int)(size.y / gridUnit);
            Gizmos.color = Color.white;
            for (int i = 0; i < xCount; i++) {
                for (int j = 0; j < yCount; j++) {
                    var x = min.x + i * gridUnit + gridUnit / 2;
                    var y = min.y + j * gridUnit + gridUnit / 2;
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), new Vector3(gridUnit, gridUnit, 0));
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, size);
        }

    }

}
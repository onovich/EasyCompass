using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MortiseFrame.Compass {

    public class ObstacleEditorEntity : MonoBehaviour {

        public Vector2 GetSize() {
            return transform.localScale;
        }

        public Vector2 GetMinPos() {
            return transform.position;
        }

        public Vector2 GetMax() {
            return GetMinPos() + GetSize();
        }

        public void GetArea(float gridUnit, Action<Vector2> action) {
            var minPos = GetMinPos();
            var maxPos = GetMax();

            var xCount = Mathf.RoundToInt((maxPos.x - minPos.x) / gridUnit);
            var yCount = Mathf.RoundToInt((maxPos.y - minPos.y) / gridUnit);

            for (int x = 0; x < xCount; x++) {
                for (int y = 0; y < yCount; y++) {
                    action(new Vector2(minPos.x + x * gridUnit, minPos.y + y * gridUnit));
                }
            }
        }


    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MortiseFrame.Compass {

    public class ObstacleEditorEntity : MonoBehaviour {

        public Vector2 GetSize() {
            return transform.GetChild(0).transform.localScale;
        }

        public Vector2 GetMinPos() {
            return transform.position;
        }

        public Vector2 GetMax() {
            return GetMinPos() + GetSize();
        }

        public void GetArea(float gridUnit, Action<Vector2> action) {
            var minPos = GetMinPos();
            var size = GetSize();
            GridUtil.SizeToGridArea(size, minPos, gridUnit, action);
        }


    }

}
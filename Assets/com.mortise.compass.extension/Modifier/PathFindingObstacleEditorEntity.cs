using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MortiseFrame.Compass.Extension {

    public class PathFindingObstacleEditorEntity : MonoBehaviour {

        internal Vector2 GetSize() {
            return transform.GetChild(0).transform.localScale;
        }

        internal Vector2 GetLBPos() {
            return transform.position;
        }

        internal Vector2 GetMax() {
            return GetLBPos() + GetSize();
        }

        internal void GetArea(float gridUnit, Vector2 gridCornerLD, float epsilon, Action<Vector2> action) {
            var lbPos = GetLBPos();
            var size = GetSize();
            PathFindingGridUtil.SizeToGridArea(size, lbPos, gridCornerLD, gridUnit, epsilon, action);
        }


    }

}
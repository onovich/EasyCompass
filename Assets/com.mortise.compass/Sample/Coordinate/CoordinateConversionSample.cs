using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Compass.Sample {

    public class CoordinateConversionSample : MonoBehaviour {

        [Header("输出")] public Vector2Int index;
        [Header("网格数据输入")] public CompassSampleSO compassSampleSO;

        void Update() {

            var pos = transform.position;
            var map = GetComponent<NavAgentSample>().Map;
            this.index = MathUtil.Pos2Index(pos, map);

        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Compass.Sample {

    public class NavAgentSample : MonoBehaviour {

        public int ID;
        public bool IsStop;

        [SerializeField] Transform target;
        public Transform Target => target;
        public void SetTarget(Transform value) => target = value;

        Compass2D compass;
        public Compass2D Compass => compass;
        public void SetCompass(Compass2D value) => compass = value;

        Map2D map;
        public Map2D Map => map;
        public void SetMap(Map2D value) => map = value;

        Node2DPool node2DPool;
        public Node2DPool Node2DPool => node2DPool;
        public void SetNode2DPool(Node2DPool value) => node2DPool = value;

        List<Vector2> path;
        public List<Vector2> Path => path;
        public void SetPath(List<Vector2> value) => path = value;

        int currentPathIndex = 0;
        public int CurrentPathIndex => currentPathIndex;
        public void SetCurrentPathIndex(int value) => currentPathIndex = value;
        public void AddCurrentPathIndex() => currentPathIndex += 1;
        public void ResetPathIndex() => currentPathIndex = 0;

        public bool isStop = false;

        public Vector2Int indexPos;

        void Update() {

            var pos = transform.position;
            this.indexPos = MathUtil.Pos2Index(pos, map);

        }

    }

}

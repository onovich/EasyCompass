using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MortiseFrame.Compass.Test {

    public class GridUtilTest {
        [TestCase(0f, 2f, 2f, -10f, 1f, 10f, 7f)]
        public void WorldToGrid_Test(float worldX, float worldY, float gridCornerX, float gridCornerY, float gridUnit, float expectedX, float expectedY) {
            Vector2 worldPoint = new Vector2(worldX, worldY);
            Vector2 gridCornerLD = new Vector2(gridCornerX, gridCornerY);
            float gridUnitValue = gridUnit;

            Vector2 expectedGridPoint = new Vector2(expectedX, expectedY);
            Vector2 actualGridPoint = GridUtil.WorldToGrid(worldPoint, gridCornerLD, gridUnitValue);

            Assert.AreEqual(expectedGridPoint, actualGridPoint);
        }

        [TestCase(0f, 0f, 0f, 0f, 1f, -9.5, -4.5f)]
        public void GridCenterToWorld_Test(float gridCenterX, float gridCenterY, float gridCornerX, float gridCornerY, float gridUnit, float expectedX, float expectedY) {
            Vector2 gridCenter = new Vector2(gridCenterX, gridCenterY);
            Vector2 gridCornerLD = new Vector2(gridCornerX, gridCornerY);
            float gridUnitValue = gridUnit;

            Vector2 expectedWorldPoint = new Vector2(expectedX, expectedY);
            Vector2 actualWorldPoint = GridUtil.GridToWorld_Center(gridCenter, gridCornerLD, gridUnitValue);

            Assert.AreEqual(expectedWorldPoint, actualWorldPoint);
        }
    }

}
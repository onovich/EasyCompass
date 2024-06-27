using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MortiseFrame.Compass.Test {

    public class GridUtilTest {
        [TestCase(5.3f, 7.8f, 2f, 2f, 1f, 3f, 5f)]
        [TestCase(4.2f, 6.4f, 1f, 1f, 2f, 1.6f, 2.7f)]
        [TestCase(10.0f, 10.0f, 0f, 0f, 5f, 2f, 2f)]
        public void WorldToGrid_Test(float worldX, float worldY, float gridCornerX, float gridCornerY, float gridUnit, float expectedX, float expectedY) {
            Vector2 worldPoint = new Vector2(worldX, worldY);
            Vector2 gridCornerLD = new Vector2(gridCornerX, gridCornerY);
            float gridUnitValue = gridUnit;

            Vector2 expectedGridPoint = new Vector2(expectedX, expectedY);
            Vector2 actualGridPoint = GridUtil.WorldToGrid(worldPoint, gridCornerLD, gridUnitValue);

            Assert.AreEqual(expectedGridPoint, actualGridPoint);
        }

        [TestCase(3f, 4f, 2f, 2f, 1f, 5.5f, 6.5f)]
        [TestCase(1.5f, 2.5f, 0f, 0f, 2f, 3.5f, 5.5f)]
        [TestCase(2f, 2f, 1f, 1f, 0.5f, 2.5f, 2.5f)]
        public void GridCenterToWorld_Test(float gridCenterX, float gridCenterY, float gridCornerX, float gridCornerY, float gridUnit, float expectedX, float expectedY) {
            Vector2 gridCenter = new Vector2(gridCenterX, gridCenterY);
            Vector2 gridCornerLD = new Vector2(gridCornerX, gridCornerY);
            float gridUnitValue = gridUnit;

            Vector2 expectedWorldPoint = new Vector2(expectedX, expectedY);
            Vector2 actualWorldPoint = GridUtil.GridCenterToWorld(gridCenter, gridCornerLD, gridUnitValue);

            Assert.AreEqual(expectedWorldPoint, actualWorldPoint);
        }
    }

}
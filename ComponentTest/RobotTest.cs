using Karol;
using Karol.Core;
using Karol.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace ComponentTest
{
    [TestClass]
    public class RobotTest
    {
        private World TestWorld => new World(8, 4, 8);

        [TestMethod("Standard Constructor")]
        public void TestConstructor1()
        {
            World w1 = TestWorld;

            Robot r1 = new Robot(1, 1, w1);
            Robot r3 = new Robot(2, 2, w1, Direction.East);
            Robot r2 = new Robot(w1);      // sollte auf Position 0, 0, 0 sein
        }

        [TestMethod("Constructor mit RobotOptions")]
        public void TestConstructor2()
        {
            RobotOptions o = new RobotOptions(TestWorld)
            {
                StartX = 0,
                StartZ = 1,
                InitialDirection = Direction.West,
                Delay = 10,
                Set = ImageSet.Magenta
            };

            Robot r1 = new Robot(o);
            Robot r2 = new Robot(1, 1, o); // überschreibt die in den Options definierte Position...
        }

        [TestMethod("Constructor mit ungültiger Position")]
        [ExpectedException(typeof(InvalidPositionException))]
        public void TestConstructor3()
        {
            Robot r1 = new Robot(-1, 0, TestWorld);
        }

        [TestMethod("Constructor mit zu großer Position")]
        [ExpectedException(typeof(InvalidPositionException))]
        public void TestConstructor3einHalb()
        {
            Robot r1 = new Robot(999999999, 666, TestWorld);
        }

        [TestMethod("Andere Constructoren?")]
        public void TestConstructor4()
        {
            World w1 = TestWorld;

            Robot r1 = new Robot(1, 0, w1);
            Robot r2 = new Robot(w1);
            Robot r3 = new Robot(2, 1, w1, Direction.South);
            Robot r4 = new Robot(TestWorld, Direction.West); // ist in anderer welt als die anderen...
        }

        [TestMethod("Standardwerte")]
        public void TestDefaultValues()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(3, 3, w1);
            Robot r2 = new Robot(0, 0, w1);

            Assert.AreNotEqual(r1, r2);

            Assert.AreEqual(new Position(3, 0, 3), r1.Position);
            Assert.AreEqual(Direction.North, r1.FaceDirection);
            Assert.AreEqual(1, r1.JumpHeight);
            Assert.AreEqual(300, r1.Delay);
            Assert.AreEqual(Color.Red, r1.Paint);
            Assert.IsTrue(r1.IsVisible);

            Assert.AreEqual(0, r1.Identifier);
            Assert.AreEqual(1, r2.Identifier);

            Assert.AreEqual(w1, r1.World);
            Assert.AreEqual(r1.World, r2.World);

            Assert.AreEqual(0, r1.FrontBrickCount);
            Assert.AreEqual(Color.Transparent, r1.FrontBrickColor);
            Assert.AreEqual(-1, r1.FrontRobotIdentifier);

            Assert.AreEqual(-1, r1.MaxBackpackSize);
            Assert.AreEqual(0, r1.BricksInBackpack);
            Assert.IsFalse(r1.IsBackpackEmpty);
            Assert.IsFalse(r1.IsBackpackFull);

            Assert.IsFalse(r1.HasMark);
            Assert.IsFalse(r1.HasBrick);
            Assert.IsFalse(r1.HasBrickLeft);
            Assert.IsFalse(r1.HasBrickRight);
            Assert.IsFalse(r1.HasRobot);
            Assert.IsFalse(r1.HasWall);

            Assert.IsFalse(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingSouth);
            Assert.IsFalse(r1.IsFacingWest);
            Assert.IsTrue(r1.IsFacingNorth);
        }

        [TestMethod("Roboter aus Geladener Welt")]
        public void TestGetRobotFromLoadedWorld()
        {
            string str = "KarolVersion2Deutsch 3 3 3 0 0 0 n n n o n n n o n n n m q q n o n n n o z n n o n n n o z n n o n n n o";
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            World w1 = World.Load(stream);
            Robot r1 = w1.Robots[0];

            Assert.AreEqual(new Position(0, 0, 2), r1.Position);
            Assert.AreEqual(Direction.South, r1.FaceDirection);
            Assert.AreEqual(0, r1.Identifier);

            Assert.AreEqual(1, w1.RoboterCount);
            Assert.AreEqual(1, w1.Robots.Length);
            Assert.AreEqual(r1, w1.Robots[0]);
        }
    }
}

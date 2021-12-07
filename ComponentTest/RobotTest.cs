using Karol;
using Karol.Core;
using Karol.Core.Exceptions;
using Karol.Core.WorldElements;
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
        private World TestWorld => new World(8, 4, 7);

        [TestMethod("Standard Constructor")]
        public void TestConstructor1()
        {
            World w1 = TestWorld;

            Robot r1 = new Robot(w1, 1, 1);
            Robot r3 = new Robot(w1, 2, 2, Direction.East);
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
            Robot r2 = new Robot(o, 1, 1); // überschreibt die in den Options definierte Position...
        }

        [TestMethod("Constructor mit ungültiger Position")]
        [ExpectedException(typeof(InvalidPositionException))]
        public void TestConstructor3()
        {
            Robot r1 = new Robot(TestWorld, -1, 0);
        }

        [TestMethod("Constructor mit zu großer Position")]
        [ExpectedException(typeof(InvalidPositionException))]
        public void TestConstructor3einHalb()
        {
            Robot r1 = new Robot(TestWorld, 999999999, 666);
        }

        [TestMethod("Andere Constructoren?")]
        public void TestConstructor4()
        {
            World w1 = TestWorld;

            Robot r1 = new Robot(w1, 1, 0);
            Robot r2 = new Robot(w1);
            Robot r3 = new Robot(w1, 2, 1, Direction.South);
            Robot r4 = new Robot(TestWorld, Direction.West); // ist in anderer welt als die anderen...
        }

        [TestMethod("Standardwerte")]
        public void TestDefaultValues()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 3, 3);
            Robot r2 = new Robot(w1, 0, 0);

            Assert.AreNotEqual(r1, r2);

            Assert.AreEqual(new Position(3, 0, 3), r1.Position);
            Assert.AreEqual(Direction.North, r1.FaceDirection);
            Assert.AreEqual(1, r1.JumpHeight);
            Assert.AreEqual(300, r1.Delay);
            Assert.AreEqual(Color.Red, r1.Paint);
            Assert.IsTrue(r1.IsVisible);

            Assert.AreEqual(1, r1.Identifier);
            Assert.AreEqual(2, r2.Identifier);

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
            Assert.AreEqual(1, r1.Identifier);

            Assert.AreEqual(1, w1.RoboterCount);
            Assert.AreEqual(1, w1.Robots.Length);
            Assert.AreEqual(r1, w1.Robots[0]);
        }

        [TestMethod("Eine drehung machen")]
        public void TestRobotRotate1()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 1, 1);
            r1.Delay = 10;

            Assert.AreEqual(Direction.North, r1.FaceDirection);
            r1.TurnRight();
            Assert.AreEqual(Direction.East, r1.FaceDirection);
            r1.TurnRight();
            Assert.AreEqual(Direction.South, r1.FaceDirection);
            r1.TurnRight();
            Assert.AreEqual(Direction.West, r1.FaceDirection);
            r1.TurnLeft();
            Assert.AreEqual(Direction.South, r1.FaceDirection);
            r1.TurnLeft();
            Assert.AreEqual(Direction.East, r1.FaceDirection);
            r1.TurnLeft();
            Assert.AreEqual(Direction.North, r1.FaceDirection);
        }

        [TestMethod("Einen schritt machen")]
        public void TestRobotMove1()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 1, 1);
            r1.Delay = 10;

            r1.Move();
            Assert.AreEqual(new Position(1, 0, 2), r1.Position);
            Assert.AreEqual(Direction.North, r1.FaceDirection);
        }

        [TestMethod("Schritte und Rotationen")]
        public void TestRobotMove2()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 1, 1);
            r1.Delay = 10;

            r1.Move();
            r1.TurnRight();
            r1.Move();
            Assert.AreEqual(new Position(2, 0, 2), r1.Position);
            Assert.AreEqual(Direction.East, r1.FaceDirection);
        }

        [TestMethod("Wandere um die Welt!")]
        public void TestRobotMove3()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 1, 1);
            r1.Delay = 10;

            for(int i = 0; i < 4; i++)
            {
                while (!r1.HasWall)
                {
                    r1.Move();
                }

                r1.TurnRight();
            }
        }

        [TestMethod("Laufen gegen eine Wand (Weltgrenze)")]
        [ExpectedException(typeof(InvalidMoveException))]
        public void TestMove4()
        {
            Robot r1 = new Robot(TestWorld, 1, 0);
            r1.Delay = 10;

            while (true)
            {
                r1.Move();
            }
        }

        [TestMethod("Laufen gegen eine Wand (Quader)")]
        [ExpectedException(typeof(InvalidMoveException))]
        public void TestMove5()
        {
            World w = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(1, 0, 4), new Cube() }
            });

            Robot r1 = new Robot(w, 1, 0);
            r1.Delay = 10;

            while (!false)
            {
                r1.Move();
            }
        }

        [TestMethod("Springen auf zu hohen Haufen")]
        [ExpectedException(typeof(InvalidMoveException))]
        public void TestMove6()
        {
            World w = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(1, 0, 2), new Brick() },
                { new Position(1, 1, 2), new Brick() },
                { new Position(1, 2, 2), new Brick() },
                { new Position(1, 3, 2), new Brick() }
            });

            Robot r1 = new Robot(w, 1, 0);
            r1.Delay = 10;
            r1.JumpHeight = 1;

            while(1 == 1)
            {
                r1.Move();
            }
        }

        [TestMethod("Springen auf Haufen")]
        public void TestMove7()
        {
            World w = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(1, 0, 2), new Brick() },
                { new Position(1, 1, 2), new Brick() },
                { new Position(1, 2, 2), new Brick() },
                { new Position(1, 3, 2), new Brick() }
            });

            Robot r1 = new Robot(w, 1, 0);
            r1.Delay = 10;
            r1.JumpHeight = 5;

            r1.Move();
            r1.Move();
        }

        [TestMethod("HasWall Test (mit Weltgrenze)")]
        public void TestHasWall1()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 0, 0);
            r1.Delay = 10;

            Assert.IsFalse(r1.HasWall);
            r1.TurnLeft();
            Assert.IsTrue(r1.HasWall);
        }

        [TestMethod("HasWall Test (mit Quader)")]
        public void TestHasWall2()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 0, 0);
            Assert.IsFalse(r1.HasWall);

            World we = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(0, 0, 1), new Cube() }
            });
            Robot re = new Robot(we, 0, 0);
            Assert.IsTrue(re.HasWall);
        }

        [TestMethod("HasMark Test")]
        public void TestHasMark1()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 0, 0);
            r1.Delay = 10;

            Assert.IsFalse(r1.HasMark);
            r1.PlaceMark();
            Assert.IsTrue(r1.HasMark);
            r1.PickUpMark();
            Assert.IsFalse(r1.HasMark);
        }

        [TestMethod("HasMark Test 2")]
        public void TestHasMark21()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 0, 0);
            r1.Delay = 10;

            Assert.IsFalse(r1.HasMark);
            r1.PlaceMark();
            Assert.IsTrue(r1.HasMark);
            r1.Move();
            Assert.IsFalse(r1.HasMark);
            r1.TurnLeft();
            r1.TurnLeft();
            r1.Move();
            Assert.IsTrue(r1.HasMark);
        }

        [TestMethod("HasBrick Test")]
        public void TestHasBrick()
        {
            World w1 = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(0, 0, 1), new Brick() }
            });

            Robot r1 = new Robot(w1, 0, 0);
            Assert.IsTrue(r1.HasBrick);
            Assert.IsFalse(r1.HasBrickLeft);
            Assert.IsFalse(r1.HasBrickRight);

            r1.TurnRight();
            Assert.IsFalse(r1.HasBrick);
            Assert.IsTrue(r1.HasBrickLeft);
            Assert.IsFalse(r1.HasBrickRight);

            r1.TurnLeft();
            r1.TurnLeft();
            Assert.IsFalse(r1.HasBrick);
            Assert.IsFalse(r1.HasBrickLeft);
            Assert.IsTrue(r1.HasBrickRight);
        }

        [TestMethod("FaceDirection Test")]
        public void TestFaceDirection()
        {
            Robot r1 = new Robot(TestWorld, 0, 0);
            r1.Delay = 10;

            Assert.IsTrue(r1.IsFacingNorth);
            Assert.IsFalse(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingSouth);
            Assert.IsFalse(r1.IsFacingWest);

            r1.TurnLeft();
            Assert.IsTrue(r1.IsFacingWest);
            Assert.IsFalse(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingSouth);
            Assert.IsFalse(r1.IsFacingNorth);

            r1.TurnLeft();
            Assert.IsTrue(r1.IsFacingSouth);
            Assert.IsFalse(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingNorth);

            r1.TurnLeft();
            Assert.IsTrue(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingWest);
            Assert.IsFalse(r1.IsFacingSouth);
            Assert.IsFalse(r1.IsFacingNorth);

            r1.TurnLeft();
            Assert.IsTrue(r1.IsFacingNorth);
            Assert.IsFalse(r1.IsFacingEast);
            Assert.IsFalse(r1.IsFacingSouth);
            Assert.IsFalse(r1.IsFacingWest);
        }

        [TestMethod("HasRobot Test")]
        public void TestHasRobot()
        {
            World w1 = TestWorld;
            Robot r1 = new Robot(w1, 4, 4);
            Robot r2 = new Robot(w1, 4, 5);
            r2.Delay = 10;

            Assert.IsFalse(r2.HasRobot);
            Assert.IsTrue(r1.HasRobot);

            r2.TurnLeft();
            r2.TurnLeft();
            Assert.IsTrue(r1.HasRobot);
            Assert.IsTrue(r2.HasRobot);
        }

        [TestMethod("Backpack Test")]
        public void TestBackpack1()
        {
            Robot r1 = new Robot(TestWorld, 2, 2);
            r1.Delay = 10;

            Assert.IsFalse(r1.IsBackpackEmpty);
            Assert.IsFalse(r1.IsBackpackFull);
            Assert.AreEqual(-1, r1.MaxBackpackSize);
            Assert.AreEqual(0, r1.BricksInBackpack);

            r1.MaxBackpackSize = 3;

            Assert.IsFalse(r1.IsBackpackFull);
            Assert.IsTrue(r1.IsBackpackEmpty);
            Assert.AreEqual(3, r1.MaxBackpackSize);
            Assert.AreEqual(0, r1.BricksInBackpack);

            r1.BricksInBackpack = 2;

            Assert.IsFalse(r1.IsBackpackEmpty);
            Assert.IsFalse(r1.IsBackpackFull);
            Assert.AreEqual(3, r1.MaxBackpackSize);
            Assert.AreEqual(2, r1.BricksInBackpack);

            r1.Place();
            r1.Place();

            Assert.IsTrue(r1.IsBackpackEmpty);
            Assert.IsFalse(r1.IsBackpackFull);
            Assert.AreEqual(3, r1.MaxBackpackSize);
            Assert.AreEqual(0, r1.BricksInBackpack);

            r1.BricksInBackpack = 3;

            Assert.IsFalse(r1.IsBackpackEmpty);
            Assert.IsTrue(r1.IsBackpackFull);
            Assert.AreEqual(3, r1.MaxBackpackSize);
            Assert.AreEqual(3, r1.BricksInBackpack);

            r1.BricksInBackpack = 1;
            r1.PickUp();

            Assert.IsFalse(r1.IsBackpackEmpty);
            Assert.IsFalse(r1.IsBackpackFull);
            Assert.AreEqual(3, r1.MaxBackpackSize);
            Assert.AreEqual(2, r1.BricksInBackpack);
        }

        [TestMethod("Pazieren eines Ziegels mit 0 in Rucksack")]
        [ExpectedException(typeof(InvalidActionException))]
        public void TestBackpack2()
        {
            Robot r1 = new Robot(TestWorld, 2, 2);
            r1.Delay = 10;
            r1.MaxBackpackSize = 10;
            r1.BricksInBackpack = 0;

            r1.Place();
        }

        [TestMethod("Aufheben eines Ziegels mit vollem Rucksack")]
        [ExpectedException(typeof(InvalidActionException))]
        public void TestBackpack3()
        {
            Robot r1 = new Robot(TestWorld, 2, 2);
            r1.Delay = 10;
            r1.MaxBackpackSize = 7;
            r1.BricksInBackpack = 7;

            r1.PickUp();
        }

        [TestMethod("Sound machen")]
        public void TestMakeSound()
        {
            Robot r1 = new Robot(TestWorld, 0, 3);
            r1.MakeSound();
        }

        [TestMethod("Sichtbarkeit testen")]
        public void TestIsVisible()
        {
            Robot r1 = new Robot(TestWorld, 4, 2);

            Assert.IsTrue(r1.IsVisible);
            r1.IsVisible = false;
            Assert.IsFalse(r1.IsVisible);
            r1.IsVisible = true;
            Assert.IsTrue(r1.IsVisible);
        }

        [TestMethod("FrontBrickCount testen")]
        public void TestFrontBrickCount()
        {
            World w = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(1, 0, 2), new Brick() },
                { new Position(1, 1, 2), new Brick() },
                { new Position(1, 2, 2), new Brick() },
                { new Position(1, 3, 2), new Brick() }
            });

            Robot r1 = new Robot(w, 1, 1);
            Robot r2 = new Robot(w, 0, 1);
            r1.Delay = 10;
            r2.Delay = 10;

            Assert.AreEqual(4, r1.FrontBrickCount);
            Assert.AreEqual(0, r2.FrontBrickCount);

            r2.Place();
            r1.PickUp();

            Assert.AreEqual(3, r1.FrontBrickCount);
            Assert.AreEqual(1, r2.FrontBrickCount);
        }

        [TestMethod("FrontBrickColor testen")]
        public void TestFrontBrickColor()
        {
            World w = WorldParser.Generate(5, 5, 5, new Dictionary<Position, WorldElement>()
            {
                { new Position(1, 0, 2), new Brick() },
                { new Position(1, 1, 2), new Brick(Color.Yellow) },
                { new Position(1, 2, 2), new Brick(Color.Blue) },
                { new Position(1, 3, 2), new Brick(Color.Green) }
            });

            Robot r1 = new Robot(w, 1, 1);
            Robot r2 = new Robot(w, 0, 1);
            r1.Delay = 10;
            r2.Delay = 10;

            Assert.AreEqual(Color.Transparent, r2.FrontBrickColor);
            Assert.AreEqual(Color.Green, r1.FrontBrickColor);

            r2.Place(Color.Orange);
            r1.PickUp();

            Assert.AreEqual(Color.Orange, r2.FrontBrickColor);
            Assert.AreEqual(Color.Blue, r1.FrontBrickColor);
        }
    }
}

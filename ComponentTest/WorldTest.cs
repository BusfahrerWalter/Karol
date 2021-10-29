using Karol;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Karol.Core.Exceptions;

namespace ComponentTest
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidSizeException))]
        public void TestConstructor1()
        {
            World w1 = new World(0, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSizeException))]
        public void TestConstructor2()
        {
            World w1 = new World(0, 0, 0);
        }

        [TestMethod]
        public void TestConstructor3()
        {
            World w1 = new World(5, 5, 5);
        }

        [TestMethod]
        public void TestLoad()
        {
            World w1 = World.Load("AAAAAAAAAAAAAAAAAAAAAA");
            Assert.AreEqual(null, w1);
        }
    }
}

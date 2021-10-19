using Karol.Core;
using Karol.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karol
{
    /// <summary>
    /// Eine erweiterung des normalen Roboters
    /// </summary>
    public class SuperRobot : Robot
    {
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter eine Marke betritt.
        /// </summary>
        public event EventHandler onEnterMark;
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter eine Marke verlässt.
        /// </summary>
        public event EventHandler onLeaveMark;
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter einen Ziegel platziert.
        /// </summary>
        public event EventHandler onPlaceBrick;
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter einen Ziegel aufhebt.
        /// </summary>
        public event EventHandler onPickUpBrick;

        internal SuperRobot() : base() { }

        internal SuperRobot(int xStart, int zStart, World world, Direction initDir, bool updateView = true, bool placeInWorld = true)
            : base(xStart, zStart, world, initDir, updateView, placeInWorld) 
        {
            onEnterMarkPreview += (s, e) => onEnterMark?.Invoke(this, e);
            onPickUpBrickPreview += (s, e) => onLeaveMark?.Invoke(this, e); ;
            onPlaceBrickPreview += (s, e) => onPlaceBrick?.Invoke(this, e); ;
            onPickUpBrickPreview += (s, e) => onPickUpBrick?.Invoke(this, e); ;
        }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <param name="initialDirection">Start Blickrichtung des Roboters. <br></br>Standard ist Direction.North</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public SuperRobot(int xStart, int zStart, World world, Direction initialDirection)
            : this(xStart, zStart, world, initialDirection, true, true) { }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public SuperRobot(int xStart, int zStart, World world)
            : this(xStart, zStart, world, Direction.North) { }

        /// <summary>
        /// Erstellt einen neuen Roboter. An der Position 0, 0
        /// </summary>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <param name="initialDirection">Start Blickrichtung des Roboters. <br></br>Standard ist Direction.North</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public SuperRobot(World world, Direction initialDirection)
            : this(0, 0, world, initialDirection) { }

        /// <summary>
        /// Erstellt einen neuen Roboter. An der Position 0, 0
        /// </summary>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public SuperRobot(World world) 
            : this(0, 0, world, Direction.North) { }

        /// <summary>
        /// Erstellt einen neuen Roboter anhand der übergebennen Optionen
        /// </summary>
        /// <param name="options">Roboter Optionen</param>
        public SuperRobot(RobotOptions options)
            : base(options) { }
    }
}

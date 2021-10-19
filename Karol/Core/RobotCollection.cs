using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core
{
    internal class RobotCollection : IEnumerable<Robot>
    {
        public int Count => Robots.Count;
        public Robot Last => Count > 0 ? this[Count - 1] : null;

        public event EventHandler<WorldChangedEventArgs> onRobotAdded;
        public event EventHandler<WorldChangedEventArgs> onRobotRemoved;

        private List<Robot> Robots { get; set; }
        private int MaxSize { get; set; }

        public RobotCollection(int maxSize)
        {
            Robots = new List<Robot>();
            MaxSize = maxSize;
        }

        public void Add(Robot robo)
        {
            if (Count + 1 > MaxSize)
                throw new ArgumentOutOfRangeException($"In dieser Welt können sich maximal {MaxSize} Roboter befinden!");

            Robots.Add(robo);
            OnRobotAdded(robo);
        }

        public void Remove(Robot robo)
        {
            if (Robots.Remove(robo))
            {
                OnRobotRemoved(robo);
            }
        }

        public void RemoveAt(int index)
        {
            if(Robots.Count > index)
            {
                var robo = this[index];
                Robots.RemoveAt(index);
                OnRobotRemoved(robo);
            }
        }

        public bool Contains(Robot robo)
        {
            return Robots.Contains(robo);
        }

        public Robot[] ToArray()
        {
            return Robots.ToArray();
        }

        public IEnumerator<Robot> GetEnumerator()
        {
            return Robots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Robot this[int index]
        {
            get => Robots[index];
            set => Robots[index] = value;
        }

        private void OnRobotAdded(Robot newR)
        {
            onRobotAdded?.Invoke(this, new WorldChangedEventArgs(newR));
        }

        private void OnRobotRemoved(Robot newR)
        {
            onRobotRemoved?.Invoke(this, new WorldChangedEventArgs(newR));
        }
    }
}

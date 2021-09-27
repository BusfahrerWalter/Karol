using Karol.Core;
using Karol.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karol
{
    public class Controller
    {
        public Robot ControlledRobot { get; set; }
        private ControllerForm Form { get; set; }

        public Controller(Robot robot)
        {
            ControlledRobot = robot;
            OpenWindow();
        }

        #region Zeug
        private void OpenWindow()
        {
            Form = new ControllerForm();
            Task.Run(() =>
            {
                Application.Run(Form);
            });

            Form.ButtonDown.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    MoveTo(Direction.South);
                });
            };

            Form.ButtonUp.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    MoveTo(Direction.North);
                });
            };

            Form.ButtonLeft.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    MoveTo(Direction.East);
                });
            };

            Form.ButtonRight.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    MoveTo(Direction.Ost);
                });
            };

            Form.TurnLeftButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.TurnLeft();
                });
            };

            Form.TurnRightButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.TurnRight();
                });
            };

            Form.PlaceBrickButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.Place();
                });
            };
        }
        #endregion

        private void MoveTo(Direction dir)
        {
            ControlledRobot.FaceDirection = dir;
            ControlledRobot.Move();
        }

        private void RobotAction(Action action)
        {
            var delay = ControlledRobot.Delay;
            ControlledRobot.Delay = 0;

            try
            {
                action.Invoke();
            }
            catch(KarolException e)
            {
                Form.LogListBox.Items.Add(e.Message);
            }

            ControlledRobot.Delay = delay;
        }
    }
}

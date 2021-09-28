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

        public static Controller Create(Robot robo)
        {
            return new Controller(robo);
        }

        #region Zeug
        private void OpenWindow()
        {
            Form = new ControllerForm();
            Form.JumpHeightInput.Value = ControlledRobot.JumpHeight;
            Form.DelayInput.Value = ControlledRobot.Delay;
            Form.ColorDialog.Color = ControlledRobot.Paint;

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

            Form.PickUpBrickButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.PickUp();
                });
            };

            Form.DelayInput.ValueChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.Delay = (int)Form.DelayInput.Value;
                });
            };

            Form.JumpHeightInput.ValueChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.JumpHeight = (int)Form.JumpHeightInput.Value;
                });
            };

            Form.onColorChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.Paint = Form.ColorDialog.Color;
                });
            };
        }

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
                Form.LogListBox.SelectedIndex = Form.LogListBox.Items.Count - 1;
            }

            ControlledRobot.Delay = delay;
        }
        #endregion
    }
}

using Karol.Core;
using Karol.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karol
{
    /// <summary>
    /// Hilfsklasse um einen Roboter zu steuern.
    /// </summary>
    public class Controller
    {
        private static List<Controller> ActiveControllers = new List<Controller>();

        public Robot ControlledRobot { get; set; }
        private ControllerForm Form { get; set; }

        public Controller(Robot robot)
        {
            ControlledRobot = robot;
            OpenWindow();
        }

        /// <summary>
        /// Erzeugt einen Controller der einen Roboter steuert.
        /// </summary>
        /// <param name="robo">Roboter der gesteuert werden soll.</param>
        /// <returns>Controller instantz die mit dem Roboter verknüpft ist.</returns>
        public static Controller Create(Robot robo)
        {
            return new Controller(robo);
        }

        #region Zeug
        private void OpenWindow()
        {
            if(ActiveControllers.Any(c => c.ControlledRobot == ControlledRobot))
            {
                MessageBox.Show("Für einen Roboter kann nur eine Controller Instanz zur gleichen Zeit aktiv sein!", "Oh nein!!!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form = new ControllerForm();
            Form.JumpHeightInput.Value = ControlledRobot.JumpHeight;
            Form.DelayInput.Value = ControlledRobot.Delay;
            Form.ColorDialog.Color = ControlledRobot.Paint;
            Form.MaxBackpackSizeInput.Value = ControlledRobot.MaxBackpackSize;
            Form.BrickCountInput.Value = ControlledRobot.BricksInBackpack;
            Form.Text = $"Karol Controller - Robot {ControlledRobot.Number}";
            ActiveControllers.Add(this);

            Task.Run(() =>
            {
                Application.Run(Form);
            });

            Form.FormClosed += (e, args) =>
            {
                ActiveControllers.Remove(this);
            };

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

            Form.PlaceMarkButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.PlaceMark();
                });
            };

            Form.PickUpMarkButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.PickUpMark();
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

            Form.BrickCountInput.ValueChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.BricksInBackpack = (int)Form.BrickCountInput.Value;
                });
            };

            Form.MaxBackpackSizeInput.ValueChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.MaxBackpackSize = (int)Form.MaxBackpackSizeInput.Value;
                });
            };

            ControlledRobot.onEnterMark += (e, args) =>
            {
                Form.PlaceMarkButton.Enabled = false;
                Form.PickUpMarkButton.Enabled = true;
            };

            ControlledRobot.onLeaveMark += (e, args) =>
            {
                Form.PlaceMarkButton.Enabled = true;
                Form.PickUpMarkButton.Enabled = false;
            };

            ControlledRobot.onPlaceBrick += (e, args) =>
            {
                Form.BrickCountInput.Value = ControlledRobot.BricksInBackpack;
            };

            ControlledRobot.onPickUpBrick += (e, args) =>
            {
                Form.BrickCountInput.Value = ControlledRobot.BricksInBackpack;
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

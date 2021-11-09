using Karol.Core;
using Karol.Core.Exceptions;
using Karol.Core.Extensions;
using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        /// <summary>
        /// Roboter der von diesem Controller gesteuert wird.
        /// </summary>
        public Robot ControlledRobot { get; private set; }

        private ControllerForm Form { get; set; }
        private Dictionary<Keys, Action> InputMap { get; set; }

        private Controller(Robot robot)
        {
            ControlledRobot = robot;
            OpenWindow();
        }

        /// <summary>
        /// Erzeugt einen Controller der einen Roboter steuert. <br></br>
        /// Gibt null zurück wenn bereits ein Controller für diesen Roboter existiert.
        /// </summary>
        /// <param name="robo">Roboter der gesteuert werden soll.</param>
        /// <returns>Controller instantz die mit dem Roboter verknüpft ist.</returns>
        public static Controller Create(Robot robo)
        {
            if (ActiveControllers.Any(c => c.ControlledRobot == robo))
            {
                MessageBox.Show("Für einen Roboter kann nur eine Controller Instanz zur gleichen Zeit aktiv sein!", "Oh nein!!!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return new Controller(robo);
        }

        /// <summary>
        /// Schließt alle offenen Controller
        /// </summary>
        public static void CloseAll()
        {
            for(int i = ActiveControllers.Count - 1; i >= 0; i--)
            {
                ActiveControllers[i].Form.Invoke((MethodInvoker)delegate
                {
                    ActiveControllers[i].Form.Close();
                });
            }
        }

        #region Zeug
        private void OpenWindow()
        {
            Form = new ControllerForm();
            Form.JumpHeightInput.Value = ControlledRobot.JumpHeight;
            Form.DelayInput.Value = ControlledRobot.Delay;
            Form.ColorDialog.Color = ControlledRobot.Paint;
            Form.MaxBackpackSizeInput.Value = ControlledRobot.MaxBackpackSize;
            Form.BrickCountInput.Value = ControlledRobot.BricksInBackpack;
            Form.PickUpMarkButton.Enabled = ControlledRobot.HasMark;
            Form.PlaceMarkButton.Enabled = !ControlledRobot.HasMark;
            Form.Text = $"Karol Controller - Robot {ControlledRobot.Identifier}";

            UpdateInfo();

            ActiveControllers.Add(this);
            InputMap = new Dictionary<Keys, Action>()
            {
                { Keys.W, Form.ButtonUp.PerformClick },
                { Keys.A, Form.ButtonLeft.PerformClick },
                { Keys.S, Form.ButtonDown.PerformClick },
                { Keys.D, Form.ButtonRight.PerformClick },
                { Keys.Up, Form.ButtonUp.PerformClick },
                { Keys.Left, Form.ButtonLeft.PerformClick },
                { Keys.Down, Form.ButtonDown.PerformClick },
                { Keys.Right, Form.ButtonRight.PerformClick },
                { Keys.E, Form.TurnRightButton.PerformClick },
                { Keys.Q, Form.TurnLeftButton.PerformClick },
                { Keys.R, Form.PickUpBrickButton.PerformClick },
                { Keys.F, Form.PlaceBrickButton.PerformClick },
                { Keys.M, PlaceOrPickUpMark },
            };

            Task.Run(() =>
            {
                Application.Run(Form);
            });

            ControlledRobot.World.onWorldClosed += (e, args) =>
            {
                if (!Form.IsHandleCreated)
                    return;

                Form.Invoke((MethodInvoker)delegate
                {
                    Form.Close();
                });
            };

            Form.FormClosed += (e, args) =>
            {
                ActiveControllers.Remove(this);
            };

            Form.KeyUp += (e, args) =>
            {
                if (!InputMap.ContainsKey(args.KeyCode))
                    return;

                InputMap[args.KeyCode].Invoke();
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
                    MoveTo(Direction.West);
                });
            };

            Form.ButtonRight.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    MoveTo(Direction.East);
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

                UpdateInfo();
            };

            Form.PickUpMarkButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.PickUpMark();
                });

                UpdateInfo();
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

            Form.GetPaintButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    if (ControlledRobot.FrontBrickColor == Color.Transparent)
                        return;

                    ControlledRobot.Paint = ControlledRobot.FrontBrickColor;
                    Form.ColorDialog.Color = ControlledRobot.FrontBrickColor;
                    Form.SelectColorButton.BackColor = ControlledRobot.FrontBrickColor;
                });
            };

            Form.BrickCountInput.ValueChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.BricksInBackpack = (int)Form.BrickCountInput.Value;
                });

                UpdateBackpackInfo();
            };

            Form.MaxBackpackSizeInput.ValueChanged += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.MaxBackpackSize = (int)Form.MaxBackpackSizeInput.Value;
                });

                Form.BrickCountInput.Maximum = Form.MaxBackpackSizeInput.Value;
                UpdateBackpackInfo();
            };

            Form.PlaceCubeButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.PlaceCube();
                });
            };

            Form.PickUpCubeButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.PickUpCube();
                });
            };

            Form.IsVisibleCheckbox.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    ControlledRobot.IsVisible = Form.IsVisibleCheckbox.Checked;
                });
            };

            Form.KillButton.Click += (e, args) =>
            {
                RobotAction(() =>
                {
                    WorldElement.Destroy(ControlledRobot, !ControlledRobot.HasMark);
                    Form.Close();
                });
            };

            ControlledRobot.onEnterMarkPreview += (e, args) =>
            {
                Form.InvokeFormMethod(() =>
                {
                    Form.PlaceMarkButton.Enabled = false;
                    Form.PickUpMarkButton.Enabled = true;
                });
            };

            ControlledRobot.onLeaveMarkPreview += (e, args) =>
            {
                Form.InvokeFormMethod(() =>
                {
                    Form.PlaceMarkButton.Enabled = true;
                    Form.PickUpMarkButton.Enabled = false;
                });
            };

            ControlledRobot.onPlaceBrickPreview += (e, args) =>
            {
                Form.InvokeFormMethod(() =>
                {
                    Form.BrickCountInput.Value = ControlledRobot.BricksInBackpack;
                });

                UpdateInfo();
            };

            ControlledRobot.onPickUpBrickPreview += (e, args) =>
            {
                Form.InvokeFormMethod(() =>
                {
                    Form.BrickCountInput.Value = ControlledRobot.BricksInBackpack;
                });
                
                UpdateInfo();
            };

            ControlledRobot.onMove += (e, args) =>
            {
                UpdateInfo();
            };
        }

        private void UpdateInfo()
        {
            Form.InvokeFormMethod(() =>
            {
                Form.PositionTextBox.Text = ControlledRobot.Position.ToString();
                Form.FaceDirectionTextBox.Text = ControlledRobot.FaceDirection.ToString();
                Form.HasWallCheckBox.Checked = ControlledRobot.HasWall;
                Form.HasMarkCheckBox.Checked = ControlledRobot.HasMark;
                Form.HasBrickCheckBox.Checked = ControlledRobot.HasBrick;
                Form.HasRoboCheckBox.Checked = ControlledRobot.HasRobot;
                UpdateBackpackInfo();
            });       
        }

        private void UpdateBackpackInfo()
        {
            Form.InvokeFormMethod(() =>
            {
                Form.BackPackEmptyCheckBox.Checked = ControlledRobot.IsBackpackEmpty;
                Form.BackPackFullCheckBox.Checked = ControlledRobot.IsBackpackFull;
            });
        }

        private void MoveTo(Direction dir)
        {
            ControlledRobot.FaceDirection = dir;
            ControlledRobot.Move();
        }

        private void PlaceOrPickUpMark()
        {
            if (ControlledRobot.HasMark)
            {
                Form.PickUpMarkButton.PerformClick();
            }
            else
            {
                Form.PlaceMarkButton.PerformClick();
            }
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

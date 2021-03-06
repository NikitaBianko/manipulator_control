using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManipulatorControl.Core;

namespace manipulator_control
{
    public partial class MainWindow : Window
    {
        private SerialCommunications serial = new SerialCommunications();

        Manipulator manipulator;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPort();

            manipulator = new Manipulator(13, 13, 13, true);
            manipulator.SetFixedWrist(false);
            manipulator.SetAngleToHorizon(0);


        }

        private void InitializeSerialPort()
        {
            string[] serialPorts = serial.GetPorts();

            SerialPortNames.Items.Clear();
            if (serialPorts.Count() != 0)
            {
                foreach (string serial in serialPorts)
                {
                    SerialPortNames.Items.Add(serial);
                }
                SerialPortNames.SelectedItem = serialPorts[0];
            }
        }

        private void ConnectToSerialPort()
        {
            try
            {
                string selectedSerialPort = SerialPortNames.SelectedItem.ToString();
                serial.Connect(selectedSerialPort, 9600);

                var message = new byte[] { (int)State.Init << 4 };
                serial.Send(message);

                SerialPortConnectBtn.Content = "Disconnect";
                SendBtn.IsEnabled = true;
                ForwardBtn.IsEnabled = true;
                LeftBtn.IsEnabled = true;   
                DownBtn.IsEnabled = true;  
                RightBtn.IsEnabled = true;  
                BackBtn.IsEnabled = true;  
                UpBtn.IsEnabled = true;
                RunBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void DisconnectToSerialPort()
        {
            serial.Close();
            SerialPortConnectBtn.Content = "Connect";
            SendBtn.IsEnabled = false;
            ForwardBtn.IsEnabled = false;
            LeftBtn.IsEnabled = false;
            DownBtn.IsEnabled = false;
            RightBtn.IsEnabled = false;
            BackBtn.IsEnabled = false;
            UpBtn.IsEnabled = false;
            RunBtn.IsEnabled = false;
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            InitializeSerialPort();
        }

        private void SerialPortConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (serial.isConnected)
            {
                DisconnectToSerialPort();
            } 
            else
            {
                ConnectToSerialPort();
            }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manipulator.MoveInPoint(new ManipulatorControl.Core.Point(double.Parse(XCoordinate.Text), double.Parse(YCoordinate.Text), double.Parse(ZCoordinate.Text)));
                manipulator.WristCompression = System.Convert.ToByte(Compress.Text);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void CoordinatesBtn_Click(object sender, RoutedEventArgs e)
        {
            CoordinatesBtn.IsEnabled = false;
            ControlBtn.IsEnabled = true;
            ProgrammingBtn.IsEnabled = true;
            Coordinates.Visibility = Visibility.Visible;
            Control.Visibility = Visibility.Collapsed;
            Programming.Visibility = Visibility.Collapsed;
        }

        private void ControlBtn_Click(object sender, RoutedEventArgs e)
        {
            CoordinatesBtn.IsEnabled = true;
            ControlBtn.IsEnabled = false;
            ProgrammingBtn.IsEnabled = true;
            Coordinates.Visibility = Visibility.Collapsed;
            Control.Visibility = Visibility.Visible;
            Programming.Visibility = Visibility.Collapsed;
        }

        private void ProgrammingBtn_Click(object sender, RoutedEventArgs e)
        {
            CoordinatesBtn.IsEnabled = true;
            ControlBtn.IsEnabled = true;
            ProgrammingBtn.IsEnabled = false;
            Coordinates.Visibility = Visibility.Collapsed;
            Control.Visibility = Visibility.Collapsed;
            Programming.Visibility = Visibility.Visible;
        }

        double stepManipulator = 1;

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentPosition = manipulator.Position;

                currentPosition.Y += stepManipulator;

                manipulator.MoveInPoint(currentPosition);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentPosition = manipulator.Position;

                currentPosition.X -= stepManipulator;

                manipulator.MoveInPoint(currentPosition);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentPosition = manipulator.Position;

                currentPosition.X += stepManipulator;

                manipulator.MoveInPoint(currentPosition);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentPosition = manipulator.Position;

                currentPosition.Y -= stepManipulator;

                manipulator.MoveInPoint(currentPosition);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentPosition = manipulator.Position;

                currentPosition.Z -= stepManipulator;

                manipulator.MoveInPoint(currentPosition);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentPosition = manipulator.Position;

                currentPosition.Z += stepManipulator;

                manipulator.MoveInPoint(currentPosition);

                byte state = ((int)State.Successful << 4) + 5;
                var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};

                serial.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ControlBtn.IsEnabled)
            {
                switch (e.Key)
                {
                    case Key.W:
                        Forward_Click(sender, e);
                        break;
                    case Key.S:
                        Back_Click(sender, e);
                        break;
                    case Key.A:
                        Left_Click(sender, e);
                        break;
                    case Key.D:
                        Right_Click(sender, e);
                        break;
                    case Key.E:
                        Up_Click(sender, e);
                        break;
                    case Key.Q:
                        Down_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }
        }
        private void runningProgram(String str)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                if ((str[i] < 'a' || str[i] > 'z') && (str[i] < 'A' || str[i] > 'Z') && (str[i] < '0' || str[i] > '9') && str[i] != ';' && str[i] != '(' && str[i] != ')' && str[i] != ',' && str[i] != '-')
                {
                    str = str.Remove(i--, 1);
                }
            }

            var commands = str.Split(';');

            foreach (var command in commands)
            {
                if (command.Length > 0)
                {
                    var commandName = command.Split('(')[0];
                    var commandParams = command.Split('(', ')')[1].Split(',').Select(x => int.Parse(x)).ToList();

                    if (commandName == "move")
                    {
                        manipulator.MoveInPoint(new ManipulatorControl.Core.Point(commandParams[0], commandParams[1], commandParams[2]));
                        byte state = ((int)State.Successful << 4) + 5;
                        var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};
                        serial.Send(message);
                    }
                    else if (commandName == "delay")
                    {
                        Thread.Sleep(commandParams[0]);
                    }
                    else if (commandName == "compress")
                    {
                        byte state = ((int)State.Successful << 4) + 5;
                        manipulator.WristCompression = System.Convert.ToByte(commandParams[0]);
                        var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};
                        serial.Send(message);
                    }
                    else if (commandName == "setAngles")
                    {
                        manipulator.SetAngles(commandParams[0], commandParams[1], commandParams[2], commandParams[3]);
                        byte state = ((int)State.Successful << 4) + 5;
                        var message = new byte[] { state,
                                System.Convert.ToByte(manipulator.Angle),
                                System.Convert.ToByte(manipulator.ShoulderAngle),
                                System.Convert.ToByte(manipulator.ElbowAngle),
                                System.Convert.ToByte(manipulator.WristAngle),
                                System.Convert.ToByte(manipulator.WristCompression)};
                        serial.Send(message);
                    }
                    else
                    {
                        throw new Exception("unknown command");
                    }
                }
            }
        }
        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                RunBtn.IsEnabled = false;

                //Task running = new Task(() => runningProgram(Code.Text));   

                //running.Start();
                //running.Wait();

                runningProgram(Code.Text);


                RunBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}

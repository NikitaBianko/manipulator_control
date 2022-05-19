using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
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

            manipulator = new Manipulator(7.65, 7.92, 4, true);
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

                SerialPortConnectBtn.Content = "Disconnect";
                SendBtn.IsEnabled = true;
                ForwardBtn.IsEnabled = true;
                LeftBtn.IsEnabled = true;   
                DownBtn.IsEnabled = true;  
                RightBtn.IsEnabled = true;  
                BackBtn.IsEnabled = true;  
                UpBtn.IsEnabled = true;
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

                byte state = ((int)State.Successful << 4) + 4; // 0000 0100 -> No instructions and 4 bytes will be sent next

                var message = new byte[] { state,
                    System.Convert.ToByte(manipulator.Angle), 
                    System.Convert.ToByte(manipulator.ShoulderAngle), 
                    System.Convert.ToByte(manipulator.ElbowAngle), 
                    System.Convert.ToByte(manipulator.WristAngle) };

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

                byte state = ((int)State.Successful << 4) + 4;

                var message = new byte[] { state,
                System.Convert.ToByte(manipulator.Angle),
                System.Convert.ToByte(manipulator.ShoulderAngle),
                System.Convert.ToByte(manipulator.ElbowAngle),
                System.Convert.ToByte(manipulator.WristAngle) };

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

                byte state = ((int)State.Successful << 4) + 4;

                var message = new byte[] { state,
                System.Convert.ToByte(manipulator.Angle),
                System.Convert.ToByte(manipulator.ShoulderAngle),
                System.Convert.ToByte(manipulator.ElbowAngle),
                System.Convert.ToByte(manipulator.WristAngle) };

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

                byte state = ((int)State.Successful << 4) + 4;

                var message = new byte[] { state,
                System.Convert.ToByte(manipulator.Angle),
                System.Convert.ToByte(manipulator.ShoulderAngle),
                System.Convert.ToByte(manipulator.ElbowAngle),
                System.Convert.ToByte(manipulator.WristAngle) };

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

                byte state = ((int)State.Successful << 4) + 4;

                var message = new byte[] { state,
                System.Convert.ToByte(manipulator.Angle),
                System.Convert.ToByte(manipulator.ShoulderAngle),
                System.Convert.ToByte(manipulator.ElbowAngle),
                System.Convert.ToByte(manipulator.WristAngle) };

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

                byte state = ((int)State.Successful << 4) + 4;

                var message = new byte[] { state,
                System.Convert.ToByte(manipulator.Angle),
                System.Convert.ToByte(manipulator.ShoulderAngle),
                System.Convert.ToByte(manipulator.ElbowAngle),
                System.Convert.ToByte(manipulator.WristAngle) };

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

                byte state = ((int)State.Successful << 4) + 4;

                var message = new byte[] { state,
                System.Convert.ToByte(manipulator.Angle),
                System.Convert.ToByte(manipulator.ShoulderAngle),
                System.Convert.ToByte(manipulator.ElbowAngle),
                System.Convert.ToByte(manipulator.WristAngle) };

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
                    case Key.Space:
                        Up_Click(sender, e);
                        break;
                    case Key.LeftShift:
                        Down_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

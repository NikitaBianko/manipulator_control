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

            manipulator = new Manipulator(7.65, 7.92, 4);
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
                serial.Connect(selectedSerialPort, 115200);

                SerialPortConnectBtn.Content = "Disconnect";
                SendBtn.IsEnabled = true;
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

                byte state = 15; // 0000 0100 -> No instructions and 4 bytes will be sent next

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
    }
}

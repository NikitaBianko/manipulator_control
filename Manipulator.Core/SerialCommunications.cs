 using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulatorControl.Core
{
    public class SerialCommunications
    {
        public SerialPort serialPort { get; private set; }
        public bool isConnected { get; private set; } = false;

        public SerialCommunications()
        {

        }

        public void Send(byte[] binary)
        {
            if(!isConnected)
            {
                throw new Exception("serial port not connected"); 
            }

            try
            {
                serialPort.Write(binary, 0, binary.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string[] GetPorts()  
        {

            return SerialPort.GetPortNames();

        }
        public void Connect(string portName, int baudRate)
        {
            try
            {
                string selectedSerialPort = portName;
                serialPort = new SerialPort(selectedSerialPort, baudRate);
                serialPort.Open();

                isConnected = true;
            }
            catch (Exception ex)
            {

            }
        }

        public void Close()
        {
            try
            {
                serialPort.Close();
                isConnected = false;
            }
            catch 
            { 

            }
        }

    }
}

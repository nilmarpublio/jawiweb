using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SvgCut
{
    public partial class Form1 : Form
    {
        private SerialPort port;

        public Form1()
        {
            InitializeComponent();
            Initial();
        }

        #region Methods
        /// <summary>
        /// Initial layout.
        /// </summary>
        private void Initial()
        {
            // define available ports in this computer
            foreach (string p in SerialPort.GetPortNames())
                comboBox1.Items.Add(p);

            // bind data bits
            for (int i = 5; i < 9; i++)
                comboBox3.Items.Add(i);

            // bind parity
            foreach (string parity in System.Enum.GetNames(typeof(System.IO.Ports.Parity)))
                comboBox4.Items.Add(parity);

            // bind stop bits
            foreach (string bit in System.Enum.GetNames(typeof(System.IO.Ports.StopBits)))
                comboBox5.Items.Add(bit);
        }

        private void BindBraudRates(string portName)
        {
            // bind braud rate
            SerialPort po = new SerialPort(portName);

            try
            {
                po.Open();
                object p = po.BaseStream.GetType().GetField("commProp", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(po.BaseStream);
                Int32 dwSettableBaud = (Int32)p.GetType().GetField("dwSettableBaud", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(p);
                int[] rates = UpdateBaudRateCollection(dwSettableBaud);

                comboBox2.Items.Clear();
                foreach (int rate in rates)
                    comboBox2.Items.Add(rate);
                po.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Updates the range of possible baud rates for device
        /// </summary>
        /// <param name="possibleBaudRates">dwSettableBaud parameter from the COMMPROP Structure</param>
        /// <returns>An updated list of values</returns>
        public int[] UpdateBaudRateCollection(int possibleBaudRates)
        {
            List<int> rates = new List<int>();

            const int BAUD_075 = 0x00000001;
            const int BAUD_110 = 0x00000002;
            const int BAUD_150 = 0x00000008;
            const int BAUD_300 = 0x00000010;
            const int BAUD_600 = 0x00000020;
            const int BAUD_1200 = 0x00000040;
            const int BAUD_1800 = 0x00000080;
            const int BAUD_2400 = 0x00000100;
            const int BAUD_4800 = 0x00000200;
            const int BAUD_7200 = 0x00000400;
            const int BAUD_9600 = 0x00000800;
            const int BAUD_14400 = 0x00001000;
            const int BAUD_19200 = 0x00002000;
            const int BAUD_38400 = 0x00004000;
            const int BAUD_56K = 0x00008000;
            const int BAUD_57600 = 0x00040000;
            const int BAUD_115200 = 0x00020000;
            const int BAUD_128K = 0x00010000;

            if ((possibleBaudRates & BAUD_075) > 0)
                rates.Add(75);
            if ((possibleBaudRates & BAUD_110) > 0)
                rates.Add(110);
            if ((possibleBaudRates & BAUD_150) > 0)
                rates.Add(150);
            if ((possibleBaudRates & BAUD_300) > 0)
                rates.Add(300);
            if ((possibleBaudRates & BAUD_600) > 0)
                rates.Add(600);
            if ((possibleBaudRates & BAUD_1200) > 0)
                rates.Add(1200);
            if ((possibleBaudRates & BAUD_1800) > 0)
                rates.Add(1800);
            if ((possibleBaudRates & BAUD_2400) > 0)
                rates.Add(2400);
            if ((possibleBaudRates & BAUD_4800) > 0)
                rates.Add(4800);
            if ((possibleBaudRates & BAUD_7200) > 0)
                rates.Add(7200);
            if ((possibleBaudRates & BAUD_9600) > 0)
                rates.Add(9600);
            if ((possibleBaudRates & BAUD_14400) > 0)
                rates.Add(14400);
            if ((possibleBaudRates & BAUD_19200) > 0)
                rates.Add(19200);
            if ((possibleBaudRates & BAUD_38400) > 0)
                rates.Add(38400);
            if ((possibleBaudRates & BAUD_56K) > 0)
                rates.Add(56000);
            if ((possibleBaudRates & BAUD_57600) > 0)
                rates.Add(57600);
            if ((possibleBaudRates & BAUD_115200) > 0)
                rates.Add(115200);
            if ((possibleBaudRates & BAUD_128K) > 0)
                rates.Add(128000);

            return rates.ToArray();
        }

        /// <summary>
        /// Convert from svg file to other format by uniconvertor.
        /// </summary>
        /// <param name="extension"></param>
        private void ConvertTo(string source, string extension)
        {
            // Convert svg to plt
            string target = source.Replace(".svg", "." + extension);
            string commandString = String.Format("\"{0}\" \"{1}\"", source, target);
            System.Diagnostics.Debug.WriteLine("uniconvertor " + commandString);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "uniconvertor";
            startInfo.Arguments = commandString;
            process.StartInfo = startInfo;
            process.Start();
        }
        /// <summary>
        /// Connects to a serial port defined through the current settings
        /// </summary>
        public void StartListening()
        {
            // Closing serial port if it is open
            StopListening();

            // Setting serial port settings
            this.port = new SerialPort(
                comboBox1.Text,
                Convert.ToInt32(comboBox2.Text),
                (Parity)System.Enum.Parse(typeof(Parity), comboBox4.Text),
                Convert.ToInt32(comboBox3.Text),
                (StopBits)System.Enum.Parse(typeof(StopBits), comboBox5.Text));

            // Subscribe to event and open serial port for data
            this.port.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            this.port.Open();
            OnPropertyChanged("IsOpen");
        }
        /// <summary>
        /// Closes the serial port
        /// </summary>
        public void StopListening()
        {
            if (this.port != null && this.port.IsOpen)
            {
                this.port.DataReceived -= new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                this.port.Close();
                OnPropertyChanged("IsOpen");
            }
        }
        /// <summary>
        /// Deprecated.
        /// </summary>
        public void Connect()
        {
            System.Diagnostics.Debug.WriteLine("Connecting to COM...");
            try
            {
                StartListening();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                StopListening();
                return;
            }
        }
        /// <summary>
        /// Disconnect COM port.
        /// </summary>
        public void Disconnect()
        {
            System.Diagnostics.Debug.WriteLine("Disconnected COM.");
            StopListening();
        }
        /// <summary>
        /// Send file to com.
        /// </summary>
        /// <param name="fileName"></param>
        private void Send(string fileName)
        {
            using (System.IO.TextReader reader = System.IO.File.OpenText(fileName))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    this.port.Write(line);
                }
            }

        }
        #endregion

        #region Events
        #region INotifyPropertyChanged Members
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null))
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved;
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = this.port.BytesToRead;
            byte[] data = new byte[dataLength];
            int nbrDataRead = this.port.Read(data, 0, dataLength);
            if (nbrDataRead == 0)
                return;

            // Send data to whom ever interested
            if (NewSerialDataRecieved != null)
                NewSerialDataRecieved(this, new SerialDataEventArgs(data));
        }


        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BindBraudRates((sender as ComboBox).Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //open up file dialog
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".svg";
            dialog.Filter = "Scalable Vector Graphic (.svg)|*.svg";
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = dialog.FileName;
        }
        private void Button3Click(object sender, EventArgs e)
        {
            ConvertTo(textBox1.Text, comboBox6.Text.ToLower());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Convert svg to plt
            string source = textBox1.Text;
            string target = source.Replace(".svg", "." + comboBox6.Text.ToLower());
            string commandString = String.Format("\"{0}\" \"{1}\"", source, target);
            System.Diagnostics.Debug.WriteLine("uniconvertor " + commandString);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "uniconvertor";
            startInfo.Arguments = commandString;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            while (process.HasExited)
            {
                // Send file to com
                Connect();
                Send(target);
                Disconnect();
            }
            // TODO: how about fail to convert?
        }
        #endregion
    }

    /// <summary>
    /// EventArgs used to send bytes recieved on serial port
    /// </summary>
    public class SerialDataEventArgs : EventArgs
    {
        public SerialDataEventArgs(byte[] dataInByteArray)
        {
            Data = dataInByteArray;
        }

        /// <summary>
        /// Byte array containing data from serial port
        /// </summary>
        public byte[] Data;
    }
}
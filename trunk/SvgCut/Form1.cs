using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
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

        private void button2_Click(object sender, EventArgs e)
        {
            //open up file dialog
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".svg";
            dialog.Filter = "Scalable Vector Graphic (.svg)|*.svg";
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = dialog.FileName;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: Convert svg to plt
            // Send to plotter at com1
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BindBraudRates((sender as ComboBox).Text);
        }

        /// <summary>
        /// Initial layout.
        /// </summary>
        private void Initial()
        {
            // define available ports in this computer
            foreach (string port in SerialPort.GetPortNames())
                comboBox1.Items.Add(port);

            // add parity
            foreach (string parity in System.Enum.GetNames(typeof(System.IO.Ports.Parity)))
                comboBox4.Items.Add(parity);
        }
        private void BindBraudRates(string portName)
        {
            // bind braud rate
            this.port = new SerialPort(portName);

            try
            {
                this.port.Open();
                object p = this.port.BaseStream.GetType().GetField("commProp", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this.port.BaseStream);
                Int32 dwSettableBaud = (Int32)p.GetType().GetField("dwSettableBaud", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(p);
                int[] rates = UpdateBaudRateCollection(dwSettableBaud);

                comboBox2.Items.Clear();
                foreach (int rate in rates)
                    comboBox2.Items.Add(rate);

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
    }
}

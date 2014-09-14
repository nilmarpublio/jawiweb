using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SvgCut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // initial layout
            foreach (string port in SerialPort.GetPortNames())
                comboBox1.Items.Add(port);
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
    }
}

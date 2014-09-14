using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}

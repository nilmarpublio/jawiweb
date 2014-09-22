﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NisanWPF.BusinessLogic;

namespace NisanWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private nisan nisan;
        public MainWindow()
        {
            InitializeComponent();

            nisan.LoadFromFile("nisan.xml", out nisan);
            nisan.Initialize(nisan);

            // start bind nisan order
            this.DataContext = nisan;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            nisan.SaveToFile("nisan.xml");
        }
    }
}

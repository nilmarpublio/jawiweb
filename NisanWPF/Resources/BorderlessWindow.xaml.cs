using System;
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
using System.Windows.Shapes;

namespace NisanWPF
{
    /// <summary>
    /// Interaction logic for BorderlessWindow.xaml
    /// </summary>
    public partial class BorderlessWindow : ResourceDictionary
    {
        public BorderlessWindow()
        {
            InitializeComponent();
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2 && e.LeftButton == MouseButtonState.Pressed)
                MaximizeButton_Click(sender, e);
            else
            {
                Window window = (sender as FrameworkElement).TemplatedParent as Window;
                window.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = (sender as FrameworkElement).TemplatedParent as Window;
            window.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window win = (sender as FrameworkElement).TemplatedParent as Window;
            if (win.WindowState == WindowState.Maximized)
                win.WindowState = WindowState.Normal;
            else
                win.WindowState = WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = (sender as FrameworkElement).TemplatedParent as Window;
            window.Close();
        }
    }
}
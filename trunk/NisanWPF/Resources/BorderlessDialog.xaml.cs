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
    /// Interaction logic for BorderlessDialog.xaml
    /// </summary>
    public partial class BorderlessDialog : ResourceDictionary
    {
        public BorderlessDialog()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = (sender as FrameworkElement).TemplatedParent as Window;
            window.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using NisanWPF.BusinessLogic;

namespace NisanWPF
{
    /// <summary>
    /// Interaction logic for DoneDialog.xaml
    /// </summary>
    public partial class DoneDialog : Window
    {
        public DoneDialog()
        {
            InitializeComponent();
            DeliveryDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            nisan nisan = this.Owner.DataContext as nisan;
            ObservableCollection<nisanOrder> orders = this.DataContext as ObservableCollection<nisanOrder>;
            foreach (nisanOrder order in orders)
                nisan.MarkDone(order, Convert.ToDateTime(DeliveryDate.Text), RemarksTextBox.Text);
            this.Close();
        }
    }
}

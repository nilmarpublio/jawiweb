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
using HLGranite.Jawi;

namespace JawiWPF
{
    /// <summary>
    /// Interaction logic for CharacterMapViewer.xaml
    /// </summary>
    public partial class CharacterMapViewer : Window
    {
        private CharacterCollection characterManager;
        public CharacterMapViewer()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //todo: this is taking up resource, should run thread
            System.Diagnostics.Debug.WriteLine("Start binding character map at " + DateTime.Now);
            characterManager = new CharacterCollection("SimHei");//Arial
            this.DataContext = characterManager;
            //mapSpace.ItemsSource = characterManager.Items;
            //todo: suppose not here. System.Diagnostics.Debug.WriteLine("Done at " + DateTime.Now);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void fontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as CharacterCollection)
                .SetFontSize(
                (double)((sender as ComboBox).SelectedValue as ComboBoxItem).Content);
        }
    }
}
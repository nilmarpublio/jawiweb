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

            //todo: this is taking up resource, should run thread
            characterManager = new CharacterCollection("Arial");
            mapSpace.ItemsSource = characterManager.Items;
        }
    }
}
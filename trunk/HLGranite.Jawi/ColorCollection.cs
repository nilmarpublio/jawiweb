﻿using System.Windows.Media;
namespace HLGranite.Jawi
{
    public class ColorCollection : GraphicCollection
    {
        public ColorViewModel SelectedColor { get { return (ColorViewModel)this.selectedGraphic; } }
        public ColorCollection()
            : base()
        {
            Initialize();
        }
        private void Initialize()
        {
            this.items.Add(new ColorViewModel(Brushes.Black, "Black"));
            this.items.Add(new ColorViewModel(Brushes.Brown, "Brown"));
            this.items.Add(new ColorViewModel(Brushes.Green, "Green"));
            this.items.Add(new ColorViewModel(Brushes.Chartreuse, "Chartreuse"));
            this.items.Add(new ColorViewModel(Brushes.Cyan, "Cyan"));
            this.items.Add(new ColorViewModel(Brushes.Blue, "Blue"));
            this.items.Add(new ColorViewModel(Brushes.Purple, "Purple"));
            this.items.Add(new ColorViewModel(Brushes.Red, "Red"));
            this.items.Add(new ColorViewModel(Brushes.Orange, "Orange"));
            this.items.Add(new ColorViewModel(Brushes.Yellow, "Yellow"));
            //todo: this.items.Add(new ColorViewModel(Brushes.White, "White"));
        }
        /// <summary>
        /// Set selected graphic object and toggle off 
        /// </summary>
        /// <param name="item"></param>
        public void Select(ColorViewModel item)
        {
            base.Select(item);
        }
    }
}
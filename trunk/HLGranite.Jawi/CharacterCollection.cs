using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class CharacterCollection : INotifyPropertyChanged
    {
        #region Properties
        private string fontFamily;
        public string Font
        {
            get { return this.fontFamily; }
            set
            {
                this.fontFamily = value;
                RaisePropertyChanged("FontFamily");
                Initialize();
            }
        }
        protected ObservableCollection<CharacterViewModel> items;
        public ObservableCollection<CharacterViewModel> Items
        {
            get { return this.items; }
            set
            {
                this.items = value;
                RaisePropertyChanged("Items");
            }
        }
        #endregion

        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="fontFamily"></param>
        public CharacterCollection(string fontFamily)
        {
            this.fontFamily = fontFamily;
            Initialize();
        }
        private void Initialize()
        {
            this.items = new ObservableCollection<CharacterViewModel>();
            // 21 bit value is 55296
            for (int i = 0; i < 55296; i++) // Int16.MaxValue //65509
                this.items.Add(new CharacterViewModel(i.ToString("X"), Char.ConvertFromUtf32(i), 72));//set fontSize
        }
        public void SetFontSize(double size)
        {
            foreach (CharacterViewModel item in this.items)
                item.FontSize = size;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }
        #endregion
    }
}
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
        /// <summary>
        /// TODO: break this method by using thread and queue.
        /// </summary>
        private void Initialize()
        {
            this.items = new ObservableCollection<CharacterViewModel>();
            // 21 bit value is 55296
            for (int i = 30000; i < 34000; i++) // Int16.MaxValue //65509
                this.items.Add(new CharacterViewModel(UnicodeFormatter(i), Char.ConvertFromUtf32(i), 72));//set fontSize
        }
        public void SetFontSize(double size)
        {
            foreach (CharacterViewModel item in this.items)
                item.FontSize = size;
        }
        /// <summary>
        /// Format a decimal integer into hexadecimal value then display in unicode format. e.g. "U+042D".
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string UnicodeFormatter(int i)
        {
            string output = string.Empty;
            output = i.ToString("X");
            if (output.Length < 4) output = "0" + output;
            //actual = string.Format("{0:0000}", actual); it fail because it no longer is a valid number
            output = "U+" + output;

            return output;
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class CharacterCollection
    {
        private string fontFamily;
        public string FontFamily { get { return this.fontFamily; } }
        protected ObservableCollection<CharacterViewModel> items;
        public ObservableCollection<CharacterViewModel> Items { get { return this.items; } }

        public CharacterCollection(string fontFamily)
        {
            this.fontFamily = fontFamily;
            this.items = new ObservableCollection<CharacterViewModel>();
            // 21 bit value
            for (int i = 0; i < 55296; i++) // Int16.MaxValue //65509
            {
                this.items.Add(new CharacterViewModel(i.ToString("X"), Char.ConvertFromUtf32(i)));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class CharacterViewModel : INotifyPropertyChanged
    {
        private string code;
        /// <summary>
        /// Hexadecimal value for this character.
        /// </summary>
        public string Code { get { return this.code; } }
        private string character;
        /// <summary>
        /// Character value.
        /// </summary>
        public string Character { get { return this.character; } }

        public CharacterViewModel(string code, string character)
        {
            this.code = code;
            this.character = character;
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
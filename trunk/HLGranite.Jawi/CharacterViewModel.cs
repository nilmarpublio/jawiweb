using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class CharacterViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string code;
        /// <summary>
        /// Gets hexadecimal value for this character.
        /// </summary>
        public string Code
        {
            get { return this.code; }
            set
            {
                this.code = value;
                RaisePropertyChanged("Code");
            }
        }
        private string character;
        /// <summary>
        /// Gets character value.
        /// </summary>
        public string Character
        {
            get { return this.character; }
            set
            {
                this.character = value;
                RaisePropertyChanged("Character");
            }
        }
        private double fontSize;
        /// <summary>
        /// Gets font size for this character.
        /// </summary>
        /// <remarks>36, 72, or 120.</remarks>
        public double FontSize
        {
            get { return this.fontSize; }
            set
            {
                this.fontSize = value;
                RaisePropertyChanged("FontSize");
            }
        }
        #endregion

        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="character"></param>
        /// <param name="fontSize"></param>
        public CharacterViewModel(string code, string character, double fontSize)
        {
            this.code = code;
            this.character = character;
            this.fontSize = fontSize;
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
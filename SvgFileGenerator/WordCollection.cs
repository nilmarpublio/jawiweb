using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Word collection logic.
    /// </summary>
    public class WordCollection : PathCollection
    {
        private ObservableCollection<PathViewModel> selectedWords;
        /// <summary>
        /// Gets selected children in word collection which ready to save to file.
        /// </summary>
        public ObservableCollection<PathViewModel> SelectedWords { get { return this.selectedWords; } }
        /// <summary>
        /// Word space collection view model.
        /// </summary>
        public WordCollection()
            : base("words")
        {
            this.selectedWords = new ObservableCollection<PathViewModel>();
        }
        /// <summary>
        /// Search contains this keyword otherwise false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new int Contains(string name)
        {
            return base.Contains(name);
        }
        /// <summary>
        /// Search contains this keyword and highlight unmatch as red.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int Contains(string name, out List<KeyValuePair<string, bool>> keys)
        {
            keys = new List<KeyValuePair<string, bool>>();
            name = name.ToLower();
            name = name.TrimEnd(new char[] { '\n', '\r' });//hack: '\n', '\r'
            string[] names = name.Split(new char[] { ' ' });
            foreach (string s in names)
            {
                if (s.Trim().Length == 0) continue;

                if (this.Items.Where(f => f.Name.Contains(s)).Count() > 0)
                    keys.Add(new KeyValuePair<string, bool>(s, true));
                else
                    keys.Add(new KeyValuePair<string, bool>(s, false));
            }

            return base.Contains(name);
        }
        public void Select(PathViewModel path)
        {
            //base.Select(path);//just done by radiobutton behavior.
            this.selectedWords.Add(path);
        }
        public void Delete(PathViewModel path)
        {
            base.Delete(path);
            this.selectedWords.Remove(path);
        }
        private string TrimHiddenCharacters(string source)
        {
            string output = source;
            char[] hiddens = new char[] { '\n', '\r', '\t' };
            foreach (char hidden in hiddens)
                //output = output.TrimStart(new char[] { hidden}).TrimEnd(new char[]{hidden});
                output = output.Replace(hidden, '\0');
            return output;
        }
    }
}
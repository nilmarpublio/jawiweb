using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HLGranite.Jawi
{
    public class WordCollection : PathCollection
    {
        /// <summary>
        /// Word space collection view model.
        /// </summary>
        public WordCollection()
            : base("words")
        {
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
        public int Contains(string name, out List<string, bool> keys)
        {
            //if (sender is RichTextBox)
            //{
            //RichTextBox textBox = (RichTextBox)sender;
            //textBox.Document.Blocks.Clear();

            //todo: consider this scenario 'hj n.m. mohd ismail bin mohd ghani'
            keys = new Dictionary<string, bool>();
            name = name.ToLower();
            name = name.TrimEnd(new char[] { '\n', '\r' });//hack: '\n', '\r'
            //name = TrimHiddenCharacters(name);
            string[] names = name.Split(new char[] { ' ' });
            foreach (string s in names)
            {
                if (s.Trim().Length == 0) continue;

                if (this.Items.Where(f => f.Name.Contains(s)).Count() > 0)
                    keys.Add(s, true);
                else
                    keys.Add(s, false);
            }

            return base.Contains(name);
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
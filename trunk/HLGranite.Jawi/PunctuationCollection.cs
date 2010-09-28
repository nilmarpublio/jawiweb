using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class PunctuationCollection : PathCollection
    {
        /// <summary>
        /// Punctuation collection view model.
        /// </summary>
        public PunctuationCollection()
            : base("khots")
        {
        }
        /// <summary>
        /// Return number match exactly otherwise 0.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Match(string name)
        {
            return base.MatchExactly(name);
        }
    }
}
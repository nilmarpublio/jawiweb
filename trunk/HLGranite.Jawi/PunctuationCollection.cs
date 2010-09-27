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
        /// <param name="source"></param>
        public PunctuationCollection()
            : base("khots")
        {
        }
        /// <summary>
        /// True if match exactly otherwise false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Match(string name)
        {
            return base.MatchExactly(name);
        }
    }
}
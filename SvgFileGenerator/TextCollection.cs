using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Character collection class.
    /// </summary>
    public class TextCollection : PathCollection
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextCollection()
            : base("characters")
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
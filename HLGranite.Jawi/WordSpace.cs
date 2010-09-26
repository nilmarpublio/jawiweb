﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class WordSpace : Workspace
    {
        /// <summary>
        /// Word space collection view model.
        /// </summary>
        /// <param name="source"></param>
        public WordSpace()
            : base("words")
        {
        }
        /// <summary>
        /// Search contains this keyword otherwise false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return base.Contains(name);
        }
    }
}
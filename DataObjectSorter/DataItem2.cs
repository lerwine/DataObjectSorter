using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataObjectSorter
{
    [Serializable]
    public class DataItem2
    {
        [DataObjectField(false, false, true)]
        public string Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DataObjectSorter
{
    public class DataItemSorter<DataItemType>
    {
        private DataObjectFieldInfo[] _fields;
        public DataItemSorter()
        {
            Type dataItemType = typeof(DataItemType);
            this._fields = dataItemType.GetProperties().Select(p => DataObjectFieldInfo.Create(p)).Where(f => f != null).ToArray();
        }
    }
}

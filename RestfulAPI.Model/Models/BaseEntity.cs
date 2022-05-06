using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulAPI.Model.Models
{
    public class BaseEntity
    {
        public override string ToString()
        {
            PropertyDescriptorCollection coll = TypeDescriptor.GetProperties(this);
            StringBuilder builder = new StringBuilder();
            foreach (PropertyDescriptor pd in coll)
            {
                builder.Append(string.Format("{0} : {1}; ", pd.Name, pd.GetValue(this) == null ? "" : pd.GetValue(this).ToString()));
            }

            return builder.ToString();
        }
    }
}

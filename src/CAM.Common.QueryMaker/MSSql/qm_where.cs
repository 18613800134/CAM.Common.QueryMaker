using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Common.QueryMaker.MSSql
{
    public class qm_where : IQueryMakerObjectWhere
    {


        public string format(QMObjectWhere obj)
        {
            StringBuilder sbStr = new StringBuilder("");
            sbStr.AppendFormat("{0} ", obj.Condition);
            return sbStr.ToString();
        }
    }
}

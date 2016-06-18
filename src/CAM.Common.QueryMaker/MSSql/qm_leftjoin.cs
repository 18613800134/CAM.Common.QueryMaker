using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Common.QueryMaker.MSSql
{
    public class qm_leftjoin : IQueryMakerObjectLeftJoin
    {


        public string format(QMObjectLeftJoin obj)
        {
            StringBuilder sbStr = new StringBuilder("");
            sbStr.AppendFormat("left join [{0}] as [{1}] on {2} ", obj.Entity.Name, obj.Alias, obj.Link);
            return sbStr.ToString();
        }
    }
}

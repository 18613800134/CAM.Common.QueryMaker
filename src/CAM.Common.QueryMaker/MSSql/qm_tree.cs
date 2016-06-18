using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Common.QueryMaker.MSSql
{
    public class qm_treebegin : IQueryMakerObjectTreeBegin
    {

        public string format(QMObjectTreeBegin obj)
        {
            StringBuilder sbStr = new StringBuilder("");

            sbStr.AppendFormat("declare @idlist varchar(max); ");
            sbStr.AppendFormat("set @idlist = ''; ");
            sbStr.AppendFormat("with cte as( ");
            sbStr.AppendFormat("select Id, Tree_ParentId ");
            sbStr.AppendFormat("from {0} where Tree_ParentId={1} ", obj.Entity.Name, obj.NodeId);
            sbStr.AppendFormat("union all ");
            sbStr.AppendFormat("select b.Id, b.Tree_ParentId ");
            sbStr.AppendFormat("from cte a ");
            sbStr.AppendFormat("inner join {0} b on a.Id=b.Tree_ParentId ", obj.Entity.Name);
            sbStr.AppendFormat(") select @idlist = @idlist + CAST(Id as varchar(10)) + ',' from cte; ");
            sbStr.AppendFormat("if @idlist<>'' set @idlist = LEFT(@idlist, LEN(@idlist)-1); ");
            sbStr.AppendFormat("declare @loopsql varchar(max); ");
            sbStr.AppendFormat("set @loopsql = ' ");
            sbStr.AppendFormat("");
            sbStr.AppendFormat("");

            return sbStr.ToString();
        }
    }

    public class qm_treeend : IQueryMakerObjectTreeEnd
    {

        public string format(QMObjectTreeEnd obj)
        {
            StringBuilder sbStr = new StringBuilder("");

            sbStr.AppendFormat(" ';");
            sbStr.AppendFormat("set @loopsql = @loopsql + 'where {0}.Id in('+@idlist+')' ", obj.Alias);
            sbStr.AppendFormat("set @loopsql = @loopsql + 'order by {0}.Tree_ParentId asc, {0}.Order_Index asc; ' ", obj.Alias);
            sbStr.AppendFormat("exec(@loopsql) ");
            sbStr.AppendFormat("");

            return sbStr.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Common.QueryMaker
{
    public interface IQueryMakerObjectSelect
    {
        string format(QMObjectSelect obj, List<_QMObjectBasic> ViewModeObjects);
    }
    public interface IQueryMakerObjectLeftJoin
    {
        string format(QMObjectLeftJoin obj);
    }
    public interface IQueryMakerObjectWhere
    {
        string format(QMObjectWhere obj);
    }
    public interface IQueryMakerObjectTreeBegin
    {
        string format(QMObjectTreeBegin obj);
    }
    public interface IQueryMakerObjectTreeEnd
    {
        string format(QMObjectTreeEnd obj);
    }
}

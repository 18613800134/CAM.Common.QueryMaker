using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Common.QueryMaker
{
    /// <summary>
    /// QueryMakerObject的基类，由此可以派生出QM的所有行为对象，例如select,leftjoin,where等
    /// </summary>
    public abstract class _QueryMakerObject
    {
    }

    public abstract class _QMObjectBasic : _QueryMakerObject
    {
        public Type Entity { get; set; }
        public Type ViewMode { get; set; }
        public string Alias { get; set; }
    }
    public class QMObjectSelect : _QMObjectBasic
    {

    }
    public class QMObjectLeftJoin : QMObjectSelect
    {
        public string Link { get; set; }
    }
    public class QMObjectWhere : _QueryMakerObject
    {
        public string Condition { get; set; }
    }
    public class QMObjectTreeBegin : _QueryMakerObject
    {
        public Type Entity { get; set; }
        public long NodeId { get; set; }
    }
    public class QMObjectTreeEnd : _QueryMakerObject
    {
        public string Alias { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CAM.Common.QueryMaker
{
    public class qmHelper
    {
        public static object readMixin(object ic, Type ViewMode, QueryMakerObjectQueue qm)
        {
            MethodInfo mi = ic.GetType().GetMethod("readMixin").MakeGenericMethod(ViewMode);
            object obj = mi.Invoke(ic, new object[] { qm });
            return obj;
        }

        public static object readMixinList(object ic, Type ViewMode, QueryMakerObjectQueue qm, Type TypeOfFilter, object filter)
        {
            MethodInfo mi = ic.GetType().GetMethod("readMixinList").MakeGenericMethod(ViewMode, TypeOfFilter);
            object obj = mi.Invoke(ic, new object[] { qm, filter });
            return obj;
        }

        public static object readMixinTree(object ic, Type ViewMode, QueryMakerObjectQueue qm)
        {
            MethodInfo mi = ic.GetType().GetMethod("readMixinTree").MakeGenericMethod(ViewMode);
            object obj = mi.Invoke(ic, new object[] { qm });
            return obj;
        }
    }
}

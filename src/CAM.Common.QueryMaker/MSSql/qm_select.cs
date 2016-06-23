using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CAM.Common.QueryMaker.MSSql
{
    public class qm_select : IQueryMakerObjectSelect
    {

        public string format(QMObjectSelect obj, List<_QMObjectBasic> ViewModeObjects)
        {

            StringBuilder sbStr = new StringBuilder("");
            sbStr.AppendFormat("select ");

            List<string> viewItems = new List<string>();
            foreach (_QMObjectBasic viewMode in ViewModeObjects)
            {
                string viewModeName = viewMode.ViewMode.Name;
                foreach (PropertyInfo pi in viewMode.ViewMode.GetProperties())
                {
                    
                    //Console.WriteLine("pi.name={0}, pi.type={1}", pi.Name, pi.PropertyType.Name);
                    //viewItems.Add(string.Format("[{0}].[{1}] as [{2}_{1}] ", viewMode.Alias, pi.Name, viewModeName));
                    string itemName = formatNullItemDefaultValue(string.Format("[{0}].[{1}]", viewMode.Alias, pi.Name), pi.PropertyType);
                    viewItems.Add(string.Format("{0} as [{2}_{1}] ", itemName, pi.Name, viewModeName));
                }
            }
            sbStr.AppendFormat("{0} ", string.Join(",", viewItems));

            sbStr.AppendFormat("from [{0}] as [{1}] ", obj.Entity.Name, obj.Alias);

            return sbStr.ToString();
        }

        private string formatNullItemDefaultValue(string piName, Type piType)
        {
            string result = "";
            switch (piType.Name.Trim().ToLower())
            {
                case "int64":
                case "int32":
                    result = string.Format(" isnull({0}, 0) ", piName);
                    break;
                case "boolean":
                    result = string.Format(" isnull({0}, 1) ", piName);
                    break;
                case "string":
                    result = string.Format(" isnull({0}, '') ", piName);
                    break;
                case "datetime":
                    result = string.Format(" isnull({0}, getdate()) ", piName);
                    break;
            }
            return result;
        }
    }
}

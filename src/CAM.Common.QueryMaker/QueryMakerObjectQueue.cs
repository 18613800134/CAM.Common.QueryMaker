using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CAM.Common.Error;

namespace CAM.Common.QueryMaker
{
    public enum QueryMakerType
    {
        QMTMSSql,
    }



    public class QueryMakerObjectQueue
    {
        public List<_QueryMakerObject> Objects { get; set; }
        public QueryMakerType MakerType { get; set; }
        private List<_QMObjectBasic> ViewModeObjects;

        public QueryMakerObjectQueue() : this(QueryMakerType.QMTMSSql) { }

        public QueryMakerObjectQueue(QueryMakerType makerType)
        {
            MakerType = makerType;
            Objects = new List<_QueryMakerObject>();
            ViewModeObjects = new List<_QMObjectBasic>();
        }



        #region Push方法集合

        public void Push(QMObjectSelect queryObj)
        {
            ViewModeObjects.Add(queryObj);
            Objects.Add(queryObj);
        }
        public void Push(QMObjectLeftJoin queryObj)
        {
            ViewModeObjects.Add(queryObj);
            Objects.Add(queryObj);
        }
        public void Push(QMObjectWhere queryObj)
        {
            Objects.Add(queryObj);
        }
        public void Push(QMObjectTreeBegin queryObj)
        {
            Objects.Add(queryObj);
        }
        public void Push(QMObjectTreeEnd queryObj)
        {
            Objects.Add(queryObj);
        }

        #endregion



        public override string ToString()
        {
            return formatQueryString();
        }



        private string formatQueryString()
        {
            StringBuilder sbQuery = new StringBuilder("");

            bool isTreeMode = false;    //如果是Tree模式的循环读取，不能设置where条件

            foreach (object item in Objects)
            {
                if (item.GetType() == typeof(QMObjectSelect))
                {
                    sbQuery.Append(makeSelectFormatter().format((QMObjectSelect)item, ViewModeObjects));
                    continue;
                }
                if (item.GetType() == typeof(QMObjectLeftJoin))
                {
                    sbQuery.Append(makeLeftJoinFormatter().format((QMObjectLeftJoin)item));
                    continue;
                }
                if (item.GetType() == typeof(QMObjectWhere))
                {
                    if (isTreeMode)
                    {
                        ErrorHandler.ThrowException("Tree模型的遍历读取不能设置Where条件！");
                    }
                    sbQuery.Append(makeWhereFormatter().format((QMObjectWhere)item));
                    continue;
                }
                if (item.GetType() == typeof(QMObjectTreeBegin))
                {
                    isTreeMode = true;
                    sbQuery.Append(makeTreeBeginFormatter().format((QMObjectTreeBegin)item));
                    continue;
                }
                if (item.GetType() == typeof(QMObjectTreeEnd))
                {
                    sbQuery.Append(makeTreeEndFormatter().format((QMObjectTreeEnd)item));
                    continue;
                }
            }

            return sbQuery.ToString();
        }


        #region 创建符合MakerType的语法接口

        private IQueryMakerObjectSelect makeSelectFormatter()
        {
            IQueryMakerObjectSelect ic = null;
            switch (MakerType)
            {
                case QueryMakerType.QMTMSSql:
                    ic = new MSSql.qm_select();
                    break;
            }
            return ic;
        }

        private IQueryMakerObjectLeftJoin makeLeftJoinFormatter()
        {
            IQueryMakerObjectLeftJoin ic = null;
            switch (MakerType)
            {
                case QueryMakerType.QMTMSSql:
                    ic = new MSSql.qm_leftjoin();
                    break;
            }
            return ic;
        }

        private IQueryMakerObjectWhere makeWhereFormatter()
        {
            IQueryMakerObjectWhere ic = null;
            switch (MakerType)
            {
                case QueryMakerType.QMTMSSql:
                    ic = new MSSql.qm_where();
                    break;
            }
            return ic;
        }

        private IQueryMakerObjectTreeBegin makeTreeBeginFormatter()
        {
            IQueryMakerObjectTreeBegin ic = null;
            switch (MakerType)
            {
                case QueryMakerType.QMTMSSql:
                    ic = new MSSql.qm_treebegin();
                    break;
            }
            return ic;
        }

        private IQueryMakerObjectTreeEnd makeTreeEndFormatter()
        {
            IQueryMakerObjectTreeEnd ic = null;
            switch (MakerType)
            {
                case QueryMakerType.QMTMSSql:
                    ic = new MSSql.qm_treeend();
                    break;
            }
            return ic;
        }

        #endregion

    }




    
    #region QueryMakerObjectQueue语法糖
    /*
     * 为QueryMaker提供链式编程模式的支持
     */

    public static class QueryMakerObjectQueueContent
    {
        public static QueryMakerObjectQueue select(this QueryMakerObjectQueue Self, Type TEntity, Type TViewMode, string Alias)
        {
            QMObjectSelect obj_s = new QMObjectSelect()
            {
                Entity = TEntity,
                ViewMode = TViewMode,
                Alias = Alias,
            };
            Self.Push(obj_s);
            return Self;
        }

        public static QueryMakerObjectQueue leftjoin(this QueryMakerObjectQueue Self, Type TEntity, Type TViewMode, string Alias, string Link)
        {
            QMObjectLeftJoin obj_left = new QMObjectLeftJoin()
            {
                Entity = TEntity,
                ViewMode = TViewMode,
                Alias = Alias,
                Link = Link,
            };
            Self.Push(obj_left);
            return Self;
        }

        public static QueryMakerObjectQueue where(this QueryMakerObjectQueue Self, string Condition)
        {
            QMObjectWhere obj_w = new QMObjectWhere()
            {
                Condition = Condition,
            };
            Self.Push(obj_w);
            return Self;
        }

        public static QueryMakerObjectQueue treeBegin(this QueryMakerObjectQueue Self, Type TTreeMainEntity, long StartParentNodeId = 0)
        {
            QMObjectTreeBegin obj_tb = new QMObjectTreeBegin()
            {
                Entity = TTreeMainEntity,
                NodeId = StartParentNodeId,
            };
            Self.Push(obj_tb);
            return Self;
        }

        public static QueryMakerObjectQueue treeEnd(this QueryMakerObjectQueue Self, string TreeMainAlias)
        {
            QMObjectTreeEnd obj_te = new QMObjectTreeEnd()
            {
                Alias = TreeMainAlias,
            };
            Self.Push(obj_te);
            return Self;
        }

    }

    #endregion

}

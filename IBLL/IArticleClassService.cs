/** 
* IArticleClassService.cs
*
* 功 能： 表ArticleClass业务层接口
* 类 名： IArticleClass
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/21 23:24:54   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------ArticleClass开始----------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;
using IDAL;
using DALMySql;

namespace IBLL
{
	public partial interface IArticleClassService : IBaseService<ArticleClass>
    {
        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		ArticleClass GetModelBy(Expression<Func<ArticleClass, bool>> queryWhere);

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		Task<ArticleClass> GetModelByAsync(Expression<Func<ArticleClass, bool>> queryWhere);

		#endregion 
    }
}

//----------ArticleClass结束----------

    
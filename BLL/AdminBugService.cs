/** 
* AdminBugService.cs
*
* 功 能： 表AdminBug业务层
* 类 名： AdminBug
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/1/24 19:04:08   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminBug开始----------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;
using IDAL;
using DALMySql;
using IBLL;
using RepositoryPattern;

namespace BLL
{
	public partial class AdminBugService : BaseService<AdminBug>, IAdminBugService 
    {
        //EF上下文
        protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IAdminBugDAL MyIAdminBugDAL;

        #region 构造函数

		public AdminBugService(CdyhcdDBContext db, IAdminBugDAL adminBugDAl) : base(adminBugDAl)
		{
            MyDBContext = db;
            MyIAdminBugDAL = adminBugDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public AdminBug GetModelBy(Expression<Func<AdminBug, bool>> queryWhere)
		{
			AdminBug result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<AdminBug> GetModelByAsync(Expression<Func<AdminBug, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            AdminBug result = modelList.FirstOrDefault();
            return result;
        }

		public override void Dispose()
        {
            MyIAdminBugDAL = null;
            MyDBContext.Dispose();
            base.Dispose();
        }

		#endregion 
    }
}

//----------AdminBug结束----------

    
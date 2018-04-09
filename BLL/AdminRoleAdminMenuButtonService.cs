/** 
* AdminRoleAdminMenuButtonService.cs
*
* 功 能： 表AdminRoleAdminMenuButton业务层
* 类 名： AdminRoleAdminMenuButton
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/9 23:03:57   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminRoleAdminMenuButton开始----------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;
using IDAL;
using DALMySql;

namespace BLL
{
	public partial class AdminRoleAdminMenuButtonService : BaseService<AdminRoleAdminMenuButton>
    {
        //EF上下文
        protected readonly CdyhcdDBContext _db;
        //操作DAL
        protected IAdminRoleAdminMenuButtonDAL adminRoleAdminMenuButtonDAL;

        #region 构造函数

		public AdminRoleAdminMenuButtonService(CdyhcdDBContext db)
		{
            _db = db;
			SetIBaseDal();
		}

		#endregion
		
        #region 重写IBaseDal获取方法

		public sealed override void SetIBaseDal()
        {
            IBaseDal = adminRoleAdminMenuButtonDAL = new DALSession(_db).IAdminRoleAdminMenuButtonDAL;
        }

		#endregion

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public AdminRoleAdminMenuButton GetModelBy(Expression<Func<AdminRoleAdminMenuButton, bool>> queryWhere)
		{
			AdminRoleAdminMenuButton result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<AdminRoleAdminMenuButton> GetModelByAsync(Expression<Func<AdminRoleAdminMenuButton, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            AdminRoleAdminMenuButton result = modelList.FirstOrDefault();
            return result;
        }

		#endregion 
    }
}

//----------AdminRoleAdminMenuButton结束----------

    
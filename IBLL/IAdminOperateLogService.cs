/** 
* IAdminOperateLogService.cs
*
* 功 能： 表AdminOperateLog业务层接口
* 类 名： IAdminOperateLog
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
//----------AdminOperateLog开始----------

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
	public partial interface IAdminOperateLogService : IBaseService<AdminOperateLog>
    {
        /// <summary>
        /// 获取后台list数据
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="totalCount">总页数</param>
        /// <param name="isdesc">升降序</param>
        /// <returns></returns>
        List<AdminOperateLog> GetListForOperateLogAdmin<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, out int totalCount, bool isdesc = false);

        /// <summary>
        /// 异步获取后台list数据
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="totalCount">总页数</param>
        /// <param name="isdesc">升降序</param>
        /// <returns></returns>
        Task<PageData<AdminOperateLog>> GetListForOperateLogAdminAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, bool isdesc = false);
    }
}

//----------AdminOperateLog结束----------

    
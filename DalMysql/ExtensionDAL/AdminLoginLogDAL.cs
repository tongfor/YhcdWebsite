﻿/** 
* AdminLoginLogDAL.cs
*
* 功 能： AdminLoginLog数据层扩展实现
* 类 名： AdminLoginLogDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/20 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DALMySql
{
    /// <summary>
    /// 账号登录日志DAL
    /// </summary>
    public partial class AdminLoginLogDAL
    {
        #region 判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录

        /// <summary>
        /// 判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录
        /// </summary>
        /// <param name="maxErrorCount">最大错误次数,如果小于等于0则不判断</param>
        /// <param name="tryMinutes">多少分钟后可重新登录</param>
        /// <param name="ip">用户ip</param>
        /// <param name="lastLoginTime">输出参数：如果30分钟没有5次的失败登录，那么返回null；如果有，就返回下一次可以登录的时间</param>
        public bool CheckLoginErrorCount(int maxErrorCount, int tryMinutes, string ip, out DateTime? lastLoginTime)
        {
            lastLoginTime = null;
            bool bResult = false;
            if (maxErrorCount <= 0)
            {
                return bResult;
            }
            int errorLoginCount = 0;
            DateTime compareDateTime = DateTime.Now.AddMinutes(-tryMinutes);
            errorLoginCount =
                _db.Set<AdminLoginLog>().Count(f => f.UserIp == ip && (f.IsSuccess == null || f.IsSuccess.Value == 0) && compareDateTime < f.LoginTime.Value);
            //.Count(f =>f.UserIp == ip && f.IsSuccess.Value == false &&(DateTime.Now - f.LoginTime.Value).TotalDays < (double)tyrMinutes);
            //.Count(f => f.UserIp == ip && f.IsSuccess == false && EntityFunctions.DiffMinutes(DateTime.Now , f.LoginTime) < tyrMinutes);
            if (errorLoginCount >= maxErrorCount)
            {
                lastLoginTime =
                    _db.Set<AdminLoginLog>().Where(f => f.UserIp == ip && (f.IsSuccess == null || f.IsSuccess.Value == 0)).Max(p => p.LoginTime);
                bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// 异步判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录
        /// </summary>
        /// <param name="maxErrorCount">最大错误次数,如果小于等于0则不判断</param>
        /// <param name="tryMinutes">多少分钟后可重新登录</param>
        /// <param name="ip">用户ip</param>
        /// <returns>元组，</returns>
        public async Task<Tuple<bool, DateTime>> CheckLoginErrorCountAsync(int maxErrorCount, int tryMinutes, string ip)
        {
            DateTime lastLoginTime = DateTime.Now;
            bool bResult = false;
            if (maxErrorCount <= 0)
            {
                return new Tuple<bool, DateTime>(bResult, DateTime.Now);
            }
            int errorLoginCount = 0;
            DateTime compareDateTime = DateTime.Now.AddMinutes(-tryMinutes);
            errorLoginCount = await
                _db.Set<AdminLoginLog>().CountAsync(f => f.UserIp == ip && (f.IsSuccess == null || f.IsSuccess.Value == 0) && compareDateTime < f.LoginTime.Value);
            if (errorLoginCount >= maxErrorCount)
            {
                var logList = await _db.Set<AdminLoginLog>()
                    .Where(f => f.UserIp == ip && (f.IsSuccess == null || f.IsSuccess.Value == 0)).ToListAsync();
                lastLoginTime = logList.Max(p => p.LoginTime.Value);
                bResult = true;
            }
            return new Tuple<bool, DateTime>(bResult, lastLoginTime);
        }

        #endregion

        #region 获取后台list数据

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
        public List<AdminLoginLog> GetListForLoginLogAdmin<TKey>(int pageIndex, int pageSize, Expression<Func<AdminLoginLog, bool>> queryWhere, Expression<Func<AdminLoginLog, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            List<AdminLoginLog> adminLoginLogList = new List<AdminLoginLog>();
            if (isdesc)
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminLoginLog>().Where(queryWhere).OrderByDescending(orderBy);
                    totalCount = dbQuery.Count();
                    adminLoginLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }
                else
                {
                    var dbQuery = _db.Set<AdminLoginLog>().OrderByDescending(orderBy);
                    totalCount = dbQuery.Count();
                    adminLoginLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }
            }
            else
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminLoginLog>().Where(queryWhere).OrderBy(orderBy);
                    totalCount = dbQuery.Count();
                    adminLoginLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }
                else
                {
                    var dbQuery = _db.Set<AdminLoginLog>().OrderBy(orderBy);
                    totalCount = dbQuery.Count();
                    adminLoginLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }

            }
            return adminLoginLogList;
        }

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
        public async Task<PageData<AdminLoginLog>> GetListForLoginLogAdminAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminLoginLog, bool>> queryWhere, Expression<Func<AdminLoginLog, TKey>> orderBy, bool isdesc = false)
        {
            PageData<AdminLoginLog> result = new PageData<AdminLoginLog>();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            if (isdesc)
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminLoginLog>().Where(queryWhere).OrderByDescending(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
                else
                {
                    var dbQuery = _db.Set<AdminLoginLog>().OrderByDescending(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminLoginLog>().Where(queryWhere).OrderBy(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
                else
                {
                    var dbQuery = _db.Set<AdminLoginLog>().OrderBy(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
            }
            return result;
        }

        #endregion
    }
}

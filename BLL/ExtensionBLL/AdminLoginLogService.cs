﻿/** 
* AdminLoginLogService.cs
*
* 功 能： AdminLoginLog逻辑层
* 类 名： AdminLoginLogService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/20 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALMySql;
using IDAL;
using Models;

namespace BLL
{
    public partial class AdminLoginLogService
    {
        #region 判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录

        /// <summary>
        /// 判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录
        /// </summary>
        /// <param name="maxErrorCount">最大错误次数,如果小于等于0则不判断</param>
        /// <param name="tyrMinutes">多少分钟后可重新登录</param>
        /// <param name="ip">用户ip</param>
        /// <param name="lastLoginTime">输出参数：如果30分钟没有5次的失败登录，那么返回null；如果有，就返回下一次可以登录的时间</param>
        public bool CheckLoginErrorCount(int maxErrorCount, int tyrMinutes, string ip, out DateTime? lastLoginTime)
        {
            return adminLoginLogDAL.CheckLoginErrorCount(maxErrorCount, tyrMinutes, ip, out lastLoginTime);
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
            Tuple<bool, DateTime> result = await adminLoginLogDAL.CheckLoginErrorCountAsync(maxErrorCount, tryMinutes, ip);
            return result;
        }

        #endregion
    }
}
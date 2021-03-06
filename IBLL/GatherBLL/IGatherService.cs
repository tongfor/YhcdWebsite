﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Config;
using Models;

namespace IBLL
{
    public interface IGatherService
    {
        /// <summary>
        /// 采集站点文章
        /// </summary>
        /// <param name="siteKey">站点关键字</param>
        /// <param name="pageStartNo">文章列表启始页</param>
        /// <param name="pageEndNo">文章列表结束页</param>
        /// <param name="classId">文章类别</param>
        /// <param name="userName">采集员账号</param>
        /// <returns></returns>
        Task<GatherResult> GatherWebsiteAsync(string siteKey, int? pageStartNo, int? pageEndNo, int classId, string userName);

        ///// <summary>
        ///// 获取要采集的文章列表
        ///// </summary>
        ///// <param name="siteConfig">本站设置</param>
        ///// <param name="pageStartNo">文章列表启始页</param>
        ///// <param name="pageEndNo">文章列表结束页</param>
        ///// <param name="classId">文章类别</param>
        ///// <param name="gatherWebsite">站点采集设置</param>
        ///// <param name="userName">采集员账号</param>
        ///// <returns></returns>
        //Task<List<Article>> GetGatherArticleListAsync(GatherWebsite gatherWebsite, int? pageStartNo, int? pageEndNo, int classId, string userName);
        ///// <summary>
        ///// 采集得到文章模型
        ///// </summary>
        ///// <param name="siteConfig">本站设置</param>
        ///// <param name="gatherWebsite">站点采集设置</param>
        ///// <param name="url"></param>
        ///// <param name="classId">文章类别</param>
        ///// <param name="userName">采集员账号</param>
        ///// <returns></returns>
        //Task<Article> GetArticleDetailsAsync(GatherWebsite gatherWebsite, string url, int? classId, string userName, Func<AngleSharp.Dom.Html.IHtmlDocument, Article, Article> callback);

        /// <summary>
        /// 采集所有已配网站数据
        /// </summary>
        /// <returns></returns>
        Task<List<GatherResult>> GatherAllWebsites(int? pageStartNo, int? pageEndNo, int classId, string username);

        /// <summary>
        /// 得到一键采集的采集结果，也用于保持心跳线连接
        /// </summary>
        /// <returns></returns>
        List<GatherResult> GetGatherAllResult();
    }
}
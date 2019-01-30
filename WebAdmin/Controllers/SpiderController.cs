﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Parser.Html;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;

namespace WebAdmin.Controllers
{
    public class SpiderController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleClassService _articleClassService;
        private readonly IGatherService _gatherService;
        private List<GatherResult> _gatherResultList = new List<GatherResult>();

        public SpiderController(IArticleService articleService, IArticleClassService articleClassService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IGatherService gatherService, ILogger<ArticleController> logger, IOptionsSnapshot<SiteConfig> options, IOptionsSnapshot<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
        {
            _articleService = articleService;
            _articleClassService = articleClassService;
            _gatherService = gatherService;
            _logger = logger;
        }

        // GET: Spider
        public async Task<IActionResult> Index()
        {
            await CreateLeftMenuAsync();
            return View();
        }

        /// <summary>
        /// 采集网站数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherWebsite(string siteKey, int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                GatherResult gatherResult = await _gatherService.GatherWebsiteAsync(siteKey, pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleList.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                _logger.LogError($"采集{siteKey}列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集所有已配网站数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherAllWebsites(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var resultList = new List<GatherResult>();
                //默认采集前3页数据
                pageStartNo = pageStartNo == null || pageStartNo == 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo == 0 ? 3 : pageEndNo;
                if (GatherSettings!=null &&GatherSettings.GatherWebsiteList!=null)
                {
                    GatherResult gatherResult = new GatherResult();
                    foreach (var site in GatherSettings.GatherWebsiteList)
                    {
                        if (string.IsNullOrEmpty(site.Key) || string.IsNullOrEmpty(site.Name) || string.IsNullOrEmpty(site.SiteUrl) || string.IsNullOrEmpty(site.UrlTemp))
                        {
                            continue;
                        }
                        try
                        {
                            gatherResult = await _gatherService.GatherWebsiteAsync(site.Key, pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                            _gatherResultList.Add(gatherResult);
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError(ex, $"一键采集中采集{site.Name}数据时报错！");
                            continue;
                        }
                    }
                }

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, resultList.Count > 0 ? $"采集成功！采集了{resultList.Count}网站的数据！" : "暂无新增数据!", resultList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                _logger.LogError($"一键采集报错！", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, $"一键采集报错！");
            }
        }

        /// <summary>
        /// 得到一键采集的采集结果，也用于保持心跳线连接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<GatherResult> GetGatherAllResult()
        {
                return _gatherResultList;
        }
    }
}
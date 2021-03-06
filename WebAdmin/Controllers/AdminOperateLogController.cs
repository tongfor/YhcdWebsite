﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class AdminOperateLogController : BaseController
    {
        public AdminOperateLogController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptionsSnapshot<SiteConfig> options, IOptionsSnapshot<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
        {
        }

        //GET: AdminOperateLog
        public async Task<IActionResult> Index(BaseRequest request)
        {
            try
            {
                ViewBag.PageIndex = request.PageIndex;
                IEnumerable<AdminOperateLog> adminOperateLogList = await MyOperateLogService.GetOperateLogForAdminListAsync(request);
                await CreateLeftMenuAsync();

                return View(adminOperateLogList as PagedList<AdminOperateLog>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "操作日志列表错误" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 导出操作日志到Excel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> OperateLogToXls(BaseRequest request)
        {
            try
            {
                ViewBag.page = request.PageIndex;//第几页
                ViewBag.Index = (request.PageIndex - 1) * request.PageSize + 1;//第几条
                ViewBag.toIndex = request.PageIndex * request.PageSize;//到多少条
                IEnumerable<AdminOperateLog> adminOperateLogList = await MyOperateLogService.GetOperateLogForAdminListAsync(request);

                var filename = $"操作日志" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = false  // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.ContentType = "application/vnd.ms-excel;utf-8";

                return View(adminOperateLogList as PagedList<AdminOperateLog>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "导出操作日志错误" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 查看操作日志详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewDescription(int? id)
        {
            try
            {
                AdminOperateLog AdminOperateLogModel = await MyOperateLogService.GetModelByAsync(a => a.Id == id);

                return View(AdminOperateLogModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "查看详细列表信息错误" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);                
            }
        }

        #region 删除操作

        //// GET: Article/Delete/5
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        MyOperateLogService.DelBy(f => f.Id == id);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "删除操作日志错误:" + ex.Message;
        //        OperateLog.IfSuccess = false;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateDate = DateTime.Now;
        //        OperateLogService.Add(OperateLog);
        //        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
        //}

        ////批量删除
        //[HttpPost]
        //public async Task<IActionResult> Delete(List<int> ids)
        //{
        //    try
        //    {
        //        if (ids != null)
        //        {
        //            MyOperateLogService.DelBy(f => ids.Contains(f.Id));
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "批量删除操作日志错误:" + ex.Message;
        //        OperateLog.IfSuccess = false;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateDate = DateTime.Now;
        //        OperateLogService.Add(OperateLog);
        //        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
        //} 
        #endregion
    }
}
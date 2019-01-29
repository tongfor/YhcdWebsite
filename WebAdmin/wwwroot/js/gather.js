﻿//互联网信息采集对象
; var siteGather = (function () {
    var _menuData = [
        {
            "id": 1,
            "siteName": "市经委",
            "siteKey": "cdgy",
            "startPageNo": 1,
            "endPageNo":5
        }, {
            "id": 2,
            "siteName": "省经信厅",
            "siteKey": "jxt",
            "startPageNo": 1,
            "endPageNo": 30
        }, {
            "id": 3,
            "siteName": "市科技局",
            "siteKey": "cdst",
            "startPageNo": 1,
            "endPageNo": 30
        }, {
            "id": 4,
            "siteName": "省科技厅",
            "siteKey": "kjt",
            "startPageNo": 1,
            "endPageNo": 30
        }, {
            "id": 5,
            "siteName": "高新区",
            "siteKey": "cdht",
            "startPageNo": 1,
            "endPageNo": 30
        }, {
            "id": 6,
            "siteName": "天府新区",
            "siteKey": "cdtf",
            "startPageNo": 1,
            "endPageNo": 30
        }, {
            "id": 7,
            "siteName": "金牛区",
            "siteKey": "jnq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 8,
            "siteName": "锦江区",
            "siteKey": "jjq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 9,
            "siteName": "武侯区经科局",
            "siteKey": "whqjkj",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 10,
            "siteName": "青羊区科经局",
            "siteKey": "qyqkjj",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 11,
            "siteName": "成华区经科局",
            "siteKey": "chqjkj",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 12,
            "siteName": "龙泉驿区",
            "siteKey": "lqyq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 13,
            "siteName": "青白江区科经局",
            "siteKey": "qbjqkjj",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 14,
            "siteName": "新都区",
            "siteKey": "xdq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 15,
            "siteName": "温江区",
            "siteKey": "wjq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 16,
            "siteName": "双流区",
            "siteKey": "slq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 17,
            "siteName": "郫都区",
            "siteKey": "pdq",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 18,
            "siteName": "简阳市",
            "siteKey": "jys",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 19,
            "siteName": "都江堰市",
            "siteKey": "djys",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 20,
            "siteName": "彭州市",
            "siteKey": "pzs",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 21,
            "siteName": "邛崃市",
            "siteKey": "qls",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 22,
            "siteName": "崇州市经信局",
            "siteKey": "czsjxj",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 23,
            "siteName": "金堂县",
            "siteKey": "jtx",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 24,
            "siteName": "新津县",
            "siteKey": "xjx",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 25,
            "siteName": "大邑县",
            "siteKey": "dyx",
            "startPageNo": 1,
            "endPageNo": 10
        }, {
            "id": 26,
            "siteName": "蒲江县经科信局",
            "siteKey": "pjxjkxj",
            "startPageNo": 1,
            "endPageNo": 10
        }
    ];
    var menuObjs = $("#gather-list")
    var _cacheName ="gatherCache"

    //菜单初始化
    var _gatherMenuInit = function () {
        menuObjs.empty()
        var ul = "<ul>\n"
        for (var i in _menuData) {            
            ul += '    <li>\n'
            ul += '<input value="采集' + _menuData[i].siteName
            ul += '信息" id="g_' + _menuData[i].siteKey + '_classId_2" data-classId="2"'
            ul += 'data-siteKey="' + _menuData[i].siteKey
            ul += '" data-startPageNo="' + _menuData[i].startPageNo
            ul += '" data-endPageNo="' + _menuData[i].endPageNo
            ul += '" type = "button" class="gatherbtn" />\n'
            ul += '<br />\n'
            ul += '<span id="submitloading_' + _menuData[i].siteKey
            ul += '_classId_2" style="display:none;text-align:center;">采集开始'
            ul += '<br /><img src="/images/loading.gif" /></span >\n'
        }
        ul += '</ul>\n'
        menuObjs.append(ul)
    }

    //在标签中显示采集结果
    var _showGatherResult = function (tagClassName, data) {
        if (!tagClassName) {
            return
        }
        var o = $("." + tagClassName)
        var showHtml = ""
        if (data) {            
            if (data.gatherMessage && data.gatherMessage != undefined && data.gatherMessage.length > 0) {
                showHtml += '采集' + data.siteName + '提示：<span style="color:red">' + data.gatherMessage + '</span></p>\n'
            }
            else if (data.gatheredArticleList) {
                showHtml += '<p>在' + data.gatherTime
                showHtml += '从' + data.siteName + '采集了' + data.gatheredArticleList.length + '条信息:</p>\n'
                for (var i in data.gatheredArticleList) {
                    showHtml += '<p><a href="' + data.resultShowDomainName
                    showHtml += '/Article/Details-' + data.gatheredArticleList[i].id
                    showHtml += '.html" title="' + data.gatheredArticleList[i].title
                    showHtml += '" target="_blank">' + data.gatheredArticleList[i].title
                    showHtml += '</a>'
                    showHtml += isRecent(data.gatheredArticleList[i].addTime, 3)
                        ? '[<span class="text-error">' + data.gatheredArticleList[i].addTime + '</span>]'
                        : '[<span>' + data.gatheredArticleList[i].addTime + '</span>]'
                    showHtml += '&nbsp; <a href="' + data.gatheredArticleList[i].gatherurl
                    showHtml += '" title="原文地址" target="_blank">原文地址</a>'
                    showHtml += '&nbsp;<a class="thickbox" title="编辑文章内容" href="/Article/Edit/'
                    showHtml += data.gatheredArticleList[i].id
                    showHtml += '?TB_iframe=true&amp;height=600&amp;width=950">编辑</a>'
                    showHtml += '&nbsp;<a title="删除此篇文章" '
                    showHtml += 'href="javascript:if(confirm(\'确实要删除该内容吗 ? \'))delArticle('
                    showHtml += data.gatheredArticleList[i].id + ')"">删除</a>'
                    showHtml += '</p>\n'
                }
            }
        }
        if (0 === o.html().length || "&nbsp;" === o.html()) {
            showHtml = '<p>采集结果如下：</p>\n' + showHtml
        }
        o.append(showHtml)
        gatherCache.set(_cacheName, o.html(), 120)
        o.scrollTop(o.prop('scrollHeight'))
    }

    function isRecent(strDate, differDays) {
        var d1 = new Date(strDate);
        if (!d1 || !differDays || 0 === differDays.length || isNaN(differDays)) {
            return false;
        }
        var d2 = new Date();
        var differMilliseconds = d2.getTime() - d1.getTime();
        return differMilliseconds <= differDays * 24 * 3600 * 1000
    }    

    return {
        menuData: _menuData,
        gatherMenuInit: _gatherMenuInit,
        showGatherResult: _showGatherResult,
        cacheName: _cacheName
    }
}())
$(function () {
    siteGather.gatherMenuInit()
    var resultEle = $(".gather-result")
    resultEle.html()
    var cacheResult = gatherCache.get(siteGather.cacheName)
    if (cacheResult && cacheResult != undefined) {
        resultEle.append(cacheResult)
    }

    $(".gatherbtn").click(function (e) {
        var obj = $(e.target);
        var classId = obj.attr("data-classId");
        var siteKey = obj.attr("data-siteKey")
        var loadingId = "submitloading_" + siteKey + "_" + "classId" + "_" + classId;
        var paras = {
            "siteKey": siteKey,
            "pageStartNo": obj.attr("data-startPageNo"),
            "pageEndNo": obj.attr("data-endPageNo"),
            "classId": classId
        };
        $.ajax({
            url: "/Spider/GatherWebsite",
            type: "POST",
            data: paras,
            timeout: 3000000, //超时时间设置，单位毫秒
            beforeSend: function () {
                obj.attr("disabled", "true");//防止连击
                $("#" + loadingId).show();
            },
            success: function (result, status) {
                if (result && "success" === status && 0 === result.status) {
                    //alert(result.msg);
                    //window.top.location.reload();
                    //window.top.tb_remove();
                    siteGather.showGatherResult("gather-result", result.data)
                }
                else if (result && "error" === status && 1 === result.status && result.msg) {
                    alert(result.msg)
                }
                $("#" + loadingId).hide();
                obj.removeAttr("disabled");
            },
            error: function (xhr, status, error) {
                $("#" + loadingId).hide();
                obj.removeAttr("disabled");
                document.write(xhr.responseText);
                console.log(xhr);
                console.log(status);
                console.log(error);
            },
            complete: function (XMLHttpRequest, status) { //请求完成后最终执行参数
                if ('timeout' === status) {//超时,status还有success,error等值的情况
                    ajaxTimeoutTest.abort();
                    console.log("超时");
                }
            }
        });
        return false;
    });
});

var gatherDb = function () {
    var store = window.localStorage, doc = document.documentElement;
    if (!store) {
        doc.style.behavior = 'url(#default#userData)';
    }
    return {
        /**
         * 保存数据
         */
        set: function (key, val, context) {
            if (store) {
                return store.setItem(key, val, context);
            } else {
                doc.setAttribute(key, value);
                return doc.save(context || 'default');
            }
        },
        /**
         * 读取数据
         */
        get: function (key, context) {
            if (store) {
                return store.getItem(key, context);
            } else {
                doc.load(context || 'default');
                return doc.getAttribute(key) || '';
            }
        },
        /**
         * 删除数据
         * @param {Object}
         * @param {Object}
         */
        rm: function (key, context) {
            if (store) {
                return store.removeItem(key, context);
            } else {
                context = context || 'default';
                doc.load(context);
                doc.removeAttribute(key);
                return doc.save(context);
            }
        },
        /**
         * 清空数据
         */
        clear: function () {
            if (store) {
                return store.clear();
            } else {
                doc.expires = -1;
            }
        }
    };
}();

var gatherCache = function () {
    return {
        get: function (key) {
            var chData = gatherDb.get(key),
                chTime = gatherDb.get(key + "__time"),
                cacheTime = gatherDb.get(key + "_expiredMinutes") || 60,
                now = new Date().getTime(),                
                ct = now - 60000 * cacheTime //默认缓存时间为1个小时
            //存在数据的情况
            if (chData && chTime) {
                //未过期的情况
                if (ct < chTime) {
                    return gatherDb.get(key)
                } else {//过期的情况
                    gatherDb.rm(key);
                    gatherDb.rm(key + "__time")
                }
            }
            else {
                return gatherDb.get(key)
            }
        },
        set: function (key, val, expiredMinutes) {
            gatherDb.set(key, val)
            gatherDb.set(key + "__time", new Date().getTime())
            gatherDb.set(key + "_expiredMinutes", expiredMinutes)
        }
    }
}();

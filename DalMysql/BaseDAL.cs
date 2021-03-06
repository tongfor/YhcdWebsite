﻿/** 
* BaseDAL.cs
*
* 功 能： 数据层通用接口MYSQL实现
* 类 名： BaseDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/8/29 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using IDAL;
using RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DALMySql
{
    public class BaseDAL<T> : IBaseDAL<T> where T : class, new()
    {
        //EF上下文
        protected readonly CdyhcdDBContext _db;        
                
        public BaseDAL(CdyhcdDBContext db)
        {
            _db = db;
        }

        #region 根据主键获取模型

        /// <summary>
        /// 根据主键获取模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetModel(int id)
        {
            return _db.Set<T>().Find(id);
        }

        /// <summary>
        /// 异步根据主键获取模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(int id)
        {
            var model = await _db.Set<T>().FindAsync(id);
            return model;
        }

        #endregion 根据主键获取模型

        #region 添加数据

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public int Add(T model)
        {
            int result;

            _db.Set<T>().Add(model);

            result = _db.SaveChanges();
            return result;
        }

        /// <summary>
        /// 异步添加数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public async Task<int> AddAsync(T model)
        {
            int result;

            _db.Set<T>().Add(model);

            result = await _db.SaveChangesAsync();
            return result;
        }

        #endregion

        #region 根据ID删除数据

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public int Del(T model)
        {
            int result;

            _db.Set<T>().Attach(model);
            _db.Set<T>().Remove(model);

            result = _db.SaveChanges();
            return result;
        }

        /// <summary>
        /// 异步根据ID删除数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public async Task<int> DelAsync(T model)
        {
            int result;

            _db.Set<T>().Attach(model);
            _db.Set<T>().Remove(model);

            result = await _db.SaveChangesAsync();
            return result;
        }

        #endregion

        #region 根据条件删除数据

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="delWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public int DelBy(Expression<Func<T, bool>> delWhere)
        {
            int result;

            List<T> delList = _db.Set<T>().Where(delWhere).ToList();
            delList.ForEach(d =>
            {
                _db.Set<T>().Attach(d);
                _db.Set<T>().Remove(d);
            });

            result = _db.SaveChanges();
            return result;
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="delWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public async Task<int> DelByAsync(Expression<Func<T, bool>> delWhere)
        {
            int result;

            List<T> delList = _db.Set<T>().Where(delWhere).ToList();
            delList.ForEach(d =>
            {
                _db.Set<T>().Attach(d);
                _db.Set<T>().Remove(d);
            });

            result = await _db.SaveChangesAsync();
            return result;
        }

        #endregion

        #region 修改数据

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        public int Modify(T model, params string[] proNames)
        {
            int result;

            EntityEntry entry = _db.Entry<T>(model);
            if (proNames.Length > 0)
            {
                //entry.State = EntityState.Unchanged;
                foreach (string proName in proNames)
                {
                    entry.Property(proName).IsModified = true;
                }
            }
            else//未指明则全部修改
            {
                entry.State = EntityState.Modified;
            }

            result = _db.SaveChanges();
            return result;
        }

        /// <summary>
        /// 异步修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(T model, params string[] proNames)
        {
            int result;

            EntityEntry entry = _db.Entry<T>(model);
            if (proNames.Length > 0)
            {
                //entry.State = EntityState.Unchanged;
                foreach (string proName in proNames)
                {
                    entry.Property(proName).IsModified = true;
                }
            }
            else//未指明则全部修改
            {
                entry.State = EntityState.Modified;
            }

            result = await _db.SaveChangesAsync();
            return result;
        }

        #endregion

        #region 批量修改数据

        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="modifyWhere">条件Lambda表达式</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        /// 
        public int ModifyBy(T model, Expression<Func<T, bool>> modifyWhere, params string[] proNames)
        {
            int result;

            List<T> modifyList = _db.Set<T>().Where(modifyWhere).ToList();
            modifyList.ForEach(m =>
            {
                Modify(model, proNames);
            });
            result= _db.SaveChanges();

            #region 反射实现
            //Type t = typeof(T);
            //List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //Dictionary<string, PropertyInfo> dicPros = new Dictionary<string, PropertyInfo>();

            //proInfos.ForEach(p =>
            //{
            //    if (proNames.Contains(p.Name))
            //    {
            //        dicPros.Add(p.Name, p);
            //    }
            //});

            //foreach (string proName in proNames)
            //{
            //    if (dicPros.Keys.Contains(proName))
            //    {
            //        PropertyInfo proInfo = dicPros[proName];
            //        object newValue = dicPros[proName].GetValue(model, null);
            //        foreach (T modify in modifyList)
            //        {
            //            proInfo.SetValue(modify, newValue, null);
            //        }
            //    }
            //}
            //return _db.SaveChanges();
            #endregion 反射实现

            return result;
        }

        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="modifyWhere">条件Lambda表达式</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        /// 
        public async Task<int> ModifyByAsync(T model, Expression<Func<T, bool>> modifyWhere, params string[] proNames)
        {
            int result;

            List<T> modifyList = _db.Set<T>().Where(modifyWhere).ToList();
            modifyList.ForEach(m =>
            {
                Modify(model, proNames);
            });
            result = await _db.SaveChangesAsync();

            #region 反射实现
            //Type t = typeof(T);
            //List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //Dictionary<string, PropertyInfo> dicPros = new Dictionary<string, PropertyInfo>();

            //proInfos.ForEach(p =>
            //{
            //    if (proNames.Contains(p.Name))
            //    {
            //        dicPros.Add(p.Name, p);
            //    }
            //});

            //foreach (string proName in proNames)
            //{
            //    if (dicPros.Keys.Contains(proName))
            //    {
            //        PropertyInfo proInfo = dicPros[proName];
            //        object newValue = dicPros[proName].GetValue(model, null);
            //        foreach (T modify in modifyList)
            //        {
            //            proInfo.SetValue(modify, newValue, null);
            //        }
            //    }
            //}
            //return _db.SaveChanges();
            #endregion 反射实现

            return result;
        }

        #endregion

        #region 获取总数

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="queryWhere">查询条件</param>
        /// <returns></returns>
        public int GetTotal(Expression<Func<T,bool>> queryWhere)
        {
            var result = _db.Set<T>().Count(queryWhere);
            return result;
        }

        /// <summary>
        /// 异步获取总数
        /// </summary>
        /// <param name="queryWhere">查询条件</param>
        /// <returns></returns>
        public async Task<int> GetTotalAsync(Expression<Func<T, bool>> queryWhere)
        {
            var result = await _db.Set<T>().CountAsync(queryWhere);
            return result;
        }

        #endregion 获取总数

        #region 根据条件查询数据

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public List<T> GetListBy(Expression<Func<T, bool>> queryWhere)
        {
            var results = _db.Set<T>().Where(queryWhere).ToList();
            return results;
        }

        /// <summary>
        /// 异步根据条件查询数据
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public async Task<List<T>> GetListByAsync(Expression<Func<T, bool>> queryWhere)
        {
            var results = await _db.Set<T>().Where(queryWhere).ToListAsync();
            return results;
        }

        #endregion

        #region 根据条件查询数据并排序

        /// <summary>
        /// 根据条件查询数据
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        /// 
        public List<T> GetOrderListBy<TKey>(Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            if (isDesc)
            {
                var results = _db.Set<T>().Where(queryWhere).OrderByDescending(orderBy).ToList();
                return results;
            }
            else
            {
                var results = _db.Set<T>().Where(queryWhere).OrderBy(orderBy).ToList();
                return results;
            }
        }

        /// <summary>
        /// 异步根据条件查询数据
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        /// 
        public async Task<List<T>> GetOrderListByAsync<TKey>(Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            if (isDesc)
            {
                var results = await _db.Set<T>().Where(queryWhere).OrderByDescending(orderBy).ToListAsync();
                return results;
            }
            else
            {
                var results = await _db.Set<T>().Where(queryWhere).OrderBy(orderBy).ToListAsync();
                return results;
            }
        }

        #endregion

        #region 根据条件分页查询数据并排序

        /// <summary>
        /// 根据条件分页查询数据并排序
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        /// 
        public List<T> GetPageListBy<TKey>(int pageIndex, int pagesize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1)*pagesize;
            if (isDesc)
            {
                var results = _db.Set<T>().Where(queryWhere).OrderByDescending(orderBy).Skip(skipIndex).Take(pagesize).ToList();
                return results;
            }
            else
            {
                var results = _db.Set<T>().Where(queryWhere).OrderBy(orderBy).Skip(skipIndex).Take(pagesize).ToList();
                return results;
            }
        }

        /// <summary>
        /// 异步根据条件分页查询数据并排序
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        /// 
        public async Task<List<T>> GetPageListByAsync<TKey>(int pageIndex, int pagesize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pagesize;
            if (isDesc)
            {
                var results = await _db.Set<T>().Where(queryWhere).OrderByDescending(orderBy).Skip(skipIndex).Take(pagesize).ToListAsync();
                return results;
            }
            else
            {
                var results = await _db.Set<T>().Where(queryWhere).OrderBy(orderBy).Skip(skipIndex).Take(pagesize).ToListAsync();
                return results;
            }
        }

        #endregion

        #region 根据条件分页查询数据并输出总行数

        /// <summary>
        /// 根据条件分页查询数据并输出总行数
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="totalCount">数据总数</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public List<T> GetPageListBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, out int totalCount, bool isDesc = false)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            var queryResult = _db.Set<T>().Where(queryWhere);
            var orderResult = isDesc ? queryResult.OrderByDescending(orderBy) : queryResult.OrderBy(orderBy);
            var results = orderResult.Skip(skipIndex).Take(pageSize).ToList();
            totalCount = queryResult.Count();
            return results;
        }

        /// <summary>
        /// 根据条件分页查询数据并并输出总行数
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public async Task<PageData<T>> GetPageDataAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            var result = new PageData<T>
            {
                DataList = await GetPageListByAsync(pageIndex, pageSize, queryWhere, orderBy, isDesc),
                TotalCount = await GetTotalAsync(queryWhere)
            };
            return result;
        }

        #endregion

        #region 根据条件分页查询数据并输出总行数(多条件排序)

        /// <summary>
        /// 根据条件分页查询数据，多条件排序，并输出总行数
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="strOrderBy">排序条件字符串，如如Id desc,ParentId asc,其中asc可以省略</param>
        /// <param name="totalCount">数据总数</param>
        /// <returns></returns>
        public List<T> GetPageListBy(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, string strOrderBy, out int totalCount)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1)*pageSize;
            var queryResult = _db.Set<T>().Where(queryWhere);
            var orderResult = queryResult;
            Type type = typeof(T);
            string[] orderByArray = strOrderBy.Split(',');
            foreach (string s in orderByArray)
            {
                string[] orderBy = s.TrimStart().TrimEnd().Split(' ');
                //排序属性名
                string orderProperty = orderBy[0];
                //正序或反序，正序可省略
                string order = orderBy.Length > 1 ? orderBy[1] : "asc";
                PropertyInfo property = type.GetProperty(orderProperty); //获取指定名称的属性
                if (property == null)
                {
                    continue;
                }
                var parameter = Expression.Parameter(type, "o");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                if (order.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending",
                        new Type[] {typeof(T), property.PropertyType}, orderResult.Expression,
                        Expression.Quote(orderByExp));
                    orderResult = orderResult.Provider.CreateQuery<T>(resultExp);
                }
                else
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy",
                        new Type[] {typeof(T), property.PropertyType}, orderResult.Expression,
                        Expression.Quote(orderByExp));
                    orderResult = orderResult.Provider.CreateQuery<T>(resultExp);
                }
            }
            var results = orderResult.Skip(skipIndex).Take(pageSize).ToList();
            totalCount = queryResult.Count();
            return results;
        }

        /// <summary>
        /// 根据条件分页查询数据，多条件排序，并输出总行数
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="strOrderBy">排序条件字符串，如如Id desc,ParentId asc,其中asc可以省略</param>
        /// <param name="totalCount">数据总数</param>
        /// <returns></returns>
        public async Task<PageData<T>> GetPageListByAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, string strOrderBy)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            var queryResult = _db.Set<T>().Where(queryWhere);
            var orderResult = queryResult;
            Type type = typeof(T);
            string[] orderByArray = strOrderBy.Split(',');
            foreach (string s in orderByArray)
            {
                string[] orderBy = s.TrimStart().TrimEnd().Split(' ');
                //排序属性名
                string orderProperty = orderBy[0];
                //正序或反序，正序可省略
                string order = orderBy.Length > 1 ? orderBy[1] : "asc";
                PropertyInfo property = type.GetProperty(orderProperty); //获取指定名称的属性
                if (property == null)
                {
                    continue;
                }
                var parameter = Expression.Parameter(type, "o");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                if (order.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending",
                        new Type[] { typeof(T), property.PropertyType }, orderResult.Expression,
                        Expression.Quote(orderByExp));
                    orderResult = orderResult.Provider.CreateQuery<T>(resultExp);
                }
                else
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy",
                        new Type[] { typeof(T), property.PropertyType }, orderResult.Expression,
                        Expression.Quote(orderByExp));
                    orderResult = orderResult.Provider.CreateQuery<T>(resultExp);
                }
            }
            PageData<T> result = new PageData<T>
            {
                DataList = await orderResult.Skip(skipIndex).Take(pageSize).ToListAsync(),
                TotalCount = await queryResult.CountAsync()
            };
            return result;
        }

        #endregion 根据条件分页查询数据并输出总行数(多条件排序)

        /// <summary>
        /// 直接SQL查询
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList<T1> SqlQuery<T1>(DbContext db, string sql, params object[] parameters)
            where T1 : new()
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var propts = typeof(T1).GetProperties();
                    var rtnList = new List<T1>();
                    T1 model;
                    object val;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model = new T1();
                            foreach (var l in propts)
                            {
                                try
                                {
                                    val = reader[l.Name];
                                    if (val == DBNull.Value)
                                    {
                                        l.SetValue(model, null);
                                    }
                                    else
                                    {
                                        l.SetValue(model, Convert.ChangeType(val, l.PropertyType));
                                    }
                                }
                                catch
                                {

                                }
                            }
                            rtnList.Add(model);
                        }
                    }
                    return rtnList;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 异步SQL查询
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<IList<T1>> SqlQueryAsync<T1>(DbContext db, string sql, params object[] parameters)
            where T1 : new()
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var propts = typeof(T1).GetProperties();
                    var rtnList = new List<T1>();
                    T1 model;
                    object val;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            model = new T1();
                            foreach (var l in propts)
                            {
                                try
                                {
                                    val = reader[l.Name];
                                    if (val == DBNull.Value)
                                    {
                                        l.SetValue(model, null);
                                    }
                                    else
                                    {
                                        l.SetValue(model, Convert.ChangeType(val, l.PropertyType));
                                    }
                                }
                                catch
                                {

                                }
                            }
                            rtnList.Add(model);
                        }
                    }
                    return rtnList;
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

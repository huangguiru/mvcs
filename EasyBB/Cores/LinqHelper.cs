﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EasyBB.Cores
{

        /// <summary>
        /// Linq通用数据访问类
        /// 指定TDataBase来代替后面要使用的数据上下文(指代)
        /// where：说明指代的类型
        /// new：限定必须有一个不带参数的构造函数
        /// </summary>
        /// <typeparam name="TDataBase"></typeparam>
        public class LinqHelper<TDataBase> where TDataBase : DataContext, new()
        {
            //private static string connectionString = ConfigurationManager.ConnectionStrings["EasyBBConnectionString"].ToString();
            TDataBase db = null;
            /// <summary>
            /// 创建数据库连接
            /// </summary>
            public LinqHelper()
            {
                db = new TDataBase();
                //db.Connection.ConnectionString = connectionString;
            }

            /// <summary>
            /// 获取所有数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public List<T> GetList<T>() where T : class
            {
                return db.GetTable<T>().ToList();
            }

        /// <summary>
        /// 计算总和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int Count<T>() where T : class
        {
            return db.GetTable<T>().Count();
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns></returns>
        public List<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
            {
                return db.GetTable<T>().Where(predicate).ToList();
            }

            /// <summary>
            /// 获取实体
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="predicate"></param>
            /// <returns></returns>
            public T GetEntity<T>(Expression<Func<T, bool>> predicate) where T : class
            {
                return db.GetTable<T>().Where(predicate).FirstOrDefault();
            }

            /// <summary>
            /// 添加实体
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="entity"></param>
            public void InsertEntity<T>(T entity) where T : class
            {
                try
                {
                    //将对象保存到上下文当中
                    db.GetTable<T>().InsertOnSubmit(entity);
                    //提交更改
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            /// <summary>
            /// 修改实体
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="entity"></param>
            public void UpdateEntity<T>(T entity) where T : class
            {
                try
                {
                    //将新实体附加到上下文
                    db.GetTable<T>().Attach(entity);
                    //刷新数据库
                    db.Refresh(RefreshMode.KeepCurrentValues, entity);
                    //提交更改
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void UpdateEntityNoAttach<T>(T entity) where T : class
        {
            try
            {
                db.Refresh(RefreshMode.KeepCurrentValues, entity);
                //提交更改
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void DeleteEntity<T>(Expression<Func<T, bool>> predicate) where T : class
            {
                try
                {
                    //获取要删除的实体
                    var entity = db.GetTable<T>().Where(predicate).FirstOrDefault();
                    if (entity == null) return;
                    db.GetTable<T>().DeleteOnSubmit(entity);
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">当前页面</param>
        /// <param name="rows">取多少条</param>
        /// <returns></returns>
        public List<T> GetListByPage<T>(int page,int rows) where T : class
        {
            return db.GetTable<T>().Skip((page - 1) * rows).Take(rows).ToList();
        }

    }
    }

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EFCodeFirst
{
    /// <summary>
    /// 已完善的仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly BookContext _bookContext;
        private readonly DbSet<T> _dbSet;
        public RepositoryBase(BookContext bookContext)
        {
            _bookContext = bookContext;
            _dbSet = _bookContext.Set<T>();
        }

        /// <summary>
        /// 返回整个可查询的数据表
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Query()
        {
            var query = _dbSet.AsQueryable().AsNoTracking();
            return query;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            var data = await _dbSet.AddAsync(entity);
            var id = await _bookContext.SaveChangesAsync();
            var res = (T)data.Entity;
            return res;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async Task<bool> BulkAddAsync(IEnumerable<T> entitys)
        {
             _dbSet.AddRange(entitys);
            return await _bookContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _bookContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async Task<bool> BulkUpdateAsync(IEnumerable<T> entitys)
        {
            _dbSet.UpdateRange(entitys);
            return await _bookContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T entity)
        {

            _dbSet.Remove(entity);
            return await _bookContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 根据id查询，id是主键
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
            
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async Task<bool> BulkDeleteAsync(IEnumerable<T> entitys)
        {
            _dbSet.RemoveRange(entitys);
            return await _bookContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(object id)
        {
            // using a stub entity to mark for deletion
            var typeInfo = typeof(T).GetTypeInfo();
            var key = _bookContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                _bookContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    await DeleteAsync(entity);
                }
            }
            return await _bookContext.SaveChangesAsync() > 0;
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filterAction"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Func<IQueryable<T>, IQueryable<T>> filterAction, PageRequest pagination)
        {
            var time = DateTime.Now;
            var context = _bookContext;
            var endTime = DateTime.Now - time;
            var query = context.Set<T>().AsQueryable();
            if (query == null)
                throw new Exception($"{typeof(T)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            pagination.Total = await query.CountAsync();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            pagination.SortField = string.IsNullOrWhiteSpace(pagination.SortField) ? "Id" : pagination.SortField;
            query = OrderBy(query, pagination);
            return await query.AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }



        /// <summary>
        /// 分页，推荐
        /// </summary>
        /// <typeparam name="condition">查询条件</typeparam>
        /// <param name="pagination">分页排序条件</param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> condition, PageRequest pagination)
        {
            var time = DateTime.Now;
            var context = _bookContext;
            var endTime = DateTime.Now - time;
            var query = context.Set<T>().AsQueryable();
            if (query == null)
                throw new Exception($"{typeof(T)}对象query为空！");
           
                query = query.Where(condition);
            pagination.Total = await query.CountAsync();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            pagination.SortField = string.IsNullOrWhiteSpace(pagination.SortField) ? "Id" : pagination.SortField;
            query = OrderBy(query, pagination);
            return await query.AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        /// <summary>
        /// 分页，推荐
        /// </summary>
        /// <typeparam name="query">带过滤条件的查询</typeparam>
        /// <param name="pagination">分页排序条件</param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(IQueryable<T> query, PageRequest pagination)
        {
            pagination.Total = await query.CountAsync();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            pagination.SortField = string.IsNullOrWhiteSpace(pagination.SortField) ? "Id" : pagination.SortField;
            query = OrderBy(query, pagination);
            return await query.AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        private IQueryable<T> OrderBy(IQueryable<T> source, PageRequest pagination)
        {
            if (string.IsNullOrWhiteSpace(pagination.SortField))
            {
                var param = Expression.Parameter(typeof(T), "p");
                var property = typeof(T).GetProperty(pagination.SortField);
                var propertyAccessExpression = Expression.MakeMemberAccess(param, property);
                var le = Expression.Lambda(propertyAccessExpression, param);
                var type = typeof(T);
                var orderByStr = pagination.Sort == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExp = Expression.Call(typeof(Queryable), orderByStr, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(le));
                return source.Provider.CreateQuery<T>(resultExp);
            }
            return source;
        }


        /// <summary>
        /// 新增，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<(T entity,bool suc)> AddTransactionAsync(BookContext db, IDbContextTransaction trans, T entity,bool lastExcutRes)
        {
            if (lastExcutRes == false) return (null, false);
            bool suc = true;
            T entityRes = null;
            try
            {
                var data = await db.Set<T>().AddAsync(entity);
                var d = await db.SaveChangesAsync();
                entityRes = (T)data.Entity;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                // TODO: Handle failure
                suc = false;
            }
            return (entityRes, suc);
        }


        /// <summary>
        /// 删除，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTransactionAsync(BookContext db, IDbContextTransaction trans, T entity, bool lastExcutRes)
        {
            if (lastExcutRes == false) return false;
            bool suc = true;
            try
            {
                db.Set<T>().Remove(entity);
                var d = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                suc = false;
            }
            return suc;
        }

        /// <summary>
        /// 更新，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTransactionAsync(BookContext db, IDbContextTransaction trans, T entity, bool lastExcutRes)
        {
            if (lastExcutRes == false) return false;
            bool suc = true;
            try
            {
                db.Set<T>().Update(entity);
                var d = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                suc = false;
            }
            return suc;
        }

        /// <summary>
        /// 批量更新，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<bool> BulkUpdateTransactionAsync(BookContext db, IDbContextTransaction trans, List<T> entitys, bool lastExcutRes)
        {
            if (lastExcutRes == false) return false;
            bool suc = true;
            try
            {
                db.Set<T>().UpdateRange(entitys);
                var d = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                suc = false;
            }
            return suc;
        }

        /// <summary>
        /// 批量删除，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<bool> BulkDeleteTransactionAsync(BookContext db, IDbContextTransaction trans, List<T> entitys, bool lastExcutRes)
        {
            if (lastExcutRes == false) return false;
            bool suc = true;
            try
            {
                db.Set<T>().RemoveRange(entitys);
                var d = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                suc = false;
            }
            return suc;
        }

        /// <summary>
        /// 批量新增，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<bool> BulkAddTransactionAsync(BookContext db, IDbContextTransaction trans, List<T> entitys, bool lastExcutRes)
        {
            if (lastExcutRes == false) return  false;
            bool suc = true;
            try
            {
                 await db.Set<T>().AddRangeAsync(entitys);
                var d = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                suc = false;
            }
            return  suc;
        }

        /// <summary>
        /// sql事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="sql"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        public async Task<(int count, bool suc)> ExcutTransFromSql(BookContext db, IDbContextTransaction trans, string sql, bool lastExcutRes)
        {
            if(lastExcutRes==false) return (0, false);
            bool suc = true;
            int count = 0;
            try
            {
                count = await db.Database.ExecuteSqlCommandAsync(sql);
            }
            catch (Exception ex)
            {
                suc = false;
            }
            return (count, suc);
        }

   
        ///// <summary>
        ///// 通过事务执行
        ///// </summary>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public async Task<bool> AddTransactionAsync(Action<BookContext, IDbContextTransaction> t)
        //{
        //    var result = true;
        //    await Task.Run(() =>
        //    {
        //        using (var transaction = _bookContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                t(_bookContext, transaction);
        //                _bookContext.SaveChanges();
        //                transaction.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transaction.Rollback();
        //                // TODO: Handle failure
        //                result = false;
        //            }
        //        }
        //    });
        //    return result;
        //}
    }
}

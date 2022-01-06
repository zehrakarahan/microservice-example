using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCodeFirst
{
    public interface IRepositoryBase<T> where T : class
    {




        /// <summary>
        /// 返回整个可查询的数据表
        /// </summary>
        /// <returns></returns>
         IQueryable<T> Query();


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task<bool> BulkAddAsync(IEnumerable<T> entitys);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);


        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task<bool> BulkUpdateAsync(IEnumerable<T> entitys);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);


        /// <summary>
        /// 根据id查询，id是主键
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> GetById(object id);


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task<bool> BulkDeleteAsync(IEnumerable<T> entitys);


        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(object id);



        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filterAction"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        Task<List<T>> GetPageListAsync(Func<IQueryable<T>, IQueryable<T>> filterAction, PageRequest pagination);




        /// <summary>
        /// 分页，推荐
        /// </summary>
        /// <typeparam name="condition">查询条件</typeparam>
        /// <param name="pagination">分页排序条件</param>
        /// <returns></returns>
        Task<List<T>> GetPageListAsync(Expression<Func<T, bool>> condition, PageRequest pagination);



        /// <summary>
        /// 分页，推荐
        /// </summary>
        /// <typeparam name="query">带过滤条件的查询</typeparam>
        /// <param name="pagination">分页排序条件</param>
        /// <returns></returns>
        Task<List<T>> GetPageListAsync(IQueryable<T> query, PageRequest pagination);
      


     

        /// <summary>
        /// 新增，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<(T entity, bool suc)> AddTransactionAsync(BookContext db, IDbContextTransaction trans, T entity, bool lastExcutRes);



        /// <summary>
        /// 删除，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<bool> DeleteTransactionAsync(BookContext db, IDbContextTransaction trans, T entity, bool lastExcutRes);


        /// <summary>
        /// 更新，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<bool> UpdateTransactionAsync(BookContext db, IDbContextTransaction trans, T entity, bool lastExcutRes);


        /// <summary>
        /// 批量更新，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<bool> BulkUpdateTransactionAsync(BookContext db, IDbContextTransaction trans, List<T> entitys, bool lastExcutRes);


        /// <summary>
        /// 批量删除，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<bool> BulkDeleteTransactionAsync(BookContext db, IDbContextTransaction trans, List<T> entitys, bool lastExcutRes);
        

        /// <summary>
        /// 批量新增，事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="entitys"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<bool> BulkAddTransactionAsync(BookContext db, IDbContextTransaction trans, List<T> entitys, bool lastExcutRes);


        /// <summary>
        /// sql事务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="trans"></param>
        /// <param name="sql"></param>
        /// <param name="lastExcutRes"></param>
        /// <returns></returns>
        Task<(int count, bool suc)> ExcutTransFromSql(BookContext db, IDbContextTransaction trans, string sql, bool lastExcutRes);
        

    }
}

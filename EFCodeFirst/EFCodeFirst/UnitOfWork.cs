using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCodeFirst
{
    public class UnitOfWork
    {
        private readonly BookContext _bookContext;
        public UnitOfWork(BookContext bookContext)
        {
            _bookContext = bookContext;
        }

        IDbContextTransaction _dbContextTransaction;
        public void BeginTran()
        {
            _dbContextTransaction = _bookContext.Database.BeginTransaction();
        }

        public bool CommitTran()
        {
            if (_dbContextTransaction == null) return false;
            try
            {
                _dbContextTransaction.Commit(); //
                return true;
            }
            catch (Exception ex)
            {
                _dbContextTransaction.Rollback();
                return false;
            }
            finally
            {
                _dbContextTransaction.Dispose();
            }
        }

        public void RollbackTran()
        {
            try
            {
                _dbContextTransaction.Rollback();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _dbContextTransaction.Dispose();
            }
        }
    }
}

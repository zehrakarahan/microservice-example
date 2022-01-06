using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCodeFirst.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;//事务用
        private readonly IRepositoryBase<Book> _repositoryBook;//普通增删改查用
        public  SamplesController(UnitOfWork unitOfWork, IRepositoryBase<Book> repositoryBook)
        {
            _unitOfWork = unitOfWork;
            _repositoryBook = repositoryBook;
        }
        // GET: api/Samples
        [HttpGet(nameof(GetPageListAsync))]
        public async Task<ResponsePageOutput<List<Book>>> GetPageListAsync([FromQuery]BookPageRequest req)
        {
            Expression<Func<Book, bool>> whereCondition = p => true;
            if (!string.IsNullOrWhiteSpace(req.Name)) whereCondition = whereCondition.And(p=>p.Name.Contains(req.Name));
             var pageList=await  _repositoryBook.GetPageListAsync(whereCondition,req);
            return new ResponsePageOutput<List<Book>>() { Data = pageList,TotalCount=req.Total};
        }

        //[HttpPost("trans")]
        //public async Task<ResponseOutput> TransAsync([FromBody] Book value)
        //{
        //    ResponseOutput response = new ResponseOutput();

        //    using (var transaction = _bookContext.Database.BeginTransaction())
        //    {
        //        Book book = new Book() { Name = $"kkk{DateTime.Now.ToString()}" };
        //        var res3 = await _repositoryBook.AddTransactionAsync(_bookContext, transaction, new Book() { Name="yy"}, true);
        //        var res = await _repositoryBook.ExcutTransFromSql(_bookContext, transaction, "update Book set  name='2' where id=1", res3.suc);
        //        var res2 = await _repositoryBook.AddTransactionAsync(_bookContext, transaction, book, res.suc);
        //        await _repositoryBook.AddAsync(new Book() { Name = "yyyyy" });
        //        if (res.suc && res2.suc && res3.suc)
        //        {
        //            transaction.Commit();
        //            response.Code = (int)ResultCode.Success;
        //        }
        //        else
        //        {
        //            transaction.Rollback();
        //            response.SetError();
        //        }
        //    }
        //    return response;
        //}


        [HttpPost("transNew")]
        public async Task<ResponseOutput> TransNew()
        {
            ResponseOutput response = new ResponseOutput();

            _unitOfWork.BeginTran();

            Book book = new Book() { Name = $"kkk{DateTime.Now.ToString()}" };
            var entity1 = await _repositoryBook.AddAsync(new Book() { Name = "yy" });


            var entity2 = await _repositoryBook.AddAsync(new Book() { Name = "yyxx" });

            bool suc = _unitOfWork.CommitTran();

            if (!suc)
            {
                _unitOfWork.RollbackTran();
                response.SetError();
            }
             
            var entity3 = await _repositoryBook.AddAsync(new Book() { Name = "yy" });


            var entity4 = await _repositoryBook.AddAsync(new Book() { Name = "yyxx" });

            suc = _unitOfWork.CommitTran();
            if (!suc)
            {
                _unitOfWork.RollbackTran();
                response.SetError();
            }

            return response;
        }

       

        // GET: api/Samples/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ResponseOutput<Book>> GetAsync(int id)
        {
           var entity= await _repositoryBook.Query().FirstOrDefaultAsync(p => p.ID == id);
            return new ResponseOutput<Book>() { Data = entity };
        }

        // POST: api/Samples
        [HttpPost]
        public async Task<ResponseOutput<Book>> PostAsync([FromBody] Book value)
        {
            var entity = await _repositoryBook.AddAsync(value);
            return new ResponseOutput<Book>() { Data = entity };
        }

        // PUT: api/Samples/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

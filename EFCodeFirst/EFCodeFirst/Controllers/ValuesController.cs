using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCodeFirst.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EFCodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly BookContext _bookContext;
        private readonly IRepositoryBase<Book> _repositoryBook;

        public ValuesController(BookContext bookContext, IRepositoryBase<Book> repositoryBook)
        {
            _bookContext = bookContext;
            _repositoryBook = repositoryBook;
        }

        [HttpGet("add")]
        public async Task<ActionResult<Book>> Add()
        {
            Book book = new Book() {ID=2, Name = $"kkk{DateTime.Now.ToString()}" };
            var t= await _repositoryBook.AddAsync(book);
            return t;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            Book book = new Book() { Name = $"kkk{DateTime.Now.ToString()}" };
            //var data = await _bookContext.Book.AddAsync(book);
            //await _bookContext.SaveChangesAsync();
            //var res = await _repositoryBook.AddAsync(book);
            //var name = res.Name;
            //var id = res.ID;
            //var list = await _repositoryBook.Query();



            //var result=   await _repositoryBook.AddTransactionAsync((db, trans) =>
            //  {

            //      var data =  db.AddAsync(book).Result;
            //      db.SaveChanges();
            //      var x = data.Entity.ID;

            //      var data2 = db.AddAsync(new Book()).Result;

            //  }
            //  );
            //  var temp = result;




            //using (var transaction = _bookContext.Database.BeginTransaction())
            //{
            //var res = await _repositoryBook.AddTransactionAsync(_bookContext, transaction, book);
            //   var res2=  await _repositoryBook.AddTransactionAsync(_bookContext, transaction, new Book() { Name=$"xx{DateTime.Now.ToString()}"});

            //    if (res.suc&&res2.suc) transaction.Commit();
            //    else transaction.Rollback();
            //}

            //using (var transaction = _bookContext.Database.BeginTransaction())
            //{
            //    //transaction.Commit();
            //    //transaction.Commit();
            //    //transaction.Rollback();
            //    //transaction.Rollback();
            //    //var res3 = await _repositoryBook.AddTransactionAsync(_bookContext, transaction, new Book(),true);
            //    var res3 = true;
            //    var res = await _repositoryBook.ExcutTransFromSql(_bookContext, transaction, "update Book set  name='2' where id=1", res3);
            //    var res2 = await _repositoryBook.AddTransactionAsync(_bookContext, transaction, book,res.suc);
            //    await _repositoryBook.AddAsync(new Book() { Name="yyyyy"});
            //    if (res.suc && res2.suc && res3) transaction.Commit();
            //    else transaction.Rollback();
            //}


            return new string[] { "value1", "value2" };
        }

    

        //[HttpGet("test")]
        //public async Task<ActionResult<IEnumerable<Book>>> TestAsync([FromQuery]BookPageRequest request)
        //{
        //    Expression<Func<Book, bool>> whereCondition = p => 1 == 1;
        //    whereCondition = whereCondition.And(p => p.Name.Contains("kkk2019"));
        //    var lists = await _repositoryBook.GetPageListAsync(whereCondition, request);
        //    return lists;
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

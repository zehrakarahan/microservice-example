using ApiCoreTest;
using EFDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiCoreTest.Controllers
{
    public partial class ValuesController : Controller
    {   
        IOptions<ConfigModel> _config;
        ILogger<ValuesController> _logger;
        ApplicationDbContext _db;
        public ValuesController(IOptions<ConfigModel> config, ILogger<ValuesController> logger, ApplicationDbContext db)
        {
            _config = config;
            _logger = logger;
            _db = db;
        }
    }
    public partial class AccountController : Controller
    {   
        IOptions<ConfigModel> _config;
        ILogger<AccountController> _logger;
        ApplicationDbContext _db;
        public AccountController(IOptions<ConfigModel> config, ILogger<AccountController> logger, ApplicationDbContext db)
        {
            _config = config;
            _logger = logger;
            _db = db;
        }
    }
}
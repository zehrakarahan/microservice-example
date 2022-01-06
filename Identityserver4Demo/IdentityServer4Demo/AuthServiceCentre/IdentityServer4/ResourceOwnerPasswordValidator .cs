using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdentitySever4.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServiceCentre.IdentityServer4
{

    /// <summary>
    /// identityserver4登录账号验证类
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IRepositoryBase<Account> _accountRepository;
        private readonly IRepositoryBase<Roles> _roleRepository;
        public ResourceOwnerPasswordValidator(IRepositoryBase<Account> accountRepository, IRepositoryBase<Roles> roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 用户账号验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            var result = await _accountRepository.Query().Where(p => p.AccountName == context.UserName && p.Password == context.Password).FirstOrDefaultAsync();
            if (result != null )
            {
                context.Result = new GrantValidationResult(
                 subject: result.AccountName,
                 authenticationMethod: "custom",
                 claims:await GetUserClaims(result.Id));
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

        /// <summary>
        /// 可以根据需要设置相应的Claim
        /// </summary>
        /// <param accountId=""></param>
        /// <returns></returns>
        private async Task<Claim[]> GetUserClaims(int accountId)
        {
            var roles = await _roleRepository.Query().Where(p => p.AccountId == accountId).ToListAsync();
            return new Claim[]
            {

            //new Claim("UserId", validationOutputDto.AcountId.ToString()),
            new Claim("sucesss", "true"),
            //new Claim(JwtClaimTypes.Name,validationOutputDto.AcountName.ToString()),
            new Claim("roles",  JsonConvert.SerializeObject(roles)),
            };
        }
    }


}

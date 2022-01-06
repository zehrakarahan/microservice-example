using IdentityServer4.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;

namespace AuthServiceCentre.IdentityServer4
{

    /// <summary>
    /// identityserver4
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        public ProfileService(ILogger<ProfileService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// identityserver4获取用户资源信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                await Task.Run(() => {
                    //设置访问用户数据的范围
                    var claims = context.Subject.Claims.ToList();
                    context.IssuedClaims = claims.ToList();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.Run(() => {
                context.IsActive = true;
            });
        }
    }


}

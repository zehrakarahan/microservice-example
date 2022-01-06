using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace AuthServiceCentre.IdentityServer4
{
    public class OAuthMemoryData
    {
        /// <summary>
        /// Api资源 静态方式定义
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
           {
                new ApiResource(OAuthConfig.UserApi.ApiName,OAuthConfig.UserApi.ApiName),
           };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
           {
               new Client()
               {
                   ClientId =OAuthConfig.UserApi.ClientId,
                   AllowedGrantTypes = new List<string>()
                   {
                       GrantTypes.ResourceOwnerPassword.FirstOrDefault(),//Resource Owner Password模式
                   },
                   ClientSecrets = {new Secret(OAuthConfig.UserApi.Secret.Sha256()) },
                   AllowOfflineAccess = true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                   AllowedScopes= {OAuthConfig.UserApi.ApiName,StandardScopes.OfflineAccess},                  
                   AccessTokenLifetime = OAuthConfig.ExpireIn,
               },
          };
        }


        /// <summary>
        /// 测试的账号和密码
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                     SubjectId = "1",
                     Username = "test",
                     Password = "123456"
                }
            };
        }
    }
}

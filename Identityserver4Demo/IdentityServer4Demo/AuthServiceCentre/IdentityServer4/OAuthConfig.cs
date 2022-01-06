﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServiceCentre.IdentityServer4
{
    public class OAuthConfig
    {
        /// <summary>
        /// 过期秒数
        /// </summary>
        public const int ExpireIn = 60;

        /// <summary>
        /// 用户Api相关
        /// </summary>
        public static class UserApi
        {
            public static string ApiName = "user_api";

            public static string ClientId = "user_clientid";

            public static string Secret = "user_secret";
        }
    }
}

﻿using DataAccessHelper.Helper;

namespace DataAccessHelper.Cache
{
    public class CacheConfig
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public static string CacheType
        {
            get
            {
                return ConfigHelper.GetConfigValue("CacheType", "Memcache");
                //return ConfigValue("DBType", "MySql");
            }
        }
        
    }
}

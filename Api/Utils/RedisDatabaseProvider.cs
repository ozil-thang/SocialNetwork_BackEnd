using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Api.Utils
{
    public class RedisDatabaseProvider
    {
        private readonly IConfiguration _configuration;
        private ConnectionMultiplexer _redisMultiplexer;

        public RedisDatabaseProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDatabase GetDatabase()
        {
            if(_redisMultiplexer == null)
            {
                _redisMultiplexer = ConnectionMultiplexer.Connect(_configuration.GetSection("RedisConnection").Value);
            }
            return _redisMultiplexer.GetDatabase();
        }
    }
}

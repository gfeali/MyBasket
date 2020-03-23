using Microsoft.Extensions.Options;
using MyBasket.Shared;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBasket.Infrastructure.Redis
{
    public sealed class RedisManager : IRedisManager
    {
        #region Fields

        private readonly string _connectionString;
        private volatile ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly object _lock = new object();
        private readonly IServer _server;

        #endregion Fields

        #region Ctor

        public RedisManager(IOptions<RedisSettings> redisSetting)
        {
            Check.That(redisSetting.Value.RedisCachingConnectionString.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(redisSetting)));

            this._connectionString = redisSetting.Value.RedisCachingConnectionString;

            this._database = GetDatabase();

            this._server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
        }

        #endregion Ctor

        #region Methods

        public T Get<T>(string key)
        {
            Check.That(key.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(key)));

            var data = _database.StringGet(key);

            return !data.HasValue ? default(T) : JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            Check.That(key.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(key)));

            var data = await _database.StringGetAsync(key);

            return !data.HasValue ? default(T) : JsonConvert.DeserializeObject<T>(data);
        }

        public List<T> GetMany<T>(string key)
        {
            return GetManyAsync<T>(key).Result;
        }

        public async Task<List<T>> GetManyAsync<T>(string key)
        {
            Check.That(key.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(key)));

            var data = await _database.StringGetAsync(key);

            return !data.HasValue ? default(List<T>) : JsonConvert.DeserializeObject<List<T>>(data);
        }

        public void Add(string key, object data, TimeSpan expiry)
        {
            AddAsync(key, data, expiry).Wait();
        }

        public async Task AddAsync(string key, object data, TimeSpan expiry)
        {
            Check.That(key.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(key)));

            Check.That(data.IsNull(), () => throw new ArgumentNullException(nameof(data)));

            Check.That(expiry.Equals(default(TimeSpan)), () => throw new ArgumentNullException(nameof(expiry)));

            await _database.StringSetAsync(key, JsonConvert.SerializeObject(data), expiry);
        }

        public void Add(string key, object data)
        {
            AddAsync(key, data).Wait();
        }

        public async Task AddAsync(string key, object data)
        {
            Check.That(key.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(key)));

            Check.That(data.IsNull(), () => throw new ArgumentNullException(nameof(data)));

            await _database.StringSetAsync(key, JsonConvert.SerializeObject(data));
        }

        public void Delete(string key)
        {
            Check.That(key.IsNull(), () => throw new ArgumentNullException(nameof(key)));

            DeleteAsync(key).Wait();
        }

        public async Task DeleteAsync(string key)
        {
            Check.That(key.IsNullOrEmpty(), () => throw new ArgumentNullException(nameof(key)));

            await _database.KeyDeleteAsync(key);
        }

        public void Flush()
        {
            _server.FlushDatabase();
        }

        public IReadOnlyList<string> Keys
        {
            get
            {
                var data = _server.Keys();

                return data?.Select(k => k.ToString()).ToList();
            }
        }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
        }

        #endregion Methods

        #region Utilities

        private ConnectionMultiplexer GetConnection()
        {
            if (_connectionMultiplexer.NotNull() && _connectionMultiplexer.IsConnected) return _connectionMultiplexer;

            lock (_lock)
            {
                if (_connectionMultiplexer != null && _connectionMultiplexer.IsConnected) return _connectionMultiplexer;

                _connectionMultiplexer?.Dispose();

                _connectionMultiplexer = ConnectionMultiplexer.Connect(_connectionString);
            }

            return _connectionMultiplexer;
        }

        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        #endregion Utilities
    }
}
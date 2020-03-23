using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBasket.Infrastructure.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MyBasket.Tests
{
    [TestClass]
    public class RedisTest
    {
        private readonly IOptions<RedisSettings> _options;

        #region Fields

        private readonly object _redis;
        private readonly Type _redisType;

        #endregion Fields

        #region Ctor

        public RedisTest()
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile($"{Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."))}\\MyBasket.Api\\appsettings.json")
                .Build();

            var redisSettings = new RedisSettings
            {
                RedisCachingConnectionString = configurationRoot.GetSection("RedisSettings").GetValue<string>("RedisCachingConnectionString")
            };
            _options = Options.Create(redisSettings);

            _redisType = typeof(RedisManager);
            _redis = Activator.CreateInstance(_redisType, _options);
        }

        #endregion Ctor

        #region Connection

        [TestMethod]
        public void Test_Connection()
        {
            var method = _redisType
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(x => x.Name == "GetConnection" && x.IsPrivate);

            var connectionMultiplexer = (ConnectionMultiplexer)method.Invoke(_redis, null);

            Assert.IsTrue(connectionMultiplexer.IsConnected);
        }

        #endregion Connection

        #region Add

        [TestMethod]
        public void Test_Add_Val()
        {
            var redisManager = new RedisManager(_options);

            Assert.ThrowsException<AggregateException>(() => redisManager.Add("Key", null));

            Assert.ThrowsException<AggregateException>(() => redisManager.Add(string.Empty, new
            {
                Id = 1,
                Name = "test name"
            }));

            var key = Guid.NewGuid().ToString();

            redisManager.Add(key, "Test Data");

            var val = JsonConvert.DeserializeObject<string>(redisManager.GetDatabase().StringGet(key));

            Assert.AreEqual("Test Data", val);
        }

        [TestMethod]
        public void Test_Add_With_Time_Val()
        {
            var redisManager = new RedisManager(_options);

            var key = Guid.NewGuid().ToString();

            //One minute
            var minute = new TimeSpan(0, 0, 1, 0);

            redisManager.Add(key, "Test Data", minute);

            var val = JsonConvert.DeserializeObject<string>(redisManager.GetDatabase().StringGet(key));

            Assert.AreEqual("Test Data", val);

            Thread.Sleep(minute);

            var val1 = redisManager.GetDatabase().StringGet(key);

            Assert.AreEqual(default(bool), val1.HasValue);
        }

        [TestMethod]
        public async Task Test_Add_Async_Val()
        {
            var redisManager = new RedisManager(_options);

            Assert.ThrowsException<AggregateException>(() => redisManager.Add("Key", null));

            Assert.ThrowsException<AggregateException>(() => redisManager.Add(string.Empty, new
            {
                Id = 1,
                Name = "test name"
            }));

            var key = Guid.NewGuid().ToString();

            await redisManager.AddAsync(key, "Test Data");

            var val = JsonConvert.DeserializeObject<string>(redisManager.GetDatabase().StringGet(key));

            Assert.AreEqual("Test Data", val);
        }

        [TestMethod]
        public async Task Test_Add_Async_With_Time_Val()
        {
            var redisManager = new RedisManager(_options);

            var key = Guid.NewGuid().ToString();

            //One minute
            var minute = new TimeSpan(0, 0, 1, 0);

            await redisManager.AddAsync(key, "Test Data", minute);

            var val = JsonConvert.DeserializeObject<string>(redisManager.GetDatabase().StringGet(key));

            Assert.AreEqual("Test Data", val);

            Thread.Sleep(minute);

            var val1 = redisManager.GetDatabase().StringGet(key);

            Assert.AreEqual(default(bool), val1.HasValue);
        }

        #endregion Add

        #region Delete

        [TestMethod]
        public void Test_Delete_Val()
        {
            var redisManager = new RedisManager(_options);

            Assert.ThrowsException<AggregateException>(() => redisManager.Delete(string.Empty));

            var key = Guid.NewGuid().ToString();

            var result = redisManager.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject("Test Data")).Result;

            redisManager.Delete(key);

            var val = redisManager.GetDatabase().StringGet(key);

            Assert.AreEqual(default(bool), val.HasValue);
        }

        [TestMethod]
        public async Task Test_Delete_Async_Val()
        {
            var redisManager = new RedisManager(_options);

            Assert.ThrowsException<AggregateException>(() => redisManager.Delete(string.Empty));

            var key = Guid.NewGuid().ToString();

            var result = redisManager.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject("Test Data")).Result;

            await redisManager.DeleteAsync(key);

            var val = redisManager.GetDatabase().StringGet(key);

            Assert.AreEqual(default(bool), val.HasValue);
        }

        #endregion Delete

        #region Get

        [TestMethod]
        public void Test_Get_Val()
        {
            var redisManager = new RedisManager(_options);

            var key = Guid.NewGuid().ToString();

            var result = redisManager.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject("Test Data")).Result;

            var val = redisManager.Get<string>(key);

            Assert.AreEqual("Test Data", val);
        }

        [TestMethod]
        public async Task Test_Get_Async_Val()
        {
            var redisManager = new RedisManager(_options);

            var key = Guid.NewGuid().ToString();

            var result = redisManager.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject("Test Data")).Result;

            var val = await redisManager.GetAsync<string>(key);

            Assert.AreEqual("Test Data", val);
        }

        [TestMethod]
        public void Test_GetMany_Val()
        {
            var redisManager = new RedisManager(_options);

            var key = Guid.NewGuid().ToString();

            var result = redisManager.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(new List<string>()
            {
                "Test Data",
                "Test Data"
            })).Result;

            var val = redisManager.GetMany<string>(key);

            Assert.AreEqual(2, val.Count);
        }

        [TestMethod]
        public async Task Test_GetMany_Async_Val()
        {
            var redisManager = new RedisManager(_options);

            var key = Guid.NewGuid().ToString();

            var result = redisManager.GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(new List<string>()
            {
                "Test Data",
                "Test Data"
            })).Result;

            var val = await redisManager.GetManyAsync<string>(key);

            Assert.AreEqual(2, val.Count);
        }

        #endregion Get
    }
}
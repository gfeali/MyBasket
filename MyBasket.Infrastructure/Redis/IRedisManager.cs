using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBasket.Infrastructure.Redis
{
    public interface IRedisManager : IDisposable
    {
        #region Properties

        IReadOnlyList<string> Keys { get; }

        #endregion Properties

        #region Methods

        T Get<T>(string key);

        Task<T> GetAsync<T>(string key);

        List<T> GetMany<T>(string key);

        Task<List<T>> GetManyAsync<T>(string key);

        void Add(string key, object data, TimeSpan expiry);

        Task AddAsync(string key, object data, TimeSpan expiry);

        void Add(string key, object data);

        Task AddAsync(string key, object data);

        void Delete(string key);

        Task DeleteAsync(string key);

        void Flush();

        #endregion Methods
    }
}
using Akavache;
using Akavache.Sqlite3;
using Learning.Core.Contracts.Persistence;
using System.Reactive.Linq;

namespace Learning.App.Impl.Persistence
{
    public class CacheObjectStore : ICacheObjectStore
    {
        public void Start(string storeName)
        {
            Akavache.Registrations.Start(storeName);
        }

        public async Task<T?> GetObjectAsync<T>(string key)
        {
            return await BlobCache.LocalMachine.GetObject<T>(key);
        }

        public async Task<(bool HasData, T? Data)> TryGetObjectAsync<T>(string key)
        {
            try
            {
                var value = await BlobCache.LocalMachine.GetObject<T>(key);
                return (true, value);
            }
            catch
            {
                return (false, default);
            }
        }

        public async Task InsertObjectAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null)
        {
            await BlobCache.LocalMachine.InsertObject(key, value, absoluteExpiration);
        }

        public async Task InvalidateObjectAsync<T>(string key)
        {
            await BlobCache.LocalMachine.InvalidateObject<T>(key);
        }

        public async Task<bool> HasObjectAsync<T>(string key)
        {
            try
            {
                var result = await BlobCache.LocalMachine.GetObject<T>(key);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task InvalidateAllAsync()
        {
            await BlobCache.LocalMachine.InvalidateAll();
        }

        public void Flush()
        {
            BlobCache.LocalMachine.Flush().Wait();
        }
    }

    public static class LinkerPreserve
    {
        static LinkerPreserve()
        {
            var persistentName = typeof(SQLitePersistentBlobCache).FullName;
            var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
        }
    }
}

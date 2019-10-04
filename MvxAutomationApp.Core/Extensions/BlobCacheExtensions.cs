using System;
using System.Threading.Tasks;
using Akavache;

namespace MvxAutomationApp.Core.Extensions
{
    public static class BlobCacheExtensions
    {
        public static Task<bool> ContainsKey(this IBlobCache blobCache, string key)
        {
            var tcs = new TaskCompletionSource<bool>();
            blobCache.Get(key).Subscribe(
                x => tcs.SetResult(true),
                ex => tcs.SetResult(false));

            return tcs.Task;
        }
    }
}

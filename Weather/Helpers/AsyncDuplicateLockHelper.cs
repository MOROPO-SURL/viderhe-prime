using System.Threading;

namespace SmartSolucionesCuba.Viderhe.Weather.Helpers
{
    internal sealed class AsyncDuplicateLockHelper
    {
        private sealed class RefCounted<T>
        {
            public RefCounted(T value)
            {
                RefCount = 1;
                Value = value;
            }

            public int RefCount { get; set; }

            public T Value { get; private set; }
        }

        private static readonly System.Collections.Generic.Dictionary<object, RefCounted<SemaphoreSlim>> SemaphoreSlims = new System.Collections.Generic.Dictionary<object, RefCounted<SemaphoreSlim>>();

        private SemaphoreSlim GetOrCreate(object key)
        {
            RefCounted<SemaphoreSlim> item;

            lock (SemaphoreSlims)
            {
                if (SemaphoreSlims.TryGetValue(key, out item))
                {
                    ++item.RefCount;
                }
                else
                {
                    item = new RefCounted<SemaphoreSlim>(new SemaphoreSlim(1, 1));

                    SemaphoreSlims[key] = item;
                }
            }

            return item.Value;
        }

        public System.IDisposable Lock(object key)
        {
            GetOrCreate(key).Wait();

            return new Releaser { Key = key };
        }

        public async System.Threading.Tasks.Task<System.IDisposable> LockAsync(object key)
        {
            await GetOrCreate(key).WaitAsync().ConfigureAwait(false);

            return new Releaser { Key = key };
        }

        private sealed class Releaser : System.IDisposable
        {
            public object Key { get; set; }

            public void Dispose()
            {
                RefCounted<SemaphoreSlim> item;

                lock (SemaphoreSlims)
                {
                    item = SemaphoreSlims[Key];

                    --item.RefCount;

                    if (item.RefCount == 0)
                    {
                        SemaphoreSlims.Remove(Key);
                    }
                }

                item.Value.Release();
            }
        }
    }
}

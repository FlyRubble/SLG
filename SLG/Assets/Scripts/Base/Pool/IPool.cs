
using System;

namespace SLG
{
    namespace Pool
    {
        public interface IPool
        {
            string name { get; }
            Type poolType { get; }
            int Count { get; }
            void Create(int capacity = 1<<6);
            void Clear();
        }
    }
}

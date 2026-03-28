using UnityEngine;

namespace Code.Core.Pool
{
    public interface IPoolable
    {
        public PoolTypeSO PoolType { get; set; }
        
        public GameObject GameObject { get; }
        
        public void SetUpPool(Pool pool);
        
        public void ResetItem();
    }
}

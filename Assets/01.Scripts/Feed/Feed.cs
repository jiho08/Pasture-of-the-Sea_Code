using System;
using Code.Core.Pool;
using UnityEngine;

namespace Code.Feed
{
    public class Feed : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public FeedInfoSO FeedInfo { get; private set; }
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }
        
        public Action OnDisableFeed;
        public GameObject GameObject => gameObject;

        private Pool _myPool;

        private void Update()
        {
            transform.position += Vector3.down * (FeedInfo.fallSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                OnDisableFeed?.Invoke();
                PushFeed();
            }
        }

        public void PushFeed() => _myPool.Push(this);

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            OnDisableFeed = null;
        }
    }
}
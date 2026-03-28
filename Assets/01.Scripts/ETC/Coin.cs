using System.Collections;
using Code.Core.Pool;
using Code.Player;
using UnityEngine;

namespace Code.ETC
{
    public class Coin : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }
        [SerializeField] private int money = 3;
        [SerializeField] private float force = 50f;
        [SerializeField] private float destroyDelay = 1.5f;
        
        public GameObject GameObject => gameObject;
        
        private Rigidbody _rigid;
        private Pool _myPool;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                //_rigid.isKinematic = true;
                StartCoroutine(DestroyCoroutine());
            }
        }

        public void GetCoin()
        {
            PlayerResourceManager.Instance.Money.Value += money;
            _myPool.Push(this);
        }
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            StopCoroutine(DestroyCoroutine());
            _rigid.isKinematic = false;
            _rigid.linearVelocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;
            _rigid.AddForce(Vector3.down * force);
        }

        private IEnumerator DestroyCoroutine()
        {
            var timer = 0f;

            while (timer < destroyDelay)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            _myPool.Push(this);
        }
    }
}
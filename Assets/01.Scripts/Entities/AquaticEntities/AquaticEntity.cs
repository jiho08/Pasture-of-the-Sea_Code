using Code.Core;
using Code.Core.Pool;
using Code.Entities;
using Code.FSM;
using EPOOutline;
using UnityEngine;

namespace Code.AquaticEntities
{
    public abstract class AquaticEntity : Entity, IPoolable
    {
        [Header("Target Settings")]
        public float targetProximityThreshold = 1f;

        [field: SerializeField] public PoolTypeSO PoolType { get; set; }
        
        public AquaticEntityInfoSO aquaticEntityInfo;
        
        public GameObject GameObject => gameObject;
        public bool IsFull => CurrentHunger >= aquaticEntityInfo.fillingHunger;
        public bool IsHungry => CurrentHunger < aquaticEntityInfo.hungryValue;

        public Rigidbody Rigid { get; private set; }
        public float CurrentHunger { get; set; } = 100f;
        public int EatFeedCount { get; set; }
        public int CurrentPrice { get; protected set; }
        public int FishLevel { get; protected set; } = 1;

        public Outlinable outline;
        
        protected readonly Collider[] _feedCashingArray = new Collider[10];
        protected EntityStateMachine _stateMachine;
        protected Pool _myPool;

        protected override void Awake()
        {
            base.Awake();
            
            Rigid = GetComponent<Rigidbody>();
            _stateMachine = new EntityStateMachine(this, aquaticEntityInfo.states);
            CurrentPrice = aquaticEntityInfo.startPrice;
        }

        protected virtual void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        protected virtual void Update()
        {
            _stateMachine.UpdateStateMachine();
            
            SpawnCoin();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine.FixedUpdateStateMachine();
            
            UpdateHunger();
        }

        public bool CheckForFeed(out Transform feed)
        {
            var feedCount = Physics.OverlapSphereNonAlloc(transform.position, aquaticEntityInfo.feedDetectionRadius,
                _feedCashingArray, aquaticEntityInfo.feedLayer);

            if (feedCount > 0)
            {
                var closestFeed = _feedCashingArray[0];
                var minDistance = Vector3.Distance(transform.position, closestFeed.transform.position);

                for (var i = 1; i < feedCount; ++i)
                {
                    var distance = Vector3.Distance(transform.position, _feedCashingArray[i].transform.position);

                    if (distance >= minDistance)
                        continue;

                    minDistance = distance;
                    closestFeed = _feedCashingArray[i];
                }

                feed = closestFeed.transform;
                return true;
            }

            feed = null;
            return false;
        }

        protected virtual void UpdateHunger()
        {
            if (CurrentHunger <= 0f)
            {
                UnityLogger.Log("굶어 죽음. UI로 띄워주기");
                IsDead = true;
                ChangeState("DEAD");
                return;
            }

            CurrentHunger -= aquaticEntityInfo.hungrySpeed;
        }
        
        public virtual void CheckCanEvolution()
        {
        }
        
        public virtual void Dead()
        {
            _myPool.Push(this);
        }

        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);

        public virtual void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _stateMachine ??= new EntityStateMachine(this, aquaticEntityInfo.states);
        }

        public virtual void ResetItem()
        {
            IsDead = false;
            CurrentHunger = 100f;
            EatFeedCount = 0;
            FishLevel = 1;
            transform.localScale = Vector3.one;
            CurrentPrice = aquaticEntityInfo.startPrice;
            outline.enabled = false;
            _stateMachine.ChangeState("IDLE");
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, aquaticEntityInfo.feedDetectionRadius);
        }

        protected abstract void SpawnCoin();
    }
}
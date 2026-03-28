using Code.Core;
using Code.Entities;
using Code.ETC;
using UnityEngine;

namespace Code.AquaticEntities.States
{
    public class AquaticEntityChaseState : AquaticEntityState
    {
        private Transform _targetFeed;

        public AquaticEntityChaseState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _aquaticEntity.CheckForFeed(out _targetFeed);
        }

        public override void Update()
        {
            base.Update();

            if (!_targetFeed)
            {
                _aquaticEntity.ChangeState("IDLE");
                return;
            }

            var distance = Vector3.Distance(_aquaticEntity.transform.position, _targetFeed.position);

            if (distance < _aquaticEntity.targetProximityThreshold)
            {
                EatFeed();
                _aquaticEntity.CheckCanEvolution();
                _aquaticEntity.ChangeState("IDLE");
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (_targetFeed)
                _movement.MoveTowardsTarget(_targetFeed.position);
        }

        private void EatFeed()
        {
            if (_targetFeed.TryGetComponent(out Feed.Feed feed))
            {
                _aquaticEntity.CurrentHunger += feed.FeedInfo.hungerValue; // 배 채우기
                _aquaticEntity.CurrentHunger = Mathf.Clamp(_aquaticEntity.CurrentHunger, 0f, 100f);
                ++_aquaticEntity.EatFeedCount;
                feed.OnDisableFeed?.Invoke();
                feed.PushFeed();
                UnityLogger.Log("<color=red>밥먹음. 현재 배고픔: </color>" + _aquaticEntity.CurrentHunger);
            }
            else if (_targetFeed.TryGetComponent(out Coin coin))
            {
                if (_aquaticEntity is Dolphin)
                    coin.GetCoin();
            }
            else if (_targetFeed.TryGetComponent(out Fish fish))
            {
                if (_aquaticEntity is Shark)
                {
                    _aquaticEntity.CurrentHunger += fish.CurrentHunger / 2f; // 배 채우기
                    _aquaticEntity.CurrentHunger = Mathf.Clamp(_aquaticEntity.CurrentHunger, 0f, 100f);
                    fish.Dead();
                }
            }
        }
    }
}
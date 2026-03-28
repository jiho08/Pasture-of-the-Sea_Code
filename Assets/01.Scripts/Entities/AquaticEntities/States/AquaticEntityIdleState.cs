using Code.Entities;
using UnityEngine;

namespace Code.AquaticEntities.States
{
    public class AquaticEntityIdleState : AquaticEntityState
    {
        private float _directionChangeTimer, _feedCheckTimer;
        
        public AquaticEntityIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _movement.SetRandomTargetPosition();
            _directionChangeTimer = 0f;
            _feedCheckTimer = 0f;
        }

        public override void Update()
        {
            base.Update();
            
            _directionChangeTimer += Time.deltaTime;
            _feedCheckTimer += Time.deltaTime;
            
            if (_directionChangeTimer >= _movement.directionChangeInterval)
            {
                _movement.SetRandomTargetPosition();
                _directionChangeTimer = 0f;
            }

            if (_feedCheckTimer >= _aquaticEntity.aquaticEntityInfo.feedCheckInterval)
            {
                if (!_aquaticEntity.IsFull && _aquaticEntity.CheckForFeed(out var feed))
                {
                    _movement.TargetPosition = feed.position;
                    _aquaticEntity.ChangeState("CHASE");
                }
                
                _feedCheckTimer = 0f;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _movement.MoveTowardsTarget(_movement.TargetPosition);
        }
    }
}
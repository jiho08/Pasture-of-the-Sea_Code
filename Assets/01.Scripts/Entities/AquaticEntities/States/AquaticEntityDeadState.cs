using Code.Entities;

namespace Code.AquaticEntities.States
{
    public class AquaticEntityDeadState : AquaticEntityState
    {
        public AquaticEntityDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            
            if (_isTriggerCall)
            {
                _aquaticEntity.OnDeadEvent?.Invoke();
            }
        }
    }
}
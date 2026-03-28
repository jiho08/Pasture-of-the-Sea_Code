using Code.Entities;
using Code.FSM;

namespace Code.AquaticEntities.States
{
    public abstract class AquaticEntityState : EntityState
    {
        protected AquaticEntity _aquaticEntity;
        protected AquaticEntityMovement _movement;
        
        protected AquaticEntityState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _aquaticEntity = entity as AquaticEntity;
            _movement = _aquaticEntity.GetCompo<AquaticEntityMovement>();
        }
    }
}
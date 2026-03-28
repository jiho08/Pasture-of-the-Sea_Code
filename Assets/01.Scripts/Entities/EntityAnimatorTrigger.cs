using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
    }
}
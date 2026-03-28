using UnityEngine;

namespace Code.FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/FSM/StateData")]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public string animParamName;
        
        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animParamName);
        }
    }
}
using Code.Core.Pool;
using Code.FSM;
using UnityEngine;

namespace Code.AquaticEntities
{
    [CreateAssetMenu(fileName = "AquaticEntityInfo", menuName = "SO/AquaticEntity/Info", order = 0)]
    public class AquaticEntityInfoSO : ScriptableObject
    {
        [Header("기본 설정")]
        public string aquaticEntityName;
        public string description;
        public int startPrice;
        public float evolutionMultiplier;

        [Header("상태 설정")]
        public StateSO[] states;

        [Header("진화 설정")]
        public int[] evolutionValues;

        [Header("먹이 설정")]
        public LayerMask feedLayer;
        public float feedDetectionRadius;
        [Range(0.1f, 1.5f)]
        public float feedCheckInterval;
        [Range(0f, 100f)]
        public float fillingHunger;
        [Range(0f, 100f)]
        public float hungryValue;
        [Range(0.01f, 0.1f)]
        public float hungrySpeed;

        [Header("코인 설정")]
        public PoolTypeSO[] coinPools;
        public float coinSpawnInterval;
        [Tooltip("물고기한테서 얼마나 밑에 스폰되는지")]
        public float coinSpawnThreshold;
    }
}
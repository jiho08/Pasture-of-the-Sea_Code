using Code.Core.Pool;
using Code.ETC;
using UnityEngine;

namespace Code.AquaticEntities
{
    public class Shark : AquaticEntity
    {
        [SerializeField] private PoolManagerSO poolManager;

        private float _coinTimer;

        protected override void SpawnCoin()
        {
            if (IsHungry)
                return;

            _coinTimer += Time.deltaTime;

            if (_coinTimer >= aquaticEntityInfo.coinSpawnInterval)
            {
                var coin = poolManager.Pop(aquaticEntityInfo.coinPools[FishLevel - 1]) as Coin;
                coin.transform.position = transform.position + Vector3.down * aquaticEntityInfo.coinSpawnThreshold;
                coin.transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                _coinTimer = 0f;
            }
        }
    }
}
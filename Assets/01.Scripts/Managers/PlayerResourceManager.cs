using Code.Core;
using Code.Core.Pool;
using UnityEngine;

namespace Code.Player
{
    public class PlayerResourceManager : MonoSingleton<PlayerResourceManager>
    {
        [field: SerializeField] public PoolTypeSO[] FeedPoolTypes { get; private set; }

        public NotifyValue<int> Money { get; set; } = new();
        public NotifyValue<int> FeedLevel { get; set; } = new(1);

        public int startMoney = 100;
        public int feedMultiCount = 1;

        private void Start()
        {
            Money.Value = startMoney;
        }
    }
}
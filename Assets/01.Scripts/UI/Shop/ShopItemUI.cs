using System;
using Code.Core.Defines;
using Code.AquaticEntities;
using Code.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.UI.Shop
{
    public class ShopItemUI : MonoBehaviour
    {
        [Header("Shop Item UI Settings")]
        [SerializeField] private RawImage itemImage;
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private Button buyButton;

        private ShopItemSO _itemData;
        private Func<ShopItemSO, bool> _onBuy;

        public void Initialize(ShopItemSO itemData, Func<ShopItemSO, bool> onBuyCallback)
        {
            _itemData = itemData;
            _onBuy = onBuyCallback;
            itemImage.texture = itemData.itemTextures[0];

            itemPriceText.text = itemData.itemPrice.ToString();
            buyButton.onClick.AddListener(ClickBuyButton);
        }

        private void ClickBuyButton()
        {
            var success = _onBuy?.Invoke(_itemData) ?? false;

            if (success)
            {
                // 새부적인 구현은 여기서

                switch (_itemData.shopItemType)
                {
                    case EnumDefine.ShopItemType.Fish:
                    {
                        // 물고기 랜덤 생성
                        AquaticEntitySpawnManager.Instance.SpawnEntity(_itemData
                            .fishPoolTypes[Random.Range(0, _itemData.fishPoolTypes.Length)]);
                        break;
                    }

                    case EnumDefine.ShopItemType.FeedCountUpgrade:
                    {
                        var feedCount = PlayerResourceManager.Instance.feedMultiCount++;

                        if (feedCount == _itemData.itemTextures.Length)
                            Destroy(gameObject);
                        else
                            itemImage.texture = _itemData.itemTextures[feedCount];

                        break;
                    }

                    case EnumDefine.ShopItemType.FeedLevelUpgrade:
                    {
                        var feedLevel = PlayerResourceManager.Instance.FeedLevel.Value++;

                        if (feedLevel == _itemData.itemTextures.Length)
                            Destroy(gameObject);
                        else
                            itemImage.texture = _itemData.itemTextures[feedLevel];
                        
                        break;
                    }
                }
            }
        }
    }
}
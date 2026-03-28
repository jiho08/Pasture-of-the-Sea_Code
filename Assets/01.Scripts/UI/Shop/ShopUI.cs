using System.Collections.Generic;
using Code.Core;
using Code.Input;
using Code.Player;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private Vector2 hiddenOffset = new(-200f, 0f);
        [SerializeField] private float slideDuration = 0.25f;
        
        [Header("Shop UI Settings")]
        [field: SerializeField] private List<ShopItemSO> itemList;
        [SerializeField] private Transform itemParent;
        [SerializeField] private ShopItemUI itemPrefab;

        private Vector2 _originalPos;
        private bool _isVisible = true;
        
        private void Awake()
        {
            _originalPos = panelRectTransform.anchoredPosition;
            playerInput.OnTabPressed += SetUIVisual;
        }

        private void Start()
        {
            foreach (var item in itemList)
            {
                var itemUI = Instantiate(itemPrefab, itemParent);
                itemUI.Initialize(item, TryBuyItem);
            }
        }

        private void OnDestroy()
        {
            playerInput.OnTabPressed -= SetUIVisual;
        }

        private bool TryBuyItem(ShopItemSO itemData)
        {
            if (PlayerResourceManager.Instance.Money.Value >= itemData.itemPrice)
            {
                PlayerResourceManager.Instance.Money.Value -= itemData.itemPrice;
                return true;
            }
            
            // 사운드나 텍스트 출력 등 공통적인 사항은 여기서 처리
            UnityLogger.Log("돈 부족");
            return false;
        }
        
        private void SetUIVisual()
        {
            _isVisible = !_isVisible;
            
            if (_isVisible)
                panelRectTransform.DOAnchorPos(_originalPos, slideDuration).SetEase(Ease.OutCubic);
            else
                panelRectTransform.DOAnchorPos(_originalPos + hiddenOffset, slideDuration)
                    .SetEase(Ease.OutCubic);
        }
    }
}
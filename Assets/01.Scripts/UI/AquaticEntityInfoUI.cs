using Code.Core;
using Code.Core.EventChannel;
using Code.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class AquaticEntityInfoUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO fishInfoChannel;
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private float slideDuration = 0.25f;
        [SerializeField] private Vector2 openOffset = new(-575f, 0f);

        [Header("Fish Info Settings")] [SerializeField]
        private TextMeshProUGUI fishNameText;

        [SerializeField] private TextMeshProUGUI currentHungerText;
        [SerializeField] private TextMeshProUGUI currentStateText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI DescriptionText;
        [SerializeField] private TextMeshProUGUI currentPriceText;
        [SerializeField] private Button saleButton;

        private Vector2 _originalPos;
        private bool _isVisible;
        private AquaticEntities.AquaticEntity _currentEntity;

        private void Awake()
        {
            _originalPos = panelRectTransform.anchoredPosition;

            saleButton.onClick.AddListener(ClickSaleButton);
            fishInfoChannel.AddListener<AquaticEntityClickEvent>(SetClickFish);
            fishInfoChannel.AddListener<HideAquaticEntityInfoEvent>(HideFishInfoUI);
        }

        private void LateUpdate()
        {
            if (!_currentEntity || !_isVisible)
                return;

            SetFishInfoText();
        }

        private void OnDestroy()
        {
            saleButton.onClick.RemoveListener(ClickSaleButton);
            fishInfoChannel.RemoveListener<AquaticEntityClickEvent>(SetClickFish);
            fishInfoChannel.RemoveListener<HideAquaticEntityInfoEvent>(HideFishInfoUI);
        }

        private void SetClickFish(AquaticEntityClickEvent evt)
        {
            if (_currentEntity != null)
                _currentEntity.outline.enabled = false;

            _currentEntity = evt.aquaticEntity;
            _currentEntity.outline.enabled = true;
            _isVisible = true;
            SetUIVisual();
        }

        private void HideFishInfoUI(HideAquaticEntityInfoEvent evt)
        {
            _isVisible = false;

            if (_currentEntity != null)
                _currentEntity.outline.enabled = false;

            SetUIVisual();
        }

        private void SetFishInfoText()
        {
            fishNameText.text = _currentEntity.aquaticEntityInfo.aquaticEntityName;
            currentHungerText.text = $"포만감 : {_currentEntity.CurrentHunger:F0}";

            if (_currentEntity.IsDead)
                currentStateText.text = "상태 : 죽음";
            else if (_currentEntity.IsFull)
                currentStateText.text = "상태 : 배부름";
            else if (_currentEntity.IsHungry)
                currentStateText.text = "상태 : 배고픔";
            else
                currentStateText.text = "상태 : 정상";

            currentLevelText.text = $"레벨 : {_currentEntity.FishLevel}";
            DescriptionText.text = _currentEntity.aquaticEntityInfo.description;
            currentPriceText.text = $"판매 : {_currentEntity.CurrentPrice}";
        }

        private void ClickSaleButton()
        {
            if (_currentEntity == null || _currentEntity.IsDead)
                return;
            
            // 판매하기. 로그 등 띄우기
            UnityLogger.Log("물고기 판매");

            PlayerResourceManager.Instance.Money.Value += _currentEntity.CurrentPrice;
            _currentEntity.Dead();
            _isVisible = false;
            SetUIVisual();
        }

        private void SetUIVisual()
        {
            if (_isVisible)
                panelRectTransform.DOAnchorPos(_originalPos + openOffset, slideDuration).SetEase(Ease.OutCubic);
            else
                panelRectTransform.DOAnchorPos(_originalPos, slideDuration)
                    .SetEase(Ease.OutCubic);
        }
    }
}
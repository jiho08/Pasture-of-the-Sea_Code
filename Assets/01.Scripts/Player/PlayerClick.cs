using System;
using System.Collections;
using Code.Core.EventChannel;
using Code.Core.Pool;
using Code.ETC;
using Code.Input;
using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Player
{
    public class PlayerClick : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO fishInfoChannel;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float clickDistance = 10f;

        private PoolTypeSO _currentFeed;
        private PlayerResourceManager _playerResourceManager;
        private Camera _mainCam;
        private int _currentFeedCount;
        private bool _isFixed;
        private bool _isClick;

        private void Awake()
        {
            _playerResourceManager = PlayerResourceManager.Instance;

            playerInput.OnClickPressed += OnClickReceived;
            playerInput.OnSpacePressed += HandleFixed;
            _playerResourceManager.FeedLevel.Subscribe(UpdateFeed);
        }

        private void Start()
        {
            _mainCam = Camera.main;
        }

        private void LateUpdate()
        {
            if (!_isClick)
                return;

            CheckClick();
        }

        private void OnDestroy()
        {
            playerInput.OnClickPressed -= OnClickReceived;
            playerInput.OnSpacePressed -= HandleFixed;
            _playerResourceManager.FeedLevel.Unsubscribe(UpdateFeed);
        }

        private void OnClickReceived() => _isClick = true;

        private void CheckClick()
        {
            _isClick = false;

            if (!EventSystem.current.IsPointerOverGameObject()) // UI 위에 클릭하지 않았을 때
                HandleClick();
        }

        private void HandleClick()
        {
            var ray = _mainCam.ScreenPointToRay(playerInput.ScreenPosition);

            if (!Physics.Raycast(ray, out var hit))
            {
                var clickPos = ray.origin + ray.direction * clickDistance;
                SpawnFeed(clickPos);
                return;
            }

            if (hit.collider.gameObject.TryGetComponent(out AquaticEntities.AquaticEntity aquaticEntity))
            {
                fishInfoChannel.RaiseEvent(AquaticEntityInfoUIEvents.AquaticEntityClickEvent.Initialize(aquaticEntity));
                return;
            }

            if (hit.collider.gameObject.TryGetComponent(out Coin coin))
                coin.GetCoin();
            else
                SpawnFeed(hit.point);
            
            if (!_isFixed)
                fishInfoChannel.RaiseEvent(AquaticEntityInfoUIEvents.HideAquaticEntityInfoEvent);
        }

        private void SpawnFeed(Vector3 pos)
        {
            if (_currentFeedCount >= _playerResourceManager.feedMultiCount)
            {
                // 사운드 재생 등
                return;
            }

            if (_playerResourceManager.Money.Value < 0)
            {
                return;
            }

            var feed = poolManager.Pop(_currentFeed) as Feed.Feed;
            feed.transform.position = pos;
            feed.OnDisableFeed += OnFeedDisable;

            _playerResourceManager.Money.Value -= feed.FeedInfo.feedPrice;
            ++_currentFeedCount;
        }

        private void OnFeedDisable()
        {
            --_currentFeedCount;

            if (_currentFeedCount < 0)
                _currentFeedCount = 0;
        }

        private void HandleFixed() => _isFixed = !_isFixed;

        private void UpdateFeed(int value)
        {
            _currentFeed = _playerResourceManager.FeedPoolTypes[value - 1];
        }
    }
}
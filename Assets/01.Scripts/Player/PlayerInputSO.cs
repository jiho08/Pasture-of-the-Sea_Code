using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Input
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/Input/PlayerInput")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;
        
        public Vector2 MoveInput { get; private set; }
        public Vector2 ScrollInput { get; private set; }
        public Vector2 ScreenPosition { get; private set; }

        public event Action OnClickPressed;
        public event Action OnTabPressed;
        public event Action OnEscapePressed;
        public event Action OnSpacePressed;
        
        private Controls _controls;
        private Vector3 _worldPosition;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }

            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            ScreenPosition = context.ReadValue<Vector2>();
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            ScrollInput = context.ReadValue<Vector2>();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnClickPressed?.Invoke();
        }

        public void OnTab(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnTabPressed?.Invoke();
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnEscapePressed?.Invoke();
        }

        public void OnSpace(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSpacePressed?.Invoke();
        }
    }
}
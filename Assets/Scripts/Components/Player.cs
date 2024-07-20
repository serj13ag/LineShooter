using Enums;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class Player : MonoBehaviour, ITimeTickable
    {
        [SerializeField] private Transform _modelTransform;

        private IInputService _inputService;
        private ITimeService _timeService;

        private float _playerMoveSpeed;

        private Direction _currentFacingDirection;

        public void Init(IInputService inputService, ITimeService timeService, int maxHealth, int damage,
            float playerMoveSpeed, float shootRange, float shootCooldownSeconds, float projectileSpeed)
        {
            _inputService = inputService;
            _timeService = timeService;

            _playerMoveSpeed = playerMoveSpeed;

            _currentFacingDirection = Direction.Left;

            _timeService.Subscribe(this); // TODO unsub
        }

        public void TimeTick(float deltaTime)
        {
            var inputAxis = _inputService.Axis;

            if (inputAxis.magnitude == 0)
            {
                return;
            }

            var newPositionX = transform.position.x;
            var newPositionY = transform.position.y;

            if (inputAxis.x != 0)
            {
                newPositionX += inputAxis.x * _playerMoveSpeed * deltaTime;

                RotateModel(inputAxis.x > 0 ? Direction.Right : Direction.Left);
            }

            if (inputAxis.y != 0)
            {
                newPositionY += inputAxis.y * _playerMoveSpeed * deltaTime;
            }

            newPositionX = Mathf.Clamp(newPositionX, -Constants.PlayerMoveHorizontalBorder, Constants.PlayerMoveHorizontalBorder);
            newPositionY = Mathf.Clamp(newPositionY, Constants.PlayerMoveBottomBorder, Constants.PlayerMoveTopBorder);

            transform.position = new Vector3(newPositionX, newPositionY, 0f);
        }

        private void RotateModel(Direction direction)
        {
            if (_currentFacingDirection != direction)
            {
                _currentFacingDirection = direction;

                _modelTransform.localScale = new Vector3(
                    -_modelTransform.localScale.x,
                    _modelTransform.localScale.y,
                    _modelTransform.localScale.z);
            }
        }
    }
}
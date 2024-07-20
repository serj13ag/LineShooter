using Enums;
using Services;
using UnityEngine;

namespace Components
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _modelTransform;

        private IInputService _inputService;

        private float _playerMoveSpeed;

        private Direction _currentFacingDirection;

        public void Init(IInputService inputService, int maxHealth, int damage, float playerMoveSpeed, float shootRange,
            float shootCooldownSeconds, float projectileSpeed)
        {
            _inputService = inputService;

            _playerMoveSpeed = playerMoveSpeed;

            _currentFacingDirection = Direction.Left;
        }

        private void Update() // TODO time service
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
                newPositionX += inputAxis.x * _playerMoveSpeed * Time.deltaTime;

                RotateModel(inputAxis.x > 0 ? Direction.Right : Direction.Left);
            }

            if (inputAxis.y != 0)
            {
                newPositionY += inputAxis.y * _playerMoveSpeed * Time.deltaTime;
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
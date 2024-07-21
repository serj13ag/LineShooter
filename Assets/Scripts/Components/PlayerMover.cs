using Services;
using UnityEngine;

namespace Components
{
    public class PlayerMover
    {
        private readonly IInputService _inputService;

        private readonly Player _player;
        private readonly float _moveSpeed;

        public PlayerMover(Player player, float moveSpeed, IInputService inputService)
        {
            _inputService = inputService;

            _player = player;
            _moveSpeed = moveSpeed;
        }

        public void TimeTick(float deltaTime)
        {
            var inputAxis = _inputService.Axis;

            if (inputAxis.magnitude == 0)
            {
                return;
            }

            var newPositionX = _player.transform.position.x;
            var newPositionY = _player.transform.position.y;

            if (inputAxis.x != 0)
            {
                newPositionX += inputAxis.x * _moveSpeed * deltaTime;

                _player.OnReceiveInputX(inputAxis.x);
            }

            if (inputAxis.y != 0)
            {
                newPositionY += inputAxis.y * _moveSpeed * deltaTime;
            }

            newPositionX = Mathf.Clamp(newPositionX, -Constants.PlayerMoveHorizontalBorder,
                Constants.PlayerMoveHorizontalBorder);
            newPositionY = Mathf.Clamp(newPositionY, Constants.PlayerMoveBottomBorder, Constants.PlayerMoveTopBorder);

            _player.transform.position = new Vector3(newPositionX, newPositionY, 0f);
        }
    }
}
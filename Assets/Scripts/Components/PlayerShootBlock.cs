using Enums;
using Services;
using UnityEngine;

namespace Components
{
    public class PlayerShootBlock : MonoBehaviour
    {
        [SerializeField] private Transform _weaponTransform;

        private IGameFactory _gameFactory;
        private IEnemyService _enemyService;

        private Player _player;
        private float _shootRange;
        private float _shootCooldownSeconds;
        private PlayerAnimator _playerAnimator;

        private float _timeTillShoot;
        private Vector3 _shootDirection;
        private int _damage;
        private float _projectileSpeed;

        public void Init(Player player, int damage, float shootRange, float shootCooldownSeconds, float projectileSpeed,
            PlayerAnimator playerAnimator, IGameFactory gameFactory, IEnemyService enemyService)
        {
            _gameFactory = gameFactory;
            _enemyService = enemyService;

            _player = player;
            _damage = damage;
            _shootRange = shootRange;
            _shootCooldownSeconds = shootCooldownSeconds;
            _projectileSpeed = projectileSpeed;
            _playerAnimator = playerAnimator;
        }

        public void TimeTick(float deltaTime)
        {
            if (_timeTillShoot <= 0
                && _enemyService.TryGetNearestEnemy(_player.transform.position, _shootRange, out var enemy))
            {
                StartShoot(enemy.transform.position);
            }
            else
            {
                _timeTillShoot -= deltaTime;
            }
        }

        private void StartShoot(Vector3 targetPosition)
        {
            _shootDirection = (targetPosition - _player.transform.position).normalized;
            _playerAnimator.PlayAttack();
            _timeTillShoot = _shootCooldownSeconds;
        }

        public void Shoot(Direction facingDirection)
        {
            _gameFactory.SpawnProjectile(_weaponTransform.position, _weaponTransform.rotation, facingDirection,
                _shootDirection, _projectileSpeed, _damage);
        }
    }
}
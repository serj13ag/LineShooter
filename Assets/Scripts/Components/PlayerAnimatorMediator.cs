using UnityEngine;

namespace Components
{
    public class PlayerAnimatorMediator : MonoBehaviour
    {
        [SerializeField] private Player _player;

        public void AttackAnimationTrigger()
        {
            _player.Shoot();
        }
    }
}
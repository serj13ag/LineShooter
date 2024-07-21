using UnityEngine;

namespace Components
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int AttackSpeedMultiplierAnimatorFloat = Animator.StringToHash("AttackSpeedMultiplier");
        private static readonly int AttackAnimatorTrigger = Animator.StringToHash("Attack");

        [SerializeField] private Animator _animator;
        [SerializeField] private float _minAttackAnimationSpeed;

        public void Init(float shootCooldownSeconds)
        {
            _animator.SetFloat(AttackSpeedMultiplierAnimatorFloat,
                GetAttackSpeedAnimatorMultiplier(shootCooldownSeconds));
        }

        public void PlayAttack()
        {
            _animator.SetTrigger(AttackAnimatorTrigger);
        }

        private float GetAttackSpeedAnimatorMultiplier(float shootCooldownSeconds)
        {
            var multiplier = 1f / shootCooldownSeconds;
            return Mathf.Max(multiplier, _minAttackAnimationSpeed);
        }
    }
}
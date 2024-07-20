using System;

namespace Components
{
    public class HealthBlock
    {
        public int Health { get; private set; }

        public event EventHandler<EventArgs> OnHealthChanged;

        public HealthBlock(int maxHealth)
        {
            Health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage > Health)
            {
                damage = Health;
            }

            if (damage <= 0)
            {
                return;
            }

            Health -= damage;

            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
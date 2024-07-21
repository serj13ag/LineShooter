using System;
using Components;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiHealthBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;

        private HealthBlock _healthBlock;

        public void Init(HealthBlock healthBlock)
        {
            _healthBlock = healthBlock;

            UpdateText();

            healthBlock.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _healthText.text = $"{Constants.HealthLocale}: {_healthBlock.Health}";
        }

        private void OnDestroy()
        {
            _healthBlock.OnHealthChanged -= OnHealthChanged;
        }
    }
}
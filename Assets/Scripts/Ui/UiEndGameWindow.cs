using System;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class UiEndGameWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;

        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _restartButtonText;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        public void Init(WindowType windowType)
        {
            UpdateTitle(windowType);

            _restartButtonText.text = Constants.RestartLocale;
        }

        private void OnRestartButtonClicked()
        {
            // TODO restart game
        }

        private void UpdateTitle(WindowType windowType)
        {
            var title = windowType switch
            {
                WindowType.Win => Constants.WinLocale,
                WindowType.Lose => Constants.LoseLocale,
                _ => throw new ArgumentOutOfRangeException(nameof(windowType), windowType, null),
            };

            _titleText.text = title;
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
    }
}
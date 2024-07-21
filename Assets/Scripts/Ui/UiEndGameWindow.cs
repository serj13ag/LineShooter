using System;
using Enums;
using Infrastructure;
using Infrastructure.StateMachine;
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

        private IGameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = ServiceLocator.Instance.Get<IGameStateMachine>();
        }

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
            _gameStateMachine.Enter<GameplayLevelState, string>(Constants.FirstLevelCode);
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
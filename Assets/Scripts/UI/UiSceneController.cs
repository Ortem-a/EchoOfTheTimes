using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI
{
    public class UiSceneController : MonoBehaviour
    {
        public bool flgIsStartAnimationEnded = false;

        [Header("Event ������� ��� ���������")]
        [SerializeField] private EventSystem _eventSystem;

        [Header("�������� HUD")]
        [SerializeField] private Canvas HUDCanvas;
        private CanvasGroup hudCanvasGroup;

        [SerializeField] private Button ToMainMenuButton;
        [SerializeField] private Transform BottomPanel;
        [SerializeField] private Transform TopPanel;
        [SerializeField] private GameObject ButtonPrefab;

        [Header("��� ������ ������ ���������")]
        public Color DefaultStateButtonColor;
        public Color DisabledStateButtonColor;
        [SerializeField] private Color _lineDopColor;
        [SerializeField] private Color _eyeColor;
        [SerializeField] private Color _backColor;
        [SerializeField] private Color _linesColor;
        [SerializeField] private Color _exitButton;

        [Header("��� ���������� ������ ���������")]
        [SerializeField] private RuntimeAnimatorController[] ButtonControllers;

        [Header("���������/�������� UI")]
        [SerializeField] private RectTransform rectFadeInOutPanel;
        private float StartFadeInDuration_sec = 2f; // ������������ ���������
        private float StartDelay_sec = 0.5f; // �������� ����� �������
        private float HUDStartBeforeEnd_sec = 1f; // ������ ��������� HUD �� K ������ �� �����

        [Header("�������� UI")]
        [SerializeField] private float UselessFinishDuration_sec;
        [SerializeField] private float FinishFadeOutDuration_sec;

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;
        private PlayerProgressHudView _sceneView;
        private InputMediator _inputMediator;
        private LevelAudioManager _levelAudioManager;
        private HUDController _hudController;

        private UiStateButton[] _stateButtons;

        private void Start()
        {
            // ��������� ���������� ������� �� �����
            _eventSystem.enabled = false;

            // ������ HUD
            InitializeHUD();

            // ���������� ����������� + �������� ����� �� ���
            ShowStartLevelCanvas();

            // ������ ������ ������ �� ������
            ApplyExitButtonColor();
        }

        [Inject]
        private void Construct(LevelStateMachine stateMachine, PlayerProgressHudView uiSceneView, InputMediator inputMediator, LevelAudioManager levelAudioManager, HUDController hudController)
        {
            _stateMachine = stateMachine;
            _sceneView = uiSceneView;
            _inputMediator = inputMediator;
            _levelAudioManager = levelAudioManager;
            _hudController = hudController;

            CreateStateButtons();
            InitializeHUDCanvasGroup();

            _loader = FindObjectOfType<SceneLoader>();

            // ToMainMenuButton.onClick.AddListener(ExitToMainMenu);
        }

        private void InitializeHUD()
        {
            HUDCanvas.gameObject.SetActive(false);
            rectFadeInOutPanel.GetComponent<CanvasGroup>().alpha = 1f; // ��������� ��������������
        }

        private void InitializeHUDCanvasGroup()
        {
            hudCanvasGroup = HUDCanvas.GetComponent<CanvasGroup>();
        }

        private void ApplyExitButtonColor()
        {
            var buttonImage = ToMainMenuButton.GetComponent<Image>();
            buttonImage.color = _exitButton;
        }

        private void CreateStateButtons()
        {
            _stateButtons = new UiStateButton[_stateMachine.States.Count];
            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var stateButton = Instantiate(ButtonPrefab, BottomPanel).GetComponent<UiStateButton>();
                stateButton.Init(i, _inputMediator, this, _hudController, ButtonControllers[i],
                    _lineDopColor, _eyeColor, _backColor, _linesColor);
                _stateButtons[i] = stateButton;
            }
            _stateButtons[0].Select();
        }

        // �������� ��� ������� ������ ������ � ������
        public void ExitToMainMenu()
        {
            PersistenceService.OnExitToMainMenu?.Invoke();

            CanvasGroup CanvasGroupFadeInOutPanel = rectFadeInOutPanel.GetComponent<CanvasGroup>();

            _eventSystem.enabled = false;
            flgIsStartAnimationEnded = false;

            CanvasGroupFadeInOutPanel.gameObject.SetActive(true);

            // ������������� �������-���� � ���������� ����� ������� ����������
            _levelAudioManager.StopAmbientSound();

            DOTween.To(() => CanvasGroupFadeInOutPanel.alpha, x => CanvasGroupFadeInOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
                .SetDelay(UselessFinishDuration_sec)
                .OnUpdate(() => {
                    // Debug.Log("����� �� ����� ���������� ������: " + CanvasGroupFadeInOutPanel.alpha);
                })
                .OnComplete(() => _loader.LoadMainMenuSceneAsync());
        }

        // ��������� ��� ������ ������
        public void ShowStartLevelCanvas()
        {
            // StartLevelCanvas.gameObject.SetActive(true);
            CanvasGroup CanvasGroupFadeInOutPanel = rectFadeInOutPanel.GetComponent<CanvasGroup>();
            CanvasGroupFadeInOutPanel.alpha = 1f;

            // ������ ���� ����� �� �������� ��� �� ������ � ������ ��� �������� ������
            _levelAudioManager.PlayAmbientSound();

            // ������ �������� ��������� ���������� ������
            DOTween.To(() => CanvasGroupFadeInOutPanel.alpha, x => CanvasGroupFadeInOutPanel.alpha = x, 0f, StartFadeInDuration_sec)
                .SetDelay(StartDelay_sec)
                .OnStart(() => HUDCanvas.gameObject.SetActive(false))
                .OnUpdate(() =>
                {
                    // ������� ��������� HUD
                    if (CanvasGroupFadeInOutPanel.alpha <= 1)
                    {
                        HUDCanvas.gameObject.SetActive(true);
                        hudCanvasGroup.alpha = Mathf.Lerp(0f, 1f, (HUDStartBeforeEnd_sec - CanvasGroupFadeInOutPanel.alpha * StartFadeInDuration_sec) / HUDStartBeforeEnd_sec * 2);
                    }

                    // Debug.Log("����� �� ����� ����������: " + StartFadeInPanel.alpha);
                })
                .OnComplete(() =>
                {
                    CanvasGroupFadeInOutPanel.gameObject.SetActive(false);
                    HUDCanvas.gameObject.SetActive(true);
                    flgIsStartAnimationEnded = true;
                    _eventSystem.enabled = true;
                });
        }

        // ��������� ��� ���������� ��������� ��������
        public void EnableFinishCanvas()
        {
            CanvasGroup CanvasGroupFadeInOutPanel = rectFadeInOutPanel.GetComponent<CanvasGroup>();

            _eventSystem.enabled = false;
            flgIsStartAnimationEnded = false;

            CanvasGroupFadeInOutPanel.gameObject.SetActive(true);

            // ������������� �������-���� � ���������� ����� ������� ����������
            _levelAudioManager.StopAmbientSound();

            DOTween.To(() => CanvasGroupFadeInOutPanel.alpha, x => CanvasGroupFadeInOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
                .SetDelay(UselessFinishDuration_sec)
                .OnUpdate(() => {
                    // Debug.Log("����� �� ����� ���������� ������: " + CanvasGroupFadeInOutPanel.alpha);
                })
                .OnComplete(() =>
                {
                    if (_loader.HasNextLevel)
                    {
                        Debug.Log("����� ������� ����� �����");
                        _loader.LoadNextSceneGroupAsync();
                    }
                    else
                    {
                        ToMainMenuButton.onClick?.Invoke();
                    }
                });
        }

        public void DeselectAllButtons(int exceptIndex)
        {
            for (int i = 0; i < _stateButtons.Length; i++)
            {
                if (i == exceptIndex) continue;
                _stateButtons[i].Deselect();
            }
        }
    }
}

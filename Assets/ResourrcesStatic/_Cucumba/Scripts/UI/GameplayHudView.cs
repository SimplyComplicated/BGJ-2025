using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public sealed class GameplayHudView : MonoBehaviour
{
    [SerializeField]
    private float _heightOffsetPixels = 100;

    [SerializeField]
    private RectTransform _popup;

    [SerializeField]
    private GameObject _contentInteract;

    [SerializeField]
    private GameObject _contentHacking;

    [SerializeField]
    private Text _txtLabel;

    [SerializeField]
    private Image _imgProgress;

    private Camera _camera;
    private PlayerInteraction _playerInteraction;
    private IInteractableObject _interactableObject;

    private void Awake()
    {
        _camera = Camera.main;
        _playerInteraction = FindAnyObjectByType<PlayerInteraction>();

        _popup.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _playerInteraction.OnInteractionStateChanged += PlayerInteraction_OnInteractionStateChanged;
        _playerInteraction.OnInteractionEnter += PlayerInteraction_OnInteractionEnter;
        _playerInteraction.OnInteractionExit += PlayerInteraction_OnInteractionExit;
    }

    private void OnDisable()
    {
        _playerInteraction.OnInteractionStateChanged -= PlayerInteraction_OnInteractionStateChanged;
        _playerInteraction.OnInteractionEnter -= PlayerInteraction_OnInteractionEnter;
        _playerInteraction.OnInteractionExit -= PlayerInteraction_OnInteractionExit;
    }

    private void Update()
    {
        UpdatePopupPosition();
        UpdatePopupProgress();
    }

    private void PlayerInteraction_OnInteractionStateChanged()
    {
        _interactableObject = _playerInteraction.CurrentInteraction;

        UpdatePopupPosition();
        UpdatePopupDisplay();

        _popup.gameObject.SetActive(_interactableObject != null);
    }

    private void PlayerInteraction_OnInteractionEnter(IInteractableObject item)
    {
        if (item is IHackableObject hackable)
        {
            hackable.OnStateChanged += Hackable_OnStateChanged;
        }
    }

    private void PlayerInteraction_OnInteractionExit(IInteractableObject item)
    {
        if (item is IHackableObject hackable)
        {
            hackable.OnStateChanged -= Hackable_OnStateChanged;
        }
    }

    private void Hackable_OnStateChanged()
    {
        UpdatePopupDisplay();
    }

    private void UpdatePopupDisplay()
    {
        if (_interactableObject != null)
        {
            var isHackable = _interactableObject is IHackableObject;

            _txtLabel.text = isHackable ? GlobalStrings.Hack : GlobalStrings.Interact;

            if (isHackable)
            {
                var hackable = _interactableObject as IHackableObject;
                _contentHacking.SetActive(hackable.IsBusy);
                _contentInteract.SetActive(!hackable.IsBusy);
            }
            else
            {
                _contentHacking.SetActive(false);
                _contentInteract.SetActive(true);
            }
        }
    }

    private void UpdatePopupProgress()
    {
        if (_interactableObject != null)
        {
            if (_interactableObject is IHackableObject hackable)
            {
                _imgProgress.fillAmount = hackable.Progress01;
            }
        }
    }

    private void UpdatePopupPosition()
    {
        if (_interactableObject != null)
        {
            var pos = _camera.WorldToScreenPoint(_interactableObject.WorldPosition);
            pos.z = 0;

            _popup.position = pos + new Vector3(0, _heightOffsetPixels, 0);
        }
    }
}

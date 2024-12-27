using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class PanelAlphaAnimator : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f; // Duration of the fade animation
    [SerializeField] private Ease fadeEase = Ease.Linear; // Easing for the fade animation

    private CanvasGroup _canvasGroup; // CanvasGroup component for alpha control
    private bool _isVisible; // State of the panel

    private void Awake()
    {
        // Get the CanvasGroup component
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is missing!");
            return;
        }

        // Initialize the panel to be fully transparent
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    [ContextMenu("Show Panel")]
    public async UniTask ShowPanel()
    {
        if (_isVisible) return;

        _isVisible = true;

        // Animate the alpha to 1
        await _canvasGroup.DOFade(1f, fadeDuration)
            .SetEase(fadeEase)
            .ToUniTask();

        // Enable interaction
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    [ContextMenu("Hide Panel")]
    public async UniTask HidePanel()
    {
        if (!_isVisible) return;

        _isVisible = false;

        // Disable interaction
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        // Animate the alpha to 0
        await _canvasGroup.DOFade(0f, fadeDuration)
            .SetEase(fadeEase)
            .ToUniTask();
    }
}
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class PanelAlphaAnimator : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f; 

    private CanvasGroup _canvasGroup;
    private bool _isVisible; 

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
    
    public async UniTask FadeIn()
    {
        if (_isVisible) return;

        _isVisible = true;

        // Animate the alpha to 1
        await _canvasGroup.DOFade(1f, fadeDuration)
            .SetEase(Ease.Linear)
            .ToUniTask();

        // Enable interaction
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }
    
    public async UniTask FadeOut()
    {
        if (!_isVisible) return;

        _isVisible = false;

        // Disable interaction
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        // Animate the alpha to 0
        await _canvasGroup.DOFade(0f, fadeDuration)
            .SetEase(Ease.Linear)
            .ToUniTask();
    }
}
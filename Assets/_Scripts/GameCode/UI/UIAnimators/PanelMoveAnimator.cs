using UnityEngine;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

public class PanelMoveAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private float animationDuration = 0.5f;

    private Vector2 _originalPosition; 
    private bool _isVisible;

    private void Awake()
    {
        if (panel == null)
        {
            Debug.LogError("Panel is not assigned!");
            return;
        }

        _originalPosition = panel.anchoredPosition;

        // Initialize the panel's position off-screen
        panel.anchoredPosition = offScreenPosition;
    }
    
    public async UniTask ShowPanel()
    {
        if (_isVisible) return;

        _isVisible = true;

        // Animate the panel to its original position
        await panel.DOAnchorPos(_originalPosition, animationDuration)
            .SetEase(Ease.Linear)
            .ToUniTask();
    }
    
    public async UniTask HidePanel()
    {
        if (!_isVisible) return;

        _isVisible = false;

        // Animate the panel back to the off-screen position
        await panel.DOAnchorPos(offScreenPosition, animationDuration)
            .SetEase(Ease.InQuad)
            .ToUniTask();
    }
}
using UnityEngine;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

public class PanelAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform panel; // Panel to animate
    [SerializeField] private Vector2 offScreenPosition; // Position off the screen
    [SerializeField] private float animationDuration = 0.5f; // Duration of the animation

    private Vector2 _originalPosition; // Panel's original position
    private bool _isVisible; // State of the panel

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

    [ContextMenu("ShowPanel")]
    public async UniTask ShowPanel()
    {
        if (_isVisible) return;

        _isVisible = true;

        // Animate the panel to its original position
        await panel.DOAnchorPos(_originalPosition, animationDuration)
            .SetEase(Ease.Linear)
            .ToUniTask();
    }

    [ContextMenu("HidePanel")]
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
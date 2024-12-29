using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMoveAnimator : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private RectTransform buttonRectTransform;
    [SerializeField] private Transform target;
    [SerializeField] private float animationDuration = 0.3f;

    private Vector3 originalPosition;

    private void Start()
    {
        if (button == null || buttonRectTransform == null)
        {
            Debug.LogError("Button or RectTransform is not assigned.");
            return;
        }

        
        // Observe the button's hold and release events
        var buttonDownStream = button.OnPointerDownAsObservable();
        var buttonUpStream = button.OnPointerUpAsObservable();

        buttonDownStream
            .Subscribe(async _ => await MoveToPosition(target.localPosition))
            .AddTo(this);

        buttonUpStream
            .Subscribe(async _ => await MoveToPosition(originalPosition))
            .AddTo(this);
    }

    private UniTask MoveToPosition(Vector3 position)
    {
        originalPosition = buttonRectTransform.localPosition;
        return buttonRectTransform
            .DOLocalMove(position, animationDuration)
            .SetEase(Ease.OutQuad)
            .ToUniTask(TweenCancelBehaviour.Kill);
    }
}

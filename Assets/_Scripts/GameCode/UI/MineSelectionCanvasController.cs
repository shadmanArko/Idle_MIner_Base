using System;
using Cysharp.Threading.Tasks;
using GameCode.UI;
using UniRx;
using UnityEngine;

public class MineSelectionCanvasController
{
    private readonly MineSelectionCanvasView _view;

    public MineSelectionCanvasController(MineSelectionCanvasView view, CompositeDisposable disposable)
    {
        _view = view;

        BindButton(_view.OpenMineSelection.OnClickAsObservable(), async () => await ShowMineSelection(), disposable);
        BindButton(_view.CloseMineSelection.OnClickAsObservable(), async () => await HideMineSelection(), disposable);
        BindButton(_view.GoToMine01.OnClickAsObservable(), async () => await GoToMine("Mine01"), disposable);
        BindButton(_view.GoToMine02.OnClickAsObservable(), async () => await GoToMine("Mine02"), disposable);
    }

    private void BindButton(IObservable<Unit> buttonObservable, Func<UniTask> action, CompositeDisposable disposable)
    {
        buttonObservable
            .Subscribe(async _ => await action())
            .AddTo(disposable);
    }

    private async UniTask ShowMineSelection()
    {
        await UniTask.WhenAll(
            _view.BlackFadeInFadeOut.FadeIn(),
            _view.MineSelection.ShowPanel()
        );
    }

    private async UniTask HideMineSelection()
    {
        await UniTask.WhenAll(
            _view.BlackFadeInFadeOut.FadeOut(),
            _view.MineSelection.HidePanel()
        );
    }

    private async UniTask GoToMine(string mineName)
    {
        await HideMineSelection();
        await OpenMine(mineName);
    }

    private async UniTask OpenMine(string mineName)
    {
        await UniTask.DelayFrame(1); 
        Debug.Log($"Opening {mineName}");
    }
}
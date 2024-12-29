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
        BindButton(_view.MineSelectionButtonMine01.Button.OnClickAsObservable(), async () => await SelectMine("mine01"), disposable);
        BindButton(_view.MineSelectionButtonMine02.Button.OnClickAsObservable(), async () => await SelectMine("mine02"), disposable);
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

    private async UniTask SelectMine(string mineId)
    {
        await _view.MineSelectionButtonMine01.SetCurrentMine(mineId);
        await HideMineSelection();
        await _view.CircleFadeIn.FadeIn();
        await UniTask.DelayFrame(1); 
        await _view.MineSelectionButtonMine01.RestartCurrentSceneAsync();
    }
    
}
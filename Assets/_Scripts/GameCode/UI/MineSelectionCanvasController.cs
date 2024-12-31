using System;
using Cysharp.Threading.Tasks;
using GameCode.UI;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class MineSelectionCanvasController
{
    private readonly MineSelectionCanvasView _view;
    
    public MineSelectionCanvasController(MineSelectionCanvasView view, CompositeDisposable disposable, UnitOfWork unitOfWork, MineSelectionCard mineSelectionCardPrefab)
    {
        _view = view;

        BindButton(_view.OpenMineSelection.OnClickAsObservable(), async () => await ShowMineSelection(), disposable);
        BindButton(_view.CloseMineSelection.OnClickAsObservable(), async () => await HideMineSelection(), disposable);
        
        foreach (var mine in unitOfWork.Mines.GetAll())
        {
            var mineSelectionCard = Object.Instantiate(mineSelectionCardPrefab, _view.MineSelectionCardsParent);
            mineSelectionCard.MineNameText.text = mine.name;
            mineSelectionCard.MineDescriptionText.text = mine.description;
            var mineSelectionButton = mineSelectionCard.MineSelectionButton;
            if (mineSelectionButton?.Button == null)
            {
                Debug.LogError("Button component is missing!");
                return;
            }
            BindButton(mineSelectionButton.Button.OnClickAsObservable(), async () => await SelectMine(mine.id, mineSelectionButton), disposable);
        }
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

    private async UniTask SelectMine(string mineId, MineSelectionButton mineSelectionButton)
    {
        await mineSelectionButton.SetCurrentMine(mineId);
        await HideMineSelection();
        await _view.CircleFadeIn.FadeIn();
        await UniTask.DelayFrame(1); 
        await mineSelectionButton.RestartCurrentSceneAsync();
    }
    
}
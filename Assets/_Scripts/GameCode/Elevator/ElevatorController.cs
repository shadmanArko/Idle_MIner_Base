using GameCode.Init;
using GameCode.Mineshaft;
using GameCode.UI;
using GameCode.Worker;
using UniRx;
using UnityEngine.UI;

namespace GameCode.Elevator
{
    public class ElevatorController
    {
        private readonly ElevatorModel _model;
        private readonly UnitOfWork _unitOfWork;

        public ElevatorController(ElevatorView view, ElevatorModel model, MineshaftCollectionModel mineshaftCollectionModel,
             GameConfig gameConfig, UnitOfWork unitOfWork, MineSelectionCanvasView mineSelectionCanvasView, CompositeDisposable disposable)
        {
            _model = model;
            _unitOfWork = unitOfWork;

            var workerModel = new WorkerModel(model, gameConfig.ElevatorWorkerConfig, disposable);
            new ElevatorWorkerController(view, model, workerModel, mineshaftCollectionModel, disposable);

            model.CanUpgrade
                .Subscribe(canUpgrade => view.AreaUiCanvasView.UpgradeButton.interactable = canUpgrade)
                .AddTo(disposable);

            view.AreaUiCanvasView.UpgradeButton.OnClickAsObservable()
                .Subscribe(_ => Upgrade())
                .AddTo(disposable);

            model.StashAmount.Subscribe(amount => view.StashAmount = amount.ToString("F0"))
                .AddTo(disposable);
            workerModel.CarryingCapacity
                .Subscribe(capacity => view.AreaUiCanvasView.CarryingCapacity = capacity.ToString("F0"))
                .AddTo(disposable);

            model.UpgradePrice
                .Subscribe(upgradePrice => view.AreaUiCanvasView.UpgradeCost = upgradePrice.ToString("F0"))
                .AddTo(disposable);
            
            foreach (var mineSelectionButton in mineSelectionCanvasView.MineSelectionButtons)
            {
                // Get the Button component
                var button = mineSelectionButton.GetComponent<Button>();
                if (button != null)
                {
                    // Bind button click with the LevelNumber from ButtonData
                    button.OnClickAsObservable()
                        .Select(_ => mineSelectionButton.mineId)  // Get LevelNumber when clicked
                        .Subscribe(LoadData)                 // Send to OpenLevel method
                        .AddTo(disposable);
                }
            }
        }

        private void Upgrade()
        {
            _model.Upgrade();
        }

        private void LoadData(string mineId)
        {
            var mine = _unitOfWork.Mines.GetById(mineId);
            _model.LoadLevel(mine.elevatorLevel);
        }
    }
}
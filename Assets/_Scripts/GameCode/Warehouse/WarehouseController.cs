using GameCode.Elevator;
using GameCode.Init;
using GameCode.Worker;
using UniRx;

namespace GameCode.Warehouse
{
    public class WarehouseController
    {
        private readonly WarehouseModel _model;
        private readonly UnitOfWork _unitOfWork;

        public WarehouseController(WarehouseView view, WarehouseModel model, ElevatorModel elevatorModel,
            GameConfig config, CompositeDisposable disposable, UnitOfWork unitOfWork)
        {
            _model = model;
            _unitOfWork = unitOfWork;

            var workerModel = new WorkerModel(model, config.WarehouseWorkerConfig, disposable);
            new WarehouseWorkerController(view, model, workerModel, elevatorModel, disposable);

            model.CanUpgrade
                .Subscribe(canUpgrade => view.AreaUiCanvasView.UpgradeButton.interactable = canUpgrade)
                .AddTo(disposable);

            view.AreaUiCanvasView.UpgradeButton.OnClickAsObservable()
                .Subscribe(_ => Upgrade())
                .AddTo(disposable);

            workerModel.CarryingCapacity
                .Subscribe(capacity => view.AreaUiCanvasView.CarryingCapacity = capacity.ToString("F0"))
                .AddTo(disposable);

            model.UpgradePrice
                .Subscribe(upgradePrice => view.AreaUiCanvasView.UpgradeCost = upgradePrice.ToString("F0"))
                .AddTo(disposable);
        }

        private void Upgrade()
        {
            _model.Upgrade();
        }
        
        public void LoadData(string mineId)
        {
            var mine = _unitOfWork.Mines.GetById(mineId);
            _model.LoadLevel(mine.warehouseLevel);
        }
    }
}
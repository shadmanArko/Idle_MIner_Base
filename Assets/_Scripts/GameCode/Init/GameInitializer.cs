using System.Threading.Tasks;
using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Finance;
using GameCode.Mineshaft;
using GameCode.Persistence;
using GameCode.Tutorial;
using GameCode.UI;
using GameCode.Warehouse;
using UniRx;
using UnityEngine;

namespace GameCode.Init
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private HudView _hudView;
        [SerializeField] private CameraView _cameraView;
        
        [SerializeField] private MineSelectionCanvasView _mineSelectionCanvasView;
        
        [SerializeField] private ElevatorView _elevatorView;
        [SerializeField] private WarehouseView _warehouseView;
        [SerializeField] private Transform _mineshaftStartingPosition;

        [SerializeField] private UnitOfWork _unitOfWork;
        [SerializeField] private Initializer _dataInitializer;
        

        private async void Start()
        {
            await _dataInitializer.LoadDataAsync();
            var disposable = new CompositeDisposable().AddTo(this);

            var tutorialModel = new TutorialModel();
            var financeModel = new FinanceModel(PlayerPrefsManager.CurrentMineId, _unitOfWork, disposable);
            
            new CameraController(_cameraView, tutorialModel);

            //Hud
            new HudController(_hudView, financeModel, tutorialModel, disposable);
            
            //MineSelectionCanvas
            new MineSelectionCanvasController(_mineSelectionCanvasView, disposable);

            //Mineshaft
            var mineshaftCollectionModel = new MineshaftCollectionModel();
            var mineshaftFactory = new MineshaftFactory(mineshaftCollectionModel, financeModel, _gameConfig, _unitOfWork, disposable);
            //mineshaftFactory.LoadMineData(PlayerPrefs.GetString("CurrentMine"));
            //mineshaftFactory.CreateMineshaft(1,1, _mineshaftStartingPosition.position, "mine01");

            //Elevator
            var elevatorModel = new ElevatorModel(1, _gameConfig, financeModel, disposable, _unitOfWork, PlayerPrefsManager.CurrentMineId );
            var elevatorController =  new ElevatorController(_elevatorView, elevatorModel, mineshaftCollectionModel, _gameConfig, _unitOfWork, disposable);
            
            //Warehouse
            var warehouseModel = new WarehouseModel(1, _gameConfig, financeModel, disposable, _unitOfWork, PlayerPrefsManager.CurrentMineId );
            var warehouseController = new WarehouseController(_warehouseView, warehouseModel, elevatorModel, _gameConfig, disposable, _unitOfWork);

            new LoadManager(elevatorController, mineshaftFactory,PlayerPrefsManager.CurrentMineId, warehouseController);
        }
    }
}
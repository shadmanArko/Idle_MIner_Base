using GameCode.CameraRig;
using GameCode.Elevator;
using GameCode.Finance;
using GameCode.Mineshaft;
using GameCode.Persistence;
using GameCode.Persistence.Repositories;
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
        [SerializeField] private MineSelectionCard _mineSelectionCard;
        
        [SerializeField] private ElevatorView _elevatorView;
        [SerializeField] private WarehouseView _warehouseView;
        [SerializeField] private Transform _mineshaftStartingPosition;
        
        [SerializeField] private TextAsset _saveDataJsonFile;
        

        private async void Start()
        {
            //Persistence
           new PlayerPrefsManager();
            var dataContext = new JsonDataContext();
            var mines = new Mines();
            var disposable = new CompositeDisposable().AddTo(this);
            var dataInitializer = new Initializer(dataContext, mines, _saveDataJsonFile);
            var unitOfWork = new UnitOfWork(dataContext, mines);
            await dataInitializer.LoadDataAsync();
            var saveManager = new SaveManager(unitOfWork, disposable);
            saveManager.StartPeriodicSave();
            //InvokeRepeating(nameof(unitOfWork.Save),5f, 5f);
            
            

            var tutorialModel = new TutorialModel();
            var financeModel = new FinanceModel(PlayerPrefsManager.CurrentMineId, unitOfWork, disposable);
            
            new CameraController(_cameraView, tutorialModel);

            //Hud
            new HudController(_hudView, financeModel, tutorialModel, disposable);
            
            //MineSelectionCanvas
            new MineSelectionCanvasController(_mineSelectionCanvasView, disposable, unitOfWork, _mineSelectionCard);

            //Mineshaft
            var mineshaftCollectionModel = new MineshaftCollectionModel();
            var mineshaftFactory = new MineshaftFactory(mineshaftCollectionModel, financeModel, _gameConfig, unitOfWork, disposable);
            //mineshaftFactory.LoadMineData(PlayerPrefs.GetString("CurrentMine"));
            //mineshaftFactory.CreateMineshaft(1,1, _mineshaftStartingPosition.position, "mine01");

            //Elevator
            var elevatorModel = new ElevatorModel(1, _gameConfig, financeModel, disposable, unitOfWork, PlayerPrefsManager.CurrentMineId );
            var elevatorController =  new ElevatorController(_elevatorView, elevatorModel, mineshaftCollectionModel, _gameConfig, unitOfWork, disposable);
            
            //Warehouse
            var warehouseModel = new WarehouseModel(1, _gameConfig, financeModel, disposable, unitOfWork, PlayerPrefsManager.CurrentMineId );
            var warehouseController = new WarehouseController(_warehouseView, warehouseModel, elevatorModel, _gameConfig, disposable, unitOfWork);

            new LoadManager(elevatorController, mineshaftFactory,PlayerPrefsManager.CurrentMineId, warehouseController);
        }
    }
}
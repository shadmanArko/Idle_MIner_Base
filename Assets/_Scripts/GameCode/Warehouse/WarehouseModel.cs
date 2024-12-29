using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using UniRx;
using UnityEngine;

namespace GameCode.Warehouse
{
    public class WarehouseModel : IAreaModel
    {
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        private const double BasePrice = 60;
        private readonly IReactiveProperty<double> _upgradePrice;
        private readonly IReactiveProperty<int> _level;
        private readonly UnitOfWork _unitOfWork;
        private readonly string _mineId;

        public WarehouseModel(int level, GameConfig config, FinanceModel financeModel, CompositeDisposable disposable, UnitOfWork unitOfWork, string mineId)
        {
            _config = config;
            _financeModel = financeModel;
            _unitOfWork = unitOfWork;
            _mineId = mineId;
            
            _level = new ReactiveProperty<int>(level);
            
            _upgradePrice = new ReactiveProperty<double>();
    
            CanUpgrade = _financeModel.Money
                .CombineLatest(_upgradePrice, (money, price) => money >= price)  // Changed to CombineLatest like ElevatorModel
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
    
            // Add subscription to level changes
            _level.Subscribe(_ => UpdateLevelDependentProperties()).AddTo(disposable);
    
            // Initialize dependent properties
            UpdateLevelDependentProperties();
        }
        
        private void UpdateLevelDependentProperties()
        {
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, 1) *
                              Mathf.Pow(_config.ActorUpgradeSkillIncrement, _level.Value - 1);
            _upgradePrice.Value = BasePrice * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1);
        }

        public double SkillMultiplier { get; set; }

        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }

        public void AddResource(double amount) => _financeModel.AddResource(amount);
        public IReadOnlyReactiveProperty<double> UpgradePrice => _upgradePrice;
        public IReadOnlyReactiveProperty<int> Level => _level;

        public void Upgrade()
        {
            if (_financeModel.Money.Value < _upgradePrice.Value) return;

            SkillMultiplier *= _config.ActorUpgradeSkillIncrement;
            var upgradePrice = _upgradePrice.Value;
            _upgradePrice.Value *= _config.ActorUpgradePriceIncrement;
            _financeModel.DrawResource(upgradePrice);
            _level.Value++;
            SaveLevel(_mineId);
        }
        
        private void SaveLevel(string mineId)
        {
            var mine = _unitOfWork.Mines.GetById(mineId);
            mine.warehouseLevel = _level.Value;
            _unitOfWork.Mines.Modify(mine);
            //_unitOfWork.Save();
        }

        public void LoadLevel(int newLevel)
        {
            _level.Value = newLevel;
        }
    }
}
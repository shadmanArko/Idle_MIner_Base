using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using UniRx;
using UnityEngine;

namespace GameCode.Elevator
{
    public class ElevatorModel : IAreaModel
    {
        private const double BasePrice = 60;
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        private readonly IReactiveProperty<double> _upgradePrice;
        private readonly IReactiveProperty<int> _level;
        private readonly UnitOfWork _unitOfWork;
        private readonly string _mineId;

        public ElevatorModel(int level, GameConfig config, FinanceModel financeModel, CompositeDisposable disposable, UnitOfWork unitOfWork, string mineId)
        {
            _config = config;
            _financeModel = financeModel;
            _unitOfWork = unitOfWork;
            _mineId = mineId;

            _level = new ReactiveProperty<int>(level);
            
            StashAmount = new ReactiveProperty<double>(_unitOfWork.Mines.GetById(mineId).elevatorStashAmount);
            _upgradePrice = new ReactiveProperty<double>();
            
            CanUpgrade = _financeModel.Money
                .CombineLatest(_upgradePrice, (money, price) => money >= price)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
            
            _level.Subscribe(_ => UpdateLevelDependentProperties()).AddTo(disposable);
            
            StashAmount.Subscribe(_ => SaveStashAmount(_mineId)).AddTo(disposable);
            
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

        public IReactiveProperty<double> StashAmount { get; }
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
             mine.elevatorLevel = _level.Value;
             mine.elevatorStashAmount = StashAmount.Value;
            _unitOfWork.Mines.Modify(mine);
            //_unitOfWork.Save();
        }
        
        private void SaveStashAmount(string mineId)
        {
            var mine = _unitOfWork.Mines.GetById(mineId);
            mine.elevatorStashAmount = StashAmount.Value;
            _unitOfWork.Mines.Modify(mine);
            //_unitOfWork.Save();
        }

        public void LoadLevel(int newLevel)
        {
            _level.Value = newLevel;
        }
        
        public void LoadStashAmount(double amount)
        {
            StashAmount.Value = amount;
        }

        public double DrawResource(double amount)
        {
            var result = 0d;
            if (StashAmount.Value <= amount)
            {
                result = StashAmount.Value;
                StashAmount.Value = 0;
            }
            else
            {
                result = amount;
                StashAmount.Value -= amount;
            }

            return result;
        }
    }
}
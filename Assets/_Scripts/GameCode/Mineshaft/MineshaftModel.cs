using System.Linq;
using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using GameCode.Persistence.Models;
using UniRx;
using UnityEngine;

namespace GameCode.Mineshaft
{
    public class MineshaftModel : IAreaModel
    {
        private const double BasePrice = 60;
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        private readonly UnitOfWork _unitOfWork;
        
        public int MineshaftNumber { get; }
        private string MineId { get; }

        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }

        private readonly IReactiveProperty<double> _upgradePrice;
        public IReadOnlyReactiveProperty<double> UpgradePrice => _upgradePrice;

        private readonly IReactiveProperty<int> _level;
        public IReadOnlyReactiveProperty<int> Level => _level;
        public readonly IReactiveProperty<double> StashAmount;

        public double NextShaftPrice { get; }
        public IReadOnlyReactiveProperty<bool> CanBuyNextShaft { get; }

        public MineshaftModel(int shaftNumber, int level, Vector2 position, GameConfig config,
            FinanceModel financeModel, CompositeDisposable disposable, UnitOfWork unitOfWork, string mineId, double stashAmount, bool isNew)
        {
            MineshaftNumber = shaftNumber;
            _config = config;
            _financeModel = financeModel;
            _unitOfWork = unitOfWork;
            MineId = mineId;
            
            _level = new ReactiveProperty<int>(level);
            StashAmount = new ReactiveProperty<double>(stashAmount);
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, MineshaftNumber) * Mathf.Pow(config.ActorUpgradeSkillIncrement, _level.Value - 1);
            
            _upgradePrice = new ReactiveProperty<double>(BasePrice * Mathf.Pow(config.ActorPriceIncrementPerShaft, MineshaftNumber - 1)
                                                                   * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1));
            NextShaftPrice = config.MineshaftConfig.BaseMineshaftCost * Mathf.Pow(config.MineshaftConfig.MineshaftCostIncrement, MineshaftNumber - 1);
            CanUpgrade = _financeModel.Money
                .Select(money => money >= _upgradePrice.Value)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
            CanBuyNextShaft = _financeModel.Money
                .Select(money => money >= NextShaftPrice)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
            
            if (isNew)
            {
                var mineshaftData = new MineshaftData
                {
                    mineshaftNumber = MineshaftNumber,
                    level = _level.Value,
                    position = position
                };
                var mine = _unitOfWork.Mines.GetById(MineId);
                mine.mineshafts.Add(mineshaftData);
            }
            StashAmount.Subscribe(_ => SaveStashAmount(MineId)).AddTo(disposable);
           
        }

        public void Upgrade()
        {
            if (_financeModel.Money.Value < _upgradePrice.Value)
                return;

            var price = _upgradePrice.Value;
            SkillMultiplier *= _config.ActorUpgradeSkillIncrement;
            _upgradePrice.Value *= _config.ActorUpgradePriceIncrement;
            _financeModel.DrawResource(price);
            _level.Value++;
            SaveLevel();
        }
        
        private void SaveLevel()
        {
            var mine = _unitOfWork.Mines.GetById(MineId);
            var mineshaft = mine.mineshafts.FirstOrDefault(data => data.mineshaftNumber == MineshaftNumber);
            mineshaft.level = _level.Value;
            _unitOfWork.Mines.Modify(mine);
            //_unitOfWork.Save();
        }
        
        private void SaveStashAmount(string mineId)
        {
            var mine = _unitOfWork.Mines.GetById(mineId);
            var mineshaft = mine.mineshafts.FirstOrDefault(data => data.mineshaftNumber == MineshaftNumber);
            mineshaft.mineshaftStashAmount = StashAmount.Value;
            _unitOfWork.Mines.Modify(mine);
            //_unitOfWork.Save();
        }
        
        public void BuyNextShaft()
        {
            if (_financeModel.Money.Value < NextShaftPrice)
                return;
            _financeModel.DrawResource(NextShaftPrice);
        }

        public double SkillMultiplier { get; set; }
        
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
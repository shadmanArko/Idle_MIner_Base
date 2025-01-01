using System.Linq;
using GameCode.Finance;
using GameCode.Init;
using GameCode.Persistence.Models;
using UniRx;
using UnityEngine;

namespace GameCode.Mineshaft
{
    public class MineshaftFactory : IMineshaftFactory
    {
        private readonly MineshaftCollectionModel _collectionModel;
        private readonly FinanceModel _financeModel;
        private readonly GameConfig _config;
        private readonly CompositeDisposable _disposable;
        private readonly UnitOfWork _unitOfWork;

        public MineshaftFactory(MineshaftCollectionModel collectionModel, FinanceModel financeModel, GameConfig config, UnitOfWork unitOfWork, CompositeDisposable disposable)
        {
            _collectionModel = collectionModel;
            _financeModel = financeModel;
            _config = config;
            _unitOfWork = unitOfWork;
            _disposable = disposable;
        }

        public void CreateMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position, string mineId, double stashAmount, bool isNew = true)
        {
            var view = Object.Instantiate(_config.MineshaftConfig.MineshaftPrefab, position, Quaternion.identity);
            var mineshaftModel = new MineshaftModel(mineshaftNumber, mineshaftLevel,position, _config, _financeModel, _disposable, _unitOfWork, mineId, stashAmount, isNew);
            new MineshaftController(view, mineshaftModel, this, _config, _disposable, mineId);
            _collectionModel.RegisterMineshaft(mineshaftNumber, mineshaftModel, view);
        }
        
        public void LoadData(string mineId)
        {
            foreach (var mineshaftData in _unitOfWork.Mines.GetById(mineId).mineshafts.ToList())
            {
                CreateMineshaft(
                    mineshaftData.mineshaftNumber,
                    mineshaftData.level,
                    mineshaftData.position,
                    mineId,
                    mineshaftData.mineshaftStashAmount,
                    isNew: false
                );
            }
        }
    }
}
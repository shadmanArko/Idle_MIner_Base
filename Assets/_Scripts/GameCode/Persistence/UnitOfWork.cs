using GameCode.Persistence.Repositories;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitOfWork
{
    private readonly DataContext _dataContext;

    private readonly Mines _mines;

    public UnitOfWork(DataContext dataContext, Mines mines)
    {
        _dataContext = dataContext;
        _mines = mines;
    }

    public Mines Mines => _mines;
    
    
    public async void Save() => await _dataContext.Save();
    
}

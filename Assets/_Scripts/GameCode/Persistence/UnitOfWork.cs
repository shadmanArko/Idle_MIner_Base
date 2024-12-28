using GameCode.Persistence.Repositories;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitOfWork : MonoBehaviour
{
    [SerializeField] private DataContext _dataContext;

    [SerializeField] private Mines mines;
    
    
    public Mines Mines => mines;
    
    
    public async void Save() => await _dataContext.Save();
    
}

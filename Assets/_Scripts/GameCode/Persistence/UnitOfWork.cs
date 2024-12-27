using GameCode.Persistence.Repositories;
using UnityEngine;

public class UnitOfWork : MonoBehaviour
{
    [SerializeField] private DataContext _dataContext;

    [SerializeField] private Shops _shops;
    
    
    public Shops Shops => _shops;
    
    
    public async void Save() => await _dataContext.Save();
    
}

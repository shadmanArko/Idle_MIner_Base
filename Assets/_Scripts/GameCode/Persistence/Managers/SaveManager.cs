using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private UnitOfWork _unitOfWork;

    private void Start()
    {
        InvokeRepeating(nameof(SaveData), 5.0f, 5.0f);
    }

    private void SaveData() => _unitOfWork.Save();

    // public void Dispose()
    // {
    //     SaveData();
    // }
    //
    // private void OnDestroy()
    // {
    //     SaveData();
    // }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCode.Persistence;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class DataContext : MonoBehaviour
{
    public SaveData saveData = new SaveData();
    
    public abstract Task Load();
    public abstract Task Save();

    public List<T> Set<T>()
    {
        // if (typeof(T) == typeof(Player))
        // {
        //     return playerData.players as List<T>;
        // }
        //
        // if (typeof(T) == typeof(Temple))
        // {
        //     return gameData.temples as List<T>;
        // }
        
        return null;
    }
}

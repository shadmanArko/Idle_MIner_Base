using UnityEngine;
using UnityEngine.Serialization;

public class MineSelectionButton : MonoBehaviour
{
    [FormerlySerializedAs("mineName")] public string mineId;

    public void SetCurrentMine()
    {
        PlayerPrefs.SetString("CurrentMine", mineId);
        PlayerPrefs.Save();
        
        Debug.Log(PlayerPrefs.GetString("CurrentMine"));
    }
}

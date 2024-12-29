using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void RestartCurrentScene()
    {
        RestartCurrentSceneAsync();
    }

    private async void RestartCurrentSceneAsync()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var operation = SceneManager.LoadSceneAsync(currentScene);
    
        // Optional: You can monitor the loading progress
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log($"Loading progress: {progress * 100}%");
            await System.Threading.Tasks.Task.Yield();
        }
    }
}

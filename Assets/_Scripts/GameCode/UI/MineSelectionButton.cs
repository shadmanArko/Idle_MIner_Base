using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class MineSelectionButton : MonoBehaviour
{
    [SerializeField]private Button _button;
    
    public Button Button => _button;
   
    public async UniTask SetCurrentMine(string mineId)
    {
        PlayerPrefsManager.CurrentMineId = mineId;
    }
    
    public async UniTask RestartCurrentSceneAsync()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var operation = SceneManager.LoadSceneAsync(currentScene);
        
        // It's optional for loading progress view
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log($"Loading progress: {progress * 100}%");
            await System.Threading.Tasks.Task.Yield();
        }
    }
}

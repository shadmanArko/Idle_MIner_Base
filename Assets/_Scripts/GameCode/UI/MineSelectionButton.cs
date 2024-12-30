using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MineSelectionButton : MonoBehaviour
{
    private Button _button;
    //[SerializeField] private ButtonMoveAnimator _buttonMoveAnimator;
    
    public Button Button => _button;
   // public ButtonMoveAnimator ButtonMoveAnimator => _buttonMoveAnimator;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        //_buttonMoveAnimator = GetComponent<ButtonMoveAnimator>();
    }

    public async UniTask SetCurrentMine(string mineId)
    {
        
        PlayerPrefsManager.CurrentMineId = mineId;
        
    }
    
    public async UniTask RestartCurrentSceneAsync()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var operation = SceneManager.LoadSceneAsync(currentScene);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log($"Loading progress: {progress * 100}%");
            await System.Threading.Tasks.Task.Yield();
        }
    }
}

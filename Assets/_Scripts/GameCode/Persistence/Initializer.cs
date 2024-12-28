using System.IO;
using System.Threading.Tasks;
using GameCode.Persistence.Repositories;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCode.Persistence
{
    public class Initializer : MonoBehaviour
    {
        public DataContext context;

        [Header("Repositories")] 
        public Mines mines;
        
        [Header("Other Dependencies")] 
        public TextAsset saveDataJsonFile;

        private static string SaveDataFilePath => $"{Application.persistentDataPath}/Saves/save_data.json";

        private async void Start()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            Debug.Log(Application.persistentDataPath);
            var rootSaveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(rootSaveDirectory))
            {
                Directory.CreateDirectory(rootSaveDirectory);
            }
            
            if (!File.Exists(SaveDataFilePath))
            {
                await BootstrapSaveData();
            }
            
            await context.Load();

            BindContexts();
        }

        private void BindContexts()
        {
            mines.context = context;
        }

        private async Task BootstrapSaveData()
        {
            using var writer = new StreamWriter(SaveDataFilePath);
            await writer.WriteAsync(saveDataJsonFile.text);
        }
    }
}
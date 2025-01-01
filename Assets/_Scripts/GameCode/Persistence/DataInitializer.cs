using System.IO;
using System.Threading.Tasks;
using GameCode.Persistence.Repositories;
using UnityEngine;

namespace GameCode.Persistence
{
    public class DataInitializer
    {
        private readonly DataContext _context;

        private readonly Mines _mines;

        private readonly TextAsset _saveDataJsonFile;

        private static string SaveDataFilePath => $"{Application.persistentDataPath}/Saves/save_data.json";
        
        public DataInitializer(DataContext context, Mines mines, TextAsset saveDataJsonFile)
        {
            _context = context;
            _mines = mines;
            _saveDataJsonFile = saveDataJsonFile;
        }
        public async Task LoadDataAsync()
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
            
            await _context.Load();

            BindContexts();
        }

        private void BindContexts()
        {
            _mines.context = _context;
        }

        private async Task BootstrapSaveData()
        {
            using var writer = new StreamWriter(SaveDataFilePath);
            await writer.WriteAsync(_saveDataJsonFile.text);
        }
    }
}
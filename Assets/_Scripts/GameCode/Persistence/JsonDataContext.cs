using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCode.Persistence
{
    public class JsonDataContext : DataContext
    {
        public override async Task Load()
        {
            if (!File.Exists(GameDataFilePath))
            {
                return;
            }
            
            using var gameDataFileReader = new StreamReader(GameDataFilePath);
            var gameDataJson = await gameDataFileReader.ReadToEndAsync();
            // Debug.Log(gameDataJson);
            JsonUtility.FromJsonOverwrite(gameDataJson, gameData);
        }

        public override async Task Save()
        {
            var json = JsonUtility.ToJson(gameData);
            using var writer = new StreamWriter(GameDataFilePath);
            await writer.WriteAsync(json);
        }
        
        private string GameDataFilePath => $"{Application.persistentDataPath}/game_data.json";
    }
}
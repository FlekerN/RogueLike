using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class PersistenceManager 
{
    private const string Extension = "*.rgl";

    [Serializable]
    public class SaveSlotInfo
    {
        public string fileName;      
        public string fullPath;      
        public DateTime lastWrite;   
        public PlayerStats preview;  
    }

    public static List<SaveSlotInfo> GetAllSaves(bool loadPreviewData = true)
    {
        var result = new List<SaveSlotInfo>();
        string dir = Application.persistentDataPath;

        if (!Directory.Exists(dir))
            return result;

        string[] files = Directory.GetFiles(dir, Extension, SearchOption.TopDirectoryOnly);

        foreach (var path in files)
        {
            var info = new FileInfo(path);

            var slot = new SaveSlotInfo
            {
                fullPath = path,
                fileName = Path.GetFileNameWithoutExtension(path),
                lastWrite = info.LastWriteTime
            };

            if (loadPreviewData)
            {
                try
                {
                    string json = File.ReadAllText(path);
                    slot.preview = JsonUtility.FromJson<PlayerStats>(json);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"No se pudo leer save {path}: {e.Message}");
                    // Puedes decidir si lo saltas o lo dejas sin preview
                }
            }

            result.Add(slot);
        }

        // Ordena por el más reciente primero (opcional)
        result.Sort((a, b) => b.lastWrite.CompareTo(a.lastWrite));

        return result;
    }
    public static PlayerStats LoadCharacter(string filename)
    {
        PlayerStats stats = null;
        string ruta = Path.Combine(Application.persistentDataPath, filename);

        if (File.Exists(ruta))
        {
            string contenidoJSON = File.ReadAllText(ruta);

            stats = JsonUtility.FromJson<PlayerStats>(contenidoJSON);
            Debug.Log("Datos cargados: " + stats.nombre);
            return stats;
        }
        else
        {
            Debug.Log("No se encontró el archivo de datos.");
            return null;
        }
    }
    public static void SavePlayerData(PlayerStats playerData, string filename)
    {

        string json = JsonUtility.ToJson(playerData);
        string saveName = Path.Combine(Application.persistentDataPath, filename + ".rgl");

        File.WriteAllText(saveName, json);

    }
}

using System.IO;
using UnityEngine;

public static class PersistenceManager 
{
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

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public static class PersistenceManager 
{
    private const string GlobalPlayerExtension = "*.rgl";
    private const string PlayerExtension = ".rgl";
    private const string AchievementsExtension = ".ach.json";
    private const string ScoreboardFileName = "scoreboard.json";
    private const int MAX_SCORES = 5;
    
    [Serializable]
    public class SaveSlotInfo
    {
        public string fileName;      
        public string fullPath;      
        public DateTime lastWrite;   
        public PlayerStats preview;  
    }
    [Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int levelReached;
        public long timestamp;

        public ScoreEntry(string name, int level)
        {
            playerName = name;
            levelReached = level;
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
    [Serializable]
    public class ScoreboardData
    {
        public List<ScoreEntry> topScores = new List<ScoreEntry>();
    }

    [Serializable]
    public class AchievementSaveData
    {
        public List<Achievment> logros;
    }

    public static List<SaveSlotInfo> GetAllSaves(bool loadPreviewData = true)
    {
        var result = new List<SaveSlotInfo>();
        string dir = Application.persistentDataPath;

        if (!Directory.Exists(dir))
            return result;

        string[] files = Directory.GetFiles(dir, GlobalPlayerExtension, SearchOption.TopDirectoryOnly);

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

        // Ordena por el más reciente primero 
        result.Sort((a, b) => b.lastWrite.CompareTo(a.lastWrite));

        return result;
    }
    public static PlayerStats LoadCharacter(string saveNameNoExt)
    {
        string ruta = Path.Combine(Application.persistentDataPath, saveNameNoExt + PlayerExtension);
        if (!File.Exists(ruta)) return null;

        string json = File.ReadAllText(ruta);
        return JsonUtility.FromJson<PlayerStats>(json);
    }
    public static void SavePlayerData(PlayerStats playerData, string filename)
    {
        string json = JsonUtility.ToJson(playerData);
        string saveName = Path.Combine(Application.persistentDataPath, filename + PlayerExtension);

        File.WriteAllText(saveName, json);
    }
    private static string GetAchievementsPath(string playerName)
    {
        return Path.Combine(Application.persistentDataPath, playerName + AchievementsExtension);
    }
    public static void SaveAchievements(List<Achievment> logros)
    {
        var data = new AchievementSaveData
        {
            logros = logros
        };

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(GetAchievementsPath(SessionManager.Instance.PlayerData.nombre), json);
    }
    public static List<Achievment> LoadAchievements(string playerName)
    {
        string path = GetAchievementsPath(playerName);

        if (!File.Exists(path))
            return null; 

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<AchievementSaveData>(json);
            return data?.logros;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No se pudieron cargar logros de {playerName}: {e.Message}");
            return null;
        }
    }

    private static string GetScoreboardPath()
    {
        return Path.Combine(Application.persistentDataPath, ScoreboardFileName);
    }
    public static ScoreboardData LoadScoreboard()
    {
        string path = GetScoreboardPath();

        if (!File.Exists(path))
            return new ScoreboardData();

        try
        {
            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
                return new ScoreboardData();

            var data = JsonUtility.FromJson<ScoreboardData>(json);
            return data ?? new ScoreboardData();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No se pudo cargar scoreboard {path}: {e.Message}");
            return new ScoreboardData();
        }
    }
    public static void AddScore(string playerName, int levelReached)
    {
        var data = LoadScoreboard();

        playerName = string.IsNullOrWhiteSpace(playerName) ? "Jugador" : playerName.Trim();

        data.topScores.Add(new ScoreEntry(playerName, levelReached));

        data.topScores = data.topScores
            .GroupBy(s => s.playerName.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(x => x.levelReached).ThenByDescending(x => x.timestamp).First())
            .OrderByDescending(s => s.levelReached)
            .ThenByDescending(s => s.timestamp)
            .Take(MAX_SCORES)
            .ToList();

        string path = GetScoreboardPath();
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(path, json);
    }
}


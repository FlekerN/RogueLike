using UnityEngine;
using System.Collections.Generic;
public class SessionManager : MonoBehaviour
{
    private static SessionManager _instance;
    public PlayerStats PlayerData { get; private set; }
    public AchievementManger AchievementManager { get; private set; }
    public List<Achievment> Achievements { get; private set; }
     public List<Item> ShopItems { get; private set; }
    public int RunLevelsCompleted { get; set; }
    public int RunTurns { get; set; }
    public int RunEXP { get; set; }

    public void SetPlayerData(PlayerStats data)
    {
        PlayerData = data;
        Debug.Log($"PlayerData guardado. STR={PlayerData.fuerza}/{PlayerData.damage}, VIT={PlayerData.resistencia}/{PlayerData.vidaMaxima}");
    }
    public void SetShopItems(List<Item> items)
    {
        ShopItems = items;
    }
    public void SetAchievements(List<Achievment> achievements)
    {
        Achievements = achievements;
        AchievementManager.Logros = achievements;
    }
    public static SessionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SessionManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SessionManager");
                    _instance = singletonObject.AddComponent<SessionManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void LogMessage(string message)
    {
        Debug.Log(message);
    }

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        AchievementManager = FindFirstObjectByType<AchievementManger>();
    }
}

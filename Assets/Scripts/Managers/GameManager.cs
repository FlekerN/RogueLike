using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.IO;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private Label m_ExpLabel;
    private Label m_PlayerLabel;
    public string m_PlayerName;
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager { get; private set;}
    private int m_FoodAmount;
    public int m_MaxFoodAmount;
    private int m_EXPAmount;
    private int m_CurrentLevel = 1;
    private int CheckSizeMap = 5;
    [Header("Shop UI Toolkit")]
    public UIDocument ShopUIDoc; 
    private VisualElement m_ShopRoot;
    private Button m_ShopExitButton;

private bool m_ShopOpen = false;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("foodLabel");
        m_ExpLabel = UIDoc.rootVisualElement.Q<Label>("xpLabel");
        m_PlayerLabel = UIDoc.rootVisualElement.Q<Label>("playerValueLabel");

        InitShopUI();
        StartNewGame();
    }
    void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    public void ChangeFood(int amount)
    {
    
        m_FoodAmount += amount;

        if (m_FoodAmount > m_MaxFoodAmount)
            m_FoodAmount = m_MaxFoodAmount;

        m_FoodLabel.text = m_FoodAmount.ToString();

        if (m_FoodAmount <= 0)
        {
            SaveRunResults();
            SaveScoreResult();

            SceneManager.LoadScene("EndScreen");
        }
    }
    public void ChangeEXP(int amount)
    {
        m_EXPAmount += amount;
        m_ExpLabel.text = m_EXPAmount.ToString();
        SessionManager.Instance.PlayerData.experiencia = m_EXPAmount;
    }
    public void NewLevel()
    {
        BoardManager.Clean();
        if (m_CurrentLevel >= CheckSizeMap)
        {
            CheckSizeMap += 5;
            BoardManager.Width +=3;
            BoardManager.Height +=3;
        }
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

        SessionManager.Instance.PlayerData.experiencia = m_EXPAmount;
        SessionManager.Instance.PlayerData.vidaMaxima = m_FoodAmount;
        SessionManager.Instance.PlayerData.CurrentLevel = m_CurrentLevel;

        SaveScoreResult();
        PersistenceManager.SavePlayerData(SessionManager.Instance.PlayerData, SessionManager.Instance.PlayerData.nombre);

        m_CurrentLevel++;
    }
    public void StartNewGame()
    {
        m_PlayerName = SessionManager.Instance.PlayerData.nombre;
        m_FoodAmount = SessionManager.Instance.PlayerData.vidaMaxima;
        m_EXPAmount = SessionManager.Instance.PlayerData.experiencia;
        m_CurrentLevel = SessionManager.Instance.PlayerData.CurrentLevel;
        m_MaxFoodAmount = SessionManager.Instance.PlayerData.vidaMaxima;
        
        m_FoodLabel.text = m_FoodAmount.ToString();
        m_ExpLabel.text = m_EXPAmount.ToString();
        m_PlayerLabel.text = m_PlayerName;

        BoardManager.Clean();
        BoardManager.Init();
     
        SaveScoreResult();
        PersistenceManager.SavePlayerData(SessionManager.Instance.PlayerData, SessionManager.Instance.PlayerData.nombre);

        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }
    void SaveRunResults()
    {
        var session = SessionManager.Instance;

        session.RunLevelsCompleted = m_CurrentLevel - 1; 
        session.RunTurns = TurnManager.TurnCount;
        session.RunEXP = m_EXPAmount;
    }
    public void SaveScoreResult()
    {
        string playerName = SessionManager.Instance.PlayerData.nombre; 
        int levelReached = SessionManager.Instance.PlayerData.CurrentLevel; 

        PersistenceManager.AddScore(playerName, levelReached);
    }
    
    void InitShopUI()
    {
        if (ShopUIDoc == null) return;

        var root = ShopUIDoc.rootVisualElement;

        m_ShopRoot = root.Q<VisualElement>("ShopRoot");
        m_ShopExitButton = root.Q<Button>("BtnShopExit");

        if (m_ShopExitButton != null)
            m_ShopExitButton.clicked += CloseShop;

        if (m_ShopRoot != null)
            m_ShopRoot.style.display = DisplayStyle.None;
    }

    public void OpenShop()
    {
        if (m_ShopOpen) return;
            m_ShopOpen = true;

        if (m_ShopRoot != null)
            m_ShopRoot.style.display = DisplayStyle.Flex;

        if (PlayerController != null)
            PlayerController.enabled = false;

        var shopController = ShopUIDoc.GetComponent<ShopController>();
        if (shopController != null) shopController.Refresh();
    }
    public void CloseShop()
    {
        if (!m_ShopOpen) return;
        m_ShopOpen = false;

        if (m_ShopRoot != null)
            m_ShopRoot.style.display = DisplayStyle.None;

        if (PlayerController != null)
            PlayerController.enabled = true;

    }
}

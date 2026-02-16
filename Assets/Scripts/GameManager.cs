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
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager { get; private set;}
    private int m_FoodAmount;
    private int m_EXPAmount = 0;
    private int m_EXPToReach = 10;
    private int m_CurrentLevel = 1;

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
        m_PlayerName = SessionManager.Instance.PlayerData.nombre;

       // m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
       // m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        
        StartNewGame();
    }
    void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = m_FoodAmount.ToString();

        if (m_FoodAmount <= 0)
        {
            PlayerController.GameOver();
           // m_GameOverPanel.style.visibility = Visibility.Visible;
           // m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels";
        }
    }
    public void ChangeEXP(int amount)
    {
        m_EXPAmount += amount;
        m_ExpLabel.text = m_EXPAmount.ToString();

        if (m_EXPAmount >= m_EXPToReach)
        {
            //sube de nivel
            m_EXPToReach += 5;
        }
    }
    public void NewLevel()
    {
        if (m_CurrentLevel >=5) 
        {

        }
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

        m_CurrentLevel++;
    }
    public void StartNewGame()
    {
       // m_GameOverPanel.style.visibility = Visibility.Hidden;
        
        m_CurrentLevel = 1;
        m_FoodAmount = SessionManager.Instance.PlayerData.vidaMaxima;
        m_FoodLabel.text = m_FoodAmount.ToString();
        m_ExpLabel.text = m_EXPAmount.ToString();
        m_PlayerLabel.text = m_PlayerName;

        BoardManager.Clean();
        BoardManager.Init();

        //PersistenceManager.SavePlayerData(SessionManager.Instance.PlayerData, SessionManager.Instance.PlayerData.nombre);

        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }

}

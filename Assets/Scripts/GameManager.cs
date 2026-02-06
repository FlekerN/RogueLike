using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private Label m_ExpLabel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    private static GameManager _instance;
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager { get; private set;}
    private int m_FoodAmount = 100;
    private int m_EXPAmount = 0;
    private int m_EXPToReach = 10;
    private int m_CurrentLevel = 1;
    public PlayerStats PlayerData { get; private set; }

    public void SetPlayerData(PlayerStats data)
    {
        PlayerData = data?.Clone();
        Debug.Log($"PlayerData guardado. STR={PlayerData.fuerza}, VIT={PlayerData.resistencia}");
    }
    public static GameManager Instance
    {
        get
        {
            // Si la instancia aún no ha sido creada, la creamos
            if (_instance == null)
            {
                Debug.Log("CREANDO GameManager desde Instance() en escena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                // Buscamos una instancia existente en la escena
                _instance = FindFirstObjectByType<GameManager>();
                // Si no existe ninguna, creamos una nueva
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
                // Aseguramos que la instancia no se destruya al cambiar de escena
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    // Método que será llamado para el manejo del puntaje, por ejemplo
    public void LogMessage(string message)
    {
        Debug.Log(message);
    }

    private void Awake()
    {
        // Si ya existe una instancia, destruye el objeto actual
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // Mantener el objeto a través de escenas
        
    }
    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_ExpLabel = UIDoc.rootVisualElement.Q<Label>("EXPLabel");

        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        
        StartNewGame();
    }
    void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        if (m_FoodAmount <= 0)
        {
            PlayerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels";
        }
    }
    public void ChangeEXP(int amount)
    {
        m_EXPAmount += amount;
        m_ExpLabel.text = "Experience: " + m_EXPAmount;

        if (m_EXPAmount >= m_EXPToReach)
        {
            //sube de nivel
            m_EXPToReach += 5;
        }
    }
    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

        m_CurrentLevel++;
    }
    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        
        m_CurrentLevel = 1;
        m_FoodAmount = 20;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        
        BoardManager.Clean();
        BoardManager.Init();
        
        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }
}

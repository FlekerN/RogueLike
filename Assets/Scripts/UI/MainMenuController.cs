using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names (opcional)")]
    [Tooltip("Escena a cargar cuando se pulsa NEW GAME (si está vacío, solo dispara evento/log).")]
    [SerializeField] private string newGameScene = "";

    [Tooltip("Escena del menú de LOAD GAME (si está vacío, solo dispara evento/log).")]
    [SerializeField] private string loadGameScene = "";

    [Tooltip("Escena del menú de SETTINGS (si está vacío, solo dispara evento/log).")]
    [SerializeField] private string settingsScene = "";

    // Referencias UI Toolkit
    private UIDocument _doc;
    private VisualElement _root;

    public GameObject settingsMenu;
    public GameObject newGame;

    private Button _btnNew;
    private Button _btnLoad;
    private Button _btnSettings;
    private Button _btnQuit;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _root = _doc.rootVisualElement;

        // Buscar por name (recomendado)
        _btnNew = _root.Q<Button>("btnNewGame");
        _btnLoad = _root.Q<Button>("btnLoadGame");
        _btnSettings = _root.Q<Button>("btnSettings");
        _btnQuit = _root.Q<Button>("btnQuit");

        // Fallback si no pusiste name (busca por texto)
        if (_btnNew == null) _btnNew = FindButtonByText("NEW GAME");
        if (_btnLoad == null) _btnLoad = FindButtonByText("LOAD GAME");
        if (_btnSettings == null) _btnSettings = FindButtonByText("SETTINGS");
        if (_btnQuit == null) _btnQuit = FindButtonByText("QUIT");

        HookEvents();
    }

    private void OnDisable()
    {
        UnhookEvents();
    }

    private void HookEvents()
    {
        if (_btnNew != null) _btnNew.clicked += OnNewGameClicked;
        else Debug.LogWarning("[MainMenuController] No se encontró el botón NEW GAME.");

        if (_btnLoad != null) _btnLoad.clicked += OnLoadGameClicked;
        else Debug.LogWarning("[MainMenuController] No se encontró el botón LOAD GAME.");

        if (_btnSettings != null) _btnSettings.clicked += OnSettingsClicked;
        else Debug.LogWarning("[MainMenuController] No se encontró el botón SETTINGS.");

        if (_btnQuit != null) _btnQuit.clicked += OnQuitClicked;
        else Debug.LogWarning("[MainMenuController] No se encontró el botón QUIT.");
    }

    private void UnhookEvents()
    {
        if (_btnNew != null) _btnNew.clicked -= OnNewGameClicked;
        if (_btnLoad != null) _btnLoad.clicked -= OnLoadGameClicked;
        if (_btnSettings != null) _btnSettings.clicked -= OnSettingsClicked;
        if (_btnQuit != null) _btnQuit.clicked -= OnQuitClicked;
    }

    private Button FindButtonByText(string text)
    {
        // Busca cualquier Button cuyo "text" coincida
        foreach (var b in _root.Query<Button>().ToList())
        {
            if (b != null && b.text == text)
                return b;
        }
        return null;
    }

    // --- Handlers ---
    private void OnNewGameClicked()
    {
        Debug.Log("[MainMenuController] NEW GAME");
        if (!string.IsNullOrEmpty(newGameScene))
            SceneManager.LoadScene(newGameScene);
        else
            StartNewGame(); 
    }

    private void OnLoadGameClicked()
    {
        Debug.Log("[MainMenuController] LOAD GAME");
        if (!string.IsNullOrEmpty(loadGameScene))
            SceneManager.LoadScene(loadGameScene);
        else
            OpenLoadGame(); 
    }

    private void OnSettingsClicked()
    {
        Debug.Log("[MainMenuController] SETTINGS");
        if (!string.IsNullOrEmpty(settingsScene))
            SceneManager.LoadScene(settingsScene);
        else
            OpenSettings(); 
    }

    private void OnQuitClicked()
    {
        Debug.Log("[MainMenuController] QUIT");
        QuitGame();
    }

    // --- Acciones (personaliza aquí) ---
    private void StartNewGame()
    {
        SceneManager.LoadScene("CharacterCreator");
    }

    private void OpenLoadGame()
    {
        //abrir panel dentro de la misma UI
    }

    private void OpenSettings()
    {
        
        settingsMenu.SetActive(true);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Blockers")]
    [SerializeField] private MonoBehaviour playerMovementScript; 

    private UIDocument uiDocument;
    private VisualElement pauseRoot;
    private Button btnResume;
    private Button btnMainMenu;

    private bool isPaused = false;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        var root = uiDocument.rootVisualElement;

        pauseRoot = root.Q<VisualElement>("PauseRoot");
        btnResume = root.Q<Button>("BtnResume");
        btnMainMenu = root.Q<Button>("BtnMainMenu");

 
        btnResume.clicked += ResumeGame;
        btnMainMenu.clicked += GoToMainMenu;


        pauseRoot.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseRoot.style.display = DisplayStyle.Flex;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseRoot.style.display = DisplayStyle.None;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
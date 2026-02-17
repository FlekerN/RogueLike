using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

 [RequireComponent(typeof(UIDocument))]
public class EndGameController : MonoBehaviour
{
    private Label _levelsValue;
    private Label _turnsValue;
    private Label _expValue;
    private Button _backButton;

    private void Awake()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        _levelsValue = root.Q<Label>("LevelsValue");
        _turnsValue  = root.Q<Label>("TurnsValue");
        _expValue    = root.Q<Label>("ExpValue");
        _backButton  = root.Q<Button>("BackToMenuButton");

        if (_backButton != null)
            _backButton.clicked += OnBackToMenuClicked;
    }

    private void Start()
    {
        var session = SessionManager.Instance;

        if (_levelsValue != null) _levelsValue.text = session.RunLevelsCompleted.ToString();
        if (_turnsValue  != null) _turnsValue.text  = session.RunTurns.ToString();
        if (_expValue    != null) _expValue.text    = session.RunEXP.ToString();
    }

    private void OnDestroy()
    {
        if (_backButton != null)
            _backButton.clicked -= OnBackToMenuClicked;
    }

    private void OnBackToMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

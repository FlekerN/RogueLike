using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class LoadGameMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Main"; 

    private UIDocument _doc;

    private VisualElement _saveList;
    private Label _selectedSaveLabel;
    private Button _loadBtn;
    private Button _refreshBtn;
    private Button _backBtn;

    private List<PersistenceManager.SaveSlotInfo> _saves = new();
    private int _selectedIndex = -1;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();

        var root = _doc.rootVisualElement;

        _saveList = root.Q<VisualElement>("saveList");
        _selectedSaveLabel = root.Q<Label>("selectedSaveLabel");

        _loadBtn = root.Q<Button>("loadBtn");
        _refreshBtn = root.Q<Button>("refreshBtn");
        _backBtn = root.Q<Button>("backBtn");

        _loadBtn.clicked += OnLoadClicked;
        _refreshBtn.clicked += Refresh;
        _backBtn.clicked += OnBackClicked;
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        _saves = PersistenceManager.GetAllSaves(loadPreviewData: true);
        _selectedIndex = -1;
        _selectedSaveLabel.text = "(none)";
        _loadBtn.SetEnabled(false);

        _saveList.Clear();

        if (_saves.Count == 0)
        {
            var empty = new Label("No saved games found.");
            empty.AddToClassList("hint");
            _saveList.Add(empty);
            return;
        }

        for (int i = 0; i < _saves.Count; i++)
        {
            int index = i;
            var slot = _saves[i];

            var item = BuildSaveItem(slot);
            item.RegisterCallback<ClickEvent>(_ => Select(index));

            _saveList.Add(item);
        }
    }

    private VisualElement BuildSaveItem(PersistenceManager.SaveSlotInfo slot)
    {
        var item = new VisualElement();
        item.AddToClassList("saveItem");

        var name = new Label(slot.fileName);
        name.AddToClassList("saveName");

        string meta = $"Last played: {slot.lastWrite:yyyy-MM-dd HH:mm}";
        if (slot.preview != null)
        {
            // Ajusta estos campos según tu PlayerStats real
            meta += $" | Experiencia: {slot.preview.experiencia}";
        }

        var metaLabel = new Label(meta);
        metaLabel.AddToClassList("saveMeta");

        item.Add(name);
        item.Add(metaLabel);

        return item;
    }

    private void Select(int index)
    {
        _selectedIndex = index;

        // Quitar selección visual anterior
        for (int i = 0; i < _saveList.childCount; i++)
        {
            _saveList.ElementAt(i).RemoveFromClassList("saveItemSelected");
        }

        // Poner selección visual
        _saveList.ElementAt(index).AddToClassList("saveItemSelected");

        _selectedSaveLabel.text = _saves[index].fileName;
        _loadBtn.SetEnabled(true);
    }

    private void OnLoadClicked()
    {
        if (_selectedIndex < 0 || _selectedIndex >= _saves.Count)
            return;

        var selected = _saves[_selectedIndex];

        try
        {
            string json = File.ReadAllText(selected.fullPath);
            var data = JsonUtility.FromJson<PlayerStats>(json);

            SessionManager.Instance.SetPlayerData(data);

            SceneManager.LoadScene(gameSceneName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load save: {selected.fullPath}\n{e}");
        }
    }

    private void OnBackClicked()
    {
        gameObject.SetActive(false);
    }


}

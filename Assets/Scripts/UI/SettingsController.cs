using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
public class SettingsController : MonoBehaviour
{
    public UIDocument uiDocument;

    // Referencias a elementos visuales
    private VisualElement m_Root;
    private DropdownField m_DisplayDropdown;
    private SliderInt m_MusicSlider;
    private SliderInt m_SfxSlider;
    private Button m_BtnClose;

    // Constantes para guardar datos (PlayerPrefs)
    private const string PREF_FULLSCREEN = "Pref_Fullscreen";
    private const string PREF_MUSIC = "Pref_MusicVol";
    private const string PREF_SFX = "Pref_SFXVol";
    void OnEnable()
    {

        if (uiDocument == null) GetComponent<UIDocument>();
        m_Root = uiDocument.rootVisualElement;
        // 1. BUSCAR ELEMENTOS POR NOMBRE (Deben coincidir con el UXML)
        m_DisplayDropdown = m_Root.Q<DropdownField>("DropdownDisplay");
        m_MusicSlider = m_Root.Q<SliderInt>("SliderMusic");
        m_SfxSlider = m_Root.Q<SliderInt>("SliderSFX");
        m_BtnClose = m_Root.Q<Button>("BtnClose");

        // 2. CARGAR VALORES GUARDADOS O POR DEFECTO
        LoadSettings();

        // 3. REGISTRAR EVENTOS (Callbacks)
        // Se ejecuta cada vez que el valor cambia
        m_DisplayDropdown.RegisterValueChangedCallback(evt =>
        OnDisplayModeChanged(evt.newValue));
        m_MusicSlider.RegisterValueChangedCallback(evt =>
        OnMusicVolumeChanged(evt.newValue));
        m_SfxSlider.RegisterValueChangedCallback(evt =>
        OnSfxVolumeChanged(evt.newValue));
        m_BtnClose.clicked += OnCloseClicked;
    }
    private void LoadSettings()
    {
        // -- Cargar Pantalla --

        // 1 = Fullscreen, 0 = Windowed. Por defecto 1.
        int fullScreenInt =
        PlayerPrefs.GetInt(PREF_FULLSCREEN, 1);
        bool isFull = fullScreenInt == 1;

        // Sincronizar UI
        m_DisplayDropdown.index = isFull ? 0 : 1; // 0 es"Fullscreen" en la lista, 1 es "Windowed"
        Screen.fullScreen = isFull;

        // -- Cargar Audio --
        int musicVol = PlayerPrefs.GetInt(PREF_MUSIC, 50);
        int sfxVol = PlayerPrefs.GetInt(PREF_SFX, 80);
        m_MusicSlider.value = musicVol;
        m_SfxSlider.value = sfxVol;
    }

    // --- LOGICA DE CAMBIOS ---
    private void OnDisplayModeChanged(string newValue)
    {
        if (newValue == "Fullscreen")
        {
            Screen.fullScreenMode =
            FullScreenMode.ExclusiveFullScreen;
            PlayerPrefs.SetInt(PREF_FULLSCREEN, 1);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt(PREF_FULLSCREEN, 0);
        }
        Debug.Log($"Modo de pantalla cambiado a: {newValue}");
    }
    private void OnMusicVolumeChanged(int newValue)
    {
        // Aquí conectarías con tu AudioManager, ej:
        // Por ahora simulamos guardando el dato
        PlayerPrefs.SetInt(PREF_MUSIC, newValue);
        Debug.Log($"Volumen Música: {newValue}%");
    }
    private void OnSfxVolumeChanged(int newValue)
    {
        // Aquí conectarías con tu AudioManager
        PlayerPrefs.SetInt(PREF_SFX, newValue);
        Debug.Log($"Volumen SFX: {newValue}%");
    }
    private void OnCloseClicked()
    {
        PlayerPrefs.Save(); // Aseguramos el guardado en disco
                            // Ocultar el menú (o desactivar el objeto)
        gameObject.SetActive(false);
        // Opcional: Reanudar el tiempo si estaba pausado
        // Time.timeScale = 1f;
    }
}

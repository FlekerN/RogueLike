using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerCreator : MonoBehaviour
{
    [Header("UI")]
    public UIDocument UI;

    private VisualElement root;

    private PlayerStats playerData;

    private TextField tfName;

    public enum ATTRIBUTES
    {
        fuerza,
        resistencia,
        supervivencia,
        inteligencia
    }

 
    private void Start()
    {
        playerData = NuevoPersonaje();
        root = UI.rootVisualElement;

        BindUI();
        ActualizarUI();
    }

    public PlayerStats NuevoPersonaje()
    {

        PlayerStats nuevo = new PlayerStats();
        nuevo.puntosDisponibles = 10;

        nuevo.nombre = "SinNombre";
        return nuevo;
    }

    private void BindUI()
    {
        tfName = root.Q<TextField>("tfName");
        if (tfName != null)
        {
            // Si ya había nombre (por defecto o cargado), lo reflejamos en UI
            tfName.value = playerData.nombre ?? "";

            // Cada vez que cambie el texto, lo guardamos en playerData
            tfName.RegisterValueChangedCallback(evt =>
            {
                playerData.nombre = SanitizarNombre(evt.newValue);
            });
        }
        // Botones +
        root.Q<Button>("btnStrPlus").clicked += () => AsignarPunto(ATTRIBUTES.fuerza);
        root.Q<Button>("btnVitPlus").clicked += () => AsignarPunto(ATTRIBUTES.resistencia);
        root.Q<Button>("btnDexPlus").clicked += () => AsignarPunto(ATTRIBUTES.supervivencia);
        root.Q<Button>("btnIntPlus").clicked += () => AsignarPunto(ATTRIBUTES.inteligencia);

        // Botones -
        root.Q<Button>("btnStrMinus").clicked += () => QuitarPunto(ATTRIBUTES.fuerza);
        root.Q<Button>("btnVitMinus").clicked += () => QuitarPunto(ATTRIBUTES.resistencia);
        root.Q<Button>("btnDexMinus").clicked += () => QuitarPunto(ATTRIBUTES.supervivencia);
        root.Q<Button>("btnIntMinus").clicked += () => QuitarPunto(ATTRIBUTES.inteligencia);

        root.Q<Button>("btnBack").clicked += () =>VolverMenuPrincipal();
        root.Q<Button>("btnConfirm").clicked += () =>AsignarAtributos();
    }
    private void AsignarAtributos()
    {
        //asegura que el último texto del campo se guarde (por si no disparó callback)
        if (tfName != null)
            playerData.nombre = SanitizarNombre(tfName.value);

        //evitar nombres vacíos
        if (string.IsNullOrWhiteSpace(playerData.nombre))
        {
            playerData.nombre = "SinNombre";
            if (tfName != null) tfName.value = playerData.nombre;
        }
        
        playerData.RecalcularStats();
        
        SessionManager.Instance.SetPlayerData(playerData);
        SceneManager.LoadScene("Main");

    }
    private void VolverMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private static string SanitizarNombre(string input)
    {
        if (input == null) return "";

        // Recorta y colapsa espacios múltiples
        var trimmed = input.Trim();
        while (trimmed.Contains("  "))
            trimmed = trimmed.Replace("  ", " ");

        // Opcional: limitar longitud
        const int maxLen = 16;
        if (trimmed.Length > maxLen)
            trimmed = trimmed.Substring(0, maxLen);

        return trimmed;
    }

    // ---------- LÓGICA ----------
    private void AsignarPunto(ATTRIBUTES atributo)
    {
        if (playerData.puntosDisponibles <= 0)
            return;

        playerData.puntosDisponibles--;

        switch (atributo)
        {
            case ATTRIBUTES.fuerza:
                playerData.fuerza++;
                break;
            case ATTRIBUTES.resistencia:
                playerData.resistencia++;
                break;
            case ATTRIBUTES.supervivencia:
                playerData.supervivencia++;
                break;
            case ATTRIBUTES.inteligencia:
                playerData.inteligencia++;
                break;
        }

        ActualizarUI();
    }

    private void QuitarPunto(ATTRIBUTES atributo)
    {
        if (GetAtributo(atributo) <= 1)
            return;

        playerData.puntosDisponibles++;

        switch (atributo)
        {
            case ATTRIBUTES.fuerza:
                playerData.fuerza--;
                break;
            case ATTRIBUTES.resistencia:
                playerData.resistencia--;
                break;
            case ATTRIBUTES.supervivencia:
                playerData.supervivencia--;
                break;
            case ATTRIBUTES.inteligencia:
                playerData.inteligencia--;
                break;
        }

        ActualizarUI();
    }

    private int GetAtributo(ATTRIBUTES atributo)
    {
        return atributo switch
        {
            ATTRIBUTES.fuerza => playerData.fuerza,
            ATTRIBUTES.resistencia => playerData.resistencia,
            ATTRIBUTES.supervivencia => playerData.supervivencia,
            ATTRIBUTES.inteligencia => playerData.inteligencia,
            _ => 0
        };
    }


    private void ActualizarUI()
    {
        playerData.RecalcularStats();

        root.Q<Label>("lblStr").text = playerData.fuerza.ToString();
        root.Q<Label>("lblVit").text = playerData.resistencia.ToString();
        root.Q<Label>("lblDex").text = playerData.supervivencia.ToString();
        root.Q<Label>("lblInt").text = playerData.inteligencia.ToString();

        root.Q<Label>("lblPoints").text = playerData.puntosDisponibles.ToString();

        root.Q<Label>("lblHP").text = playerData.vidaMaxima.ToString();
        root.Q<Label>("lblDMG").text = (9 + playerData.fuerza).ToString();
        root.Q<Label>("lblDEF").text = playerData.recoleccion.ToString();
        root.Q<Label>("lblCRIT").text = $"{1 + playerData.supervivencia * 0.5f:0.#}%";
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCreator : MonoBehaviour
{
    [Header("UI")]
    public UIDocument UI;

    private VisualElement root;

    private PlayerStats playerData;

    public enum ATTRIBUTES
    {
        fuerza,
        resistencia,
        destreza,
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
        return nuevo;
    }

    private void BindUI()
    {
        // Botones +
        root.Q<Button>("btnStrPlus").clicked += () => AsignarPunto(ATTRIBUTES.fuerza);
        root.Q<Button>("btnVitPlus").clicked += () => AsignarPunto(ATTRIBUTES.resistencia);
        root.Q<Button>("btnDexPlus").clicked += () => AsignarPunto(ATTRIBUTES.destreza);
        root.Q<Button>("btnIntPlus").clicked += () => AsignarPunto(ATTRIBUTES.inteligencia);

        // Botones -
        root.Q<Button>("btnStrMinus").clicked += () => QuitarPunto(ATTRIBUTES.fuerza);
        root.Q<Button>("btnVitMinus").clicked += () => QuitarPunto(ATTRIBUTES.resistencia);
        root.Q<Button>("btnDexMinus").clicked += () => QuitarPunto(ATTRIBUTES.destreza);
        root.Q<Button>("btnIntMinus").clicked += () => QuitarPunto(ATTRIBUTES.inteligencia);
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
            case ATTRIBUTES.destreza:
                playerData.destreza++;
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
            case ATTRIBUTES.destreza:
                playerData.destreza--;
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
            ATTRIBUTES.destreza => playerData.destreza,
            ATTRIBUTES.inteligencia => playerData.inteligencia,
            _ => 0
        };
    }


    private void ActualizarUI()
    {
        root.Q<Label>("lblStr").text = playerData.fuerza.ToString();
        root.Q<Label>("lblVit").text = playerData.resistencia.ToString();
        root.Q<Label>("lblDex").text = playerData.destreza.ToString();
        root.Q<Label>("lblInt").text = playerData.inteligencia.ToString();

        root.Q<Label>("lblPoints").text = playerData.puntosDisponibles.ToString();

        root.Q<Label>("lblHP").text = (80 + playerData.resistencia * 20).ToString();
        root.Q<Label>("lblDMG").text = (5 + playerData.fuerza * 3).ToString();
        root.Q<Label>("lblDEF").text = (playerData.resistencia * 2 + playerData.destreza).ToString();
        root.Q<Label>("lblCRIT").text = $"{1 + playerData.destreza * 0.5f:0.#}%";
        root.Q<Label>("lblAS").text = $"{1f + playerData.destreza * 0.02f:0.00}";
    }
}

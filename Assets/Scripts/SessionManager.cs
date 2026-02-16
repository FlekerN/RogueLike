using UnityEngine;

public class SessionManager : MonoBehaviour
{
    private static SessionManager _instance;
    public PlayerStats PlayerData { get; private set; }


    public void SetPlayerData(PlayerStats data)
    {
        PlayerData = data?.Clone();
        Debug.Log($"PlayerData guardado. STR={PlayerData.fuerza}/{PlayerData.damage}, VIT={PlayerData.resistencia}/{PlayerData.vidaMaxima}");
    }

    // Propiedad pública para acceder a la instancia
    public static SessionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SessionManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SessionManager");
                    _instance = singletonObject.AddComponent<SessionManager>();
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

    // Método Awake que asegura que la instancia esté configurada al principio
    private void Awake()
    {
        // Si ya existe una instancia, destruye el objeto actual
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el objeto a través de escenas
        }
    }
}

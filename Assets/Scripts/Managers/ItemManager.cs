using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    private readonly List<Item> _baseItems = new List<Item>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildBaseItems();
        EnsureSessionCopy();
    }

    private void BuildBaseItems()
    {
        _baseItems.Clear();

        // IDs: fuerza, resistencia, supervivencia (tal como pediste)
        _baseItems.Add(new Item(
            iD: "fuerza",
            name: "Guantes de Chatarrero",
            description: "Aumenta tu Fuerza en +1.",
            price: 5,
            stat: StatType.Fuerza
        ));
        _baseItems.Add(new Item(
            iD: "fuerza",
            name: "Mazo Oxidado",
            description: "Aumenta tu Fuerza en +1.",
            price: 5,
            stat: StatType.Fuerza
        ));
        _baseItems.Add(new Item(
            iD: "fuerza",
            name: "Mutación Muscular Nivel I",
            description: "Aumenta tu Fuerza en +1.",
            price: 5,
            stat: StatType.Fuerza
        ));
        _baseItems.Add(new Item(
            iD: "fuerza",
            name: "Amplificador de Impacto",
            description: "Aumenta tu Fuerza en +1.",
            price: 5,
            stat: StatType.Fuerza
        ));

        _baseItems.Add(new Item(
            iD: "resistencia",
            name: "Inyección Berserker",
            description: "Aumenta tu Resistencia en +1.",
            price: 5,
            stat: StatType.Resistencia
        ));

        _baseItems.Add(new Item(
            iD: "resistencia",
            name: "Refuerzo Óseo Experimental",
            description: "Aumenta tu Resistencia en +1.",
            price: 5,
            stat: StatType.Resistencia
        ));

        _baseItems.Add(new Item(
            iD: "resistencia",
            name: "Chaleco de Placas Improvisado",
            description: "Aumenta tu Resistencia en +1.",
            price: 5,
            stat: StatType.Resistencia
        ));
        
        _baseItems.Add(new Item(
            iD: "resistencia",
            name: "Vacuna Antirrad",
            description: "Aumenta tu Resistencia en +1.",
            price: 5,
            stat: StatType.Resistencia
        ));

        _baseItems.Add(new Item(
            iD: "supervivencia",
            name: "Kit de Rastreador",
            description: "Aumenta tu Supervivencia en +1.",
            price: 5,
            stat: StatType.Supervivencia
        ));

        _baseItems.Add(new Item(
            iD: "supervivencia",
            name: "Purificador Portátil",
            description: "Aumenta tu Supervivencia en +1.",
            price: 5,
            stat: StatType.Supervivencia
        ));
        
        _baseItems.Add(new Item(
            iD: "supervivencia",
            name: "Sensor de Movimiento Casero",
            description: "Aumenta tu Supervivencia en +1.",
            price: 5,
            stat: StatType.Supervivencia
        ));
        
        _baseItems.Add(new Item(
            iD: "supervivencia",
            name: "Detector de Recursos",
            description: "Aumenta tu Supervivencia en +1.",
            price: 5,
            stat: StatType.Supervivencia
        ));
    }

    // Copia profunda para que el estado bought sea independiente por sesión
    private void EnsureSessionCopy()
    {
        if (SessionManager.Instance.ShopItems != null && SessionManager.Instance.ShopItems.Count > 0)
            return;

        SessionManager.Instance.SetShopItems(_baseItems);
    }

    public List<Item> GetSessionShopItems()
    {
        EnsureSessionCopy();
        return SessionManager.Instance.ShopItems;
    }
}
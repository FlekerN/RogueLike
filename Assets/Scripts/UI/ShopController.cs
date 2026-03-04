using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ShopController : MonoBehaviour
{

    private UIDocument uiDocument;
    private VisualElement root;
    // UI de items (3 slots)
    private readonly VisualElement[] rows = new VisualElement[3];
    private readonly Label[] nameLabels = new Label[3];
    private readonly Label[] statLabels = new Label[3];
    private readonly Label[] priceLabels = new Label[3];
    private readonly Button[] buyButtons = new Button[3];

    private Label playerGoldLabel;

    private readonly Item[] slotItems = new Item[3];

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        playerGoldLabel = root.Q<Label>("PlayerGoldLabel");

        // --- Bind slots ---
        for (int i = 0; i < 3; i++)
        {
            rows[i] = root.Q<VisualElement>($"ItemRow{i}");
            nameLabels[i] = root.Q<Label>($"ItemName{i}");
            statLabels[i] = root.Q<Label>($"ItemStat{i}");
            priceLabels[i] = root.Q<Label>($"ItemPrice{i}");
            buyButtons[i] = root.Q<Button>($"BuyBtn{i}");

            int captured = i;
            if (buyButtons[i] != null)
                buyButtons[i].clicked += () => OnBuyClicked(captured);
        }
    }

    private void OnDestroy()
    {

        // Desuscribir buys
        for (int i = 0; i < 3; i++)
        {
            if (buyButtons[i] != null)
            {
                int captured = i;
                buyButtons[i].clicked -= () => OnBuyClicked(captured); 
            }
        }
    }

    public void Refresh()
    {
        var items = ItemManager.Instance.GetSessionShopItems();
        var available = items
            .Where(x => x != null && !x.bought)
            .OrderBy(_ => Random.value)
            .ToList();

        UpdateCurrencyLabel();

        for (int i = 0; i < 3; i++)
        {
            if (rows[i] == null) continue;

            if (i < available.Count)
            {
                var it = available[i];
                slotItems[i] = it;

                rows[i].style.display = DisplayStyle.Flex;

                if (nameLabels[i] != null) nameLabels[i].text = it.Name;
                if (statLabels[i] != null) statLabels[i].text = StatToUIText(it);
                if (priceLabels[i] != null) priceLabels[i].text = it.Price.ToString();

                if (buyButtons[i] != null)
                    buyButtons[i].SetEnabled(CanAfford(it.Price));
            }
            else
            {
                slotItems[i] = null;
                rows[i].style.display = DisplayStyle.None;
            }
        }
    }

    private void OnBuyClicked(int slotIndex)
    {
        var item = slotItems[slotIndex];
        if (item == null) return;

        Spend(item.Price);
        item.bought = true;

        SessionManager.Instance.PlayerData.AplicarItem(item);
        if (GameManager.Instance != null)
            GameManager.Instance.m_MaxFoodAmount = SessionManager.Instance.PlayerData.vidaMaxima;

        Refresh();
    }

    private void UpdateCurrencyLabel()
    {
        if (playerGoldLabel == null) return;
        playerGoldLabel.text = $"GEMAS: {SessionManager.Instance.PlayerData.experiencia}";
    }

    private bool CanAfford(int price)
    {
        return SessionManager.Instance.PlayerData.experiencia >= price;
    }

    private void Spend(int price)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeEXP(-price);
            return;
        }
    }

    private string StatToUIText(Item it)
    {
        return it.Stat switch
        {
            StatType.Fuerza => "+Fuerza",
            StatType.Resistencia => "+Resistencia",
            StatType.Supervivencia => "+Supervivencia",
            _ => "+Stat"
        };
    }
}
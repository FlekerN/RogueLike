using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ShopController : MonoBehaviour
{
    private UIDocument _doc;
    private VisualElement _root;

    private VisualElement[] _rows = new VisualElement[3];
    private Label[] _nameLabels = new Label[3];
    private Label[] _statLabels = new Label[3];
    private Label[] _priceLabels = new Label[3];
    private Button[] _buyButtons = new Button[3];

    private Label _playerGoldLabel;
    private Button _exitBtn;

    private Item[] _slotItems = new Item[3];

    private bool _uiBound;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        // IMPORTANTÍSIMO: esperar a que el UIDocument clone el UXML
        _doc.rootVisualElement.schedule.Execute(() =>
        {
            BindUIIfNeeded();
            Refresh();
        }).ExecuteLater(0);
    }

    private void OnDestroy()
    {
        // desuscribir botones
        for (int i = 0; i < 3; i++)
        {
            if (_buyButtons[i] != null)
            {
                int captured = i;
                _buyButtons[i].clicked -= () => OnBuyClicked(captured);
            }
        }

        if (_exitBtn != null)
            _exitBtn.clicked -= OnExitClicked;
    }

    private void BindUIIfNeeded()
    {
        if (_uiBound) return;

        _root = _doc.rootVisualElement;
        if (_root == null)
        {
            Debug.LogError("[ShopController] rootVisualElement null.");
            return;
        }

        // Estos names existen en tu ShopUI.uxml
        _playerGoldLabel = _root.Q<Label>("PlayerGoldLabel");
        _exitBtn = _root.Q<Button>("BtnShopExit");

        if (_exitBtn != null)
            _exitBtn.clicked += OnExitClicked;

        for (int i = 0; i < 3; i++)
        {
            _rows[i] = _root.Q<VisualElement>($"ItemRow{i}");
            _nameLabels[i] = _root.Q<Label>($"ItemName{i}");
            _statLabels[i] = _root.Q<Label>($"ItemStat{i}");
            _priceLabels[i] = _root.Q<Label>($"ItemPrice{i}");
            _buyButtons[i] = _root.Q<Button>($"BuyBtn{i}");

            if (_rows[i] == null) Debug.LogError($"[ShopController] Falta ItemRow{i}");
            if (_nameLabels[i] == null) Debug.LogError($"[ShopController] Falta ItemName{i}");
            if (_statLabels[i] == null) Debug.LogError($"[ShopController] Falta ItemStat{i}");
            if (_priceLabels[i] == null) Debug.LogError($"[ShopController] Falta ItemPrice{i}");
            if (_buyButtons[i] == null) Debug.LogError($"[ShopController] Falta BuyBtn{i}");

            int captured = i;
            if (_buyButtons[i] != null)
                _buyButtons[i].clicked += () => OnBuyClicked(captured);
        }

        _uiBound = true;
    }

    public void Refresh()
    {
        if (!_uiBound)
        {
            BindUIIfNeeded();
            if (!_uiBound) return;
        }

        if (SessionManager.Instance == null || ItemManager.Instance == null)
        {
            Debug.LogError("[ShopController] Falta SessionManager o ItemManager en escena.");
            return;
        }

        // si PlayerData es null, también rompe
        if (SessionManager.Instance.PlayerData == null)
        {
            Debug.LogError("[ShopController] SessionManager.PlayerData es null (no seteaste SetPlayerData?).");
            return;
        }

        var items = ItemManager.Instance.GetSessionShopItems();
        var available = items
            .Where(x => x != null && !x.bought)
            .OrderBy(_ => Random.value)   // shuffle
            .ToList();

        UpdateCurrencyLabel();

        for (int i = 0; i < 3; i++)
        {
            if (_rows[i] == null) continue;

            if (i < available.Count)
            {
                var it = available[i];
                _slotItems[i] = it;

                _rows[i].style.display = DisplayStyle.Flex;

                if (_nameLabels[i] != null) _nameLabels[i].text = it.Name;
                if (_statLabels[i] != null) _statLabels[i].text = StatToUIText(it);
                if (_priceLabels[i] != null) _priceLabels[i].text = it.Price.ToString();

                if (_buyButtons[i] != null)
                    _buyButtons[i].SetEnabled(CanAfford(it.Price));
            }
            else
            {
                _slotItems[i] = null;
                _rows[i].style.display = DisplayStyle.None;
            }
        }
    }

    private void OnBuyClicked(int slotIndex)
    {
        var item = _slotItems[slotIndex];
        if (item == null) return;

        if (!CanAfford(item.Price))
        {
            Refresh();
            return;
        }

        Spend(item.Price);
        item.bought = true;

        SessionManager.Instance.PlayerData.AplicarItem(item);
        GameManager.Instance.m_MaxFoodAmount = SessionManager.Instance.PlayerData.vidaMaxima;
        
        Refresh();
    }

    private void OnExitClicked()
    {
        // tu GameManager ya tiene CloseShop()
        if (GameManager.Instance != null)
            GameManager.Instance.CloseShop();
    }

    private void UpdateCurrencyLabel()
    {
        if (_playerGoldLabel == null) return;

        // usando experiencia como gemas (como estabas haciendo)
        _playerGoldLabel.text = $"GEMAS: {SessionManager.Instance.PlayerData.experiencia}";
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
        
        var p = SessionManager.Instance.PlayerData;

        if (p.experiencia < 0) p.experiencia = 0;
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
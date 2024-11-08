using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform _menuContent;
    [SerializeField] private GameObject _menuBtnPrefab;
    [SerializeField] private Transform _categoryContent;
    private List<ItemData> _allItems = new List<ItemData>(); // ��� ������ ������
    private Dictionary<string, Button> _menuBtns = new Dictionary<string, Button>();
    private Dictionary<string, int> _selectedFoodCount = new Dictionary<string, int>();

    //Bg
    [SerializeField] private Image _bgImage; // Bg ��ü�� Image ������Ʈ
    [SerializeField] private List<Sprite> _bgSprites; // ī�װ����� ����� ��������Ʈ ����Ʈ


    private void Start()
    {
        // Resources�� ��� ItemData �ε�
        LoadAllItems();

        int index = 0;
        foreach (Button categoryButton in _categoryContent.GetComponentsInChildren<Button>())
        {
            int categoryIndex = index;
            categoryButton.onClick.AddListener(() => OnCategoryButtonClick(categoryIndex));
            index++;
        }

        // ó������ 0�� ī�װ� �����۸� �ε�
        LoadItemsByCategory(0);
    }

    private void LoadAllItems()
    {
        // Resources �������� ItemData Ÿ���� ��� ������Ʈ�� �ε�
        ItemData[] items = Resources.LoadAll<ItemData>("ScriptableObject/Item");

        if (items != null && items.Length > 0)
        {
            _allItems.AddRange(items); // ����Ʈ�� ������ �߰�
            Debug.Log($"�ε�� ������ ��: {items.Length}");
        }
    }

    public void OnCategoryButtonClick(int categoryIndex)
    {
        LoadItemsByCategory(categoryIndex);

        // _bgSprites�� ��ȿ�� �˻� �� _bgImage ��������Ʈ ����
        if (_bgSprites != null && categoryIndex < _bgSprites.Count)
        {
            _bgImage.sprite = _bgSprites[categoryIndex]; // ���õ� ī�װ��� �´� ��������Ʈ ����
        }
    }

    public void LoadItemsByCategory(int category)
    {
        // ���� �޴� ��ư�� ����
        foreach (Transform child in _menuContent)
        {
            Destroy(child.gameObject);
        }
        _menuBtns.Clear();

        // ���õ� ī�װ��� �����۸� ��ư���� ����
        foreach (ItemData item in _allItems)
        {
            if ((int)item.itemType == category)
            {
                GameObject newButton = Instantiate(_menuBtnPrefab, _menuContent);

                newButton.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

                Image secondImage = newButton.transform.GetChild(1).GetComponent<Image>();
                secondImage.sprite = item.itemImage;

                // �̹��� ũ�� Ȯ�� �� ���� (�ʺ� �Ǵ� ���̰� 100���� ũ�� ���̱�)
                if (secondImage.sprite.rect.width > 1000 || secondImage.sprite.rect.height > 1000)
                {
                    secondImage.rectTransform.sizeDelta = new Vector2(150, 150); // ������ ũ��
                }

                newButton.GetComponent<SpawnDragItem>().Setup(item);

                Button buttonComponent = newButton.GetComponent<Button>();
                _menuBtns[item.itemName] = buttonComponent;
                buttonComponent.onClick.AddListener(() => OnMenuButtonClick(item.itemName));
            }
        }
    }

    public void OnMenuButtonClick(string foodName)
    {
        if (_menuBtns.TryGetValue(foodName, out Button buttonComponent))
        {
            Debug.Log($"{foodName} ��ư�� Ŭ���Ǿ����ϴ�.");

            if (_selectedFoodCount.ContainsKey(foodName))
            {
                _selectedFoodCount[foodName]++;
            }
            else
            {
                _selectedFoodCount[foodName] = 1;
            }

            foreach (var item in _selectedFoodCount)
            {
                Debug.Log($"{item.Key}�� Ŭ�� Ƚ��: {item.Value}");
            }

           // UIManager.Instance.CheckOrderStatus(_selectedFoodCount);
        }
    }

    public void ResetSelection()
    {
        _selectedFoodCount.Clear();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class POSManager : MonoBehaviour
{
    [SerializeField] private LevelData _currentLevel;
    public LevelData currentLevel { get { return _currentLevel; } }

    [SerializeField] private GameObject _orderPanel;
    [SerializeField] private GameObject _orderItemPrefab;
    [SerializeField] private Button _readyButton;
    public Button ReadyButton => _readyButton;
    [SerializeField] private Button _returnButton;

    private List<OrderItem> _activeOrders = new List<OrderItem>();
    private HashSet<PuzzleData> _usedFoodItems = new HashSet<PuzzleData>();
    private List<RecipeData> _orderRecipeData= new List<RecipeData>();

    private void Start()
    {
        CreateNewOrder();
        _readyButton.onClick.AddListener(OnReadyButtonClicked);
        //_returnButton.onClick.AddListener(OnReturnButtonClicked);
    }

    // ���ο� �ֹ��� ���� // (��ư Ŭ�� ȣ��)
    public void CreateNewOrder()
    {
        // ���� �ֹ��� �ʱ�ȭ
        foreach (Transform child in _orderPanel.transform)
        {
            Destroy(child.gameObject);
        }
        _activeOrders.Clear();
        _usedFoodItems.Clear();
        _orderRecipeData.Clear();

        // ������ PuzzleData ����
        PuzzleData randomPuzzleData;
        do
        {
            randomPuzzleData = _currentLevel.puzzleDatas[Random.Range(0, _currentLevel.puzzleDatas.Length)];
        } while (_usedFoodItems.Contains(randomPuzzleData));

        _usedFoodItems.Add(randomPuzzleData);

        // Dictionary�� ����Ͽ� �ߺ� ������ �� ���� ����
        Dictionary<RecipeData, int> recipeCountDict = new Dictionary<RecipeData, int>();

        // puzzleData���� GridRecipeSizeData�� ��ȸ�ϸ� ������ ����
        foreach (var gridRecipeSizeData in randomPuzzleData.gridRecipeDatas)
        {
            int countToSelect = gridRecipeSizeData.gridRecipeCount;

            for (int i = 0; i < countToSelect; i++)
            {
                // ������ ��Ͽ��� �������� ����
                RecipeData selectedRecipe = gridRecipeSizeData.gridRecipeData.recipeDatas[Random.Range(0, gridRecipeSizeData.gridRecipeData.recipeDatas.Length)];

                // ���� RecipeData�� ������ count�� ������Ű�� ������ �߰�
                if (recipeCountDict.ContainsKey(selectedRecipe))
                {
                    recipeCountDict[selectedRecipe] += 1;
                }
                else
                {
                    recipeCountDict[selectedRecipe] = 1;
                }

                _orderRecipeData.Add(selectedRecipe); //�� ������ ������ �߰�
            }
        }

        // Dictionary�� ����� �� �����Ƿ� �ֹ� �׸� ����
        foreach (var kvp in recipeCountDict)
        {
            RecipeData recipe = kvp.Key;
            int itemCount = kvp.Value;

            GameObject orderItemObject = Instantiate(_orderItemPrefab, _orderPanel.transform);
            OrderItem orderItem = orderItemObject.GetComponent<OrderItem>();
            orderItem.SetupOrder(recipe, itemCount); // �ߺ��� ��� itemCount�� ������ ������ ����
            _activeOrders.Add(orderItem);
        }
    }

    // ���� ��ư Ŭ�� ����/�Ұ��� ���� //
    public void SetReadyButtonInteractable(bool interactable)
    {
        _readyButton.interactable = interactable;
    }

    public void OnReadyButtonClicked()
    {
        CheckOrder(); // �ֹ� �׸� üũ
        bool isOrderMatched = CheckOrderItemList(); // �ֹ� �׸� üũ
        if (isOrderMatched)
        {
            Debug.Log("��� �ֹ��� ��ġ�մϴ�!");
            Debug.Log("���� +1, ���� ����: " );
            GameManager.Instance.OnPackagingSuccess();
        }
        else
        {
            Debug.Log("������ ����ġ");
            GameManager.Instance.PlayWitchAction(CharacterState.Sad);
        }

        CreateNewOrder();

        GridManager.Instance.ClearAllItems();
    }

    private void CheckOrder()
    {
        // ���� ���� ������ ��� ��������
        List<ItemData>[] cellItemData = GridManager.Instance.GetCellItemData();
        List<RecipeData> _orderRecipeDataRemove = new List<RecipeData>();

        // �ֹ��� ������ ������ Ȯ��
        foreach (var recipe in _orderRecipeData)
        {
            // �������� ���� ���� ��Ҹ� ������
            List<ItemData> packagingComponents = recipe.packagingComponents;

            // ��� �������� �߰ߵǾ������� üũ�ϴ� ����
            bool allItemsFound = true;

            // ������ �������� ������ ����Ʈ
            List<ItemData> itemsToRemove = new List<ItemData>();

            // ���� ���� ��Ҹ� Ȯ���Ͽ� GridManager�� �����۰� ��
            foreach (ItemData requiredItem in packagingComponents)
            {
                bool found = false; // ������ �߰� ���� üũ

                // ���� �迭�� ��ȸ�ϸ鼭 ������ ã��
                for (int x = 0; x < cellItemData.GetLength(0); x++)
                {
                    if (cellItemData[x].Contains(requiredItem))
                    {
                        found = true; // ������ �߰�

                        // �ش� ������ ����
                        cellItemData[x].Remove(requiredItem);
                        Debug.Log($"������ {requiredItem}��(��) �׸��忡�� ���ŵǾ����ϴ�.");

                        // ������ ������ ����Ʈ�� �߰�
                        itemsToRemove.Add(requiredItem);
                        break; // �������� ã�����Ƿ� �� �̻� üũ�� �ʿ� ����
                    }
                    
                    if (found) break; // �̹� �������� ã�Ҵٸ� �� �̻� Ȯ���� �ʿ� ����
                }

                // �������� �߰ߵ��� �ʾҴٸ�
                if (!found)
                {
                    allItemsFound = false; // ��� �������� �߰ߵ��� ����
                    Debug.Log($"������ {requiredItem}��(��) �׸��忡�� �߰ߵ��� �ʾҽ��ϴ�.");
                    break; // �� �̻� Ȯ���� �ʿ� ����
                }
            }

            // ��� �������� �߰ߵǾ��ٸ� �����ǿ��� ����
            if (allItemsFound)
            {
                _orderRecipeDataRemove.Add(recipe);
            }
        }

        // �߰ߵ� ��� ������ ������ ����
        foreach (var recipe in _orderRecipeDataRemove)
        {
            _orderRecipeData.Remove(recipe);
        }

        // ��� �����ǰ� ���ŵǾ����� Ȯ��
        if (_orderRecipeDataRemove.Count == _orderRecipeData.Count)
        {
            _orderRecipeData.Clear();
        }
    }


    private bool CheckOrderItemList() 
    {
        return _orderRecipeData.Count <= 0; // ����Ʈ�� �������� ������ occupied
    }


    public List<OrderItem> GetActiveOrders()
    {
        return _activeOrders;
    }

    private void OnReturnButtonClicked()
    {
        UIManager.Instance.MenuPanel.ResetSelection();
        Debug.Log("��� ���õ� ���� ���� �ʱ�ȭ �Ϸ�.");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class POSManager : MonoBehaviour
{
    [SerializeField] private GameObject _orderPanel;
    [SerializeField] private GameObject _orderItemPrefab;
    [SerializeField] private Button _readyButton;
    public Button ReadyButton => _readyButton;

    private List<OrderItem> _reqirements = new List<OrderItem>(); // �ֹ���(�䱸����) ������ ���

    private int _currentRequirementIndex = 0; // ���� (���嵥���� ��) �䱸���� �ε���
    public int CurrentRequirementIndex => _currentRequirementIndex;

    private void Start()
    {
        CreateNewOrder();
        _readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    // ���ο� �ֹ��� ���� // (��ư Ŭ�� ȣ��)
    public void CreateNewOrder()
    {
        if (_currentRequirementIndex >= GameManager.Instance.CurrentRoundData.requirementsGroups.Count)
        {
            Debug.Log("���� ��");
            return;
        }

        // ���� �ֹ��� �ʱ�ȭ
        foreach (Transform child in _orderPanel.transform)
        {
            Destroy(child.gameObject);
        }
        _reqirements.Clear();

        // ���� �䱸���� ��������
        ItemRequirementsGroup currentRequirementGroup = GameManager.Instance.CurrentRoundData.requirementsGroups[_currentRequirementIndex];

        // �ֹ��� �׸� ����
        foreach (var requirement in currentRequirementGroup.itemRequirements)
        {
            PuzzleItemData randomPuzzleItem = GetRandomOrderItem(requirement.itemSize);
            GameObject orderItemObject = Instantiate(_orderItemPrefab, _orderPanel.transform);
            OrderItem orderItem = orderItemObject.GetComponent<OrderItem>();
            orderItem.SetupOrder(randomPuzzleItem, requirement.itemCount);
            _reqirements.Add(orderItem);
        }
    }

    // ���� ������ ���� //
    private PuzzleItemData GetRandomOrderItem(Vector2Int itemSize)
    {
        PuzzleItemData[] allPuzzleItems = Resources.LoadAll<PuzzleItemData>("ScriptableObject/PuzzleItem");
        List<PuzzleItemData> matchingItems = new List<PuzzleItemData>();

        // �־��� itemSize�� ������ �����۸� ���͸�
        foreach (var item in allPuzzleItems)
        {
            if (item.itemSize == itemSize)
            {
                matchingItems.Add(item);
            }
        }

        // �ߺ����� �ʵ��� ���� ������ ����
        if (matchingItems.Count > 0)
        {
            int randomIndex = Random.Range(0, matchingItems.Count);
            return matchingItems[randomIndex];
        }

        return null;
    }

    public void OnReadyButtonClicked()
    {
        bool isOrderMatched = CheckOrderItemList(); // �ֹ� �׸� üũ

        if (isOrderMatched)
        {
            Debug.Log("��� �ֹ��� ��ġ�մϴ�!");
            OnPuzzleSuccess();
            GameManager.Instance.OnPackagingSuccess();
        }
        else
        {
            Debug.Log("������ ����ġ");
            GameManager.Instance.PlayWitchAction(CharacterState.Sad);
        }

        GridManager.Instance.ClearAllItems();
    }

    // ���� ���� �� ó�� //
    private void OnPuzzleSuccess()
    {
        _currentRequirementIndex++;
        if (_currentRequirementIndex < _currentRound.requirementsGroups.Count)
        {
            CreateNewOrder(); // ���� �䱸�������� ���ο� �ֹ��� ����
        }
        else
        {
            Debug.Log($"{_currentRequirementIndex+1}���� Ŭ����");
        }
    }

    private bool CheckOrderItemList() 
    {
        return _orderRecipeData.Count <= 0; // ����Ʈ�� �������� ������ occupied
    }

    public List<OrderItem> GetActiveOrders()
    {
        return _reqirements;
    }
}
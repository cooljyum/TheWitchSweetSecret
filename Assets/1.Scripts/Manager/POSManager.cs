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

    private List<OrderItem> _reqirements = new List<OrderItem>(); // 주문서(요구사항) 아이템 목록

    private int _currentRequirementIndex = 0; // 현재 (라운드데이터 내) 요구사항 인덱스
    public int CurrentRequirementIndex => _currentRequirementIndex;

    private void Start()
    {
        CreateNewOrder();
        _readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    // 새로운 주문서 생성 // (버튼 클릭 호출)
    public void CreateNewOrder()
    {
        if (_currentRequirementIndex >= GameManager.Instance.CurrentRoundData.requirementsGroups.Count)
        {
            Debug.Log("라운드 끝");
            return;
        }

        // 기존 주문서 초기화
        foreach (Transform child in _orderPanel.transform)
        {
            Destroy(child.gameObject);
        }
        _reqirements.Clear();

        // 현재 요구사항 가져오기
        ItemRequirementsGroup currentRequirementGroup = GameManager.Instance.CurrentRoundData.requirementsGroups[_currentRequirementIndex];

        // 주문서 항목 생성
        foreach (var requirement in currentRequirementGroup.itemRequirements)
        {
            PuzzleItemData randomPuzzleItem = GetRandomOrderItem(requirement.itemSize);
            GameObject orderItemObject = Instantiate(_orderItemPrefab, _orderPanel.transform);
            OrderItem orderItem = orderItemObject.GetComponent<OrderItem>();
            orderItem.SetupOrder(randomPuzzleItem, requirement.itemCount);
            _reqirements.Add(orderItem);
        }
    }

    // 랜덤 아이템 추출 //
    private PuzzleItemData GetRandomOrderItem(Vector2Int itemSize)
    {
        PuzzleItemData[] allPuzzleItems = Resources.LoadAll<PuzzleItemData>("ScriptableObject/PuzzleItem");
        List<PuzzleItemData> matchingItems = new List<PuzzleItemData>();

        // 주어진 itemSize와 동일한 아이템만 필터링
        foreach (var item in allPuzzleItems)
        {
            if (item.itemSize == itemSize)
            {
                matchingItems.Add(item);
            }
        }

        // 중복되지 않도록 랜덤 아이템 선택
        if (matchingItems.Count > 0)
        {
            int randomIndex = Random.Range(0, matchingItems.Count);
            return matchingItems[randomIndex];
        }

        return null;
    }

    public void OnReadyButtonClicked()
    {
        bool isOrderMatched = CheckOrderItemList(); // 주문 항목 체크

        if (isOrderMatched)
        {
            Debug.Log("모든 주문이 일치합니다!");
            OnPuzzleSuccess();
            GameManager.Instance.OnPackagingSuccess();
        }
        else
        {
            Debug.Log("아이템 불일치");
            GameManager.Instance.PlayWitchAction(CharacterState.Sad);
        }

        GridManager.Instance.ClearAllItems();
    }

    // 퍼즐 성공 시 처리 //
    private void OnPuzzleSuccess()
    {
        _currentRequirementIndex++;
        if (_currentRequirementIndex < _currentRound.requirementsGroups.Count)
        {
            CreateNewOrder(); // 다음 요구사항으로 새로운 주문서 생성
        }
        else
        {
            Debug.Log($"{_currentRequirementIndex+1}라운드 클리어");
        }
    }

    private bool CheckOrderItemList() 
    {
        return _orderRecipeData.Count <= 0; // 리스트에 아이템이 있으면 occupied
    }

    public List<OrderItem> GetActiveOrders()
    {
        return _reqirements;
    }
}
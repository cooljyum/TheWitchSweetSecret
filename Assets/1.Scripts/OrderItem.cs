using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderItem : MonoBehaviour
{
    [SerializeField] private Image _itemImage;             // 음식 이미지
    [SerializeField] private TextMeshProUGUI _itemName;    // 음식 이름 텍스트
    public TextMeshProUGUI ItemName => _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;   // 음식 개수 텍스트
    public TextMeshProUGUI ItemCount => _itemCount;

    private HashSet<PuzzleItemData> _usedPuzzleItems = new HashSet<PuzzleItemData>(); // 랜덤 중복 방지용 


    // 주문서 항목 초기화
    public void SetupOrder(PuzzleItemData puzzleData, int itemCount)
    {
        _itemImage.sprite = puzzleData.itemImage; // 이미지 설정
        _itemName.text = puzzleData.itemName;     // 아이템 이름 설정
        _itemCount.text = "X " + itemCount;       // 아이템 개수 설정
    }
}

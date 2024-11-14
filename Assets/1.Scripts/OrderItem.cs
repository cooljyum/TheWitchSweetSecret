using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderItem : MonoBehaviour
{
    [SerializeField] private Image _itemImage;             // ���� �̹���
    [SerializeField] private TextMeshProUGUI _itemName;    // ���� �̸� �ؽ�Ʈ
    public TextMeshProUGUI ItemName => _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;   // ���� ���� �ؽ�Ʈ
    public TextMeshProUGUI ItemCount => _itemCount;

    private HashSet<PuzzleItemData> _usedPuzzleItems = new HashSet<PuzzleItemData>(); // ���� �ߺ� ������ 


    // �ֹ��� �׸� �ʱ�ȭ
    public void SetupOrder(PuzzleItemData puzzleData, int itemCount)
    {
        _itemImage.sprite = puzzleData.itemImage; // �̹��� ����
        _itemName.text = puzzleData.itemName;     // ������ �̸� ����
        _itemCount.text = "X " + itemCount;       // ������ ���� ����
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderItem : MonoBehaviour
{
    [SerializeField] private List<Image> _itemImage;                 // ���� �̹���
    [SerializeField] private TextMeshProUGUI _itemNameText;    // ���� �̸� �ؽ�Ʈ
    public TextMeshProUGUI ItemName => _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemCountText;   // ���� ���� �ؽ�Ʈ
    public TextMeshProUGUI ItemCount => _itemCountText;

    private int _remainingCount;                               // ���� �ֹ� ����

    // �ֹ� �׸� �ʱ�ȭ //
    public void SetupOrder(RecipeData foodItem, int itemCount)
    {
        // �̹��� ���� �� Ȱ��ȭ
        for (int i = 0; i < _itemImage.Count; i++)
        {
            if (i < foodItem.packagingComponents.Count)
            {
                _itemImage[i].sprite = foodItem.packagingComponents[i].itemImage; // ������� �̹��� �Ҵ�
                _itemImage[i].gameObject.SetActive(true);                        // �̹��� Ȱ��ȭ

                _itemNameText.text = foodItem.recipeName; // �ʱ� ���� �̸� ����

                if (foodItem.packagingComponents[i].itemType > 0)
                {
                    _itemNameText.text = foodItem.packagingComponents[i].itemName; // ���� �̸� ����
                }
            }
            else
            {
                _itemImage[i].gameObject.SetActive(false);                       // ������ �̹��� ��Ȱ��ȭ
            }
        }

        _itemCountText.text = "X " + itemCount;      // ���� ���� ����
        _remainingCount += itemCount;
    }

    // �ֹ� �Ϸ� ó�� (POSManager���� ȣ��) //
    public void CompleteOrder()
    {
        _remainingCount = 0;
    }

    public bool IsCompleted()
    {
        return _remainingCount == 0;
    }
}

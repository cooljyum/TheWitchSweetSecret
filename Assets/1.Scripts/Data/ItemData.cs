using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Packaging = 0,  // ����
    Beverage = 1,   // ����
    Macaron = 2,    // ��ī��
    Pudding = 3,    // Ǫ��
    Cake = 4        // ����ũ
}

[CreateAssetMenu(fileName = "New Item", menuName = "Order System/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Item Infor")]
    public string itemName;         // �������� �̸�
    public Sprite itemImage;        // �������� �̹���

    [Header("Item Type")]
    public ItemType itemType;       // �������� �� Ÿ�� (��: ����, ���� ��)
    public int subType;             // �������� ���� Ÿ�� (Ÿ�� ������ ���� �߰� ����)

    [Header("Order Infor")]
    public int orderIndex;          // �������� ���� �� �ִ� ���� (�����ǿ����� ��ġ)
    public int maxAmount;           // �ֹ� �� �ִ� ����

    [Header("Item Size")]
    public Vector2Int itemSize;     // �������� ũ�� (width, height)
}

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

[CreateAssetMenu(fileName = "New Ingredient Data", menuName = "Order System/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [Header("Ingredient Item Information")]
    public string itemName;                 // �������� �̸�
    public Sprite itemImage;                // �������� �̹���
    public Vector2Int itemSize;             // �������� ũ�� (width, height)

    [Header("Ingredient Item Feature")]
    public ItemType itemType;               // �������� �� Ÿ�� (��: ����, ���� ��)
    public int orderIndex;                  // �������� ���� �� �ִ� ���� (�����ǿ����� ��ġ)
}

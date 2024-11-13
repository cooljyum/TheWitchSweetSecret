using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Item Data", menuName = "Order System/Puzzle Item Data")]
public class PuzzleItemData : ScriptableObject
{
    [Header("Puzzle Item Information")]
    public string itemName;                  // �ϼ��� ������ �̸�
    public Sprite itemImage;                 // �ϼ��� ������ �̹���
    public Vector2Int itemSize;              // �ϼ��� ������ ũ�� (width, height)
    public List<IngredientData> itemRecipe;  // �����ϴ� ������
}
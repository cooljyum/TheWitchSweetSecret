using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Packaging = 0,  // 포장
    Beverage = 1,   // 음료
    Macaron = 2,    // 마카롱
    Pudding = 3,    // 푸딩
    Cake = 4        // 케이크
}

[CreateAssetMenu(fileName = "New Ingredient Data", menuName = "Order System/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [Header("Ingredient Item Information")]
    public string itemName;                 // 아이템의 이름
    public Sprite itemImage;                // 아이템의 이미지
    public Vector2Int itemSize;             // 아이템의 크기 (width, height)

    [Header("Ingredient Item Feature")]
    public ItemType itemType;               // 아이템의 주 타입 (예: 포장, 음료 등)
    public int orderIndex;                  // 아이템의 놓을 수 있는 순서 (레시피에서의 위치)
}

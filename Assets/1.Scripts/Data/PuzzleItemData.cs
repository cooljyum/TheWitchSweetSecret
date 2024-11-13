using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Item Data", menuName = "Order System/Puzzle Item Data")]
public class PuzzleItemData : ScriptableObject
{
    [Header("Puzzle Item Information")]
    public string itemName;                  // 완성된 아이템 이름
    public Sprite itemImage;                 // 완성된 아이템 이미지
    public Vector2Int itemSize;              // 완성된 아이템 크기 (width, height)
    public List<IngredientData> itemRecipe;  // 조합하는 레시피
}
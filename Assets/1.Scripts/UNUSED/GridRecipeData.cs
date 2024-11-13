using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GridRecipeData", menuName = "Order System/GridRecipeData")]
public class GridRecipeData : ScriptableObject
{
    public Vector2Int recipeSize;     // �������� ũ�� (width, height)
    public RecipeData[] recipeDatas;  // �ش� ũ���� �����ǵ� ������
}

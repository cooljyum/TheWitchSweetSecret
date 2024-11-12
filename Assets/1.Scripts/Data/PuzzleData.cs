using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridRecipeSizeData
{
    public int gridRecipeCount;           // �ش� ������ �������� ����
    public GridRecipeData gridRecipeData; // �ش� ������ ������ ���
}


[CreateAssetMenu(fileName = "New Puzzle Data", menuName = "Order System/Puzzle Data")]
public class PuzzleData : ScriptableObject
{
    public List<GridRecipeSizeData> gridRecipeDatas;    // �ش� ������ ���
}
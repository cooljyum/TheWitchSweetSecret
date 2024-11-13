using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Order System/Recipe Data")]
public class RecipeData : ScriptableObject
{
    public string recipeName;                       // �޴� �̸�
    public List<IngredientData> packagingComponents;      // ���忡 �ʿ��� �����۵� (�巡�� ����)
    //public IngredientData additionalItem;                 // �߰��� �ʿ��� ������ (ex. ��ũ, ����)
}
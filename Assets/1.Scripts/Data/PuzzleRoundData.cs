using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRequirement
{
    public Vector2Int itemSize;     // �ʿ��� ������ ũ��
    public int itemCount;           // �ʿ��� ������ ����
}

[System.Serializable]
public class ItemRequirementsGroup
{
    public List<ItemRequirement> itemRequirements; // Ư�� �׷��� ������ �䱸 ���� ���
}

[CreateAssetMenu(fileName = "New Puzzle Round", menuName = "Order System/Puzzle Round Data")]
public class PuzzleRoundData : ScriptableObject
{
    public int roundNumber;                                // ���� ��
    public List<int> activeCellIndices;                   // Ȱ��ȭ�� ĭ �ε��� (0~24)
    public List<ItemRequirementsGroup> requirementsGroups; // ���� ���� ������ �䱸 ���� �׷�
}
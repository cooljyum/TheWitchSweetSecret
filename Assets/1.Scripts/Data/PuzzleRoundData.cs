using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRequirement
{
    public Vector2Int itemSize;     // 필요한 아이템 크기
    public int itemCount;           // 필요한 아이템 개수
}

[System.Serializable]
public class ItemRequirementsGroup
{
    public List<ItemRequirement> itemRequirements; // 특정 그룹의 아이템 요구 사항 목록
}

[CreateAssetMenu(fileName = "New Puzzle Round", menuName = "Order System/Puzzle Round Data")]
public class PuzzleRoundData : ScriptableObject
{
    public int roundNumber;                                // 라운드 수
    public List<int> activeCellIndices;                   // 활성화된 칸 인덱스 (0~24)
    public List<ItemRequirementsGroup> requirementsGroups; // 여러 개의 아이템 요구 사항 그룹
}
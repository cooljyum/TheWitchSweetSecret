using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Order System/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Basic Info")]
    public int levelNumber;             // ���� ����
    public int maxMoney;                // ���� ���� �ݾ�(?�����𸣰���)

    [Header("PuzzleData")]
    public PuzzleData[] puzzleDatas;    // �ش� ���� ���� ���
}
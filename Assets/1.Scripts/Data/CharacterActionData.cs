using UnityEngine;

public enum CharacterState
{
    Basic,
    Happy,
    Angry,
    Sad,
    Fail
}

[CreateAssetMenu(fileName = "New CharacterActionData", menuName = "Character/ActionData")]
public class CharacterActionData : ScriptableObject
{
    public CharacterState characterState;   // ĳ���� ����
    public Sprite expression;               // ������ ǥ�� �̹���
    public string[] speechText;               // ��ǳ���� ǥ���� �ؽ�Ʈ
    public Sprite speechBalloonImg;   // ����� ��ǳ�� ������
    public float displayDuration = 3f;      // ǥ���� ������ �ð�
}

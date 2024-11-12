using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchManager : MonoBehaviour
{
    [SerializeField] private CharacterManager _characterManager; // �ϳ��� ĳ���� �Ŵ���
    [SerializeField] private List<CharacterActionData> _characterActionDataList; // ĳ���� �ൿ ������ ����Ʈ

    private void Awake()
    {
        // CharacterManager ������Ʈ �ʱ�ȭ (�� ���, ���� GameObject�� �ִٰ� ����)
        _characterManager = GetComponent<CharacterManager>();

        // CharacterActionData ����Ʈ �ʱ�ȭ
        // ���� ���, Resources �������� �ε��ϴ� ���
         _characterActionDataList = new List<CharacterActionData>(Resources.LoadAll<CharacterActionData>("ScriptableObject/CharacterAction/Main/Witch"));
    }

    public void PerformCharacterAction(CharacterState state)
    {
        // ���¿� �ش��ϴ� �׼� ������ ��������
        CharacterActionData actionData = _characterActionDataList.Find(data => data.characterState == state);

        if (actionData != null)
        {
            _characterManager.PerformAction(actionData); // ĳ���� �׼� ����

            // ���� �� ������� ���ϱ⸦ �����ϴ� �ڷ�ƾ ����
            StartCoroutine(CatSpeakAfterWitch(actionData));
        }
        else
        {
            Debug.LogWarning("�ش� ���¿� ���� �׼� �����Ͱ� �����ϴ�.");
        }
    }

    private IEnumerator CatSpeakAfterWitch(CharacterActionData actionData)
    {
        // ������ ���� �ð�(���� �ð�) ���� ������ ���
        yield return new WaitForSeconds(actionData.displayDuration + Random.Range(1f, 2f));

        // ������� ���¿� ���� �ൿ ����
        switch (actionData.characterState)
        {
            case CharacterState.Happy:
                GameManager.Instance.PlayCatAction(CharacterState.Happy); // ��� ���·� ����� ���ϱ�
                break;

            case CharacterState.Sad:
                GameManager.Instance.PlayCatAction(CharacterState.Angry); // ȭ�� ���·� ����� ���ϱ�
                break;
            case CharacterState.Fail:
                break;
            default:
                GameManager.Instance.PlayCatAction(CharacterState.Basic); // �⺻ ���·� ����� ���ϱ�
                break;
        }
    }
}

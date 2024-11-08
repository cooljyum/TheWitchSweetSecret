using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatManager : MonoBehaviour
{
    [SerializeField] private CharacterManager _characterManager; // �ϳ��� ĳ���� �Ŵ���
    [SerializeField] private List<CharacterActionData> _characterActionDataList; // ĳ���� �ൿ ������ ����Ʈ
    [SerializeField] private Image _catWing;// Ĺ���̹���

    private void Awake()
    {
        // CharacterManager ������Ʈ �ʱ�ȭ (�� ���, ���� GameObject�� �ִٰ� ����)
        _characterManager = GetComponent<CharacterManager>();

        // CharacterActionData ����Ʈ �ʱ�ȭ
        // ���� ���, Resources �������� �ε��ϴ� ���
         _characterActionDataList = new List<CharacterActionData>(Resources.LoadAll<CharacterActionData>("ScriptableObject/CharacterAction/Main/Cat"));

        StartCoroutine(WingAnimation());
    }

    public void PerformCharacterAction(CharacterState state)
    {
        // ���¿� �ش��ϴ� �׼� ������ ��������
        CharacterActionData actionData = _characterActionDataList.Find(data => data.characterState == state);

        if (actionData != null)
        {
            _characterManager.PerformAction(actionData); // ĳ���� �׼� ����
        }
        else
        {
            Debug.LogWarning("�ش� ���¿� ���� �׼� �����Ͱ� �����ϴ�.");
        }
    }

    private IEnumerator WingAnimation()
    {
        Sprite wingDown = Resources.Load<Sprite>("Sprite/Character/Cat/Cat_WingDown");
        Sprite wingUp = Resources.Load<Sprite>("Sprite/Character/Cat/Cat_WingUp");

        while (true)
        {
            _catWing.sprite = wingUp;
            yield return new WaitForSeconds(1f);
            _catWing.sprite = wingDown;
            yield return new WaitForSeconds(1f);
        }
    }
}

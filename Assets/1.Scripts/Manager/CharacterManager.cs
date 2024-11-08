using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Image _faceImage; // ĳ���� �� �̹���
    [SerializeField] private GameObject _balloon; // ��ǳ�� ��ġ�� ���� �θ� ��ü

    private Sprite _originalFace; // ���� ǥ�� ����

    private void Awake()
    {
        _originalFace = _faceImage.sprite; // �ʱ� �� �̹��� ����
    }

    // ĳ������ �׼��� �����ϴ� �޼ҵ�
    public void PerformAction(CharacterActionData actionData)
    {
        // ���¿� ǥ�� ����
        _faceImage.sprite = actionData.expression;
        CharacterState currentState = actionData.characterState;

        // ��ǳ�� �ν��Ͻ�ȭ �� �ؽ�Ʈ ����
        _balloon.GetComponent<Image>().sprite = actionData.speechBalloonImg;
         TextMeshProUGUI speechText = _balloon.GetComponentInChildren<TextMeshProUGUI>();

        // speechText �迭���� �������� �ؽ�Ʈ ����
        string randomText = actionData.speechText[Random.Range(0, actionData.speechText.Length)];
        speechText.text = randomText;

        // ���¿� ���� �߰� �ൿ ���� ���� (��: ���¿� ���� ���� ȿ��)
        HandleStateBehavior(currentState);

        //��ǳ�� Ȱ��ȭ
        _balloon.SetActive(true);

        // �ڷ�ƾ���� ���� �ð� �� ���� ���·� ����
        StartCoroutine(RevertToOriginalState(actionData.displayDuration));
    }

    // ���¿� ���� �ൿ ó��
    private void HandleStateBehavior(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.Happy:
                Debug.Log("ĳ���Ͱ� �⻵�մϴ�!");
                // ��ݿ� ���� �߰� ȿ�� (��: �̸�Ƽ�� ǥ��) ���� ����
                break;
            case CharacterState.Sad:
                Debug.Log("ĳ���Ͱ� �����մϴ�.");
                // ���Ŀ� ���� �߰� ȿ�� (��: ��� �Ҹ�) ���� ����
                break;
            case CharacterState.Angry:
                Debug.Log("ĳ���Ͱ� ȭ�����ϴ�!");
                // ����� ���� �߰� ȿ�� (��: ���� �ִϸ��̼�) ���� ����
                break;
            default:
               // Debug.Log("�⺻ �����Դϴ�.");
                break;
        }
    }

    // ���� ���·� �����ϴ� �ڷ�ƾ
    private IEnumerator RevertToOriginalState(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���� ǥ���� ���·� ����
        _faceImage.sprite = _originalFace;

        // ��ǳ�� ��Ȱ��ȭ
        _balloon.SetActive(false);
    }
}

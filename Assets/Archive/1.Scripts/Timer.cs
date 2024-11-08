using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;       // �ð� �ؽ�Ʈ�� ǥ���� UI Text ������Ʈ
    public Image timeImage;     // ��/�� �̹����� ������ �̹���
    public Sprite sunSprite;    // �� �̹���
    public Sprite moonSprite;   // �� �̹���

    private float currentTime = 8f; // ���� �ð��� 9�÷� ����
    private float timeSpeed = 2.5f; // 2.5�ʴ� 1�ð� �߰�

    void Update()
    {
        if (GameManager.Instance != null)
        {
            // �ð� �ؽ�Ʈ ������Ʈ
            timeText.text = GameManager.Instance.GetFormattedTime();

            // ��/�� �̹��� ������Ʈ
            timeImage.sprite = GameManager.Instance.GetTimeSprite(sunSprite, moonSprite);
        }
    }
}

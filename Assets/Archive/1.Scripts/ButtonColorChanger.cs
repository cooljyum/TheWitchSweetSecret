using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    public Color pressedColor; // Ŭ���� �� ����
    [SerializeField] private Color originalColor;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color; // ���� ���� ����
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.color = pressedColor; // Ŭ�� �� ���� ����
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.color = originalColor; // ���� �������� ����
    }
}
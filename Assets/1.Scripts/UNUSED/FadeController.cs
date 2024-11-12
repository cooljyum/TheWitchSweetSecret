using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float startFadeDuration = 1f; // ���� �� ���̵�ƿ� ���� �ð� ����

    private void Start()
    {
        // ���� �� ���̵�ƿ�
        FadeIn(startFadeDuration);
    }

    // ���̵� �� �Լ�
    public void FadeIn(float duration)
    {
        if (fadeImage == null) InstantiateFadeOverlay();

        StartCoroutine(Fade(1, 0, duration));
    }

    // ���̵� �ƿ� �Լ�
    public void FadeOut(float duration)
    {
        if (fadeImage == null) InstantiateFadeOverlay();

        StartCoroutine(Fade(0, 1, duration));
    }

    private void InstantiateFadeOverlay()
    {
        // fadeImage�� null�� ��� �������� �̹��� ���� �� Canvas�� �߰�
        GameObject overlayInstance = new GameObject("FadeOverlay");
        fadeImage = overlayInstance.AddComponent<Image>();
        overlayInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

        RectTransform rt = fadeImage.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        // ���̵� �� �� �̹��� ����
        if (endAlpha == 0)
        {
            Destroy(fadeImage.gameObject);
            fadeImage = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    private void Start()
    {
        // transform �� scale ���� ��� 0.1f�� �����մϴ�.
        transform.localScale = Vector3.one * 0.1f;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ScaleOverTime(transform, Vector3.one * 1.1f, Vector3.one, 0.3f));
    }

    public void Hide()
    {
        StartCoroutine(ScaleOverTime(transform, Vector3.one * 1.1f, Vector3.one * 0.2f, 0.3f, () =>
        {
            gameObject.SetActive(false);
        }));
    }

    private IEnumerator ScaleOverTime(Transform target, Vector3 startScale, Vector3 endScale, float duration, System.Action onComplete = null)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = target.localScale;

        // ù ��° �ܰ�� startScale�� Ȯ��
        while (elapsedTime < duration / 2)
        {
            target.localScale = Vector3.Lerp(initialScale, startScale, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �� ��° �ܰ�� endScale�� ���
        elapsedTime = 0f;
        initialScale = target.localScale;
        while (elapsedTime < duration / 2)
        {
            target.localScale = Vector3.Lerp(initialScale, endScale, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �Ϸ� �� �ݹ� ����
        onComplete?.Invoke();
    }
}

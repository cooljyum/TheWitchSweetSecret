using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonHandler : MonoBehaviour
{
    public PanelHandler popupWindow;

    public void OnButtonClick()
    {
        StartCoroutine(ScaleButtonAndClose());
    }

    private IEnumerator ScaleButtonAndClose()
    {
        // 0.95f�� ����
        yield return StartCoroutine(ScaleOverTime(transform, Vector3.one, Vector3.one * 0.95f, 0.1f));
        // 1.05f�� �ø�
        yield return StartCoroutine(ScaleOverTime(transform, Vector3.one * 0.95f, Vector3.one * 1.05f, 0.1f));
        // 1f�� �ǵ���
        yield return StartCoroutine(ScaleOverTime(transform, Vector3.one * 1.05f, Vector3.one, 0.1f));

        // ��ư �ִϸ��̼��� ���� �� â�� ����
        popupWindow.Hide();
    }

    private IEnumerator ScaleOverTime(Transform target, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localScale = endScale; // ��Ȯ�� �������� ����
    }
}

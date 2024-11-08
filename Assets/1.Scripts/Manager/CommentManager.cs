using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CommentManager : MonoBehaviour
{
    [SerializeField] private Text _commentPrefab;  // ��� ������
    [SerializeField] private Transform _commentParent;  // ��� ǥ�õ� �θ� ������
    [SerializeField] private float _commentSpeed = 50f;  // ��� �ö󰡴� �ӵ�
    [SerializeField] private int _initialCommentsCount = 6;  // ó���� ������ ��� ����
    [SerializeField] private int _subsequentCommentsCount = 6;  // �� �� ������ ��� ����
    [SerializeField] private float _rating = 3.0f;  // ����
    [SerializeField] private float _ratingRange = 1.0f;  // ���� ��������
    [SerializeField] private float _positionOffset = 100.0f;  // ��ġ offset

    private List<CommentData> _comments = new List<CommentData>();  // ��� ������ ����Ʈ
    private Queue<Text> _activeComments = new Queue<Text>(); // ȭ�鿡 ǥ�õ� ���
    private int _outOfBoundsCount = 0;  // ȭ���� ��� ��� ��

    private void Start()
    {
        LoadCommentsFromCSV("csv/Comments");  // CSV���� ��� ������ �ε�
        ShowBatchOfComments(_initialCommentsCount);  // ó���� 10�� ����
    }

    // ��� �����͸� CSV���� �ε�
    private void LoadCommentsFromCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName); // Ȯ���� ���� ���� �̸��� ���
        if (csvFile != null)
        {
            string[] data = csvFile.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < data.Length; i++)  // ù ���� ����̹Ƿ� ����
            {
                string[] row = data[i].Split(',');
                if (row.Length < 2) continue;

                string commentText = row[0].Trim();
                float rating;
                if (float.TryParse(row[1], out rating))
                {
                    _comments.Add(new CommentData(commentText, rating));
                }
                else
                {
                    Debug.LogError("Invalid rating value: " + row[1]);
                }
            }
        }
        else
        {
            Debug.LogError("CSV ������ ã�� �� �����ϴ�.");
        }
    }

    // ����� �� ���� ���� ���� ǥ���ϴ� �Լ�
    private void ShowBatchOfComments(int commentsToSpawn)
    {
        if (_comments.Count == 0) return;

        for (int i = 0; i < commentsToSpawn; i++)
        {
            CommentData selectedComment = GetWeightedRandomComment(_rating - _ratingRange, _rating + _ratingRange);
            if (selectedComment != null)
            {
                // ����� ȭ�鿡 �����ϰ� ���� ��ũ���ϴ� �ִϸ��̼�
                Text newComment = Instantiate(_commentPrefab, _commentParent);

                // ����� �ؽ�Ʈ ����
                newComment.text = selectedComment.commentText;

                // Y ��ġ�� �ٸ��� �����Ͽ� ��ġ�� �ʰ� ����
                RectTransform rectTransform = newComment.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, (-i * 100f) - _positionOffset);

                // ������ ����� ť�� �߰�
                _activeComments.Enqueue(newComment);

                StartCoroutine(MoveComment(newComment));
            }
        }
    }

    // ���� ������ ���� �������� ����� �����ϴ� �Լ�
    private CommentData GetWeightedRandomComment(float minRating, float maxRating)
    {
        float totalWeight = 0f;
        List<CommentData> filteredComments = new List<CommentData>();

        foreach (CommentData comment in _comments)
        {
            if (comment.rating >= minRating && comment.rating <= maxRating)
            {
                filteredComments.Add(comment);
                totalWeight += comment.rating;
            }
        }

        if (filteredComments.Count == 0)
        {
            Debug.LogWarning("�ش� ���� ������ ����� �����ϴ�.");
            return null;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (CommentData comment in filteredComments)
        {
            cumulativeWeight += comment.rating;
            if (randomValue <= cumulativeWeight)
            {
                return comment;
            }
        }

        return null;
    }

    // ����� ���� �����̴� �ڷ�ƾ
    private IEnumerator MoveComment(Text comment)
    {
        RectTransform rectTransform = comment.GetComponent<RectTransform>();
        RectTransform parentRectTransform = _commentParent.GetComponent<RectTransform>();

        while (true)
        {
            rectTransform.anchoredPosition += new Vector2(0, _commentSpeed * Time.deltaTime);

            // ȭ���� ��� ���
            if (rectTransform.anchoredPosition.y > parentRectTransform.anchoredPosition.y + (parentRectTransform.rect.height / 2) + 400.0f)
            {
                // ����� ȭ�� �Ʒ��� ��ġ���� ����
                rectTransform.anchoredPosition = new Vector2(0, -_positionOffset*4);
            }

            yield return null;
        }
    }
}

// ��� ������ ����ü
[System.Serializable]
public class CommentData
{
    public string commentText;
    public float rating;

    public CommentData(string commentText, float rating)
    {
        this.commentText = commentText;
        this.rating = rating;
    }
}

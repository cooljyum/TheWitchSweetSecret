using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGamePlay = false;

    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private HeartManager _heartManager;

    [SerializeField] private WitchManager _witchManager;
    [SerializeField] private CatManager _catManager;

    private float _currentTime = 8f; // ���� �ð� (9��)
    private float _timeSpeed = 30f; // 2.5�ʴ� 1�ð� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // HeartManager�� ScoreManager �ν��Ͻ��� ã�ų� ���� �Ҵ�
        _scoreManager = _scoreManager ?? FindObjectOfType<ScoreManager>();
        _heartManager = _heartManager ?? FindObjectOfType<HeartManager>();

        IsGamePlay = true;

        // ������ �׼��� ������ �������� �����ϴ� �ڷ�ƾ ����
        StartCoroutine(WitchActionRoutine());

        // �ð� ������Ʈ �ڷ�ƾ ����
        StartCoroutine(TimeUpdateRoutine());
    }

    // �ð��� �����ϴ� �ڷ�ƾ
    private IEnumerator TimeUpdateRoutine()
    {
        while (IsGamePlay)
        {
            _currentTime += 1f; // 1�ð� ����
            if (_currentTime >= 18f)
            {
                EndGame();
                break;
            }

            yield return new WaitForSeconds(_timeSpeed);
        }
    }

    // �ð��� �ؽ�Ʈ �������� ��ȯ�ϴ� �޼ҵ�
    public string GetFormattedTime()
    {
        int hours = (int)_currentTime;
        string hourString = hours < 10 ? "0" + hours : hours.ToString();
        return hourString + ":00";
    }

    // ���� �ð��� ���� ��/�� ���� ��ȯ
    public Sprite GetTimeSprite(Sprite sunSprite, Sprite moonSprite)
    {
        return _currentTime < 14f ? sunSprite : moonSprite;
    }

    // ���� �߰� �� ��Ʈ ������Ʈ
    public void AddScore(int amount)
    {
        _scoreManager.AddScore(amount);
        _heartManager.OnOrderSuccess();
    }

    // ���� ������ �θ��� �Լ�
    public void OnPackagingSuccess()
    {
        _scoreManager.AddPackagingCount(1);
        _heartManager.OnOrderSuccess();
    }

    // ���� ������ ��Ʈ �� ��ȯ
    public int GetCurrentScore() => _scoreManager.CurrentScore;
    public int GetCurrentHearts() => _heartManager.CalculateHearts();

    // �������� ���� �� �ʱ�ȭ
    public void ResetStage()
    {
        _scoreManager.ResetScore();
        _heartManager.ResetHearts();
    }

    // ���� �׼�����
    public void PlayWitchAction(CharacterState state)
    {
        if (_witchManager != null)
        {
            _witchManager.PerformCharacterAction(state);
        }
    }

    // �׼� ����
    public void PlayCatAction(CharacterState state)
    {
        if (_catManager != null)
        {
            _catManager.PerformCharacterAction(state);
        }
    }

    // ���� �׼��� ������ �������� �����ϴ� �ڷ�ƾ
    private IEnumerator WitchActionRoutine()
    {
        while (true) // ���� ����
        {
            float waitTime = Random.Range(7f, 10f);
            yield return new WaitForSeconds(waitTime);
            CharacterState randomState = CharacterState.Basic;
            PlayWitchAction(randomState);
        }
    }

    public void EndGame()
    {
        Debug.Log("���� ��");
        IsGamePlay = false;
        int hearts = HeartManager.Instance.CalculateHearts();
        Debug.Log("���� ����. ���� ��Ʈ: " + hearts + "��");
        SceneManager.LoadScene("EndingScene");
    }
}

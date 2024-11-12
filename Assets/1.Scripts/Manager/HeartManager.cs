using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public static HeartManager Instance;

    [SerializeField] private int _currentScore = 50;       // �ʱ� ȣ����
    [SerializeField] private float _successIncrement = 10f; // ���� �� ȣ���� ������
    [SerializeField] private float _failureDecrement;       // ���� �� ȣ���� ���ҷ� (������ 2/3)
    [SerializeField] private int _fullHeartThreshold = 80;  // ���� ����

    private int _maxScore = 100;           // �ִ� ȣ����
    private int _minScore = 0;             // �ּ� ȣ����

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
    }
   

    void Start()
    {
        // ���� �� ���ҷ��� ���� �� �������� 2/3�� ����
        _failureDecrement = _successIncrement * 2 / 3;

        // 1�ʸ��� ȣ���� ����
        InvokeRepeating("UpdateScoreOverTime", 1.5f, 1.5f);

        // �ʱ� ȣ���� ǥ��
        MainManager.Instance.UpdateScoreUI();
    }

    public string GetCurScore() 
    {
        return _currentScore.ToString();
    }

    // �ֹ� ���� �� ȣ��
    public void OnOrderSuccess()
    {
        IncreaseScore(_successIncrement);
    }

    // �ֹ� ���� �� ȣ��
    public void OnOrderFailure()
    {
        DecreaseScore(_failureDecrement);
    }

    // ��� ���� �� ȣ��
    public void OnDeliverySuccess()
    {
        IncreaseScore(_successIncrement);
    }

    // ��� ���� �� ȣ��
    public void OnDeliveryFailure()
    {
        DecreaseScore(_failureDecrement);
    }

    // 1�ʸ��� ȣ��Ǿ� ȣ���� 1�� ����
    private void UpdateScoreOverTime()
    {
        DecreaseScore(1f);
    }

    // ȣ���� ���� �Լ�
    private void IncreaseScore(float amount)
    {
        _currentScore = Mathf.Clamp(_currentScore + (int)amount, _minScore, _maxScore);
        MainManager.Instance.UpdateScoreUI();
    }

    // ȣ���� ���� �Լ�
    private void DecreaseScore(float amount)
    {
        _currentScore = Mathf.Clamp(_currentScore - (int)amount, _minScore, _maxScore);
        MainManager.Instance.UpdateScoreUI();
    }

    // UI ������Ʈ �Լ�

    // ��Ʈ �� �ʱ�ȭ �Լ�
    public void ResetHearts()
    {
        _currentScore = 50; // �ʱⰪ���� ���� (���⼭ �ʱⰪ�� �ʿ信 ���� ���� ����)
        MainManager.Instance.UpdateScoreUI();    // UI ������Ʈ
    }

    // ���� ����� 1�� ���� ���̴� ȣ���� ����
    public void EndCalculateHeart()
    {
        CancelInvoke("UpdateScoreOverTime"); // ���� ���� �� ȣ�� ����
    }

    // ��Ʈ ���� ���
    public int CalculateHearts()
    {
        if (_currentScore >= _fullHeartThreshold)
            return 3;
        else if (_currentScore >= _fullHeartThreshold * 2 / 3)
            return 2;
        else if (_currentScore >= _fullHeartThreshold / 3)
            return 1;
        else
            return 0;
    }
}

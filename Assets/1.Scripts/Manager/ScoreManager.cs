using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private float _currentStamina; // ���� ���� ü��
    private float _maxStamina = 100f; // �ִ� ü��
    private int _staminaIncrementThreshold = 5; // ���׹̳� ������ ���� ���� ���� ����
    private int _packagingCount; // ���� ���� ���� �߰�

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

    private int _score;

    // ���� ���� ��ȯ
    public int CurrentScore => _score;

    // ���� ���� ��ȯ
    public int PackagingCount => _packagingCount;

    // ���� �߰�
    public void AddScore(int amount)
    {
        _score += amount;
    }

    // ���� ���� �߰� �� ���׹̳� üũ
    public void AddPackagingCount(int amount)
    {
        _packagingCount += amount;

        IncreaseStamina();

        Debug.Log($"���� ���� ���� = {_packagingCount}");
    }

    // ���׹̳� ����
    private void IncreaseStamina()
    {
        _currentStamina += 0.2f;
        if (_currentStamina > _maxStamina) // �ִ� ü�� �ʰ� ����
        {
            _currentStamina = _maxStamina;
        }
        Debug.Log($"���� ���׹̳� = {_currentStamina}");

        GameManager.Instance.PlayWitchAction(CharacterState.Happy);
    }

    // ���� �ʱ�ȭ
    public void ResetScore()
    {
        _score = 0;
        _packagingCount = 0; // ���� ������ �ʱ�ȭ
        _currentStamina = 0; // ���׹̳��� �ʱ�ȭ
    }

    //���׹̳� ����
    public float GetCurStaminaGauge()
    {
        return _currentStamina;
    }
    public void SetCurStaminaGauge(float curStamina)
    {
        _currentStamina = curStamina;
    }
}

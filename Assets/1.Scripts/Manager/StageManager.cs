using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject _stagePanel;

    private GameObject _stageIconPrefab;
    private Dictionary<int, int> _starPointByStage = new Dictionary<int, int>(); // ���������� ���� �����
    public Dictionary<int, int> StarPointByStage => _starPointByStage;
    private int _totalStarPoint = 0;                                              // �� ����
    private int _maxStarPoint;                                                    // �ִ� ����(���� ����)

    private void Start()
    {
        //CreateStageIcons();

    }

    // �������� �ڵ� ���� //
    private void CreateStageIcons()
    {
        // �ִ� ���� ���� ����
        _maxStarPoint = Resources.LoadAll<ScriptableObject>("ScriptableObject/Level").Length * 5;

        // ���� ������ ������ ���� �������� ����
        var levelDataObjects = Resources.LoadAll<ScriptableObject>("ScriptableObject/Level");

        foreach (var levelData in levelDataObjects)
        {
            GameObject stageIcon = Instantiate(_stageIconPrefab, _stagePanel.transform);
            stageIcon.name = "Day" + levelData.name.Substring(5);

            Transform stageTxtTransform = stageIcon.transform.GetChild(0);
            TextMeshProUGUI stageTxtTMP = stageTxtTransform.GetComponent<TextMeshProUGUI>();
            stageTxtTMP.text = levelData.name.Substring(5);

            TextMeshProUGUI pointTxtTMP = _stagePanel.GetComponentInChildren<TextMeshProUGUI>();
            pointTxtTMP.text = $"{_totalStarPoint} / {_maxStarPoint}";
        }
    }

    // ���� �� ���� ��Ȳ ������Ʈ
    private void UpdateStarPoint(int points)
    {
        if (!_starPointByStage.ContainsKey(UIManager.Instance.CurrentDayNumber))
        {
            _starPointByStage[UIManager.Instance.CurrentDayNumber] = 0;
        }

        _starPointByStage[UIManager.Instance.CurrentDayNumber] += points;
        _totalStarPoint += points;

        // ���� ������ �ڵ� ��ȯ üũ
        if (_totalStarPoint >= _maxStarPoint)
        {
            SceneManager.LoadScene("EndingScene");
        }
    }
}

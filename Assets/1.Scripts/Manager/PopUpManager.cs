using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;    // ���� ����â
    [SerializeField] private GameObject _statusPanel;   // ����/����â
    [SerializeField] private GameObject _gameOverPanel; // ���ӿ���â

    private void Start()
    {
        HideAllPanels();
        AssignOkButtonEvents();
    }

    // gamestart �г� Ȱ��ȭ //
    public void ShowGameStartPanel(int currentDay)
    {
        _startPanel.SetActive(true);

        TextMeshProUGUI dayText = _startPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dayText.text = $"ROUND {(UIManager.Instance.PosPanel.CurrentRound)+1}";
        SoundManager.Instance.PlayFX("GameStart");
    }

    // orderstatus �г� Ȱ��ȭ //
    public void ShowOrderStatusPanel(bool orderStatus)
    {
        _statusPanel.SetActive(true);

        TextMeshProUGUI statusText = _statusPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        statusText.text = orderStatus ? "�ֹ� ó�� ����" : "�ֹ� ó�� ����";
        string fxFileName = orderStatus ? "PackageSuccess" : "PackageFail";
        SoundManager.Instance.PlayFX(fxFileName);
    }

    // gameover �г� Ȱ��ȭ //
    public void ShowGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        SoundManager.Instance.PlayFX("PackageFail");
    }

    // �г� ��� ��Ȱ��ȭ //
    public void HideAllPanels()
    {
        _startPanel.SetActive(false);
        _statusPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
    }

    // Ȯ�� ��ư Ŭ�� �� �θ� �г� ��Ȱ��ȭ //
    public void HidePopUpPanel(Button okButton)
    {
        GameObject parentPanel = okButton.transform.parent.gameObject;
        if (parentPanel != null)
        {
            parentPanel.SetActive(false);
        }
    }

    // Ȯ�� ��ư �̺�Ʈ �Ҵ� //
    private void AssignOkButtonEvents()
    {
        Button startOkButton = _startPanel.GetComponentInChildren<Button>();
        if (startOkButton != null)
        {
            startOkButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(startOkButton);                            // start �г� ��Ȱ��ȭ
                StartCoroutine(GameManager.Instance.TimeUpdateRoutine()); // TimeUpdateRoutine ����
            });
        }

        Button statusOkButton = _statusPanel.GetComponentInChildren<Button>();
        if (statusOkButton != null)
        {
            statusOkButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(statusOkButton);      // status �г� ��Ȱ��ȭ
            });
        }

        Button gameOverOKButton = _gameOverPanel.GetComponentInChildren<Button>();
        if (gameOverOKButton != null)
        {
            gameOverOKButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(gameOverOKButton);               // gameover �г� ��Ȱ��ȭ
                SceneManager.LoadScene("StartScene");
            });
        }
    }

    public bool IsGameStartPanelActive()
    {
        return _startPanel.activeSelf;
    }

    public bool IsOrderStatusPanelActive()
    {
        return _statusPanel.activeSelf;
    }

    public bool IsDayEndPanelActive()
    {
        return _gameOverPanel.activeSelf;
    }
}
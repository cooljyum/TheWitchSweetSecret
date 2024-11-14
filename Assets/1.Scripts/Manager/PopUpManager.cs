using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;    // 게임 시작창
    [SerializeField] private GameObject _statusPanel;   // 성공/실패창
    [SerializeField] private GameObject _gameOverPanel; // 게임오버창

    private void Start()
    {
        HideAllPanels();
        AssignOkButtonEvents();
    }

    // gamestart 패널 활성화 //
    public void ShowGameStartPanel(int currentDay)
    {
        _startPanel.SetActive(true);

        TextMeshProUGUI dayText = _startPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dayText.text = $"ROUND {(UIManager.Instance.PosPanel.CurrentRound)+1}";
        SoundManager.Instance.PlayFX("GameStart");
    }

    // orderstatus 패널 활성화 //
    public void ShowOrderStatusPanel(bool orderStatus)
    {
        _statusPanel.SetActive(true);

        TextMeshProUGUI statusText = _statusPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        statusText.text = orderStatus ? "주문 처리 성공" : "주문 처리 실패";
        string fxFileName = orderStatus ? "PackageSuccess" : "PackageFail";
        SoundManager.Instance.PlayFX(fxFileName);
    }

    // gameover 패널 활성화 //
    public void ShowGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        SoundManager.Instance.PlayFX("PackageFail");
    }

    // 패널 모두 비활성화 //
    public void HideAllPanels()
    {
        _startPanel.SetActive(false);
        _statusPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
    }

    // 확인 버튼 클릭 시 부모 패널 비활성화 //
    public void HidePopUpPanel(Button okButton)
    {
        GameObject parentPanel = okButton.transform.parent.gameObject;
        if (parentPanel != null)
        {
            parentPanel.SetActive(false);
        }
    }

    // 확인 버튼 이벤트 할당 //
    private void AssignOkButtonEvents()
    {
        Button startOkButton = _startPanel.GetComponentInChildren<Button>();
        if (startOkButton != null)
        {
            startOkButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(startOkButton);                            // start 패널 비활성화
                StartCoroutine(GameManager.Instance.TimeUpdateRoutine()); // TimeUpdateRoutine 시작
            });
        }

        Button statusOkButton = _statusPanel.GetComponentInChildren<Button>();
        if (statusOkButton != null)
        {
            statusOkButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(statusOkButton);      // status 패널 비활성화
            });
        }

        Button gameOverOKButton = _gameOverPanel.GetComponentInChildren<Button>();
        if (gameOverOKButton != null)
        {
            gameOverOKButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(gameOverOKButton);               // gameover 패널 비활성화
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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _statusPanel;
    [SerializeField] private GameObject _endPanel;

    private void Start()
    {
        HideAllPanels();
        AssignOkButtonEvents();
    }

    // daystart �г� Ȱ��ȭ //
    public void ShowDayStartPanel(int currentDay)
    {
        _startPanel.SetActive(true);

        TextMeshProUGUI dayText = _startPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dayText.text = $"Day{UIManager.Instance.CurrentDayNumber}";
    }

    // orderstatus �г� Ȱ��ȭ //
    public void ShowOrderStatusPanel(bool orderStatus)
    {
        _statusPanel.SetActive(true);

        TextMeshProUGUI statusText = _statusPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        statusText.text = orderStatus ? "�ֹ� ó�� ����" : "�ֹ� ó�� ����";
    }

    // dayend �г� Ȱ��ȭ //
    public void ShowDayEndPanel()
    {
        _endPanel.SetActive(true);

        TextMeshProUGUI resultText = _endPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        resultText.text = $"������ �ֹ� {UIManager.Instance.SuccessCount}\n������ �ֹ� {UIManager.Instance.FailureCount}\n������ ���� {UIManager.Instance.StagePanel.StarPointByStage[UIManager.Instance.CurrentDayNumber]}";
    }

    // �г� ��� ��Ȱ��ȭ //
    public void HideAllPanels()
    {
        _startPanel.SetActive(false);
        _statusPanel.SetActive(false);
        _endPanel.SetActive(false);
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
                HidePopUpPanel(startOkButton);            // start �г� ��Ȱ��ȭ
                UIManager.Instance.StartDayTime();        // StartDayTime ȣ��
            });
        }

        Button statusOkButton = _statusPanel.GetComponentInChildren<Button>();
        if (statusOkButton != null)
        {
            statusOkButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(statusOkButton);                 // status �г� ��Ȱ��ȭ
                UIManager.Instance.PosPanel.CreateNewOrder();   // CreateNewOrder ȣ��
            });
        }

        Button endOkButton = _endPanel.GetComponentInChildren<Button>();
        if (endOkButton != null)
        {
            endOkButton.onClick.AddListener(() =>
            {
                HidePopUpPanel(endOkButton);               // end �г� ��Ȱ��ȭ
                UIManager.Instance.PrepareForNextDay();    // PrepareForNextDay ȣ��
            });
        }
    }

    public bool IsDayStartPanelActive()
    {
        return _startPanel.activeSelf;
    }

    public bool IsOrderStatusPanelActive()
    {
        return _statusPanel.activeSelf;
    }

    public bool IsDayEndPanelActive()
    {
        return _endPanel.activeSelf;
    }
}
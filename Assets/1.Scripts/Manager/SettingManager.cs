using UnityEngine;

public class SettingManager : MonoBehaviour
{

    [SerializeField] private GameObject _settingsBtn; // ���� �����ϴ� ���� ��ư ������Ʈ
    [SerializeField] private GameObject _settingsPanel;  // ���� �����ϴ� ���� �г� ������Ʈ

    [SerializeField] private GameObject bgmOnObj; // BGM ����� On ���� ������Ʈ
    [SerializeField] private GameObject bgmOffObj; // BGM ����� Off ���� ������Ʈ
    [SerializeField] private GameObject effectOnObj; // Effect ����� On ���� ������Ʈ
    [SerializeField] private GameObject effectOffObj; // Effect ����� Off ���� ������Ʈ

    private bool isBGMOn = true; // �ʱ� BGM ����
    private bool isEffectOn = true; // �ʱ� Effect ����

    private void Start()
    {
        InitializeSettingsUI();
    }

    // ���� UI �ʱ�ȭ �Լ�
    private void InitializeSettingsUI()
    {
        _settingsBtn.SetActive(true); // ���� ��ư Ȱ��ȭ
        _settingsPanel.SetActive(false); // ���� �г� ��Ȱ��ȭ

        // SoundManager�� ���� ���¸� �޾ƿ� �ʱⰪ ����
        isBGMOn = SoundManager.Instance.GetBGVolume() > 0;
        isEffectOn = SoundManager.Instance.GetFXVolume() > 0;

        UpdateBGMButtonState();
        UpdateEffectButtonState();
    }

    // ���� �г��� Ȱ��ȭ �� ��Ȱ��ȭ�ϴ� �Լ�
    public void ToggleSettingsPanel()
    {
        if (_settingsPanel != null)
        {
            _settingsPanel.SetActive(!_settingsPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Settings panel not initialized.");
        }
    }

    // BGM ��� ��ư
    public void ToggleBGM()
    {
        isBGMOn = !isBGMOn;
        SoundManager.Instance.SetBGVolume(isBGMOn ? 1f : 0f);
        UpdateBGMButtonState();
    }

    // Effect ��� ��ư
    public void ToggleEffect()
    {
        isEffectOn = !isEffectOn;
        SoundManager.Instance.SetFXVolume(isEffectOn ? 1f : 0f);
        UpdateEffectButtonState();
    }

    // BGM ��ư ���� ������Ʈ
    private void UpdateBGMButtonState()
    {
        bgmOnObj.SetActive(isBGMOn);
        bgmOffObj.SetActive(!isBGMOn);
    }

    // Effect ��ư ���� ������Ʈ
    private void UpdateEffectButtonState()
    {
        effectOnObj.SetActive(isEffectOn);
        effectOffObj.SetActive(!isEffectOn);
    }
}

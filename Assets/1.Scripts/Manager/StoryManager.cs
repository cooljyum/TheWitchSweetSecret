using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GameObject _storyPanel;
    [SerializeField] private Image _catWing;
    [SerializeField] private Image _catFace;
    [SerializeField] private Image _witchFace;
    [SerializeField] private Button _skipButton;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private int _currentIndex = 0;
    private Coroutine _wingCoroutine;
    private Coroutine _typingCoroutine;

    private void Start()
    {
        _skipButton.onClick.AddListener(OnSkipButtonClicked);
        ShowDialogue(_currentIndex);
        SoundManager.Instance.PlayBG("StoryBGM1");
    }

    private void Update()
    {
        // ��Ŭ������ ��� ����
        if (Input.GetMouseButtonDown(1))
        {
            _currentIndex++;
            ShowDialogue(_currentIndex);
        }
    }

    private void ShowDialogue(int index)
    {
        var entry = _dialogueManager.GetDialogueEntry(index);

        if (entry != null)
        {
            // Ÿ���� ȿ���� ���� �ڷ�ƾ ����
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }
            _typingCoroutine = StartCoroutine(TypeDialogue(entry.Dialogue));

            // ĳ���ͺ� ǥ�� ������Ʈ
            UpdateCharacterEmotion(entry.Speaker, entry.CatEmotion, entry.WitchEmotion);
        }
        else
        {
            // ��簡 ������ ��
            EndDialogue();
        }
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        _dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // ���� ���� ������ (���� ����)
        }
    }

    private void UpdateCharacterEmotion(string speaker, string catEmotion, string witchEmotion)
    {
        // ����� ǥ�� ������Ʈ
        _catFace.sprite = GetEmotionSprite("Cat", catEmotion);

        // ����� ����� ���� ���� �ִϸ��̼� ����
        if (speaker == "Cat")
        {
            if (_wingCoroutine == null)
            {
                _wingCoroutine = StartCoroutine(WingAnimation());
            }
        }
        else
        {
            // ���డ ��� ���� ���� ���� �ִϸ��̼� ����
            if (_wingCoroutine != null)
            {
                StopCoroutine(_wingCoroutine);
                _wingCoroutine = null;
                _catWing.sprite = Resources.Load<Sprite>("Sprite/Character/Cat/Cat_WingDown"); // �⺻ ���·� ����
            }
        }

        // ���� ǥ�� ������Ʈ
        _witchFace.sprite = GetEmotionSprite("Witch", witchEmotion);
    }

    private Sprite GetEmotionSprite(string speaker, string emotion)
    {
        if (speaker == "Cat")
        {
            return Resources.Load<Sprite>($"Sprite/Character/Cat/Cat_{emotion}");
        }
        else
        {
            return Resources.Load<Sprite>($"Sprite/Character/Witch/Witch_{emotion}");
        }
    }

    private IEnumerator WingAnimation()
    {
        Sprite wingDown = Resources.Load<Sprite>("Sprite/Character/Cat/Cat_WingDown");
        Sprite wingUp = Resources.Load<Sprite>("Sprite/Character/Cat/Cat_WingUp");

        while (true)
        {
            _catWing.sprite = wingUp;
            yield return new WaitForSeconds(1f);
            _catWing.sprite = wingDown;
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnSkipButtonClicked()
    {
        EndDialogue();
    }

    private void EndDialogue()
    {
        // ���� �ִϸ��̼� ����
        if (_wingCoroutine != null)
        {
            StopCoroutine(_wingCoroutine);
            _wingCoroutine = null;
        }

        // Ÿ���� ȿ�� ����
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        SceneManager.LoadScene("MainScene");
    }
}
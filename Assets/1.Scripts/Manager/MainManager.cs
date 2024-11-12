using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [SerializeField] private Text scoreText;                // ȣ���� ǥ�ÿ� �ؽ�Ʈ UI

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayBG("MainBGM");

    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text =  HeartManager.Instance.GetCurScore();
        }
    }
}

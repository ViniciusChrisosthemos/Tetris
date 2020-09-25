using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Unity Editor Variables
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private List<Sprite> fullBlocksSprites;
    [SerializeField] private List<Image> fullBlocksQueue;
    [SerializeField] private PauseScreenController pauseScreen;
    [SerializeField] private string pauseScreenSoundName;
    #endregion

    #region Singleton
    public static UIManager Instance { get; private set; }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region UIManager Functions
    public void UpdateBlockQueue(int[] _queue)
    {
        for (int i = 0; i < fullBlocksQueue.Count; i++)
        {
            fullBlocksQueue[i].sprite = fullBlocksSprites[_queue[i]];
        }
    }

    public void SetScore(int _score)
    {
        scoreText.text = $"Score: {_score}";
    }

    public void SetNextLevelXp(int _nextLevelXp)
    {
        xpSlider.maxValue = _nextLevelXp;
    }

    public void SetXp(int _xp)
    {
        xpText.text = $"{_xp} / {xpSlider.maxValue}";
        xpSlider.value = _xp;
    }

    public void SetLevel(int _level)
    {
        levelText.text = $"Level {_level}";
    }

    public void SetCurrentCombo(int _currentCombo)
    {
        comboText.text = $"Combo {_currentCombo}x";
    }

    public void SetPauseScreen(bool _value)
    {
        if (_value)
            pauseScreen.gameObject.SetActive(true);
        
        pauseScreen.SetActive(_value);
        SoundManager.Instance.PlaySound(pauseScreenSoundName);
    }
    #endregion
}

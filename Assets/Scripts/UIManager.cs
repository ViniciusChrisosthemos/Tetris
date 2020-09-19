using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI nextLevelXp;

    [SerializeField] private List<Sprite> fullBlocksSprites;
    [SerializeField] private List<Image> fullBlocksQueue;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBlockQueue(int[] _queue)
    {
        for (int i = 0; i < fullBlocksQueue.Count; i++)
        {
            fullBlocksQueue[i].sprite = fullBlocksSprites[_queue[i]];
        }
    }

    public void SetScore(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void SetCombo(float _combo)
    {
        comboText.text = string.Format("{0:0.00}x", _combo);
    }

    public void SetXpInfo(int _level, int _xp, int _nextLevelXp)
    {
        levelText.text = _level.ToString();
        xpText.text = _xp.ToString();
        nextLevelXp.text = _nextLevelXp.ToString();
    }
}

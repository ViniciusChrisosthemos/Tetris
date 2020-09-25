using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameMaster : MonoBehaviour
{
    #region Singleton
    public static GameMaster Instance { get; private set; }
    #endregion

    #region Unity Editor Variables
    [Header("Gameplay")]
    [SerializeField] private Board board;

    [Header("UI")]
    [SerializeField] private List<GameObject> blocksPrefabs;

    [Header("SFX")]
    [SerializeField] private List<string> lineDeletedSounds;
    [SerializeField] private string blockAppliedAudio;
    [SerializeField] private GameObject explosionParticles;
    #endregion

    #region Control Variables
    private Random random;
    private Transform[,] grid;
    private Queue<int> blocksQueue;
    private TetrisBlock block;
    #endregion

    #region Gameplay Configuration
    private int score = 0;
    private int pointsPerLine = 100;

    private int comboStreak = 0;
    private int comboValue = 0;

    private int xp = 0;
    private int level = 1;
    private int xpPerLine = 10;
    private int defaultXpLevel = 50;
    private int nextLevelXp;
    private float xpRate = 1.2f;

    private float defaultBlockSpeed = .8f;
    private float blockSpeed;
    private float speedRate = .8f;

    public int NextLevelXp
    {
        get => nextLevelXp;
        set
        {
            nextLevelXp = value;
            UIManager.Instance.SetNextLevelXp(nextLevelXp);
        }
    }
    public float BlockSpeed
    {
        get => blockSpeed;
        set
        {
            blockSpeed = value;
        }
    }
    public int Level
    {
        get => level;
        set
        {
            level = value;
            UIManager.Instance.SetLevel(level);
        }
    }
    public int Xp
    {
        get => xp;
        set
        {
            xp = value;
            UIManager.Instance.SetXp(xp);
        }
    }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            UIManager.Instance.SetScore(score);
        }
    }

    public int ComboValue
    {
        get => comboValue;
        set
        {
            comboValue = value;
            UIManager.Instance.SetCurrentCombo(comboValue);
        }
    }

    #endregion

    #region Unity Functions
    private void Awake()
    {
        Instance = this;

        grid = new Transform[board.BoardWidth, board.BoardHeight];
        random = new Random();

        StartBlockQueue();

        ResetConfigs();
    }

    private void Start()
    {
        SpawnBlock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 0)
            {
                block.enabled = true;
                UIManager.Instance.SetPauseScreen(false);
                Time.timeScale = 1;
            }
            else
            {
                block.enabled = false;
                UIManager.Instance.SetPauseScreen(true);
                Time.timeScale = 0;
            }
                
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    #endregion

    #region GameMaster Functions
    private void ResetConfigs()
    {
        NextLevelXp = defaultXpLevel;
        Xp = 0;
        Score = 0;
        Level = 1;
        ComboValue = 0;
        BlockSpeed = defaultBlockSpeed;
    }

    private void StartBlockQueue()
    {
        int queueCapacity = 4;
        blocksQueue = new Queue<int>(queueCapacity);

        for (int i = 0; i < queueCapacity; i++)
        {
            blocksQueue.Enqueue(random.Next(blocksPrefabs.Count));
        }
    }

    public void SpawnBlock()
    {
        block = Instantiate(blocksPrefabs[blocksQueue.Dequeue()], board.SpawnPosition, Quaternion.identity).GetComponent<TetrisBlock>();
        block.SetSpeed(blockSpeed);

        if (!CheckBlockPosition(block.Blocks))
        {
            Destroy(block.gameObject);
            RestartGame();
        }
        else
        {
            blocksQueue.Enqueue(random.Next(blocksPrefabs.Count));
            UIManager.Instance.UpdateBlockQueue(blocksQueue.ToArray());
        }
    }

    private void RestartGame()
    {
        for (int x = 0; x < board.BoardWidth; x++)
        {
            for (int y = 0; y < board.BoardHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }

        ResetConfigs();
        StartBlockQueue();
        SpawnBlock();
    }

    public void ApplyBlock(Transform[] blocks)
    {
        int _roundedX, _roundedY;

        foreach (Transform _transform in blocks)
        {
            _roundedX = Mathf.RoundToInt(_transform.position.x);
            _roundedY = Mathf.RoundToInt(_transform.position.y);

            grid[_roundedX, _roundedY] = _transform;

        }

        SoundManager.Instance.PlaySound(blockAppliedAudio);
    }

    public bool CheckBlockPosition(Transform[] blocks)
    {
        int _roundedX, _roundedY;

        foreach (Transform _transform in blocks)
        {
            _roundedX = Mathf.RoundToInt(_transform.position.x);
            _roundedY = Mathf.RoundToInt(_transform.position.y);

            if (_roundedX < 0 || _roundedX >= board.BoardWidth || _roundedY < 0 || _roundedY > board.BoardHeight)
                return false;

            if (grid[_roundedX, _roundedY] != null)
                return false;
        }

        return true;
    }

    public void CheckLines()
    {
        int linesDeleted = 0;

        for (int _y = 0; _y < board.BoardHeight; _y++)
        {
            if (HasLine(_y))
            {
                linesDeleted++;

                DestroyLine(_y);
                RowDown(_y);
                _y--;
            }
        }

        if (linesDeleted > 0)
        {
            comboStreak++;
            ComboValue += linesDeleted;
            Score += (int)(ComboValue * pointsPerLine);

            Xp += (int)(xpPerLine * ComboValue);

            if (Xp >= nextLevelXp)
            {
                Level++;
                NextLevelXp = (int)(NextLevelXp * xpRate);
                Xp = 0;

                BlockSpeed *= speedRate; 
            }

            SoundManager.Instance.PlaySound(lineDeletedSounds[Math.Min(comboStreak - 1, lineDeletedSounds.Count-1)]);
        }
        else
        {
            comboStreak = 0;
            ComboValue = 0;
        }

    }

    private bool HasLine(int _y)
    {
        for (int _x = 0; _x < board.BoardWidth; _x++)
        {
            if (grid[_x, _y] == null)
                return false;
        }

        return true;
    }

    private void DestroyLine(int _y)
    {
        for (int _x = 0; _x < board.BoardWidth; _x++)
        {
            Instantiate(explosionParticles, grid[_x, _y].position, Quaternion.identity);
            Destroy(grid[_x, _y].gameObject);
            grid[_x, _y] = null;
        }
    }

    private void RowDown(int _y)
    {
        for (; _y < board.BoardHeight; _y++)
        {
            for (int _x = 0; _x < board.BoardWidth; _x++)
            {
                if (grid[_x, _y] != null)
                {
                    grid[_x, _y - 1] = grid[_x, _y];
                    grid[_x, _y] = null;
                    grid[_x, _y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
    #endregion
}

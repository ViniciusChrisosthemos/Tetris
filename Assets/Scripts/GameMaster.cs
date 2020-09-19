using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }

    [SerializeField] private Board board;
    [SerializeField] private List<GameObject> blocksPrefabs;
    [SerializeField] private List<string> lineDeletedSounds;

    private Queue<int> blocksQueue;
    private Random random;

    private Transform[,] grid;

    private int score = 0;
    private int pointsPerLine = 100;
    private float currentCombo = 0;
    private float comboRate = .5f;

    private int xp = 0;
    private int level = 1;
    private int xpPerLine = 10;
    private int defaultXpLevel = 50;
    private int nextLevelXp;
    private float xpRate = 1.2f;

    private float defaultBlockSpeed = .8f;
    private float blockSpeed;
    private float speedRate = .8f;

    private void Awake()
    {
        Instance = this;

        grid = new Transform[board.BoardWidth, board.BoardHeight];
        random = new Random();

        StartBlockQueue();

        ResetConfigs();
    }

    private void ResetConfigs()
    {
        blockSpeed = defaultBlockSpeed;
        score = 0;
        currentCombo = 0;
        level = 1;
        xp = 0;
        nextLevelXp = (int)(defaultXpLevel * xpRate * level);

        UIManager.Instance.SetXpInfo(level, xp, nextLevelXp);
        UIManager.Instance.SetScore(score);
        UIManager.Instance.SetCombo(currentCombo);
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

    private void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Block _block = Instantiate(blocksPrefabs[blocksQueue.Dequeue()], board.SpawnPosition, Quaternion.identity).GetComponent<Block>();
        _block.SetSpeed(blockSpeed);

        if (!CheckBlockPosition(_block.Blocks))
        {
            Destroy(_block.gameObject);
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
                    Destroy(grid[x, y].gameObject);
            }
        }

        ResetConfigs();
        StartBlockQueue();
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
            currentCombo += comboRate * linesDeleted + .5f;
            score += (int)(currentCombo * 100);

            xp += (int)(xpPerLine * currentCombo);

            if (xp >= nextLevelXp)
            {
                level++;
                xp = 0;
                nextLevelXp = (int)(defaultXpLevel * xpRate);

                blockSpeed *= speedRate; 
            }

            UIManager.Instance.SetXpInfo(level, xp, nextLevelXp);
            UIManager.Instance.SetScore(score);
            UIManager.Instance.SetCombo(currentCombo);

            SoundManager.Instance.PlaySound(lineDeletedSounds[Math.Min(linesDeleted, lineDeletedSounds.Count-1)]);
        }
        else
        {
            currentCombo = 0;
            UIManager.Instance.SetCombo(currentCombo);
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
}

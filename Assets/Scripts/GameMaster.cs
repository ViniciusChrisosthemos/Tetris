using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private int boardWidth = 10;
    [SerializeField] private int boardHeight = 15;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject board;

    private int[,] grid;

    public static GameMaster Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        grid = new int[boardWidth, boardHeight];

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                grid[i, j] = 0;
            }
        }

        board.transform.localScale = new Vector3(boardWidth, boardHeight);
        board.transform.position = new Vector3(boardWidth/2f, boardHeight/2f);
        spawnPoint.transform.position = new Vector3(boardWidth / 2, boardHeight-4);
    }

    private void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void ApplyBlock(Transform[] blocks)
    {
        int _roundedX, _roundedY;

        foreach (Transform _transform in blocks)
        {
            _roundedX = Mathf.RoundToInt(_transform.position.x);
            _roundedY = Mathf.RoundToInt(_transform.position.y);

            grid[_roundedX, _roundedY] = 1;
            
        }
    }

    public bool CheckBlockPosition(Transform[] blocks)
    {
        int _roundedX, _roundedY;

        foreach (Transform _transform in blocks)
        {
            _roundedX = Mathf.RoundToInt(_transform.position.x);
            _roundedY = Mathf.RoundToInt(_transform.position.y);

            if (_roundedX < 0 || _roundedX >= boardWidth || _roundedY < 0 || _roundedY > boardHeight)
                return false;

            if (grid[_roundedX, _roundedY] == 1)
                return false;
        }

        return true;
    }
}

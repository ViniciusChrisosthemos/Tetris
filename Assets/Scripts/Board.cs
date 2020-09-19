using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Board : MonoBehaviour
{
    [SerializeField] private int boardWidth = 10;
    [SerializeField] private int boardHeight = 15;

    public Vector3 SpawnPosition { get; private set; }

    public int BoardWidth => boardWidth;
    public int BoardHeight => boardHeight;

    private void Awake()
    {
        ReshapeBoard();
    }

    private void ReshapeBoard()
    {
        transform.localScale = new Vector3(boardWidth, boardHeight);
        transform.position = new Vector3(boardWidth / 2f - 0.5f, boardHeight / 2f - 0.5f);

        SpawnPosition = new Vector3((int)transform.position.x, boardHeight - 4);
    }

    private void OnValidate()
    {
        ReshapeBoard();
    }
}

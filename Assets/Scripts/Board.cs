using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Board : MonoBehaviour
{
    #region Unity Editor Variables
    [SerializeField] private int boardWidth = 10;
    [SerializeField] private int boardHeight = 15;
    #endregion

    #region Control Variables
    public Vector3 SpawnPosition { get; private set; }
    public int BoardWidth => boardWidth;
    public int BoardHeight => boardHeight;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        ReshapeBoard();
    }
    private void OnValidate()
    {
        ReshapeBoard();
    }
    #endregion

    #region Board Functions
    private void ReshapeBoard()
    {
        transform.localScale = new Vector3(boardWidth, boardHeight);
        transform.position = new Vector3(boardWidth / 2f - 0.5f, boardHeight / 2f - 0.5f);

        SpawnPosition = new Vector3((int)transform.position.x, boardHeight - 4);
    }
    #endregion
}

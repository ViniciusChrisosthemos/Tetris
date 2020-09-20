using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    #region Unity Editor Variables
    [Header("SFX")]
    [SerializeField] private string moveAudioName;
    [SerializeField] private string blockedMoveAudioName;
    [SerializeField] private string rotateAudioName;
    #endregion

    #region Control Variables
    public Transform[] Blocks { get; private set; }
    private Transform myTransform;
    public Vector3 rotationPoint;
    #endregion

    #region Gameplay Configuration
    private float moveInterval = 0.08f;
    private float blockSpeed = 1;
    private float nextMove = 0;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        myTransform = GetComponent<Transform>();

        int i = 0;
        Transform[] _blocks = GetComponentsInChildren<Transform>();

        Blocks = new Transform[_blocks.Length-1];
        foreach (Transform _tranform in _blocks)
        {
            if (_tranform != myTransform)
                Blocks[i++] = _tranform;
        }
    }

    private void Update()
    {
        nextMove += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveBlock(new Vector3(-1, 0, 0));


        }else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveBlock(new Vector3(1, 0, 0));

        }else if (Input.GetKeyDown(KeyCode.W))
        {
            RotateBlock();
        }

        if (nextMove > (Input.GetKey(KeyCode.S) ? moveInterval:blockSpeed))
        {
            nextMove = 0;
            if (!MoveBlock(new Vector3(0, -1, 0)))
            {
                GameMaster.Instance.ApplyBlock(Blocks);
                GameMaster.Instance.CheckLines();
                GameMaster.Instance.SpawnBlock();

                //myScript.enabled = false;
                DestroyBlock();
            }
        }
    }
    #endregion

    #region Block Functions
    private void DestroyBlock()
    {
        foreach (Transform _block in Blocks)
        {
            _block.parent = GameMaster.Instance.gameObject.transform;
        }

        Destroy(gameObject);
    }

    private void RotateBlock()
    {
        //myTransform.Rotate(new Vector3(0, 0, 90));
        myTransform.RotateAround(myTransform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

        if (!GameMaster.Instance.CheckBlockPosition(Blocks))
        {
            myTransform.RotateAround(myTransform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            SoundManager.Instance.PlaySound(blockedMoveAudioName);
        }
        else
        {
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].transform.rotation = Quaternion.identity;
            }

            SoundManager.Instance.PlaySound(moveAudioName);
        }
            
        //myTransform.Rotate(new Vector3(0, 0, -90));
    }

    public void SetSpeed(float _blockSpeed)
    {
        blockSpeed = _blockSpeed;
    }

    private bool MoveBlock(Vector3 _direction)
    {
        myTransform.position += _direction;

        if (!GameMaster.Instance.CheckBlockPosition(Blocks))
        {
            myTransform.position += _direction * -1;

            SoundManager.Instance.PlaySound(blockedMoveAudioName);
            return false;
        }

        SoundManager.Instance.PlaySound(moveAudioName);
        return true;
    }
    #endregion
}

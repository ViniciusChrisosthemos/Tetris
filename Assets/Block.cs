using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Transform myTransform;
    public Vector3 rotationPoint;
    private float moveInterval = 0.5f;
    private float nextMove = 0;

    private Transform[] blocks;

    private Block myScript;

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
        myScript = GetComponent<Block>();

        int i = 0;
        Transform[] _blocks = GetComponentsInChildren<Transform>();

        blocks = new Transform[_blocks.Length-1];
        foreach (Transform _tranform in _blocks)
        {
            if (_tranform != myTransform)
                blocks[i++] = _tranform;
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

        if (nextMove > (Input.GetKeyDown(KeyCode.S) ? moveInterval / 10 : moveInterval))
        {
            if (!MoveBlock(new Vector3(0, -1, 0)))
            {
                GameMaster.Instance.ApplyBlock(blocks);
                GameMaster.Instance.SpawnBlock();

                myScript.enabled = false;
            }
        }
    }

    private void RotateBlock()
    {
        //myTransform.Rotate(new Vector3(0, 0, 90));
        myTransform.RotateAround(myTransform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

        if (!GameMaster.Instance.CheckBlockPosition(blocks))
            myTransform.RotateAround(myTransform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        //myTransform.Rotate(new Vector3(0, 0, -90));
    }

    private bool MoveBlock(Vector3 _direction)
    {
        nextMove = 0;

        myTransform.position += _direction;

        if (!GameMaster.Instance.CheckBlockPosition(blocks))
        {
            myTransform.position += _direction * -1;
            return false;
        }

        return true;
    }
}

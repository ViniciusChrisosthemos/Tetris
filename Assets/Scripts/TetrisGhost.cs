using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGhost : MonoBehaviour
{
    private Vector3 rotationPoint;
    private Transform myTransform;
    public Transform[] Blocks { get; private set; }

    private void Awake()
    {
        myTransform = GetComponent<Transform>();

        int i = 0;
        Transform[] _blocks = GetComponentsInChildren<Transform>();

        Blocks = new Transform[_blocks.Length - 1];
        foreach (Transform _tranform in _blocks)
        {
            if (_tranform != myTransform)
                Blocks[i++] = _tranform;
        }
    }

    public void UpdateGhost(Vector3 _position)
    {
        myTransform.position = _position;

        while (MoveBlockDown()) { continue; }
    }


    public void SetRotation(Quaternion _rotation)
    {
        myTransform.rotation = _rotation;

        for (int i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].transform.rotation = Quaternion.identity;
        }
    }

    private bool MoveBlockDown()
    {
        myTransform.position += Vector3.down;

        if (!GameMaster.Instance.CheckBlockPosition(Blocks))
        {
            myTransform.position += Vector3.up;
         
            return false;
        }

        return true;
    }
}

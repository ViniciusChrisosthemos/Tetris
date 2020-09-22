using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGhost : MonoBehaviour
{
    [SerializeField] private GameObject blocksAnchor;

    private Transform myTransform;
    private Animator animator;

    public Transform[] Blocks { get; private set; }

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        int i = 0;
        Transform[] _blocks = blocksAnchor.GetComponentsInChildren<Transform>();

        Blocks = new Transform[_blocks.Length - 1];
        foreach (Transform _tranform in _blocks)
        {
            if (_tranform != blocksAnchor.transform)
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

    public void DestroyBlock()
    {
        StartCoroutine(AnimateAndDestroy());
    }

    private IEnumerator AnimateAndDestroy()
    {
        animator.SetTrigger("DestroyGhost");

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

}

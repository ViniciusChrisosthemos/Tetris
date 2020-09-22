using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetActive(bool _value)
    {
        animator.SetBool("active", _value);
    }
}

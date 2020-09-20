using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class Block : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("inGame", true);
        }
    }
}
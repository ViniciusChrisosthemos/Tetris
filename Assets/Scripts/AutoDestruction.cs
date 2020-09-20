using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
}

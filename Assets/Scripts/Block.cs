using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private Color explosionColor;

    private void OnDestroy()
    {
        //BlockDestruction blockDestruction = Instantiate(explosionEffect, transform.position, Quaternion.identity).GetComponent<BlockDestruction>();
        //blockDestruction.SetColor(explosionColor);
    }
}
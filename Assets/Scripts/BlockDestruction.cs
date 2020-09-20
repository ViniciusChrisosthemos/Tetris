using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestruction : MonoBehaviour
{
    private ParticleSystem.MainModule myParticleSystem;

    private void Awake()
    {
        myParticleSystem = GetComponent<ParticleSystem>().main;
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void SetColor(Color _color)
    {
        myParticleSystem.startColor = _color;
    }
}

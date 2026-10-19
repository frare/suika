using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MergeParticle : MonoBehaviour
{
    public Action OnFinished;

    private ParticleSystem particlesSystem;

    private void Awake()
    {
        particlesSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particlesSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        particlesSystem.Play();
    }
}
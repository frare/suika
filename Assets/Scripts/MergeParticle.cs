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
        var main = particlesSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnEnable()
    {
        particlesSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        particlesSystem.Play();
    }

    // called automatically through main's ParticleSystemStopAction.Callback
    private void OnParticleSystemStopped()
    {
        OnFinished?.Invoke();
    }
}
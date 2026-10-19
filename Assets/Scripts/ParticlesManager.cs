using frare;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlesManager : Singleton<ParticlesManager>
{
    protected override bool DontDestroyWhenLoad => false;

    [SerializeField] private MergeParticle mergeParticlePrefab;

    private ObjectPool<MergeParticle> mergeParticlePool;

    protected override void Awake()
    {
        mergeParticlePool = new (
            createFunc: () =>
            {
                var particle = Instantiate(mergeParticlePrefab.gameObject).GetComponent<MergeParticle>();
                particle.OnFinished += () => mergeParticlePool.Release(particle);
                return particle;
            },
            actionOnGet: (particle) => particle.gameObject.SetActive(true),
            actionOnRelease: (particle) => particle.gameObject.SetActive(false),
            actionOnDestroy: (particle) => Destroy(particle.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 25
        );
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void PlayMergeParticles(Vector3 position)
    {
        var particles = mergeParticlePool.Get();
        particles.transform.position = position;
    }
}
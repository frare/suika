using System.Collections;
using UnityEngine;
using frare.CoroutineHelper;
using DG.Tweening;

public class UIOrderAnimation : MonoBehaviour
{
    [SerializeField] private float idleScale;
    [SerializeField] private float animationScale;
    [SerializeField] private float animationTime;
    [SerializeField] private Transform[] images;

    private Coroutine animationCoroutine;
    private WaitForSeconds waitForSeconds;

    private void Awake()
    {
        waitForSeconds = new(animationTime / 2f);
        CoroutineHelper.StartCoroutineIfNone(this, ref animationCoroutine, BounceAnimation());
    }

    private void OnDestroy()
    {
        foreach (var image in images) image.DOKill();
    }

    private IEnumerator BounceAnimation()
    {
        for (int i = 0; i < images.Length; i++)
        {
            Transform target = images[i];

            DOTween.Sequence()
                .Append(target.DOScale(animationScale, animationTime).SetEase(Ease.OutBack))
                .AppendInterval(animationTime / 2f)
                .Append(target.DOScale(idleScale, animationTime).SetEase(Ease.InOutSine));

            yield return waitForSeconds;
        }

        CoroutineHelper.StartCoroutineOverride(this, ref animationCoroutine, BounceAnimation());
    }
}
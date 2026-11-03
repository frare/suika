using System.Collections;
using UnityEngine;
using TMPro;
using frare.CoroutineHelper;

[RequireComponent(typeof(TMP_Text))]
public class AnimatedCounter : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 100f;

    private float currentValue = 0;
    private TMP_Text counterTmp;
    private Coroutine counterCoroutine;

    private void Awake()
    {
        counterTmp = GetComponent<TMP_Text>();
        UpdateCounterText(0, false);
    }

    public void UpdateCounterText(int value, bool animate = true)
    {
        if (animate) CoroutineHelper.StartCoroutineOverride(this, ref counterCoroutine, AnimateCounter(value));
        else
        {
            currentValue = value;
            counterTmp.text = currentValue.ToString();
        }
    }

    private IEnumerator AnimateCounter(int target)
    {
        for (float t = currentValue; t < target; t += animationSpeed * Time.unscaledDeltaTime)
        {
            currentValue = t;
            counterTmp.text = Mathf.RoundToInt(currentValue).ToString();
            yield return null;
        }

        currentValue = target;
        counterTmp.text = currentValue.ToString();
    }
}
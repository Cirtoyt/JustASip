using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSitting : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private int maxStateTime;
    [SerializeField] private int minStateTime;

    private List<string> animationStates = new() { "talk", "text", "eat", "drink" };

private void Start()
    {
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        while (animator != null)
        {
            animator.SetTrigger(animationStates[Random.Range(0, animationStates.Count)]);

            int animationTime = Random.Range(minStateTime, maxStateTime);
            yield return new WaitForSeconds(animationTime);

            yield return null;
        }
    }
}

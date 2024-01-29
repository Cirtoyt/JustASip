using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverUpAndDown : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float speed = 1;
    [SerializeField] private float spinSpeed = 45;
    [SerializeField] float bounceSize = 1;
    private float timer = 0;

    private float originalY;

    private void Awake()
    {
        originalY = transform.localPosition.y;
    }

    private void Update()
    {
        timer += Time.deltaTime * speed;
        timer = timer % 2;
        transform.localPosition = new Vector3(transform.localPosition.x, originalY + (animCurve.Evaluate(timer) * bounceSize), transform.localPosition.z);
        transform.RotateAround(transform.position, Vector3.up, spinSpeed * Time.deltaTime);
    }
}

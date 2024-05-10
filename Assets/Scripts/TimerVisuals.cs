using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class TimerVisuals : MonoBehaviour
{
    Vector3 initPosition;
    Quaternion initRotation;

    public float Angle = -360f;
    public float smoothDelay;

    [SerializeField]
    private float shakeIntensity = 0.2f;
    [SerializeField]
    private float smallShakeDuration = 0.2f;

    private bool klinging;

    private void Awake()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    public void Rotate(float time)
    {
        Tween.LocalRotation(transform, new Vector3(0f, 0f, Angle), time,0f);
    }

    public void Shake()
    {
    }

    public void MakeStill()
    {
        
    }


}

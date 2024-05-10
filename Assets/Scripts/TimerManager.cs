using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class TimerManager : MonoBehaviour
{
    public float timerTime = 1800f;
    [SerializeField]
    float windingUpTime = 0.2f;
    [SerializeField]
    float windUpAngles = 10f;
    [SerializeField]
    float skippingTime = 1f;


    [SerializeField]
    Transform visuals;
    [SerializeField]
    AudioSource audioSource;
    [Space(10)]
    [SerializeField]
    AudioClip alarmSound, windUpSound, skipSound;

    [SerializeField]
    private float shakeIntensity = 0.2f;

    public TimerState currentTimerState = TimerState.Idle;

    Vector3 initPosition;
    Quaternion initRotation;

    private void Awake()
    {
        initPosition = visuals.position;
        initRotation = visuals.rotation;
    }

    private void Start()
    {
        TimerInput.Instance.onInteraction += onInteraction;
    }

    TweenBase skipping;
    TweenBase shaking;
    TweenBase windingUp;
    TweenBase clocking;
    private void onInteraction()
    {
        //transition into other states
        switch (currentTimerState)
        {
            case TimerState.Idle:
                {
                    setState(TimerState.Set);
                    break;
                }
            case TimerState.Set:
                {
                    if(skipping == null)
                    {
                        if (windingUp != null)
                            windingUp.Stop();
                        if (clocking != null)
                            clocking.Stop();
                        skipping = Tween.LocalRotation(visuals, initRotation, skippingTime, 0f, completeCallback: onSkipCallBack);
                    }
                    break;
                }
            case TimerState.Ringing:
                {
                    setState(TimerState.Idle);
                    break;
                }
            default:
                throw new UnityException("false Timer state");
        }

    }

    private void onSkipCallBack()
    {
        skipping = null;
        setState(TimerState.Ringing);
    }

    private void shakeRecursive()
    {
        shaking = Tween.Shake(visuals, initPosition, Vector2.one * shakeIntensity, 0.2f, 0f, completeCallback: onShakeCallBack);
    }

    private void onShakeCallBack()
    {
        shakeRecursive();
    }

    private void onTimerCallBack()
    {
        setState(TimerState.Ringing);
    }

    private void stopAlarm()
    {
        audioSource.Stop();
        if(shaking != null)
            shaking.Stop();
    }

    private void setState(TimerState state)
    {
        //transition into other states
        switch (state)
        {
            case TimerState.Idle:
                {
                    stopAlarm();
                    break;
                }
            case TimerState.Set:
                {
                    if(windUpSound != null)
                        audioSource.PlayOneShot(windUpSound);
                    windingUp = Tween.Rotate(visuals, new Vector3(0f, 0f, -windUpAngles), Space.Self, windingUpTime, 0f, Tween.EaseInOutBack);
                    clocking = Tween.Rotate(visuals, new Vector3(0f, 0f, -360f + windUpAngles), Space.Self, timerTime - windingUpTime, windingUpTime, completeCallback: onTimerCallBack);
                    break;
                }
            case TimerState.Ringing:
                {
                    audioSource.clip = alarmSound;
                    audioSource.loop = true;
                    audioSource.Play();
                    shakeRecursive();
                    break;
                }
            default:
                throw new UnityException("false Timer state");
        }

        currentTimerState = state;
    }

    [System.Serializable]
    public enum TimerState { Idle, Set, Ringing}
}

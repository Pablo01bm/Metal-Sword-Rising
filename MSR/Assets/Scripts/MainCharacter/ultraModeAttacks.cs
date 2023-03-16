using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ultraModeAttacks : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f) { 
                //when the time is over
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();

        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        //cooldown time
        //if (Time.time > nextFireTime)
        //{
        // Check for mouse input
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OnClick();

        //}
        //}
    }

    void OnClick()
    {
        //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            print("Buenas");
            anim.SetBool("ultraModeAttack1", true);
            ShakeCamera(4f, .01f);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 )
        {
            anim.SetBool("ultraModeAttack1", false);
            anim.SetBool("ultraModeAttack2", true);
            ShakeCamera(4f, .01f);

        }
        if (noOfClicks >= 3)
        {
            anim.SetBool("ultraModeAttack2", false);
            anim.SetBool("ultraModeAttack1", true);
            ShakeCamera(4f, .01f);
            noOfClicks = 0;

        }
    }

    public void ShakeCamera(float intensity, float time) { 

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

}
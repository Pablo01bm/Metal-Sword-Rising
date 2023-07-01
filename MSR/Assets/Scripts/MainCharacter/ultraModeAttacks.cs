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

    public GameObject sparksParticles;
    ParticleSystem[] particles;

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private bool timerStarted = false;

    private void Start()
    {
        particles = sparksParticles.GetComponentsInChildren<ParticleSystem>(true);
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                //when the time is over
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Melee1"))
        {
            sparksParticles.SetActive(true);
            particles[0].Play();
            particles[1].Play();
            OnClick();
            timerStarted = true;
        }

        if (timerStarted && Time.time - lastClickedTime > 2f)
        {
            // Call your custom method here
            ResetBools();

            timerStarted = false;
        }

        if (Time.time - lastClickedTime > 0.5)
        {
            sparksParticles.SetActive(false);
            //particles[0].Pause();
            //particles[1].Pause();
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

            anim.SetBool("ultraModeAttack2", false);
            anim.SetBool("ultraModeAttack1", true);
            ShakeCamera(2f, .01f);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2)
        {
            anim.SetBool("ultraModeAttack1", false);
            anim.SetBool("ultraModeAttack2", true);
            ShakeCamera(2f, .01f);
            noOfClicks = 0;

        }
        //if (noOfClicks >= 3)
        //{
        //    anim.SetBool("ultraModeAttack2", false);
        //    anim.SetBool("ultraModeAttack1", true);
        //    ShakeCamera(4f, .01f);
        //    noOfClicks = 0;

        //}
    }

    void ResetBools()
    {
        anim.SetBool("ultraModeAttack1", false);
        anim.SetBool("ultraModeAttack2", false);
    }

    public void ShakeCamera(float intensity, float time)
    {

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

}
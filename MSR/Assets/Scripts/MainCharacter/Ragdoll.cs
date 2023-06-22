using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
   // [SerializeField] private Animator animator;

    private Rigidbody[] rigidbodies;
    private Animator[] animators;
    private Collider[] colliders;
    public Collider PlayerCollider1;
    public Collider PlayerCollider2;

    bool playerIsDead = false;

    void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        animators = transform.GetComponentsInChildren<Animator>();

        colliders = transform.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider != PlayerCollider1 && collider != PlayerCollider2)
            {
                collider.enabled = false;
            }
        }
        // SetEnabled(false);
    }

    void SetEnabled(bool enabled)
    {

        bool isKinematic = !enabled;
        foreach (Animator animatorAux in animators)
        {
            animatorAux.enabled = isKinematic;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            //rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = isKinematic;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        foreach (Collider collider in colliders)
        {
            collider.enabled = !isKinematic;
        }

        

        // animator.enabled = !enabled;
    }

    void Update()
    {

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            // Clamp the velocity magnitude to the maximum speed
            if (rigidbody.velocity.magnitude > 10f)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * 10f;
            }
        }

        if (playerIsDead)
        {
            SetEnabled(true);
        }
        
    }

    public void playerDead() 
    {
        playerIsDead = true;
    }
}
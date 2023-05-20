using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeleeEnemy : MonoBehaviour
{
    public int routine;
    public float chronometer;
    public Animator ani;
    public Quaternion angle;
    public float grade;


    public GameObject target;
    public bool atacando;

    public float health;
    public float damage;

    public HealthBar healthBar;

    private bool oneTime;


    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        healthBar.setMaxHealth(health);


    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        { 
            enemyBehaviour();
        }


        if (health <= 0 && !oneTime)
        {
            ani.SetBool("Dead", true);
            oneTime = true;
            Destroy(gameObject, 3f);
        }
    }

    public void enemyBehaviour()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, target.transform.position) > 7)
        {
            ani.SetBool("run", false);
            chronometer += 1 * Time.deltaTime;
            if (chronometer >= 4)
            {
                routine = Random.Range(0, 2);
                chronometer = 0;
            }
            switch (routine)
            {
                case 0:
                    ani.SetBool("walk", false);
                    break;

                case 1:
                    grade = Random.Range(0, 360);
                    angle = Quaternion.Euler(0f, grade, 0f);
                    routine++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    ani.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)
            {

                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                ani.SetBool("walk", false);

                ani.SetBool("run", true);
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);

                ani.SetBool("attack", false);
            }
            else
            {
                ani.SetBool("walk", false);
                ani.SetBool("run", false);

                ani.SetBool("attack", true);
                atacando = true;
            }
        }
    }

    public void finalAni()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Sword"))
        {
            health = health - 30f;
            healthBar.setHealth(health);
        }

    }

}

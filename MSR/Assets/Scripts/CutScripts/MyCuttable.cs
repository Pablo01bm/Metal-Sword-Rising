using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCuttable : MonoBehaviour
{
    public Transform enemyPos;
    public bool isAttackingM1;
    GameObject enemy;


    List<Vector3> collisionPoints;

    private Collider attackedObjectCollider;
    private Collider swordCollider;

    // Start is called before the first frame update
    void Start()
    {
        swordCollider = GetComponent<Collider>();
       
       
    }

    // Update is called once per frame
    void Update()
    {
        isAttackingM1 = GameObject.FindWithTag("Player").GetComponent<Animator>().GetBool("IsAttackingM1");
        ////Only if the character is attacking
        //if (isAttackingM1) {

        //  //  attackedObjectCollider = GetComponent<Collider>();

        //    swordCollider.enabled = true;


        //}
       // print("La posicion de la espada es: "+ swordPos.position);


    }

    private void LeaveTrail(Vector3 point, float scale)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * scale;
        sphere.transform.position = point;
        sphere.transform.parent = transform.parent;
        sphere.GetComponent<Collider>().enabled = false;
        //sphere.GetComponent<Renderer>().material = material;
        Destroy(sphere, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sword") {
            enemy = collision.collider.gameObject;
            enemyPos = enemy.GetComponent<Transform>();
            print("COLISION");
            // Make an empty list to hold contact points
            ContactPoint[] contacts = new ContactPoint[10];

            // Get the contact points for this collision
            int numContacts = collision.GetContacts(contacts);

            print("Numero de contactos: " + numContacts);
            print("Posicion Enemigo: " + enemyPos.position);

            // Iterate through each contact point
            for (int i = 0; i < numContacts; i++)
            {
                // Test the distance from the contact point to the right hand
                if (Vector3.Distance(contacts[i].point, collision.collider.ClosestPoint(contacts[i].point)) < .2f)
                {
                    LeaveTrail(contacts[i].point, 0.1f);
                }
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Sword")
        {
            enemy = collision.collider.gameObject;
            enemyPos = enemy.GetComponent<Transform>();
            print("COLISION Salgo");
            // Make an empty list to hold contact points
            ContactPoint[] contacts = new ContactPoint[10];

            // Get the contact points for this collision
            int numContacts = collision.GetContacts(contacts);

            print("Numero de contactos: " + numContacts);
            print("Posicion Enemigo: " + enemyPos.position);

            // Iterate through each contact point
            for (int i = 0; i < numContacts; i++)
            {
                // Test the distance from the contact point to the right hand
                if (Vector3.Distance(contacts[i].point, collision.collider.ClosestPoint(contacts[i].point)) < .2f)
                {
                    LeaveTrail(contacts[i].point, 0.1f);
                }
            }
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        print("LO TOCO");



    //        Vector3 aux = other.ClosestPoint(swordPos.position);

    //        print("El punto tocado es: " + aux.ToString());

    //        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //        sphere.transform.position = aux;
    //        sphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
    //    }

    //}



    //Function called all the time a collision is happening
    //private void OnCollisionStay(Collision collision) 
    //{
    //    ContactPoint[] contactPoints = new ContactPoint[20];

    //    int numContacts = collision.GetContacts(contactPoints);

    //    for (int i = 0; i < contactPoints.Length; i++) { 

    //    }

    //}


}

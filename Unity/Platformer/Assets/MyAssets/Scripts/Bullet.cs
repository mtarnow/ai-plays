using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public RobotAgent shooter;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(shooter);
        Debug.Log(shooter.transform.position.x.ToString());
        rigidBody.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
        Destroy(gameObject);
        RobotAgent enemy = hitInfo.GetComponent<RobotAgent>();
        if (enemy != null)
        {
            enemy.AddReward(-1.0f);
            shooter.AddReward(1.0f);

            //Debug.Log(shooter.transform.position.x.ToString());
            //Debug.Log(enemy.transform.position.x.ToString());
            shooter.Done();
            enemy.Done();
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
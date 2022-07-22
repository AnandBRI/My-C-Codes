using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank2D : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletspeed = 10;
    public float movespeed = 2;

    private void Update()
    {
        Movement();
        Shooting();
    }
    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0)
        {
            transform.position += new Vector3(h * movespeed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, -90 * h);
        }
        else if (v != 0)
        {
            transform.position += new Vector3(0, v * movespeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.Euler(0, 0, 90 - 90 * v);
        }
    }

    void Shooting()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletspeed;
        }
    }
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    void Awake()
    {
        Destroy(gameObject, life);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

}
*/

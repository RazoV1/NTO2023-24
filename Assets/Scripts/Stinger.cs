using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    public float speed;
    public float lifetime;
    private Rigidbody rb;
    public float damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        StartCoroutine(ded());
    }

    IEnumerator ded()
    {
        yield return new WaitForSeconds(lifetime);
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Shot!!");
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
        }
        GameObject.Destroy(gameObject);
    }
}

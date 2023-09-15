using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float Speed = 500f;
    Vector3 diretion = Vector3.zero;
    BulletPool pool;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pool = FindObjectOfType<BulletPool>();
    }
    private void Start()
    {
        StartCoroutine(IEReturn());
    }
    private void Update()
    {
        rb.velocity = diretion * Speed;
    }
    public void SetDiretion(Vector3 direction)
    {
        this.diretion = direction;
    }
    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(IEReturn());
        pool.ReturnBullet(this.gameObject);
    }
    IEnumerator IEReturn()
    {
        yield return new WaitForSeconds(3f);
        pool.ReturnBullet(this.gameObject);
    }
}

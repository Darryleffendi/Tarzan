using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public bool dropped, collided;
    [SerializeField]
    private ParticleSystem splash;

    private void Update()
    {
        transform.Rotate(0, 360 * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!dropped && collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Audio3D audio = GetComponent<Audio3D>();
            audio.SoundEffect("RockDrop" + Random.Range(1, 3).ToString());
            dropped = true;
        }

        if (collided) return;

        ITakesDamage enemy = collision.gameObject.GetComponent<ITakesDamage>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            collided = true;
        }

        Animal animal = collision.gameObject.GetComponent<Animal>();
        if (animal != null)
        {
            animal.GetState().TakeDamage(damage);
            collided = true;
        }

        StartCoroutine(DestroyObj());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Audio3D audio = GetComponent<Audio3D>();
            splash.Play();
            audio.SoundEffect("Splash");
            dropped = true;
        }
    }

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}

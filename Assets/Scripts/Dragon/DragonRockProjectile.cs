using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRockProjectile : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem dust;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if(player != null) player.deductHealth(10);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            dust.Play();
            Audio3D audio = GetComponent<Audio3D>();
            if (audio != null) audio.Boulder();
            Invoke(nameof(DestroyObject), 3f);
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}

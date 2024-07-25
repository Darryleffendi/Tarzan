using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    Animator animator;
    new Audio3D audio;

    [SerializeField]
    private Transform weaponTail, weaponLeft, weaponRight, fireBreath, fireTrigger, projectileParent;

    [SerializeField]
    private GameObject debris;

    void Start()
    {
        audio = GetComponent<Audio3D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    public void FlyUp()
    {
        audio.Dragon("DragonRoar");
        audio.Dragon("DragonFly");
        StartCoroutine(DragonRoarDebris());
    }

    public void FireBreathStart()
    {
        fireBreath.GetComponent<ParticleSystem>().Play();
        CameraController.Instance.CameraShake(1.5f, 4);
        audio.Dragon("DragonFire" + Random.Range(1, 3));
        fireTrigger.gameObject.SetActive(true);
    }

    public void FireBreathStop()
    {
        fireBreath.GetComponent<ParticleSystem>().Stop();
        fireTrigger.gameObject.SetActive(false);
    }

    public void DragonRoar()
    {
        CameraController.Instance.CameraShake(1.5f);
        CameraController.Instance.ChangeFov(100);
        StartCoroutine(RestoreFOV());
    }

    private IEnumerator DragonRoarDebris()
    {
        for(int i = 0; i < 4; i ++)
        {
            SpawnDebris();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RestoreFOV()
    {
        yield return new WaitForSeconds(1.3f);
        CameraController.Instance.RestoreFov();
    }

    public void StopAttack()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
    }

    private void SpawnDebris()
    {
        float xRand = Random.Range(-20.0f, 20.0f);
        float zRand = Random.Range(-20.0f, 20.0f);

        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 pos = new Vector3(playerPos.x + xRand, 70, playerPos.z + zRand);

        Instantiate(debris, pos, Random.rotation, projectileParent);
    }

    public void ActivateLeft()
    {
        weaponLeft.gameObject.SetActive(true);
    }
    public void ActivateRight()
    {
        weaponRight.gameObject.SetActive(true);
    }
    public void ActivateTail()
    {
        weaponTail.gameObject.SetActive(true);
    }
    public void DeactivateLeft()
    {
        weaponLeft.gameObject.SetActive(false);
    }
    public void DeactivateRight()
    {
        weaponTail.gameObject.SetActive(false);
    }
    public void DeactivateTail()
    {
        weaponTail.gameObject.SetActive(false);
    }
}

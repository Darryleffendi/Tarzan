using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private int comboIndex = 0;
    private float timeDelay = 0f;

    [SerializeField]
    protected Transform projectileSpawnpoint, projectileParent, LWeapon, RWeapon;
    [SerializeField]
    protected GameObject rockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            BasicAttack();
        }
        if (Input.GetKey(KeyCode.X))
        {
            ThrowRock();
        }
    }

    public void BasicAttack()
    {
        if (Time.time - timeDelay < 0)
            return;

        timeDelay = Time.time + 0.4f;
        if(comboIndex == 0)
        {
            animator.SetBool("basic1", true);
            comboIndex = 1;
            StartCoroutine(BackToIdle());
        }
        else
        {
            animator.SetBool("basic2", true);
            comboIndex = 0;
            StartCoroutine(BackToIdle());
        }
    }

    private IEnumerator BackToIdle()
    {
        yield return new WaitForSeconds(.5f);
        comboIndex = 0;
        animator.SetBool("basic2", false);
        animator.SetBool("basic1", false);
        DeactivateWeapon(0);
        DeactivateWeapon(1);
    }

    public void ThrowRock()
    {
        if(animator.GetBool("isThrowing"))
        {
            return;
        }

        animator.SetBool("isThrowing", true);
        StartCoroutine(ThrowCoroutine());
    }

    private IEnumerator ThrowCoroutine()
    {
        yield return new WaitForSeconds(.3f);

        Vector3 direction = Camera.main.transform.forward;
        var rock = Instantiate(rockPrefab, projectileSpawnpoint.position, transform.rotation, projectileParent);
        rock.GetComponent<PlayerProjectile>().damage = 20;
        rock.GetComponent<Rigidbody>().AddForce(direction * 10000);

        yield return new WaitForSeconds(.4f);
        animator.SetBool("isThrowing", false);
    }

    public void ActivateWeapon(int idx)
    {
        if (idx == 0)
            LWeapon.gameObject.SetActive(true);
        else if (idx == 1)
            RWeapon.gameObject.SetActive(true);
    }

    public void DeactivateWeapon(int idx)
    {
        if (idx == 0)
            LWeapon.gameObject.SetActive(false);
        else if (idx == 1)
            RWeapon.gameObject.SetActive(false);
    }
}

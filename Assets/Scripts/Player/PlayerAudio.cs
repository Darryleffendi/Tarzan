using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    Audio3D footAudio, chestAudio;
    LayerMask layer;
    LayerMask triggerLayer = 0;
    LayerMask whatIsGround;

    private void Start()
    {
        chestAudio.ChangeFootstep(LayerMask.NameToLayer("DeepWater"));
        layer = LayerMask.NameToLayer("Ground");
        whatIsGround = layer;
    }

    public void ChangeFootstep(LayerMask layer)
    {
        this.layer = layer;

        if (layer == LayerMask.NameToLayer("Ground")) layer = whatIsGround;

        if (layer == LayerMask.NameToLayer("DeepWater"))
            chestAudio.ChangeFootstep(layer);
        else if (IsValidLayer(triggerLayer))
            footAudio.ChangeFootstep(triggerLayer);
        else
            footAudio.ChangeFootstep(layer);
    }

    private bool IsValidLayer(LayerMask layer)
    {
        if (layer == LayerMask.NameToLayer("Default"))
            return false;
        if (layer == LayerMask.NameToLayer("Dragon"))
            return false;
        if (layer == LayerMask.NameToLayer("Enemy"))
            return false;
        if (layer == LayerMask.NameToLayer("Player"))
            return false;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerLayer = other.gameObject.layer;
        AudioManager audio = AudioManager.Instance;

        if (other.gameObject.name == "CaveZone" && audio.GetBGM() != "DragonBGM")
        {
            audio.SwitchBGM("DragonBGM");
            audio.StopBackground("ForestAmbience");
            Dragon.Instance.SetPlayer(Player.Instance);
        }

        if (other.gameObject.CompareTag("DrylandBoundary"))
            whatIsGround = LayerMask.NameToLayer("Default");
       else  if (other.gameObject.CompareTag("ForestBoundary"))
            whatIsGround = LayerMask.NameToLayer("Ground");
    }

    private void OnTriggerExit(Collider other)
    {
        triggerLayer = 0;

        if (other.gameObject.name == "CaveZone")
        {
            AudioManager.Instance.SwitchBGM("MainBGM");
            AudioManager.Instance.Background("ForestAmbience");
            Dragon.Instance.SetPlayer(null);
        }
    }

    public void Walk()
    {
        if (layer == LayerMask.NameToLayer("DeepWater"))
            chestAudio.Walk();
        else
            footAudio.Walk();
    }

    public void Attack()
    {
        chestAudio.Attack("Punch" + Random.Range(1, 6).ToString());
    }

    public void Grapple()
    {
        chestAudio.Attack("Rope");
    }

    public void Fly()
    {
        chestAudio.Attack("Wind");
    }

    public void Jump()
    {
        footAudio.Attack("Jump" + Random.Range(1, 3));
    }

    public void Land()
    {
        footAudio.Attack("Land");
    }

    public void Throw()
    {
        chestAudio.Attack("Throw");
    }
}

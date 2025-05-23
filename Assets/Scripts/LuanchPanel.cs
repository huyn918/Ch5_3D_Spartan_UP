using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuanchPanel : MonoBehaviour, IInteractable
{
    public ItemData data;
    private bool isInCollider = false;
    [SerializeField]
    public float luanchPower;
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        if (isInCollider) LuanchPlayer();
    }
    private void LuanchPlayer()
    {
        Debug.Log("플레이어 발사");

        CharacterManager.Instance.Player.controller.player_Rigidbody.AddForce(transform.forward * luanchPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isInCollider = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isInCollider = false;
    }

}

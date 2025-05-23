using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    [SerializeField] public float jumpPanelPower;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            CharacterManager.Instance.Player.controller.player_Rigidbody
                .AddForce(Vector2.up * jumpPanelPower, ForceMode.Impulse);
        }
    }

}

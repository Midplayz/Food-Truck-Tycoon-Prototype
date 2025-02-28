using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnPlayer(other.gameObject);
            SubtitleManager.Instance.ShowSubtitle("Oops! Try Again?");
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        if (respawnPoint != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            CharacterController cc = player.GetComponent<CharacterController>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; 
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            if (cc != null)
            {
                cc.enabled = false;
            }

            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;

            if (rb != null)
            {
                rb.isKinematic = false;
            }

            if (cc != null)
            {
                cc.enabled = true;
            }
        }
        else
        {
            Debug.LogError("Respawn point not assigned!");
        }
    }
}

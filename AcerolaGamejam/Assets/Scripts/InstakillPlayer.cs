using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillPlayer : MonoBehaviour
{
    public GameObject gameManager;
    private GameManager gm;
    private void Start()
    {
        gm = gameManager.GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gm.playerDied = true;
        }
    }
}

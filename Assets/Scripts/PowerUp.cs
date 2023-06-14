using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;
    void OnTriggerEnter (Collider other)
    {
        Debug.Log("Algo ha tocado");

        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider other){
        Debug.Log("Power up picked up!");

        // Spawn cool effect
        Instantiate(pickupEffect, transform.position,transform.rotation);

        // 
        var tablero = GameObject.Find("TerrainGroup_0");
        if (tablero){
            var script = tablero.GetComponent<Tablero>();
            
            var name = gameObject.name;

            if (name == "AmmoBox"){
                script.refillAmmo(other.name);
            }

            if (name == "Power_Up"){
                script.refillCharge(other.name);
            }

            if (name == "AmmoBox"){
                script.refillHealth(other.name);
            }
            Debug.Log("Name of my parent: "+name);

        }



        // Remove powerup object
        Destroy(gameObject);
    }
}

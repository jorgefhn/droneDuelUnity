using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;


[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour
{


    public Transform bulletSpawnPoint;
    public GameObject target; 

    public float gunRange = 300;
    public float shootzone;
    public float laserDuration = 0.2f;
    public string hitName = "";
    public string winner = "";
    public Text winText;
    public bool gameEnded = false;

    LineRenderer  laserLine;
    float fireTimer;
    public float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void EndGame(string message)
    {
        Time.timeScale = 0f;
        float elapsedTime = Time.time-startTime;;
        UnityEngine.Debug.Log("Juego terminado. Tiempo transcurrido: "+ elapsedTime.ToString("F2")+ " segundos");
        winText.text = message+ " wins!";
        winText.gameObject.SetActive(true);
    }

    void Update(){
        fireTimer += Time.deltaTime;

        
            fireTimer = 0;
            if (transform && target){
                shootzone = Vector3.Distance(transform.position,target.transform.position);
                StartCoroutine(ShootLaser());
                if (hitName != ""){
                UnityEngine.Debug.Log(transform.gameObject.name + " hit: "+hitName);
                }

                if (gameEnded){
                    EndGame(winner);
                }
            }
            

            
            
           


        
       

        IEnumerator ShootLaser()
        {
            laserLine.SetPosition(0,bulletSpawnPoint.position);
            RaycastHit hit;
            
            var tablero = GameObject.Find("TerrainGroup_0");
            if (tablero)
                
            {
                var script = tablero.GetComponent<Tablero>();

                if (shootzone <= 250){


                    if (Physics.Raycast(bulletSpawnPoint.transform.position,target.transform.position,out hit,gunRange))
                    {
                        laserLine.SetPosition(1,hit.point);
                        
                        
                            

                            if (hit.transform.gameObject.name == "drone1" || hit.transform.gameObject.name == "drone2")
                            {
                                hitName = hit.transform.gameObject.name;
                                    if (hitName == "drone1")
                                    {
                                    script.gameInfo.drone1.health -= 2;
                                    if (script.gameInfo.drone1.health <= 0){
                                        Destroy(hit.transform.gameObject);
                                        gameEnded = true;
                                        winner = "drone2";   
                                    }
                                    }
                                    if (hitName == "drone2")
                                    {
                                    script.gameInfo.drone2.health -= 2;
                                    if (script.gameInfo.drone2.health <= 0){
                                        Destroy(hit.transform.gameObject);
                                        gameEnded = true;
                                        winner = "drone1";

                                    }
                                    }
                                }
                                
                        }
                    }

                    else
                    {
                        laserLine.SetPosition(1, target.transform.position);
                    }

                    laserLine.enabled = true;
                    var self = transform.gameObject.name;

                    if (self == "drone1"){
                        if (script.gameInfo.drone1.ammo > 0){
                            script.gameInfo.drone1.ammo -= 0.01f;
                        }
                    }

                    if (self == "drone2"){
                        if (script.gameInfo.drone2.ammo > 0){
                            script.gameInfo.drone2.ammo -= 0.01f;
                        }
                    } 
                    yield return new WaitForSeconds(laserDuration);
                    laserLine.enabled = false;

                }

        }
    }
}

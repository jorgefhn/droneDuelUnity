using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;


[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour
{


    public Transform bulletSpawnPoint;
    private GameObject target; 

    private float gunRange = 300;
    public float shootzone;
    private float laserDuration = 0.4f;
    public string hitName = "";
    public string winner = "";
    public Text winText;
    private bool gameEnded = false;
    private bool d1destroyed = false;
    private bool d2destroyed = false;
    private bool d3destroyed = false;
    private bool d4destroyed = false;


    LineRenderer  laserLine;
    float fireTimer = 0;
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
    }

    

    void Update(){

        fireTimer += Time.deltaTime;

        if (d1destroyed && d2destroyed ){
                gameEnded = true;
                winner = "drone3 & drone4";   
        }

        if (d3destroyed && d4destroyed ){
                gameEnded = true;
                winner = "drone1 & drone2";   
        }

        if (gameEnded){
            EndGame(winner);
        }

        
        StartCoroutine(ShootLaser());
    
        
        
            
        IEnumerator ShootLaser()
        {
            laserLine.SetPosition(0,bulletSpawnPoint.position);
            RaycastHit hit;
            
            var drone1 = GameObject.Find("drone1");
            var drone2 = GameObject.Find("drone2");
            var drone3 = GameObject.Find("drone3");
            var drone4 = GameObject.Find("drone4");
            
            var tablero = GameObject.Find("TerrainGroup_0");
            
            if (tablero)
                
            {

                var script = tablero.GetComponent<Tablero>();

                // decideTarget();

                double h1 = Double.Parse(script.gameInfo.drone1.health.ToString());
                double h2 = Double.Parse(script.gameInfo.drone2.health.ToString());
                double h3 = Double.Parse(script.gameInfo.drone3.health.ToString());
                double h4 = Double.Parse(script.gameInfo.drone4.health.ToString());

                d1destroyed = (h1 == 0.0);
                d2destroyed = (h2 == 0.0);
                d3destroyed = (h3 == 0.0);
                d4destroyed = (h4 == 0.0);

                var name = transform.name;

                if (name == "drone1"){
                    if (h3 > 0.0){
                        target = drone3;
                    }
                    if (h3 == 0.0 && h4 > 0.0 ){
                        target = drone4;
                    }

                }

                if (name == "drone2"){
                    if (h4 > 0.0){
                        target = drone4;
                    }

                    if (h4 == 0.0 && h3 > 0.0 ){
                        target = drone3;
                    }
                }

                if (name == "drone3"){
                    if (h1 > 0.0){
                        target = drone1;
                    }


                    if (h1 == 0.0 && h2 > 0.0 ){
                        target = drone2;
                    }
                }

                if (name == "drone4"){
                    if (h2 > 0.0){
                        target = drone2;
                    }
                    if (h2 == 0.0 && h1 > 0.0 ){
                        target = drone1;                        
                    }

                }


        
                if (target){
                    shootzone = Vector3.Distance(transform.position,target.transform.position);
                    if (shootzone <= 250){

                        UnityEngine.Debug.Log("Self: "+transform.name+ " Target: "+target);


                        if (Physics.Raycast(bulletSpawnPoint.transform.position,target.transform.position,out hit,gunRange))
                        {
                            laserLine.SetPosition(1,hit.point);


                            hitName = hit.transform.gameObject.name;

                            if (hitName != ""){
                            UnityEngine.Debug.Log(transform.gameObject.name + " hit: "+hitName);
                            }

                            // if (hitName == "drone1" && (transform.name == "drone3" || transform.name == "drone4")){

                            if (hitName == "drone1"){
                                script.gameInfo.drone1.health -= 2;
                                if (script.gameInfo.drone1.health <= 0.0){
                                    Destroy(hit.transform.gameObject);
                                    d1destroyed = true;
                            }


                            }

                            // if (hitName == "drone2" && (transform.name == "drone3" || transform.name == "drone4")){
                            if (hitName == "drone2"){
                                script.gameInfo.drone2.health -= 2;
                                if (script.gameInfo.drone2.health <= 0.0){
                                    Destroy(hit.transform.gameObject);
                                    d2destroyed = true;
                                    
                            }

                            }

                            if (hitName == "drone3"){
                            // if (hitName == "drone3" && (transform.name == "drone1" || transform.name == "drone2")){
                                    script.gameInfo.drone3.health -= 2;
                                    if (script.gameInfo.drone3.health <= 0.0){
                                        Destroy(hit.transform.gameObject);
                                        d3destroyed = true;
                                }

                            }

                            if (hitName == "drone4"){
                            // if (hitName == "drone4" && (transform.name == "drone1" || transform.name == "drone2")){
                                script.gameInfo.drone4.health -= 2;
                                if (script.gameInfo.drone4.health <= 0.0){
                                    Destroy(hit.transform.gameObject);
                                    d4destroyed = true;
                                }

                            }
                            
                        } else
                        {
                            laserLine.SetPosition(1, target.transform.position);
                        }

                                    
                            
                                    
                        }
                    


                        laserLine.enabled = true;
                        var self = transform.gameObject.name;

                        if (self == "drone1"){
                            if (script.gameInfo.drone1.ammo > 0.0){
                                script.gameInfo.drone1.ammo -= 0.01f;
                            }
                        }

                        if (self == "drone2"){
                            if (script.gameInfo.drone2.ammo > 0.0){
                                script.gameInfo.drone2.ammo -= 0.01f;
                            }
                        } 

                        if (self == "drone3"){
                            if (script.gameInfo.drone3.ammo > 0.0){
                                script.gameInfo.drone3.ammo -= 0.01f;
                            }
                        }

                        if (self == "drone4"){
                            if (script.gameInfo.drone4.ammo > 0.0){
                                script.gameInfo.drone4.ammo -= 0.01f;
                            }
                        }
                        yield return new WaitForSeconds(laserDuration);
                        laserLine.enabled = false;

                }   

                    
                    }
                }


                }



       

}

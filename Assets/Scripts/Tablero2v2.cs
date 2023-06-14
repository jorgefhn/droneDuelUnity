using UnityEngine;
using System.Collections;
using System.Threading;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using static UnityMainThreadDispatcher;

public class Tablero : MonoBehaviour 
{

    public float batteryDrainRate; // Tasa de drenaje de la batería por unidad de tiempo
    public bool receivedInfo = false;
    public bool sent = false;
    public String lastPosd1,lastPosd2,lastPosd3,lastPosd4;



    public bool isRunning = true;

    public String newPositions;
    public List<String> healthPackages,chargePackages,ammoPackages;


    
            

    // class to represent drone info

    public class droneInfo
    {
        public float health;
        public float charge;
        public float ammo;
        public String position;

    }
    public class Info
    {
        public droneInfo drone1;
        public droneInfo drone2; 
        public droneInfo drone3;
        public droneInfo drone4;

        public List<String> healthPackages;
        public List<String> chargePackages;
        public List<String> ammoPackages;
       

        public Info() {
            drone1 = new droneInfo();
            drone2 = new droneInfo();
            drone3 = new droneInfo();
            drone4 = new droneInfo();
            healthPackages = new List<String>();
            chargePackages = new List<String>();
            ammoPackages = new List<String>();
            
        }
    }
    public Info gameInfo = new Info();   

    Thread senderThread;
    Thread listenerThread;


    void Start () 
    {

        SpawnRandomObjects();

        // initial drone1 systems at 100
        gameInfo.drone1.health = 100f;
        gameInfo.drone1.charge = 100f;
        gameInfo.drone1.ammo = 100f;

        // initial drone2 systems at 100
        gameInfo.drone2.health = 100f;
        gameInfo.drone2.charge = 100f;
        gameInfo.drone2.ammo = 100f;

        // initial drone3 systems at 100
        gameInfo.drone3.health = 100f;
        gameInfo.drone3.charge = 100f;
        gameInfo.drone3.ammo = 100f;

        // initial drone4 systems at 100
        gameInfo.drone4.health = 100f;
        gameInfo.drone4.charge = 100f;
        gameInfo.drone4.ammo = 100f;



        
        
        senderThread = new Thread( Sender ) { IsBackground = true };
        senderThread.Start();

      

        listenerThread = new Thread( Listener ) { IsBackground = true };
        listenerThread.Start();


    }

    
    Vector3 StringToVector3(string str)
    {
        str = str.Replace("[", "").Replace("]", "");
        string[] components = str.Split(',');
        CultureInfo culture = new CultureInfo("en-US"); // Especifica el uso del punto como separador decimal
        return new Vector3(float.Parse(components[0],culture), float.Parse(components[1],culture), float.Parse(components[2],culture));
    }

    public void refillAmmo(String droneName)
    {
        if (droneName == "drone1"){
            gameInfo.drone1.ammo = 100f;
        }
        if (droneName == "drone2"){
            gameInfo.drone2.ammo = 100f;
        }

        if (droneName == "drone3"){
            gameInfo.drone3.ammo = 100f;
        }

        if (droneName == "drone4"){
            gameInfo.drone4.ammo = 100f;
        }

    }

    public void refillCharge(String droneName)
    {
        if (droneName == "drone1"){
            gameInfo.drone1.charge = 100f;
        }
        if (droneName == "drone2"){
            gameInfo.drone2.charge = 100f;
        }

        if (droneName == "drone3"){
            gameInfo.drone3.charge = 100f;
        }

        if (droneName == "drone4"){
            gameInfo.drone4.charge = 100f;
        }

    }

    public void refillHealth(String droneName)
    {
        if (droneName == "drone1"){
            gameInfo.drone1.health = 100f;
        }
        if (droneName == "drone2"){
            gameInfo.drone2.health = 100f;
        }
        if (droneName == "drone3"){
            gameInfo.drone3.health = 100f;
        }
        if (droneName == "drone4"){
            gameInfo.drone4.health = 100f;
        }

    }

    void Update()
    {
        System.Random random = new System.Random(5);


       
        batteryDrainRate = 1f;


        gameInfo.healthPackages = healthPackages;
        gameInfo.chargePackages = chargePackages;
        gameInfo.ammoPackages = ammoPackages;

        
        
        

        // get drone1 position
        var drone1 = GameObject.Find("drone1");

        
        if (drone1){


            if (drone1.transform.position.ToString() != lastPosd1){



                gameInfo.drone1.charge -= batteryDrainRate * Time.deltaTime;

                if (gameInfo.drone1.charge <= 0f){
                    gameInfo.drone1.charge = 0f;
                    Debug.Log("La batería del dron 1 se ha agotado");
                }

                lastPosd1 = drone1.transform.position.ToString();


            }
            var mov1 = drone1.GetComponent<DroneMovement>();

            // Move
            if (newPositions != null & receivedInfo){
                string[] positions = newPositions.Split('?');
                Vector3 drone1Positions = StringToVector3(positions[0]);

                int binaryNumber = random.Next(0,1);
                    

                if (binaryNumber == 0){
                    drone1Positions[1] += 50f;
                }
                if (binaryNumber == 1){
                    drone1Positions[1] -= 50f;

                }


                if (gameInfo.drone1.health > 0 && gameInfo.drone1.charge > 0)
                {
                    mov1.moveTo(drone1Positions);    
                }


            }
            gameInfo.drone1.position = drone1.transform.position.ToString();       

     
        }

        // get drone2 position
        var drone2 = GameObject.Find("drone2");
        Debug.Log("Drone 2: "+drone2);


        if (drone2){



             if (drone2.transform.position.ToString() != lastPosd2){
                gameInfo.drone2.charge -= batteryDrainRate * Time.deltaTime;

                if (gameInfo.drone2.charge <= 0f){
                    gameInfo.drone2.charge = 0f;
                    Debug.Log("La batería del dron 2 se ha agotado");
                }

                lastPosd2 = drone2.transform.position.ToString();
            }

            var mov2 = drone2.GetComponent<DroneMovement>();

            // Move
            if (newPositions != null && receivedInfo)
            {
                string[] positions = newPositions.Split('?');
                Vector3 drone2Positions = StringToVector3(positions[1]);

                int binaryNumber = random.Next(0,1);
                    

                if (binaryNumber == 0){
                    drone2Positions[1] += 50f;
                }
                if (binaryNumber == 1){
                    drone2Positions[1] -= 50f;

                }
                if (gameInfo.drone2.health > 0 && gameInfo.drone2.charge > 0)
                {
                mov2.moveTo(drone2Positions);   
                }

            } 
            
            gameInfo.drone2.position = drone2.transform.position.ToString();

            
        }

        // get drone3 position
        var drone3 = GameObject.Find("drone3");

        if (drone3){



             if (drone3.transform.position.ToString() != lastPosd3){
                gameInfo.drone3.charge -= batteryDrainRate * Time.deltaTime;

                if (gameInfo.drone3.charge <= 0f){
                    gameInfo.drone3.charge = 0f;
                    Debug.Log("La batería del dron 3 se ha agotado");
                }

                lastPosd3 = drone3.transform.position.ToString();
            }

            var mov3 = drone3.GetComponent<DroneMovement>();

            // Move
            if (newPositions != null && receivedInfo)
            {
                string[] positions = newPositions.Split('?');
                Vector3 drone3Positions = StringToVector3(positions[2]);

                int binaryNumber = random.Next(0,1);
                    

                if (binaryNumber == 0){
                    drone3Positions[1] += 50f;
                }
                if (binaryNumber == 1){
                    drone3Positions[1] -= 50f;

                }
                if (gameInfo.drone3.health > 0 && gameInfo.drone3.charge > 0)
                {
                mov3.moveTo(drone3Positions);   
                }

            } 
            
            gameInfo.drone3.position = drone3.transform.position.ToString();

            
        }
        
        // get drone4 position
        var drone4 = GameObject.Find("drone4");


        if (drone4){



             if (drone4.transform.position.ToString() != lastPosd4){
                gameInfo.drone4.charge -= batteryDrainRate * Time.deltaTime;

                if (gameInfo.drone4.charge <= 0f){
                    gameInfo.drone4.charge = 0f;
                    Debug.Log("La batería del dron 4 se ha agotado");
                }

                lastPosd4 = drone4.transform.position.ToString();
            }

            var mov4 = drone4.GetComponent<DroneMovement>();

            // Move
            if (newPositions != null && receivedInfo)
            {
                string[] positions = newPositions.Split('?');
                Vector3 drone4Positions = StringToVector3(positions[3]);

                int binaryNumber = random.Next(0,1);
                    

                if (binaryNumber == 0){
                    drone4Positions[1] += 50f;
                }
                if (binaryNumber == 1){
                    drone4Positions[1] -= 50f;

                }


                if (gameInfo.drone4.health > 0 && gameInfo.drone4.charge > 0)
                {
                mov4.moveTo(drone4Positions);   
                }

            } 
            
            gameInfo.drone4.position = drone4.transform.position.ToString();

            
        }
        

        


    }

   

    void OnApplicationQuit()
    {
        isRunning = false;
    }
       

    void Sender()
    {

        Socket s = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
        IPAddress broadcast = IPAddress.Parse("127.0.0.1");



        try
        {
            

            // Then, send the drone information periodically
            while (isRunning){
            
                if (gameInfo.drone1.position != null && gameInfo.drone2.position != null && gameInfo.drone3.position != null && gameInfo.drone4.position != null){
                    string json = JsonConvert.SerializeObject(gameInfo);
                    byte[] data = Encoding.ASCII.GetBytes(json);        
                    IPEndPoint ep = new IPEndPoint(broadcast,11004);
                    s.SendTo(data,ep);
                    Thread.Sleep(1000); // sleep for 2 seconds


                }

                
            }
           
            
        } 
        catch (SocketException e){
            Debug.Log(e);
        }
        finally 
        {
            s.Close();
        } 

    }

    void Listener()
    {

        UdpClient listener = new UdpClient(11000);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11000);

        try
        {
 

    
            while (isRunning){
                byte[] bytes = listener.Receive(ref groupEP);

                // Aquí recibe la información de a dónde tienen que ir los drones
                Debug.Log($"Mensaje recibido desde Java{Encoding.ASCII.GetString(bytes,0,bytes.Length)}");


                newPositions = Encoding.ASCII.GetString(bytes,0,bytes.Length);
                receivedInfo = true;

                Thread.Sleep(1000); // sleep for 1 seconds


                
            }
        }
        catch (SocketException e){
            Debug.Log(e);
        }
        finally 
        {
            listener.Close();
        }

        

    }

    void SpawnRandomObjects()
    {
        // Spawn random objects: health, ammo, battery packages
        System.Random random = new System.Random(5);
        int randomPackages = random.Next(1,5);
    
        // Health packages 
        for (int i = 0; i < randomPackages; i++)
        {
            // Instantiate random health packages
            Vector3 randomHealthPosition = new Vector3(UnityEngine.Random.Range(-300,300), 10, UnityEngine.Random.Range(-300,300));
            var healthPackage = GameObject.Find("Heal_Up");
            if (healthPackage){
                Instantiate(healthPackage,randomHealthPosition,Quaternion.identity);
            }
            healthPackages.Add(randomHealthPosition.ToString());

            // Instantiate random charge packages
            Vector3 randomChargePosition = new Vector3(UnityEngine.Random.Range(-300,300), 10, UnityEngine.Random.Range(-300,300));
            var chargePackage = GameObject.Find("Power_Up");
            if (chargePackage){
                Instantiate(chargePackage,randomChargePosition,Quaternion.identity);
            }
            chargePackages.Add(randomChargePosition.ToString());

            // Instantiate random ammo packages
            Vector3 randomAmmoPosition = new Vector3(UnityEngine.Random.Range(-300,300), 10, UnityEngine.Random.Range(-300,300));
            var ammoPackage = GameObject.Find("AmmoBox");
            if (ammoPackage){
                Instantiate(ammoPackage,randomAmmoPosition,Quaternion.identity);
            }
            ammoPackages.Add(randomAmmoPosition.ToString());
            
        } 

       
            
            
        
    }
}


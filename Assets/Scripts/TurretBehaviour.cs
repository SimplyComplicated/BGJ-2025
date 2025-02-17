using System.Security.Cryptography;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System;

public class TurretBehaviour : MonoBehaviour
{   

    [SerializeField] float AngleSpeed;
    [SerializeField] float AngleOfView;
    [SerializeField] GameObject Player;
    [SerializeField] int TimeToEscape;
    float movementFactor;
    
    bool isLocked = false;

    float timer = 0;


    void Update()
    {  
       if (Aim() == Player) 
       {
           LockOnPlayer();
           isLocked = true;
       }
       else
       {
           SearchForTarget();
           isLocked = false;
           timer = 0;
       } 
    }

    void SearchForTarget() 
    {
       movementFactor = Mathf.PingPong(Time.time * AngleSpeed, AngleOfView);
       transform.rotation = Quaternion.Euler(0,movementFactor,0);
    }
     
    void LockOnPlayer() 
    {
       transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), AngleSpeed * Time.deltaTime);
       movementFactor = transform.rotation.y;
       timer += Time.deltaTime;
       if (timer >= TimeToEscape) Fire();
       if (timer >= 1000) timer = 0;
    } 
    void Fire() 
    {
       Debug.Log("You're dead");
    }
    GameObject Aim() 
    {
       Ray ray = new Ray(transform.position, transform.forward);
       Debug.DrawRay(transform.position, transform.forward * 200, Color.red);

       RaycastHit obj;
       if (Physics.Raycast(ray, out obj))
       {
           return obj.collider.gameObject;
       }
       else return null ;
    }

}

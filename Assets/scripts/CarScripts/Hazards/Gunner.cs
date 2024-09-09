using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class Gunner : CarHazard
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] FieldOfView eye;
    [SerializeField] Transform bulletOrigin;
    [SerializeField] float fireInterval;
    [SerializeField] float trackingInterval;
    [SerializeField] Transform barrel;
    Vector3 lastKnownTargetPos;
    float fireTimeStamp;
    float trackingTimeStamp;
    Transform currentTarget;
    
    public override bool CanInteract(GameObject interactor)
    {
        return true;
    }

    public override void Interact(GameObject interactor)
    {
        SpawnInteractEffect();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (eye.visibleTargets.Count > 0)
        {
            currentTarget = eye.visibleTargets[0];
            if (currentTarget)
            {
                if (Time.time > trackingTimeStamp)
                {
                    lastKnownTargetPos = currentTarget.transform.position;
                    trackingTimeStamp = Time.time + trackingInterval;
                    barrel.LookAt(lastKnownTargetPos);
                }
                if(Time.time > fireTimeStamp) FireBullet();
            }
            
        }
    }

    void FireBullet()
    {
        Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation,transform.parent);
        fireTimeStamp = Time.time + fireInterval;
    }
}

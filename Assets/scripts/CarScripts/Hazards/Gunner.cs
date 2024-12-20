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
    [SerializeField] float trackingSpeed;
    [SerializeField] Transform barrel;
    [SerializeField] int burstCount = 3;
    [SerializeField] float burstDelay = 0.2f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform tracker;
    [SerializeField] float trackerSpinSpeed = 5;
    Vector3 lastKnownTargetPos;
    float fireTimeStamp;
    float trackingTimeStamp;
    Transform currentTarget;
    int currentBurst = 0;
    bool isBursting;

	[SerializeField] int gainedFromKill = 20;
	private CarModeManager manager;

    private void Start()
    {
		manager = CarModeManager.singleton;
        eye.targetSearchStarted.AddListener(ResetFireInterval);
    }

    void ResetFireInterval()
    {
        fireTimeStamp = Time.time + fireInterval;
    }

    public override bool CanInteract(GameObject interactor)
    {
        return true;
    }

    public override void Interact(GameObject interactor)
    {
        SpawnInteractEffect();
		manager.turretScore += gainedFromKill;
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
                tracker.transform.position = Vector3.MoveTowards(tracker.transform.position,currentTarget.position,trackingSpeed * Time.fixedDeltaTime);
                tracker.Rotate(Vector3.up, trackingSpeed * Time.fixedDeltaTime * trackerSpinSpeed * Mathf.Min(1, (fireTimeStamp-Time.time)/fireInterval));
                barrel.LookAt(tracker.transform.position);
                lineRenderer.SetPosition(0, bulletOrigin.position);
                lineRenderer.SetPosition(1, tracker.position);
                if (Time.time > fireTimeStamp && !isBursting)
                {
                    isBursting = true;
                    StartCoroutine(FireBurst());
                }
                if(Vector3.Distance(currentTarget.position,transform.position) > eye.viewRadius)
                {
                    eye.visibleTargets.Clear();
                    currentTarget = null;
                }
            }

        }
        else
        {
            currentTarget = null;
        }
    }

    void FireBullet()
    {
        Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation, transform.parent);
    }

    IEnumerator FireBurst()
    {
        if (currentBurst < burstCount)
        {
            yield return new WaitForSeconds(burstDelay);
            FireBullet();
            ++currentBurst;
            StartCoroutine(FireBurst());
        }
        else
        {
            isBursting = false;
            currentBurst = 0;
            ResetFireInterval();
        }
        
    }
}

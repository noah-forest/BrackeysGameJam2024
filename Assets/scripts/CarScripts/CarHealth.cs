using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CarHealth : MonoBehaviour
{
    public bool IsImmune { get; private set; }
    [SerializeField] Material damagedMat;
    [SerializeField] Material damagedMat1;
    [SerializeField] Material starndardMat;
    [SerializeField] float immuneDuration = 0.5f;
    [SerializeField] float flashInterval = 0.1f;
    [SerializeField] MeshRenderer meshRenderer;
    bool damageVisualOn;

    float timeStamp;
    public UnityEvent carDamaged;
    float deltaStorage;


    public void TakeDamage()
    {
        if (IsImmune) return;
        IsImmune = true;
        timeStamp = Time.time + immuneDuration;
        carDamaged.Invoke();
    }

    private void FixedUpdate()
    {
        if (IsImmune)
        {
            if (Time.time > timeStamp)
            {
                IsImmune = false;
                meshRenderer.material = starndardMat;

            }


        }


    }
    private void Update()
    {
        if (IsImmune)
        {
            deltaStorage += Time.deltaTime;
            if(deltaStorage > flashInterval)
            {
                meshRenderer.material = damageVisualOn? damagedMat : damagedMat1;
                damageVisualOn = !damageVisualOn;
                deltaStorage = 0;
            }
            
        }else if (damageVisualOn)
        {
            damageVisualOn = false;
            meshRenderer.material = starndardMat;
        }
    }
}

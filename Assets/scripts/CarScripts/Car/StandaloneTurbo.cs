using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StandaloneTurbo : MonoBehaviour
{
    [SerializeField] GameObject[] turboVisuals;
    [SerializeField] Rigidbody carBody;
    [SerializeField] BoxCollider mainCollider;
    [SerializeField] float unstickForce = 40000;
    [SerializeField] GameObject superUnstickPrefab;
    float VerticalInput;
    [SerializeField] float turboPower = 40000;
    bool FootOnTurbo;

    private void Update()
    {
        VerticalInput = Input.GetAxis("Vertical");
        FootOnTurbo = Input.GetKey(KeyCode.LeftShift);

        turboVisuals[0].SetActive(FootOnTurbo && VerticalInput > 0);
        turboVisuals[1].SetActive(FootOnTurbo && VerticalInput < 0);

        Turbo();
        Unstick();


    }
    void Turbo()
    {
        if (!FootOnTurbo) return;
        Vector3 turboTarget = transform.position;
        turboTarget += transform.forward * VerticalInput * 5;
        Vector3 turboForce = transform.forward * turboPower * VerticalInput;
        carBody.AddForceAtPosition(turboForce, turboTarget);
    }
    void Unstick()
    {
        if (Input.GetMouseButton(0))
        {
            float xPos = Random.Range(mainCollider.bounds.min.x, mainCollider.bounds.max.x);
            float yPos = Random.Range(mainCollider.bounds.min.y, mainCollider.bounds.max.y);
            float zPos = Random.Range(mainCollider.bounds.min.z, mainCollider.bounds.max.z);
            Vector3 forceAxis = Vector3.zero;
            switch (Random.Range(0, 2))
            {
                case 0:
                    forceAxis = Vector3.forward;
                    break;
                case 1:
                    forceAxis = Vector3.right;
                    break;
                case 2:
                    forceAxis = Vector3.up;
                    break;
            }
            forceAxis *= (Random.value > 0.5 ? 1 : -1);
            carBody.AddForceAtPosition(unstickForce * forceAxis, new Vector3(xPos, yPos, zPos));
        }
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(superUnstickPrefab, transform.position, Quaternion.identity);
        }
    }
    //asdfadslkf
}

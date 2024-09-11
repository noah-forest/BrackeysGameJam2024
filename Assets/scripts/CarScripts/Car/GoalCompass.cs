using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCompass : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    public Transform currentGoal;

    private void Update()
    {
        if (currentGoal)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentGoal.position);
        }
    }
}

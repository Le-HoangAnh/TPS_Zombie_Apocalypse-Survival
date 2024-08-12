using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]

public class WalkPointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(WalkPoint walkPoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f;
        }

        Gizmos.DrawSphere(walkPoint.transform.position, 0.1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(walkPoint.transform.position + (walkPoint.transform.right * walkPoint.walkpointWidth / 2f), walkPoint.transform.position - (walkPoint.transform.right * walkPoint.walkpointWidth / 2f));

        //now draw a line from previous to next walk point

        if (walkPoint.previousWalkPoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = walkPoint.transform.right * walkPoint.walkpointWidth / 2f;
            Vector3 offsetTo = walkPoint.previousWalkPoint.transform.right * walkPoint.previousWalkPoint.walkpointWidth / 2f;

            Gizmos.DrawLine(walkPoint.transform.position + offset, walkPoint.previousWalkPoint.transform.position + offsetTo);
        }

        if (walkPoint.nextWalkPoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = walkPoint.transform.right * - walkPoint.walkpointWidth / 2f;
            Vector3 offsetTo = walkPoint.previousWalkPoint.transform.right * - walkPoint.previousWalkPoint.walkpointWidth / 2f;

            Gizmos.DrawLine(walkPoint.transform.position + offset, walkPoint.previousWalkPoint.transform.position + offsetTo);
        }
    }
}

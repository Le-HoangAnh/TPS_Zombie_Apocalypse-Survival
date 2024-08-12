using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class WalkPointManagerWindow : EditorWindow
{
    [MenuItem("Window/WalkPoints Editor Tools")]
    public static void showWindow()
    {
        GetWindow<WalkPointManagerWindow>("WalkPoints Editor Tools");
    }

    public Transform walkpointOrigin;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("walkpointOrigin"));

        if (walkpointOrigin == null)
        {
            EditorGUILayout.HelpBox("Please assign a walkpoint origin transform. ", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    void CreateButtons()
    {
        if (GUILayout.Button("Create Walkpoint"))
        {
            CreateWalkPoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WalkPoint>())
        {
            if (GUILayout.Button("Create Walkpoint Before"))
            {
                CreateWalkpointBefore();
            }
            if(GUILayout.Button("Create Walkpoint After"))
            {
                CreateWalkpointAfter();
            }
            if(GUILayout.Button("Remove Walkpoint"))
            {
                RemoveWalkpoint();
            }
        }
    }

    void CreateWalkPoint()
    {
        GameObject walkpointObject = new GameObject("zombieWalkpoint " + walkpointOrigin.childCount, typeof(WalkPoint));
        walkpointObject.transform.SetParent(walkpointOrigin, false);

        WalkPoint walkpoint = walkpointObject.GetComponent<WalkPoint>();
        if (walkpointOrigin.childCount > 1)
        {
            walkpoint.previousWalkPoint = walkpointOrigin.GetChild(walkpointOrigin.childCount - 2).GetComponent<WalkPoint>();
            walkpoint.previousWalkPoint.nextWalkPoint = walkpoint;

            walkpoint.transform.position = walkpoint.previousWalkPoint.transform.position;
            walkpoint.transform.forward = walkpoint.previousWalkPoint.transform.forward;
        }

        Selection.activeObject = walkpoint.gameObject;
    }

    void CreateWalkpointBefore()
    {
        GameObject walkpointObject = new GameObject("zombieWalkpoint " + walkpointOrigin.childCount, typeof(WalkPoint));
        walkpointObject.transform.SetParent(walkpointOrigin, false);

        WalkPoint newWalkPoint = walkpointObject.GetComponent<WalkPoint>();

        WalkPoint selectedWalkPoint = Selection.activeObject.GetComponent<WalkPoint>();

        walkpointObject.transform.position = selectedWalkPoint.transform.position;
        walkpointObject.transform.position = selectedWalkPoint.transform.forward;

        if (selectedWalkPoint.previousWalkPoint)
        {
            newWalkPoint.previousWalkPoint = selectedWalkPoint.previousWalkPoint;
            selectedWalkPoint.previousWalkPoint.nextWalkPoint = newWalkPoint;
        }

        newWalkPoint.nextWalkPoint = selectedWalkPoint;
        selectedWalkPoint.previousWalkPoint = newWalkPoint;

        newWalkPoint.transform.SetSiblingIndex(selectedWalkPoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWalkPoint.gameObject;
    }

    void CreateWalkpointAfter()
    {
        GameObject walkpointObject = new GameObject("zombieWalkpoint " + walkpointOrigin.childCount, typeof(WalkPoint));
        walkpointObject.transform.SetParent(walkpointOrigin, false);

        WalkPoint newWalkPoint = walkpointObject.GetComponent<WalkPoint>();

        WalkPoint selectedWalkPoint = Selection.activeObject.GetComponent<WalkPoint>();

        walkpointObject.transform.position = selectedWalkPoint.transform.position;
        walkpointObject.transform.position = selectedWalkPoint.transform.forward;

        if (selectedWalkPoint.nextWalkPoint != null)
        {
            selectedWalkPoint.nextWalkPoint.previousWalkPoint = newWalkPoint;
            newWalkPoint.nextWalkPoint = selectedWalkPoint.nextWalkPoint;
        }

        selectedWalkPoint.nextWalkPoint = newWalkPoint;

        newWalkPoint.transform.SetSiblingIndex(selectedWalkPoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWalkPoint.gameObject;
    }

    void RemoveWalkpoint()
    {
        WalkPoint selectedWalkPoint = Selection.activeObject.GetComponent<WalkPoint>();

        if (selectedWalkPoint.nextWalkPoint != null)
        {
            selectedWalkPoint.nextWalkPoint.previousWalkPoint = selectedWalkPoint.previousWalkPoint;
        }

        if (selectedWalkPoint.previousWalkPoint)
        {
            selectedWalkPoint.previousWalkPoint.nextWalkPoint = selectedWalkPoint.nextWalkPoint;
            Selection.activeGameObject = selectedWalkPoint.previousWalkPoint.gameObject;

            DestroyImmediate(selectedWalkPoint.gameObject);
        }
    }
}

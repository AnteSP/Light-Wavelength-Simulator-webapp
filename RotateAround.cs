using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [Header("Angle delta/update")]
    public float angle;

    [Header("The axis by which it will rotate around")]
    public Vector3 axis;

    public GameObject Target;
    Vector3 StartPos;


    void Start()
    {
        StartPos = transform.position;
    }

    void Update()
    {
        transform.RotateAround(Target.transform.position, axis, angle);


        float dist = Vector3.Distance(transform.position, Target.transform.position); 
        Vector3 newPos = transform.position + ( (Input.mouseScrollDelta.y != 0) ? Input.mouseScrollDelta.y * transform.forward * 0.1f * dist : Vector3.zero );

        dist = Vector3.Distance(newPos, Target.transform.position);
        transform.position = ( dist > 50  || dist < 0.1f) ? transform.position : newPos;
    }

    public void CHANGEAXIS(int x)
    {
        //1 for X axis, 2 for Y, 3 for Z, 4 for Reset
        switch (x)
        {
            case 1:
                axis = new Vector3(axis.x == 1 ? 0 : 1, axis.y, axis.z);
                break;
            case 2:
                axis = new Vector3(axis.x , axis.y == 1 ? 0 : 1, axis.z);
                break;
            case 3:
                axis = new Vector3(axis.x, axis.y, axis.z == 1 ? 0 : 1);
                break;
            case 4:
                transform.rotation = Quaternion.Euler( new Vector3(0, 0, 0));
                transform.position = StartPos;
                transform.LookAt(Target.transform);
                axis = Vector3.zero;
                break;

        }


    }


}

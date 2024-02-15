using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int pointNum;
    public Transform p1;
    public Transform p2;

    public float distanceMultiplier;
    public float minDistance;
    public GameObject point;

    public FTLMapPointLogic[] points;
    public Vector2[] pointPos;

    public Transform canvas;

    void Start()
    {
        points = new FTLMapPointLogic[pointNum];
        pointPos = new Vector2[pointNum];
        Vector2 previousPoint = new Vector2(p1.transform.position.x, p1.transform.position.y);
        for (int i = 0; i < pointNum; i++)
        {
            FTLMapPointLogic newPoint = Instantiate(point,canvas).GetComponent<FTLMapPointLogic>();
            points[i] = newPoint;
            Vector2 circle = Random.insideUnitCircle;
            circle[0] = Mathf.Clamp01(circle[0]);
            newPoint.transform.position = previousPoint + (circle * distanceMultiplier);
            previousPoint = newPoint.transform.position;
            pointPos[i] = previousPoint;
        }
        /*for (int i = 0; i < pointNum; i++)
        {
            for (int j =)
            points[i].connectedPoints = 
        }*/
    }
}

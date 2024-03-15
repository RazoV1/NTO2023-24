using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public int pointNum;
    public Transform p1;
    public Transform p2;

    public TMPro.TextMeshProUGUI score;
    public int rightAns;
    public TMP_InputField input;

    public float distanceMultiplier;
    public float maxDistance;
    public GameObject point;

    public FTLMapPointLogic movePoint;

    public Sprite exit;

    public FTLMapPointLogic[] points;
    public Vector2[] pointPos;
    public List<bool> isVisitedAll;

    public bool isOGE;

    public Transform canvas;

    private void Update()
    {
        if (isOGE)
        {
            if (input.text == rightAns.ToString())
            {
                score.text = (int.Parse(input.text) + 1).ToString();
                movePoint.PlayerMoveOnPoint();
                gameObject.SetActive(false);
                
            }
        }
    }
    
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
            circle[0] = Mathf.Clamp(circle[0], 0.3f, 1f);
            //circle[1] = Mathf.Clamp(circle[1],-01f,1f);
            newPoint.transform.position = previousPoint + (circle * distanceMultiplier);
            newPoint.transform.position = new Vector2(newPoint.transform.position.x, circle[1] * distanceMultiplier);
            previousPoint = newPoint.transform.position;
            pointPos[i] = previousPoint;
        }
        points[0].isActive = false;
        points[0].isPlayerOnPoint = true;
        points[0].isVisited = true;
        points[points.Length - 1].button.sprite = exit;
        for (int i = 0; i < pointNum; i++)
        {
            foreach (FTLMapPointLogic p in points[i..(pointNum)])
            {
                if (Vector2.Distance(points[i].transform.position,p.transform.position) < maxDistance)
                {
                    points[i].connectedPoints.Add(p);
                }
            }
            points[i].ConnectPoints();
        }
        foreach (FTLMapPointLogic p in points[0].connectedPoints)
        {
            p.isActive = true;
            p.button.GetComponent<Button>().interactable = true;
        }
        if (isOGE)
        {
            points[points.Length - 1].IsOGEEnd = true;
            /*foreach (var p in points)
            {
                rightAns += p.minLen;
                Debug.Log(rightAns);
            }
            rightAns = (int)(rightAns%100);*/
        }
    }
}

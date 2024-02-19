using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FTLMapPointLogic : MonoBehaviour
{
    public bool isActive;
    public bool isPlayerOnPoint;
    public List<FTLMapPointLogic> connectedPoints;
    public List<FTLMapPointLogic> pointsToDelete;
    public float minConnectDistance;
    public LineRenderer lineRenderer;

    public Color playerColor;
    public Color exploredColor;
    public Color unexploredColor;

    public Scene scene;

    public Transform canvas;

    public Image button;

    public void ConnectPoints()
    {
        int maxChance = connectedPoints.Count;
        int chance = maxChance;
        foreach (FTLMapPointLogic point in connectedPoints)
        {
            if (pointsToDelete.Contains(point))
            {
                continue;
            }
            if (chance == maxChance)
            {
                LineRenderer line = Instantiate(lineRenderer,canvas);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, point.transform.position);
                chance--;
            }
            else if (Random.Range(chance, maxChance) == chance)
            {
                LineRenderer line = Instantiate(lineRenderer, canvas);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, point.transform.position);
                chance--;
            }
            else
            {
                pointsToDelete.Add(point);
                Debug.Log(gameObject.name);
            }
        }
        foreach (FTLMapPointLogic point in pointsToDelete)
        {
            connectedPoints.Remove(point);
        }
    }

    private void Update()
    {
        if (isPlayerOnPoint)
        {
            button.color = playerColor;
        }
        else if (isActive)
        {
            button.color = unexploredColor;
        }
        else
        {
            button.color = exploredColor;
        }
    }

    public void PlayerMoveOnPoint()
    {
        if (isActive && !isPlayerOnPoint)
        {
            isPlayerOnPoint = true;
            isActive = false;
            Scene s = SceneManager.GetSceneByName("SampleScene");
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
            
            foreach (FTLMapPointLogic point in connectedPoints)
            {
                point.isActive = true;
                point.button.GetComponent<Button>().interactable = true;
            }
        }
    }
}

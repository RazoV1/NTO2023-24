using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FTLMapPointLogic : MonoBehaviour
{
    public bool isActive;
    public bool isPlayerOnPoint;

    public GameObject[] tasks;

    public List<FTLMapPointLogic> connectedPoints;
    public List<GameObject> lines;
    public List<GameObject> weightsObj;
    public List<FTLMapPointLogic> pointsToDelete;
    public List<FTLMapPointLogic> sourcePoints;
    public List<int> Weights;
    public float minConnectDistance;
    public LineRenderer lineRenderer;

    public SpriteRenderer dangerZone;
    public float dangerZoneStep;

    public bool IsOGE;
    public bool IsOGEEnd;
    public int minLen = 0;
    public bool isVisited = false;
    public int WeightRange;
    public TMPro.TextMeshProUGUI text;

    public Sprite playerColor;
    public Sprite exploredColor;
    public Sprite unexploredColor;
    public Sprite OGEStart;
    public Sprite OGEEnd;
    public Sprite OGEDull;
    public Sprite OGEActive;

    public Scene scene;

    public Transform canvas;

    public Image button;

    /*public bool FindLength(int prevL)
    {
        if (isPlayerOnPoint)
        {
            minLen = 0;
            return true;
        }
        List<FTLMapPointLogic> visited = new List<FTLMapPointLogic>();
        foreach (FTLMapPointLogic point in sourcePoints)
        {
            if (point.isVisited)
            {
                visited.Add(point);
            }
        }
        if (visited.Count != sourcePoints.Count) 
        {
            return false;
        }
        int MinLength = 10000000;
        foreach (FTLMapPointLogic p in visited)
        {
            if (p.minLen <= MinLength)
            {
                MinLength = p.minLen;
            }
        }
        minLen = MinLength + prevL;
        return true;
    }*/
    private void Start()
    {
        if (IsOGE)
        {
            ConnectPoints();
        }
    }

    public void ConnectPoints()
    {
        int maxChance = connectedPoints.Count;
        int chance = maxChance;
        if (connectedPoints.Contains(this))
        {
            connectedPoints.Remove(this);
        }
        foreach (FTLMapPointLogic point in connectedPoints)
        {
            if (pointsToDelete.Contains(point))
            {
                continue;
            }
            /*if (sourcePoints.Count == 0 && !isPlayerOnPoint && IsOGE)
            {
                foreach (FTLMapPointLogic p in connectedPoints)
                {
                    p.sourcePoints.Remove(this);
                }
                foreach (GameObject i in lines)
                {
                    Destroy(i);
                }
                foreach (GameObject i in weightsObj)
                {
                    Destroy(i);
                }
                Destroy(gameObject);
            }*/
            if (chance == maxChance)
            {
                LineRenderer line = Instantiate(lineRenderer,canvas);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, point.transform.position);
                if (IsOGE)
                {
                    lines.Add(line.gameObject);
                    //Weights.Add(Random.Range(1, 2));
                    //TMPro.TextMeshProUGUI t = Instantiate(text, canvas);
                    //t.text = Weights[Weights.Count - 1].ToString();
                    //t.transform.position = transform.position + (point.transform.position - transform.position) / 2f;
                    //weightsObj.Add(t.gameObject);
                    //point.sourcePoints.Add(this);
                    //minLen = Weights[0];
                    //point.sourcePoints.Add(this);
                }
                point.sourcePoints.Add(this);
                if (!IsOGE)
                {
                    maxChance -= 3;
                    continue;
                }
            }
            else if (Random.Range(chance, maxChance) == chance)
            {
                LineRenderer line = Instantiate(lineRenderer, canvas);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, point.transform.position);
                if (IsOGE)
                {
                    lines.Add(line.gameObject);
                    //Weights.Add(Random.Range(3, WeightRange + 1));
                    //TMPro.TextMeshProUGUI t = Instantiate(text, canvas);
                    //t.text = Weights[Weights.Count - 1].ToString();
                    //t.transform.position = transform.position + (point.transform.position - transform.position) / 2f;
                    //weightsObj.Add(t.gameObject);
                    //point.sourcePoints.Add(this);
                }
                point.sourcePoints.Add(this);
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

    void CheckSource()
    {
        if (IsOGE)
        {
            if (sourcePoints.Contains(null))
            {
                if (sourcePoints.Count == 1)
                {
                    foreach (FTLMapPointLogic point in connectedPoints)
                    {
                        point.CheckSource();
                    }
                    Destroy(gameObject);
                }
                else
                {
                    sourcePoints.Remove(null);
                }
            }
        }
    }
    private void Update()
    {
        if (IsOGEEnd)
        {
            button.sprite = OGEEnd;
        }
        else if (IsOGE) 
        {
            if (isPlayerOnPoint)
            {
                button.sprite = OGEStart;
            }
            else
            {
                button.sprite = OGEActive;
            }
        }
        else
        {
            if (isPlayerOnPoint)
            {
                button.sprite = playerColor;
            }
            else if (isActive)
            {
                button.sprite = unexploredColor;
            }
        }
    }

    public void ActivateOGE()
    {
        if (isActive && !isPlayerOnPoint && !IsOGE)
        {
            tasks[Random.Range(0, tasks.Length)].SetActive(true);
            PlayerMoveOnPoint();
        }
    }

    public void PlayerMoveOnPoint()
    {
        if (isActive && !isPlayerOnPoint && !IsOGE)
        {
            isPlayerOnPoint = true;
            isActive = false;
            dangerZone.size = new Vector2(dangerZone.size.x + dangerZoneStep, 10.24976f);
            //Scene s = SceneManager.GetSceneByName("SampleScene");
            //SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
            foreach (FTLMapPointLogic point in connectedPoints)
            {
                point.isActive = true;
                point.button.GetComponent<Button>().interactable = true;
            }
            foreach (FTLMapPointLogic p in sourcePoints)
            {
                if (p.isPlayerOnPoint)
                {
                    p.isPlayerOnPoint = false;
                    p.button.sprite = p.exploredColor;
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTLList : MonoBehaviour
{
    [SerializeField] private int listSize;
    [SerializeField] private GameObject part;
    [SerializeField] private Transform content;
    [SerializeField] private List<Sprite> avatars;
    [SerializeField] public List<GameObject> list = new List<GameObject>();
    [SerializeField] private List<string> names;
    public void Start()
    {
        list = new List<GameObject>();
        for (int i = 0; i < listSize; i++)
        {
            GameObject p = Instantiate(part,content);
            list.Add(p);
            FTLListPart l = p.GetComponent<FTLListPart>();
            l.score = Random.Range(0,100);
            l.name = names[Random.Range(0,names.Count)];
            l.im.sprite = avatars[Random.Range(0,avatars.Count)];
            p.name  = l.score.ToString();
            names.Remove(l.name);
        }
        list.Add(Instantiate(part, content));
        list[list.Count - 1].name = 0.ToString();
        list[list.Count - 1].GetComponent<FTLListPart>().name = "Вы";
        list[list.Count - 1].GetComponent<FTLListPart>().score = 0;
        
        //list.Sort();
    }
}

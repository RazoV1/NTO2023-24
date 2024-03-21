using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DronePersonalitu : MonoBehaviour
{
    public DroneController droneController;
    public List<string> normalQuotes;
    public List<string> appearQuotes;
    public List<string> dissapearQuotes;
    public List<string> attackQuotes;
    public List<string> shieldUpQuotes;
    public List<string> shieldDownQuotes;
    public List<string> shieldErrorQuotes;

    public List<string> exelentPuzzleQuotes;
    public List<string> goodPuzzleQuotes;
    public List<string> badPuzzleQuotes;

    public CharacterDialog characterDialog;

    public bool isTicking;
    public float time;
    
    public Animator emotions;
    public Animator surprised;
    public Animator angry;

    public void Appear(List<string> l)
    {
        string quote = l[Random.Range(0, l.Count)];
        if (quote[0] == 'h')
        {
            emotions.SetTrigger("happy");
        }
        else if (quote[0] == 'a')
        {
            emotions.SetTrigger("angry");
        }
        else
        {
            emotions.SetTrigger("surprised");
        }
        quote = quote[1..];
        List<string> q = new List<string>();
        q.Add(quote);
        characterDialog.StopAllCoroutines();
        characterDialog.StartText(q);
    }

    public void StartTick()
    {
        if (!isTicking)
        {
            isTicking = true;
            time = 0;
        }
        else
        {
            isTicking= false;
            if (time > 5 * 60f)
            {
                Appear(badPuzzleQuotes);
            }
            else if (time > 3 * 60f)
            {
                Appear(goodPuzzleQuotes);
            }
            else
            {
                Appear(exelentPuzzleQuotes);
            }
        }
    }

    public void Timer()
    {
        if (isTicking)
        {
            time += Time.deltaTime;
        }
    }

    private void Update()
    {
        Timer();
    }
}

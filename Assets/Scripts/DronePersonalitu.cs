using System.Collections;
using System.Collections.Generic;
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

    public CharacterDialog characterDialog;

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
        characterDialog.StartText(q);
    }

}

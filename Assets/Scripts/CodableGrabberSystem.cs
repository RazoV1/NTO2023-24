using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodableGrabberSystem : CodablePlatformSystem
{
    public override IEnumerator Execute()
    {
        if (targetedPosition != null)
        {
            while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
            {
                platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            targetedPosition = platform.position;
        }
        foreach (string i in commands)
        {
            inputField.text = "Executing... " + execotionPercent / commands.Length * 100f + "%";
            if (i == "")
            {
                execotionPercent++;
                continue;
            }
            if (i.Contains(' '))
            {
                string command = i.Split(' ')[0];
                string arg = i.Split(' ')[1];
                if (numArray.Contains(arg))
                {
                    if (command == "delay")
                    {
                        yield return new WaitForSeconds(int.Parse(arg));
                        execotionPercent++;
                        continue;
                    }
                    
                    else if (command == "right")
                    {
                        targetedPosition.x = platform.position.x + baseStep * int.Parse(arg);
                        can_move = CanMove(targetedPosition);
                    }
                    else if (command == "left")
                    {
                        targetedPosition.x = platform.position.x - baseStep * int.Parse(arg);
                        can_move = CanMove(targetedPosition);
                    }
                    else
                    {
                        Debug.Log("Error! Unknown command");
                        execotionPercent = 0;
                        StartCoroutine(Error("Error! Unknown command"));
                        StopCoroutine(Execute());
                        yield return null;
                    }

                    if (!can_move)
                    {
                        Debug.Log("Error! Cant move here and ur stupid");
                        execotionPercent = 0;
                        StartCoroutine(Error("Error! Cant move here and ur stupid"));
                        yield return null;
                        StopCoroutine(Execute());
                        
                    }
                    execotionPercent++;
                }
                else
                {
                    Debug.Log("Error! Invalid argument");
                    execotionPercent = 0;
                    StartCoroutine(Error("Error! Invalid argument"));
                    StopCoroutine(Execute());
                    yield return null;
                    
                }
            }
            else
            {
                Debug.Log("Error! Invalid input");
                execotionPercent = 0;
                StartCoroutine(Error("Error! Invalid input"));
                StopCoroutine(Execute());
                yield return null;
            }
            if (targetedPosition != null && can_move)
            {
                while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
                {
                    platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                targetedPosition = platform.position;
            }
            platform.position = targetedPosition;
            
            yield return null;
        }

        if (execotionPercent == 1)
        {
            inputField.text = "Finished!";
        }
        execotionPercent = 0;
        commands = new string[0];
    }
}

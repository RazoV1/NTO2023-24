using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodableGrabberSystem : CodablePlatformSystem
{
    [SerializeField] private bool is_grabbing = false;
    [SerializeField] private Transform currentHold;
    private RaycastHit hit;

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
                    else if (command == "grab")
                    {
                        
                        if (Physics.Raycast(platform.transform.position, -platform.transform.up, out hit))
                        {
                            targetedPosition = hit.point;
                            can_move = true;
                            is_grabbing = true;
                        }
                    }
                    else if (command == "drop")
                    {
                        if (currentHold != null)
                        {
                            currentHold.parent = null;
                            currentHold.GetComponent<Rigidbody>().isKinematic = false;
                            currentHold = null;
                        }
                        else
                        {
                            Debug.Log("Error! Nothing to drop!");
                            execotionPercent = 0;
                            StartCoroutine(Error("Error! Nothing to drop!"));
                            break;
                        }
                    }
                    else
                    {
                        Debug.Log("Error! Unknown command");
                        execotionPercent = 0;
                        StartCoroutine(Error("Error! Unknown command"));
                        break;
                        StopCoroutine(Execute());
                        yield return null;
                    }

                    if (!can_move)
                    {
                        Debug.Log("Error! Cant move here and ur stupid");
                        execotionPercent = 0;
                        StartCoroutine(Error("Error! Cant move here and ur stupid"));
                        break;
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
                    break;
                    StopCoroutine(Execute());
                    yield return null;
                    
                }
            }
            else
            {
                Debug.Log("Error! Invalid input");
                execotionPercent = 0;
                StartCoroutine(Error("Error! Invalid input"));
                break;
                StopCoroutine(Execute());
                yield return null;
            }
            if (targetedPosition != null && can_move)
            {
                if (is_grabbing)
                {
                    Vector3 StartPos = platform.position;
                    while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
                    {
                        platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed * Time.deltaTime);
                        yield return null;
                    }
                    if (hit.collider.tag == "is_movable")
                    {
                        currentHold = hit.collider.transform;
                        currentHold.GetComponent<Rigidbody>().isKinematic = true;
                        currentHold.SetParent(platform.transform,true);
                    }
                    yield return new WaitForSeconds(1f);
                    targetedPosition = StartPos;
                    while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
                    {
                        platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed / 2 * Time.deltaTime);
                        yield return null;
                    }
                    is_grabbing = false;

                }
                else
                {
                    while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
                    {
                        platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed * Time.deltaTime);
                        yield return null;
                    }
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

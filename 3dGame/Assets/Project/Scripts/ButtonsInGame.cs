using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsInGame : MonoBehaviour
{

    public GameObject[] players;
    public Text text;
    public GameObject target;

    public void ChangePlayer()
    {
        CameraFollow targetToFollow = target.GetComponent<CameraFollow>();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].activeInHierarchy)
            {
                players[i].SetActive(false);

                if (i < (players.Length - 1))
                {
                    foreach (Transform child in players[i + 1].transform)
                    {
                        if (child.tag == "target")
                        {
                            targetToFollow.CameraFollowObj = child.gameObject;
                        }
                    }

                    players[i + 1].SetActive(true);

                    return;
                }
                else
                {
                    foreach (Transform child in players[0].transform)
                    {
                        if (child.tag == "target")
                        {
                            targetToFollow.CameraFollowObj = child.gameObject;
                        }
                    }

                    players[0].SetActive(true);

                    return;
                }
            }
        }
    }
}

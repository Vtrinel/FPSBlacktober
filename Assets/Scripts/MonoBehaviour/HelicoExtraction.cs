using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicoExtraction : MonoBehaviour
{
    public GameObject player;
    public GameObject helico;

    public void PlayerInsideHelico()
    {
        player.transform.parent = helico.transform;
    }
}

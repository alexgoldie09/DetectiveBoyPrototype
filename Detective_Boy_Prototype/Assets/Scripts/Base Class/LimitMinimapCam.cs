using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitMinimapCam : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 12f, player.transform.position.z);
    }
}

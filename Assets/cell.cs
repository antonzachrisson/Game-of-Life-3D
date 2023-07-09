using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{
    [SerializeField] public bool alive;
    [SerializeField] public int aliveAdjacent;

    private void Start()
    {
        aliveAdjacent = 0;
    }

    private void Update()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = alive;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    public GameObject m_cell;
    public int grid_size;

    private GameObject[,,] m_grid;
    private Camera main_camera;

    private int numFrames;

    private void Start()
    {
        int halfpoint = grid_size / 2;
        numFrames = 0;

        m_grid = new GameObject[grid_size, grid_size, grid_size];

        for (int x = 0; x < grid_size; x++)
        {
            for (int y = 0; y < grid_size; y++)
            {
                for (int z = 0; z < grid_size; z++)
                {
                    m_grid[x, y, z] = GameObject.Instantiate(m_cell, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }

        main_camera = Camera.main;
        main_camera.transform.position = new Vector3(-halfpoint, grid_size + halfpoint, -halfpoint);
        main_camera.transform.LookAt(m_grid[halfpoint, halfpoint - 1, halfpoint].transform);

        //starting pattern
        //Each cell has a 1 in 7 chance of starting as alive
        for (int x = 0; x < grid_size; x++)
        {
            for (int y = 0; y < grid_size; y++)
            {
                for (int z = 0; z < grid_size; z++)
                {
                    if(Random.Range(0f,6f) < 1f)
                        m_grid[x, y, z].GetComponent<cell>().alive = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (numFrames >= 30)
        {
            numFrames = 0;
            turn();
        }
        numFrames++;
    }

    private void calcAliveAdjacent(GameObject m_cell)
    {
        int m_aliveAdjacent = 0;

        for (int x = (int)m_cell.transform.position.x - 1; x <= (int)m_cell.transform.position.x + 1; x++)
        {
            for (int y = (int)m_cell.transform.position.y - 1; y <= (int)m_cell.transform.position.y + 1; y++)
            {
                for (int z = (int)m_cell.transform.position.z - 1; z <= (int)m_cell.transform.position.z + 1; z++)
                {
                    if(x >= 0 && x < grid_size && y >= 0 && y < grid_size && z >= 0 && z < grid_size)
                    {
                        if (m_grid[x, y, z].GetComponent<cell>().alive)
                            m_aliveAdjacent++;
                    }
                }
            }
        }

        if (m_cell.GetComponent<cell>().alive)
            m_aliveAdjacent--;

        m_cell.GetComponent<cell>().aliveAdjacent = m_aliveAdjacent;
    }

    private void act(GameObject m_cell)
    {
        cell m_cellScript = m_cell.GetComponent<cell>();

        if (m_cellScript.alive)
        {
            if (m_cellScript.aliveAdjacent < 4)
                m_cellScript.alive = false;
            else if (m_cellScript.aliveAdjacent <= 5)
                m_cellScript.alive = true;
            else if (m_cellScript.aliveAdjacent > 5)
                m_cellScript.alive = false;
        }
        else
        {
            if (m_cellScript.aliveAdjacent == 5)
                m_cellScript.alive = true;
            else
                m_cellScript.alive = false;
        }
    }

    private void turn()
    {
        for (int x = 0; x < grid_size; x++)
        {
            for (int y = 0; y < grid_size; y++)
            {
                for (int z = 0; z < grid_size; z++)
                {
                    calcAliveAdjacent(m_grid[x, y, z]);
                    //act(m_grid[x, y, z]);
                }
            }
        }

        for (int x = 0; x < grid_size; x++)
        {
            for (int y = 0; y < grid_size; y++)
            {
                for (int z = 0; z < grid_size; z++)
                {
                    //calcAliveAdjacent(m_grid[x, y, z]);
                    act(m_grid[x, y, z]);
                }
            }
        }
    }
}

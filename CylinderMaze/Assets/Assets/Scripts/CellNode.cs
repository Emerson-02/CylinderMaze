using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellNode : MonoBehaviour
{
    public List<GameObject> neighbors = new List<GameObject>();
    public int gridX; // Posição X no grid
    public int gridY; // Posição Y no grid
    public bool northWall, southWall, eastWall, westWall; // Atributos representando as paredes

    public bool HasWallBetween(CellNode neighbor)
    {
        if (neighbor.gridX > this.gridX) // Vizinho está à leste
        {
            return this.eastWall || neighbor.westWall;
        }
        else if (neighbor.gridX < this.gridX) // Vizinho está à oeste
        {
            return this.westWall || neighbor.eastWall;
        }
        else if (neighbor.gridY > this.gridY) // Vizinho está ao norte
        {
            return this.northWall || neighbor.southWall;
        }
        else if (neighbor.gridY < this.gridY) // Vizinho está ao sul
        {
            return this.southWall || neighbor.northWall;
        }

        return false; // As células são vizinhas diagonais, não há parede entre elas
    }


}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstra
{
    private List<CellNode> nodes;
    private CellNode startNode;
    private CellNode endNode;

    public Dijkstra(List<CellNode> nodes, CellNode startNode, CellNode endNode)
    {
        this.nodes = nodes;
        this.startNode = startNode;
        this.endNode = endNode;
    }

    public List<CellNode> FindShortestPath()
    {
        Dictionary<CellNode, int> distances = new Dictionary<CellNode, int>();
        Dictionary<CellNode, CellNode> previousNodes = new Dictionary<CellNode, CellNode>();

        List<CellNode> unvisitedNodes = new List<CellNode>(nodes);

        foreach (CellNode node in nodes)
        {
            distances[node] = int.MaxValue;
        }

        distances[startNode] = 0;

        while (unvisitedNodes.Count > 0)
        {
            CellNode currentNode = unvisitedNodes.OrderBy(node => distances[node]).First();

            if (currentNode == endNode)
            {
                break;
            }

            unvisitedNodes.Remove(currentNode);

            foreach (GameObject neighborGameObject in currentNode.neighbors)
            {
                CellNode neighbor = neighborGameObject.GetComponent<CellNode>();
                int tentativeDistance = distances[currentNode] + 1; // Assume all edges have weight 1

                if (tentativeDistance < distances[neighbor])
                {
                    distances[neighbor] = tentativeDistance;
                    previousNodes[neighbor] = currentNode;
                }
            }
        }

        // Criação do caminho e retorno movidos para fora do loop
        CellNode endNodePath = endNode;
        List<CellNode> path = new List<CellNode>();
        while (endNodePath != null)
        {
            path.Insert(0, endNodePath);
            endNodePath = previousNodes.ContainsKey(endNodePath) ? previousNodes[endNodePath] : null;
        }

        return path;
    }
}

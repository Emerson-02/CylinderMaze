using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class LabyrinthGenerator : MonoBehaviour
{
    [SerializeField] int width = 5;
    [SerializeField] int height = 10;
    [SerializeField] float cellSize = 1f;
    [SerializeField] Material lineMaterial;
    public Material defaultMaterial = null;
    [SerializeField] GameObject plane;
    public GameObject cubeEsq, cubeCim, cubeDir, cubeBai, cubeChao;
    private int[,] directions = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } }; // Direções: cima, baixo, direita, esquerda
    private bool[,] visited; // Matriz para armazenar se a célula foi visitada ou não
    //private GameObject[,,] walls; // Matriz para armazenar as paredes
    Vector2Int entrada, saida; // Entrada e saída do labirinto
    GameObject[,,] grid;
    public Stack<CellNode> stack = new Stack<CellNode>();
    Dictionary<(CellNode, CellNode), GameObject> walls = new Dictionary<(CellNode, CellNode), GameObject>();
    List<CellNode> allNodes = new List<CellNode>();
    Stack<CellNode> visitedNodes = new Stack<CellNode>();

    
    

    void Start()
    {
        grid = new GameObject[width, height, 5];
        GenerateGrid();
        // string path = Application.dataPath + "Assets/ExportedMeshes/Plane.obj";
        // ObjExporter.MeshToFile(plane.GetComponent<MeshFilter>(), path);


        cubeEsq = Resources.Load("Cube esq") as GameObject;
        cubeEsq.tag = "Wall";
        Rigidbody rbEsq = cubeEsq.GetComponent<Rigidbody>();
        rbEsq.useGravity = false;
        rbEsq.isKinematic = true;

        cubeCim = Resources.Load("Cube cim") as GameObject;
        cubeCim.tag = "Wall";
        Rigidbody rbCim = cubeCim.GetComponent<Rigidbody>();
        rbCim.useGravity = false;
        rbCim.isKinematic = true;

        cubeDir = Resources.Load("Cube dir") as GameObject;
        cubeDir.tag = "Wall";
        Rigidbody rbDir = cubeDir.GetComponent<Rigidbody>();
        rbDir.useGravity = false;
        rbDir.isKinematic = true;

        cubeBai = Resources.Load("Cube bai") as GameObject;
        cubeBai.tag = "Wall";
        Rigidbody rbBai = cubeBai.GetComponent<Rigidbody>();
        rbBai.useGravity = false;
        rbBai.isKinematic = true;

        cubeChao = Resources.Load("Cube chao") as GameObject;
        cubeChao.tag = "Cell";
        Rigidbody rbChao = cubeChao.GetComponent<Rigidbody>();
        rbChao.useGravity = false;
        rbChao.isKinematic = true;

        // Adicione o script CellNode ao cubeChao
        CellNode cellNode = cubeChao.AddComponent<CellNode>();

        cubeEsq.GetComponent<Renderer>().sharedMaterial.color = Color.white; // Mude a cor para branco
        cubeCim.GetComponent<Renderer>().sharedMaterial.color = Color.white; // Mude a cor para branco
        cubeDir.GetComponent<Renderer>().sharedMaterial.color = Color.white; // Mude a cor para branco
        cubeBai.GetComponent<Renderer>().sharedMaterial.color = Color.white; // Mude a cor para branco
        cubeChao.GetComponent<Renderer>().sharedMaterial.color = Color.white; // Mude a cor para branco


        visited = new bool[width, height];
        //walls = new GameObject[width, height, 4]; // 4 paredes por célula

        // Inicialize as paredes
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Crie o cubeChao em cada célula
                GameObject chaoObject = Instantiate(cubeChao, new Vector3(i * cellSize + 0.500f, 0, j * cellSize + 0.500f), cubeChao.transform.rotation);
                chaoObject.transform.SetParent(plane.transform); // Defina "Plane" como o pai
                // muda o nome do objeto para ser o nome de sua posição no grid
                chaoObject.name = $"Chao ({i}, {j})";

                // Obtenha o componente CellNode do GameObject
                CellNode chao = chaoObject.GetComponent<CellNode>();

                // Adicione o cubeChao à matriz Grid
                grid[i, j, 0] = chaoObject;

                chao.gridX = i;
                chao.gridY = j;

                // Conecte a célula com seus vizinhos
                if (i > 0) // Se não for a primeira coluna
                {
                    // Conecte com a célula à esquerda
                    if (grid[i - 1, j, 0] != null)
                    {
                        ConnectCells(grid[i, j, 0], grid[i - 1, j, 0]);
                    }
                }
                if (j > 0) // Se não for a primeira linha
                {
                    if (grid[i, j - 1, 0] != null)
                    {
                        // Conecte com a célula acima
                        ConnectCells(grid[i, j, 0], grid[i, j - 1, 0]);
                    }
                }
                if (i < width - 1) // Se não for a última coluna
                {
                    if (grid[i + 1, j, 0] != null)
                    {
                        // Conecte com a célula à direita
                        ConnectCells(grid[i, j, 0], grid[i + 1, j, 0]);
                    }
                }
                if (j < height - 1) // Se não for a última linha
                {
                    if (grid[i, j + 1, 0] != null)
                    {
                        // Conecte com a célula abaixo
                        ConnectCells(grid[i, j, 0], grid[i, j + 1, 0]);
                    }
                }



                

            }


        }

        // Crie uma lista de CellNode
        //allNodes = new List<CellNode>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allNodes.Add(grid[i, j, 0].GetComponent<CellNode>());
            }
        }


        // cria as paredes
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {

                // Crie as paredes
                if (i < width - 1) // Se não for a última coluna
                {
                    // Crie uma parede vertical à direita da célula atual
                    GameObject wallDir = Instantiate(cubeDir, new Vector3((i + 1) * cellSize, 0, j * cellSize + 0.500f), cubeDir.transform.rotation);
                    wallDir.transform.SetParent(plane.transform); // Defina "Plane" como o pai
                    // muda o nome da parede para ser o a posição dos nodos que ela conecta
                    wallDir.name = $"Wall ({i},{j}) - ({i+1},{j})";

                    // adiciona os atributos de parede às células
                    AddWallsAttributes(grid[i, j, 0].GetComponent<CellNode>(), grid[i + 1, j, 0].GetComponent<CellNode>());

                    // Adicione a parede ao dicionário, associando-a com a célula atual e a célula à direita
                    CellNode node1 = grid[i, j, 0].GetComponent<CellNode>();
                    CellNode node2 = grid[i + 1, j, 0].GetComponent<CellNode>();
                    walls.Add(NormalizeNodes(node1, node2), wallDir);
                }
                if (j < height - 1) // Se não for a última linha
                {
                    // Crie uma parede horizontal abaixo da célula atual
                    GameObject wallCim = Instantiate(cubeCim, new Vector3(i * cellSize + 0.500f, 0, (j + 1) * cellSize), cubeCim.transform.rotation);
                    wallCim.transform.SetParent(plane.transform); // Defina "Plane" como o pai
                    // muda o nome da parede para ser o a posição dos nodos que ela conecta
                    wallCim.name = $"Wall ({i},{j}) - ({i},{j+1})";

                    // adiciona os atributos de parede às células
                    AddWallsAttributes(grid[i, j, 0].GetComponent<CellNode>(), grid[i, j + 1, 0].GetComponent<CellNode>());

                    // Adicione a parede ao dicionário, associando-a com a célula atual e a célula abaixo
                    CellNode node1 = grid[i, j, 0].GetComponent<CellNode>();
                    CellNode node2 = grid[i, j+1, 0].GetComponent<CellNode>();
                    walls.Add(NormalizeNodes(node1, node2), wallCim);
                }

            }
        }



        //* esse trecho acha o menor caminho entre dois pontos aleatórios
        // // Escolha dois nós aleatórios
        // System.Random random = new System.Random();
        // CellNode startNode = allNodes[random.Next(allNodes.Count)];
        // // imprima a posição do nó inicial no grid
        // Debug.Log($"({startNode.gridX}, {startNode.gridY})");
        // CellNode endNode = allNodes[random.Next(allNodes.Count)];
        // // imprima a posição do nó final no grid
        // Debug.Log($"({endNode.gridX}, {endNode.gridY})");

        // // Use o algoritmo de Dijkstra para encontrar o caminho mais curto
        // Dijkstra dijkstra = new Dijkstra(allNodes, startNode, endNode);
        // List<CellNode> shortestPath = dijkstra.FindShortestPath();

        // // imprima a posição de cada nó no caminho mais curto no grid
        // foreach (CellNode node in shortestPath)
        // {
        //     Debug.Log($"({node.gridX}, {node.gridY})");
        //     PaintCell(node.gridX, node.gridY, 0, lineMaterial);
        // }
        //* fim do trecho

        //* mostra os nodos vizinhos de um nodo aleatório
        // // Escolha um nó aleatório
        // System.Random random = new System.Random();
        // CellNode startNode = allNodes[random.Next(allNodes.Count)];

        // //mostra os vizinhos do nó escolhido
        // foreach (GameObject neighborObject in startNode.neighbors)
        // {
        //     CellNode neighbor = neighborObject.GetComponent<CellNode>();
        //     if (neighbor == null)
        //     {
        //         continue; // Skip this neighbor if it doesn't have a CellNode component
        //     }
        //     Debug.Log($"({neighbor.gridX}, {neighbor.gridY})");
        //     PaintCell(neighbor.gridX, neighbor.gridY, 0, lineMaterial);
        // }
        //* fim do trecho



        GenerateMaze();
        

        // adiciona parede em toda a borda de cima do grid
        for(int i = 0; i < width; i++)
        {
            // se não for igual a entrada
            if(i != entrada.x)
            {
                GameObject wallCim = Instantiate(cubeCim, new Vector3(i * cellSize + 0.500f, 0, height * cellSize), cubeCim.transform.rotation);
                wallCim.transform.SetParent(plane.transform); // Defina "Plane" como o pai
                // muda o nome da parede para ser o a posição dos nodos que ela conecta
                wallCim.name = $"Wall ({i},{height}) - ({i},{height+1})";
            }
        }

        // adiciona parede na posição de baixo da borda (0,0)
        GameObject wallBai = Instantiate(cubeBai, new Vector3(0 * cellSize + 0.500f, 0, 0 * cellSize), cubeBai.transform.rotation);
        wallBai.transform.SetParent(plane.transform); // Defina "Plane" como o pai
        // muda o nome da parede para ser o a posição dos nodos que ela conecta
        wallBai.name = $"Wall ({0},{0}) - ({0},{-1})";


        // adiciona parede em toda a borda de baixo do grid
        for (int i = 0; i < width; i++)
        {
            // se não for igual a saída
            if (i != saida.x)
            {
                wallBai = Instantiate(cubeBai, new Vector3(i * cellSize + 0.500f, 0, 0 * cellSize), cubeBai.transform.rotation);
                wallBai.transform.SetParent(plane.transform); // Defina "Plane" como o pai
                // muda o nome da parede para ser o a posição dos nodos que ela conecta
                wallBai.name = $"Wall ({i},{0}) - ({i},{-1})";
            }
            
        }

        // adiciona parede em toda borda esquerda
        for (int i = 0; i < height; i++)
        {
        
            GameObject wallEsq = Instantiate(cubeEsq, new Vector3(0 * cellSize, 0, i * cellSize + 0.500f), cubeEsq.transform.rotation);
            wallEsq.transform.SetParent(plane.transform); // Defina "Plane" como o pai
            // muda o nome da parede para ser o a posição dos nodos que ela conecta
            wallEsq.name = $"Wall ({0},{i}) - ({-1},{i})";
        }

        // adiciona parede em toda borda direita
        for (int i = 0; i < height; i++)
        {
            GameObject wallDir = Instantiate(cubeDir, new Vector3(width * cellSize, 0, i * cellSize + 0.500f), cubeDir.transform.rotation);
            wallDir.transform.SetParent(plane.transform); // Defina "Plane" como o pai
            // muda o nome da parede para ser o a posição dos nodos que ela conecta
            wallDir.name = $"Wall ({width},{i}) - ({width+1},{i})";
        }

        


    }

    void AddWallsAttributes(CellNode node1, CellNode node2)
    {
        (CellNode, CellNode) normalizedNodes = NormalizeNodes(node1, node2);

        // Se node1 ficar a esquerda de node2
        if (normalizedNodes.Item1 == node1)
        {
            // Adicione a parede à esquerda de node1
            node1.westWall = true;

            // Adicione a parede à direita de node2
            node2.eastWall = true;
        }
        // Se node1 ficar a direita de node2
        else
        {
            // Adicione a parede à direita de node1
            node1.eastWall = true;

            // Adicione a parede à esquerda de node2
            node2.westWall = true;
        }

        // Se node1 ficar acima de node2
        if (normalizedNodes.Item1 == node1)
        {
            // Adicione a parede acima de node1
            node1.northWall = true;

            // Adicione a parede abaixo de node2
            node2.southWall = true;
        }
        // Se node1 ficar abaixo de node2
        else
        {
            // Adicione a parede abaixo de node1
            node1.southWall = true;

            // Adicione a parede acima de node2
            node2.northWall = true;
        }


    }

    // Método para conectar duas células
    void ConnectCells(GameObject cell1, GameObject cell2)
    {
        // Verifique se ambos os GameObjects têm o componente Cell
        CellNode cellComponent1 = cell1.GetComponent<CellNode>();
        CellNode cellComponent2 = cell2.GetComponent<CellNode>();

        if (cellComponent1 == null)
        {
            // Se o cell1 não tem o componente Cell, adicione-o
            cellComponent1 = cell1.AddComponent<CellNode>();
        }

        if (cellComponent2 == null)
        {
            // Se o cell2 não tem o componente Cell, adicione-o
            cellComponent2 = cell2.AddComponent<CellNode>();
        }

        // Adicione cell2 à lista de vizinhos de cell1
        if (!cellComponent1.neighbors.Contains(cell2))
        {
            cellComponent1.neighbors.Add(cell2);
        }

        // Adicione cell1 à lista de vizinhos de cell2
        if (!cellComponent2.neighbors.Contains(cell1))
        {
            cellComponent2.neighbors.Add(cell1);
        }
    }


    (CellNode, CellNode) NormalizeNodes(CellNode node1, CellNode node2)
    {
        // Se node1 vem antes de node2 na grade, retorne (node1, node2)
        if (node1.gridX < node2.gridX || (node1.gridX == node2.gridX && node1.gridY < node2.gridY))
        {
            return (node1, node2);
        }
        // Caso contrário, retorne (node2, node1)
        else
        {
            return (node2, node1);
        }
    }

    


    void GenerateGrid()
    {

        // Cria o Plane
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "GridPlane"; 
        plane.transform.position = new Vector3(width * cellSize / 2, 0, height * cellSize / 2);
        plane.transform.localScale = new Vector3(width * cellSize / 10, 1, height * cellSize / 10);


        // for (int x = 0; x <= width; x++)
        // {
        //     DrawLine(new Vector3(x * cellSize, 0.01f, 0), new Vector3(x * cellSize, 0.01f, height * cellSize));
        // }

        // for (int y = 0; y <= height; y++)
        // {
        //     DrawLine(new Vector3(0, 0.01f, y * cellSize), new Vector3(width * cellSize, 0.01f, y * cellSize));
        // }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject();
        lineObj.transform.parent = plane.transform;
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void PaintCell(int x, int y, int z, Material material)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("As coordenadas estão fora dos limites do grid.");
            return;
        }

        // Obtenha a referência ao objeto existente
        GameObject cell = grid[x, y, z];

        // Verifique se a célula existe
        if (cell == null)
        {
            Debug.LogError("Não há célula existente nas coordenadas fornecidas.");
            return;
        }

        // Altere a cor do objeto existente
        Renderer renderer = cell.GetComponent<Renderer>();
        renderer.material = material;
    }

    public void RemovePaintFromCell(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("As coordenadas estão fora dos limites do grid.");
            return;
        }

        // Encontre a célula na posição especificada
        Vector3 cellPosition = new Vector3(x * cellSize + cellSize / 2, 0.01f, y * cellSize + cellSize / 2);
        Collider[] hitColliders = Physics.OverlapSphere(cellPosition, cellSize / 2);
        foreach (var hitCollider in hitColliders)
        {
            Renderer renderer = hitCollider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Redefina o material para o material padrão
                renderer.material = defaultMaterial;
            }
        }
    }

    void GenerateMaze()
    {
        ChooseEntrance();

        StartCoroutine(DFS(grid[entrada.x, entrada.y, 0].GetComponent<CellNode>()));
        //DFS(grid[entrada.x, entrada.y, 0].GetComponent<CellNode>());

        // destroi a parede de baixo da saida
        //GameObject wallBai = walls[NormalizeNodes(grid[saida.x, saida.y, 0].GetComponent<CellNode>(), grid[saida.x, saida.y - 1, 0].GetComponent<CellNode>())];
        //Destroy(wallBai);

        //DisconnectNodesWithWalls();

        // Encontre o caminho mais curto entre a entrada e a saída
        //Dijkstra dijkstra = new Dijkstra(allNodes, grid[entrada.x, entrada.y, 0].GetComponent<CellNode>(), grid[saida.x, saida.y, 0].GetComponent<CellNode>());
        //List<CellNode> path = dijkstra.FindShortestPath();

        // Stack<CellNode> path = new Stack<CellNode>(); 

        // path = RightWay(grid[entrada.x, entrada.y, 0].GetComponent<CellNode>(), grid[saida.x, saida.y, 0].GetComponent<CellNode>());

        // // pega o tamanho do caminho
        // int pathSize = path.Count;
        // Debug.Log(pathSize);
        // while(pathSize > 0)
        // {
        //     CellNode node = path.Pop();
        //     Debug.Log($"({node.gridX}, {node.gridY})");
        // }


        // Pinte o caminho no labirinto
        //StartCoroutine(PaintPath(path));
        //PaintPath(path);
    }

    void ChooseEntrance()
    {
        // escolhe uma posição aleatória na borda superior do grid
        int x = Random.Range(0, width);
        int y = height - 1;
        

        // pinta a célula de entrada
        PaintCell(x, y, 0, lineMaterial);

        // marca a posição como entrada
        entrada = new Vector2Int(x, y);
    }


    
    IEnumerator DFS(CellNode startNode)
    {
        stack.Push(startNode);
        visited[startNode.gridX, startNode.gridY] = true;
        visitedNodes.Push(startNode);

        while (stack.Count > 0)
        {
            CellNode currentNode = stack.Pop();
            //saida = new Vector2Int(currentNode.gridX, currentNode.gridY);   
            int x = currentNode.gridX;
            int y = currentNode.gridY;

            //Imprime a célula atual
            //Debug.Log($"atual: ({x}, {y})");

            // Se a célula atual estiver na borda inferior, marque-a como a saída
            if (y == 0)
            {
                saida = new Vector2Int(x, y);
                //Imprime
                //Debug.Log($"saida: ({x}, {y})");
            }

            // Obtenha a lista de vizinhos
            List<GameObject> neighbors = new List<GameObject>(currentNode.neighbors); 

            // Remova os vizinhos que já foram visitados
            neighbors.RemoveAll(neighborObject => {
                CellNode neighbor = neighborObject.GetComponent<CellNode>();
                return neighbor == null || visited[neighbor.gridX, neighbor.gridY];
            }); 

            // Verifique se existem vizinhos para visitar
            if (neighbors.Count > 0)
            {
                // Escolha um vizinho aleatório
                int randomIndex = Random.Range(0, neighbors.Count);
                GameObject neighborObject = neighbors[randomIndex];

                CellNode neighbor = neighborObject.GetComponent<CellNode>();

                //imprime o vizinho escolhido
                //Debug.Log($"vizinho: ({neighbor.gridX}, {neighbor.gridY})");

                // Marque o vizinho como visitado
                visited[neighbor.gridX, neighbor.gridY] = true;

                // Empurre o vizinho para a pilha
                stack.Push(neighbor);
                visitedNodes.Push(neighbor);

                // Pinte a célula
                PaintCell(neighbor.gridX, neighbor.gridY, 0, lineMaterial);

                // Remove a parede entre a célula atual e o vizinho
                GameObject wall = walls[NormalizeNodes(currentNode, neighbor)];
                Destroy(wall);

                // Pausa a execução do script por 0.1 segundos
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                // Se não houver vizinhos não visitados, volte pela pilha visitedNodes
                while (visitedNodes.Count > 0)
                {
                    CellNode visitedNode = visitedNodes.Pop();

                    // Obtenha a lista de vizinhos do nó visitado
                    List<GameObject> visitedNodeNeighbors = new List<GameObject>(visitedNode.neighbors);

                    // Remova os vizinhos que já foram visitados
                    visitedNodeNeighbors.RemoveAll(neighborObject => {
                        CellNode neighbor = neighborObject.GetComponent<CellNode>();
                        return neighbor == null || visited[neighbor.gridX, neighbor.gridY];
                    });

                    // Se o nó visitado ainda tiver vizinhos não visitados, empurre-o de volta para a pilha e saia do loop
                    if (visitedNodeNeighbors.Count > 0)
                    {
                        stack.Push(visitedNode);
                        break;
                    }
                }
            }
            
        }

        // Remove a parede de baixo da saída
        // caça a parede pelo nome
        GameObject wallBai = GameObject.Find($"Wall ({saida.x},0) - ({saida.x},-1)");
        Destroy(wallBai);
    }


    void PaintPath(List<CellNode> path)
    {
        foreach (CellNode node in path)
        {
            PaintCell(node.gridX, node.gridY, 0, lineMaterial);
            //yield return new WaitForSeconds(0.1f);
        }
    }

    public void DisconnectNodes(CellNode node1, CellNode node2)
    {
        // Remove node2 from the neighbors of node1
        node1.neighbors.Remove(node2.gameObject);

        // Remove node1 from the neighbors of node2
        node2.neighbors.Remove(node1.gameObject);
    }

    void DisconnectNodesWithWalls()
    {
        // Percorra todos os nós do labirinto
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Obtenha o nó atual
                CellNode currentNode = grid[x, y, 0].GetComponent<CellNode>();

                // Crie uma cópia da lista de vizinhos para evitar problemas de modificação durante a iteração
                List<GameObject> neighbors = new List<GameObject>(currentNode.neighbors);

                // Verifique cada vizinho
                foreach (GameObject neighborObject in neighbors)
                {
                    CellNode neighbor = neighborObject.GetComponent<CellNode>();

                    // Se houver uma parede pelo nome do nó atual e do vizinho, remova-a
                    if (walls.ContainsKey(NormalizeNodes(currentNode, neighbor)))
                    {
                        GameObject wall = walls[NormalizeNodes(currentNode, neighbor)];
                        Destroy(wall);
                    }
                }
            }
        }
    }

    Stack<CellNode> RightWay(CellNode startNode, CellNode endNode)
    {
        Stack<CellNode> nodes = new Stack<CellNode>();
        Stack<CellNode> visitedNodes = new Stack<CellNode>();
        nodes.Push(startNode);
        visitedNodes.Push(startNode);

        Dictionary<CellNode, List<CellNode>> visitedNeighbors = new Dictionary<CellNode, List<CellNode>>();

        Stack<CellNode> path = new Stack<CellNode>();

        while(nodes.Count > 0)
        {
            CellNode currentNode = nodes.Pop();
            path.Push(currentNode); // Adiciona o nó atual ao caminho
            PaintCell(currentNode.gridX, currentNode.gridY, 0, lineMaterial); // Pinta o nó atual

            bool hasUnvisitedNeighbor = false;
            foreach (GameObject neighborObject in currentNode.neighbors)
            {
                CellNode neighbor = neighborObject.GetComponent<CellNode>();
                if (neighbor != null && !visitedNodes.Contains(neighbor) && !currentNode.HasWallBetween(neighbor) && (!visitedNeighbors.ContainsKey(currentNode) || !visitedNeighbors[currentNode].Contains(neighbor)))
                {
                    nodes.Push(neighbor);
                    visitedNodes.Push(neighbor);
                    path.Push(neighbor); // Adiciona o vizinho à pilha do caminho
                    hasUnvisitedNeighbor = true;

                    // Adiciona o vizinho à lista de vizinhos visitados para o nó atual
                    if (!visitedNeighbors.ContainsKey(currentNode))
                    {
                        visitedNeighbors[currentNode] = new List<CellNode>();
                    }
                    visitedNeighbors[currentNode].Add(neighbor);

                    break; // Sai do loop assim que encontrar um vizinho sem parede
                }
            }

            // Se o nó atual não tem vizinhos não visitados, continue removendo nós da pilha
            while(nodes.Count > 0 && !hasUnvisitedNeighbor)
            {
                RemovePaintFromCell(currentNode.gridX, currentNode.gridY); // Despinta o nó atual
                path.Pop(); // Remove o nó atual do caminho
                currentNode = nodes.Pop();
                PaintCell(currentNode.gridX, currentNode.gridY, 0, defaultMaterial); // Pinta o novo nó atual

                foreach (GameObject neighborObject in currentNode.neighbors)
                {
                    CellNode neighbor = neighborObject.GetComponent<CellNode>();
                    if (neighbor != null && !visitedNodes.Contains(neighbor) && !currentNode.HasWallBetween(neighbor) && (!visitedNeighbors.ContainsKey(currentNode) || !visitedNeighbors[currentNode].Contains(neighbor)))
                    {
                        nodes.Push(neighbor);
                        visitedNodes.Push(neighbor);
                        path.Push(neighbor); // Adiciona o vizinho à pilha do caminho
                        hasUnvisitedNeighbor = true;

                        // Adiciona o vizinho à lista de vizinhos visitados para o nó atual
                        if (!visitedNeighbors.ContainsKey(currentNode))
                        {
                            visitedNeighbors[currentNode] = new List<CellNode>();
                        }
                        visitedNeighbors[currentNode].Add(neighbor);

                        break;
                    }
                }
            }

            if (currentNode == endNode)
            {
                break;
            }
        }
        return path; // Retorna o caminho
        
        
    }

}

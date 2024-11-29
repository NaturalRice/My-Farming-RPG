using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合类
using UnityEngine; // 引用Unity引擎的命名空间

public class AStar : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    [Header("Tiles & Tilemap References")] // 标题注释，用于编辑器中组织字段
    [Header("Options")] // 标题注释，用于编辑器中组织字段
    [SerializeField] private bool observeMovementPenalties = true; // 可序列化的私有字段，用于在编辑器中设置是否考虑移动惩罚

    [Range(0, 20)] // 属性用于在编辑器中限制字段的值在0到20之间
    [SerializeField] private int pathMovementPenalty = 0; // 可序列化的私有字段，用于设置路径移动惩罚
    [Range(0, 20)] // 属性用于在编辑器中限制字段的值在0到20之间
    [SerializeField] private int defaultMovementPenalty = 0; // 可序列化的私有字段，用于设置默认移动惩罚

    private GridNodes gridNodes; // 私有字段，存储网格节点
    private Node startNode; // 私有字段，存储起始节点
    private Node targetNode; // 私有字段，存储目标节点
    private int gridWidth; // 私有字段，存储网格宽度
    private int gridHeight; // 私有字段，存储网格高度
    private int originX; // 私有字段，存储原点X坐标
    private int originY; // 私有字段，存储原点Y坐标

    private List<Node> openNodeList; // 私有字段，存储开放节点列表
    private HashSet<Node> closedNodeList; // 私有字段，存储关闭节点列表

    private bool pathFound = false; // 私有字段，标记是否找到路径

    /// <summary>
    /// 构建给定场景名称的路径，从startGridPosition到endGridPosition，并将移动步骤添加到传入的npcMovementStepStack中。如果找到路径返回true，否则返回false。
    /// </summary>
    public bool BuildPath(SceneName sceneName, Vector2Int startGridPosition, Vector2Int endGridPosition, Stack<NPCMovementStep> npcMovementStepStack)
    {
        pathFound = false; // 重置路径找到标记

        if (PopulateGridNodesFromGridPropertiesDictionary(sceneName, startGridPosition, endGridPosition)) // 从字典中填充网格节点
        {
            if (FindShortestPath()) // 查找最短路径
            {
                UpdatePathOnNPCMovementStepStack(sceneName, npcMovementStepStack); // 更新NPC移动步骤栈

                return true; // 返回路径找到
            }
        }
        return false; // 返回路径未找到
    }

    private void UpdatePathOnNPCMovementStepStack(SceneName sceneName, Stack<NPCMovementStep> npcMovementStepStack)
    {
        Node nextNode = targetNode; // 从目标节点开始

        while (nextNode != null) // 遍历路径直到起始节点
        {
            NPCMovementStep npcMovementStep = new NPCMovementStep(); // 创建新的NPC移动步骤

            npcMovementStep.sceneName = sceneName; // 设置场景名称
            npcMovementStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x + originX, nextNode.gridPosition.y + originY); // 设置网格坐标

            npcMovementStepStack.Push(npcMovementStep); // 将移动步骤推入栈

            nextNode = nextNode.parentNode; // 移动到父节点
        }
    }

    /// <summary>
    /// 返回是否找到路径
    /// </summary>
    private bool FindShortestPath()
    {
        // 将起始节点添加到开放列表
        openNodeList.Add(startNode);

        // 循环遍历开放节点列表直到为空
        while (openNodeList.Count > 0)
        {
            // 排序列表
            openNodeList.Sort();

            // 当前节点 = 开放列表中fCost最低的节点
            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);

            // 将当前节点添加到关闭列表
            closedNodeList.Add(currentNode);

            // 如果当前节点 = 目标节点，则结束
            if (currentNode == targetNode)
            {
                pathFound = true;
                break;
            }

            // 评估当前节点的每个邻居的fCost
            EvaluateCurrentNodeNeighbours(currentNode);
        }

        if (pathFound)
        {
            return true; // 返回路径找到
        }
        else
        {
            return false; // 返回路径未找到
        }
    }

    private void EvaluateCurrentNodeNeighbours(Node currentNode)
    {
        Vector2Int currentNodeGridPosition = currentNode.gridPosition; // 当前节点的网格位置

        Node validNeighbourNode; // 有效的邻居节点

        // 遍历所有方向
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue; // 跳过自身

                validNeighbourNode = GetValidNodeNeighbour(currentNodeGridPosition.x + i, currentNodeGridPosition.y + j); // 获取有效的邻居节点

                if (validNeighbourNode != null) // 如果邻居节点有效
                {
                    // 计算邻居的新gCost
                    int newCostToNeighbour;

                    if (observeMovementPenalties) // 如果考虑移动惩罚
                    {
                        newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, validNeighbourNode) + validNeighbourNode.movementPenalty; // 计算新的gCost
                    }
                    else
                    {
                        newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, validNeighbourNode); // 计算新的gCost
                    }

                    bool isValidNeighbourNodeInOpenList = openNodeList.Contains(validNeighbourNode); // 检查邻居节点是否在开放列表中

                    if (newCostToNeighbour < validNeighbourNode.gCost || !isValidNeighbourNodeInOpenList) // 如果新gCost更低或邻居不在开放列表中
                    {
                        validNeighbourNode.gCost = newCostToNeighbour; // 更新gCost
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode); // 更新hCost

                        validNeighbourNode.parentNode = currentNode; // 设置父节点

                        if (!isValidNeighbourNodeInOpenList) // 如果邻居不在开放列表中
                        {
                            openNodeList.Add(validNeighbourNode); // 添加到开放列表
                        }
                    }
                }
            }
        }
    }

    private int GetDistance(Node nodeA, Node nodeB) // 计算两个节点之间的距离
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x); // X坐标差的绝对值
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y); // Y坐标差的绝对值

        if (dstX > dstY) // 使用对角线距离公式
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    private Node GetValidNodeNeighbour(int neighboutNodeXPosition, int neighbourNodeYPosition) // 获取有效的邻居节点
    {
        // 如果邻居节点位置超出网格，则返回null
        if (neighboutNodeXPosition >= gridWidth || neighboutNodeXPosition < 0 || neighbourNodeYPosition >= gridHeight || neighbourNodeYPosition < 0)
        {
            return null;
        }

        // 如果邻居是障碍物或在关闭列表中，则跳过
        Node neighbourNode = gridNodes.GetGridNode(neighboutNodeXPosition, neighbourNodeYPosition); // 获取邻居节点

        if (neighbourNode.isObstacle || closedNodeList.Contains(neighbourNode)) // 检查是否是障碍物或在关闭列表中
        {
            return null;
        }
        else
        {
            return neighbourNode; // 返回邻居节点
        }
    }

    private bool PopulateGridNodesFromGridPropertiesDictionary(SceneName sceneName, Vector2Int startGridPosition, Vector2Int endGridPosition) // 从字典中填充网格节点
    {
        // 获取场景的网格属性字典
        SceneSave sceneSave;

        if (GridPropertiesManager.Instance.GameObjectSave.sceneData.TryGetValue(sceneName.ToString(), out sceneSave)) // 尝试获取场景保存数据
        {
            // 获取网格属性详情字典
            if (sceneSave.gridPropertyDetailsDictionary != null)
            {
                // 获取网格尺寸
                if (GridPropertiesManager.Instance.GetGridDimensions(sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin))
                {
                    // 根据网格属性字典创建节点网格
                    gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);
                    gridWidth = gridDimensions.x;
                    gridHeight = gridDimensions.y;
                    originX = gridOrigin.x;
                    originY = gridOrigin.y;

                    // 创建开放节点列表
                    openNodeList = new List<Node>();

                    // 创建关闭节点列表
                    closedNodeList = new HashSet<Node>();
                }
                else
                {
                    return false; // 如果获取网格尺寸失败，返回false
                }

                // 填充起始节点
                startNode = gridNodes.GetGridNode(startGridPosition.x - gridOrigin.x, startGridPosition.y - gridOrigin.y);

                // 填充目标节点
                targetNode = gridNodes.GetGridNode(endGridPosition.x - gridOrigin.x, endGridPosition.y - gridOrigin.y);

                // 填充网格的障碍物和路径信息
                for (int x = 0; x < gridDimensions.x; x++)
                {
                    for (int y = 0; y < gridDimensions.y; y++)
                    {
                        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(x + gridOrigin.x, y + gridOrigin.y, sceneSave.gridPropertyDetailsDictionary);

                        if (gridPropertyDetails != null) // 如果获取到网格属性详情
                        {
                            // 如果是NPC障碍物
                            if (gridPropertyDetails.isNPCObstacle == true)
                            {
                                Node node = gridNodes.GetGridNode(x, y);
                                node.isObstacle = true;
                            }
                            // 如果是路径
                            else if (gridPropertyDetails.isPath == true)
                            {
                                Node node = gridNodes.GetGridNode(x, y);
                                node.movementPenalty = pathMovementPenalty;
                            }
                            // 否则
                            else
                            {
                                Node node = gridNodes.GetGridNode(x, y);
                                node.movementPenalty = defaultMovementPenalty;
                            }
                        }
                    }
                }
            }
            else
            {
                return false; // 如果网格属性详情字典为空，返回false
            }
        }
        else
        {
            return false; // 如果尝试获取场景保存数据失败，返回false
        }

        return true; // 返回成功填充网格节点
    }
}

/*1. * *AStar 类 * *：这个类实现了A* 路径查找算法，继承自`MonoBehaviour`，可以附加到Unity场景中的GameObject上。

2. **序列化字段**：`observeMovementPenalties`、`pathMovementPenalty`和`defaultMovementPenalty`是可序列化的字段，用于在Unity编辑器中设置路径查找的参数。

3. **网格和节点**：`gridNodes`、`startNode`、`targetNode`、`gridWidth`、`gridHeight`、`originX`和`originY`字段用于存储网格和节点信息。

4. **开放和关闭列表**：`openNodeList`和`closedNodeList`分别存储A*算法中的开放节点和关闭节点。

5. **路径查找**：`BuildPath`方法用于构建路径，它调用`PopulateGridNodesFromGridPropertiesDictionary`方法从字典中填充网格节点，然后调用`FindShortestPath`方法查找最短路径，最后调用`UpdatePathOnNPCMovementStepStack`方法更新NPC移动步骤栈。

6. **更新NPC移动步骤栈**：`UpdatePathOnNPCMovementStepStack`方法用于根据找到的路径更新NPC移动步骤栈。

7. **查找最短路径**：`FindShortestPath`方法实现了A*算法的核心逻辑，它使用开放列表和关闭列表来查找从起始节点到目标节点的最短路径。

8. **评估邻居节点**：`EvaluateCurrentNodeNeighbours`方法用于评估当前节点的邻居节点，并更新它们的成本和父节点。

9. **计算距离**：`GetDistance`方法用于计算两个节点之间的距离，使用对角线距离公式。

10. **获取有效邻居节点**：`GetValidNodeNeighbour`方法用于获取有效的邻居节点，跳过障碍物和超出网格的节点。

11. **填充网格节点**：`PopulateGridNodesFromGridPropertiesDictionary`方法从字典中填充网格节点，设置障碍物和路径信息。

这个脚本通常用于游戏中的NPC路径查找，可以根据NPC的位置和目标位置自动计算出一条最短路径，并指导NPC移动。*/
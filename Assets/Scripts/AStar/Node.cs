using System; // 引用System命名空间，提供基本的类和基础结构
using UnityEngine; // 引用Unity引擎的命名空间

public class Node : IComparable<Node> // 定义一个公共类Node，实现IComparable<Node>接口，用于比较节点
{
    public Vector2Int gridPosition; // 公开字段，存储节点在网格中的位置
    public int gCost = 0; // 公开字段，存储从起始节点到当前节点的距离（gCost）
    public int hCost = 0; // 公开字段，存储从当前节点到结束节点的预估距离（hCost）
    public bool isObstacle = false; // 公开字段，标记节点是否为障碍物
    public int movementPenalty; // 公开字段，存储节点的移动惩罚值
    public Node parentNode; // 公开字段，存储当前节点的父节点

    public Node(Vector2Int gridPosition) // 类的构造函数，用于初始化节点的位置
    {
        this.gridPosition = gridPosition; // 设置节点的网格位置

        parentNode = null; // 初始化时，父节点设置为null
    }

    public int FCost // 公开属性，计算节点的总成本（FCost = gCost + hCost）
    {
        get // FCost的getter方法
        {
            return gCost + hCost; // 返回gCost和hCost的和
        }
    }

    public int CompareTo(Node nodeToCompare) // 实现IComparable<Node>接口的CompareTo方法，用于比较两个节点
    {
        // 如果当前实例的FCost小于nodeToCompare的FCost，比较结果为小于0
        // 如果当前实例的FCost大于nodeToCompare的FCost，比较结果为大于0
        // 如果值相同，比较结果为0

        int compare = FCost.CompareTo(nodeToCompare.FCost); // 比较两个节点的FCost
        if (compare == 0) // 如果FCost相同
        {
            compare = hCost.CompareTo(nodeToCompare.hCost); // 进一步比较hCost
        }
        return compare; // 返回比较结果
    }
}

/*Node 类：这个类表示路径查找算法中的一个节点，它包含位置、成本和障碍物等信息。

属性：

gridPosition：节点在网格中的位置。
gCost：从起始节点到当前节点的实际代价。
hCost：从当前节点到目标节点的预估代价（启发式成本）。
isObstacle：标记节点是否为障碍物。
movementPenalty：节点的移动惩罚值，用于影响路径成本。
parentNode：当前节点的父节点，用于重建路径。
构造函数：初始化节点的位置，并设置父节点为null。

FCost 属性：计算节点的总成本，即gCost和hCost的和。

CompareTo 方法：实现IComparable<Node>接口的方法，用于比较两个节点的FCost。如果FCost相同，则进一步比较hCost。这个方法在排序算法中非常重要，因为它决定了节点的处理顺序。

这个类是A*算法中的核心数据结构，通过比较节点的FCost来确定哪些节点应该优先处理，从而找到从起点到终点的最短路径。*/
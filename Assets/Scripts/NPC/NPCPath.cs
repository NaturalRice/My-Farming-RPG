// 引用系统和Unity引擎的命名空间
using System;
using System.Collections.Generic;
using UnityEngine;

// 这个类需要依附于包含NPCMovement组件的游戏对象
[RequireComponent(typeof(NPCMovement))]
public class NPCPath : MonoBehaviour
{
    // 公开字段，用于存储NPC的移动步骤堆栈
    public Stack<NPCMovementStep> npcMovementStepStack;

    // 私有字段，用于存储NPCMovement组件的引用
    private NPCMovement npcMovement;

    // 唤醒时调用，用于获取NPCMovement组件的引用并初始化移动步骤堆栈
    private void Awake()
    {
        npcMovement = GetComponent<NPCMovement>();
        npcMovementStepStack = new Stack<NPCMovementStep>();
    }

    // 清除路径方法，用于清空移动步骤堆栈
    public void ClearPath()
    {
        npcMovementStepStack.Clear();
    }

    // 构建路径方法，用于根据日程事件构建NPC的移动路径
    public void BuildPath(NPCScheduleEvent npcScheduleEvent)
    {
        ClearPath();

        // 如果日程事件是当前NPC场景中的位置
        if (npcScheduleEvent.toSceneName == npcMovement.npcCurrentScene)
        {
            Vector2Int npcCurrentGridPosition = (Vector2Int)npcMovement.npcCurrentGridPosition;

            Vector2Int npcTargetGridPosition = (Vector2Int)npcScheduleEvent.toGridCoordinate;

            // 构建路径并将移动步骤添加到移动步骤堆栈
            NPCManager.Instance.BuildPath(npcScheduleEvent.toSceneName, npcCurrentGridPosition, npcTargetGridPosition, npcMovementStepStack);
        }
        // 否则，如果日程事件是另一个场景中的位置
        else if (npcScheduleEvent.toSceneName != npcMovement.npcCurrentScene)
        {
            SceneRoute sceneRoute;

            // 获取匹配日程的场景路径
            sceneRoute = NPCManager.Instance.GetSceneRoute(npcMovement.npcCurrentScene.ToString(), npcScheduleEvent.toSceneName.ToString());

            // 是否找到了有效的场景路径？
            if (sceneRoute != null)
            {
                // 反向遍历场景路径

                for (int i = sceneRoute.scenePathList.Count - 1; i >= 0; i--)
                {
                    int toGridX, toGridY, fromGridX, fromGridY;

                    ScenePath scenePath = sceneRoute.scenePathList[i];

                    // 检查这是否是最终目的地
                    if (scenePath.toGridCell.x >= Settings.maxGridWidth || scenePath.toGridCell.y >= Settings.maxGridHeight)
                    {
                        // 如果是，则使用最终目的地的网格单元
                        toGridX = npcScheduleEvent.toGridCoordinate.x;
                        toGridY = npcScheduleEvent.toGridCoordinate.y;
                    }
                    else
                    {
                        // 否则，使用场景路径的位置
                        toGridX = scenePath.toGridCell.x;
                        toGridY = scenePath.toGridCell.y;
                    }

                    // 检查这是否是起始位置
                    if (scenePath.fromGridCell.x >= Settings.maxGridWidth || scenePath.fromGridCell.y >= Settings.maxGridHeight)
                    {
                        // 如果是，则使用NPC的位置
                        fromGridX = npcMovement.npcCurrentGridPosition.x;
                        fromGridY = npcMovement.npcCurrentGridPosition.y;
                    }
                    else
                    {
                        // 否则，使用场景路径的起始位置
                        fromGridX = scenePath.fromGridCell.x;
                        fromGridY = scenePath.fromGridCell.y;
                    }

                    Vector2Int fromGridPosition = new Vector2Int(fromGridX, fromGridY);

                    Vector2Int toGridPosition = new Vector2Int(toGridX, toGridY);

                    // 构建路径并将移动步骤添加到移动步骤堆栈
                    NPCManager.Instance.BuildPath(scenePath.sceneName, fromGridPosition, toGridPosition, npcMovementStepStack);
                }
            }
        }

        // 如果堆栈计数大于1，更新时间和路径上的第一步（起始位置）
        if (npcMovementStepStack.Count > 1)
        {
            UpdateTimesOnPath();
            npcMovementStepStack.Pop(); // 丢弃起始步骤

            // 在NPC移动中设置日程事件详情
            npcMovement.SetScheduleEventDetails(npcScheduleEvent);
        }
    }

    /// <summary>
    /// 使用预期的游戏时间更新路径移动步骤
    /// </summary>
    public void UpdateTimesOnPath()
    {
        // 获取当前游戏时间
        TimeSpan currentGameTime = TimeManager.Instance.GetGameTime();

        NPCMovementStep previousNPCMovementStep = null;

        foreach (NPCMovementStep npcMovementStep in npcMovementStepStack)
        {
            if (previousNPCMovementStep == null)
                previousNPCMovementStep = npcMovementStep;

            npcMovementStep.hour = currentGameTime.Hours;
            npcMovementStep.minute = currentGameTime.Minutes;
            npcMovementStep.second = currentGameTime.Seconds;

            TimeSpan movementTimeStep;

            // 如果是对角线移动
            if (MovementIsDiagonal(npcMovementStep, previousNPCMovementStep))
            {
                movementTimeStep = new TimeSpan(0, 0, (int)(Settings.gridCellDiagonalSize / Settings.secondsPerGameSecond / npcMovement.npcNormalSpeed));
            }
            else
            {
                movementTimeStep = new TimeSpan(0, 0, (int)(Settings.gridCellSize / Settings.secondsPerGameSecond / npcMovement.npcNormalSpeed));
            }

            currentGameTime = currentGameTime.Add(movementTimeStep);

            previousNPCMovementStep = npcMovementStep;
        }
    }

    /// <summary>
    /// 如果前一个移动步骤是对角线移动，则返回true，否则返回false
    /// </summary>
    private bool MovementIsDiagonal(NPCMovementStep npcMovementStep, NPCMovementStep previousNPCMovementStep)
    {
        if ((npcMovementStep.gridCoordinate.x != previousNPCMovementStep.gridCoordinate.x) && (npcMovementStep.gridCoordinate.y != previousNPCMovementStep.gridCoordinate.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/*类定义和组件依赖：

NPCPath 类继承自 MonoBehaviour，用于管理NPC的路径和移动步骤。
使用 RequireComponent 属性确保该脚本依附的游戏对象包含 NPCMovement 组件。
字段：

npcMovementStepStack：一个 Stack<NPCMovementStep> 类型的公开字段，用于存储NPC的移动步骤堆栈。
npcMovement：一个 NPCMovement 类型的私有字段，用于存储 NPCMovement 组件的引用。
方法：

Awake：在对象唤醒时调用，用于获取 NPCMovement 组件的引用并初始化移动步骤堆栈。
ClearPath：清空移动步骤堆栈。
BuildPath：根据 NPCScheduleEvent 构建NPC的移动路径，并将移动步骤添加到堆栈中。
UpdateTimesOnPath：更新路径上每个移动步骤的预期游戏时间。
MovementIsDiagonal：判断两个连续的移动步骤是否是对角线移动。
路径构建逻辑：

BuildPath 方法首先清空路径，然后根据日程事件是否在同一场景中，调用 NPCManager 的 BuildPath 方法构建路径。
如果日程事件在另一个场景中，需要通过场景路径找到从当前位置到目标位置的路径，并构建路径。
如果路径堆栈中有多个步骤，更新路径上的时间并丢弃起始步骤。
时间更新逻辑：

UpdateTimesOnPath 方法根据当前游戏时间和NPC的移动速度，计算每个移动步骤的预期时间，并更新步骤的时间。
对角线移动判断：

MovementIsDiagonal 方法用于判断两个连续的移动步骤是否是对角线移动，这对于计算移动时间是必要的。
这个类的设计目的是为了管理NPC的移动路径和时间，确保NPC能够按照预定的时间表和路径移动到指定的位置。*/
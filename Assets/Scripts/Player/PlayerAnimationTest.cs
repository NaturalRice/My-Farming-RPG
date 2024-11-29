// 使用Unity引擎的命名空间
using UnityEngine;

// 定义一个名为PlayerAnimationTest的类，它继承自MonoBehaviour
public class PlayerAnimationTest : MonoBehaviour
{
    // 以下为公开变量，可以在Unity编辑器中设置它们的值
    public float inputX; // 水平输入值，可能用于控制角色移动
    public float inputY; // 垂直输入值，可能用于控制角色移动
    public bool isWalking; // 角色是否正在行走
    public bool isRunning; // 角色是否正在跑步
    public bool isIdle; // 角色是否处于空闲状态
    public bool isCarrying; // 角色是否正在携带物品
    public ToolEffect toolEffect; // 工具效果枚举，可能用于定义不同工具的行为
    public bool isUsingToolRight; // 角色是否正在向右使用工具
    public bool isUsingToolLeft; // 角色是否正在向左使用工具
    public bool isUsingToolUp; // 角色是否正在向上使用工具
    public bool isUsingToolDown; // 角色是否正在向下使用工具
    public bool isLiftingToolRight; // 角色是否正在向右举起工具
    public bool isLiftingToolLeft; // 角色是否正在向左举起工具
    public bool isLiftingToolUp; // 角色是否正在向上举起工具
    public bool isLiftingToolDown; // 角色是否正在向下举起工具
    public bool isPickingRight; // 角色是否正在向右捡起物品
    public bool isPickingLeft; // 角色是否正在向左捡起物品
    public bool isPickingUp; // 角色是否正在向上捡起物品
    public bool isPickingDown; // 角色是否正在向下捡起物品
    public bool isSwingingToolRight; // 角色是否正在向右挥动工具
    public bool isSwingingToolLeft; // 角色是否正在向左挥动工具
    public bool isSwingingToolUp; // 角色是否正在向上挥动工具
    public bool isSwingingToolDown; // 角色是否正在向下挥动工具
    public bool idleUp; // 角色是否处于向上的空闲状态
    public bool idleDown; // 角色是否处于向下的空闲状态
    public bool idleLeft; // 角色是否处于向左的空闲状态
    public bool idleRight; // 角色是否处于向右的空闲状态

    // Update方法在每一帧调用一次，用于更新游戏对象的状态
    private void Update()
    {
        // 调用EventHandler的CallMovementEvent方法，传递角色的移动和动作状态
        EventHandler.CallMovementEvent(inputX, inputY, isWalking, isRunning, isIdle, isCarrying, toolEffect,
        isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
        isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
        isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
        isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
        idleUp, idleDown, idleLeft, idleRight);
    }
}

/*这段代码的主要功能是将角色的输入和状态传递给一个名为EventHandler的类（这个类在代码中没有给出，可能是另一个脚本），
 * 以便根据这些输入和状态来控制角色的动画。Update方法在游戏的每一帧都会执行，这意味着角色的状态会不断地更新，以响
 * 应玩家的输入和游戏世界的变化。EventHandler.CallMovementEvent方法可能是一个自定义方法，用于处理角色的移动和动作事件，并触发相应的动画。*/
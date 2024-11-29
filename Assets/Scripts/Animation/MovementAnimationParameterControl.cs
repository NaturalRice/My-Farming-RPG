using UnityEngine; // 引入Unity引擎的命名空间

public class MovementAnimationParameterControl : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    private Animator animator; // 私有字段，用于存储Animator组件

    // 初始化时调用
    private void Awake() // Awake方法在对象被创建时调用
    {
        animator = GetComponent<Animator>(); // 获取当前GameObject上的Animator组件
    }

    private void OnEnable() // 当这个组件被启用时调用
    {
        EventHandler.MovementEvent += SetAnimationParameters; // 订阅MovementEvent事件，并添加事件处理函数SetAnimationParameters
    }

    private void OnDisable() // 当这个组件被禁用时调用
    {
        EventHandler.MovementEvent -= SetAnimationParameters; // 取消订阅MovementEvent事件，并移除事件处理函数SetAnimationParameters
    }

    private void SetAnimationParameters(float xInput, float yInput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
        bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
        bool idleUp, bool idleDown, bool idleLeft, bool idleRight) // 设置动画参数的方法
    {
        animator.SetFloat(Settings.xInput, xInput); // 设置水平输入的浮点数值
        animator.SetFloat(Settings.yInput, yInput); // 设置垂直输入的浮点数值
        animator.SetBool(Settings.isWalking, isWalking); // 设置是否正在行走的布尔值
        animator.SetBool(Settings.isRunning, isRunning); // 设置是否正在跑步的布尔值

        animator.SetInteger(Settings.toolEffect, (int)toolEffect); // 设置工具效果的整数值

        // 以下代码设置各种工具使用状态的触发器
        if (isUsingToolRight)
            animator.SetTrigger(Settings.isUsingToolRight); // 设置是否正在右
        if (isUsingToolLeft)
            animator.SetTrigger(Settings.isUsingToolLeft);// ... 省略其他工具使用状态的设置，与上面类似 ...
        if (isUsingToolUp)
            animator.SetTrigger(Settings.isUsingToolUp);
        if (isUsingToolDown)
            animator.SetTrigger(Settings.isUsingToolDown);

        if (isLiftingToolRight)
            animator.SetTrigger(Settings.isLiftingToolRight);
        if (isLiftingToolLeft)
            animator.SetTrigger(Settings.isLiftingToolLeft);
        if (isLiftingToolUp)
            animator.SetTrigger(Settings.isLiftingToolUp);
        if (isLiftingToolDown)
            animator.SetTrigger(Settings.isLiftingToolDown);

        if (isSwingingToolRight)
            animator.SetTrigger(Settings.isSwingingToolRight);
        if (isSwingingToolLeft)
            animator.SetTrigger(Settings.isSwingingToolLeft);
        if (isSwingingToolUp)
            animator.SetTrigger(Settings.isSwingingToolUp);
        if (isSwingingToolDown)
            animator.SetTrigger(Settings.isSwingingToolDown);

        if (isPickingRight)
            animator.SetTrigger(Settings.isPickingRight);
        if (isPickingLeft)
            animator.SetTrigger(Settings.isPickingLeft);
        if (isPickingUp)
            animator.SetTrigger(Settings.isPickingUp);
        if (isPickingDown)
            animator.SetTrigger(Settings.isPickingDown);
        // 以下代码设置各种闲置状态的触发器
        if (idleUp)
            animator.SetTrigger(Settings.idleUp);// 设置向上闲置的触发器
        if (idleDown)
            animator.SetTrigger(Settings.idleDown);
        if (idleLeft)
            animator.SetTrigger(Settings.idleLeft);
        if (idleRight)
            animator.SetTrigger(Settings.idleRight);
    }

    private void AnimationEventPlayFootstepSound()// 在动画事件中播放脚步声的方法
    {
        AudioManager.Instance.PlaySound(SoundName.effectFootstepHardGround);// 播放硬地面的脚步声
    }
}

/*MovementAnimationParameterControl 类：这个类用于控制角色的动画参数，它继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

animator 字段：用于存储角色的Animator组件，这样脚本就可以控制角色的动画状态。

Awake 方法：在对象被创建时调用，用于获取并存储当前GameObject上的Animator组件。

OnEnable 和 OnDisable 方法：这两个方法用于订阅和取消订阅EventHandler.MovementEvent事件。当组件被启用或禁用时，分别添加或移除SetAnimationParameters方法作为事件处理函数。

SetAnimationParameters 方法：这个方法作为事件处理函数，用于根据输入参数设置Animator组件的各种参数，包括移动输入、行走、跑步、工具使用状态等。这些参数控制角色的动画状态和行为。

AnimationEventPlayFootstepSound 方法：这个方法在动画的特定事件时被调用，用于播放脚步声效果。它使用AudioManager单例来播放指定的声音。

这个脚本通常用于游戏中角色的动画控制，可以根据角色的移动和动作状态来动态设置动画参数，以及在特定动画事件时播放声音，增强游戏的沉浸感和真实感。*/
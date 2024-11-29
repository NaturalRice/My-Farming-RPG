// 引用Unity引擎和系统的基本命名空间
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 这个类需要依附于包含Rigidbody2D、Animator、NPCPath、SpriteRenderer和BoxCollider2D组件的游戏对象
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NPCPath))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class NPCMovement : MonoBehaviour
{
    // 定义NPC当前和目标场景、网格位置和世界位置
    public SceneName npcCurrentScene;
    [HideInInspector] public SceneName npcTargetScene;
    [HideInInspector] public Vector3Int npcCurrentGridPosition;
    [HideInInspector] public Vector3Int npcTargetGridPosition;
    [HideInInspector] public Vector3 npcTargetWorldPosition;
    public Direction npcFacingDirectionAtDestination;

    // 私有变量，用于存储NPC的前一个移动场景步骤、下一个网格位置、下一个世界位置
    private SceneName npcPreviousMovementStepScene;
    private Vector3Int npcNextGridPosition;
    private Vector3 npcNextWorldPosition;

    // NPC移动的标题
    [Header("NPC Movement")]
    public float npcNormalSpeed = 2f;

    // 序列化字段，用于存储NPC的最小和最大速度
    [SerializeField] private float npcMinSpeed = 1f;
    [SerializeField] private float npcMaxSpeed = 3f;
    private bool npcIsMoving = false;

    // 用于存储NPC目标动画剪辑
    [HideInInspector] public AnimationClip npcTargetAnimationClip;

    // NPC动画的标题
    [Header("NPC Animation")]
    [SerializeField] private AnimationClip blankAnimation = null;

    // 私有变量，用于存储网格、刚体2D、碰撞器2D、等待固定更新、动画器、动画覆盖控制器、最后移动动画参数、NPC路径和精灵渲染器
    private Grid grid;
    private Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider2D;
    private WaitForFixedUpdate waitForFixedUpdate;
    private Animator animator;
    private AnimatorOverrideController animatorOverrideController;
    private int lastMoveAnimationParameter;
    private NPCPath npcPath;
    private bool npcInitialised = false;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool npcActiveInScene = false;

    // 私有变量，用于标记场景是否已加载
    private bool sceneLoaded = false;

    // 移动到网格位置的协程
    private Coroutine moveToGridPositionRoutine;

    // 当启用时，注册场景加载和卸载事件
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloaded;
    }

    // 当禁用时，注销场景加载和卸载事件
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloaded;
    }

    // 唤醒时，获取组件并初始化目标世界位置、网格位置和场景为当前值
    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        npcPath = GetComponent<NPCPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        // Initialise target world position, target grid position & target scene to current
        npcTargetScene = npcCurrentScene;
        npcTargetGridPosition = npcCurrentGridPosition;
        npcTargetWorldPosition = transform.position;
    }

    // 在第一帧更新之前调用
    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        SetIdleAnimation();
    }

    // 固定更新时调用，处理NPC移动逻辑
    private void FixedUpdate()
    {
        if (sceneLoaded)
        {
            if (npcIsMoving == false)
            {
                // set npc current and next grid position - to take into account the npc might be animating
                npcCurrentGridPosition = GetGridPosition(transform.position);
                npcNextGridPosition = npcCurrentGridPosition;

                if (npcPath.npcMovementStepStack.Count > 0)
                {
                    NPCMovementStep npcMovementStep = npcPath.npcMovementStepStack.Peek();

                    npcCurrentScene = npcMovementStep.sceneName;

                    // 如果NPC即将移动到新场景，重置位置到新场景的起点，并更新路径步骤时间
                    if (npcCurrentScene != npcPreviousMovementStepScene)
                    {
                        npcCurrentGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;
                        npcNextGridPosition = npcCurrentGridPosition;
                        transform.position = GetWorldPosition(npcCurrentGridPosition);
                        npcPreviousMovementStepScene = npcCurrentScene;
                        npcPath.UpdateTimesOnPath();
                    }

                    // 如果NPC在当前场景，则设置NPC为活动状态以使其可见，弹出路径步骤并调用移动NPC的方法
                    if (npcCurrentScene.ToString() == SceneManager.GetActiveScene().name)
                    {
                        SetNPCActiveInScene();

                        npcMovementStep = npcPath.npcMovementStepStack.Pop();

                        npcNextGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;

                        TimeSpan npcMovementStepTime = new TimeSpan(npcMovementStep.hour, npcMovementStep.minute, npcMovementStep.second);

                        MoveToGridPosition(npcNextGridPosition, npcMovementStepTime, TimeManager.Instance.GetGameTime());
                    }

                    // 否则如果NPC不在当前场景，则设置NPC为非活动状态以使其不可见
                    // - 一旦移动步骤时间小于游戏时间（在过去），则从堆栈中弹出移动步骤并将NPC位置设置为移动步骤位置
                    else
                    {
                        SetNPCInactiveInScene();

                        npcCurrentGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;
                        npcNextGridPosition = npcCurrentGridPosition;
                        transform.position = GetWorldPosition(npcCurrentGridPosition);

                        TimeSpan npcMovementStepTime = new TimeSpan(npcMovementStep.hour, npcMovementStep.minute, npcMovementStep.second);

                        TimeSpan gameTime = TimeManager.Instance.GetGameTime();

                        if (npcMovementStepTime < gameTime)
                        {
                            npcMovementStep = npcPath.npcMovementStepStack.Pop();

                            npcCurrentGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;
                            npcNextGridPosition = npcCurrentGridPosition;
                            transform.position = GetWorldPosition(npcCurrentGridPosition);
                        }
                    }

                }
                // 否则如果没有更多的NPC移动步骤
                else
                {
                    ResetMoveAnimation();

                    SetNPCFacingDirection();

                    SetNPCEventAnimation();
                }
            }
        }
    }

    // 设置NPC的日程事件详情
    public void SetScheduleEventDetails(NPCScheduleEvent npcScheduleEvent)
    {
        npcTargetScene = npcScheduleEvent.toSceneName;
        npcTargetGridPosition = (Vector3Int)npcScheduleEvent.toGridCoordinate;
        npcTargetWorldPosition = GetWorldPosition(npcTargetGridPosition);
        npcFacingDirectionAtDestination = npcScheduleEvent.npcFacingDirectionAtDestination;
        npcTargetAnimationClip = npcScheduleEvent.animationAtDestination;
        ClearNPCEventAnimation();
    }

    // 设置NPC事件动画
    private void SetNPCEventAnimation()
    {
        if (npcTargetAnimationClip != null)
        {
            ResetIdleAnimation();
            animatorOverrideController[blankAnimation] = npcTargetAnimationClip;
            animator.SetBool(Settings.eventAnimation, true);
        }
        else
        {
            animatorOverrideController[blankAnimation] = blankAnimation;
            animator.SetBool(Settings.eventAnimation, false);
        }
    }

    // 清除NPC事件动画
    public void ClearNPCEventAnimation()
    {
        animatorOverrideController[blankAnimation] = blankAnimation;
        animator.SetBool(Settings.eventAnimation, false);

        // 清除NPC上的任何旋转
        transform.rotation = Quaternion.identity;
    }

    // 设置NPC面向方向
    private void SetNPCFacingDirection()
    {
        ResetIdleAnimation();

        switch (npcFacingDirectionAtDestination)
        {
            case Direction.up:
                animator.SetBool(Settings.idleUp, true);
                break;

            case Direction.down:
                animator.SetBool(Settings.idleDown, true);
                break;

            case Direction.left:
                animator.SetBool(Settings.idleLeft, true);
                break;

            case Direction.right:
                animator.SetBool(Settings.idleRight, true);
                break;

            case Direction.none:
                break;

            default:
                break;
        }
    }

    // 设置NPC在场景中活动
    public void SetNPCActiveInScene()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        npcActiveInScene = true;
    }

    // 设置NPC在场景中非活动
    public void SetNPCInactiveInScene()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        npcActiveInScene = false;
    }

    // 场景加载后调用
    private void AfterSceneLoad()
    {
        grid = GameObject.FindObjectOfType<Grid>();

        if (!npcInitialised)
        {
            InitialiseNPC();
            npcInitialised = true;
        }

        sceneLoaded = true;
    }

    // 场景卸载前调用
    private void BeforeSceneUnloaded()
    {
        sceneLoaded = false;
    }

    /// <summary>
    /// 根据世界位置返回网格位置
    /// </summary>
    private Vector3Int GetGridPosition(Vector3 worldPosition)
    {
        if (grid != null)
        {
            return grid.WorldToCell(worldPosition);
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    /// <summary>
    /// 根据网格位置返回世界位置（网格中心）
    /// </summary>
    public Vector3 GetWorldPosition(Vector3Int gridPosition)
    {
        Vector3 worldPosition = grid.CellToWorld(gridPosition);

        // 获取网格中心
        return new Vector3(worldPosition.x + Settings.gridCellSize / 2f, worldPosition.y + Settings.gridCellSize / 2f, worldPosition.z);
    }

    // 取消NPC移动
    public void CancelNPCMovement()
    {
        npcPath.ClearPath();
        npcNextGridPosition = Vector3Int.zero;
        npcNextWorldPosition = Vector3.zero;
        npcIsMoving = false;

        if (moveToGridPositionRoutine != null)
        {
            StopCoroutine(moveToGridPositionRoutine);
        }

        // 重置移动动画
        ResetMoveAnimation();

        // 清除事件动画
        ClearNPCEventAnimation();
        npcTargetAnimationClip = null;

        // 重置空闲动画
        ResetIdleAnimation();

        // 设置空闲动画
        SetIdleAnimation();
    }

    // 初始化NPC
    private void InitialiseNPC()
    {
        // 如果NPC在当前场景中，则设置为活动状态
        if (npcCurrentScene.ToString() == SceneManager.GetActiveScene().name)
        {
            SetNPCActiveInScene();
        }
        else
        {
            SetNPCInactiveInScene();
        }

        npcPreviousMovementStepScene = npcCurrentScene;

        // 获取NPC当前网格位置
        npcCurrentGridPosition = GetGridPosition(transform.position);

        // 设置下一个网格位置和目标网格位置为当前网格位置
        npcNextGridPosition = npcCurrentGridPosition;
        npcTargetGridPosition = npcCurrentGridPosition;
        npcTargetWorldPosition = GetWorldPosition(npcTargetGridPosition);

        // 获取NPC世界位置
        npcNextWorldPosition = GetWorldPosition(npcCurrentGridPosition);
    }

    // 移动到网格位置
    private void MoveToGridPosition(Vector3Int gridPosition, TimeSpan npcMovementStepTime, TimeSpan gameTime)
    {
        moveToGridPositionRoutine = StartCoroutine(MoveToGridPositionRoutine(gridPosition, npcMovementStepTime, gameTime));
    }

    // 移动到网格位置的协程
    private IEnumerator MoveToGridPositionRoutine(Vector3Int gridPosition, TimeSpan npcMovementStepTime, TimeSpan gameTime)
    {
        npcIsMoving = true;

        SetMoveAnimation(gridPosition);

        npcNextWorldPosition = GetWorldPosition(gridPosition);

        // 如果移动步骤时间在未来，则计算时间差并移动NPC
        if (npcMovementStepTime > gameTime)
        {
            // 计算时间差（秒）
            float timeToMove = (float)(npcMovementStepTime.TotalSeconds - gameTime.TotalSeconds);

            // 计算速度
            float npcCalculatedSpeed = Mathf.Max(npcMinSpeed, Vector3.Distance(transform.position, npcNextWorldPosition) / timeToMove / Settings.secondsPerGameSecond);

            // 如果速度至少为NPC最小速度且小于NPC最大速度，则处理，否则立即移动NPC到位置
            if (npcCalculatedSpeed <= npcMaxSpeed)
            {
                while (Vector3.Distance(transform.position, npcNextWorldPosition) > Settings.pixelSize)
                {
                    Vector3 unitVector = Vector3.Normalize(npcNextWorldPosition - transform.position);
                    Vector2 move = new Vector2(unitVector.x * npcCalculatedSpeed * Time.fixedDeltaTime, unitVector.y * npcCalculatedSpeed * Time.fixedDeltaTime);

                    rigidBody2D.MovePosition(rigidBody2D.position + move);

                    yield return waitForFixedUpdate;
                }
            }
        }

        rigidBody2D.position = npcNextWorldPosition;
        npcCurrentGridPosition = gridPosition;
        npcNextGridPosition = npcCurrentGridPosition;
        npcIsMoving = false;
    }

    // 设置移动动画
    private void SetMoveAnimation(Vector3Int gridPosition)
    {
        // 重置空闲动画
        ResetIdleAnimation();

        // 重置移动动画
        ResetMoveAnimation();

        // 获取世界位置
        Vector3 toWorldPosition = GetWorldPosition(gridPosition);

        // 获取方向向量
        Vector3 directionVector = toWorldPosition - transform.position;

        if (Mathf.Abs(directionVector.x) >= Mathf.Abs(directionVector.y))
        {
            // 使用左右动画
            if (directionVector.x > 0)
            {
                animator.SetBool(Settings.walkRight, true);
            }
            else
            {
                animator.SetBool(Settings.walkLeft, true);
            }
        }
        else
        {
            // 使用上下动画
            if (directionVector.y > 0)
            {
                animator.SetBool(Settings.walkUp, true);
            }
            else
            {
                animator.SetBool(Settings.walkDown, true);
            }
        }
    }

    // 设置空闲动画
    private void SetIdleAnimation()
    {
        animator.SetBool(Settings.idleDown, true);
    }

    // 重置移动动画
    private void ResetMoveAnimation()
    {
        animator.SetBool(Settings.walkRight, false);
        animator.SetBool(Settings.walkLeft, false);
        animator.SetBool(Settings.walkUp, false);
        animator.SetBool(Settings.walkDown, false);
    }

    // 重置空闲动画
    private void ResetIdleAnimation()
    {
        animator.SetBool(Settings.idleRight, false);
        animator.SetBool(Settings.idleLeft, false);
        animator.SetBool(Settings.idleUp, false);
        animator.SetBool(Settings.idleDown, false);
    }
}

/*1. **类定义和组件依赖**：
   - `NPCMovement` 类继承自 `MonoBehaviour`，用于控制NPC的移动。
   - 使用 `RequireComponent` 属性确保该脚本依附的游戏对象包含必要的组件，如 `Rigidbody2D`、`Animator`、`NPCPath`、`SpriteRenderer` 和 `BoxCollider2D`。

2. **字段和属性**：
   - 定义了NPC的当前和目标场景、网格位置、世界位置以及面向方向。
   - 包含NPC移动速度、动画剪辑等属性。

3. **事件处理**：
   - `OnEnable` 和 `OnDisable` 方法用于注册和注销场景加载和卸载事件。
   - `Awake` 方法用于获取组件并初始化目标位置。
   - `Start` 方法用于设置初始空闲动画。

4. **固定更新处理**：
   - `FixedUpdate` 方法用于处理NPC的移动逻辑，包括检查是否在当前场景、处理路径步骤、移动到目标位置等。

5. **动画和活动状态**：
   - `SetNPCEventAnimation` 和 `ClearNPCEventAnimation` 方法用于设置和清除NPC的事件动画。
   - `SetNPCFacingDirection` 方法用于设置NPC的面向方向。
   - `SetNPCActiveInScene` 和 `SetNPCInactiveInScene` 方法用于设置NPC在场景中的活动状态。

6. **场景加载和卸载**：
   - `AfterSceneLoad` 和 `BeforeSceneUnloaded` 方法用于处理场景加载和卸载时的逻辑。

7. **网格和世界位置转换**：
   - `GetGridPosition` 和 `GetWorldPosition` 方法用于在世界位置和网格位置之间转换。

8. **移动和动画设置**：
   - `MoveToGridPosition` 和 `MoveToGridPositionRoutine` 方法用于将NPC移动到指定的网格位置。
   - `SetMoveAnimation` 和 `SetIdleAnimation` 方法用于设置NPC的移动和空闲动画。

这段代码提供了一个完整的NPC移动和动画控制框架，包括场景间移动、动画播放和面向方向控制。*/
// 使用System.Collections.Generic命名空间
using System.Collections.Generic;
// 使用UnityEngine命名空间
using UnityEngine;
// 使用UnityEngine.SceneManagement命名空间
using UnityEngine.SceneManagement;

// 这个类需要AStar组件
[RequireComponent(typeof(AStar))]
public class NPCManager : SingletonMonobehaviour<NPCManager>
{
    // 可以被序列化的ScriptableObject，用于存储场景间路线
    [SerializeField] private SO_SceneRouteList so_SceneRouteList = null;
    // 存储场景间路线的字典
    private Dictionary<string, SceneRoute> sceneRouteDictionary;

    // 隐藏在Inspector中的NPC数组
    [HideInInspector]
    public NPC[] npcArray;

    // AStar组件，用于路径规划
    private AStar aStar;

    // 重写Awake方法
    protected override void Awake()
    {
        base.Awake();

        // 创建场景路线字典
        sceneRouteDictionary = new Dictionary<string, SceneRoute>();

        if (so_SceneRouteList.sceneRouteList.Count > 0)
        {
            foreach (SceneRoute so_sceneRoute in so_SceneRouteList.sceneRouteList)
            {
                // 检查字典中是否有重复的路线
                if (sceneRouteDictionary.ContainsKey(so_sceneRoute.fromSceneName.ToString() + so_sceneRoute.toSceneName.ToString()))
                {
                    Debug.Log("** Duplicate Scene Route Key Found ** Check for duplicate routes in the scriptable object scene route list");
                    continue;
                }

                // 将路线添加到字典
                sceneRouteDictionary.Add(so_sceneRoute.fromSceneName.ToString() + so_sceneRoute.toSceneName.ToString(), so_sceneRoute);
            }
        }

        aStar = GetComponent<AStar>();

        // 获取场景中的NPC游戏对象
        npcArray = FindObjectsOfType<NPC>();
    }

    // 当对象启用时调用
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    // 当对象禁用时调用
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    // 场景加载后的回调方法
    private void AfterSceneLoad()
    {
        // 设置NPC的活动状态
        SetNPCsActiveStatus();
    }

    // 设置NPC的活动状态
    private void SetNPCsActiveStatus()
    {
        foreach (NPC npc in npcArray)
        {
            NPCMovement npcMovement = npc.GetComponent<NPCMovement>();

            if (npcMovement.npcCurrentScene.ToString() == SceneManager.GetActiveScene().name)
            {
                npcMovement.SetNPCActiveInScene();
            }
            else
            {
                npcMovement.SetNPCInactiveInScene();
            }
        }
    }

    // 根据起始和目标场景名称获取场景间路线
    public SceneRoute GetSceneRoute(string fromSceneName, string toSceneName)
    {
        SceneRoute sceneRoute;

        // 从字典中获取场景路线
        if (sceneRouteDictionary.TryGetValue(fromSceneName + toSceneName, out sceneRoute))
        {
            return sceneRoute;
        }
        else
        {
            return null;
        }
    }

    // 构建从起始网格位置到目标网格位置的路径
    public bool BuildPath(SceneName sceneName, Vector2Int startGridPosition, Vector2Int endGridPosition, Stack<NPCMovementStep> npcMovementStepStack)
    {
        if (aStar.BuildPath(sceneName, startGridPosition, endGridPosition, npcMovementStepStack))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/*命名空间引用：

using System.Collections.Generic;：引用了 System.Collections.Generic 命名空间，提供了泛型集合类，如字典。
using UnityEngine;：引用了 UnityEngine 命名空间，提供了 Unity 引擎的基本功能。
using UnityEngine.SceneManagement;：引用了 UnityEngine.SceneManagement 命名空间，提供了场景管理的功能。
类定义：

[RequireComponent(typeof(AStar))]：这个属性要求附加此脚本的游戏对象必须同时拥有 AStar 组件。
public class NPCManager : SingletonMonobehaviour<NPCManager>：定义了一个公共类 NPCManager，它继承自 SingletonMonobehaviour，确保只有一个实例存在。
成员变量：

[SerializeField] private SO_SceneRouteList so_SceneRouteList = null;：一个可以被序列化的私有变量，用于存储场景间路线的ScriptableObject。
private Dictionary<string, SceneRoute> sceneRouteDictionary;：一个私有字典，用于存储场景间路线。
[HideInInspector] public NPC[] npcArray;：一个公共数组，用于存储场景中的NPC对象，Inspector中不显示。
private AStar aStar;：一个私有变量，用于存储AStar组件。
Awake方法：

protected override void Awake()：重写Awake方法，在对象被创建时调用，初始化场景路线字典和AStar组件，获取场景中的NPC对象。
OnEnable和OnDisable方法：

private void OnEnable()：当对象启用时调用，注册场景加载后的事件。
private void OnDisable()：当对象禁用时调用，取消注册场景加载后的事件。
AfterSceneLoad方法：

private void AfterSceneLoad()：场景加载后的回调方法，设置NPC的活动状态。
SetNPCsActiveStatus方法：

private void SetNPCsActiveStatus()：设置NPC的活动状态，根据当前场景和NPC的目标场景启用或禁用NPC。
GetSceneRoute方法：

public SceneRoute GetSceneRoute(string fromSceneName, string toSceneName)：根据起始和目标场景名称获取场景间路线。
BuildPath方法：

public bool BuildPath(SceneName sceneName, Vector2Int startGridPosition, Vector2Int endGridPosition, Stack<NPCMovementStep> npcMovementStepStack)：构建从起始网格位置到目标网格位置的路径，使用AStar算法。
总的来说，NPCManager 类提供了NPC的场景间移动和路径规划的管理功能，通过AStar算法实现路径规划，并管理NPC的活动状态。*/
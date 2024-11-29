// 使用System命名空间
using System;
// 使用System.Collections.Generic命名空间
using System.Collections.Generic;
// 使用UnityEngine命名空间
using UnityEngine;

// 这个类需要NPCMovement组件和GenerateGUID组件
[RequireComponent(typeof(NPCMovement))]
[RequireComponent(typeof(GenerateGUID))]
public class NPC : MonoBehaviour, ISaveable
{
    // 私有字符串，用于保存ISaveable接口的唯一ID
    private string _iSaveableUniqueID;
    // ISaveable接口的属性，用于获取和设置唯一ID
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    // 私有GameObjectSave对象，用于保存游戏对象的状态
    private GameObjectSave _gameObjectSave;
    // 公共属性，用于获取和设置游戏对象的状态
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    // 私有NPCMovement组件，用于控制NPC的移动
    private NPCMovement npcMovement;

    // 当对象启用时调用
    private void OnEnable()
    {
        ISaveableRegister();
    }

    // 当对象禁用时调用
    private void OnDisable()
    {
        ISaveableDeregister();
    }
    // 当对象被创建时调用
    private void Awake()
    {
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    // 当对象开始时调用
    private void Start()
    {
        // 获取NPC运动组件
        npcMovement = GetComponent<NPCMovement>();
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    // ISaveable接口的方法，用于从保存数据中加载NPC状态
    public void ISaveableLoad(GameSave gameSave)
    {
        // 从保存数据中获取游戏对象状态
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // 从保存数据中获取场景状态
            if (GameObjectSave.sceneData.TryGetValue(Settings.PersistentScene, out SceneSave sceneSave))
            {
                // 如果字典不为空
                if (sceneSave.vector3Dictionary != null && sceneSave.stringDictionary != null)
                {
                    // 目标网格位置
                    if (sceneSave.vector3Dictionary.TryGetValue("npcTargetGridPosition", out Vector3Serializable savedNPCTargetGridPosition))
                    {
                        npcMovement.npcTargetGridPosition = new Vector3Int((int)savedNPCTargetGridPosition.x, (int)savedNPCTargetGridPosition.y, (int)savedNPCTargetGridPosition.z);
                        npcMovement.npcCurrentGridPosition = npcMovement.npcTargetGridPosition;
                    }

                    // 目标世界位置
                    if (sceneSave.vector3Dictionary.TryGetValue("npcTargetWorldPosition", out Vector3Serializable savedNPCTargetWorldPosition))
                    {
                        npcMovement.npcTargetWorldPosition = new Vector3(savedNPCTargetWorldPosition.x, savedNPCTargetWorldPosition.y, savedNPCTargetWorldPosition.z);
                        transform.position = npcMovement.npcTargetWorldPosition;
                    }

                    // 目标场景
                    if (sceneSave.stringDictionary.TryGetValue("npcTargetScene", out string savedTargetScene))
                    {
                        if (Enum.TryParse<SceneName>(savedTargetScene, out SceneName sceneName))
                        {
                            npcMovement.npcTargetScene = sceneName;
                            npcMovement.npcCurrentScene = npcMovement.npcTargetScene;
                        }
                    }

                    // 清除当前NPC移动
                    npcMovement.CancelNPCMovement();
                }
            }
        }
    }

    // ISaveable接口的方法，用于注册保存对象
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    // ISaveable接口的方法，用于恢复场景
    public void ISaveableRestoreScene(string sceneName)
    {
        // 在持久场景中不需要做任何事情
    }

    // ISaveable接口的方法，用于保存NPC状态
    public GameObjectSave ISaveableSave()
    {
        // 移除当前场景保存
        GameObjectSave.sceneData.Remove(Settings.PersistentScene);

        // 创建场景保存
        SceneSave sceneSave = new SceneSave();

        // 创建可序列化的Vector3字典
        sceneSave.vector3Dictionary = new Dictionary<string, Vector3Serializable>();

        // 创建字符串字典
        sceneSave.stringDictionary = new Dictionary<string, string>();

        // 存储目标网格位置、目标世界位置和目标场景
        sceneSave.vector3Dictionary.Add("npcTargetGridPosition", new Vector3Serializable(npcMovement.npcTargetGridPosition.x, npcMovement.npcTargetGridPosition.y, npcMovement.npcTargetGridPosition.z));
        sceneSave.vector3Dictionary.Add("npcTargetWorldPosition", new Vector3Serializable(npcMovement.npcTargetWorldPosition.x, npcMovement.npcTargetWorldPosition.y, npcMovement.npcTargetWorldPosition.z));
        sceneSave.stringDictionary.Add("npcTargetScene", npcMovement.npcTargetScene.ToString());

        // 将场景保存添加到游戏对象
        GameObjectSave.sceneData.Add(Settings.PersistentScene, sceneSave);

        return GameObjectSave;
    }

    // ISaveable接口的方法，用于存储场景
    public void ISaveableStoreScene(string sceneName)
    {
        // 在持久场景中不需要做任何事情
    }
}

/*命名空间引用：

using System;：引用了 System 命名空间，提供了基本的功能和数据类型。
using System.Collections.Generic;：引用了 System.Collections.Generic 命名空间，提供了泛型集合类。
using UnityEngine;：引用了 UnityEngine 命名空间，提供了 Unity 引擎的基本功能。
类定义：

[RequireComponent(typeof(NPCMovement))]：这个属性要求附加此脚本的游戏对象必须同时拥有 NPCMovement 组件。
[RequireComponent(typeof(GenerateGUID))]：这个属性要求附加此脚本的游戏对象必须同时拥有 GenerateGUID 组件。
public class NPC : MonoBehaviour, ISaveable：定义了一个公共类 NPC，它继承自 MonoBehaviour 并实现了 ISaveable 接口。
成员变量和属性：

private string _iSaveableUniqueID;：一个私有字符串，用于存储 ISaveable 接口的唯一ID。
public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }：一个公共属性，用于获取和设置唯一ID。
private GameObjectSave _gameObjectSave;：一个私有 GameObjectSave 对象，用于保存游戏对象的状态。
public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }：一个公共属性，用于获取和设置游戏对象的状态。
NPC运动组件：

private NPCMovement npcMovement;：一个私有 NPCMovement 组件，用于控制NPC的移动。
生命周期方法：

private void OnEnable()：当对象启用时调用，注册保存对象。
private void OnDisable()：当对象禁用时调用，取消注册保存对象。
private void Awake()：当对象被创建时调用，初始化唯一ID和游戏对象状态。
private void Start()：当对象开始时调用，获取NPC运动组件。
ISaveable接口方法：

public void ISaveableLoad(GameSave gameSave)：从保存数据中加载NPC状态。
public void ISaveableRegister()：注册保存对象。
public void ISaveableRestoreScene(string sceneName)：恢复场景，对于持久场景不需要做任何事情。
public GameObjectSave ISaveableSave()：保存NPC状态。
public void ISaveableStoreScene(string sceneName)：存储场景，对于持久场景不需要做任何事情。
总的来说，NPC 类提供了NPC的保存和加载逻辑，确保NPC的状态可以在游戏会话之间保持一致。通过实现 ISaveable 接口，NPC 类可以与游戏的保存和加载系统无缝集成。*/
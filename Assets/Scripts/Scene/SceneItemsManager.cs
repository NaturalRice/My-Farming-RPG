// 引用所需的命名空间
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 要求该类附加的GameObject上必须有GenerateGUID组件
[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : SingletonMonobehaviour<SceneItemsManager>, ISaveable
{
    // 私有变量，用于存储物品的父Transform
    private Transform parentItem;
    // 可序列化的私有变量，用于存储物品的预制体
    [SerializeField] private GameObject itemPrefab = null;

    // 私有字符串变量，用于存储ISaveable接口的唯一标识符
    private string _iSaveableUniqueID;
    // ISaveable接口的属性，用于获取和设置唯一标识符
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    // 私有GameObjectSave变量，用于存储游戏对象的保存数据
    private GameObjectSave _gameObjectSave;
    // ISaveable接口的属性，用于获取和设置游戏对象的保存数据
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    // 在场景加载后调用的方法
    private void AfterSceneLoad()
    {
        // 获取标记为Tags.ItemsParentTransform的GameObject的Transform组件
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }

    // 重写Awake方法，在游戏对象创建时初始化
    protected override void Awake()
    {
        base.Awake();

        // 设置ISaveable接口的唯一标识符为GenerateGUID组件生成的GUID
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    /// <summary>
    /// 销毁场景中当前的所有物品
    /// </summary>
    private void DestroySceneItems()
    {
        // 获取场景中所有的Item组件
        Item[] itemsInScene = GameObject.FindObjectsOfType<Item>();

        // 循环销毁所有物品
        for (int i = itemsInScene.Length - 1; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }

    // 实例化场景物品的方法
    public void InstantiateSceneItem(int itemCode, Vector3 itemPosition)
    {
        // 实例化物品预制体，并设置其位置
        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemCode);
    }

    // 从保存数据实例化场景物品的方法
    private void InstantiateSceneItems(List<SceneItem> sceneItemList)
    {
        GameObject itemGameObject;

        foreach (SceneItem sceneItem in sceneItemList)
        {
            // 实例化物品预制体，并设置其位置
            itemGameObject = Instantiate(itemPrefab, new Vector3(sceneItem.position.x, sceneItem.position.y, sceneItem.position.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = sceneItem.itemCode;
            item.name = sceneItem.itemName;
        }
    }

    // 当该组件被禁用时调用的方法
    private void OnDisable()
    {
        // 从保存管理器中注销
        ISaveableDeregister();
        // 移除AfterSceneLoad事件的监听
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    // 当该组件被启用时调用的方法
    private void OnEnable()
    {
        // 在保存管理器中注册
        ISaveableRegister();
        // 添加AfterSceneLoad事件的监听
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    // ISaveable接口的方法，用于注销
    public void ISaveableDeregister()
    {
        // 从保存管理器的列表中移除当前对象
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    // ISaveable接口的方法，用于加载保存数据
    public void ISaveableLoad(GameSave gameSave)
    {
        // 如果保存数据中包含当前对象的GUID，则加载对应的数据
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // 恢复当前场景的数据
            ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }



    // ISaveable接口的方法，用于恢复场景数据
    public void ISaveableRestoreScene(string sceneName)
    {
        // 如果保存数据中包含当前场景的数据，则恢复
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if (sceneSave.listSceneItem != null)
            {
                // 销毁场景中现有的物品
                DestroySceneItems();

                // 实例化场景物品列表
                InstantiateSceneItems(sceneSave.listSceneItem);
            }
        }
    }

    // ISaveable接口的方法，用于注册
    public void ISaveableRegister()
    {
        // 将当前对象添加到保存管理器的列表中
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    // ISaveable接口的方法，用于保存当前对象的数据
    public GameObjectSave ISaveableSave()
    {
        // 存储当前场景的数据
        ISaveableStoreScene(SceneManager.GetActiveScene().name);

        return GameObjectSave;
    }



    // ISaveable接口的方法，用于存储场景数据
    public void ISaveableStoreScene(string sceneName)
    {
        // 如果保存数据中已存在当前场景的数据，则移除
        GameObjectSave.sceneData.Remove(sceneName);

        // 获取场景中所有的Item组件
        List<SceneItem> sceneItemList = new List<SceneItem>();
        Item[] itemsInScene = FindObjectsOfType<Item>();

        // 循环遍历所有物品
        foreach (Item item in itemsInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemCode = item.ItemCode;
            sceneItem.position = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
            sceneItem.itemName = item.name;

            // 将物品添加到列表
            sceneItemList.Add(sceneItem);
        }

        // 创建场景保存数据，并设置物品列表
        SceneSave sceneSave = new SceneSave();
        sceneSave.listSceneItem = sceneItemList;

        // 将场景保存数据添加到游戏对象的保存数据中
        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }
}

/*SceneItemsManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保SceneItemsManager在游戏场景中是唯一的实例。同时，它实现了ISaveable接口，用于保存和加载场景中的物品。

parentItem是一个私有变量，用于存储物品的父Transform，所有物品都将作为其子对象。

itemPrefab是一个可序列化的私有变量，用于存储物品的预制体。

_iSaveableUniqueID和_gameObjectSave是私有变量，分别用于存储ISaveable接口的唯一标识符和游戏对象的保存数据。

AfterSceneLoad方法在场景加载后调用，用于获取标记为Tags.ItemsParentTransform的GameObject的Transform组件。

Awake方法在游戏对象创建时调用，用于初始化唯一标识符和游戏对象的保存数据。

DestroySceneItems方法销毁场景中所有的物品。

InstantiateSceneItem方法根据物品代码和位置实例化一个新的物品。

InstantiateSceneItems方法根据保存的数据实例化场景中的物品。

OnDisable和OnEnable方法分别在组件被禁用和启用时调用，用于注册和注销事件。

ISaveableDeregister、ISaveableLoad、ISaveableRestoreScene、ISaveableRegister、ISaveableSave和ISaveableStoreScene方法实现了ISaveable接口，用于管理物品的保存和加载。

这个类的主要用途是提供一个集中的接口来管理游戏场景中的物品，包括物品的创建、销毁和保存加载。通过实现ISaveable接口，SceneItemsManager可以与游戏的保存和加载系统无缝集成。*/
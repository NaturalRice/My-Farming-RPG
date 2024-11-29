// 引用所需的命名空间
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

// 继承自SingletonMonobehaviour，确保SaveLoadManager类在游戏场景中是唯一的
public class SaveLoadManager : SingletonMonobehaviour<SaveLoadManager>
{
    // 公开的GameSave对象，用于存储游戏的保存数据
    public GameSave gameSave;
    // 公开的ISaveable对象列表，用于存储所有可保存的游戏对象
    public List<ISaveable> iSaveableObjectList;

    // 重写Awake方法，确保在游戏对象创建时初始化iSaveableObjectList
    protected override void Awake()
    {
        base.Awake();

        iSaveableObjectList = new List<ISaveable>();
    }

    // 从文件加载数据的方法
    public void LoadDataFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();

        // 检查保存文件是否存在
        if (File.Exists(Application.persistentDataPath + "/WildHopeCreek.dat"))
        {
            gameSave = new GameSave();

            // 打开文件
            FileStream file = File.Open(Application.persistentDataPath + "/WildHopeCreek.dat", FileMode.Open);

            // 反序列化文件内容到gameSave对象
            gameSave = (GameSave)bf.Deserialize(file);

            // 遍历所有ISaveable对象并应用保存数据
            for (int i = iSaveableObjectList.Count - 1; i > -1; i--)
            {
                if (gameSave.gameObjectData.ContainsKey(iSaveableObjectList[i].ISaveableUniqueID))
                {
                    iSaveableObjectList[i].ISaveableLoad(gameSave);
                }
                // 如果ISaveable对象的唯一ID不在游戏对象数据中，则销毁对象
                else
                {
                    Component component = (Component)iSaveableObjectList[i];
                    Destroy(component.gameObject);
                }
            }

            // 关闭文件
            file.Close();
        }

        // 禁用暂停菜单
        UIManager.Instance.DisablePauseMenu();
    }

    // 将数据保存到文件的方法
    public void SaveDataToFile()
    {
        gameSave = new GameSave();

        // 遍历所有ISaveable对象并生成保存数据
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            gameSave.gameObjectData.Add(iSaveableObject.ISaveableUniqueID, iSaveableObject.ISaveableSave());
        }

        BinaryFormatter bf = new BinaryFormatter();

        // 创建文件
        FileStream file = File.Open(Application.persistentDataPath + "/WildHopeCreek.dat", FileMode.Create);

        // 序列化gameSave对象到文件
        bf.Serialize(file, gameSave);

        // 关闭文件
        file.Close();

        // 禁用暂停菜单
        UIManager.Instance.DisablePauseMenu();
    }

    // 存储当前场景数据的方法
    public void StoreCurrentSceneData()
    {
        // 遍历所有ISaveable对象并触发存储场景数据
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }

    // 恢复当前场景数据的方法
    public void RestoreCurrentSceneData()
    {
        // 遍历所有ISaveable对象并触发恢复场景数据
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}

/*SaveLoadManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保SaveLoadManager在游戏场景中是唯一的实例。

gameSave是一个GameSave对象，用于存储整个游戏的保存数据。

iSaveableObjectList是一个ISaveable对象列表，用于存储所有可保存的游戏对象。

Awake方法在游戏对象创建时调用，用于初始化iSaveableObjectList。

LoadDataFromFile方法用于从文件加载保存数据。它使用BinaryFormatter将文件内容反序列化到gameSave对象，并遍历所有ISaveable对象，将保存数据应用到这些对象上。如果某个ISaveable对象的唯一ID不在保存数据中，则销毁该对象。

SaveDataToFile方法用于将保存数据序列化到文件。它遍历所有ISaveable对象，生成保存数据，并使用BinaryFormatter将gameSave对象序列化到文件。

StoreCurrentSceneData方法用于存储当前场景的数据。它遍历所有ISaveable对象，触发每个对象存储场景数据。

RestoreCurrentSceneData方法用于恢复当前场景的数据。它遍历所有ISaveable对象，触发每个对象恢复场景数据。

这个类的主要用途是提供一个集中的接口来管理游戏的保存和加载操作，确保游戏状态的一致性和完整性。通过使用单例模式，可以确保游戏中只有一个SaveLoadManager实例，从而避免数据冲突和不一致性。*/
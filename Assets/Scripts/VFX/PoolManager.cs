// 引用所需的命名空间
using System.Collections.Generic;
using UnityEngine;

// 继承自SingletonMonobehaviour，确保PoolManager类在游戏场景中是唯一的
public class PoolManager : SingletonMonobehaviour<PoolManager>
{
    // 私有字典，用于存储对象池
    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    // 可序列化的私有变量，用于在Inspector中设置对象池配置
    [SerializeField] private Pool[] pool = null;
    // 可序列化的私有变量，用于在Inspector中设置对象池的父Transform
    [SerializeField] private Transform objectPoolTransform = null;

    // 可序列化的内部结构体，用于定义对象池的配置
    [System.Serializable]
    public struct Pool
    {
        public int poolSize; // 池大小
        public GameObject prefab; // 预制体
    }

    // 在游戏开始时调用
    private void Start()
    {
        // 创建对象池
        for (int i = 0; i < pool.Length; i++)
        {
            CreatePool(pool[i].prefab, pool[i].poolSize);
        }
    }

    // 创建对象池的方法
    private void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID(); // 获取预制体的唯一标识符
        string prefabName = prefab.name; // 获取预制体名称

        GameObject parentGameObject = new GameObject(prefabName + "Anchor"); // 创建父游戏对象

        parentGameObject.transform.SetParent(objectPoolTransform); // 设置父Transform

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>()); // 添加到字典

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject; // 实例化预制体
                newObject.SetActive(false); // 设置为非活动状态

                poolDictionary[poolKey].Enqueue(newObject); // 添加到队列
            }
        }
    }

    // 重用对象池中的对象的方法
    public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID(); // 获取预制体的唯一标识符

        if (poolDictionary.ContainsKey(poolKey))
        {
            // 从池队列中获取对象
            GameObject objectToReuse = GetObjectFromPool(poolKey);

            ResetObject(position, rotation, objectToReuse, prefab); // 重置对象位置和旋转

            return objectToReuse;
        }
        else
        {
            Debug.Log("No object pool for " + prefab); // 没有找到对象池
            return null;
        }
    }

    // 从对象池中获取对象的方法
    private GameObject GetObjectFromPool(int poolKey)
    {
        GameObject objectToReuse = poolDictionary[poolKey].Dequeue(); // 出队
        poolDictionary[poolKey].Enqueue(objectToReuse); // 入队

        if (objectToReuse.activeSelf == true)
        {
            objectToReuse.SetActive(false); // 设置为非活动状态
        }

        return objectToReuse;
    }

    // 重置对象位置和旋转的方法
    private static void ResetObject(Vector3 position, Quaternion rotation, GameObject objectToReuse, GameObject prefab)
    {
        objectToReuse.transform.position = position; // 设置位置
        objectToReuse.transform.rotation = rotation; // 设置旋转
        objectToReuse.transform.localScale = prefab.transform.localScale; // 设置缩放
    }
}

/*PoolManager类继承自SingletonMonobehaviour，这是一个单例模式的实现，确保PoolManager在游戏场景中是唯一的实例。

poolDictionary是一个私有字典，用于存储对象池，其中键是预制体的实例ID，值是预制体实例的队列。

pool是一个可序列化的私有变量，用于在Unity编辑器的Inspector面板中设置对象池配置。

objectPoolTransform是一个可序列化的私有变量，用于设置对象池的父Transform。

Pool结构体定义了对象池的配置，包括池大小和预制体。

Start方法在游戏开始时调用，用于创建对象池。

CreatePool方法创建对象池，包括实例化预制体和添加到队列。

ReuseObject方法重用对象池中的对象，包括从池队列中获取对象和重置对象位置和旋转。

GetObjectFromPool方法从对象池中获取对象。

ResetObject方法重置对象的位置、旋转和缩放。

这个类的主要用途是管理游戏中的对象池，通过重用对象实例来减少垃圾收集和提高性能。通过实现这些方法，它可以在游戏中高效地管理对象的创建和销毁。*/
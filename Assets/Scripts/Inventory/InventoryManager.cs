using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合类
using UnityEngine; // 引用Unity引擎的命名空间

public class InventoryManager : SingletonMonobehaviour<InventoryManager>, ISaveable // 定义一个公共类InventoryManager，继承自SingletonMonobehaviour并实现ISaveable接口
{
    private UIInventoryBar inventoryBar; // 私有字段，存储UI库存条

    private Dictionary<int, ItemDetails> itemDetailsDictionary; // 私有字段，存储物品详情字典

    private int[] selectedInventoryItem; // 私有字段，存储选中的库存项的索引，数组的索引是库存列表，值是物品代码

    public List<InventoryItem>[] inventoryLists; // 公开字段，存储库存列表数组

    [HideInInspector] public int[] inventoryListCapacityIntArray; // 公开字段，存储每个库存列表的容量，数组的索引是库存列表（来自InventoryLocation枚举），值是该库存列表的容量

    [SerializeField] private SO_ItemList itemList = null; // 可序列化的私有字段，存储物品列表

    private string _iSaveableUniqueID; // 私有字段，存储ISaveable接口的唯一ID
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } } // ISaveableUniqueID属性

    private GameObjectSave _gameObjectSave; // 私有字段，存储GameObjectSave
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } } // GameObjectSave属性


    protected override void Awake() // 保护的重写方法，Awake在对象被创建时调用
    {
        base.Awake(); // 调用基类的Awake方法

        // 创建库存列表
        CreateInventoryLists(); // 创建库存列表

        // 创建物品详情字典
        CreateItemDetailsDictionary(); // 创建物品详情字典

        // 初始化选中库存项数组
        selectedInventoryItem = new int[(int)InventoryLocation.count]; // 初始化选中库存项数组

        for (int i = 0; i < selectedInventoryItem.Length; i++) // 遍历选中库存项数组
        {
            selectedInventoryItem[i] = -1; // 将每个元素设置为-1
        }

        // 获取游戏对象的唯一ID并创建保存数据对象
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID; // 获取唯一ID

        GameObjectSave = new GameObjectSave(); // 创建GameObjectSave
    }

    private void OnDisable() // 当组件被禁用时调用
    {
        ISaveableDeregister(); // 调用ISaveableDeregister方法
    }


    private void OnEnable() // 当组件被启用时调用
    {
        ISaveableRegister(); // 调用ISaveableRegister方法
    }

    private void Start() // Start方法在对象启用时调用
    {
        inventoryBar = FindObjectOfType<UIInventoryBar>(); // 查找UI库存条
    }

    private void CreateInventoryLists() // 创建库存列表的方法
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count]; // 创建库存列表数组

        for (int i = 0; i < (int)InventoryLocation.count; i++) // 遍历库存位置
        {
            inventoryLists[i] = new List<InventoryItem>(); // 为每个位置创建一个新的库存列表
        }

        // 初始化库存列表容量数组
        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count]; // 初始化库存列表容量数组

        // 初始化玩家库存列表容量
        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity; // 设置玩家初始库存容量
    }

    /// <summary>
    /// 从脚本对象物品列表中填充itemDetailsDictionary
    /// </summary>
    private void CreateItemDetailsDictionary() // 创建物品详情字典的方法
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>(); // 创建物品详情字典

        foreach (ItemDetails itemDetails in itemList.itemDetails) // 遍历物品列表
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails); // 将物品详情添加到字典中
        }
    }

    /// <summary>
    /// 将物品添加到inventoryLocation的库存列表中，然后销毁gameObjectToDelete
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete) // 公共方法，添加物品
    {
        AddItem(inventoryLocation, item); // 调用AddItem方法添加物品

        Destroy(gameObjectToDelete); // 销毁游戏对象
    }

    /// <summary>
    /// 将物品添加到inventoryLocation的库存列表中
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item) // 公共方法，添加物品
    {
        int itemCode = item.ItemCode; // 获取物品代码
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation]; // 获取库存列表

        // 检查库存是否已经包含该物品
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode); // 查找物品在库存中的位置

        if (itemPosition != -1) // 如果找到物品
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition); // 在位置添加物品
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode); // 在位置添加物品
        }

        // 发送库存已更新的事件
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]); // 调用库存更新事件
    }

    /// <summary>
    /// 将类型为itemCode的物品添加到inventoryLocation的库存列表中
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, int itemCode) // 公共方法，添加物品
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation]; // 获取库存列表

        // 检查库存是否已经包含该物品
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode); // 查找物品在库存中的位置

        if (itemPosition != -1) // 如果找到物品
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition); // 在位置添加物品
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode); // 在位置添加物品
        }

        // 发送库存已更新的事件
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]); // 调用库存更新事件
    }



    /// <summary>
    /// 将物品添加到库存末尾
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode) // 在位置添加物品的方法
    {
        InventoryItem inventoryItem = new InventoryItem(); // 创建库存项

        inventoryItem.itemCode = itemCode; // 设置物品代码
        inventoryItem.itemQuantity = 1; // 设置物品数量
        inventoryList.Add(inventoryItem); // 将库存项添加到列表

        //DebugPrintInventoryList(inventoryList); // 调试打印库存列表
    }

    /// <summary>
    /// 将物品添加到库存中的位置
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position) // 在位置添加物品的方法
    {
        InventoryItem inventoryItem = new InventoryItem(); // 创建库存项

        int quantity = inventoryList[position].itemQuantity + 1; // 增加物品数量
        inventoryItem.itemQuantity = quantity; // 设置物品数量
        inventoryItem.itemCode = itemCode; // 设置物品代码
        inventoryList[position] = inventoryItem; // 将库存项添加到列表

        //DebugPrintInventoryList(inventoryList); // 调试打印库存列表
    }

    ///<summary>
    ///在inventoryLocation库存列表中交换fromItem索引和toItem索引的物品
    ///</summary>

    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem) // 交换库存项的方法
    {
        // 如果fromItem索引和toItem索引在列表范围内，不相同，且大于等于零
        if (fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count
             && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem]; // 获取fromItem索引的物品
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem]; // 获取toItem索引的物品

            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem; // 交换物品
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem; // 交换物品

            // 发送库存已更新的事件
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);// 调用库存更新事件
        }
    }

    /// <summary>
    /// 清除inventoryLocation的选中库存项
    /// </summary>
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation) // 清除选中库存项的方法
    {
        selectedInventoryItem[(int)inventoryLocation] = -1; // 将选中库存项设置为-1
    }



    /// <summary>
    /// 查找物品代码是否已经在库存中。如果在库存列表中找到物品，返回物品位置，否则返回-1
    /// </summary>
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode) // 查找物品在库存中的位置的方法
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation]; // 获取库存列表

        for (int i = 0; i < inventoryList.Count; i++) // 遍历库存列表
        {
            if (inventoryList[i].itemCode == itemCode) // 如果找到物品代码
            {
                return i; // 返回物品位置
            }
        }

        return -1; // 如果没有找到物品，返回-1
    }

    /// <summary>
    /// 返回物品代码对应的ItemDetails（来自SO_ItemList），如果物品代码不存在，则返回null
    /// </summary>
    public ItemDetails GetItemDetails(int itemCode) // 获取物品详情的方法
    {
        ItemDetails itemDetails; // 创建ItemDetails变量

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails)) // 如果字典中包含物品代码
        {
            return itemDetails; // 返回物品详情
        }
        else
        {
            return null; // 如果没有找到物品详情，返回null
        }
    }

    /// <summary>
    /// 返回inventoryLocation中当前选中物品的ItemDetails，如果没有物品被选中，则返回null
    /// </summary>
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation) // 获取选中库存项详情的方法
    {
        int itemCode = GetSelectedInventoryItem(inventoryLocation); // 获取选中库存项的物品代码

        if (itemCode == -1) // 如果物品代码为-1
        {
            return null; // 返回null
        }
        else
        {
            return GetItemDetails(itemCode); // 返回物品详情
        }
    }


    /// <summary>
    /// 获取inventoryLocation的选中物品 - 返回物品代码或-1（如果没有物品被选中）
    /// </summary>
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation) // 获取选中库存项的方法
    {
        return selectedInventoryItem[(int)inventoryLocation]; // 返回选中库存项的物品代码
    }



    /// <summary>
    /// 获取物品类型的描述 - 为给定的ItemType返回物品类型的描述字符串
    /// </summary>
    public string GetItemTypeDescription(ItemType itemType) // 获取物品类型描述的方法
    {
        string itemTypeDescription; // 创建物品类型描述字符串

        switch (itemType) // 根据物品类型
        {
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool; // 设置物品类型描述
                break;

            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool; // 设置物品类型描述
                break;

            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool; // 设置物品类型描述
                break;

            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool; // 设置物品类型描述
                break;

            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool; // 设置物品类型描述
                break;

            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool; // 设置物品类型描述
                break;

            default:
                itemTypeDescription = itemType.ToString(); // 默认情况下，返回物品类型的字符串表示
                break;
        }

        return itemTypeDescription; // 返回物品类型描述
    }

    public void ISaveableRegister() // ISaveableRegister方法
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this); // 将当前对象添加到保存对象列表
    }

    public void ISaveableDeregister() // ISaveableDeregister方法
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this); // 从保存对象列表中移除当前对象
    }

    public GameObjectSave ISaveableSave() // ISaveableSave方法
    {
        // 创建新的场景保存
        SceneSave sceneSave = new SceneSave();

        // 移除任何现有的持久场景的保存数据
        GameObjectSave.sceneData.Remove(Settings.PersistentScene);

        // 将库存列表数组添加到持久场景保存
        sceneSave.listInvItemArray = inventoryLists;

        // 将库存列表容量数组添加到持久场景保存
        sceneSave.intArrayDictionary = new Dictionary<string, int[]>();
        sceneSave.intArrayDictionary.Add("inventoryListCapacityArray", inventoryListCapacityIntArray);

        // 将游戏对象的场景保存添加到保存数据
        GameObjectSave.sceneData.Add(Settings.PersistentScene, sceneSave);

        return GameObjectSave; // 返回GameObjectSave
    }


    public void ISaveableLoad(GameSave gameSave) // ISaveableLoad方法
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave)) // 如果找到游戏对象保存数据
        {
            GameObjectSave = gameObjectSave; // 设置GameObjectSave

            // 需要找到库存列表 - 首先尝试定位游戏对象的保存场景
            if (gameObjectSave.sceneData.TryGetValue(Settings.PersistentScene, out SceneSave sceneSave)) // 如果找到场景保存数据
            {
                // 如果持久场景存在库存列表数组
                if (sceneSave.listInvItemArray != null) // 如果场景保存数据包含库存列表数组
                {
                    inventoryLists = sceneSave.listInvItemArray; // 设置库存列表

                    // 发送库存已更新的事件
                    for (int i = 0; i < (int)InventoryLocation.count; i++) // 遍历库存位置
                    {
                        EventHandler.CallInventoryUpdatedEvent((InventoryLocation)i, inventoryLists[i]); // 调用库存更新事件
                    }

                    // 清除玩家携带的物品
                    Player.Instance.ClearCarriedItem(); // 清除玩家携带的物品

                    // 清除库存条上的高亮显示
                    inventoryBar.ClearHighlightOnInventorySlots(); // 清除库存条上的高亮显示
                }

                // 如果场景保存数据包含整数数组字典
                if (sceneSave.intArrayDictionary != null && sceneSave.intArrayDictionary.TryGetValue("inventoryListCapacityArray", out int[] inventoryCapacityArray)) // 如果场景保存数据包含库存列表容量数组
                {
                    inventoryListCapacityIntArray = inventoryCapacityArray; // 设置库存列表容量数组
                }
            }

        }
    }

    public void ISaveableStoreScene(string sceneName) // ISaveableStoreScene方法
    {
        // 由于库存管理器位于持久场景中，因此不需要执行任何操作
    }

    public void ISaveableRestoreScene(string sceneName) // ISaveableRestoreScene方法
    {
        // 由于库存管理器位于持久场景中，因此不需要执行任何操作
    }



    /// <summary>
    /// 从库存中移除物品，并在丢弃位置创建游戏对象
    /// </summary>
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode) // 从库存中移除物品的方法
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation]; // 获取库存列表

        // 检查库存是否已经包含该物品
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode); // 查找物品在库存中的位置

        if (itemPosition != -1) // 如果找到物品
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition); // 在位置移除物品
        }

        // 发送库存已更新的事件
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]); // 调用库存更新事件
    }

    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position) // 在位置移除物品的方法
    {
        InventoryItem inventoryItem = new InventoryItem(); // 创建库存项

        int quantity = inventoryList[position].itemQuantity - 1; // 减少物品数量

        if (quantity > 0) // 如果物品数量大于0
        {
            inventoryItem.itemQuantity = quantity; // 设置物品数量
            inventoryItem.itemCode = itemCode; // 设置物品代码
            inventoryList[position] = inventoryItem; // 更新库存列表
        }
        else
        {
            inventoryList.RemoveAt(position); // 从库存列表中移除物品
        }
    }

    /// <summary>
    /// 将inventoryLocation的选中库存项设置为itemCode
    /// </summary>
    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode) // 设置选中库存项的方法
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode; // 将选中库存项设置为itemCode
    }


    //private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    //{
    //    foreach (InventoryItem inventoryItem in inventoryList) // 遍历库存列表
    //    {
    //        Debug.Log("Item Description:" + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription + "    Item Quantity: " + inventoryItem.itemQuantity); // 调试打印物品描述和数量
    //    }
    //    Debug.Log("******************************************************************************"); // 调试打印分隔线
    //}
}

/*1. **InventoryManager 类**：这个类作为游戏的库存管理器，继承自`SingletonMonobehaviour`以确保整个游戏中只有一个实例，并实现了`ISaveable`接口以支持数据保存和加载。

2. **字段和属性**：
   - `inventoryBar`：存储UI库存条的私有字段。
   - `itemDetailsDictionary`：存储物品详情字典的私有字段。
   - `selectedInventoryItem`：存储选中的库存项的索引的私有字段。
   - `inventoryLists`：存储库存列表数组的公开字段。
   - `inventoryListCapacityIntArray`：存储每个库存列表的容量的公开字段。
   - `itemList`：存储物品列表的可序列化私有字段。
   - `ISaveableUniqueID`：存储ISaveable接口的唯一ID的属性。
   - `GameObjectSave`：存储GameObjectSave的属性。

3. **Awake 方法**：在对象被创建时调用，用于创建库存列表、物品详情字典和初始化选中库存项数组。

4. **OnDisable 和 OnEnable 方法**：分别在组件被禁用和启用时调用，用于注册和注销ISaveable。

5. **Start 方法**：在对象启用时调用，用于查找UI库存条。

6. **CreateInventoryLists 方法**：创建库存列表数组和库存列表容量数组。

7. **CreateItemDetailsDictionary 方法**：从脚本对象物品列表中填充物品详情字典。

8. **AddItem 方法**：将物品添加到指定库存位置的库存列表中，并发送库存更新事件。

9. **AddItemAtPosition 方法**：在库存列表的指定位置添加物品。

10. **SwapInventoryItems 方法**：在库存列表中交换两个物品的位置。

11. **ClearSelectedInventoryItem 方法**：清除指定库存位置的选中库存项。

12. **FindItemInInventory 方法**：在库存列表中查找物品的位置。

13. **GetItemDetails 方法**：根据物品代码获取物品详情。

14. **GetSelectedInventoryItemDetails 方法**：获取选中库存项的物品详情。

15. **GetSelectedInventoryItem 方法**：获取选中库存项的物品代码。

16. **GetItemTypeDescription 方法**：根据物品类型获取物品类型的描述。

17. **ISaveableRegister 和 ISaveableDeregister 方法**：注册和注销ISaveable。

18. **ISaveableSave 和 ISaveableLoad 方法**：保存和加载ISaveable数据。

19. **ISaveableStoreScene 和 ISaveableRestoreScene 方法**：存储和恢复场景数据（由于库存管理器位于持久场景中，因此不需要执行任何操作）。

20. **RemoveItem 方法**：从库存中移除物品，并发送库存更新事件。

21. **RemoveItemAtPosition 方法**：在库存列表的指定位置移除物品。

22. **SetSelectedInventoryItem 方法**：设置选中库存项的物品代码。

这个类是游戏中库存系统的一个核心组件，负责管理库存列表、物品详情和选中库存项。通过实现ISaveable接口，它还支持数据的保存和加载，使得游戏状态可以被持久化。*/

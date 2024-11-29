using System.Collections; // 引用System.Collections命名空间，提供非泛型集合接口
using System.Collections.Generic; // 引用System.Collections.Generic命名空间，提供泛型集合接口
using UnityEngine; // 引用Unity引擎的命名空间

/// <summary>
/// 附加到作物预制体上，以在网格属性字典中设置值
/// </summary>
public class CropInstantiator : MonoBehaviour // 定义一个继承自MonoBehaviour的公共类
{
    private Grid grid; // 私有字段，存储网格游戏对象
    [SerializeField] private int daysSinceDug = -1; // 可序列化的私有字段，存储自土地被翻耕以来的天数
    [SerializeField] private int daysSinceWatered = -1; // 可序列化的私有字段，存储自上次浇水以来的天数
    [ItemCodeDescription] // 自定义属性，可能用于显示物品代码的描述
    [SerializeField] private int seedItemCode = 0; // 可序列化的私有字段，存储种子物品代码
    [SerializeField] private int growthDays = 0; // 可序列化的私有字段，存储生长天数

    private void OnDisable() // 当组件被禁用时调用
    {
        EventHandler.InstantiateCropPrefabsEvent -= InstantiateCropPrefabs; // 取消订阅InstantiateCropPrefabs事件
    }

    private void OnEnable() // 当组件被启用时调用
    {
        EventHandler.InstantiateCropPrefabsEvent += InstantiateCropPrefabs; // 订阅InstantiateCropPrefabs事件
    }

    private void InstantiateCropPrefabs() // 实例化作物预制体的方法
    {
        // 获取网格游戏对象
        grid = GameObject.FindObjectOfType<Grid>();

        // 获取作物的网格位置
        Vector3Int cropGridPosition = grid.WorldToCell(transform.position);

        // 设置作物网格属性
        SetCropGridProperties(cropGridPosition);

        // 销毁这个游戏对象
        Destroy(gameObject);
    }

    private void SetCropGridProperties(Vector3Int cropGridPosition) // 设置作物网格属性的方法
    {
        if (seedItemCode > 0) // 如果种子物品代码大于0
        {
            GridPropertyDetails gridPropertyDetails; // 网格属性详情

            gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y); // 获取网格属性详情

            if (gridPropertyDetails == null) // 如果网格属性详情为null
            {
                gridPropertyDetails = new GridPropertyDetails(); // 创建新的网格属性详情
            }

            gridPropertyDetails.daysSinceDug = daysSinceDug; // 设置自土地被翻耕以来的天数
            gridPropertyDetails.daysSinceWatered = daysSinceWatered; // 设置自上次浇水以来的天数
            gridPropertyDetails.seedItemCode = seedItemCode; // 设置种子物品代码
            gridPropertyDetails.growthDays = growthDays; // 设置生长天数

            GridPropertiesManager.Instance.SetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y, gridPropertyDetails); // 设置网格属性详情
        }
    }
}

/*CropInstantiator 类：这个类用于在作物预制体被实例化时设置网格属性字典中的值，继承自MonoBehaviour，可以附加到Unity场景中的GameObject上。

字段：

grid：存储网格游戏对象的私有字段。
daysSinceDug：存储自土地被翻耕以来的天数的可序列化私有字段。
daysSinceWatered：存储自上次浇水以来的天数的可序列化私有字段。
seedItemCode：存储种子物品代码的可序列化私有字段。
growthDays：存储生长天数的可序列化私有字段。
OnDisable 和 OnEnable 方法：这两个方法用于在组件被启用和禁用时订阅和取消订阅InstantiateCropPrefabsEvent事件。

InstantiateCropPrefabs 方法：当InstantiateCropPrefabsEvent事件被触发时调用，用于获取网格位置，设置作物网格属性，并销毁当前游戏对象。

SetCropGridProperties 方法：设置作物网格属性的方法，包括自土地被翻耕以来的天数、自上次浇水以来的天数、种子物品代码和生长天数。

这个类通常用于游戏中的农业模拟，允许玩家种植作物，并在作物预制体被实例化时自动设置网格属性，从而管理作物的生长状态。*/
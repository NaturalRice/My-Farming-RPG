using UnityEngine; // 引用Unity引擎的命名空间

[System.Serializable] // 标记这个类为可序列化，意味着它可以被保存到文件中或通过网络传输
public class CropDetails // 定义一个公共类
{
    [ItemCodeDescription] // 自定义属性，可能用于显示物品代码的描述
    public int seedItemCode; // 这是对应种子的物品代码

    public int[] growthDays; // 每个生长阶段的天数
    public GameObject[] growthPrefab; // 实例化生长阶段时使用的预制体
    public Sprite[] growthSprite; // 生长阶段的精灵图
    public Season[] seasons; // 生长的季节
    public Sprite harvestedSprite; // 收割后使用的精灵图
    [ItemCodeDescription] // 自定义属性，可能用于显示物品代码的描述
    public int harvestedTransformItemCode; // 如果物品在收割时转变为另一个物品，将填充此物品代码
    public bool hideCropBeforeHarvestedAnimation; // 在收割动画之前是否禁用作物
    public bool disableCropCollidersBeforeHarvestedAnimation; // 在收割动画之前是否禁用作物的碰撞器，以避免动画影响其他游戏对象
    public bool isHarvestedAnimation; // 如果为真，则在最终生长阶段的预制体上播放收割动画
    public bool isHarvestActionEffect = false; // 标志，用于确定是否有收割动作效果
    public bool spawnCropProducedAtPlayerPosition; // 是否在玩家位置生成产出的作物
    public HarvestActionEffect harvestActionEffect; // 作物的收割动作效果
    public SoundName harvestSound; // 作物的收割声音

    [ItemCodeDescription] // 自定义属性，可能用于显示物品代码的描述
    public int[] harvestToolItemCode; // 可以用于收割的工具的物品代码数组，如果没有工具要求，则数组元素为0
    public int[] requiredHarvestActions; // 对应于收割工具物品代码数组中的每个工具所需的收割动作次数
    [ItemCodeDescription] // 自定义属性，可能用于显示物品代码的描述
    public int[] cropProducedItemCode; // 收割作物产出的物品代码数组
    public int[] cropProducedMinQuantity; // 收割作物产出的最小数量数组
    public int[] cropProducedMaxQuantity; // 如果最大数量大于最小数量，则在最小和最大之间随机产出作物数量
    public int daysToRegrow; // 下一个作物重新生长的天数，如果是单次作物则为-1


    /// <summary>
    /// 如果工具物品代码可以用于收割此作物，返回true，否则返回false
    /// </summary>
    public bool CanUseToolToHarvestCrop(int toolItemCode) // 公共方法，判断工具是否可以用于收割作物
    {
        if (RequiredHarvestActionsForTool(toolItemCode) == -1) // 如果工具不能用于收割
        {
            return false; // 返回false
        }
        else // 否则
        {
            return true; // 返回true
        }
    }


    /// <summary>
    /// 如果工具不能用于收割此作物，返回-1，否则返回此工具所需的收割动作次数
    /// </summary>
    public int RequiredHarvestActionsForTool(int toolItemCode) // 公共方法，返回工具所需的收割动作次数
    {
        for (int i = 0; i < harvestToolItemCode.Length; i++) // 遍历工具物品代码数组
        {
            if (harvestToolItemCode[i] == toolItemCode) // 如果找到匹配的工具物品代码
            {
                return requiredHarvestActions[i]; // 返回对应的收割动作次数
            }
        }
        return -1; // 如果没有找到匹配的工具物品代码，返回-1
    }
}

/*CropDetails 类：这个类用于存储和管理作物的详细信息，包括生长阶段、收割工具、产出物品等。

字段：

seedItemCode：作物种子的物品代码。
growthDays：每个生长阶段所需的天数。
growthPrefab：每个生长阶段对应的预制体。
growthSprite：每个生长阶段的精灵图。
seasons：作物生长的季节。
harvestedSprite：收割后的精灵图。
harvestedTransformItemCode：收割后转变为的物品代码。
hideCropBeforeHarvestedAnimation：在收割动画前是否隐藏作物。
disableCropCollidersBeforeHarvestedAnimation：在收割动画前是否禁用作物的碰撞器。
isHarvestedAnimation：是否有收割动画。
isHarvestActionEffect：是否有收割动作效果。
spawnCropProducedAtPlayerPosition：是否在玩家位置生成产出的作物。
harvestActionEffect：收割动作效果。
harvestSound：收割声音。
harvestToolItemCode：可以用于收割的工具的物品代码数组。
requiredHarvestActions：每个工具所需的收割动作次数。
cropProducedItemCode：收割产出的物品代码数组。
cropProducedMinQuantity：收割产出的最小数量数组。
cropProducedMaxQuantity：收割产出的最大数量数组。
daysToRegrow：作物重新生长的天数。
CanUseToolToHarvestCrop 方法：判断给定的工具物品代码是否可以用于收割此作物。

RequiredHarvestActionsForTool 方法：返回给定工具物品代码所需的收割动作次数，如果工具不能用于收割，则返回-1。

这个类是游戏中作物系统的一个核心组件，它提供了一个结构化的方式来存储和管理作物的详细信息，使得游戏开发者可以轻松地定义和管理不同类型的作物。*/
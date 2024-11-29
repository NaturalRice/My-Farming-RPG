public enum AnimationName
{
    // 动画名称枚举，包含各种角色的闲置、行走、跑步、使用工具和挥动工具的动画
    idleDown, idleUp, idleRight, idleLeft,
    walkUp, walkDown, walkRight, walkLeft,
    runUp, runDown, runRight, runLeft,
    useToolUp, useToolDown, useToolRight, useToolLeft,
    swingToolUp, swingToolDown, swingToolRight, swingToolLeft,
    liftToolUp, liftToolDown, liftToolRight, liftToolLeft,
    holdToolUp, holdToolDown, holdToolRight, holdToolLeft,
    pickDown, pickUp, pickRight, pickLeft,
    count // 计数器，用于跟踪枚举中值的数量
}

public enum CharacterPartAnimator
{
    // 角色部件动画枚举，包含角色的身体、手臂、头发、工具和帽子
    body, arms, hair, tool, hat, count
}

public enum PartVariantColour
{
    // 部件颜色变体枚举，目前只包含无（none）
    none, count
}

public enum PartVariantType
{
    // 部件类型变体枚举，包含无（none）、携带（carry）、锄头（hoe）、铲子（pickaxe）、斧头（axe）、镰刀（scythe）和水壶（wateringCan）
    none, carry, hoe, pickaxe, axe, scythe, wateringCan, count
}

public enum GridBoolProperty
{
    // 网格布尔属性枚举，包含可挖掘（diggable）、可放置物品（canDropItem）、可放置家具（canPlaceFurniture）、是路径（isPath）和是NPC障碍物（isNPCObstacle）
    diggable, canDropItem, canPlaceFurniture, isPath, isNPCObstacle
}

public enum InventoryLocation
{
    // 物品位置枚举，包含玩家（player）和箱子（chest）
    player, chest, count
}

public enum SceneName
{
    // 场景名称枚举，包含农场（Scene1_Farm）、田野（Scene2_Field）和小木屋（Scene3_Cabin）
    Scene1_Farm, Scene2_Field, Scene3_Cabin
}

public enum Season
{
    // 季节枚举，包含春季（Spring）、夏季（Summer）、秋季（Autumn）、冬季（Winter）和无（none）
    Spring, Summer, Autumn, Winter, none, count
}

public enum ToolEffect
{
    // 工具效果枚举，目前只包含无（none）
    none, watering
}

public enum HarvestActionEffect
{
    // 收割动作效果枚举，包含落叶（deciduousLeavesFalling）、松果（pineConesFalling）、砍树（choppingTreeTrunk）、碎石（breakingStone）、收割（reaping）和无（none）
    deciduousLeavesFalling, pineConesFalling, choppingTreeTrunk, breakingStone, reaping, none
}

public enum Weather
{
    // 天气枚举，包含干燥（dry）、下雨（raining）、下雪（snowing）和无（none）
    dry, raining, snowing, none, count
}

public enum Direction
{
    // 方向枚举，包含上（up）、下（down）、左（left）、右（right）和无（none）
    up, down, left, right, none
}

public enum SoundName
{
    // 声音名称枚举，包含无（none）、不同的脚步声（effectFootstepSoftGround、effectFootstepHardGround）、工具声音（effectAxe、effectPickaxe、effectScythe、effectHoe、effectWateringCan、effectBasket）、拾取声（effectPickupSound）、沙沙声（effectRustle）、树木倒下声（effectTreeFalling）、种植声（effectPlantingSound）、拔出声（effectPluck）、石头碎裂声（effectStoneShatter）、木头碎片声（effectWoodSplinters）、环境声音（ambientCountryside1、ambientCountryside2、ambientIndoors1）和音乐（musicCalm3、musicCalm1）
    none = 0, effectFootstepSoftGround = 10, effectFootstepHardGround = 20, effectAxe = 30, effectPickaxe = 40, effectScythe = 50, effectHoe = 60, effectWateringCan = 70, effectBasket = 80, effectPickupSound = 90, effectRustle = 100, effectTreeFalling = 110, effectPlantingSound = 120, effectPluck = 130, effectStoneShatter = 140, effectWoodSplinters = 150, ambientCountryside1 = 1000, ambientCountryside2 = 1010, ambientIndoors1 = 1020, musicCalm3 = 2000, musicCalm1 = 2010
}

public enum ItemType
{
    // 物品类型枚举，包含种子（Seed）、商品（Commodity）、水壶（Watering_tool）、锄头（Hoeing_tool）、砍伐工具（Chopping_tool）、破拆工具（Breaking_tool）、收割工具（Reaping_tool）、收集工具（Collecting_tool）、可收割的景观（Reapable_scenary）和家具（Furniture）
    Seed, Commodity, Watering_tool, Hoeing_tool, Chopping_tool, Breaking_tool, Reaping_tool, Collecting_tool, Reapable_scenary, Furniture, none, count
}

public enum Facing
{
    // 面向枚举，包含无（none）、前（front）、后（back）和右（right）
    none, front, back, right
}

/*AnimationName 枚举：包含角色的各种动画状态，如闲置、行走、跑步等。

CharacterPartAnimator 枚举：包含角色的不同部位，如身体、手臂、头发等。

PartVariantColour 枚举：包含部件的颜色变体，目前只有无。

PartVariantType 枚举：包含部件的类型变体，如携带、锄头、铲子等。

GridBoolProperty 枚举：包含网格的布尔属性，如是否可挖掘、是否可放置物品等。

InventoryLocation 枚举：包含物品的位置，如玩家身上或箱子中。

SceneName 枚举：包含游戏的不同场景名称。

Season 枚举：包含一年中的四个季节和无。

ToolEffect 枚举：包含工具的效果，目前只有无和浇水。

HarvestActionEffect 枚举：包含收割动作的效果，如落叶、松果等。

Weather 枚举：包含不同的天气状况，如干燥、下雨等。

Direction 枚举：包含四个基本方向和无。

SoundName 枚举：包含游戏中使用的各种声音名称。

ItemType 枚举：包含游戏中物品的类型，如种子、商品等。

Facing 枚举：包含角色或物体的面向方向，如前、后等。*/
// 静态类，用于存储游戏中使用的标签常量
public static class Tags
{
    // 边界限制器的标签
    public const string BoundsConfiner = "BoundsConfiner";
    // 作物父对象的标签
    public const string CropsParentTransform = "CropsParentTransform";
    //地面装饰1的标签
    public const string GroundDecoration1 = "GroundDecoration1";
    //地面装饰2的标签
    public const string GroundDecoration2 = "GroundDecoration2";

    // 物品父对象的标签
    public const string ItemsParentTransform = "ItemsParentTransform";
}

/*静态类定义：

public static class Tags：定义了一个公共静态类 Tags。静态类不能被实例化，并且它的所有成员都是自动静态的。
常量字符串定义：

public const string BoundsConfiner = "BoundsConfiner";：定义了一个常量字符串，用于标识边界限制器对象的标签。这个标签可能用于游戏中的一个逻辑，比如限制玩家或物体移动的范围。
public const string CropsParentTransform = "CropsParentTransform";：定义了一个常量字符串，用于标识作物父对象的标签。这个标签可能用于组织和管理游戏中的所有作物对象。
public const string GroundDecoration1 = "GroundDecoration1";：定义了一个常量字符串，用于标识地面装饰1的标签。这个标签可能用于区分游戏中不同类型的地面装饰。
public const string GroundDecoration2 = "GroundDecoration2";：定义了一个常量字符串，用于标识地面装饰2的标签。这个标签可能用于区分游戏中不同类型的地面装饰。
public const string ItemsParentTransform = "ItemsParentTransform";：定义了一个常量字符串，用于标识物品父对象的标签。这个标签可能用于组织和管理游戏中的所有物品对象。
总的来说，Tags 类提供了一个集中的地方来定义和管理游戏中使用的标签，这样可以避免在代码中直接使用字符串字面量，减少错误并提高代码的可维护性。通过使用常量，可以确保这些标签值在整个项目中保持一致。*/
// 定义一个名为ISaveable的接口，用于规定可保存对象的行为和属性
public interface ISaveable
{
    // 一个属性，用于获取和设置唯一标识符，用于区分不同的可保存对象
    string ISaveableUniqueID { get; set; }

    // 一个属性，用于获取和设置与游戏对象相关的保存数据
    GameObjectSave GameObjectSave { get; set; }

    // 注册方法，用于在保存系统中注册当前对象
    void ISaveableRegister();

    // 注销方法，用于在保存系统中注销当前对象
    void ISaveableDeregister();

    // 保存方法，用于创建当前对象的保存数据
    GameObjectSave ISaveableSave();

    // 加载方法，用于从提供的GameSave对象中加载当前对象的状态
    void ISaveableLoad(GameSave gameSave);

    // 存储场景方法，用于将当前对象的状态存储到指定的场景中
    void ISaveableStoreScene(string sceneName);

    // 恢复场景方法，用于从指定的场景中恢复当前对象的状态
    void ISaveableRestoreScene(string sceneName);
}

/*ISaveable接口定义了一组方法和属性，这些方法和属性必须被任何实现了这个接口的类所实现。这使得这些类的对象可以被保存和加载。

ISaveableUniqueID属性是一个字符串类型的属性，用于存储一个唯一标识符，这个标识符可以用来区分不同的可保存对象。

GameObjectSave属性是一个GameObjectSave类型的属性，用于存储与游戏对象相关的保存数据。

ISaveableRegister方法是一个注册方法，当一个对象需要被保存系统跟踪时，应该调用这个方法。

ISaveableDeregister方法是一个注销方法，当一个对象不再需要被保存系统跟踪时，应该调用这个方法。

ISaveableSave方法是一个保存方法，它应该返回一个包含当前对象状态的GameObjectSave对象。

ISaveableLoad方法是一个加载方法，它接受一个GameSave对象作为参数，并使用这个对象中的数据来恢复当前对象的状态。

ISaveableStoreScene方法是一个存储场景方法，它接受一个场景名称作为参数，并将当前对象的状态存储到这个场景中。

ISaveableRestoreScene方法是一个恢复场景方法，它接受一个场景名称作为参数，并从这个场景中恢复当前对象的状态。

这个接口的主要用途是为游戏中的保存和加载功能提供一个统一的接口，确保所有需要被保存的对象都遵循相同的规则和行为。通过实现这个接口，不同的游戏对象可以以一种一致的方式被保存和加载，这有助于代码的模块化和可维护性。*/
// 使用Unity引擎的命名空间
using UnityEngine;

// 定义一个泛型MonoBehaviour抽象类，确保泛型参数T是MonoBehaviour的子类
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // 私有静态变量，用于存储类的实例
    private static T instance;

    // 公共静态属性，用于访问类的实例
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    // 保护虚方法，用于在对象被创建时调用
    protected virtual void Awake()
    {
        // 如果实例为空，则将当前对象转换为泛型类型T并赋值给实例
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            // 如果实例已存在，则销毁当前对象的游戏对象
            Destroy(gameObject);
        }
    }
}

/*// 使用Unity引擎的命名空间
using UnityEngine;

// 定义一个泛型MonoBehaviour抽象类，确保泛型参数T是MonoBehaviour的子类
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // 私有静态变量，用于存储类的实例
    private static T instance;

    // 公共静态属性，用于访问类的实例
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    // 保护虚方法，用于在对象被创建时调用
    protected virtual void Awake()
    {
        // 如果实例为空，则将当前对象转换为泛型类型T并赋值给实例
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            // 如果实例已存在，则销毁当前对象的游戏对象
            Destroy(gameObject);
        }
    }
}*/
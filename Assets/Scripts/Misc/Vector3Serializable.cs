// 使类可以被序列化
[System.Serializable]
public class Vector3Serializable
{
    // 类的成员变量，分别代表三维空间中的x、y、z坐标
    public float x, y, z;

    // 构造函数，用于创建Vector3Serializable对象时初始化x、y、z坐标
    public Vector3Serializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // 无参构造函数
    public Vector3Serializable()
    {
    }
}

/*类定义：

[System.Serializable]：这是一个属性，它指示 Vector3Serializable 类可以被序列化。序列化是将对象的状态信息转换为可以存储（如文件或内存缓冲区）或传输（如通过网络）的过程。
public class Vector3Serializable：定义了一个公共类 Vector3Serializable。
成员变量：

public float x, y, z;：三个公共成员变量，分别用于存储三维向量在 x、y、z 轴上的分量。
构造函数：

public Vector3Serializable(float x, float y, float z)：这是一个带参数的构造函数，它允许在创建 Vector3Serializable 对象时初始化其 x、y、z 坐标。
this.x = x;：将传入的 x 参数赋值给对象的 x 成员变量。
this.y = y;：将传入的 y 参数赋值给对象的 y 成员变量。
this.z = z;：将传入的 z 参数赋值给对象的 z 成员变量。
public Vector3Serializable()：这是一个无参构造函数，用于创建一个 x、y、z 坐标均为默认值（0）的 Vector3Serializable 对象。
总的来说，Vector3Serializable 类提供了一个可序列化的替代品，用于表示三维空间中的点或向量。这个类通过两个构造函数允许在创建对象时设置或不设置初始坐标值。由于这个类是可序列化的，因此可以方便地在 Unity 编辑器中存储和编辑这些值，也可以通过网络传输或存储到文件中。*/
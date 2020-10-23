/// <summary>
/// 单例泛型，返回一个实例
/// </summary>
/// <typeparam name="约束条件，可以被new"></typeparam>
public class SingleTon<T> where T:new()
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            if(_Instance==null)
                _Instance=new T();
            return _Instance;
        }
    }    

}

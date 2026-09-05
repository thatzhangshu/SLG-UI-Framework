/// <summary>
/// UI Item 对象池生命周期接口。
/// 
/// 用于让被池化的 Item 在取出和回收时重置自身状态。
/// </summary>
public interface IUIItemPoolable
{
    /// <summary>
    /// 从对象池中取出时调用。
    /// </summary>
    void OnGetFromPool();

    /// <summary>
    /// 回收到对象池时调用。
    /// </summary>
    void OnReleaseToPool();
}
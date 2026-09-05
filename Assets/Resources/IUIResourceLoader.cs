/// <summary>
/// UI 资源加载接口。
/// 
/// UIManager 只依赖这个接口，
/// 不直接关心底层是 Resources、Addressables 还是 AssetBundle。
/// </summary>
public interface IUIResourceLoader
{
    T LoadUI<T>(string path) where T : UIBase;
}
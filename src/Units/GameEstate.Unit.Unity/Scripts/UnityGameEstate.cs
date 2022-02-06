using GameEstate;

public class UnityGameEstate : UnityEngine.MonoBehaviour
{
    static UnityGameEstate() => EstatePlatform.Startups.Add(UnityPlatform.Startup);
}
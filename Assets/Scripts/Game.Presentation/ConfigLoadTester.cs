using UnityEngine;

using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public class ConfigLoadTester : MonoBehaviour
{
    private void Start()
    {
        var loader = new JsonConfigLoader();
        var configService = new GameConfigService(loader);

        Debug.Log($"Player MaxHealth: {configService.PlayerConfig.MaxHealth}");
        Debug.Log($"Player MaxSpeed: {configService.PlayerConfig.MaxSpeed}");
        Debug.Log($"World Width: {configService.WorldConfig.Width}");
        Debug.Log($"Laser MaxCharges: {configService.LaserConfig.MaxCharges}");
    }
}
}

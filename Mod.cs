using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.Simulation;
using Unity.Entities;

namespace InstantBoarding
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(InstantBoarding)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        
        public static Setting m_Setting;
        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            // Enable customization once we see if we can disable Burst Compilation (but i fear it is needed for performance)
            // m_Setting = new Setting(this);
            // m_Setting.RegisterInOptionsUI();
            // GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));


            //AssetDatabase.global.LoadSettings(nameof(InstantBoarding), m_Setting, new Setting(this));


            var oldTrainSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<TransportTrainAISystem>();
            oldTrainSystem.Enabled = false;

            var oldCarSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<TransportCarAISystem>();
            oldCarSystem.Enabled = false;

            updateSystem.UpdateAt<PatchedTransportCarAISystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<PatchedTransportTrainAISystem>(SystemUpdatePhase.GameSimulation);
            log.Info("Completed Patching of Base Train/CarAI Systems.");
            //updateSystem.UpdateBefore<InstantBoardingSystem>(SystemUpdatePhase.GameSimulation);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}

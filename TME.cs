using DM;
using HarmonyLib;
using Landfall.TABS;
using Landfall.TABS.GameMode;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TABSModdingEssentials
{
    public class TME
    {
        public static void LoadAssets()
        {
            LandfallContentDatabase CDB = ContentDatabase.Instance().LandfallContentDatabase;
            AssetBundle assetbundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "swordtest"));
            //Load all our prefabs in
            List<GameObject> prefabs = assetbundle.LoadAllAssets<GameObject>().ToList();
            //Filter lists out based on their components due to the fact that all of the prefabs are gameobjects and cant be loaded directly as weaponitem for instance
            List<WeaponItem> weapons = prefabs.Where(x => x.TryGetComponent<WeaponItem>(out WeaponItem weap) == true).Select(x => x.GetComponent<WeaponItem>()).ToList();
            List<ProjectileEntity> projectiles = prefabs.Where(x => x.TryGetComponent<ProjectileEntity>(out ProjectileEntity projectile) == true).Select(x => x.GetComponent<ProjectileEntity>()).ToList();
            List<PropItem> props = prefabs.Where(x => x.TryGetComponent<PropItem>(out PropItem prop) == true).Select(x => x.GetComponent<PropItem>()).ToList();
            //Load assets that are directly loadable like Faction
            List<UnitBlueprint> blueprints = assetbundle.LoadAllAssets<UnitBlueprint>().ToList();
            List<Faction> factions = assetbundle.LoadAllAssets<Faction>().ToList();
            //Load the various types of assets
            if (prefabs.Count > 0)
            {
                foreach (var obj in prefabs)
                {
                    Unit ubase = obj.GetComponent<Unit>();
                    if (ubase != null)
                    {
                        CDB.m_unitBases.Add(ubase.Entity.GUID, obj);
                        ContentDatabase.Instance().AssetLoader.m_nonStreamableAssets.Add(ubase.Entity.GUID, obj);
                    }
                }
            }
            if (weapons.Count > 0)
            {
                foreach (var obj in weapons)
                {
                    CDB.m_weapons.Add(obj.Entity.GUID, obj.gameObject);
                    ContentDatabase.Instance().AssetLoader.m_nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                }
            }
            if (projectiles.Count > 0)
            {
                foreach (var obj in projectiles)
                {
                    CDB.m_projectiles.Add(obj.Entity.GUID, obj.gameObject);
                    ContentDatabase.Instance().AssetLoader.m_nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                }
            }
            if (props.Count > 0)
            {
                foreach (var obj in props)
                {
                    CDB.m_characterProps.Add(obj.Entity.GUID, obj.gameObject);
                    ContentDatabase.Instance().AssetLoader.m_nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                }
            }
            if (blueprints.Count > 0)
            {
                foreach (var obj in blueprints)
                {
                    CDB.m_unitBlueprints.Add(obj.Entity.GUID, obj);
                    ContentDatabase.Instance().AssetLoader.m_nonStreamableAssets.Add(obj.Entity.GUID, obj);
                }
            }
            if (factions.Count > 0)
            {
                foreach (var obj in factions)
                {
                    CDB.m_factions.Add(obj.Entity.GUID, obj);
                    ContentDatabase.Instance().AssetLoader.m_nonStreamableAssets.Add(obj.Entity.GUID, obj);
                }
            }
        }
        public static MapAsset MapToLoad;
        public static GameModeState GameMode = GameModeState.Sandbox;
        public static bool quickboot;
        public static void LoadLevel()
        {
            GameModeService service = ServiceLocator.GetService<GameModeService>();

            if (GameMode == GameModeState.Sandbox)
            {
                CampaignPlayerDataHolder.StartedPlayingSandbox();
                service.SetGameMode<SandboxGameMode>();
            }
            else if (GameMode == GameModeState.LocalMultiplayer)
                service.SetGameMode<LocalMultiplayerGameMode>();

            //If you know the map ID specifically, you can change the map it loads here
            MapToLoad = ContentDatabase.Instance().GetAllMapAssetsOrdered().ToArray().Where(x => x.MapName == "00_Simulation_Day_VC").First();
            TABSSceneManager.LoadMap(MapToLoad);
        }
        //Patch for skipping intros for quickbooting
        [HarmonyPatch(typeof(TABSBooter), "Init")]
        class FastBoot
        {
            [HarmonyPrefix]
            public static void Prefix(TABSBooter __instance)
            {
                if (TME.quickboot)
                {
                    TME.LoadLevel();
                    GameObject.Destroy(__instance);
                }
            }
        }
    }
}

using DM;
using HarmonyLib;
using Landfall.TABS;
using Landfall.TABS.GameMode;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Landfall.TABS.UnitEditor;

namespace CoolTABSMod
{
    public class TME : PROJECTNAMEHERE
    {
        public static MapAsset MapToLoad;
        public static GameModeState GameMode = GameModeState.Sandbox;
        public static bool quickboot;
        public static bool hotbundle;
        public static void LoadAssets(bool reload)
        {
            LandfallContentDatabase CDB = ContentDatabase.Instance().LandfallContentDatabase;
            string customfolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TMEBundles");
            if (Directory.Exists(customfolder) == false)
            {
                Debug.LogError("No TMEBundles Folder Exists!!!");
                return;
            }
            string[] assetbundles = Directory.GetFiles(customfolder);
            if (assetbundles.Count() < 1)
            {
                Debug.LogError("No Bundles in Folder!!!");
                return;
            }
            foreach (var assetfile in assetbundles)
            {
                if (reload)
                {
                    AssetBundle.UnloadAllAssetBundles(true);
                }
                AssetBundle assetbundle = AssetBundle.LoadFromFile(assetfile);
                //Load all our prefabs in
                List<GameObject> prefabs = assetbundle.LoadAllAssets<GameObject>().ToList();
                //Filter lists out based on their components due to the fact that all of the prefabs are gameobjects and cant be loaded directly as weaponitem for instance
                List<WeaponItem> weapons = prefabs.Where(x => x.GetComponent<WeaponItem>() != null).Select(x => x.GetComponent<WeaponItem>()).ToList();
                List<ProjectileEntity> projectiles = prefabs.Where(x => x.GetComponent<ProjectileEntity>() != null).Select(x => x.GetComponent<ProjectileEntity>()).ToList();
                List<SpecialAbility> combatMoves = prefabs.Where(x => x.GetComponent<SpecialAbility>() != null).Select(x => x.GetComponent<SpecialAbility>()).ToList();
                List<PropItem> props = prefabs.Where(x => x.GetComponent<PropItem>() != null).Select(x => x.GetComponent<PropItem>()).ToList();
                //Load assets that are directly loadable like Faction
                List<UnitBlueprint> blueprints = assetbundle.LoadAllAssets<UnitBlueprint>().ToList();
                List<Faction> factions = assetbundle.LoadAllAssets<Faction>().ToList();
                Dictionary<DatabaseID, UnityEngine.Object> nonStreamableAssets = (Dictionary<DatabaseID, UnityEngine.Object>)typeof(AssetLoader).GetField("m_nonStreamableAssets", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ContentDatabase.Instance().AssetLoader);
                if (weapons.Count > 0)
                {
                    Dictionary<DatabaseID, GameObject> objdict = (Dictionary<DatabaseID, GameObject>)typeof(LandfallContentDatabase).GetField("m_weapons", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CDB);
                    foreach (var obj in weapons)
                    {
                        if (reload)
                        {
                            if (objdict.ContainsKey(obj.Entity.GUID))
                            {
                                objdict.Remove(obj.Entity.GUID);
                            }
                            if (nonStreamableAssets.ContainsKey(obj.Entity.GUID))
                            {
                                nonStreamableAssets.Remove(obj.Entity.GUID);
                            }
                        }
                        objdict.Add(obj.Entity.GUID, obj.gameObject);
                        nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                    }
                    typeof(LandfallContentDatabase).GetField("m_weapons", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(CDB, objdict);
                }
                if (projectiles.Count > 0)
                {
                    Dictionary<DatabaseID, GameObject> objdict = (Dictionary<DatabaseID, GameObject>)typeof(LandfallContentDatabase).GetField("m_projectiles", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CDB);
                    foreach (var obj in projectiles)
                    {
                        if (reload)
                        {
                            if (objdict.ContainsKey(obj.Entity.GUID))
                            {
                                objdict.Remove(obj.Entity.GUID);
                            }
                            if (nonStreamableAssets.ContainsKey(obj.Entity.GUID))
                            {
                                nonStreamableAssets.Remove(obj.Entity.GUID);
                            }
                        }
                        objdict.Add(obj.Entity.GUID, obj.gameObject);
                        nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                    }
                    typeof(LandfallContentDatabase).GetField("m_projectiles", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(CDB, objdict);
                }
                if (weapons.Count > 0)
                {
                    Dictionary<DatabaseID, GameObject> objdict = (Dictionary<DatabaseID, GameObject>)typeof(LandfallContentDatabase).GetField("m_combatMoves", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CDB);
                    foreach (var obj in combatMoves)
                    {
                        if (reload)
                        {
                            if (objdict.ContainsKey(obj.Entity.GUID))
                            {
                                objdict.Remove(obj.Entity.GUID);
                            }
                            if (nonStreamableAssets.ContainsKey(obj.Entity.GUID))
                            {
                                nonStreamableAssets.Remove(obj.Entity.GUID);
                            }
                        }
                        objdict.Add(obj.Entity.GUID, obj.gameObject);
                        nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                    }
                    typeof(LandfallContentDatabase).GetField("m_combatMoves", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(CDB, objdict);
                }
                if (props.Count > 0)
                {
                    Dictionary<DatabaseID, GameObject> objdict = (Dictionary<DatabaseID, GameObject>)typeof(LandfallContentDatabase).GetField("m_characterProps", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CDB);
                    foreach (var obj in props)
                    {
                        if (reload)
                        {
                            if (objdict.ContainsKey(obj.Entity.GUID))
                            {
                                objdict.Remove(obj.Entity.GUID);
                            }
                            if (nonStreamableAssets.ContainsKey(obj.Entity.GUID))
                            {
                                nonStreamableAssets.Remove(obj.Entity.GUID);
                            }
                        }
                        objdict.Add(obj.Entity.GUID, obj.gameObject);
                        nonStreamableAssets.Add(obj.Entity.GUID, obj.gameObject);
                    }
                    typeof(LandfallContentDatabase).GetField("m_characterProps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(CDB, objdict);
                }
                if (blueprints.Count > 0)
                {
                    Dictionary<DatabaseID, UnitBlueprint> objdict = (Dictionary<DatabaseID, UnitBlueprint>)typeof(LandfallContentDatabase).GetField("m_unitBlueprints", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CDB);
                    foreach (var obj in blueprints)
                    {
                        if (reload)
                        {
                            if (objdict.ContainsKey(obj.Entity.GUID))
                            {
                                objdict.Remove(obj.Entity.GUID);
                            }
                            if (nonStreamableAssets.ContainsKey(obj.Entity.GUID))
                            {
                                nonStreamableAssets.Remove(obj.Entity.GUID);
                            }
                        }
                        objdict.Add(obj.Entity.GUID, obj);
                        nonStreamableAssets.Add(obj.Entity.GUID, obj);
                    }
                    typeof(LandfallContentDatabase).GetField("m_unitBlueprints", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(CDB, objdict);
                }
                if (factions.Count > 0)
                {
                    Dictionary<DatabaseID, Faction> objdict = (Dictionary<DatabaseID, Faction>)typeof(LandfallContentDatabase).GetField("m_factions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CDB);
                    foreach (var obj in factions)
                    {
                        if (reload)
                        {
                            if (objdict.ContainsKey(obj.Entity.GUID))
                            {
                                objdict.Remove(obj.Entity.GUID);
                            }
                            if (nonStreamableAssets.ContainsKey(obj.Entity.GUID))
                            {
                                nonStreamableAssets.Remove(obj.Entity.GUID);
                            }
                        }
                        objdict.Add(obj.Entity.GUID, obj);
                        nonStreamableAssets.Add(obj.Entity.GUID, obj);
                    }
                    typeof(LandfallContentDatabase).GetField("m_factions", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(CDB, objdict);
                }
            }
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadAssets(true);
            }

        }
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

using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace PROJECTNAMEHERE
{
    //Attribute that tells bepinex to load this in, be sure to name yours different than the default, and has a unique name, or else it will cause issues!
    [BepInPlugin("org.bepinex.plugins.INSERTMODNAME", "INSERTMODNAME", "1.0.0")]
    class Loader : BaseUnityPlugin
    {
        //Code that creates our patches and adds our main script for executing scripts
        void Awake()
        {
            //Harmony is used for patches, one is provided in TME.cs
            //This method basically finds all of the patches and patches them.
            Harmony harmony = new Harmony("INSERTMODNAME");
            harmony.PatchAll();
            //This creates a gameobject for a main script to run, letting us use methods like awake/etc on our Main.cs class
            var go = new GameObject("INSERTMODNAME");
            //Prevents scene changes from deleting our mod class
            DontDestroyOnLoad(go);
            go.AddComponent<Main>();
            go.AddComponent<TME>();
        }
    }
    }

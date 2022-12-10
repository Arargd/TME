using UnityEngine;

namespace PROJECTNAMEHERE 
{
    public class Main : MonoBehaviour
    {
        public void Awake()
        {
            Debug.Log("Loaded INSERTMODNAME Successfully!");
            //Runs code to load all of our custom content
            TME.LoadAssets(false);
            //Boots into flat simulation for quick testing, set false to disable.
            //Be sure to set this false for your final mod or else everyone will be instantly booted into the map
            TME.quickboot = true;
            //Hotbundle enables you to hot reload assetbundles, letting you make changes without having to restart the game
            //Accessed by pressing F9 in-game
            //Togglable just incase users use F9 for some reason and it lags the game to reload the bundles constantly ((it will also make units suddenly lose items))
            TME.hotbundle = true;
        }

    }
}

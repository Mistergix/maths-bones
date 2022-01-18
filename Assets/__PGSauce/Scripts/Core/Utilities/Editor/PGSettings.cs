using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.Strings;
using UnityEngine;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PGSauce.Core
{
    [CreateAssetMenu(menuName = MenuPaths.Settings + "PGSettings", fileName = "_____________PGSettings")]
    public class PGSettings : SOSingleton<PGSettings>
    {
#if UNITY_EDITOR
        [SerializeField, BoxGroup("Splash Screen")] private Sprite splashscreen;
        [SerializeField, BoxGroup("Splash Screen")] private PlayerSettings.SplashScreen.DrawMode drawMode;
        [SerializeField, BoxGroup("Splash Screen")] private Color splashscreenBackgroundColor = PGColors.Black;
        [SerializeField] private string gameName;
        [SerializeField] private string gameNameInNamespace;

        [ShowInInspector]
        public string CompanyName => "Big Catto";
        
        public string OrganizationId => "3058504";

        public string GameNameInNamespace => gameNameInNamespace.Trim();
        public string GamesNamespace => $"PGSauce.Games.{GameNameInNamespace}";
        public string GameName => gameName;
        public bool HasProLicense => false;
        
        public Sprite Splashscreen => splashscreen;
        public Color SplashscreenBackgroundColor => splashscreenBackgroundColor;
        
        public PlayerSettings.SplashScreen.DrawMode LogoDrawMode => drawMode;
        
        public string DiawiToken => "m23h6YWiAaoP5QL0Z9eiEUNrPWgSqEO3N9FbdLVX9s";
        public string EmailsDiawi => "nicolas.ruche94@gmail.com";

        [Button(ButtonSizes.Gigantic)]
        public static void UpdateProject()
        {
#if UNITY_EDITOR
#endif
        }

#endif
    }
}

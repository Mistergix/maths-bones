// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>A <see cref="ScriptableObject"/> which holds a <see cref="MixerState.Transition2D"/>.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/animancer/docs/manual/transitions/types#transition-assets">Transition Assets</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer/MixerTransition2D
    /// 
    [CreateAssetMenu(menuName = Strings.MenuPrefix + "Mixer Transition/2D", order = Strings.AssetMenuOrder + 3)]
    [HelpURL(Strings.DocsURLs.APIDocumentation + "/" + nameof(MixerTransition2D))]
    public class MixerTransition2D : AnimancerTransition<MixerState.Transition2D> { }
}

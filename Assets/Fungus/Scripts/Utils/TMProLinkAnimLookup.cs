using System.Collections.Generic;
#if UNITY_2018_1_OR_NEWER

namespace Fungus
{
    /// <summary>
    /// Static lookup for Text Mesh Pro Link animations. TMPro tracks and holds information about
    /// link tags in its text body and is recommended as one of the ways to achieve per effects within
    /// a body of text. Giving you the text within the link and the name/hash of the link id itself.
    /// 
    /// Such that {link="shake"}this text will be marked up as within shake link{/link}.
    /// 
    /// By assigning to the LinkHashToEffect dictionary with a key of TMPro.TMP_TextUtilities.GetSimpleHashCode("shake")
    /// and a matching function signature that can then be used the the TMProLinkAnimator.
    /// 
    /// See TMProLinkAnimEffects for sample basis for creating effects.
    /// </summary>
    public static class TMProLinkAnimLookup
    {
        //required signature for all TMProAnim functions for use in the lookup
        public delegate void TMProAnimFunc(TMProLinkAnimator beh, int start, int length);

        //static lookup for all tmpro link style lookups
        //this is where additional effects would be added
        static public Dictionary<int, TMProAnimFunc> LinkHashToEffect = new Dictionary<int, TMProAnimFunc>()
        {
            {TMPro.TMP_TextUtilities.GetSimpleHashCode("shake"),
                new TMProLinkAnimEffects.ShakeEffect()
                {
                    mode = TMProLinkAnimEffects.TMPLinkAnimatorMode.PerLine,
                    offsetScale = 2,
                    rotScale = 15
                }.DoEffect },
            {TMPro.TMP_TextUtilities.GetSimpleHashCode("wiggle"),
                new TMProLinkAnimEffects.WiggleEffect()
                {
                    mode = TMProLinkAnimEffects.TMPLinkAnimatorMode.PerSection,
                    scale = 5
                }.DoEffect },
            {TMPro.TMP_TextUtilities.GetSimpleHashCode("wave"),
                new TMProLinkAnimEffects.WaveEffect()
                {
                    mode = TMProLinkAnimEffects.TMPLinkAnimatorMode.PerCharacter,
                    speed = 10,
                    indexStep = 0.3f,
                    scale = 2
                }.DoEffect },
            {TMPro.TMP_TextUtilities.GetSimpleHashCode("pivot"),
                new TMProLinkAnimEffects.PivotEffect()
                {
                    mode = TMProLinkAnimEffects.TMPLinkAnimatorMode.PerWord,
                    speed = 10,
                    degScale = 15
                }.DoEffect
            },
            {TMPro.TMP_TextUtilities.GetSimpleHashCode("rainbow"),
                new TMProLinkAnimEffects.RainbowEffect()
                {
                    mode = TMProLinkAnimEffects.TMPLinkAnimatorMode.PerCharacter,
                    speed = 2,
                    indexStep = 0.1f,
                    s = 0.8f,
                    v = 0.8f
                }.DoEffect
            },

        };
    }
}
#endif
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Enumeration for Harmony Search type
    /// </summary>
    public enum HarmonySearchType
    {
        Standard,

        [Display(Name = "IHS")]
        Improved,

        [Display(Name = "SubHM")]
        Divided,

        [Display(Name = "PSL")]
        Dynamic,

        [Display(Name = "IHS + SubHM")]
        ImprovedDivided,

        [Display(Name = "PSL + SubHM")]
        DynamicDivided,

        [Display(Name = "HS + ACO")]
        AntColony
    }
}
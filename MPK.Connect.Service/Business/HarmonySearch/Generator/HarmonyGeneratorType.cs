using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Enumeration for harmony generator
    /// </summary>
    public enum HarmonyGeneratorType
    {
        [Display(Name = "W pełni losowe")]
        RandomStopTime,

        [Display(Name = "Kierowane")]
        RandomDirectedStop,

        [Display(Name = "W części losowe")]
        RandomStop
    }
}
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    /// <summary>
    /// Enumeration for objective function type
    /// </summary>
    public enum ObjectiveFunctionType
    {
        [Display(Name = "Czas przejazdu")]
        TravelTime,

        [Display(Name = "Liczba przesiadek")]
        Transfers,

        [Display(Name = "Mieszana")]
        Comprehensive
    }
}
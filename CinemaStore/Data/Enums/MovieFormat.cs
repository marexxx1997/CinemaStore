using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Data.Enums
{
    public enum MovieFormat
    {
        [Display(Name = "2D")]
        TwoD = 1,
        [Display(Name = "3D")]
        ThreeD,
        [Display(Name = "4D")]
        FourD,
        ScreenX
    }
}

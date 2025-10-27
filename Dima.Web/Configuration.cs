using MudBlazor;

namespace Dima.Web;

public static class Configuration
{

    public static string HttpClientName = "dima";
    public static string BackendUrl { get; set; } = "http://localhost:45";

    public static MudTheme Theme = new()
    {
        Typography = new Typography()
        {
            Default = new Body1Typography()
            {
                FontFamily = ["Raleway", "sans-serif"]
            }
        },
        PaletteLight = new PaletteLight()
        {
            Primary = new MudBlazor.Utilities.MudColor("#1EFA2D"),
            PrimaryContrastText = new MudBlazor.Utilities.MudColor("#000000"),
            Secondary = Colors.LightGreen.Darken3,
            Background = Colors.Gray.Lighten4,
            AppbarBackground = new MudBlazor.Utilities.MudColor("#1EFA2D"),
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.White,
            DrawerBackground = Colors.LightGreen.Darken4
        },
        PaletteDark = new PaletteDark()
        {
            Primary = Colors.LightGreen.Accent3,
            Secondary = Colors.LightGreen.Darken3,
            AppbarBackground = Colors.LightGreen.Accent3,
            AppbarText = Colors.Shades.Black,
            PrimaryContrastText = new MudBlazor.Utilities.MudColor("#000000")
        }
    };

}

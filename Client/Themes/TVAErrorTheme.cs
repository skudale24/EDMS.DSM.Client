using MudBlazor.Utilities;

namespace EDMS.DSM.Client.Themes;

public class TVAErrorTheme : MudTheme
{
    public TVAErrorTheme()
    {
        Palette = new PaletteLight
        {
            Primary = "#3F51B5",
            PrimaryDarken = Colors.Amber.Darken4,
            Secondary = "#2c91991a", //Colors.BlueGrey.Darken4,
            Background = "#E8E4E1", //Colors.BlueGrey.Lighten5,
            Divider = Colors.BlueGrey.Lighten1,
            OverlayLight = "#fff",
            PrimaryContrastText = Colors.DeepOrange.Lighten1,
            PrimaryLighten = Colors.DeepOrange.Lighten1,
            DarkLighten = Colors.DeepOrange.Lighten1,
            DarkContrastText = Colors.DeepOrange.Lighten1,
            HoverOpacity = .5,
            LinesDefault = Colors.BlueGrey.Lighten5,
            AppbarBackground = "#0072CF",
            DrawerBackground = "#FFF",
            DrawerText = "#262626",
            TableLines = "#BBC1DA",
            TableStriped = "#2c91991a",
            TableHover = "#2c91994d",
            Success = "rgba(106, 144, 51, 0.2)",
            Info = "#2C9199", //rgba(68, 125, 174, 0.2)",
            Warning = "rgba(229, 96, 39, 0.2)",
            Error = "rgba(255, 80, 80, 0.2)",
            SuccessContrastText = "rgb(0, 58, 112)",
            InfoContrastText = "rgb(0, 58, 112)",
            WarningContrastText = "rgb(0, 58, 112)",
            ErrorContrastText = "rgb(0, 58, 112)",
        };

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "3px"
        };

        Typography = new Typography
        {
            Default =
                new Default
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica" },
                    FontSize = "12px",
                    FontWeight = 400,
                    LineHeight = 1.43,
                    LetterSpacing = ".01071em"
                },
            H1 =
                new H1
                {
                    FontFamily = new[] { "Helvetica Neue Bold", "Helvetica", "Arial Bold" },
                    FontSize = "38px",
                    FontWeight = 500,
                    LineHeight = 1.167,
                    LetterSpacing = "-.01562em"
                },
            H2 =
                new H2
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica", "Arial Regular" },
                    FontSize = "34px",
                    FontWeight = 500,
                    LineHeight = 1.2,
                    LetterSpacing = "-.00833em"
                },
            H3 =
                new H3
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica", "Arial Regular" },
                    FontSize = "30px",
                    FontWeight = 500,
                    LineHeight = 1.167,
                    LetterSpacing = "0"
                },
            H4 =
                new H4
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica", "Arial Regular" },
                    FontSize = "28px",
                    FontWeight = 500,
                    LineHeight = 1.235,
                    LetterSpacing = ".00735em"
                },
            H5 =
                new H5
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica", "Arial Regular" },
                    FontSize = "26px",
                    FontWeight = 500,
                    LineHeight = 1.334,
                    LetterSpacing = "0"
                },
            H6 =
                new H6
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica" },
                    FontSize = ".75rem",
                    FontWeight = 600,
                    LineHeight = 1.1,
                    LetterSpacing = ".0075em"
                },
            Button =
                new Button
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica", "Arial Regular" },
                    FontSize = ".75rem",
                    FontWeight = 500,
                    LineHeight = 1.75,
                    LetterSpacing = ".02857em"
                },
            Body1 =
                new Body1
                {
                    FontFamily = new[] { "Helvetica Neue", "Helvetica" },
                    FontSize = ".75rem",
                    FontWeight = 400,
                    LineHeight = 1.5,
                    LetterSpacing = ".00938em"
                },
            Body2 = new Body2
            {
                FontFamily = new[] { "Helvetica Neue", "Helvetica" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption
            {
                FontFamily = new[] { "Helvetica Neue", "Helvetica", "Arial Regular" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2
            {
                FontFamily = new[] { "Helvetica Neue Bold", "Helvetica", "Arial Regular" },
                FontSize = ".75rem",
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            }
        };
        Shadows = new Shadow();
        ZIndex = new ZIndex();
    }
}

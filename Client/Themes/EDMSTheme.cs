namespace EDMS.DSM.Client.Themes;

public class EDMSTheme : MudTheme
{
    public EDMSTheme()
    {
        Palette = new PaletteLight
        {
            Primary = "#F18C27",
            PrimaryDarken = "#D4710D",
            Secondary = "#0770B4",
            Background = Colors.BlueGrey.Lighten5,
            Divider = Colors.BlueGrey.Lighten1,
            TableHover = Colors.BlueGrey.Lighten5,
            TableLines = Colors.BlueGrey.Lighten5,
            LinesDefault = Colors.BlueGrey.Lighten5,
            AppbarBackground = "#0770B4",
            DrawerBackground = "#FFF",
            DrawerText = "#262626",
            Success = "#06d79c"
        };

        LayoutProperties = new LayoutProperties { DefaultBorderRadius = "6px" };

        Typography = new Typography
        {
            Default =
                new Default
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "14px",
                    FontWeight = 400,
                    LineHeight = 1.43,
                    LetterSpacing = ".01071em"
                },
            H1 =
                new H1
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1.6rem",
                    FontWeight = 500,
                    LineHeight = 1.167,
                    LetterSpacing = "-.01562em"
                },
            H2 =
                new H2
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1.6rem",
                    FontWeight = 500,
                    LineHeight = 1.2,
                    LetterSpacing = "-.00833em"
                },
            H3 =
                new H3
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1.4rem",
                    FontWeight = 500,
                    LineHeight = 1.167,
                    LetterSpacing = "0"
                },
            H4 =
                new H4
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1.3rem",
                    FontWeight = 500,
                    LineHeight = 1.135,
                    LetterSpacing = ".00735em"
                },
            H5 =
                new H5
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1.2rem",
                    FontWeight = 500,
                    LineHeight = 1.334,
                    LetterSpacing = "0"
                },
            H6 =
                new H6
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = 600,
                    LineHeight = 1.1,
                    LetterSpacing = ".0075em"
                },
            Button =
                new Button
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = 500,
                    LineHeight = 1.75,
                    LetterSpacing = ".02857em"
                },
            Body1 =
                new Body1
                {
                    FontFamily = new[] { "Poppins", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = 400,
                    LineHeight = 1.5,
                    LetterSpacing = ".00938em"
                },
            Body2 = new Body2
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2
            {
                FontFamily = new[] { "Poppins", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            }
        };
        Shadows = new Shadow();
        ZIndex = new ZIndex();
    }
}

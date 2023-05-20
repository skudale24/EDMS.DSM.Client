﻿namespace EDMS.DSM.Client.Themes;

public class TVATheme : MudTheme
{
    public TVATheme()
    {
        Palette = new PaletteLight
        {
            Primary = "#3d49f5",
            PrimaryDarken = Colors.Amber.Darken4,
            Secondary = Colors.BlueGrey.Darken4,
            Background = Colors.BlueGrey.Lighten5,
            Divider = Colors.BlueGrey.Lighten1,
            TableHover = "#D7D2CB", //"#8CB8D6", //Colors.BlueGrey.Lighten5,
            TableLines = "#BBC1DA",
            LinesDefault = Colors.BlueGrey.Lighten5,
            AppbarBackground = "#0072CF",
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
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "12px",
                    FontWeight = 400,
                    LineHeight = 1.43,
                    LetterSpacing = ".01071em"
                },
            H1 =
                new H1
                {
                    FontFamily = new[] { "Helvetica Neue Bold", "Arial Bold" },
                    FontSize = "38px",
                    FontWeight = 500,
                    LineHeight = 1.167,
                    LetterSpacing = "-.01562em"
                },
            H2 =
                new H2
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "34px",
                    FontWeight = 500,
                    LineHeight = 1.2,
                    LetterSpacing = "-.00833em"
                },
            H3 =
                new H3
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "30px",
                    FontWeight = 500,
                    LineHeight = 1.167,
                    LetterSpacing = "0"
                },
            H4 =
                new H4
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "28px",
                    FontWeight = 500,
                    LineHeight = 1.235,
                    LetterSpacing = ".00735em"
                },
            H5 =
                new H5
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "26px",
                    FontWeight = 500,
                    LineHeight = 1.334,
                    LetterSpacing = "0"
                },
            H6 =
                new H6
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "1rem",
                    FontWeight = 600,
                    LineHeight = 1.1,
                    LetterSpacing = ".0075em"
                },
            Button =
                new Button
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = ".75rem",
                    FontWeight = 500,
                    LineHeight = 1.75,
                    LetterSpacing = ".02857em"
                },
            Body1 =
                new Body1
                {
                    FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                    FontSize = "1rem",
                    FontWeight = 400,
                    LineHeight = 1.5,
                    LetterSpacing = ".00938em"
                },
            Body2 = new Body2
            {
                FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption
            {
                FontFamily = new[] { "Helvetica Neue", "Arial Regular" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2
            {
                FontFamily = new[] { "Helvetica Neue Bold", "Arial Regular" },
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

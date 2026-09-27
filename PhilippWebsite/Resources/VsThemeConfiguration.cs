using MudBlazor;


namespace PhilippWebsite.Resources
{
    public static class VsThemeConfiguration
    {
        public static MudTheme GetTheme()
        {
            return new MudTheme()
            {
                Typography = new Typography()
                {
                    Default = new DefaultTypography()
                    {
                        FontFamily = new[] { "Segoe UI", "-apple-system", "sans-serif" },
                        //FontWeight = "300",
                    },
                },

                PaletteDark = new PaletteDark()
                {
                    // Фирменный фиолетовый акцент Visual Studio (кнопки, активные элементы)
                    Primary = "#9184EE",

                    // Рабочая область редактора кода (Editor Background)
                    Background = "#1e1e1e",

                    // Панели инструментов, Solution Explorer, вкладки (Tool Windows / Surfaces)
                    Surface = "#252526",

                    // Фон верхней панели меню / заголовка окна
                    AppbarBackground = "#2d2d30",
                    AppbarText = "#cccccc",

                    // Боковые панели (Explorer / Solution View)
                    DrawerBackground = "#252526",
                    DrawerText = "#cccccc",
                    DrawerIcon = "#858585",

                    // Текст (дефолтный светлый текст кода и приглушенные подписи)
                    TextPrimary = "#d4d4d4",
                    TextSecondary = "#858585",
                    TextDisabled = "#5a5a5a",

                    // Интерактивные иконки и действия
                    ActionDefault = "#c5c5c5",
                    ActionDisabled = "#4d4d4d",
                    ActionDisabledBackground = "#2d2d30",

                    // Разделители окон и границы табов (VS Border Line)
                    LinesDefault = "#3e3e42",
                    TableLines = "#3e3e42",
                    Divider = "#3e3e42",

                    // Системные статусы Visual Studio (Error List / Output)
                    Info = "#75beff",       // VS Blue (линки, информационные токены)
                    Success = "#4ec9b0",    // VS Teal (типы/классы C#, признак успешной сборки)
                    Warning = "#cca700",    // VS Warning Yellow
                    Error = "#f14c4c",      // VS Error Red

                    // Вспомогательные серые оттенки
                    BackgroundGray = "#2a2a2d",
                    GrayLight = "#333337",
                    GrayLighter = "#252526",
                    OverlayLight = "rgba(30, 30, 30, 0.6)"
                }
            };
        }
    }
}

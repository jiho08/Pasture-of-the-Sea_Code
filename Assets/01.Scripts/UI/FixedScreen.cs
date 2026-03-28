using UnityEngine;

namespace Code.UI
{
    public class FixedScreen
    {
        public static int ScreenWidth { get; set; } = 1920;
        public static int ScreenHeight { get; set; } = 1080;
        public static bool FullScreen { get; set; } = true;

        public static void FixedScreenSet() => fixedScreenSet(ScreenWidth, ScreenHeight, FullScreen);
        public static void FixedScreenSet(int Width, int Height) => fixedScreenSet(Width, Height, FullScreen);
        public static void FixedScreenSet(int Width, int Height, bool fullScreen) => fixedScreenSet(Width, Height, fullScreen);

        private static void fixedScreenSet(int Width, int Height, bool fullScreen)
        {
            Screen.SetResolution(Width, Height, fullScreen);
        }
    }
}
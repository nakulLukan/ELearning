using Microsoft.Maui.Platform;

namespace Learning.App.Controls;

/// <summary>
/// Global state controller for all controls
/// </summary>
public static class AppViewStateController
{
    public static ITextInput LastActiveTextInput { get; set; } = null;

    /// <summary>
    /// Hides soft keyboard
    /// </summary>
    public static async Task HideSoftKeyboard()
    {
        if (LastActiveTextInput != null)
        {
            if (CommunityToolkit.Maui.Core.Platform.KeyboardExtensions.IsSoftKeyboardShowing(LastActiveTextInput))
            {
                await CommunityToolkit.Maui.Core.Platform.KeyboardExtensions.HideKeyboardAsync(LastActiveTextInput);
            }
        }
    }

    /// <summary>
    /// This disables the platform scroll when keyboard is shown
    /// </summary>
    public static void DisableKeyboardScroll()
    {
#if IOS
        KeyboardAutoManagerScroll.Disconnect();
#endif
    }

    /// <summary>
    /// Enables keyboard scroll
    /// </summary>
    public static void EnableKeyboardScroll()
    {
#if IOS
        KeyboardAutoManagerScroll.Connect();
#endif
    }
}

namespace Learning.App.Controls.AppInputs;

/// <summary>
/// Custom entry that can be used for the application with extra properties which are not present by default in the <see cref="Entry"/>
/// </summary>
public class AppEditorBase : Editor
{
    public Thickness Padding { get; set; }
}

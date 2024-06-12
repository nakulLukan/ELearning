namespace Learning.Core.Exceptions;

public class UIException : Exception
{
    private readonly string _controlName;
    private readonly string _message;
    public UIException(string controlName, string message)
    {
        _controlName = controlName;
        _message = message;
    }

    public override string Message => $"[{_controlName}]: {_message}; {base.Message}";
}

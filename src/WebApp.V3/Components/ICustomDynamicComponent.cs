namespace WebApp.V3.Components;

public interface ICustomDynamicComponent
{
    public object GetValue();

    public void SetValue(object data);
}
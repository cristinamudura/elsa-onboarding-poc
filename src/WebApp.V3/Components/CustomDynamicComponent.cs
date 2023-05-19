using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace WebApp.V3.Components;

public class CustomDynamicComponent<TType> : ComponentBase, ICustomDynamicComponent
{
    [Parameter] public TType Value { get; set; }

    public object GetValue()
    {
        return Value;
    }

    public void SetValue(object data)
    {
        if (data != null)
        {
            if (typeof(TType) == typeof(string))
            {
                Value = (TType) (object) data.ToString();
            }
            else
            {
                Value = JsonConvert.DeserializeObject<TType>(data.ToString());
            }
        }
        StateHasChanged();
    }
}
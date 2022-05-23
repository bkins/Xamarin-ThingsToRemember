using ThingsToRemember.Models;
using Xamarin.Forms;

namespace ThingsToRemember.Views.Helpers;

public class UrlTemplateSelector : DataTemplateSelector
{
    public DataTemplate ImageTemplate { get; set; }
    public DataTemplate VideoTemplate { get; set; }
    public DataTemplate OtherTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var model = (Media)item;

        return model.Type switch
        {
            MediaType.Video => VideoTemplate
          , MediaType.Image => ImageTemplate
          , _ => OtherTemplate
        };
    }
}
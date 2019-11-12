using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeiPo.Services.Models;

namespace WeiPo.Common
{
    public class StatusTypeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? StatusTemplate { get; set; }
        public DataTemplate? CommentTemplate { get; set; }
        public DataTemplate? AttitudeTemplate { get; set; }
        public DataTemplate? MessageListTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectTemplateCore(item, null);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject? container)
        {
            return item switch
            {
                StatusModel status => StatusTemplate,
                CommentModel comment => CommentTemplate,
                AttitudeModel attitude => AttitudeTemplate,
                MessageListModel message => MessageListTemplate,
                _ => new DataTemplate()
            } ?? new DataTemplate();
        }
    }
}
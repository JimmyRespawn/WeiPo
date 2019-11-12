using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WeiPo.Common
{
    public class XamlExtensions : DependencyObject
    {
        public static readonly DependencyProperty TappedMessageParameterProperty = DependencyProperty.Register(
            "TappedMessageParameter", typeof(object), typeof(XamlExtensions), new PropertyMetadata(default));

        public static readonly DependencyProperty TappedMessageProperty = DependencyProperty.Register(
            "TappedMessage", typeof(string), typeof(XamlExtensions), new PropertyMetadata(default(string)));

        public static void SetTappedMessage(UIElement element, string value)
        {
            element.SetValue(TappedMessageProperty, value);
            element.Tapped += delegate(object sender, TappedRoutedEventArgs args)
            {
                if (sender is UIElement element)
                {
                    args.Handled = true;
                    Singleton<BroadcastCenter>.Instance.Send(sender, GetTappedMessage(element), GetTappedMessageParameter(element));
                }
            };
        }

        public static string GetTappedMessage(UIElement element)
        {
            return (string) element.GetValue(TappedMessageProperty);
        }

        public static void SetTappedMessageParameter(UIElement element, object value)
        {
            element.SetValue(TappedMessageParameterProperty, value);
        }

        public static object GetTappedMessageParameter(UIElement element)
        {
            return element.GetValue(TappedMessageParameterProperty);
        }
    }
}
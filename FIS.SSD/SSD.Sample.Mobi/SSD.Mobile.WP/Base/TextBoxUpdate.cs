using System;
using System.Net;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SSD.Mobile.WP
{
    public class TextBoxUpdate : TextBox
    {
        public TextBoxUpdate()
        {
            TextChanged += OnTextBoxTextChanged;
        }
        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox senderText = (TextBox)sender;
            BindingExpression bindingExp = senderText.GetBindingExpression(TextBox.TextProperty);
            bindingExp.UpdateSource();
        }
    }
}


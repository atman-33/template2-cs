using MS.WindowsAPICodePack.Internal;
using System.Windows;

namespace Template2.WPF.Services
{
    public sealed class MessageService : IMessageService
    {
        public MessageBoxResult Question(string message)
        {
            return MessageBox.Show(message, "問い合わせ", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public MessageBoxResult Warning(string message)
        {
            return MessageBox.Show(message, "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        }

        public MessageBoxResult Error(string message)
        {
            return MessageBox.Show(message, "エラー", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }

        public void ShowDialog(string message)
        {
            MessageBox.Show(message); 
        }
        public void ShowDialog(string message, string caption, MessageBoxButton messageBoxButton,MessageBoxImage messageBoxImage)
        {
            MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
        }
    }
}

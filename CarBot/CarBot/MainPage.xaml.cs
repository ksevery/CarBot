using CarBot.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.ConversationalUI;
using Xamarin.Forms;

namespace CarBot
{
    public partial class MainPage : ContentPage
    {
        private BotService botService;
        private Author botAuthor;

        public MainPage()
        {
            InitializeComponent();

            this.botService = new BotService();
            this.botService.AttachOnReceiveMessage(this.OnBotMessageReceived);
            this.botAuthor = new Author { Name = "botty" };

            ((INotifyCollectionChanged)this.chat.Items).CollectionChanged += ChatItems_CollectionChanged;
        }

        private void OnBotMessageReceived(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TextMessage textMessage = new TextMessage();
                textMessage.Data = message;
                textMessage.Author = this.botAuthor;
                textMessage.Text = message;
                chat.Items.Add(textMessage);
            });
        }

        private async void ChatItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TextMessage chatMessage = (TextMessage)e.NewItems[0];
                if (chatMessage.Author == chat.Author)
                {
                    await this.botService.SendToBot(chatMessage.Text);
                }
            }
        }
    }
}

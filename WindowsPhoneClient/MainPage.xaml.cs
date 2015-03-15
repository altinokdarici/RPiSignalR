using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WindowsPhoneClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HubConnection hubConnection;
        private SpeechRecognizer speechRecog = new SpeechRecognizer();
        private IHubProxy messageProxy;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            hubConnection = new HubConnection("http://iotdemorpi.azurewebsites.net/signalr");
            messageProxy = hubConnection.CreateHubProxy("MessageHub");
            hubConnection.Start().Wait();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await messageProxy.Invoke("Send", TextBoxPin.Text, ToggleSwitchStatus.IsOn ? 255 : 0);
        }

        private async void ButtonSTT_Click(object sender, RoutedEventArgs e)
        {
            // Compile the dictation grammar
            await speechRecog.CompileConstraintsAsync();
            // Start Recognition
            SpeechRecognitionResult speechRecognitionResult = await this.speechRecog.RecognizeWithUIAsync();
            if (speechRecognitionResult.Text.ToLower().Contains("turn on the light"))
            {
                await messageProxy.Invoke("Send", "0", 255);
            }

            else if (speechRecognitionResult.Text.ToLower().Contains("turn off the light"))
            {
                await messageProxy.Invoke("Send", "0", 0);

            }
        }
    }
}

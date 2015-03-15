using Microsoft.AspNet.SignalR.Client;
using System;
using System.Dynamic;
using System.Threading;
using WiringPi;

namespace RPiClient
{
    class MainClass
    {
        static ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
			if (WiringPi.Init.WiringPiSetup() != -1)
            {
                var hubConnection = new HubConnection("http://iotdemorpi.azurewebsites.net/signalr");
                var messageProxy = hubConnection.CreateHubProxy("MessageHub");
                hubConnection.Start().Wait();
                Console.WriteLine("Started");
                messageProxy.On<Model>("messageArrived", message =>
                {
					if(message.Value==0)
					{
						SoftPwm.Stop(message.Pin);
					}
					else
					{
						SoftPwm.Create(message.Pin, 0, 100);
						Thread.Sleep(500);
						SoftPwm.Write(message.Pin, message.Value);
					}
                    
                
                    Console.WriteLine(message.ToString());
					Thread.Sleep(1000);
                });

                CompletedEvent.WaitOne();
            }
            else
            {
                Console.WriteLine("WiringPiSetup failed");
            }
            Console.ReadLine();
		}
	}


public class Model
{
	public int Pin { get; set; }
	public int Value { get; set; }
	public override string ToString()
        {
            return string.Format("Pin:{0},Value:{1}", Pin, Value);
        }
    }
}

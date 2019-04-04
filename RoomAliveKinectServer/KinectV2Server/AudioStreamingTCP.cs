using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HoloGroup.Networking.Internal.Sockets;


namespace KinectV2Server
{
    public class AudioStreamingTCP : ICustomTCP
    {
        public static AudioStreamingTCP Instance;
        private Queue<Byte[]> packageQueue = new Queue<byte[]>();


        public AudioStreamingTCP()
        {
            Instance = this;
        }

        public void AddPackage(byte[] package)
        {
            packageQueue.Enqueue(package);
        }




        public void OnSocketStartingError(Exception e) { }
        public void OnSocketListenerStartingError(Exception e) { }
        public void OnListenerOpened(bool successfully) { }


        public bool ListenerTransferingProcess(NetworkStream networkStream)
        {
            while(true)
            {
                if(packageQueue.Count == 0)
                {
                    Thread.Sleep(50);
                }
                else
                {
                    while (packageQueue.Count > 0)
                    {
                        var buffer = packageQueue.Dequeue();
                        networkStream.Write(buffer, 0, buffer.Length);
                        networkStream.FlushAsync();
                    }
                }
            }


            return true;
        }

        public async Task SocketTransferingProcess(bool connectSuccessful, NetworkStream networkStream = null)
        {
            //if (!connectSuccessful) throw new ApplicationException("Can not connect by TCP");
            //await ProjectSynchronizationManager.Instance.NetworkProvider.SendingProjectProcess(networkStream);
            throw new NotImplementedException();
        }
    }
}

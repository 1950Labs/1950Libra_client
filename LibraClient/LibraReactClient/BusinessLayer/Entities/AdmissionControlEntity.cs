using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AdmissionControl.AdmissionControl;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class AdmissionControlEntity
    {
        public AdmissionControlClient Client { get; set; }

        public AdmissionControlEntity()
        {
            Channel channel = new Channel("ac.testnet.libra.org:8000", ChannelCredentials.Insecure);
            Client = new AdmissionControl.AdmissionControl.AdmissionControlClient(channel);
        }
    }
}

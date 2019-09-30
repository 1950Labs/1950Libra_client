
namespace LibraReactClient.BusinessLayer.LCSTypes
{
    public class SignedTransactionLCS
    {
        public byte[] Signature { get; internal set; }
        public byte[] Key { get; internal set; }
        public RawTransactionLCS RawTransaction { get; internal set; }
    }
}

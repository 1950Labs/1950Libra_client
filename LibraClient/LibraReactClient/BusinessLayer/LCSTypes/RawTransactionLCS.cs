using System;

namespace LibraReactClient.BusinessLayer.LCSTypes
{
    public class RawTransactionLCS
    {
        public ulong VersionId { get; set; }

        public ulong MaxGasAmount { get; internal set; }
        public ulong GasUnitPrice { get; internal set; }
        public ulong ExpirationTime { get; internal set; }
        public AddressLCS Sender { get; internal set; }
        public ulong SequenceNumber { get; internal set; }
        public TransactionPayloadLCS TransactionPayload { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("sender: {0},{1}", Sender, Environment.NewLine);
            retStr +=
                string.Format("sequence_number: {0},{1}", SequenceNumber, Environment.NewLine);
            retStr +=
                string.Format("payload: {0},{1}", TransactionPayload, Environment.NewLine);
            retStr +=
                string.Format("max_gas_amount: {0},{1}", MaxGasAmount, Environment.NewLine);
            retStr +=
               string.Format("gas_unit_price: {0},{1}", GasUnitPrice, Environment.NewLine);
            retStr +=
               string.Format("expiration_time: {0} seconds", ExpirationTime) +
               "}";
            return retStr;
        }
    }
}

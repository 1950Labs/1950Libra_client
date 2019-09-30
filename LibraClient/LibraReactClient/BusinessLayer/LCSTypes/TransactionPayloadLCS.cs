using LibraReactClient.BusinessLayer.Enums;
using System;

namespace LibraReactClient.BusinessLayer.LCSTypes
{
    public class TransactionPayloadLCS
    {
        public uint PayloadType { get; set; }

        public TransactionPayloadLCSEnum PayloadTypeEnum
        {
            get
            {
                return (TransactionPayloadLCSEnum)PayloadType;
            }
        }

        public ProgramLCS Program { get; set; }
        public WriteSetLCS WriteSet { get; set; }
        public ScriptLCS Script { get; internal set; }
        public ModuleLCS Module { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("PayloadType = {0},{1}", PayloadTypeEnum, Environment.NewLine);

            if (PayloadTypeEnum == TransactionPayloadLCSEnum.Program)
            {
                retStr += string.Format(" Program = {0},", Program);
            }
            else if (PayloadTypeEnum == TransactionPayloadLCSEnum.WriteSet)
            {
                retStr += string.Format(" WriteSet = {0},", WriteSet);
            }
            retStr += "}";
            return retStr;
        }
    }
}
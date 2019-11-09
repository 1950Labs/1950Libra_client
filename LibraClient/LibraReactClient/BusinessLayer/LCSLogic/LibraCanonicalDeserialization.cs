using LibraReactClient.BusinessLayer.Common;
using LibraReactClient.BusinessLayer.Enums;
using LibraReactClient.BusinessLayer.LCSTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraReactClient.BusinessLayer.LCSLogic
{
    public class LibraCanonicalSerialization
    {
        const int ADDRESS_LENGTH = 32;

        public AccountResourceLCS GetAccountResource(byte[] source, ref int cursor)
        {
            var retVal = new AccountResourceLCS();

            retVal.AuthenticationKey = source.LCSDeserialization<AddressLCS>(ref cursor);
            retVal.Balance = Read_u64(source, ref cursor);
            retVal.DelegatedWithdrawalCapability = source.LCSDeserialization<bool>(ref cursor);
            retVal.ReceivedEvents = source.LCSDeserialization<byte[]>(ref cursor);
            retVal.SentEvents = source.LCSDeserialization<byte[]>(ref cursor);
            retVal.SequenceNumber = Read_u64(source, ref cursor);

            return retVal;
        }

        public RawTransactionLCS GetRawTransaction(byte[] source, ref int cursor)
        {
            var retVal = new RawTransactionLCS();

            retVal.Sender = source.LCSDeserialization<AddressLCS>(ref cursor);
            retVal.SequenceNumber = source.LCSDeserialization<ulong>(ref cursor);
            retVal.TransactionPayload =
                source.LCSDeserialization<TransactionPayloadLCS>(ref cursor);
            retVal.MaxGasAmount = Read_u64(source, ref cursor);
            retVal.GasUnitPrice = Read_u64(source, ref cursor);
            retVal.ExpirationTime = Read_u64(source, ref cursor);

            return retVal;
        }

        public SignedTransactionLCS GetSignedTransaction(byte[] source, ref int cursor)
        {
            var retVal = new SignedTransactionLCS();

            retVal.Signature = GetByteArray(source, ref cursor);
            retVal.Key = GetByteArray(source, ref cursor);
            retVal.RawTransaction = GetRawTransaction(source, ref cursor);

            //retVal.SequenceNumber = source.LCSDeserialization<ulong>(ref cursor);
            //retVal.TransactionPayload =
            //    source.LCSDeserialization<TransactionPayloadLCS>(ref cursor);
            //retVal.MaxGasAmount = Read_u64(source, ref cursor);
            //retVal.GasUnitPrice = Read_u64(source, ref cursor);
            //retVal.ExpirationTime = Read_u64(source, ref cursor);

            return retVal;
        }

        public WriteOpLCS GetWriteOp(byte[] source, ref int cursor)
        {
            var retVal = new WriteOpLCS();
            retVal.WriteOpType = source.LCSDeserialization<uint>(ref cursor);

            if (retVal.WriteOpTypeEnum == WriteOpLCSEnum.Value)
                retVal.Value = source.LCSDeserialization<byte[]>(ref cursor);

            return retVal;
        }

        public AccessPathLCS GetAccessPath(byte[] source, ref int cursor)
        {
            var retVal = new AccessPathLCS();

            retVal.Address = source.LCSDeserialization<AddressLCS>(ref cursor);
            retVal.Path = source.LCSDeserialization<byte[]>(ref cursor);

            return retVal;
        }

        public AccountEventLCS GetAccountEvent(byte[] source, ref int cursor)
        {
            var retVal = new AccountEventLCS();

            retVal.Amount = source.LCSDeserialization<ulong>(ref cursor);
            retVal.Account = source.LCSDeserialization<byte[]>(ref cursor)
                .ByteArryToString();

            return retVal;
        }

        public WriteSetLCS GetWriteSet(byte[] source, ref int cursor)
        {
            var retVal = new WriteSetLCS();
            retVal.WriteSet = new Dictionary<AccessPathLCS, WriteOpLCS>();
            retVal.Length = Read_u32(source, ref cursor);

            for (int i = 0; i < retVal.Length; i++)
            {
                var key = source.LCSDeserialization<AccessPathLCS>(ref cursor);
                var value = source.LCSDeserialization<WriteOpLCS>(ref cursor);

                retVal.WriteSet.Add(key, value);
            }
            return retVal;
        }

        public bool GetBool(byte[] source, ref int cursor)
        {
            return Convert.ToBoolean(Read_u8(source, ref cursor, 1).FirstOrDefault());
        }

        public string GetString(byte[] source, ref int cursor)
        {
            var length = Read_u32(source, ref cursor);
            return Read_String(source, ref cursor, (int)length);
        }

        public IEnumerable<byte[]> GetListByteArrays(byte[] source, ref int cursor)
        {
            List<byte[]> retVal = new List<byte[]>();
            var length = Read_u32(source, ref cursor);

            for (int i = 0; i < length; i++)
                retVal.Add(source.LCSDeserialization<byte[]>(ref cursor));

            return retVal;
        }

        public byte[] GetByteArray(byte[] source, ref int cursor)
        {
            var length = Read_u32(source, ref cursor);

            var retVal = Read_u8(source, ref cursor, (int)length)
                .ToArray();

            return retVal;
        }

        public byte GetByte(byte[] source, ref int cursor)
        {
            return Read_u8(source, ref cursor, 1).FirstOrDefault();
        }

        public AddressLCS GetAddress(byte[] source, ref int cursor)
        {
            var retVal = new AddressLCS();
            retVal.Length = 32;

            retVal.ValueByte = Read_u8(source, ref cursor, (int)retVal.Length)
                .ToArray();
            retVal.Value = retVal.ValueByte
                .ByteArryToString();

            return retVal;
        }

        public ulong GetU64(byte[] source, ref int cursor)
        {
            return Read_u64(source, ref cursor);
        }

        public uint GetU32(byte[] source, ref int cursor)
        {
            return Read_u32(source, ref cursor);
        }

        public TransactionPayloadLCS GetTransactionPayload(byte[] source, ref int cursor)
        {
            var retVal = new TransactionPayloadLCS();
            retVal.PayloadType = Read_u32(source, ref cursor);

            if (retVal.PayloadTypeEnum == TransactionPayloadLCSEnum.Program)
                retVal.Program = source.LCSDeserialization<ProgramLCS>(ref cursor);
            else if (retVal.PayloadTypeEnum == TransactionPayloadLCSEnum.WriteSet)
                retVal.WriteSet = source.LCSDeserialization<WriteSetLCS>(ref cursor);
            else if (retVal.PayloadTypeEnum == TransactionPayloadLCSEnum.Script)
                retVal.Script = source.LCSDeserialization<ScriptLCS>(ref cursor);
            else if (retVal.PayloadTypeEnum == TransactionPayloadLCSEnum.Module)
                retVal.Module = source.LCSDeserialization<ModuleLCS>(ref cursor);

            return retVal;
        }

        public ProgramLCS GetProgram(byte[] source, ref int cursor)
        {
            var retVal = new ProgramLCS();
            //struct Program
            // {
            //  code: Vec<u8>, // Variable length byte array
            //  args: Vec<TransactionArgument>, // Variable length array of TransactionArguments
            //  modules: Vec<Vec<u8>>, // Variable length array of variable length byte arrays
            // }
            retVal.Code = source.LCSDeserialization<byte[]>(ref cursor);
            retVal.TransactionArguments =
                source.LCSDeserialization<List<TransactionArgumentLCS>>(ref cursor);
            retVal.Modules = source.LCSDeserialization<List<byte[]>>(ref cursor);

            return retVal;
        }
        public ScriptLCS GetScript(byte[] source, ref int cursor)
        {
            var retVal = new ScriptLCS();
            retVal.Code = source.LCSDeserialization<byte[]>(ref cursor);
            retVal.TransactionArguments =
                source.LCSDeserialization<List<TransactionArgumentLCS>>(ref cursor);

            return retVal;
        }
        public ModuleLCS GetModule(byte[] source, ref int cursor)
        {
            var retVal = new ModuleLCS();
            retVal.Code = source.LCSDeserialization<byte[]>(ref cursor);
            return retVal;
        }

        public TransactionArgumentLCS GetTransactionArgument(byte[] source,
            ref int cursor)
        {
            var retVal = new TransactionArgumentLCS();
            retVal.ArgType = Read_u32(source, ref cursor);

            if (retVal.ArgTypeEnum == TransactionArgumentLCSEnum.U64)
            {
                retVal.U64 = Read_u64(source, ref cursor);
            }
            else if (retVal.ArgTypeEnum == TransactionArgumentLCSEnum.Address)
            {
                retVal.Address = source.LCSDeserialization<AddressLCS>(ref cursor);
            }
            else if (retVal.ArgTypeEnum == TransactionArgumentLCSEnum.ByteArray)
            {
                retVal.ByteArray = source.LCSDeserialization<byte[]>(ref cursor);
            }
            else if (retVal.ArgTypeEnum == TransactionArgumentLCSEnum.String)
            {
                retVal.String = source.LCSDeserialization<string>(ref cursor);
            }

            return retVal;
        }

        public IEnumerable<TransactionArgumentLCS> GetTransactionArguments(
            byte[] source, ref int cursor)
        {
            var retVal = new List<TransactionArgumentLCS>();
            var length = Read_u32(source, ref cursor);

            for (int i = 0; i < length; i++)
                retVal.Add(source.LCSDeserialization<TransactionArgumentLCS>(ref cursor));

            return retVal;
        }
        #region Helpers

        public IEnumerable<byte> Read_u8(IEnumerable<byte> source,
          ref int localCursor, int count)
        {
            var retArr = source.Skip(localCursor).Take(count);
            localCursor += count;
            return retArr;
        }

        public string Read_String(IEnumerable<byte> source,
         ref int localCursor, int count)
        {
            var retArr = Read_u8(source, ref localCursor, count);
            return Encoding.UTF8.GetString(retArr.ToArray());
        }

        public ushort Read_u16(IEnumerable<byte> source,
          ref int localCursor)
        {
            var bytes = Read_u8(source, ref localCursor, 2);
            return BitConverter.ToUInt16(bytes.ToArray());
        }

        public uint Read_u32(IEnumerable<byte> source,
         ref int localCursor)
        {
            var bytes = Read_u8(source, ref localCursor, 4);
            return BitConverter.ToUInt32(bytes.ToArray());
        }

        public ulong Read_u64(IEnumerable<byte> source,
         ref int localCursor)
        {
            var bytes = Read_u8(source, ref localCursor, 8);
            return BitConverter.ToUInt64(bytes.ToArray());
        }
        #endregion

    }
}

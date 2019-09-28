using AdmissionControl;
using Google.Protobuf;
using Grpc.Core;
using LibraClient.Enums;
using LibraClient.LCSTypes;
using NBitcoin.DataEncoders;
using NSec.Cryptography;
using SHA3.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraClient
{
    class Program
    {
        static void Main(string[] args)
        {
            #region ADDRESS

            AddressLCS AddressLCS = new AddressLCS()
            {
                Length = (uint)32,
                Value = "ca820bf9305eb97d0d784f71b3955457fbf6911f5300ceaa5d7e8621529eae19"
            };

            byte[] AddressByteLcs = LCSCore.LCSSerialization(AddressLCS);

            Console.WriteLine("ADDRESS SERIALIZED- " + AddressByteLcs.ByteArryToString());
            Console.WriteLine("ADDRESS DESERIALIZED - " + LCSCore.LCSDeserialization<AddressLCS>(AddressByteLcs));

            #endregion

            #region TRANSACTION ARGUMENTS 

            //U64

            TransactionArgumentLCS U64Argument = new TransactionArgumentLCS()
            {
                U64 = 9213671392124193148,
                ArgType = (uint)TransactionArgumentLCSEnum.U64
            };

            byte[] U64ArgumentByteLcs = LCSCore.LCSSerialization(U64Argument);

            Console.WriteLine("\nU64 ARGUMENT SERIALIZED - " + U64ArgumentByteLcs.ByteArryToString());
            Console.WriteLine("U64 ARGUMENT DESERIALIZED - " + LCSCore.LCSDeserialization<TransactionArgumentLCS>(U64ArgumentByteLcs));

            //STRING

            TransactionArgumentLCS StringArgument = new TransactionArgumentLCS()
            {
                String = "Hello, World!",
                ArgType = (uint)TransactionArgumentLCSEnum.String

            };

            byte[] StringArgumentByteLcs = LCSCore.LCSSerialization(StringArgument);

            Console.WriteLine("\nSTRING ARGUMENT SERIALIZED- " + StringArgumentByteLcs.ByteArryToString());
            Console.WriteLine("nSTRING ARGUMENT DESERIALIZED - " + LCSCore.LCSDeserialization<TransactionArgumentLCS>(StringArgumentByteLcs));

            //ADDRESS

            TransactionArgumentLCS AddressArgument = new TransactionArgumentLCS()
            {
                Address = new AddressLCS{
                    Value = "2c25991785343b23ae073a50e5fd809a2cd867526b3c1db2b0bf5d1924c693ed",
                    Length = 32
                },

                ArgType = (uint)TransactionArgumentLCSEnum.Address
            };

            byte[] AddressArgumentByteLcs = LCSCore.LCSSerialization(AddressArgument);

            Console.WriteLine("\nADDRESS ARGUMENT LCS - " + AddressArgumentByteLcs.ByteArryToString());
            Console.WriteLine("ADDRESS ARGUMENT DESERIALIZED - " + LCSCore.LCSDeserialization<TransactionArgumentLCS>(AddressArgumentByteLcs));

            //BYTE ARRAY

            TransactionArgumentLCS ByteArrayArgument = new TransactionArgumentLCS()
            {
                // TESTING BYTE ARRAY
                ByteArray = Encoding.UTF8.GetBytes("01217da6c6b3e19f18"),
                ArgType = (uint)TransactionArgumentLCSEnum.ByteArray
            };

            byte[] ByteArrayArgumentByteLcs = LCSCore.LCSSerialization(ByteArrayArgument);

            Console.WriteLine("\nBYTE ARRAY ARGUMENT SERIALIZED - " + ByteArrayArgumentByteLcs.ByteArryToString());
            Console.WriteLine("BYTE ARRAY ARGUMENT DESERIALIZED - " + LCSCore.LCSDeserialization<TransactionArgumentLCS>(ByteArrayArgumentByteLcs));

            #endregion

            #region ACCESS PATH

            // TESTING BYTE ARRAY
            // TESTING BYTE ARRAY
            byte[] Path = Encoding.UTF8.GetBytes("01217da6c6b3e19f18");

            AccessPathLCS AccessPath = new AccessPathLCS()
            {
                Address = new AddressLCS
                {
                    Value = "9a1ad09742d1ffc62e659e9a7797808b206f956f131d07509449c01ad8220ad4",
                    Length = 32
                },
                Path = Path
            };

            byte[] AccessPathByteLcs = LCSCore.LCSSerialization(AccessPath);

            Console.WriteLine("\nACCESS PATH SERIALIZED - " + AccessPathByteLcs.ByteArryToString());
            Console.WriteLine("ACCESS PATH DESERIALIZED - " + LCSCore.LCSDeserialization<AccessPathLCS>(AccessPathByteLcs));

            #endregion
        }
    }
}



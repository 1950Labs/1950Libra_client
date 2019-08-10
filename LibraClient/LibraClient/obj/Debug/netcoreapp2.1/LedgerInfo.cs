// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/ledger_info.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Types {

  /// <summary>Holder for reflection information generated from Protos/ledger_info.proto</summary>
  public static partial class LedgerInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for Protos/ledger_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static LedgerInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChhQcm90b3MvbGVkZ2VyX2luZm8ucHJvdG8SBXR5cGVzIqgBCgpMZWRnZXJJ",
            "bmZvEg8KB3ZlcnNpb24YASABKAQSJAocdHJhbnNhY3Rpb25fYWNjdW11bGF0",
            "b3JfaGFzaBgCIAEoDBIbChNjb25zZW5zdXNfZGF0YV9oYXNoGAMgASgMEhoK",
            "EmNvbnNlbnN1c19ibG9ja19pZBgEIAEoDBIRCgllcG9jaF9udW0YBSABKAQS",
            "FwoPdGltZXN0YW1wX3VzZWNzGAYgASgEInEKGExlZGdlckluZm9XaXRoU2ln",
            "bmF0dXJlcxItCgpzaWduYXR1cmVzGAEgAygLMhkudHlwZXMuVmFsaWRhdG9y",
            "U2lnbmF0dXJlEiYKC2xlZGdlcl9pbmZvGAIgASgLMhEudHlwZXMuTGVkZ2Vy",
            "SW5mbyI9ChJWYWxpZGF0b3JTaWduYXR1cmUSFAoMdmFsaWRhdG9yX2lkGAEg",
            "ASgMEhEKCXNpZ25hdHVyZRgCIAEoDGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Types.LedgerInfo), global::Types.LedgerInfo.Parser, new[]{ "Version", "TransactionAccumulatorHash", "ConsensusDataHash", "ConsensusBlockId", "EpochNum", "TimestampUsecs" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Types.LedgerInfoWithSignatures), global::Types.LedgerInfoWithSignatures.Parser, new[]{ "Signatures", "LedgerInfo" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Types.ValidatorSignature), global::Types.ValidatorSignature.Parser, new[]{ "ValidatorId", "Signature" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  //// Even though we don't always need all hashes, we pass them in and return them
  //// always so that we keep them in sync on the client and don't make the client
  //// worry about which one(s) to pass in which cases
  ////
  //// This structure serves a dual purpose.
  ////
  //// First, if this structure is signed by 2f+1 validators it signifies the state
  //// of the ledger at version `version` -- it contains the transaction
  //// accumulator at that version which commits to all historical transactions.
  //// This structure may be expanded to include other information that is derived
  //// from that accumulator (e.g. the current time according to the time contract)
  //// to reduce the number of proofs a client must get.
  ////
  //// Second, the structure contains a `consensus_data_hash` value. This is the
  //// hash of an internal data structure that represents a block that is voted on
  //// by consensus.
  ////
  //// Combining these two concepts when the consensus algorithm votes on a block B
  //// it votes for a LedgerInfo with the `version` being the latest version that
  //// will be committed if B gets 2f+1 votes. It sets `consensus_data_hash` to
  //// represent B so that if those 2f+1 votes are gathered, the block is valid to
  //// commit
  /// </summary>
  public sealed partial class LedgerInfo : pb::IMessage<LedgerInfo> {
    private static readonly pb::MessageParser<LedgerInfo> _parser = new pb::MessageParser<LedgerInfo>(() => new LedgerInfo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LedgerInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Types.LedgerInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LedgerInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LedgerInfo(LedgerInfo other) : this() {
      version_ = other.version_;
      transactionAccumulatorHash_ = other.transactionAccumulatorHash_;
      consensusDataHash_ = other.consensusDataHash_;
      consensusBlockId_ = other.consensusBlockId_;
      epochNum_ = other.epochNum_;
      timestampUsecs_ = other.timestampUsecs_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LedgerInfo Clone() {
      return new LedgerInfo(this);
    }

    /// <summary>Field number for the "version" field.</summary>
    public const int VersionFieldNumber = 1;
    private ulong version_;
    /// <summary>
    /// Current latest version of the system
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong Version {
      get { return version_; }
      set {
        version_ = value;
      }
    }

    /// <summary>Field number for the "transaction_accumulator_hash" field.</summary>
    public const int TransactionAccumulatorHashFieldNumber = 2;
    private pb::ByteString transactionAccumulatorHash_ = pb::ByteString.Empty;
    /// <summary>
    /// Root hash of transaction accumulator at this version
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString TransactionAccumulatorHash {
      get { return transactionAccumulatorHash_; }
      set {
        transactionAccumulatorHash_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "consensus_data_hash" field.</summary>
    public const int ConsensusDataHashFieldNumber = 3;
    private pb::ByteString consensusDataHash_ = pb::ByteString.Empty;
    /// <summary>
    /// Hash of consensus-specific data that is opaque to all parts of the system
    /// other than consensus.  This is needed to verify signatures because
    /// consensus signing includes this hash
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString ConsensusDataHash {
      get { return consensusDataHash_; }
      set {
        consensusDataHash_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "consensus_block_id" field.</summary>
    public const int ConsensusBlockIdFieldNumber = 4;
    private pb::ByteString consensusBlockId_ = pb::ByteString.Empty;
    /// <summary>
    /// The block id of the last committed block corresponding to this ledger info.
    /// This field is not particularly interesting to the clients, but can be used
    /// by the validators for synchronization.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString ConsensusBlockId {
      get { return consensusBlockId_; }
      set {
        consensusBlockId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "epoch_num" field.</summary>
    public const int EpochNumFieldNumber = 5;
    private ulong epochNum_;
    /// <summary>
    /// Epoch number corresponds to the set of validators that are active for this
    /// ledger info. The main motivation for keeping the epoch number in the
    /// LedgerInfo is to ensure that the client has enough information to verify
    /// that the signatures for this info are coming from the validators that
    /// indeed form a quorum. Without epoch number a potential attack could reuse
    /// the signatures from the validators in one epoch in order to sign the wrong
    /// info belonging to another epoch, in which these validators do not form a
    /// quorum. The very first epoch number is 0.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong EpochNum {
      get { return epochNum_; }
      set {
        epochNum_ = value;
      }
    }

    /// <summary>Field number for the "timestamp_usecs" field.</summary>
    public const int TimestampUsecsFieldNumber = 6;
    private ulong timestampUsecs_;
    /// <summary>
    /// Timestamp that represents the microseconds since the epoch (unix time) that is
    /// generated by the proposer of the block.  This is strictly increasing with every block.
    /// If a client reads a timestamp > the one they specified for transaction expiration time,
    /// they can be certain that their transaction will never be included in a block in the future
    /// (assuming that their transaction has not yet been included)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong TimestampUsecs {
      get { return timestampUsecs_; }
      set {
        timestampUsecs_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LedgerInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LedgerInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Version != other.Version) return false;
      if (TransactionAccumulatorHash != other.TransactionAccumulatorHash) return false;
      if (ConsensusDataHash != other.ConsensusDataHash) return false;
      if (ConsensusBlockId != other.ConsensusBlockId) return false;
      if (EpochNum != other.EpochNum) return false;
      if (TimestampUsecs != other.TimestampUsecs) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Version != 0UL) hash ^= Version.GetHashCode();
      if (TransactionAccumulatorHash.Length != 0) hash ^= TransactionAccumulatorHash.GetHashCode();
      if (ConsensusDataHash.Length != 0) hash ^= ConsensusDataHash.GetHashCode();
      if (ConsensusBlockId.Length != 0) hash ^= ConsensusBlockId.GetHashCode();
      if (EpochNum != 0UL) hash ^= EpochNum.GetHashCode();
      if (TimestampUsecs != 0UL) hash ^= TimestampUsecs.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Version != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(Version);
      }
      if (TransactionAccumulatorHash.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(TransactionAccumulatorHash);
      }
      if (ConsensusDataHash.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(ConsensusDataHash);
      }
      if (ConsensusBlockId.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(ConsensusBlockId);
      }
      if (EpochNum != 0UL) {
        output.WriteRawTag(40);
        output.WriteUInt64(EpochNum);
      }
      if (TimestampUsecs != 0UL) {
        output.WriteRawTag(48);
        output.WriteUInt64(TimestampUsecs);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Version != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(Version);
      }
      if (TransactionAccumulatorHash.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(TransactionAccumulatorHash);
      }
      if (ConsensusDataHash.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(ConsensusDataHash);
      }
      if (ConsensusBlockId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(ConsensusBlockId);
      }
      if (EpochNum != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(EpochNum);
      }
      if (TimestampUsecs != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(TimestampUsecs);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LedgerInfo other) {
      if (other == null) {
        return;
      }
      if (other.Version != 0UL) {
        Version = other.Version;
      }
      if (other.TransactionAccumulatorHash.Length != 0) {
        TransactionAccumulatorHash = other.TransactionAccumulatorHash;
      }
      if (other.ConsensusDataHash.Length != 0) {
        ConsensusDataHash = other.ConsensusDataHash;
      }
      if (other.ConsensusBlockId.Length != 0) {
        ConsensusBlockId = other.ConsensusBlockId;
      }
      if (other.EpochNum != 0UL) {
        EpochNum = other.EpochNum;
      }
      if (other.TimestampUsecs != 0UL) {
        TimestampUsecs = other.TimestampUsecs;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Version = input.ReadUInt64();
            break;
          }
          case 18: {
            TransactionAccumulatorHash = input.ReadBytes();
            break;
          }
          case 26: {
            ConsensusDataHash = input.ReadBytes();
            break;
          }
          case 34: {
            ConsensusBlockId = input.ReadBytes();
            break;
          }
          case 40: {
            EpochNum = input.ReadUInt64();
            break;
          }
          case 48: {
            TimestampUsecs = input.ReadUInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  //// The validator node returns this structure which includes signatures
  //// from each validator to confirm the state.  The client needs to only pass
  //// back the LedgerInfo element since the validator node doesn't need to know
  //// the signatures again when the client performs a query, those are only there
  //// for the client to be able to verify the state
  /// </summary>
  public sealed partial class LedgerInfoWithSignatures : pb::IMessage<LedgerInfoWithSignatures> {
    private static readonly pb::MessageParser<LedgerInfoWithSignatures> _parser = new pb::MessageParser<LedgerInfoWithSignatures>(() => new LedgerInfoWithSignatures());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LedgerInfoWithSignatures> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Types.LedgerInfoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LedgerInfoWithSignatures() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LedgerInfoWithSignatures(LedgerInfoWithSignatures other) : this() {
      signatures_ = other.signatures_.Clone();
      ledgerInfo_ = other.ledgerInfo_ != null ? other.ledgerInfo_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LedgerInfoWithSignatures Clone() {
      return new LedgerInfoWithSignatures(this);
    }

    /// <summary>Field number for the "signatures" field.</summary>
    public const int SignaturesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Types.ValidatorSignature> _repeated_signatures_codec
        = pb::FieldCodec.ForMessage(10, global::Types.ValidatorSignature.Parser);
    private readonly pbc::RepeatedField<global::Types.ValidatorSignature> signatures_ = new pbc::RepeatedField<global::Types.ValidatorSignature>();
    /// <summary>
    /// Signatures of the root node from each validator
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Types.ValidatorSignature> Signatures {
      get { return signatures_; }
    }

    /// <summary>Field number for the "ledger_info" field.</summary>
    public const int LedgerInfoFieldNumber = 2;
    private global::Types.LedgerInfo ledgerInfo_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Types.LedgerInfo LedgerInfo {
      get { return ledgerInfo_; }
      set {
        ledgerInfo_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LedgerInfoWithSignatures);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LedgerInfoWithSignatures other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!signatures_.Equals(other.signatures_)) return false;
      if (!object.Equals(LedgerInfo, other.LedgerInfo)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= signatures_.GetHashCode();
      if (ledgerInfo_ != null) hash ^= LedgerInfo.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      signatures_.WriteTo(output, _repeated_signatures_codec);
      if (ledgerInfo_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(LedgerInfo);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += signatures_.CalculateSize(_repeated_signatures_codec);
      if (ledgerInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(LedgerInfo);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LedgerInfoWithSignatures other) {
      if (other == null) {
        return;
      }
      signatures_.Add(other.signatures_);
      if (other.ledgerInfo_ != null) {
        if (ledgerInfo_ == null) {
          LedgerInfo = new global::Types.LedgerInfo();
        }
        LedgerInfo.MergeFrom(other.LedgerInfo);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            signatures_.AddEntriesFrom(input, _repeated_signatures_codec);
            break;
          }
          case 18: {
            if (ledgerInfo_ == null) {
              LedgerInfo = new global::Types.LedgerInfo();
            }
            input.ReadMessage(LedgerInfo);
            break;
          }
        }
      }
    }

  }

  public sealed partial class ValidatorSignature : pb::IMessage<ValidatorSignature> {
    private static readonly pb::MessageParser<ValidatorSignature> _parser = new pb::MessageParser<ValidatorSignature>(() => new ValidatorSignature());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ValidatorSignature> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Types.LedgerInfoReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ValidatorSignature() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ValidatorSignature(ValidatorSignature other) : this() {
      validatorId_ = other.validatorId_;
      signature_ = other.signature_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ValidatorSignature Clone() {
      return new ValidatorSignature(this);
    }

    /// <summary>Field number for the "validator_id" field.</summary>
    public const int ValidatorIdFieldNumber = 1;
    private pb::ByteString validatorId_ = pb::ByteString.Empty;
    /// <summary>
    /// The account address of the validator, which can be used for retrieving its
    /// public key during the given epoch.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString ValidatorId {
      get { return validatorId_; }
      set {
        validatorId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "signature" field.</summary>
    public const int SignatureFieldNumber = 2;
    private pb::ByteString signature_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Signature {
      get { return signature_; }
      set {
        signature_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ValidatorSignature);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ValidatorSignature other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ValidatorId != other.ValidatorId) return false;
      if (Signature != other.Signature) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ValidatorId.Length != 0) hash ^= ValidatorId.GetHashCode();
      if (Signature.Length != 0) hash ^= Signature.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ValidatorId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(ValidatorId);
      }
      if (Signature.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(Signature);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ValidatorId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(ValidatorId);
      }
      if (Signature.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Signature);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ValidatorSignature other) {
      if (other == null) {
        return;
      }
      if (other.ValidatorId.Length != 0) {
        ValidatorId = other.ValidatorId;
      }
      if (other.Signature.Length != 0) {
        Signature = other.Signature;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ValidatorId = input.ReadBytes();
            break;
          }
          case 18: {
            Signature = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/admission_control.proto
// </auto-generated>
// Original file comments:
// Copyright (c) The Libra Core Contributors
// SPDX-License-Identifier: Apache-2.0
//
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace AdmissionControl {
  /// <summary>
  /// -----------------------------------------------------------------------------
  /// ---------------- Service definition
  /// -----------------------------------------------------------------------------
  /// </summary>
  public static partial class AdmissionControl
  {
    static readonly string __ServiceName = "admission_control.AdmissionControl";

    static readonly grpc::Marshaller<global::AdmissionControl.SubmitTransactionRequest> __Marshaller_admission_control_SubmitTransactionRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AdmissionControl.SubmitTransactionRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::AdmissionControl.SubmitTransactionResponse> __Marshaller_admission_control_SubmitTransactionResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AdmissionControl.SubmitTransactionResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Types.UpdateToLatestLedgerRequest> __Marshaller_types_UpdateToLatestLedgerRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Types.UpdateToLatestLedgerRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Types.UpdateToLatestLedgerResponse> __Marshaller_types_UpdateToLatestLedgerResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Types.UpdateToLatestLedgerResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::AdmissionControl.SubmitTransactionRequest, global::AdmissionControl.SubmitTransactionResponse> __Method_SubmitTransaction = new grpc::Method<global::AdmissionControl.SubmitTransactionRequest, global::AdmissionControl.SubmitTransactionResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SubmitTransaction",
        __Marshaller_admission_control_SubmitTransactionRequest,
        __Marshaller_admission_control_SubmitTransactionResponse);

    static readonly grpc::Method<global::Types.UpdateToLatestLedgerRequest, global::Types.UpdateToLatestLedgerResponse> __Method_UpdateToLatestLedger = new grpc::Method<global::Types.UpdateToLatestLedgerRequest, global::Types.UpdateToLatestLedgerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateToLatestLedger",
        __Marshaller_types_UpdateToLatestLedgerRequest,
        __Marshaller_types_UpdateToLatestLedgerResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::AdmissionControl.AdmissionControlReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of AdmissionControl</summary>
    [grpc::BindServiceMethod(typeof(AdmissionControl), "BindService")]
    public abstract partial class AdmissionControlBase
    {
      /// <summary>
      /// Public API to submit transaction to a validator.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::AdmissionControl.SubmitTransactionResponse> SubmitTransaction(global::AdmissionControl.SubmitTransactionRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// This API is used to update the client to the latest ledger version and
      /// optionally also request 1..n other pieces of data.  This allows for batch
      /// queries.  All queries return proofs that a client should check to validate
      /// the data. Note that if a client only wishes to update to the latest
      /// LedgerInfo and receive the proof of this latest version, they can simply
      /// omit the requested_items (or pass an empty list)
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Types.UpdateToLatestLedgerResponse> UpdateToLatestLedger(global::Types.UpdateToLatestLedgerRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for AdmissionControl</summary>
    public partial class AdmissionControlClient : grpc::ClientBase<AdmissionControlClient>
    {
      /// <summary>Creates a new client for AdmissionControl</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public AdmissionControlClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AdmissionControl that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public AdmissionControlClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected AdmissionControlClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected AdmissionControlClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Public API to submit transaction to a validator.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::AdmissionControl.SubmitTransactionResponse SubmitTransaction(global::AdmissionControl.SubmitTransactionRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SubmitTransaction(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Public API to submit transaction to a validator.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::AdmissionControl.SubmitTransactionResponse SubmitTransaction(global::AdmissionControl.SubmitTransactionRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SubmitTransaction, null, options, request);
      }
      /// <summary>
      /// Public API to submit transaction to a validator.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::AdmissionControl.SubmitTransactionResponse> SubmitTransactionAsync(global::AdmissionControl.SubmitTransactionRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SubmitTransactionAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Public API to submit transaction to a validator.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::AdmissionControl.SubmitTransactionResponse> SubmitTransactionAsync(global::AdmissionControl.SubmitTransactionRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SubmitTransaction, null, options, request);
      }
      /// <summary>
      /// This API is used to update the client to the latest ledger version and
      /// optionally also request 1..n other pieces of data.  This allows for batch
      /// queries.  All queries return proofs that a client should check to validate
      /// the data. Note that if a client only wishes to update to the latest
      /// LedgerInfo and receive the proof of this latest version, they can simply
      /// omit the requested_items (or pass an empty list)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Types.UpdateToLatestLedgerResponse UpdateToLatestLedger(global::Types.UpdateToLatestLedgerRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateToLatestLedger(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// This API is used to update the client to the latest ledger version and
      /// optionally also request 1..n other pieces of data.  This allows for batch
      /// queries.  All queries return proofs that a client should check to validate
      /// the data. Note that if a client only wishes to update to the latest
      /// LedgerInfo and receive the proof of this latest version, they can simply
      /// omit the requested_items (or pass an empty list)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Types.UpdateToLatestLedgerResponse UpdateToLatestLedger(global::Types.UpdateToLatestLedgerRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_UpdateToLatestLedger, null, options, request);
      }
      /// <summary>
      /// This API is used to update the client to the latest ledger version and
      /// optionally also request 1..n other pieces of data.  This allows for batch
      /// queries.  All queries return proofs that a client should check to validate
      /// the data. Note that if a client only wishes to update to the latest
      /// LedgerInfo and receive the proof of this latest version, they can simply
      /// omit the requested_items (or pass an empty list)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Types.UpdateToLatestLedgerResponse> UpdateToLatestLedgerAsync(global::Types.UpdateToLatestLedgerRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateToLatestLedgerAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// This API is used to update the client to the latest ledger version and
      /// optionally also request 1..n other pieces of data.  This allows for batch
      /// queries.  All queries return proofs that a client should check to validate
      /// the data. Note that if a client only wishes to update to the latest
      /// LedgerInfo and receive the proof of this latest version, they can simply
      /// omit the requested_items (or pass an empty list)
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Types.UpdateToLatestLedgerResponse> UpdateToLatestLedgerAsync(global::Types.UpdateToLatestLedgerRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_UpdateToLatestLedger, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override AdmissionControlClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AdmissionControlClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(AdmissionControlBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SubmitTransaction, serviceImpl.SubmitTransaction)
          .AddMethod(__Method_UpdateToLatestLedger, serviceImpl.UpdateToLatestLedger).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, AdmissionControlBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SubmitTransaction, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::AdmissionControl.SubmitTransactionRequest, global::AdmissionControl.SubmitTransactionResponse>(serviceImpl.SubmitTransaction));
      serviceBinder.AddMethod(__Method_UpdateToLatestLedger, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Types.UpdateToLatestLedgerRequest, global::Types.UpdateToLatestLedgerResponse>(serviceImpl.UpdateToLatestLedger));
    }

  }
}
#endregion
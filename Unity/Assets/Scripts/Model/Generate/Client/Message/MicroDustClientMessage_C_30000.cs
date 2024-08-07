using ET;
using MemoryPack;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(NetClient2Main_MicroDust_Login))]
	[Message(MicroDustClientMessage.Main2NetClient_MicroDust_Login)]
	[MemoryPackable]
	public partial class Main2NetClient_MicroDust_Login: MessageObject, IRequest
	{
		public static Main2NetClient_MicroDust_Login Create(bool isFromPool = true) 
		{ 
			return !isFromPool? new Main2NetClient_MicroDust_Login() : ObjectPool.Instance.Fetch(typeof(Main2NetClient_MicroDust_Login)) as Main2NetClient_MicroDust_Login; 
		}

		[MemoryPackOrder(0)]
		public int RpcId { get; set; }

		[MemoryPackOrder(1)]
		public int OwnerFiberId { get; set; }

		[MemoryPackOrder(2)]
		public string Account { get; set; }

		[MemoryPackOrder(3)]
		public string Password { get; set; }

		public override void Dispose() 
		{
			if (!this.IsFromPool) return;
			this.RpcId = default;
			this.OwnerFiberId = default;
			this.Account = default;
			this.Password = default;
			
			ObjectPool.Instance.Recycle(this); 
		}

	}

	[Message(MicroDustClientMessage.NetClient2Main_MicroDust_Login)]
	[MemoryPackable]
	public partial class NetClient2Main_MicroDust_Login: MessageObject, IResponse
	{
		public static NetClient2Main_MicroDust_Login Create(bool isFromPool = true) 
		{ 
			return !isFromPool? new NetClient2Main_MicroDust_Login() : ObjectPool.Instance.Fetch(typeof(NetClient2Main_MicroDust_Login)) as NetClient2Main_MicroDust_Login; 
		}

		[MemoryPackOrder(0)]
		public int RpcId { get; set; }

		[MemoryPackOrder(1)]
		public int Error { get; set; }

		[MemoryPackOrder(2)]
		public string Message { get; set; }

		[MemoryPackOrder(3)]
		public long PlayerId { get; set; }

		[MemoryPackOrder(4)]
		public string UserId { get; set; }

		public override void Dispose() 
		{
			if (!this.IsFromPool) return;
			this.RpcId = default;
			this.Error = default;
			this.Message = default;
			this.PlayerId = default;
			this.UserId = default;
			
			ObjectPool.Instance.Recycle(this); 
		}

	}

	public static class MicroDustClientMessage
	{
		 public const ushort Main2NetClient_MicroDust_Login = 30001;
		 public const ushort NetClient2Main_MicroDust_Login = 30002;
	}
}
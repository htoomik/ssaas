using System.Collections.Generic;

namespace SSaaS.Shared
{
	public class Batch 
	{
		public long? Id;
		public List<Request> Requests = new List<Request>();
	}


	public class Request
	{
		public long? Id;
		public long? BatchId;
		public string Url;
		public RequestStatus Status;
	}
}
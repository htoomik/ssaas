using System.Collections.Generic;
using System.Linq;

namespace SSaaS.Shared
{
	public class Batch 
	{
		public long? Id;
		public List<Request> Requests = new List<Request>();

		public RequestStatus Status 
		{
			get
			{
				var anyFailed = Requests.Any(r => r.Status == RequestStatus.Failed);
				var allDone = Requests.All(r => r.Status == RequestStatus.Done);
				var allNew = Requests.All(r => r.Status == RequestStatus.New);

				if (anyFailed)
					return RequestStatus.Failed;
				if (allDone)
					return RequestStatus.Done;
				if (allNew)
					return RequestStatus.New;
				return RequestStatus.Processing;
			}
		}
	}


	public class Request
	{
		public long? Id;
		public long? BatchId;
		public string Url;
		public RequestStatus Status;
		public string Message;
		public string Path;
	}
}
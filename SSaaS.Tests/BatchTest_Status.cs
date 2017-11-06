using System.Collections.Generic;
using SSaaS.Shared;
using Xunit;

namespace SSaaS.Tests
{
	public class BatchTest_Status
	{
		[Fact]
		public void When_AllNew_Expect_New()
		{
			var batch = new Batch
			{
				Requests = new List<Request> 
				{
					new Request { Status = RequestStatus.New },
					new Request { Status = RequestStatus.New },
				}
			};

			Assert.Equal(RequestStatus.New, batch.Status);
		}


		[Fact]
		public void When_AllDone_Expect_Done()
		{
			var batch = new Batch
			{
				Requests = new List<Request> 
				{
					new Request { Status = RequestStatus.Done },
					new Request { Status = RequestStatus.Done },
				}
			};

			Assert.Equal(RequestStatus.Done, batch.Status);
		}


		[Fact]
		public void When_AnyFailed_Expect_Failed()
		{
			var batch = new Batch
			{
				Requests = new List<Request> 
				{
					new Request { Status = RequestStatus.Failed },
					new Request { Status = RequestStatus.Done },
				}
			};

			Assert.Equal(RequestStatus.Failed, batch.Status);
		}
		
		
		[Fact]
		public void When_Mixed_And_NoneFailed_Expect_Processing()
		{
			var batch = new Batch
			{
				Requests = new List<Request> 
				{
					new Request { Status = RequestStatus.New },
					new Request { Status = RequestStatus.Processing },
					new Request { Status = RequestStatus.Done },
				}
			};

			Assert.Equal(RequestStatus.Processing, batch.Status);
		}
	}
}
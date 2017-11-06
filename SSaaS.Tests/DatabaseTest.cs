using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SSaaS.Shared;
using Xunit;

namespace SSaaS.Tests
{
	public class DatabaseTest : IDisposable
	{
		private List<long> batchIds = new List<long>();

		[Fact]
		public void AddBatch_Saves()
		{
			const string url = "some url";
			const RequestStatus status = RequestStatus.Processing;
			var batch = new Batch
			{
				Requests = new List<Request> { new Request { Url = url, Status = status } }
			};

			AddBatch(batch);

			var actual = GetBatch(batch.Id.Value);
			Assert.Equal(1, actual.Requests.Count);
			Assert.Equal(url, actual.Requests[0].Url);
			Assert.Equal(status, actual.Requests[0].Status);
		}


		[Fact]
		public void GetBatch_ReturnsBatch()
		{
			var batch = new Batch
			{ 
				Requests = new List<Request> { new Request { Status = RequestStatus.Done } }
			};
			AddBatch(batch);

			var actual = Database.GetBatch(batch.Id.Value);
			Assert.Equal(batch.Requests.Count, actual.Requests.Count);
			Assert.Equal(batch.Requests[0].Status, actual.Requests[0].Status);
		}


		[Fact]
		public void GetNextRequest_ReturnsOldestRequest_WithStatusNew()
		{
			var batch = new Batch
			{ 
				Requests = new List<Request> 
				{ 
					new Request { Url = "1", Status = RequestStatus.Done },
					new Request { Url = "2", Status = RequestStatus.New },
					new Request { Url = "3", Status = RequestStatus.Processing },
					new Request { Url = "4", Status = RequestStatus.New },
				}
			};
			AddBatch(batch);

			var actual = Database.GetNextRequest();
			Assert.Equal("2", actual.Url);
		}


		[Fact]
		public void GetNextRequest_When_NoNewRequests_ReturnsNull()
		{
			var batch = new Batch
			{ 
				Requests = new List<Request> 
				{ 
					new Request { Url = "1", Status = RequestStatus.Done },
					new Request { Url = "2", Status = RequestStatus.Done },
					new Request { Url = "3", Status = RequestStatus.Processing },
					new Request { Url = "4", Status = RequestStatus.Processing },
				}
			};
			AddBatch(batch);

			var actual = Database.GetNextRequest();
			Assert.Null(actual);
		}


		[Fact]
		public void SetStatus_SetsStatus()
		{
			var batch = new Batch
			{ 
				Requests = new List<Request> { new Request { Status = RequestStatus.New } }
			};
			AddBatch(batch);

			var newStatus = RequestStatus.Processing;
			Database.SetStatus(batch.Requests[0], newStatus);

			var actualStatus = Database.GetBatch(batch.Id.Value).Requests[0].Status;
			Assert.Equal(newStatus, actualStatus);
		}


		private void AddBatch(Batch batch)
		{
			Database.AddBatch(batch);
			batchIds.Add(batch.Id.Value);
		}


		private static Batch GetBatch(long batchId)
		{
			using (var connection = Database.GetConnection())
			{
				var sql = @"
				SELECT requestId, url, status
				FROM requests
				WHERE batchId = @batchId";
				var command = new SqliteCommand(sql, connection);
				command.Parameters.AddWithValue("@batchId", batchId);

				using (var reader = command.ExecuteReader())
				{
					var batch = new Batch { Id = batchId };
					while (reader.Read())
					{
						batch.Requests.Add(new Request 
						{ 
							Id = (long)reader["requestId"],
							Url = reader["url"].ToString(),
							Status = reader["status"].ToString().ParseAs<RequestStatus>()
						});
					}
					return batch;
				}
			}
		}


		public void Dispose()
		{
			using (var connection = Database.GetConnection())
			{
				foreach (var batchId in batchIds)
				{
					var command = new SqliteCommand(@"
						DELETE FROM requests WHERE batchId = @batchId;
						DELETE FROM batches WHERE batchId = @batchId;", 
					connection);
					command.Parameters.AddWithValue("@batchId", batchId);
					command.ExecuteNonQuery();
				}
			}
		}
	}
}

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

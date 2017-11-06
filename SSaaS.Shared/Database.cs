using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SSaaS.Shared
{
	public static class Database
	{
		public static void AddBatch(Batch batch)
		{
			const string sql1 = @"
				INSERT INTO batches (batchId) VALUES (null);
				SELECT last_insert_rowid();";

			const string sql2 = @"
				INSERT INTO requests (batchId, url, status) VALUES (@batchId, @url, @status);
				SELECT last_insert_rowid();";

			using (var connection = GetConnection())
			{
				var command1 = new SqliteCommand(sql1, connection);
				batch.Id = (long)command1.ExecuteScalar();

				foreach (var request in batch.Requests)
				{
					request.BatchId = batch.Id;

					var command2 = new SqliteCommand(sql2, connection);
					command2.Parameters.AddWithValue("@batchId", request.BatchId);
					command2.Parameters.AddWithValue("@url", ((object)request.Url) ?? DBNull.Value);
					command2.Parameters.AddWithValue("@status", request.Status.ToString());
					request.Id = (long)command2.ExecuteScalar();
				}
			}
		}


		public static Batch GetBatch(long batchId)
		{
			const string sql = "SELECT requestId, url, status FROM requests WHERE batchId = @batchId";
			using (var connection = GetConnection())
			{
				var command = new SqliteCommand(sql, connection);
				command.Parameters.AddWithValue("@batchId", batchId);
				using (var reader = command.ExecuteReader())
				{
					var requests = new List<Request>();
					while (reader.Read())
					{
						requests.Add(new Request 
						{
							Id = (long)reader["requestId"],
							Url = reader["url"].ToString(), 
							Status = reader["status"].ToString().ParseAs<RequestStatus>()
						});
					}
					return new Batch { Id = batchId, Requests = requests };
				}
			}
		}

		
		public static Request GetNextRequest()
		{
			const string sql = @"
				SELECT batchId, requestId, url, status
				FROM requests
				WHERE status = 'New'
				ORDER BY requestId
				LIMIT 1";
			using (var connection = GetConnection())
			{
				var command = new SqliteCommand(sql, connection);
				using (var reader = command.ExecuteReader())
				{
					if (!reader.Read())
						return null;
					return new Request
					{
						Id = (long)reader["requestId"],
						BatchId = (long)reader["batchId"],
						Url = reader["url"].ToString(),
						Status = reader["status"].ToString().ParseAs<RequestStatus>()
					};
				}
			}
		}


		public static void SetStatus(Request request, RequestStatus newStatus)
		{
			const string sql = @"
				UPDATE requests
				SET status = @status
				WHERE requestId = @requestId
					AND batchId = @batchId;";
			
			using (var connection = GetConnection())
			{
				var command = new SqliteCommand(sql, connection);
				command.Parameters.AddWithValue("@batchId", request.BatchId.Value);
				command.Parameters.AddWithValue("@requestId", request.Id.Value);
				command.Parameters.AddWithValue("@status", newStatus.ToString());
				command.ExecuteNonQuery();
			}
		}


		public static SqliteConnection GetConnection()
		{
			var path = new ConfigLoader().LoadConfig().DatabasePath;
			var fullPath = Path.GetFullPath(path);
			if (!File.Exists(fullPath))
				throw new Exception("File not found at " + fullPath);
			var connectionString = $"Filename={path}; Mode=ReadWrite;";
			var connection = new SqliteConnection(connectionString);
			connection.Open();
			return connection;
		}
	}
}

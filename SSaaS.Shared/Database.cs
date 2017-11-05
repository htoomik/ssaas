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
				INSERT INTO requests (batchId, url, status) VALUES (@batchId, @url, @status)";

			using (var connection = GetConnection())
			{
				var command1 = new SqliteCommand(sql1, connection);
				batch.Id = (long)command1.ExecuteScalar();

				foreach (var request in batch.Requests)
				{
					var command2 = new SqliteCommand(sql2, connection);
					command2.Parameters.AddWithValue("@batchId", batch.Id.Value);
					command2.Parameters.AddWithValue("@url", ((object)request.Url) ?? DBNull.Value);
					command2.Parameters.AddWithValue("@status", request.Status.ToString());
					command2.ExecuteNonQuery();
				}
			}
		}


		public static List<RequestStatus> GetStatus(long batchId)
		{
			const string sql = "SELECT status FROM requests WHERE batchId = @batchId";
			using (var connection = GetConnection())
			{
				var command = new SqliteCommand(sql, connection);
				command.Parameters.AddWithValue("@batchId", batchId);
				using (var reader = command.ExecuteReader())
				{
					var results = new List<RequestStatus>();
					while (reader.Read())
					{
						results.Add(reader[0].ToString().ParseAs<RequestStatus>());
					}
					return results;
				}
			}
		}


		public static SqliteConnection GetConnection()
		{
			// TODO: relative path
			const string path = "/Users/helen/Code/ssaas/ssaas.sqlite3";
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

using Microsoft.Data.Sqlite;

namespace SSaaS.Shared
{
	public interface IDatabase
	{
		void AddBatch(Batch batch);
		Batch GetBatch(long batchId);
		Request GetNextRequest();
		void SetStatus(Request request, RequestStatus newStatus, string message, string path);

		// TODO: move to separate class/interface
		SqliteConnection GetConnection();
	}
}
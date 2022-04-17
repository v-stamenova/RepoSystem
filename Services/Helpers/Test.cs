using Microsoft.Data.SqlClient;

namespace Services.Helpers
{
	public static class Test
	{
		public static bool IsServerConnected(string connectionString)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();
					connection.Close();
					return true;
				}
				catch (SqlException)
				{
					return false;
				}
			}
		}
	}
}

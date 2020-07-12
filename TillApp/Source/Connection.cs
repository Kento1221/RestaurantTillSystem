using System;
using System.Configuration;
using System.Data.SqlClient;

namespace TillApp.Source
{
    class Connection : IDisposable
    {
        private SqlConnection SqlConnection { get; }

        private bool disposed = false;
        System.Runtime.InteropServices.SafeHandle safeHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        public Connection(string conStringName)
        {
            if (ConfigurationManager.ConnectionStrings[conStringName].ConnectionString != null)
                SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[conStringName].ConnectionString);
            else 
                throw (new Exception("No connection string named " + conStringName + " has been found!"));
        }

        public void OpenConnection()
        {
            try
            {
            SqlConnection.Open();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        public void CloseConnection() => SqlConnection.Close();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                safeHandle.Dispose();
                SqlConnection.Dispose();
            }

            disposed = true;
        }
    }
}

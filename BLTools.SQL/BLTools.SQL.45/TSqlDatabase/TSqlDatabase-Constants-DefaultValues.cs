using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Constants for connection string
    private const string CS_SERVERNAME = "server";
    private const string CS_DATABASENAME = "database";
    private const string CS_USERID = "user id";
    private const string CS_USERNAME = "username";
    private const string CS_TRUSTED_CONNECTION = "trusted_connection";
    private const string CS_INTEGRATED_SECURITY = "integrated security";
    private const string CS_PASSWORD = "password";
    private const string CS_USE_MARS = "multipleactiveresultsets";
    private const string CS_ATTACH_DBFILENAME = "attachdbfilename";
    private const string CS_USER_INSTANCE = "userinstance";
    #endregion Constants for connection string

    #region Default values
    public static string DEFAULT_SERVERNAME = "(local)";
    public static string DEFAULT_DATABASENAME = "master";
    public static string DEFAULT_USERNAME = "";
    public static string DEFAULT_PASSWORD = "";
    public static bool DEFAULT_USE_INTEGRATED_SECURITY = true;
    /// <summary>
    /// Default connection timeout in secs
    /// </summary>
    public static int DEFAULT_CONNECTION_TIMEOUT = 300;
    public const int MAX_CONNECTION_TIMEOUT = 1000;

    /// <summary>
    /// This value will be used later to set UsePooledConnections when no specific value is passed
    /// </summary>
    public static bool DefaultUsePooledConnections = false;
    /// <summary>
    /// This value will be used later to set UseMars when no specific value is passed
    /// </summary>
    public static bool DefaultUseMars = true;
    #endregion Default values
  }
}

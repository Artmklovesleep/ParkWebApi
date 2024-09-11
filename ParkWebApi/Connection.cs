using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

public class LinqToDbSettings : ILinqToDBSettings
{
    public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

    public string DefaultConfiguration => "MySql";
    public string DefaultDataProvider => "MySql";

    public IEnumerable<IConnectionStringSettings> ConnectionStrings
    {
        get
        {
            yield return new ConnectionStringSettings
            {
                Name = "MySql",
                ProviderName = "MySql",
                ConnectionString = "Server=p684612.mysql.ihc.ru;Port=3306;Database=p684612_tema;Uid=p684612_tema;Pwd=7AZNus94iR;"
            };
        }
    }
}

public class ConnectionStringSettings : IConnectionStringSettings
{
    public string ConnectionString { get; set; }
    public string Name { get; set; }
    public string ProviderName { get; set; }
    public bool IsGlobal => false;
}

using System;
using EfPostgre.Utils;

namespace EfPostgre.Config
{
    public class PgDbConnection
    {
        Uri _uri = new Uri("pgsql://postgres:postgres@localhost:5432/eftest1");


        public string Params { get; set; }
        public string DbName => _uri.LocalPath.Replace("/", "");

        public string User => _uri.UserInfo.Split(':')[0];

        public string Pwd => _uri.UserInfo.Split(':')[1];

        public string Host => _uri.Host;
        public int Port => _uri.Port;

        //public string RestUrl => new UriBuilder("http", _uri.Host, _uri.Port).ToString();

        /// <inheritdoc />
        public PgDbConnection()
        {
            Params = "SSL Mode = Disable; Trust Server Certificate = false";
        }

        public string ConnectionString
        {
            get
            {
                if (Params.IsEmpty())
                {
                    return $"Server={Host};Port={Port};Database={DbName};UserId={User};Password={Pwd}";
                }

                return $"Server={Host};Port={Port};Database={DbName};UserId={User};Password={Pwd};{Params}";
            }
        }

    }
}
using LoveLetter.Core.Entities;
using Microsoft.Data.SqlClient;

namespace LoveLetter.Core.Adapters
{
    public interface ISqlDataAdapter
    {
        public SqlConnection Connection { get; }

        public DomainEntity Populate(string command);

        public void SaveChanges(string command);
    }
}

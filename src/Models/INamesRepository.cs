using System.Threading.Tasks;
using System.Collections.Generic;
namespace NamesApi.Models
{
    public interface INamesRepository
    {
        IEnumerable<NameEntry> Names {get;}
        Task<bool> NameEntryExists(int id);
        bool NameEntryExists(string name);
        Task<NameEntry> UpdateName(NameEntry n);
        Task<NameEntry> GetName(int id);
        Task<NameEntry> CreateName(NameEntry n);
        Task DeleteName(NameEntry n);
    }
}

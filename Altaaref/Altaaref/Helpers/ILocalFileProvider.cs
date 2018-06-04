using System.IO;
using System.Threading.Tasks;

namespace Altaaref.Helpers
{
    public interface ILocalFileProvider
    {
        Task<string> SaveFileToDisk(Stream stream, string fileName);
    }
}

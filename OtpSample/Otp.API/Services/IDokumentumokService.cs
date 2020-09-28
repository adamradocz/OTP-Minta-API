using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Otp.API.Services
{
    public interface IDokumentumokService
    {
        IEnumerable<string> GetDokumentumok();
        Task<string> GetDokumentum(string fileName, CancellationToken cancellationToken);
        long? GetFileSize(string fileName);
        Task<(bool, string)> PostDokumentum(string fileName, string file, CancellationToken cancellationToken);
    }
}

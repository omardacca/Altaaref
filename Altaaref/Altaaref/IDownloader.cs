using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref
{
    public interface IDownloader
    {
        void StartDownload(string url, string filename);
        void SaveTo();
    }
}

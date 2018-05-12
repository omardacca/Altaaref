using Altaaref.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Altaaref
{
    public interface IUploader
    {
        void UploadToBlob(int CourseId, string Name, int StudentId);
    }
}

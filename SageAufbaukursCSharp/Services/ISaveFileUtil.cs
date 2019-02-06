using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SageAufbaukursCSharp.Services
{
    public interface ISaveFileUtil
    {
        Exception Fault { get; }
        string Message { get; }
        bool Save(object beleg, string path);
        bool Save(object beleg);

    }
}

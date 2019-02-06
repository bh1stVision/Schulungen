using SageAufbaukursCSharp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SageAufbaukursCSharp.ServiceImplementations
{
    public class SaveFileUtil : ISaveFileUtil
    {
        #region ISaveFileUtil
        public Exception Fault { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Message { get; private set; }

        public bool Save(object beleg, string path)
        {
            throw new NotImplementedException();
        }

        public bool Save(object beleg)
        {
            try
            {
                using (var sw = new StreamWriter(""))
                {
                    sw.Write("hello");
                }

                return true;
            }
            catch (UnauthorizedAccessException uae)
            {
                Message = uae.Message;
                Fault = uae;
                return false;
            }
            catch (ArgumentException ae)
            {
                //if (Directory.Exists(_fallbackPath) /*&& Schreibberechtigung*/)
                //{
                //    var path = Path.Combine(_fallbackPath, "SaveFile.txt");
                //    var sw = new StreamWriter(path);
                //    sw.Write("hello");
                //    sw.Dispose();

                //    Message = $"Datei unter {path} gespeichert!";

                //    return true;
                //}

                //Message = $"{_fallbackPath} exisiert nicht, oder Sie haben nicht die erforderlichen Berechtigungen!";
                //return false;

                try
                {
                    var path = Path.Combine(_fallbackPath, "SaveFile.txt");

                    using (var sw = new StreamWriter(path))
                    {
                        sw.Write("hello");
                    }
                    _problemSolver.GetProblem(path);
                    Message = "Fallback wurde genutzt";
                    return false;
                }
                catch (Exception )
                {
                    Fault = ae;
                    return false;
                }
            }
            catch (Exception e)
            {
                Fault = e;
                return false;
            }
        }
        #endregion ISaveFileUtil

        #region constructors
        public SaveFileUtil(IProblemSolver problemSolver)
        {
            //Constructor Injection
            _problemSolver = problemSolver;
        }
        #endregion constructors

        #region private
        private string _fallbackPath = Environment.GetEnvironmentVariable("TEMP");
        private IProblemSolver _problemSolver;
        #endregion private




    }
}

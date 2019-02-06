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
            catch (PathTooLongException pte)
            {
                int pathLength = _path.Length;
                int charactersToCut = (260 - pathLength) * -1;

                string filename = Path.GetFileName(_path);
                int fileExtensionLength = filename.Length - (filename.LastIndexOf('.') + 1);
                string filenameWithoutFileExtension = filename.Substring(0, filename.Length - fileExtensionLength);
                string fileExtension = filename.Substring(filename.LastIndexOf('.'));
                string pathWithoutFilename = Path.GetDirectoryName(_path);

                if (filenameWithoutFileExtension.Length > charactersToCut)
                {
                    string cutFilename = filenameWithoutFileExtension.Substring(0, filenameWithoutFileExtension.Length - charactersToCut);
                    string newpath = Path.Combine(pathWithoutFilename, cutFilename, fileExtension);

                    var sw = new StreamWriter(newpath);
                    sw.Write("hello");
                    sw.Dispose();

                    Message = "Dateiname wurde gekürzt!";
                }
                else
                {
                    Fault = pte;
                    Message = pte.Message;
                }

                return false;
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
                    _problemSolver.SetProblem(path);
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

        #region services
        private readonly IProblemSolver _problemSolver;
        #endregion services

        #region constructors
        public SaveFileUtil(IProblemSolver problemSolver, string fallbackPath, string path)
        {
            //Constructor Injection
            _problemSolver = problemSolver;
        }
        #endregion constructors

        #region private
        private string _path;
        private string _fallbackPath; // = Environment.GetEnvironmentVariable("TEMP");
        #endregion private
    }
}

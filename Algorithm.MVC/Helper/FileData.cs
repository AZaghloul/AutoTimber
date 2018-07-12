using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace AutoTimber.MVC.Helper
{
    public enum FileType
    {
        InputName,
        OutputName,
        WexBIMArcName,
        WexBIMStrName,
        InputPath,
        OutputPath,
        WexBIMPathArc,
        WexBIMPAthStr,

    }
    public struct FileData
    {
        #region Static Variable
        public string InputDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public string WexBIMDirectory { get; set; }
        public string BoqDirectory { get; set; }
        #endregion
        public static string ExcelContentType { get; set; }
        = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string TextContentType { get; set; }
        = @"application/Text";

        #region Properties
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string InputName { get; set; }
        public string OutputName { get; set; }
        public string BoqName { get; set; }
        public string WexBIMArcName { get; set; }
        public string WexBIMStrName { get; set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string WexBIMPathArc { get; set; }
        public string WexBIMPathStr { get; set; }
        public string BoqPath { get; set; }
        #endregion
        public FileData(string inputName, string userId)
        {
            // get File Name To store in DB;

            InputName = inputName;
            UserId = userId;
            FileName = InputName;
            //set /data

            var ext = Path.GetExtension(FileName);
            var withoutExt = Path.GetFileNameWithoutExtension(FileName);
            OutputName = withoutExt + "- Structure" + ".ifc";
            WexBIMArcName = withoutExt + ".WexBIM";
            WexBIMStrName = withoutExt + "- Structure" + ".WexBIM";
            BoqName = withoutExt + "-BOQ" + ".xlsx";
            // GetDirectoryPaths
            InputDirectory = $"~/Users/{UserId}/input-files/";
            OutputDirectory = $"~/Users/{UserId}/output-files/";
            WexBIMDirectory = $"~/Users/{UserId}/wexbim-files/";
            BoqDirectory = $"~/Users/{UserId}/boq-files/";
            //get File Path to retrive from directories;
            InputPath = HostingEnvironment.MapPath(InputDirectory + InputName);
            OutputPath = HostingEnvironment.MapPath(OutputDirectory + OutputName);
            WexBIMPathArc = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMArcName);
            WexBIMPathStr = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMStrName);
            BoqPath = HostingEnvironment.MapPath(BoqDirectory + BoqName);

        }
        //public void FromName(string name)
        //{
        //    FileName = name;
        //    SetData();
        //}
        private void SetData()
        {
            // get File Name To store in DB;

            FileName = Path.GetFileNameWithoutExtension(InputName);
            //set /data
            var ext = Path.GetExtension(FileName);
            OutputName = FileName + "- Structure" + ext;
            WexBIMArcName = FileName + ".WexBIM";
            WexBIMStrName = FileName + "- Structure" + ".WexBIM";
            BoqName = FileName + "-BOQ" + ".xlsx";
            // GetDirectoryPaths
            InputDirectory = $"~/Users/{UserId}/input-files/";
            OutputDirectory = $"~/Users/{UserId}/output-files/";
            WexBIMDirectory = $"~/Users/{UserId}/wexbim-files/";
            BoqDirectory = $"~/Users/{UserId}/boq-files/";
            //get File Path to retrive from directories;
            InputPath = HostingEnvironment.MapPath(InputDirectory + InputName);
            OutputPath = HostingEnvironment.MapPath(OutputDirectory + OutputName);
            WexBIMPathArc = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMArcName);
            WexBIMPathStr = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMStrName);
            BoqPath = HostingEnvironment.MapPath(BoqDirectory + BoqName);
        }
        public bool Exists(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.InputName:
                    return File.Exists(InputName);

                case FileType.OutputName:
                    return File.Exists(OutputName);

                case FileType.WexBIMArcName:
                    return File.Exists(WexBIMArcName);

                case FileType.WexBIMStrName:
                    return File.Exists(WexBIMStrName);

                case FileType.InputPath:
                    return File.Exists(InputPath);

                case FileType.OutputPath:
                    return File.Exists(OutputPath);

                case FileType.WexBIMPathArc:
                    return File.Exists(WexBIMPathArc);

                case FileType.WexBIMPAthStr:
                    return File.Exists(WexBIMPathStr);
                default:
                    return false;
            }

        }

        public static void CheckDirectory(string UserId)
        {
            var input = $"~/Users/{UserId}/input-files/";
            var output = $"~/Users/{UserId}/output-files/";
            var wexbim = $"~/Users/{UserId}/wexbim-files/";
            var boq = $"~/Users/{UserId}/boq-files/";
            string[] paths = new string[] { input, output, wexbim, boq };
            foreach (var path in paths)
            {
                if (!Directory.Exists(HostingEnvironment.MapPath(path))) ;
                {
                    Directory.CreateDirectory(HostingEnvironment.MapPath(path));
                }
            }

        }

    }
}
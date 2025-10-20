using System.Text;
using NiceIO;
using Unity.CodeEditor;
using UnityEngine;

namespace UnityZed
{
    public class ZedProcess
    {
        private static readonly ILogger sLogger = ZedLogger.Create();

        private readonly NPath m_ExecPath;
        private readonly NPath m_ProjectPath;

        public ZedProcess(string execPath)
        {
            m_ExecPath = execPath;
            m_ProjectPath = new NPath(Application.dataPath).Parent;
        }

        public bool OpenProject(string filePath = "", int line = -1, int column = -1)
        {
            sLogger.Log($"OpenProject - ExecPath: {m_ExecPath}, FilePath: {filePath}, Line: {line}, Column: {column}");

            // always add project path
            var args = new StringBuilder($"\"{m_ProjectPath}\"");

            // if file path is provided, add it too
            if (!string.IsNullOrEmpty(filePath))
            {
                args.Append(" -a ");
                args.Append($"\"{filePath}\"");

                if (line >= 0)
                {
                    args.Append(":");
                    args.Append(line);

                    if (column >= 0)
                    {
                        args.Append(":");
                        args.Append(column);
                    }
                }
            }

            try
            {
                sLogger.Log($"Executing: {m_ExecPath} {args}");
                return CodeEditor.OSOpenFile(m_ExecPath.ToString(), args.ToString());
            }
            catch (System.Exception ex)
            {
                sLogger.Log($"Failed to open Zed: {ex.Message}");
                return false;
            }
        }
    }
}

using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System;
using System.Diagnostics;

namespace WinExt
{
    [ContextClass("ИнформацияОбИсполняемомФайле")]
    public class ExecutableFileInfo : AutoContext<ExecutableFileInfo>
    {
        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor()
        {
            return new ExecutableFileInfo();
        }

        /// <summary>
        /// Информация об основном модуле.
        /// - FileName
        /// - ModuleName
        /// - FileVersionInfo:
        /// 
        ///         * FileMajorPart
        ///         * FileMinorPart
        ///         * FileBuildPart
        ///         * FilePrivatePart
        /// </summary>
        /// <param name="proccessID"></param>
        /// <returns>ФиксированнаяСтруктура</returns>
        [ContextMethod("ОсновнойМодуль", "MainModule")]
        public IValue MainModuleInfo(int proccessID)
        {
            var proc = Process.GetProcessById((int)proccessID);
            try
            {
                StructureImpl strct = new StructureImpl();

                strct.Insert("FileName", ValueFactory.Create(proc.MainModule.FileName));
                strct.Insert("ModuleName", ValueFactory.Create(proc.MainModule.ModuleName));

                var fileVersionInfo = proc.MainModule.FileVersionInfo;
                StructureImpl versionInfo = new StructureImpl();
                versionInfo.Insert("FileMajorPart", ValueFactory.Create(fileVersionInfo.FileMajorPart));
                versionInfo.Insert("FileMinorPart", ValueFactory.Create(fileVersionInfo.FileMinorPart));
                versionInfo.Insert("FileBuildPart", ValueFactory.Create(fileVersionInfo.FileBuildPart));
                versionInfo.Insert("FilePrivatePart", ValueFactory.Create(fileVersionInfo.FilePrivatePart));

                strct.Insert("FileVersionInfo", versionInfo);

                FixedStructureImpl FixStruct = new FixedStructureImpl(strct);

                return FixStruct;
            }
            catch (Exception)
            {
                return ValueFactory.Create();
            }
        }
    }
}

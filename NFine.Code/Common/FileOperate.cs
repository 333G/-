using System;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;

namespace NFine.Code
{
    /// <summary>
    /// FObject : 文件系统的处理类
    /// </summary>
    public abstract class FileOperate
    {

        /// <summary>
        /// 文件系统的处理对象
        /// </summary>
        public enum FsoMethod
        {
            /// <summary>
            /// 仅用于处理文件夹
            /// </summary>
            Folder = 0,
            /// <summary>
            /// 仅用于处理文件
            /// </summary>
            File,
            /// <summary>
            /// 文件和文件夹都参与处理
            /// </summary>
            All
        }

        # region "文件的读写操作"

        /// <summary>
        /// 以文件流的形式读取指定文件的内容
        /// </summary>
        /// <param name="file">指定的文件及其全路径</param>
        /// <returns>返回 String</returns>
        public static string ReadFile(string file)
        {
            string strResult;

            var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var sReader = new StreamReader(fStream, Encoding.Default);

            try
            {
                strResult = sReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fStream.Flush();
                fStream.Close();
                sReader.Close();
            }

            return strResult;
        }

        /// <summary>
        /// 以文件流的形式读取指定文件的内容
        /// </summary>
        /// <param name="file">指定的文件及其全路径</param>
        /// <returns>返回 String</returns>
        public static Stream ReadFileToStream(string file)
        {
            var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);

            try
            {
                return fStream;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fStream.Flush();
                fStream.Close();
            }
        }
        public static string WriteFile(string file, string fileContent)
        {
            return WriteFile(file, fileContent, "utf-8");
        }

        /// <summary>
        /// 以文件流的形式将内容写入到指定文件中（如果该文件或文件夹不存在则创建）
        /// </summary>
        /// <param name="file">文件名和指定路径</param>
        /// <param name="fileContent">文件内容</param>
        /// <param name="encoding"></param>
        /// <returns>返回布尔值</returns>
        public static string WriteFile(string file, string fileContent, string encoding)
        {
            var f = new FileInfo(file);
            // 如果文件所在的文件夹不存在则创建文件夹
            if (f.DirectoryName != null && !Directory.Exists(f.DirectoryName)) Directory.CreateDirectory(f.DirectoryName);

            //FileStream fStream = new FileStream(file, FileMode.Create, FileAccess.Write);
            //StreamWriter sWriter = new StreamWriter(fStream, Encoding.UTF8);

            //try
            //{
            //    sWriter.Write(fileContent);
            //    return fileContent;
            //}
            //catch (Exception exc)
            //{
            //    throw new Exception(exc.ToString());
            //}
            //finally
            //{
            //    sWriter.Flush();
            //    fStream.Flush();
            //    sWriter.Close();
            //    fStream.Close();
            //}
            using (var sw = new StreamWriter(file, false, Encoding.GetEncoding(encoding))) 
            {
                sw.Write(fileContent);
            }
            return "";
        }

        /// <summary>
        /// 以文件流的形式将内容写入到指定文件中（如果该文件或文件夹不存在则创建）
        /// </summary>
        /// <param name="file">文件名和指定路径</param>
        /// <param name="fileContent">文件内容</param>
        /// <param name="append">是否追加指定内容到该文件中</param>
        /// <returns>返回布尔值</returns>
        public static void WriteFile(string file, string fileContent, bool append)
        {
            var f = new FileInfo(file);
            // 如果文件所在的文件夹不存在则创建文件夹
            if (f.DirectoryName != null && !Directory.Exists(f.DirectoryName)) Directory.CreateDirectory(f.DirectoryName);

            var sWriter = new StreamWriter(file, append, Encoding.GetEncoding("gb2312"));

            try
            {
                sWriter.Write(fileContent);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
            finally
            {
                sWriter.Flush();
                sWriter.Close();
            }
        }
        # endregion


        # region "文件夹信息的读取"

        /// <summary>
        /// 获取指定目录下的所有目录及其文件信息
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="method">获取方式。</param>
        /// <param name="searchPattern"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDirectoryAllInfos(string dir, FsoMethod method, string searchPattern)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var dInfo = new DirectoryInfo(dir);
            return GetDirectoryAllInfo(dInfo, method, searchPattern);
        }

        /// <summary>
        /// 获取指定目录下的文件信息
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static DataTable SearchDirectoryAllInfo(String dir, String search)
        {
            var d = new DirectoryInfo(dir);
            var dt = new DataTable();

            dt.Columns.Add("name");			//名称
            dt.Columns.Add("rname");		//名称
            dt.Columns.Add("content_type");	//文件MIME类型，如果是文件夹则置空
            dt.Columns.Add("type");			//类型：1为文件夹，2为文件
            dt.Columns.Add("path");			//文件路径
            dt.Columns.Add("creatime");		//创建时间
            dt.Columns.Add("size");			//文件大小


            var files = d.GetFiles(search);
            foreach (var file in files)
            {
                var dr = dt.NewRow();

                dr[0] = file.Name;
                dr[1] = file.FullName;
                dr[2] = file.Extension.Replace(".", "");
                dr[3] = 2;
                dr[4] = file.DirectoryName + "\\";
                dr[5] = file.CreationTime;
                dr[6] = file.Length;

                dt.Rows.Add(dr);
            }


            return dt;
        }


        /// <summary>
        /// 获取指定目录下的所有目录及其文件信息
        /// </summary>
        /// <param name="d">实例化的目录</param>
        /// <param name="method">获取方式。1、仅获取文件夹结构  2、仅获取文件结构  3、同时获取文件和文件夹信息</param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        private static DataTable GetDirectoryAllInfo(DirectoryInfo d, FsoMethod method, string searchPattern)
        {
            var dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("name");			//名称
            dt.Columns.Add("rname");		//名称
            dt.Columns.Add("content_type");	//文件MIME类型，如果是文件夹则置空
            dt.Columns.Add("type");			//类型：1为文件夹，2为文件
            dt.Columns.Add("path");			//文件路径
            dt.Columns.Add("creatime");		//创建时间
            dt.Columns.Add("size");			//文件大小
            dt.Columns.Add("file_path");

            // 获取文件夹结构信息
            var dirs = d.GetDirectories();
            foreach (var dir in dirs)
            {
                if (method == FsoMethod.File)
                {
                    dt = CopyDt(dt, GetDirectoryAllInfo(dir, method, searchPattern));
                }
                else
                {
                    dr = dt.NewRow();

                    dr[0] = dir.Name;
                    dr[1] = dir.FullName;
                    dr[2] = "";
                    dr[3] = 1;
                    dr[4] = dir.FullName.Replace(dir.Name, "");
                    dr[5] = dir.CreationTime;
                    dr[6] = "";
                    dr[7] = "";

                    dt.Rows.Add(dr);

                    dt = CopyDt(dt, GetDirectoryAllInfo(dir, method, searchPattern));
                }
            }

            // 获取文件结构信息
            if (method == FsoMethod.Folder) return dt;
            var files = searchPattern == "" ? d.GetFiles() : d.GetFiles(searchPattern);
            foreach (var file in files)
            {
                dr = dt.NewRow();

                dr[0] = file.Name;
                dr[1] = file.FullName;
                dr[2] = file.Extension.Replace(".", "");
                dr[3] = 2;
                dr[4] = file.DirectoryName + "\\";
                dr[5] = file.CreationTime;
                dr[6] = file.Length;
                dr[7] = file.FullName;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 将两个结构一样的 DataTable 组合成一个 DataTable
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>DataTable</returns>
        public static DataTable CopyDt(DataTable parent, DataTable child)
        {
            for (var i = 0; i < child.Rows.Count; i++)
            {
                var dr = parent.NewRow();
                for (var j = 0; j < parent.Columns.Count; j++)
                {
                    dr[j] = child.Rows[i][j];
                }
                parent.Rows.Add(dr);
            }

            return parent;
        }



        /// <summary>
        /// 获取指定文件夹的信息，如：文件夹大小，文件夹数，文件数
        /// </summary>
        /// <param name="dir">指定文件夹路径</param>
        /// <returns>返回 String</returns>
        public static long[] GetDirInfos(string dir)
        {
            var d = new DirectoryInfo(dir);
            var intResult = DirInfo(d);

            return intResult;
        }


        private static long[] DirInfo(DirectoryInfo d)
        {
            var intResult = new long[3];

            long dirLength = 0;
            long fileLength = 0;
            // 计算文件大小
            var files = d.GetFiles();
            fileLength += files.Length;
            var size = files.Sum(file => file.Length);
            // 计算文件夹
            var dirs = d.GetDirectories();
            dirLength += dirs.Length;
            foreach (var dir in dirs)
            {
                size += DirInfo(dir)[0];
                dirLength += DirInfo(dir)[1];
                fileLength += DirInfo(dir)[2];
            }

            intResult[0] = size;
            intResult[1] = dirLength;
            intResult[2] = fileLength;
            return intResult;
        }


        /// <summary>
        /// 获取指定目录的目录信息
        /// </summary>
        /// <param name="dir">指定目录</param>
        /// <param name="method">获取方式。1、仅获取文件夹结构  2、仅获取文件结构  3、同时获取文件和文件夹信息</param>
        /// <returns>返回 DataTable</returns>
        public static DataTable GetDirectoryInfos(string dir, FsoMethod method)
        {
            var dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("name");//名称
            dt.Columns.Add("type");//类型：1为文件夹，2为文件
            dt.Columns.Add("size");//文件大小，如果是文件夹则置空
            dt.Columns.Add("content_type");//文件MIME类型，如果是文件夹则置空
            dt.Columns.Add("createTime");//创建时间
            dt.Columns.Add("lastWriteTime");//最后修改时间

            // 获取文件夹结构信息
            if (method != FsoMethod.File)
            {
                for (var i = 0; i < GetDirs(dir).Length; i++)
                {
                    dr = dt.NewRow();
                    var d = new DirectoryInfo(GetDirs(dir)[i]);

                    dr[0] = d.Name;
                    dr[1] = 1;
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = d.CreationTime;
                    dr[5] = d.LastWriteTime;

                    dt.Rows.Add(dr);
                }
            }

            // 获取文件结构信息
            if (method == FsoMethod.Folder) return dt;
            for (var i = 0; i < GetFiles(dir).Length; i++)
            {
                dr = dt.NewRow();
                var f = new FileInfo(GetFiles(dir)[i]);

                dr[0] = f.Name;
                dr[1] = 2;
                dr[2] = f.Length;
                dr[3] = f.Extension.Replace(".", "");
                dr[4] = f.CreationTime;
                dr[5] = f.LastWriteTime;

                dt.Rows.Add(dr);
            }

            return dt;
        }


        private static string[] GetDirs(string dir)
        {
            return Directory.GetDirectories(dir);
        }

        private static string[] GetFiles(string dir)
        {
            return Directory.GetFiles(dir);
        }

        # endregion


        # region "文件系统的相应操作"

        /// <summary>
        /// 判断文件或文件夹是否存在
        /// </summary>
        /// <param name="file">指定文件及其路径</param>
        /// <param name="method">判断方式</param>
        /// <returns>返回布尔值</returns>
        public static bool IsExist(string file, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    return File.Exists(file);
                }
                return method == FsoMethod.Folder && Directory.Exists(file);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # region "新建"

        /// <summary>
        /// 新建文件或文件夹
        /// </summary>
        /// <param name="file">文件或文件夹及其路径</param>
        /// <param name="method">新建方式</param>
        public static void Create(string file, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    WriteFile(file, "");
                }
                else if (method == FsoMethod.Folder)
                {
                    Directory.CreateDirectory(file);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion


        # region "复制"

        # region "复制文件"
        /// <summary>
        /// 复制文件，如果目标文件已经存在则覆盖掉
        /// </summary>
        /// <param name="oldFile">源文件</param>
        /// <param name="newFile">目标文件</param>
        public static void CopyFile(string oldFile, string newFile)
        {
            try
            {
                File.Copy(oldFile, newFile, true);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }


        /// <summary>
        /// 以流的形式复制拷贝文件
        /// </summary>
        /// <param name="oldPath">源文件</param>
        /// <param name="newPath">目标文件</param>
        /// <returns></returns>
        public static bool CopyFileStream(string oldPath, string newPath)
        {
            try
            {
                //建立两个FileStream对象
                var fsOld = new FileStream(oldPath, FileMode.Open, FileAccess.Read);
                var fsNew = new FileStream(newPath, FileMode.Create, FileAccess.Write);

                //分别建立一个读写类
                var br = new BinaryReader(fsOld);
                var bw = new BinaryWriter(fsNew);

                //将读取文件流的指针指向流的头部
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                //将写入文件流的指针指向流的尾部
                br.BaseStream.Seek(0, SeekOrigin.End);

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    //从br流中读取一个Byte并马上写入bw流
                    bw.Write(br.ReadByte());
                }
                //释放所有被占用的资源
                br.Close();
                bw.Close();
                fsOld.Flush();
                fsOld.Close();
                fsNew.Flush();
                fsNew.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }

        # endregion

        # region "复制文件夹"

        /// <summary>
        /// 复制文件夹中的所有内容及其子目录所有文件
        /// </summary>
        /// <param name="oldDir">源文件夹及其路径</param>
        /// <param name="newDir">目标文件夹及其路径</param>
        public static void CopyDirectory(string oldDir, string newDir)
        {
            try
            {
                var dInfo = new DirectoryInfo(oldDir);
                CopyDirInfo(dInfo, oldDir, newDir);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        private static void CopyDirInfo(DirectoryInfo od, string oldDir, string newDir)
        {
            // 寻找文件夹
            if (!IsExist(newDir, FsoMethod.Folder)) Create(newDir, FsoMethod.Folder);
            var dirs = od.GetDirectories();
            foreach (var dir in dirs)
            {
                CopyDirInfo(dir, dir.FullName, newDir + dir.FullName.Replace(oldDir, ""));
            }
            // 寻找文件
            var files = od.GetFiles();
            foreach (var file in files)
            {
                CopyFile(file.FullName, newDir + file.FullName.Replace(oldDir, ""));
            }
        }

        # endregion

        # endregion


        # region "移动"

        /// <summary>
        /// 移动文件或文件夹
        /// </summary>
        /// <param name="oldFile">原始文件或文件夹</param>
        /// <param name="newFile">目标文件或文件夹</param>
        /// <param name="method">移动方式：1、为移动文件，2、为移动文件夹</param>
        public static void Move(string oldFile, string newFile, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    File.Move(oldFile, newFile);
                }
                if (method == FsoMethod.Folder)
                {
                    Directory.Move(oldFile, newFile);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion


        # region "删除"

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        /// <param name="file">文件或文件夹及其路径</param>
        /// <param name="method">删除方式：1、为删除文件，2、为删除文件夹</param>
        public static void Delete(string file, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    File.Delete(file);
                }
                if (method == FsoMethod.Folder)
                {
                    Directory.Delete(file, true);//删除该目录下的所有文件以及子目录
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion

        # endregion


        #region 获取文件权限信息
        public static string get_file_authority()
        {
            var sb = new StringBuilder();

            //检查系统目录的有效性
            const string folderstr = "include/upload,include/upload/products,backup/database";
            foreach (var foldler in folderstr.Split(','))
            {
                if (!SystemFolderCheck(foldler))
                {
                    sb.Append("<font color=red>对 " + foldler + " 目录没有写入和删除权限!</font><br>");
                }
                else
                {
                    sb.Append("对 " + foldler + " 目录权限验证通过!<br>");
                }
            }

            //检查系统文件的有效性
            const string filestr = "sitemap.xml";
            foreach (var file in filestr.Split(','))
            {
                if (!SystemFileCheck(file))
                {
                    sb.Append("<font color=red>" + file + " 没有写入和删除权限!</font><br>");
                }
                else
                {
                    sb.Append("" + file + " 权限验证通过!<br>");
                }
            }

            return sb.ToString();
        }

        public static bool SystemFolderCheck(string foldername)
        {
            var physicsPath = Utils.GetMapPath(@"..\" + foldername);
            try
            {
                using (var fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (!File.Exists(physicsPath + "\\a.txt")) return false;
                File.Delete(physicsPath + "\\a.txt");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SystemFileCheck(string filename)
        {
            filename = Utils.GetMapPath(@"..\" + filename);
            try
            {
                if (filename.IndexOf("systemfile.aspx", StringComparison.Ordinal) == -1 && !File.Exists(filename))
                    return false;
                if (filename.IndexOf("systemfile.aspx", StringComparison.Ordinal) != -1)  //做删除测试
                {
                    File.Delete(filename);
                    return true;
                }
                var sr = new StreamReader(filename);
                var content = sr.ReadToEnd();
                sr.Close();
                content += " ";
                var sw = new StreamWriter(filename, false);
                sw.Write(content);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}

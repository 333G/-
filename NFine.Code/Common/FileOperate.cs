using System;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;

namespace NFine.Code
{
    /// <summary>
    /// FObject : �ļ�ϵͳ�Ĵ�����
    /// </summary>
    public abstract class FileOperate
    {

        /// <summary>
        /// �ļ�ϵͳ�Ĵ������
        /// </summary>
        public enum FsoMethod
        {
            /// <summary>
            /// �����ڴ����ļ���
            /// </summary>
            Folder = 0,
            /// <summary>
            /// �����ڴ����ļ�
            /// </summary>
            File,
            /// <summary>
            /// �ļ����ļ��ж����봦��
            /// </summary>
            All
        }

        # region "�ļ��Ķ�д����"

        /// <summary>
        /// ���ļ�������ʽ��ȡָ���ļ�������
        /// </summary>
        /// <param name="file">ָ�����ļ�����ȫ·��</param>
        /// <returns>���� String</returns>
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
        /// ���ļ�������ʽ��ȡָ���ļ�������
        /// </summary>
        /// <param name="file">ָ�����ļ�����ȫ·��</param>
        /// <returns>���� String</returns>
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
        /// ���ļ�������ʽ������д�뵽ָ���ļ��У�������ļ����ļ��в������򴴽���
        /// </summary>
        /// <param name="file">�ļ�����ָ��·��</param>
        /// <param name="fileContent">�ļ�����</param>
        /// <param name="encoding"></param>
        /// <returns>���ز���ֵ</returns>
        public static string WriteFile(string file, string fileContent, string encoding)
        {
            var f = new FileInfo(file);
            // ����ļ����ڵ��ļ��в������򴴽��ļ���
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
        /// ���ļ�������ʽ������д�뵽ָ���ļ��У�������ļ����ļ��в������򴴽���
        /// </summary>
        /// <param name="file">�ļ�����ָ��·��</param>
        /// <param name="fileContent">�ļ�����</param>
        /// <param name="append">�Ƿ�׷��ָ�����ݵ����ļ���</param>
        /// <returns>���ز���ֵ</returns>
        public static void WriteFile(string file, string fileContent, bool append)
        {
            var f = new FileInfo(file);
            // ����ļ����ڵ��ļ��в������򴴽��ļ���
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


        # region "�ļ�����Ϣ�Ķ�ȡ"

        /// <summary>
        /// ��ȡָ��Ŀ¼�µ�����Ŀ¼�����ļ���Ϣ
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <param name="method">��ȡ��ʽ��</param>
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
        /// ��ȡָ��Ŀ¼�µ��ļ���Ϣ
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static DataTable SearchDirectoryAllInfo(String dir, String search)
        {
            var d = new DirectoryInfo(dir);
            var dt = new DataTable();

            dt.Columns.Add("name");			//����
            dt.Columns.Add("rname");		//����
            dt.Columns.Add("content_type");	//�ļ�MIME���ͣ�������ļ������ÿ�
            dt.Columns.Add("type");			//���ͣ�1Ϊ�ļ��У�2Ϊ�ļ�
            dt.Columns.Add("path");			//�ļ�·��
            dt.Columns.Add("creatime");		//����ʱ��
            dt.Columns.Add("size");			//�ļ���С


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
        /// ��ȡָ��Ŀ¼�µ�����Ŀ¼�����ļ���Ϣ
        /// </summary>
        /// <param name="d">ʵ������Ŀ¼</param>
        /// <param name="method">��ȡ��ʽ��1������ȡ�ļ��нṹ  2������ȡ�ļ��ṹ  3��ͬʱ��ȡ�ļ����ļ�����Ϣ</param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        private static DataTable GetDirectoryAllInfo(DirectoryInfo d, FsoMethod method, string searchPattern)
        {
            var dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("name");			//����
            dt.Columns.Add("rname");		//����
            dt.Columns.Add("content_type");	//�ļ�MIME���ͣ�������ļ������ÿ�
            dt.Columns.Add("type");			//���ͣ�1Ϊ�ļ��У�2Ϊ�ļ�
            dt.Columns.Add("path");			//�ļ�·��
            dt.Columns.Add("creatime");		//����ʱ��
            dt.Columns.Add("size");			//�ļ���С
            dt.Columns.Add("file_path");

            // ��ȡ�ļ��нṹ��Ϣ
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

            // ��ȡ�ļ��ṹ��Ϣ
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
        /// �������ṹһ���� DataTable ��ϳ�һ�� DataTable
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
        /// ��ȡָ���ļ��е���Ϣ���磺�ļ��д�С���ļ��������ļ���
        /// </summary>
        /// <param name="dir">ָ���ļ���·��</param>
        /// <returns>���� String</returns>
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
            // �����ļ���С
            var files = d.GetFiles();
            fileLength += files.Length;
            var size = files.Sum(file => file.Length);
            // �����ļ���
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
        /// ��ȡָ��Ŀ¼��Ŀ¼��Ϣ
        /// </summary>
        /// <param name="dir">ָ��Ŀ¼</param>
        /// <param name="method">��ȡ��ʽ��1������ȡ�ļ��нṹ  2������ȡ�ļ��ṹ  3��ͬʱ��ȡ�ļ����ļ�����Ϣ</param>
        /// <returns>���� DataTable</returns>
        public static DataTable GetDirectoryInfos(string dir, FsoMethod method)
        {
            var dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("name");//����
            dt.Columns.Add("type");//���ͣ�1Ϊ�ļ��У�2Ϊ�ļ�
            dt.Columns.Add("size");//�ļ���С��������ļ������ÿ�
            dt.Columns.Add("content_type");//�ļ�MIME���ͣ�������ļ������ÿ�
            dt.Columns.Add("createTime");//����ʱ��
            dt.Columns.Add("lastWriteTime");//����޸�ʱ��

            // ��ȡ�ļ��нṹ��Ϣ
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

            // ��ȡ�ļ��ṹ��Ϣ
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


        # region "�ļ�ϵͳ����Ӧ����"

        /// <summary>
        /// �ж��ļ����ļ����Ƿ����
        /// </summary>
        /// <param name="file">ָ���ļ�����·��</param>
        /// <param name="method">�жϷ�ʽ</param>
        /// <returns>���ز���ֵ</returns>
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

        # region "�½�"

        /// <summary>
        /// �½��ļ����ļ���
        /// </summary>
        /// <param name="file">�ļ����ļ��м���·��</param>
        /// <param name="method">�½���ʽ</param>
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


        # region "����"

        # region "�����ļ�"
        /// <summary>
        /// �����ļ������Ŀ���ļ��Ѿ������򸲸ǵ�
        /// </summary>
        /// <param name="oldFile">Դ�ļ�</param>
        /// <param name="newFile">Ŀ���ļ�</param>
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
        /// ��������ʽ���ƿ����ļ�
        /// </summary>
        /// <param name="oldPath">Դ�ļ�</param>
        /// <param name="newPath">Ŀ���ļ�</param>
        /// <returns></returns>
        public static bool CopyFileStream(string oldPath, string newPath)
        {
            try
            {
                //��������FileStream����
                var fsOld = new FileStream(oldPath, FileMode.Open, FileAccess.Read);
                var fsNew = new FileStream(newPath, FileMode.Create, FileAccess.Write);

                //�ֱ���һ����д��
                var br = new BinaryReader(fsOld);
                var bw = new BinaryWriter(fsNew);

                //����ȡ�ļ�����ָ��ָ������ͷ��
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                //��д���ļ�����ָ��ָ������β��
                br.BaseStream.Seek(0, SeekOrigin.End);

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    //��br���ж�ȡһ��Byte������д��bw��
                    bw.Write(br.ReadByte());
                }
                //�ͷ����б�ռ�õ���Դ
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

        # region "�����ļ���"

        /// <summary>
        /// �����ļ����е��������ݼ�����Ŀ¼�����ļ�
        /// </summary>
        /// <param name="oldDir">Դ�ļ��м���·��</param>
        /// <param name="newDir">Ŀ���ļ��м���·��</param>
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
            // Ѱ���ļ���
            if (!IsExist(newDir, FsoMethod.Folder)) Create(newDir, FsoMethod.Folder);
            var dirs = od.GetDirectories();
            foreach (var dir in dirs)
            {
                CopyDirInfo(dir, dir.FullName, newDir + dir.FullName.Replace(oldDir, ""));
            }
            // Ѱ���ļ�
            var files = od.GetFiles();
            foreach (var file in files)
            {
                CopyFile(file.FullName, newDir + file.FullName.Replace(oldDir, ""));
            }
        }

        # endregion

        # endregion


        # region "�ƶ�"

        /// <summary>
        /// �ƶ��ļ����ļ���
        /// </summary>
        /// <param name="oldFile">ԭʼ�ļ����ļ���</param>
        /// <param name="newFile">Ŀ���ļ����ļ���</param>
        /// <param name="method">�ƶ���ʽ��1��Ϊ�ƶ��ļ���2��Ϊ�ƶ��ļ���</param>
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


        # region "ɾ��"

        /// <summary>
        /// ɾ���ļ����ļ���
        /// </summary>
        /// <param name="file">�ļ����ļ��м���·��</param>
        /// <param name="method">ɾ����ʽ��1��Ϊɾ���ļ���2��Ϊɾ���ļ���</param>
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
                    Directory.Delete(file, true);//ɾ����Ŀ¼�µ������ļ��Լ���Ŀ¼
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion

        # endregion


        #region ��ȡ�ļ�Ȩ����Ϣ
        public static string get_file_authority()
        {
            var sb = new StringBuilder();

            //���ϵͳĿ¼����Ч��
            const string folderstr = "include/upload,include/upload/products,backup/database";
            foreach (var foldler in folderstr.Split(','))
            {
                if (!SystemFolderCheck(foldler))
                {
                    sb.Append("<font color=red>�� " + foldler + " Ŀ¼û��д���ɾ��Ȩ��!</font><br>");
                }
                else
                {
                    sb.Append("�� " + foldler + " Ŀ¼Ȩ����֤ͨ��!<br>");
                }
            }

            //���ϵͳ�ļ�����Ч��
            const string filestr = "sitemap.xml";
            foreach (var file in filestr.Split(','))
            {
                if (!SystemFileCheck(file))
                {
                    sb.Append("<font color=red>" + file + " û��д���ɾ��Ȩ��!</font><br>");
                }
                else
                {
                    sb.Append("" + file + " Ȩ����֤ͨ��!<br>");
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
                if (filename.IndexOf("systemfile.aspx", StringComparison.Ordinal) != -1)  //��ɾ������
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

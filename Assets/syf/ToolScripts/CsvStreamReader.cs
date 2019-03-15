#if UNITY_EDITOR
using System;
using System.Text;
using System.Collections;
using System.IO;
//using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;

//解析CSV的文件

namespace CSV
{
    public class CsvStreamReader
    {
        private ArrayList rowAL;//行链表,CSV文件的每一行就是一个链
        private string fileName;//文件名

        private Encoding encoding;//编码

        public CsvStreamReader()
        {
            this.rowAL = new ArrayList();
            this.fileName = "";
            this.encoding = Encoding.Unicode;
        }

        //fileName:文件名,包括文件路径
        public CsvStreamReader(string fileName)
        {
            this.rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = Encoding.Unicode;
        }

        //fileName:文件名,包括文件路径
        //encoding:文件编码
        public CsvStreamReader(string fileName, Encoding encoding)
        {
            this.rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = encoding;
        }

        public ArrayList GetRowList()
        {
            return rowAL;
        }
        //文件名,包括文件路径
        public string FileName
        {
            set
            {
                this.fileName = value;
                LoadCsvFile();
            }
        }

        //文件编码
        public Encoding FileEncoding
        {
            set
            {
                this.encoding = value;
            }
        }

        //获取行数
        public int RowCount
        {
            get
            {
                return this.rowAL.Count;
            }
        }

        //获取列数
        public int ColCount
        {
            get
            {
                int maxCol;

                maxCol = 0;
                for (int i = 0; i < this.rowAL.Count; i++)
                {
                    ArrayList colAL = (ArrayList)this.rowAL[i];
                    maxCol = (maxCol > colAL.Count) ? maxCol : colAL.Count;
                }

                return maxCol;
            }
        }

        //获取某行某列的数据
        public string this[int row, int col]
        {
            get
            {
                //数据有效性验证
                CheckRowValid(row);
                CheckColValid(col);
                ArrayList colAL = (ArrayList)this.rowAL[row - 1];

                //如果请求列数据大于当前行的列时,返回空值
                if (colAL.Count < col)
                {
                    return "";
                }

                return colAL[col - 1].ToString();
            }
        }

        //根据最小行,最大行,最小列,最大列,来生成一个DataTable类型的数据
        //maxrow:-1代表最大行
        //maxcol:-1代表最大列
        //public DataTable this[int minRow, int maxRow, int minCol, int maxCol]
        //{
        //    get
        //    {
        //        //数据有效性验证
        //        CheckRowValid(minRow);
        //        CheckMaxRowValid(maxRow);
        //        CheckColValid(minCol);
        //        CheckMaxColValid(maxCol);

        //        if (maxRow == -1)
        //        {
        //            maxRow = RowCount;
        //        }
        //        if (maxCol == -1)
        //        {
        //            maxCol = ColCount;
        //        }

        //        if (maxRow < minRow)
        //        {
        //            throw new Exception("最大行数不能小于最小行数");
        //        }
        //        if (maxCol < minCol)
        //        {
        //            throw new Exception("最大列数不能小于最小列数");
        //        }

        //        DataTable csvDT = new DataTable();
        //        int i;
        //        int col;
        //        int row;

        //        //增加列
        //        for (i = minCol; i <= maxCol; i++)
        //        {
        //            csvDT.Columns.Add(i.ToString());
        //        }
        //        for (row = minRow; row <= maxRow; row++)
        //        {
        //            DataRow csvDR = csvDT.NewRow();
        //            i = 0;
        //            for (col = minCol; col <= maxCol; col++)
        //            {
        //                csvDR[i] = this[row, col];
        //                i++;
        //            }
        //            csvDT.Rows.Add(csvDR);
        //        }
        //        return csvDT;
        //    }
        //}

        //检查行数是否有效的
        private void CheckRowValid(int row)
        {
            if (row <= 0)
            {
                throw new Exception("行数不能小于0");
            }
            if (row > RowCount)
            {
                throw new Exception("没有当前行的数据");
            }
        }

        //检查最大行数是否是有效的
        private void CheckMaxRowValid(int maxRow)
        {
            if (maxRow <= 0 && maxRow != -1)
            {
                throw new Exception("行数不能等于0或小于-1");
            }
            if (maxRow > RowCount)
            {
                throw new Exception("没有当前行的数据");
            }
        }

        //检查列数是否有效的
        private void CheckColValid(int col)
        {
            if (col <= 0)
            {
                throw new Exception("列数不能小于0");
            }
            if (col > ColCount)
            {
                throw new Exception("没有当前列的数据");
            }
        }

        //检查最大列数是否是有效的
        private void CheckMaxColValid(int maxCol)
        {
            if (maxCol <= 0 && maxCol != -1)
            {
                throw new Exception("列数不能等于0或小于-1");
            }
            if (maxCol > ColCount)
            {
                throw new Exception("没有当前列的数据");
            }
        }

        public bool LoadCsvFile()
        {
            if (this.fileName == null)
            {
                return false;
            }
            //else if (!File.Exists(this.fileName))
            //{
            //    return false;
            //}
            else
            {
            }
            if (this.encoding == null)
            {
                this.encoding = Encoding.Unicode;
            }

            StreamReader sr = new StreamReader(this.fileName, this.encoding);
            string csvDataLine;

            csvDataLine = "";
            while (true)
            {
                string fileDataLine;

                fileDataLine = sr.ReadLine();
                if (fileDataLine == null)
                {
                    break;
                }
                if (fileDataLine.StartsWith("\t"))
                {
                    break;
                }
                if (csvDataLine == "")
                {
                    csvDataLine = fileDataLine;
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;
                }

                if (!IfOddQuota(csvDataLine))
                {
                    AddNewDataLine(csvDataLine);
                    csvDataLine = "";
                }
            }
            sr.Close();

            //数据行出现奇数个引号
            if (csvDataLine.Length > 0)
            {
                return false;
            }

            return true;

        }

        //获取两个连续引号变成单个引号的数据行
        private string GetDeleteQuotaDataLine(string fileDataLine)
        {
            return fileDataLine.Replace("\"\"", "\"");
        }

        //判断字符串是否包含奇数个引号
        //奇数返回真,否则返回假
        private bool IfOddQuota(string dataLine)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataLine.Length; i++)
            {
                if (dataLine[i] == '\"')
                {
                    quotaCount++;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        //判断是否以奇数个引号开始
        private bool IfOddStartQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataCell.Length; i++)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        //判断是否以奇数个引号结尾
        private bool IfOddEndQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = dataCell.Length - 1; i >= 0; i--)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        private void AddNewDataLine(string newDataLine)
        {
            ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split('\t');

            bool oddStartQuota;//是否以奇数个引号开始

            string cellData;

            oddStartQuota = false;
            cellData = "";
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (oddStartQuota)
                {
                    //因为前面用逗号分割,所以要加上逗号
                    cellData += '\t' + dataArray[i];
                    //是否以奇数个引号结尾
                    if (IfOddEndQuota(dataArray[i]))
                    {
                        colAL.Add(GetHandleData(cellData));
                        oddStartQuota = false;
                        continue;
                    }
                }
                else
                {
                    //是否以奇数个引号开始
                    if (IfOddStartQuota(dataArray[i]))
                    {
                        //是否以奇数个引号结尾,不能是一个双引号,并且不是奇数个引号
                        if (IfOddEndQuota(dataArray[i]) && dataArray[i].Length > 2 && !IfOddQuota(dataArray[i]))
                        {
                            colAL.Add(GetHandleData(dataArray[i]));
                            oddStartQuota = false;
                            continue;
                        }
                        else
                        {
                            oddStartQuota = true;
                            cellData = dataArray[i];
                            continue;
                        }
                    }
                    else
                    {
                        colAL.Add(GetHandleData(dataArray[i]));
                    }
                }
            }
            if (oddStartQuota)
            {
                throw new Exception("数据格式有问题");
            }
            this.rowAL.Add(colAL);
        }

        //去掉各自的首尾引号,把双引号变成单引号
        private string GetHandleData(string fileCellData)
        {
            if (fileCellData == "")
            {
                return "";
            }

            if (IfOddStartQuota(fileCellData))
            {
                if (IfOddEndQuota(fileCellData))
                {
                    return fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\"");
                }
                else
                {
                    throw new Exception("数据引号无法匹配" + fileCellData);
                }
            }
            else
            {
                if (fileCellData.Length > 2 && fileCellData[0] == '\"')
                {
                    fileCellData = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\"");
                }
            }

            return fileCellData;
        }
    }
}
#endif
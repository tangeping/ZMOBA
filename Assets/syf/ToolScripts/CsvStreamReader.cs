#if UNITY_EDITOR
using System;
using System.Text;
using System.Collections;
using System.IO;
//using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;

//����CSV���ļ�

namespace CSV
{
    public class CsvStreamReader
    {
        private ArrayList rowAL;//������,CSV�ļ���ÿһ�о���һ����
        private string fileName;//�ļ���

        private Encoding encoding;//����

        public CsvStreamReader()
        {
            this.rowAL = new ArrayList();
            this.fileName = "";
            this.encoding = Encoding.Unicode;
        }

        //fileName:�ļ���,�����ļ�·��
        public CsvStreamReader(string fileName)
        {
            this.rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = Encoding.Unicode;
        }

        //fileName:�ļ���,�����ļ�·��
        //encoding:�ļ�����
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
        //�ļ���,�����ļ�·��
        public string FileName
        {
            set
            {
                this.fileName = value;
                LoadCsvFile();
            }
        }

        //�ļ�����
        public Encoding FileEncoding
        {
            set
            {
                this.encoding = value;
            }
        }

        //��ȡ����
        public int RowCount
        {
            get
            {
                return this.rowAL.Count;
            }
        }

        //��ȡ����
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

        //��ȡĳ��ĳ�е�����
        public string this[int row, int col]
        {
            get
            {
                //������Ч����֤
                CheckRowValid(row);
                CheckColValid(col);
                ArrayList colAL = (ArrayList)this.rowAL[row - 1];

                //������������ݴ��ڵ�ǰ�е���ʱ,���ؿ�ֵ
                if (colAL.Count < col)
                {
                    return "";
                }

                return colAL[col - 1].ToString();
            }
        }

        //������С��,�����,��С��,�����,������һ��DataTable���͵�����
        //maxrow:-1���������
        //maxcol:-1���������
        //public DataTable this[int minRow, int maxRow, int minCol, int maxCol]
        //{
        //    get
        //    {
        //        //������Ч����֤
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
        //            throw new Exception("�����������С����С����");
        //        }
        //        if (maxCol < minCol)
        //        {
        //            throw new Exception("�����������С����С����");
        //        }

        //        DataTable csvDT = new DataTable();
        //        int i;
        //        int col;
        //        int row;

        //        //������
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

        //��������Ƿ���Ч��
        private void CheckRowValid(int row)
        {
            if (row <= 0)
            {
                throw new Exception("��������С��0");
            }
            if (row > RowCount)
            {
                throw new Exception("û�е�ǰ�е�����");
            }
        }

        //�����������Ƿ�����Ч��
        private void CheckMaxRowValid(int maxRow)
        {
            if (maxRow <= 0 && maxRow != -1)
            {
                throw new Exception("�������ܵ���0��С��-1");
            }
            if (maxRow > RowCount)
            {
                throw new Exception("û�е�ǰ�е�����");
            }
        }

        //��������Ƿ���Ч��
        private void CheckColValid(int col)
        {
            if (col <= 0)
            {
                throw new Exception("��������С��0");
            }
            if (col > ColCount)
            {
                throw new Exception("û�е�ǰ�е�����");
            }
        }

        //�����������Ƿ�����Ч��
        private void CheckMaxColValid(int maxCol)
        {
            if (maxCol <= 0 && maxCol != -1)
            {
                throw new Exception("�������ܵ���0��С��-1");
            }
            if (maxCol > ColCount)
            {
                throw new Exception("û�е�ǰ�е�����");
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

            //�����г�������������
            if (csvDataLine.Length > 0)
            {
                return false;
            }

            return true;

        }

        //��ȡ�����������ű�ɵ������ŵ�������
        private string GetDeleteQuotaDataLine(string fileDataLine)
        {
            return fileDataLine.Replace("\"\"", "\"");
        }

        //�ж��ַ����Ƿ��������������
        //����������,���򷵻ؼ�
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

        //�ж��Ƿ������������ſ�ʼ
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

        //�ж��Ƿ������������Ž�β
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

            bool oddStartQuota;//�Ƿ������������ſ�ʼ

            string cellData;

            oddStartQuota = false;
            cellData = "";
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (oddStartQuota)
                {
                    //��Ϊǰ���ö��ŷָ�,����Ҫ���϶���
                    cellData += '\t' + dataArray[i];
                    //�Ƿ������������Ž�β
                    if (IfOddEndQuota(dataArray[i]))
                    {
                        colAL.Add(GetHandleData(cellData));
                        oddStartQuota = false;
                        continue;
                    }
                }
                else
                {
                    //�Ƿ������������ſ�ʼ
                    if (IfOddStartQuota(dataArray[i]))
                    {
                        //�Ƿ������������Ž�β,������һ��˫����,���Ҳ�������������
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
                throw new Exception("���ݸ�ʽ������");
            }
            this.rowAL.Add(colAL);
        }

        //ȥ�����Ե���β����,��˫���ű�ɵ�����
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
                    throw new Exception("���������޷�ƥ��" + fileCellData);
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
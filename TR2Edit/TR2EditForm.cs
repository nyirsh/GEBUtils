using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TR2Edit
{
    public partial class TR2EditForm : Form
    {
        private static int CompressionLevel = 0;

        static TR2 loadedTR2;
        static string tr2Path = "";
        static bool isSaved = true;

        public TR2EditForm()
        {
            InitializeComponent();
        }

        #region Form Events

        #region ToolStripMenu

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "tr2 database (.tr2)|*.tr2";
            ofd.ShowDialog();

            if ((ofd.FileName != "") && (ofd.FileName != null))
            {
                this.Text = "TR2 Editor - " + Path.GetFileName(ofd.FileName);

                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                optionsToolStripMenuItem.Enabled = true;
                toolsToolStripMenuItem.Enabled = true;

                foreach (Control cc in searchPanel.Controls)
                {
                    cc.Enabled = true;
                }

                dataTable.Rows.Clear();
                dataTable.Columns.Clear();

                OpenTR2(ofd.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((tr2Path != "") && (tr2Path != null))
            {
                dataTable.EndEdit();
                SaveTR2(tr2Path);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((tr2Path != "") && (tr2Path != null))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "tr2 files (.tr2)|*.tr2";
                sfd.ShowDialog();

                if ((sfd.FileName != "") && (sfd.FileName != null))
                {
                    dataTable.EndEdit();
                    SaveTR2(sfd.FileName);
                }
            }
        }

        #endregion

        private void TR2EditForm_Resize(object sender, EventArgs e)
        {
            dataTable.Size = new Size(this.Size.Width - 40, this.Size.Height - 105);
        }

        #endregion

        #region Save Functions

        public void SaveTR2(string TR2fileName)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 1; j < dataTable.Columns.Count; j++)
                {
                    if (loadedTR2.columnsList[j - 1].dataLenght == -1)
                    {
                        string theValue = dataTable.Rows[i].Cells[j].Value.ToString();
                        theValue = theValue.Replace("\\n", "\n");
                        dataTable.Rows[i].Cells[j].Value = theValue;
                    }
                }
            }

            // Building Header
            byte[] header = new byte[0x40];
            Buffer.BlockCopy(TR2Info.header, 0, header, 0, TR2Info.header.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(loadedTR2.tableType), 0, header, 0x06, 2);

            bool hasSpecialHeader = false;
            if (loadedTR2.fileName.Contains("enus"))
                hasSpecialHeader = true;
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(loadedTR2.fileName), 0, header, 0x08, Encoding.UTF8.GetByteCount(loadedTR2.fileName));
            header[0x38] = 0x40;
            Buffer.BlockCopy(BitConverter.GetBytes(loadedTR2.columnNum), 0, header, 0x3C, 4);

            // Building the TableHeader - Part 1
            byte[] tableHeader = new byte[loadedTR2.columnNum * 0x14];
            for (int i = 0; i < loadedTR2.columnNum; i++)
            {
                tableHeader[0 + (i * 0x14)] = BitConverter.GetBytes(loadedTR2.columnsList[i].id)[0];
                tableHeader[0x8 + (i * 0x14)] = 0x14;
            }

            // Building the RowsHeader Values Table
            int rowHeaderValuesLenght = 0x04 + (0x04 * dataTable.Rows.Count);

            int totLenght = header.Length + tableHeader.Length + rowHeaderValuesLenght;

            if (CompressionLevel == 0)
            {
                if (totLenght % 0x10 != 0)
                    rowHeaderValuesLenght += 0x10 - (totLenght % 0x10);
            }
            byte[] rowHeaderValues = new byte[rowHeaderValuesLenght];

            Buffer.BlockCopy(BitConverter.GetBytes(dataTable.Rows.Count), 0, rowHeaderValues, 0, 0x04);

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                try
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((int)dataTable.Rows[i].Cells[0].Value), 0, rowHeaderValues, (0x04 + (0x04 * i)), 0x04);
                }
                catch
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(int.Parse((string)dataTable.Rows[i].Cells[0].Value)), 0, rowHeaderValues, (0x04 + (0x04 * i)), 0x04);
                }
            }

            //  Building Columns Headers Array
            List<byte[]> columnHeaders = new List<byte[]>();

            for (int i = 0; i < loadedTR2.columnNum; i++)
            {
                byte[] currColumnHeader = new byte[0x7C];
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(loadedTR2.columnsList[i].name), 0, currColumnHeader, 0, Encoding.UTF8.GetByteCount(loadedTR2.columnsList[i].name));
                Buffer.BlockCopy(TR2Info.columnNameSeparator, 0, currColumnHeader, 0x3C, 0x04);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(loadedTR2.columnsList[i].dataType), 0, currColumnHeader, 0x40, Encoding.UTF8.GetByteCount(loadedTR2.columnsList[i].dataType));
                Buffer.BlockCopy(loadedTR2.columnsList[i].dataTypeBytes, 0, currColumnHeader, 0x70, 0x0C);

                columnHeaders.Add(currColumnHeader);
            }

            // Building the RowsHeader Columns Values - Part 1
            List<byte[]> rowsHeaderColumns = new List<byte[]>();

            int multiplier = 3;
            if (CompressionLevel == 2)
                multiplier = 2;
            for (int i = 0; i < loadedTR2.columnNum; i++)
            {        
                byte[] currRowsHeaderColumns = new byte[0x04 * (dataTable.Rows.Count + multiplier)];
                Buffer.BlockCopy(BitConverter.GetBytes(dataTable.Rows.Count), 0, currRowsHeaderColumns, 0, 0x04);
                
                rowsHeaderColumns.Add(currRowsHeaderColumns);
            }

            // Building the RowsColumns Values
            List<byte[]> rowsColumnsValues = new List<byte[]>();

            for (int i = 0; i < loadedTR2.columnNum; i++)
            {
                int currTableLenght = 0;

                if (loadedTR2.columnsList[i].dataLenght >= 0)
                {
                    for (int x = 0; x < dataTable.Rows.Count; x++)
                    {
                        if (dataTable.Rows[x].Cells[i + 1].Value != null)
                        {
                            if (loadedTR2.columnsList[i].dataLenght != loadedTR2.columnsList[i].singleDataLenght)
                            {
                                string valueToSplit = (string)dataTable.Rows[x].Cells[i + 1].Value;
                                string[] splittedValues = valueToSplit.Split(" |".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                currTableLenght += splittedValues.Length * loadedTR2.columnsList[i].singleDataLenght;
                            }
                            else
                            {
                                currTableLenght += loadedTR2.columnsList[i].dataLenght;
                            }
                        }
                    }
                    //currTableLenght = loadedTR2.rowNum * loadedTR2.columnsList[i].dataLenght;
                }
                else
                {
                    for (int x = 0; x < dataTable.Rows.Count; x++)
                    {
                        string currString = (string)dataTable.Rows[x].Cells[i + 1].Value;
                        if (loadedTR2.columnsList[i].dataType == "ASCII")
                            currTableLenght += Encoding.ASCII.GetByteCount(currString) + 1;
                        else
                            currTableLenght += Encoding.UTF8.GetByteCount(currString) + 1;
                    }
                }

                int currTotTableLenght = columnHeaders[i].Length + rowsHeaderColumns[i].Length + currTableLenght;

                if (CompressionLevel == 0)
                {
                    if (currTotTableLenght % 0x10 != 0)
                    {
                        currTableLenght += 0x10 - (currTotTableLenght % 0x10);
                    }
                }

                byte[] currTableValues = new byte[currTableLenght];

                int lastValueLenght = 0;
                int lastOffset = 0;
                for (int x = 0; x < dataTable.Rows.Count; x++)
                {
                    if (dataTable.Rows[x].Cells[i + 1].Value != null)
                    {
                        byte[] currValue;
                        if (loadedTR2.columnsList[i].dataLenght >= 0)
                        {
                            if (loadedTR2.columnsList[i].dataLenght != loadedTR2.columnsList[i].singleDataLenght)
                            {
                                string valueToSplit = (string)dataTable.Rows[x].Cells[i + 1].Value;
                                string[] splittedValues = valueToSplit.Split(" |".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                int doneTot = 0;
                                currValue = new byte[loadedTR2.columnsList[i].singleDataLenght * splittedValues.Length];
                                foreach (string splittedValue in splittedValues)
                                {
                                    byte[] splitValueBuffer = BitConverter.GetBytes(int.Parse(splittedValue));
                                    Buffer.BlockCopy(splitValueBuffer, 0, currValue, doneTot, loadedTR2.columnsList[i].singleDataLenght);
                                    doneTot += loadedTR2.columnsList[i].singleDataLenght;
                                }

                                lastValueLenght = currValue.Length;
                            }
                            else
                            {
                                currValue = BitConverter.GetBytes((int)dataTable.Rows[x].Cells[i + 1].Value);
                                lastValueLenght = loadedTR2.columnsList[i].dataLenght;
                            }
                        }
                        else
                        {
                            if (loadedTR2.columnsList[i].dataType == "UTF-8")
                                currValue = Encoding.UTF8.GetBytes((string)dataTable.Rows[x].Cells[i + 1].Value);
                            else
                                currValue = Encoding.ASCII.GetBytes((string)dataTable.Rows[x].Cells[i + 1].Value);
                            lastValueLenght = currValue.Length;
                        }

                        // Adding Values
                        if (lastValueLenght <= currValue.Length)
                            Buffer.BlockCopy(currValue, 0, currTableValues, lastOffset, lastValueLenght);
                        else
                            Buffer.BlockCopy(currValue, 0, currTableValues, lastOffset, currValue.Length);
                    }
                    else
                        lastValueLenght = 0;

                    // Calculating values table offsets
                    int realOffset = columnHeaders[i].Length + rowsHeaderColumns[i].Length + lastOffset;
                    byte[] offsetBuffer = BitConverter.GetBytes(realOffset);
                    Buffer.BlockCopy(offsetBuffer, 0, rowsHeaderColumns[i], (0x04 + (0x04 * x)), 0x04);

                    lastOffset += lastValueLenght;
                    if (loadedTR2.columnsList[i].dataLenght < 0)
                        lastOffset++;
                }

                // Inserting the last offset
                int realLastOffset = columnHeaders[i].Length + rowsHeaderColumns[i].Length + lastOffset;
                byte[] lastOffsetBuffer = BitConverter.GetBytes(realLastOffset);
                Buffer.BlockCopy(lastOffsetBuffer, 0, rowsHeaderColumns[i], (0x04 + (0x04 * dataTable.Rows.Count)), 0x04);

                rowsColumnsValues.Add(currTableValues);
            }

            // Creating Full Tables
            List<byte[]> fullTables = new List<byte[]>();

            int lastTableOffset = 0;
            for (int i = 0; i < loadedTR2.columnNum; i++)
            {
                int tableLenght = columnHeaders[i].Length + rowsHeaderColumns[i].Length + rowsColumnsValues[i].Length;

                if (CompressionLevel == 0)
                {
                    if (tableLenght % 0x10 != 0)
                        tableLenght += 0x10 - (tableLenght % 0x10);
                }
                byte[] currFullTable = new byte[tableLenght];

                Buffer.BlockCopy(columnHeaders[i], 0, currFullTable, 0, columnHeaders[i].Length);
                Buffer.BlockCopy(rowsHeaderColumns[i], 0, currFullTable, columnHeaders[i].Length, rowsHeaderColumns[i].Length);
                Buffer.BlockCopy(rowsColumnsValues[i], 0, currFullTable, columnHeaders[i].Length + rowsHeaderColumns[i].Length, rowsColumnsValues[i].Length);

                fullTables.Add(currFullTable);

                int fullTableOffset = header.Length + rowHeaderValues.Length + tableHeader.Length + lastTableOffset;
                Buffer.BlockCopy(BitConverter.GetBytes(fullTableOffset), 0, tableHeader, (0x04 + (0x14 * i)), 0x04);

                Buffer.BlockCopy(BitConverter.GetBytes(tableLenght), 0, tableHeader, (0x0C + (0x14 * i)), 0x04);
                Buffer.BlockCopy(BitConverter.GetBytes(tableLenght), 0, tableHeader, (0x10 + (0x14 * i)), 0x04);

                lastTableOffset += tableLenght;
            }

            // Saving File
            FileStream outTr2 = new FileStream(TR2fileName, FileMode.Create, FileAccess.Write);
            outTr2.Write(header, 0, header.Length);
            outTr2.Write(tableHeader, 0, tableHeader.Length);
            outTr2.Write(rowHeaderValues, 0, rowHeaderValues.Length);
            
            foreach (byte[] currFullTable in fullTables)
            {
                outTr2.Write(currFullTable, 0, currFullTable.Length);
            }

            if (loadedTR2.hasFinalHeader)
            {
                outTr2.Write(loadedTR2.endingPointers, 0, loadedTR2.endingPointers.Length);

                string finalName = loadedTR2.fileName.Replace("[enus]", "");
                outTr2.Write(Encoding.UTF8.GetBytes(finalName), 0, Encoding.UTF8.GetByteCount(finalName));

                if (hasSpecialHeader)
                    outTr2.Write(TR2Info.specialendingheader, 0, TR2Info.specialendingheader.Length);
                else
                    outTr2.Write(TR2Info.endingheader, 0, TR2Info.endingheader.Length);
            }
            outTr2.Flush();
            outTr2.Close();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 1; j < dataTable.Columns.Count; j++)
                {
                    if (loadedTR2.columnsList[j - 1].dataLenght == -1)
                    {
                        string theValue = dataTable.Rows[i].Cells[j].Value.ToString();
                        theValue = theValue.Replace("\n", "\\n");
                        dataTable.Rows[i].Cells[j].Value = theValue;
                    }
                }
            }

            isSaved = true;
            MessageBox.Show("Saved File: " + Path.GetFileName(TR2fileName) + "\nCompression: " + CompressionLevel);
        }

        public void OpenTR2(string TR2fileName)
        {
            tr2Path = TR2fileName;
            loadedTR2 = new TR2(tr2Path);

            // Adding header column
            DataGridViewTextBoxColumn headColumn = new DataGridViewTextBoxColumn();
            headColumn.ValueType = Type.GetType("System.Int32");
            headColumn.Name = loadedTR2.fileName;
            dataTable.Columns.Add(headColumn);
            columnListBox.Items.Add(loadedTR2.fileName);

            // Adding dynamic columns
            for (int i = 0; i < loadedTR2.columnNum; i++)
            {
                DataGridViewTextBoxColumn currColumn = new DataGridViewTextBoxColumn();
                if (loadedTR2.columnsList[i].dataLenght == loadedTR2.columnsList[i].singleDataLenght)
                {
                    int int8 = 0;
                    Int16 int16 = 0;
                    Int32 int32 = 0;
                    UInt16 uint16 = 0;

                    switch (loadedTR2.columnsList[i].dataType.ToLower())
                    {
                        case "int8":
                            currColumn.ValueType = int8.GetType();
                            break;
                        case "uint8":
                            currColumn.ValueType = int8.GetType();
                            break;
                        case "int16":
                            currColumn.ValueType = int16.GetType();
                            break;
                        case "uint16":
                            currColumn.ValueType = uint16.GetType();
                            break;
                        case "int32":
                            currColumn.ValueType = int32.GetType();
                            break;
                        case "utf-8":
                            currColumn.ValueType = "".GetType();
                            break;
                        case "ascii":
                            currColumn.ValueType = "".GetType();
                            break;
                        default:
                            currColumn.ValueType = Type.GetType(loadedTR2.columnsList[i].dataType);
                            break;
                    }
                }
                else
                    currColumn.ValueType = "".GetType();
                currColumn.Name = loadedTR2.columnsList[i].name;
                dataTable.Columns.Add(currColumn);

                columnListBox.Items.Add(currColumn.Name);
            }

            columnListBox.SelectedIndex = 0;

            // Adding Rows
            for (int i = 0; i < loadedTR2.rowNum; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell[] cells = new DataGridViewTextBoxCell[loadedTR2.columnNum + 1];

                cells[0] = new DataGridViewTextBoxCell();
                cells[0].Value = loadedTR2.rowHeaderValues[i];

                for (int j = 1; j <= loadedTR2.columnNum; j++)
                {
                    cells[j] = new DataGridViewTextBoxCell();
                    if (loadedTR2.columnsList[j - 1].dataLenght != loadedTR2.columnsList[j - 1].singleDataLenght)
                    {
                        int doneTot = 0;
                        string currValue = "";
                        byte[] dataBuffer = (byte[])loadedTR2.columnsList[j - 1].data[i];
                        if (dataBuffer != null)
                        {
                            while (doneTot != dataBuffer.Length)
                            {
                                byte[] valueBuffer = new byte[loadedTR2.columnsList[j - 1].singleDataLenght];
                                Buffer.BlockCopy(dataBuffer, doneTot, valueBuffer, 0, valueBuffer.Length);
                                currValue += TR2Utils.ConvertNumber(valueBuffer);
                                doneTot += valueBuffer.Length;

                                if (doneTot != dataBuffer.Length)
                                    currValue += " | ";
                            }
                        }
                        else
                            currValue = null;
                        cells[j].Value = currValue;
                    }
                    else
                    {
                        if (loadedTR2.columnsList[j - 1].dataLenght == -1)
                        {
                            string laStringa = (string)loadedTR2.columnsList[j - 1].data[i];
                            cells[j].Value = laStringa.Replace("\n", "\\n");
                        }
                        else
                            cells[j].Value = loadedTR2.columnsList[j - 1].data[i];
                    }
                }
                row.Cells.AddRange(cells);
                dataTable.Rows.Add(row);
            }
        }

        #endregion

        #region Compression Modifiers

        private void noCompressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noCompressionToolStripMenuItem.Checked = true;
            toolStripMenuItem3.Checked = false;
            toolStripMenuItem4.Checked = false;
            CompressionLevel = 0;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            noCompressionToolStripMenuItem.Checked = false;
            toolStripMenuItem3.Checked = true;
            toolStripMenuItem4.Checked = false;
            CompressionLevel = 1;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            noCompressionToolStripMenuItem.Checked = false;
            toolStripMenuItem3.Checked = false;
            toolStripMenuItem4.Checked = true;
            CompressionLevel = 2;
        }
        
        #endregion

        #region Tool

        private void tR2MergerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "tr2 database (.tr2)|*.tr2";
            ofd.ShowDialog();

            if ((ofd.FileName != "") && (ofd.FileName != null))
            {
                TR2 filler = new TR2(ofd.FileName);
                DataGridView fillerView = new DataGridView();
                fillerView.Rows.Clear();
                fillerView.Columns.Clear();

                // Adding header column
                DataGridViewTextBoxColumn headColumn = new DataGridViewTextBoxColumn();
                headColumn.Name = filler.fileName;
                fillerView.Columns.Add(headColumn);

                // Adding dynamic columns
                for (int i = 0; i < filler.columnNum; i++)
                {
                    DataGridViewTextBoxColumn currColumn = new DataGridViewTextBoxColumn();
                    if (filler.columnsList[i].dataLenght == filler.columnsList[i].singleDataLenght)
                        currColumn.ValueType = Type.GetType(filler.columnsList[i].dataType);
                    else
                        currColumn.ValueType = "".GetType();
                    currColumn.Name = filler.columnsList[i].name;
                    fillerView.Columns.Add(currColumn);
                }

                // Adding Rows
                for (int i = 0; i < filler.rowNum; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    DataGridViewTextBoxCell[] cells = new DataGridViewTextBoxCell[filler.columnNum + 1];

                    cells[0] = new DataGridViewTextBoxCell();
                    cells[0].Value = filler.rowHeaderValues[i];

                    for (int j = 1; j <= filler.columnNum; j++)
                    {
                        cells[j] = new DataGridViewTextBoxCell();
                        if (filler.columnsList[j - 1].dataLenght != filler.columnsList[j - 1].singleDataLenght)
                        {
                            int doneTot = 0;
                            string currValue = "";
                            byte[] dataBuffer = (byte[])filler.columnsList[j - 1].data[i];
                            if (dataBuffer != null)
                            {
                                while (doneTot != dataBuffer.Length)
                                {
                                    byte[] valueBuffer = new byte[filler.columnsList[j - 1].singleDataLenght];
                                    Buffer.BlockCopy(dataBuffer, doneTot, valueBuffer, 0, valueBuffer.Length);
                                    currValue += TR2Utils.ConvertNumber(valueBuffer);
                                    doneTot += valueBuffer.Length;

                                    if (doneTot != filler.columnsList[j - 1].dataLenght)
                                        currValue += " | ";
                                }
                            }
                            else
                                currValue = null;
                            cells[j].Value = currValue;
                        }
                        else
                        {
                            cells[j].Value = filler.columnsList[j - 1].data[i];
                        }
                    }
                    row.Cells.AddRange(cells);
                    fillerView.Rows.Add(row);
                }


                // Checking Fillers
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int index = 0; index < fillerView.Rows.Count - 1; index++)
                    {
                        if (fillerView.Rows[index].Cells[0].Value.Equals(dataTable.Rows[i].Cells[0].Value))
                        {
                            for (int j = 0; j < fillerView.Rows[index].Cells.Count; j++)
                            {
                                dataTable.Rows[i].Cells[j].Value = fillerView.Rows[index].Cells[j].Value;
                            }
                            break;
                        }
                    }
                }


            }
        }

        private void advancedRowControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataTable.AllowUserToAddRows == false)
            {
                advancedRowControlToolStripMenuItem.Checked = true;
                dataTable.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            }
            else
            {
                advancedRowControlToolStripMenuItem.Checked = false;
                dataTable.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }

            dataTable.AllowUserToAddRows = advancedRowControlToolStripMenuItem.Checked;
            dataTable.AllowUserToDeleteRows = advancedRowControlToolStripMenuItem.Checked;
            dataTable.RowHeadersVisible = advancedRowControlToolStripMenuItem.Checked;
        }

        private void accentsCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> columns = new List<int>();
            for (int i = 0; i < loadedTR2.columnNum; i++)
            {
                if (loadedTR2.columnsList[i].dataLenght < 0)
                {
                    columns.Add(i + 1);
                }
            }

            if (columns.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    foreach (int x in columns)
                    {
                        string toCheck = (string)dataTable.Rows[i].Cells[x].Value;
                        if (toCheck.Contains("à"))
                            toCheck = toCheck.Replace("à", "a'");
                        if (toCheck.Contains("è") || toCheck.Contains("é"))
                            toCheck = toCheck.Replace("è", "e'").Replace("é", "e'");
                        if (toCheck.Contains("ì"))
                            toCheck = toCheck.Replace("ì", "i'");
                        if (toCheck.Contains("ò"))
                            toCheck = toCheck.Replace("ò", "o'");
                        if (toCheck.Contains("ù"))
                            toCheck = toCheck.Replace("ù", "u'");
                        dataTable.Rows[i].Cells[x].Value = toCheck;
                    }
                }
            }

        }

        #endregion

        #region Search & Replace

        private void findButton_Click(object sender, EventArgs e)
        {
            bool notFound = true;
            if (search_valueBox.Text != "")
            {
                int columnIndex = columnListBox.SelectedIndex;

                if (columnIndex >= 0)
                {
                    DataGridViewSelectedCellCollection coll = dataTable.SelectedCells;
                    
                    int searchIndex = 0;
                    if (coll.Count > 0)
                    {
                        searchIndex = coll[0].RowIndex + 1;
                    }

                    if (searchIndex >= dataTable.Rows.Count)
                        searchIndex = 0;

                    for (int i = searchIndex; i < dataTable.Rows.Count; i++)
                    {
                        string toTest = dataTable.Rows[i].Cells[columnIndex].Value.ToString();
                        if (toTest.Contains(search_valueBox.Text))
                        {
                            notFound = false;
                            dataTable.Rows[i].Cells[columnIndex].Selected = true;
                            dataTable.Rows[searchIndex].Cells[columnIndex].Selected = false;
                            break;
                        }
                    }
                }

                if (notFound)
                {
                    MessageBox.Show("Reached the end of the table, value not found");
                }
            }
        }

        private void replaceAllButton_Click(object sender, EventArgs e)
        {
            bool notFound = true;
            if (search_valueBox.Text != "")
            {
                int columnIndex = columnListBox.SelectedIndex;

                if (columnIndex >= 0)
                {
                    int searchIndex = 0;

                    if (allToReplaceChecks.Checked == false)
                    {
                        DataGridViewSelectedCellCollection coll = dataTable.SelectedCells;
                        if (coll.Count > 0)
                        {
                            string toReplace = dataTable.Rows[coll[0].RowIndex].Cells[columnIndex].Value.ToString();
                            toReplace = toReplace.Replace(search_valueBox.Text, search_replaceBox.Text);
                            dataTable.Rows[coll[0].RowIndex].Cells[columnIndex].Value = toReplace;
                            searchIndex = coll[0].RowIndex + 1;
                        }
                    }
                    if (searchIndex >= dataTable.Rows.Count)
                        searchIndex = 0;

                    for (int i = searchIndex; i < dataTable.Rows.Count; i++)
                    {
                        string toTest = dataTable.Rows[i].Cells[columnIndex].Value.ToString();
                        if (toTest.Contains(search_valueBox.Text))
                        {
                            notFound = false;
                            if (allToReplaceChecks.Checked)
                            {
                                toTest = toTest.Replace(search_valueBox.Text, search_replaceBox.Text);
                                dataTable.Rows[i].Cells[columnIndex].Value = toTest;
                            }
                            else
                            {
                                dataTable.Rows[i].Cells[columnIndex].Selected = true;
                                dataTable.Rows[i].Cells[columnIndex].Selected = false;
                                break;
                            }
                        }
                    }

                    if (!notFound)
                    {
                        if ((searchIndex + 1) < dataTable.Rows.Count)
                            dataTable.Rows[searchIndex + 1].Cells[columnIndex].Selected = true;
                    }

                    dataTable.Refresh();

                    if ((!allToReplaceChecks.Checked) && (notFound))
                    {
                        MessageBox.Show("Reached the end of the table, value not found");
                    }
                    if (allToReplaceChecks.Checked)
                    {
                        MessageBox.Show("All occurrencies replaced");
                    }
                }
            }
        }

        #endregion

        #region Backups

        private void TR2EditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((tr2Path != "")&& (!isSaved))
            {
                try
                {
                    SaveTR2("bak-" + Path.GetFileName(tr2Path));
                }
                catch { }
            }
        }

        private void dataTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            isSaved = false;
        }

        #endregion

    }
}

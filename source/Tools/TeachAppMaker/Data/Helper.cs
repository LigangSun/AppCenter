using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Threading;

using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Controls;
using System.Collections;
using System.Diagnostics;
using SoonLearning.TeachAppMaker.Dialogs;
using SoonLearning.Assessment.Data;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Management;

namespace SoonLearning.TeachAppMaker.Data
{
    internal class Helper
    {
        // Build a table with a given number of rows and columns
        internal static Table UpdateTable(Table table,
                                         int rowCount,
                                         int columnCount,
                                         System.Windows.Media.Brush borderBrush,
                                         Thickness borderThickness,
                                         double dLineHeight,
                                         TableType tableType)
        {
            table.Tag = tableType;
            table.CellSpacing = 2;
            table.BorderBrush = borderBrush;
            table.BorderThickness = borderThickness;
            table.MouseEnter += new MouseEventHandler(table_MouseEnter);
            table.MouseLeave += new MouseEventHandler(table_MouseLeave);

            if (0 >= table.Columns.Count)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    TableColumn tableColumn = new TableColumn();
                    tableColumn.Width = double.IsNaN(dLineHeight) ? GridLength.Auto : new GridLength(dLineHeight);
                    table.Columns.Add(tableColumn);
                }
            }
            else
            {
                foreach (TableColumn tableColumn in table.Columns)
                {
                    tableColumn.Width = double.IsNaN(dLineHeight) ? GridLength.Auto : new GridLength(dLineHeight);
                }
            }

            foreach (TableRowGroup rowGroup in table.RowGroups)
            {
                foreach (TableRow row in rowGroup.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.BorderBrush = borderBrush;
                        cell.BorderThickness = borderThickness;
                    }
                }
            }

            return table;
        }

        internal static Table BuildTable(int rowCount,
                                         int columnCount,
                                         System.Windows.Media.Brush borderBrush,
                                         Thickness borderThickness,
                                         double dLineHeight,
                                         TableType tableType)
        {
            Table table = new Table();
            table.Tag = tableType;
            table.CellSpacing = 2;
            table.BorderBrush = borderBrush;
            table.BorderThickness = borderThickness;
            table.MouseEnter += new MouseEventHandler(table_MouseEnter);
            table.MouseLeave += new MouseEventHandler(table_MouseLeave);

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                TableColumn tableColumn = new TableColumn();
                tableColumn.Width = double.IsNaN(dLineHeight) ? GridLength.Auto : new GridLength(dLineHeight);
                table.Columns.Add(tableColumn);
            }

            TableRowGroup rowGroup = new TableRowGroup();
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                TableRow row = BuildTableRow(columnCount, borderBrush, borderThickness, dLineHeight);
                rowGroup.Rows.Add(row);
            }
            table.RowGroups.Add(rowGroup);
            return table;
        }

        public static void table_MouseLeave(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("table_MouseLeave");
            //if(null != TableDragHandle.LastEnteredObjectTable)
            //{
            //    Debug.WriteLine("table_MouseLeave - reset cursor");
            //    TableDragHandle.LastEnteredObjectTable.Cursor = Cursors.SizeWE;
            //    TableDragHandle.Reset();
            //    e.Handled = true;
            //}
            //Debug.WriteLine("table_MouseLeave - exit");
        }

        public static void table_MouseEnter(object sender, MouseEventArgs e)
        {
            Table table = sender as Table;
            if (null != table)
            {
                TableDragHandle.LastEnteredObjectTable = sender as Table;
            }
        }

        public static void cell_MouseEnter(object sender, MouseEventArgs e)
        {
            TableCell tableCell = sender as TableCell;
            //Debug.WriteLine("cell_MouseEnter dirOver: " + tableCell.IsMouseDirectlyOver + ", "+ e.GetPosition(tableCell).X + " " + e.GetPosition(tableCell).Y);
            if (null != tableCell && tableCell.IsMouseDirectlyOver)
            {
                //Point point = e.GetPosition(tableCell);

                tableCell.Cursor = Cursors.SizeWE;
            }
        }

        public static void cell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (null != TableDragHandle.LastEnteredObjectTable)
            {
                //TableDragHandle.LastEnteredObjectTable.Cursor = Cursors.Arrow;
                TableDragHandle.Reset();
                TableCell tableCell = sender as TableCell;
                if (null != tableCell)
                {
                    tableCell.Focus();
                    tableCell.Cursor = Cursors.Arrow;
                }
            }
        }

        public static void cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != TableDragHandle.LastEnteredObjectTable)
            {
                try
                {
                    TableCell tableCell = sender as TableCell;
                    if (null == tableCell || Cursors.SizeWE != tableCell.Cursor)
                    {
                        return;
                    }
                    TableRow tableRow = tableCell.Parent as TableRow;
                    if (null == tableRow)
                    {
                        return;
                    }

                    TableDragHandle.RowIdx = tableRow.Cells.IndexOf(tableCell);
                    TableDragHandle.DragStartPoint = e.GetPosition(tableCell);
                    GridLength gridLength = TableDragHandle.LastEnteredObjectTable.Columns[TableDragHandle.RowIdx].Width;
                    if (GridLength.Auto == gridLength)
                    {
                        Debug.WriteLine("cell_MouseLeftButtonDown - width is auto - return ");
                        return;
                    }
                    TableDragHandle.InitialCellWidth = (GridLength.Auto == gridLength ? 100 : gridLength.Value);
                    e.Handled = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("### Exception: " + ex);
                }
            }
        }

        public static void cell_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                TableDragHandle.IsHandleValid)
            {
                try
                {
                    TableCell tableCell = sender as TableCell;
                    if (null == tableCell)
                    {
                        return;
                    }
                    System.Windows.Point point = e.GetPosition(tableCell);
                    double dDistance = TableDragHandle.DragStartPoint.Value.X - point.X;
                    double dCurrLen = TableDragHandle.LastEnteredObjectTable.Columns[TableDragHandle.RowIdx].Width.Value;
                    double dNewSize = TableDragHandle.InitialCellWidth - dDistance;
                    TableDragHandle.LastEnteredObjectTable.Columns[TableDragHandle.RowIdx].Width = new GridLength(1 > dNewSize ? 1 : dNewSize);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("### Exception: " + ex);
                }
            }
        }

        internal static TableRow BuildTableRow(int columnCount,
                                               System.Windows.Media.Brush borderBrush,
                                               Thickness borderThickness,
                                                double dLineHeight)
        {
            TableRow row = new TableRow();

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                TableCell cell = BuildTableCell(borderBrush, borderThickness, dLineHeight);
                row.Cells.Add(cell);
            }

            return row;
        }

        internal static TableCell BuildTableCell(System.Windows.Media.Brush borderBrush,
                                               Thickness borderThickness,
                                                double dLineHeight)
        {
            TableCell cell = new TableCell(new Paragraph());
            cell.BorderBrush = borderBrush;
            cell.BorderThickness = borderThickness;

            cell.MouseLeftButtonDown += new MouseButtonEventHandler(cell_MouseLeftButtonDown);
            cell.MouseLeftButtonUp += new MouseButtonEventHandler(cell_MouseLeftButtonUp);
            cell.MouseMove += new MouseEventHandler(cell_MouseMove);
            cell.MouseEnter += new MouseEventHandler(cell_MouseEnter);
            return cell;
        }

        internal static bool HasAncestor(TextPointer position, Type ancestorType)
        {
            return GetAncestor(position, ancestorType) == null ? false : true;
        }

        internal static TextElement GetAncestor(TextPointer position, Type ancestorType)
        {
            TextElement parent = position.Parent as TextElement;
            while (parent != null)
            {
                if (ancestorType.IsAssignableFrom(parent.GetType()))
                {
                    return parent;
                }
                parent = parent.Parent as TextElement;
            }
            return null;
        }

        internal static InlineUIContainer GetAncestorInline(TextPointer position, Type ancestorType)
        {
            InlineUIContainer cont = null;
            Paragraph parent = position.Parent as Paragraph;
            Inline run = position.Parent as Inline;

            if (parent != null && parent.Inlines != null)
            {
                foreach (Inline inline in parent.Inlines)
                {
                    if (inline is InlineUIContainer)
                    {
                        cont = inline as InlineUIContainer;
                        //cont = parent.Inlines.FirstInline as InlineUIContainer;
                    }
                    else
                    {
                        cont = GetInlineUICont(inline as Span);
                    }
                    if (null != cont)
                    {
                        break;
                    }
                }
            }
            else if (run != null && run.NextInline != null)
            {
                //while(run.NextInline != null)
                cont = GetInlineFromCollection(run.SiblingInlines);
            }
            return cont;
        }

        private static InlineUIContainer GetInlineFromCollection(InlineCollection collection)
        {
            InlineUIContainer cont = null;

            foreach (Inline inline in collection)
            {
                cont = inline as InlineUIContainer;
                if (null == cont)
                {
                    cont = GetInlineUICont(inline as Span);
                }

                if (null != cont)
                {
                    break;
                }
            }
            return cont;
        }

        private static InlineUIContainer GetInlineUICont(Span parent)
        {
            InlineUIContainer cont = null;
            if (null != parent)
            {
                foreach (Inline inline in parent.Inlines)
                {
                    if (inline is InlineUIContainer)
                    {
                        cont = (InlineUIContainer)inline;
                        return cont;
                    }
                    else
                    {
                        cont = GetInlineUICont(inline as Span);
                    }
                    if (null != cont)
                    {
                        break;
                    }
                }
            }
            return cont;
        }

        internal static Table GetTableAncestor(TextPointer position)
        {
            TextElement textElement = GetAncestor(position, typeof(Table));
            return textElement as Table;
        }

        internal static TableRow GetTableRowAncestor(TextPointer position)
        {
            TextElement textElement = GetAncestor(position, typeof(TableRow));
            return textElement as TableRow;
        }

        internal static InlineUIContainer GetInlineUIContainer(TextPointer position)
        {
            return GetAncestorInline(position, typeof(InlineUIContainer));
        }

        internal static System.Windows.Controls.Image GetImageAncestor(TextPointer position)
        {
            InlineUIContainer inlineUIContainer = GetInlineUIContainer(position);
            return (null == inlineUIContainer ? null : inlineUIContainer.Child as System.Windows.Controls.Image);
        }

        internal static TableCell GetTableCellAncestor(TextPointer position)
        {
            TextElement textElement = GetAncestor(position, typeof(TableCell));
            return textElement as TableCell;
        }

        internal static int GetLineNumberFromSelection(TextPointer position)
        {
            if (position == null)
            {
                return 0;
            }

            int lineNumber = 0;
            int linesMoved;
            do
            {
                position = position.GetLineStartPosition(-1, out linesMoved);
                lineNumber++;
            }
            while (position != null && linesMoved != 0);

            return lineNumber;
        }

        internal static int GetColumnNumberFromSelection(TextPointer position)
        {
            if (position == null)
            {
                return 0;
            }

            int linesMoved;
            TextPointer lineStartPosition = position.GetLineStartPosition(0, out linesMoved);

            int columnNumber = 0;
            do
            {
                columnNumber++;
                position = position.GetNextInsertionPosition(LogicalDirection.Backward);
            }
            while (position != null && position.CompareTo(lineStartPosition) > 0);

            return columnNumber;
        }

        internal static string CreateImageFile(QuestionFractionPart fraction, string fontFamilly)
        {
            SizeF iamgeSize = SizeF.Empty;

            var image = new Bitmap(56, 200, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(image))
            {
                string temp = string.Empty;
                if (!string.IsNullOrEmpty(fraction.WithPart.Content))
                {
                    temp = fraction.WithPart.Content;
                }

                if (fraction.Numerator.Content.Length > fraction.Denominator.Content.Length)
                {
                    temp += fraction.Numerator.Content;
                }
                else
                {
                    temp += fraction.Denominator.Content;
                }

                temp += "99";

                using (Font font = new Font(fontFamilly, 14f))
                {
                    iamgeSize = g.MeasureString(temp, font);
                }
            }

            image.Dispose();

            image = new Bitmap((int)(iamgeSize.Width+1), (int)(iamgeSize.Height + 1) * 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(image))
            {
                RectangleF rectF = new RectangleF(new PointF(0f, 0f), iamgeSize);
                rectF.Height = rectF.Height * 2;

                using (Font font = new Font(fontFamilly, 14f))
                {
                    if (!string.IsNullOrEmpty(fraction.WithPart.Content))
                    {
                        StringFormat strFmt = new StringFormat();
                        strFmt.Alignment = StringAlignment.Near;
                        strFmt.LineAlignment = StringAlignment.Center;
                        g.DrawString(fraction.WithPart.Content, font, System.Drawing.Brushes.Black, rectF, strFmt);

                        SizeF withSize = g.MeasureString(fraction.WithPart.Content, font);

                        RectangleF rightRectF = new RectangleF(withSize.Width, 0, rectF.Width - withSize.Width, rectF.Height);

                        strFmt.Alignment = StringAlignment.Center;
                        strFmt.LineAlignment = StringAlignment.Near;
                        g.DrawString(fraction.Numerator.Content, font, System.Drawing.Brushes.Black, rightRectF, strFmt);

                        g.DrawLine(System.Drawing.Pens.Black, rightRectF.Left, rightRectF.Height / 2, rightRectF.Right, rightRectF.Height / 2);

                        strFmt.LineAlignment = StringAlignment.Far;
                        g.DrawString(fraction.Denominator.Content, font, System.Drawing.Brushes.Black, rightRectF, strFmt);
                    }
                    else
                    {
                        StringFormat strFmt = new StringFormat();
                        strFmt.Alignment = StringAlignment.Center;
                        strFmt.LineAlignment = StringAlignment.Near;
                        g.DrawString(fraction.Numerator.Content, font, System.Drawing.Brushes.Black, rectF, strFmt);

                        g.DrawLine(System.Drawing.Pens.Black, 0, iamgeSize.Height, rectF.Right, iamgeSize.Height);

                        strFmt.LineAlignment = StringAlignment.Far;
                        g.DrawString(fraction.Denominator.Content, font, System.Drawing.Brushes.Black, rectF, strFmt);
                    }
                }
            }

            string imageFile = getFractionImageFile(fraction.Id);
            if (File.Exists(imageFile))
                File.Delete(imageFile);

            image.Save(imageFile);

            return imageFile;
        }

        private static string getFractionImageFile(string id)
        {
            return System.IO.Path.Combine(ProjectMgr.Instance.ImagePath, id + ".png");
        }

        internal static bool Is64BitOS()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return (addressWidth == "64");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        internal static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
    }
}

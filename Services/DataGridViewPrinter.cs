using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace MajesticDesktop.Services
{
    public class DataGridViewPrinter : IDisposable
    {
        private readonly DataGridView _dgv;
        private readonly PrintDocument _printDocument;

        private int _currentRowIndex;
        private int _pageNumber;

        public Font HeaderFont { get; set; } = new Font("Segoe UI", 12, FontStyle.Bold);
        public Font CellFont { get; set; } = new Font("Segoe UI", 11, FontStyle.Regular);
        public Font FooterFont { get; set; } = new Font("Segoe UI", 9, FontStyle.Italic);
        public Brush TextBrush { get; set; } = Brushes.Black;
        public Pen GridPen { get; set; } = new Pen(Color.Gray, 1);
        public Color HeaderBackColor { get; set; } = Color.FromArgb(230, 230, 230);

        public DataGridViewPrinter(DataGridView dgv)
        {
            _dgv = dgv ?? throw new ArgumentNullException(nameof(dgv));
            _printDocument = new PrintDocument();
            _printDocument.DefaultPageSettings.Landscape = true;
            _printDocument.BeginPrint += PrintDocument_BeginPrint;
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        public void Print()
        {
            using (PrintDialog dialog = new PrintDialog())
            {
                dialog.Document = _printDocument;
                dialog.UseEXDialog = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _printDocument.Print();
                }
            }
        }

        public void PrintPreview()
        {
            using (PrintPreviewDialog preview = new PrintPreviewDialog())
            {
                preview.Document = _printDocument;
                preview.WindowState = FormWindowState.Maximized;
                preview.ShowDialog();
            }
        }

        private void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            _currentRowIndex = 0;
            _pageNumber = 0;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            _pageNumber++;

            Rectangle bounds = e.MarginBounds;
            float currentY = bounds.Top;

            List<DataGridViewColumn> visibleCols = _dgv.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            if (visibleCols.Count == 0)
            {
                e.HasMorePages = false;
                return;
            }

            float totalDgvWidth = visibleCols.Sum(c => c.Width);
            Dictionary<int, float> colWidths = new Dictionary<int, float>();

            foreach (var col in visibleCols)
            {
                float proportion = col.Width / totalDgvWidth;
                colWidths[col.Index] = proportion * bounds.Width;
            }

            StringFormat sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DirectionRightToLeft
            };

            // 1. الهيدر
            float headerHeight = 40;
            float currentX = bounds.Right;

            using (SolidBrush headerBgBrush = new SolidBrush(HeaderBackColor))
            {
                g.FillRectangle(headerBgBrush, bounds.Left, currentY, bounds.Width, headerHeight);
            }

            foreach (var col in visibleCols)
            {
                float colWidth = colWidths[col.Index];

                // FIX: كان بيقرا col.DefaultCellStyle.Alignment (ستايل العمود نفسه، غالبًا NotSet
                // لأن الـAlignment بيتحدد على مستوى الـGrid كله مش لكل عمود لوحده)، فكان بيطبع
                // دايمًا Near (مش متمركز) رغم إن الشاشة بتعرض العمود Center فعليًا.
                // InheritedStyle بيحسب الستايل الفعلي المُدمج (Column + Grid) زي المعروض بالظبط.
                sf.Alignment = GetAlignment(col.HeaderCell.InheritedStyle.Alignment);

                RectangleF cellRect = new RectangleF(currentX - colWidth, currentY, colWidth, headerHeight);
                g.DrawRectangle(GridPen, cellRect.Left, cellRect.Top, cellRect.Width, cellRect.Height);
                g.DrawString(col.HeaderText, HeaderFont, TextBrush, cellRect, sf);

                currentX -= colWidth;
            }

            currentY += headerHeight;

            // 2. البيانات
            float bottomLimit = bounds.Bottom - 30;

            while (_currentRowIndex < _dgv.Rows.Count)
            {
                DataGridViewRow row = _dgv.Rows[_currentRowIndex];

                if (row.IsNewRow)
                {
                    _currentRowIndex++;
                    continue;
                }

                float rowHeight = 35;

                if (currentY + rowHeight > bottomLimit)
                {
                    e.HasMorePages = true;
                    DrawFooter(g, bounds, _pageNumber);
                    return;
                }

                currentX = bounds.Right;

                foreach (var col in visibleCols)
                {
                    float colWidth = colWidths[col.Index];
                    RectangleF cellRect = new RectangleF(currentX - colWidth, currentY, colWidth, rowHeight);

                    g.DrawRectangle(GridPen, cellRect.Left, cellRect.Top, cellRect.Width, cellRect.Height);

                    DataGridViewCell cell = row.Cells[col.Index];
                    object val = cell.Value;
                    string text = val != null ? val.ToString() : string.Empty;

                    // FIX: نفس المبدأ - InheritedStyle بدل DefaultCellStyle
                    sf.Alignment = GetAlignment(cell.InheritedStyle.Alignment);
                    g.DrawString(text, CellFont, TextBrush, cellRect, sf);

                    currentX -= colWidth;
                }

                currentY += rowHeight;
                _currentRowIndex++;
            }

            DrawFooter(g, bounds, _pageNumber);
            e.HasMorePages = false;
        }

        private void DrawFooter(Graphics g, Rectangle bounds, int pageNum)
        {
            string footerText = $"صفحة {pageNum}";
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.DirectionRightToLeft
            };
            RectangleF footerRect = new RectangleF(bounds.Left, bounds.Bottom - 20, bounds.Width, 20);
            g.DrawString(footerText, FooterFont, TextBrush, footerRect, sf);
        }

        private StringAlignment GetAlignment(DataGridViewContentAlignment align)
        {
            switch (align)
            {
                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return StringAlignment.Center;
                case DataGridViewContentAlignment.TopLeft:
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.BottomLeft:
                    return StringAlignment.Far;
                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return StringAlignment.Near;
                default:
                    return StringAlignment.Near;
            }
        }

        public void Dispose()
        {
            _printDocument?.Dispose();
            HeaderFont?.Dispose();
            CellFont?.Dispose();
            FooterFont?.Dispose();
            GridPen?.Dispose();
        }
    }
}
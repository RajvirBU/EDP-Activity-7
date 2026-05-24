using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace NetAndBrewWinForms
{
    public class DashboardForm : Form
    {
        private TabControl tabControl;
        private TabPage tabUsers;
        private TabPage tabReports;

        // User Management Controls (Activity 5)
        private DataGridView gridUsers;
        private TextBox txtSearchUser;
        private Button btnSearchUser;
        private Button btnAddUser;
        private Button btnUpdateUser;
        private Button btnToggleStatus;

        // Reports Controls (Activity 6)
        private DataGridView gridReports;
        private Button btnExportExcel;
        private ComboBox comboTransactions;

        private DatabaseConnection db;

        public DashboardForm()
        {
            db = new DatabaseConnection();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            LoadDummyData();
        }

        private void InitializeComponent()
        {
            this.Text = "Net & Brew Dashboard";
            this.Size = new Size(1100, 650); // Increased width to prevent button clipping
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl = new TabControl { Dock = DockStyle.Fill };
            
            tabUsers = new TabPage("User Management (Activity 5)");
            tabUsers.BackColor = Color.FromArgb(30, 15, 50);
            tabReports = new TabPage("Transactions & Reports (Activity 6)");
            tabReports.BackColor = Color.FromArgb(30, 15, 50);

            SetupUserTab();
            SetupReportsTab();

            tabControl.TabPages.Add(tabUsers);
            tabControl.TabPages.Add(tabReports);

            this.Controls.Add(tabControl);
        }

        private void SetupUserTab()
        {
            FlowLayoutPanel panelTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10), AutoScroll = true, BackColor = Color.FromArgb(130, 82, 255) };
            
            if (File.Exists("logo.png")) {
                PictureBox pb = new PictureBox { Size = new Size(40, 40), SizeMode = PictureBoxSizeMode.Zoom, Margin = new Padding(0, 0, 15, 0) };
                try { pb.Image = Program.MakeCircularImage(Image.FromFile("logo.png")); } catch { }
                panelTop.Controls.Add(pb);
            }

            txtSearchUser = new TextBox { Width = 200, Margin = new Padding(5, 7, 5, 5), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle };
            btnSearchUser = CreateFlatButton("Search");
            btnSearchUser.Click += (s, e) => MessageBox.Show("Search executed.");

            btnAddUser = CreateFlatButton("Add Account");
            btnUpdateUser = CreateFlatButton("Update Account");
            btnToggleStatus = CreateFlatButton("Active / Inactive");

            panelTop.Controls.Add(txtSearchUser);
            panelTop.Controls.Add(btnSearchUser);
            panelTop.Controls.Add(btnAddUser);
            panelTop.Controls.Add(btnUpdateUser);
            panelTop.Controls.Add(btnToggleStatus);

            Button btnLogout = CreateFlatButton("Logout");
            btnLogout.BackColor = Color.FromArgb(200, 50, 50); // Red
            btnLogout.ForeColor = Color.White;
            btnLogout.Margin = new Padding(30, 5, 5, 5);
            btnLogout.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
            panelTop.Controls.Add(btnLogout);

            gridUsers = CreateStyledGrid();

            tabUsers.Controls.Add(gridUsers);
            tabUsers.Controls.Add(panelTop);
        }

        private void SetupReportsTab()
        {
            FlowLayoutPanel panelTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10), AutoScroll = true, BackColor = Color.FromArgb(130, 82, 255) };

            if (File.Exists("logo.png")) {
                PictureBox pb = new PictureBox { Size = new Size(40, 40), SizeMode = PictureBoxSizeMode.Zoom, Margin = new Padding(0, 0, 15, 0) };
                try { pb.Image = Program.MakeCircularImage(Image.FromFile("logo.png")); } catch { }
                panelTop.Controls.Add(pb);
            }

            Label lblType = new Label { Text = "Transaction Type:", AutoSize = true, Margin = new Padding(5, 10, 5, 5), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            
            comboTransactions = new ComboBox { Width = 150, Margin = new Padding(5, 7, 5, 5), Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            comboTransactions.Items.AddRange(new string[] { "Sales", "Inventory Count", "Staff Shifts" });
            comboTransactions.SelectedIndex = 0;
            comboTransactions.SelectedIndexChanged += ComboTransactions_SelectedIndexChanged;

            btnExportExcel = CreateFlatButton("Export to MS Excel");
            btnExportExcel.Margin = new Padding(15, 5, 5, 5);
            btnExportExcel.BackColor = Color.FromArgb(100, 200, 100); // Green for Excel
            btnExportExcel.ForeColor = Color.White;
            btnExportExcel.Click += BtnExportExcel_Click;

            panelTop.Controls.Add(lblType);
            panelTop.Controls.Add(comboTransactions);
            panelTop.Controls.Add(btnExportExcel);

            Button btnLogout = CreateFlatButton("Logout");
            btnLogout.BackColor = Color.FromArgb(200, 50, 50); // Red
            btnLogout.ForeColor = Color.White;
            btnLogout.Margin = new Padding(30, 5, 5, 5);
            btnLogout.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
            panelTop.Controls.Add(btnLogout);

            gridReports = CreateStyledGrid();

            tabReports.Controls.Add(gridReports);
            tabReports.Controls.Add(panelTop);
        }

        private Button CreateFlatButton(string text)
        {
            return new Button {
                Text = text,
                AutoSize = true,
                Margin = new Padding(5),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(130, 82, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private DataGridView CreateStyledGrid()
        {
            var grid = new DataGridView { 
                Dock = DockStyle.Fill, 
                AllowUserToAddRows = false, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(30, 15, 50),
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(130, 82, 255),
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false
            };
            grid.DefaultCellStyle.BackColor = Color.FromArgb(50, 30, 70);
            grid.DefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(130, 82, 255);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 50, 200);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            
            return grid;
        }

        private void ComboTransactions_SelectedIndexChanged(object? sender, EventArgs e)
        {
            DataTable dtReports = new DataTable();
            if (comboTransactions.SelectedIndex == 0) // Sales
            {
                dtReports.Columns.Add("TransactionID", typeof(int));
                dtReports.Columns.Add("Date", typeof(string));
                dtReports.Columns.Add("Amount", typeof(double));
                dtReports.Columns.Add("Cashier", typeof(string));

                dtReports.Rows.Add(101, "2024-01-01", 150.50, "Alice");
                dtReports.Rows.Add(102, "2024-01-02", 200.00, "Bob");
                dtReports.Rows.Add(103, "2024-01-03", 350.75, "Alice");
                dtReports.Rows.Add(104, "2024-01-04", 120.00, "Charlie");
            }
            else if (comboTransactions.SelectedIndex == 1) // Inventory
            {
                dtReports.Columns.Add("ItemID", typeof(int));
                dtReports.Columns.Add("ItemName", typeof(string));
                dtReports.Columns.Add("Stock", typeof(int));
                
                dtReports.Rows.Add(201, "Espresso Beans", 45);
                dtReports.Rows.Add(202, "Milk (Liters)", 20);
                dtReports.Rows.Add(203, "Syrup (Vanilla)", 15);
            }
            else if (comboTransactions.SelectedIndex == 2) // Staff Shifts
            {
                dtReports.Columns.Add("ShiftID", typeof(int));
                dtReports.Columns.Add("Employee", typeof(string));
                dtReports.Columns.Add("HoursWorked", typeof(int));
                dtReports.Columns.Add("ShiftDate", typeof(string));
                
                dtReports.Rows.Add(301, "Alice Smith", 8, "2024-01-01");
                dtReports.Rows.Add(302, "Bob Jones", 6, "2024-01-01");
                dtReports.Rows.Add(303, "Charlie Brown", 8, "2024-01-02");
            }
            gridReports.DataSource = dtReports;
        }

        private void LoadDummyData()
        {
            // Dummy data for Users (Activity 5)
            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add("ID");
            dtUsers.Columns.Add("Username");
            dtUsers.Columns.Add("Role");
            dtUsers.Columns.Add("Status");

            dtUsers.Rows.Add("1", "admin@netandbrew.com", "Admin", "Active");
            dtUsers.Rows.Add("2", "staff1@netandbrew.com", "Staff", "Active");
            dtUsers.Rows.Add("3", "oldstaff@netandbrew.com", "Staff", "Inactive");

            gridUsers.DataSource = dtUsers;
            
            // Trigger load of default report data
            ComboTransactions_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                FileName = "NetAndBrew_Report.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GenerateExcelReport(sfd.FileName);
                    MessageBox.Show("Excel report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerateExcelReport(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                // SHEET 1: Data and Template
                var ws = package.Workbook.Worksheets.Add("Transaction Report");

                // Header with Logo
                if (File.Exists("logo.png"))
                {
                    try
                    {
                        Image circular = Program.MakeCircularImage(Image.FromFile("logo.png"));
                        string tempPath = Path.Combine(Path.GetTempPath(), "netbrew_circle.png");
                        circular.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                        var picture = ws.Drawings.AddPicture("Logo", new FileInfo(tempPath));
                        picture.SetPosition(0, 5, 0, 5);
                        picture.SetSize(60, 60); // Clean size
                    }
                    catch { /* Ignore if invalid image */ }
                }

                ws.Cells["B1"].Value = "NET & BREW CAFE";
                ws.Cells["B1:E1"].Merge = true;
                ws.Cells["B1"].Style.Font.Size = 18;
                ws.Cells["B1"].Style.Font.Bold = true;
                ws.Cells["B1"].Style.Font.Color.SetColor(Color.FromArgb(130, 82, 255)); // Purple Theme
                ws.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                ws.Cells["B1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Row(1).Height = 35; // Taller row for logo

                ws.Cells["B2"].Value = "Official " + comboTransactions.Text + " Report";
                ws.Cells["B2:E2"].Merge = true;
                ws.Cells["B2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                // Load data from DataGrid
                DataTable dt = (DataTable)gridReports.DataSource;
                
                // Column Headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.Cells[5, i + 1].Value = dt.Columns[i].ColumnName;
                    ws.Cells[5, i + 1].Style.Font.Bold = true;
                    ws.Cells[5, i + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                // Data Rows
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        ws.Cells[6 + r, c + 1].Value = dt.Rows[r][c];
                    }
                }

                // AutoFit Columns to avoid cropped text
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                // Signature Placeholder
                int sigRow = 6 + dt.Rows.Count + 3;
                ws.Cells[$"A{sigRow}"].Value = "Prepared By:";
                ws.Cells[$"B{sigRow}"].Value = "_________________________";
                ws.Cells[$"A{sigRow + 1}"].Value = "Authorized Signature";
                ws.Cells[$"A{sigRow}:B{sigRow + 1}"].AutoFitColumns();

                // SHEET 2: Graph (Activity 6 requirement)
                var wsChart = package.Workbook.Worksheets.Add("Graph");
                var chart = wsChart.Drawings.AddChart("DataChart", eChartType.ColumnClustered);
                chart.Title.Text = comboTransactions.Text + " Overview";
                chart.SetPosition(1, 0, 1, 0);
                chart.SetSize(600, 400);

                // Dynamically use columns for chart depending on data
                int xAxisCol = 2; // Default 2nd col
                int yAxisCol = 3; // Default 3rd col
                var xRange = ws.Cells[6, xAxisCol, 5 + dt.Rows.Count, xAxisCol]; 
                var yRange = ws.Cells[6, yAxisCol, 5 + dt.Rows.Count, yAxisCol]; 

                var series = chart.Series.Add(yRange, xRange);
                series.Header = dt.Columns[yAxisCol - 1].ColumnName;

                // Save
                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}

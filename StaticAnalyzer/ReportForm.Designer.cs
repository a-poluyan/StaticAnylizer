namespace StaticAnalyzer
{
    partial class ReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lwReport = new System.Windows.Forms.ListView();
            this.File = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Details = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Criticality = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Suggestions = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lwReport
            // 
            this.lwReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.File,
            this.Location,
            this.Details,
            this.Criticality,
            this.Suggestions});
            this.lwReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lwReport.FullRowSelect = true;
            this.lwReport.GridLines = true;
            this.lwReport.HideSelection = false;
            this.lwReport.Location = new System.Drawing.Point(0, 0);
            this.lwReport.Name = "lwReport";
            this.lwReport.Size = new System.Drawing.Size(1606, 840);
            this.lwReport.TabIndex = 0;
            this.lwReport.UseCompatibleStateImageBehavior = false;
            this.lwReport.View = System.Windows.Forms.View.Details;
            // 
            // File
            // 
            this.File.Tag = "";
            this.File.Text = "Файл";
            this.File.Width = 511;
            // 
            // Location
            // 
            this.Location.Text = "Местонахождение";
            this.Location.Width = 111;
            // 
            // Details
            // 
            this.Details.Text = "Описание инцидента";
            this.Details.Width = 351;
            // 
            // Criticality
            // 
            this.Criticality.Text = "Критичность";
            this.Criticality.Width = 87;
            // 
            // Suggestions
            // 
            this.Suggestions.Text = "Предложения";
            this.Suggestions.Width = 121;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1606, 840);
            this.Controls.Add(this.lwReport);
            this.Name = "ReportForm";
            this.Text = "Отчёт по анализу";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lwReport;
        private System.Windows.Forms.ColumnHeader File;
        private System.Windows.Forms.ColumnHeader Location;
        private System.Windows.Forms.ColumnHeader Details;
        private System.Windows.Forms.ColumnHeader Criticality;
        private System.Windows.Forms.ColumnHeader Suggestions;
    }
}
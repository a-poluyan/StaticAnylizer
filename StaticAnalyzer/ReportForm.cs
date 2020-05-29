using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzer
{
    public partial class ReportForm : Form
    {
        public ReportForm(List<ListViewErrors> files)
        {
            InitializeComponent();

            foreach(var file in files)
            {
                foreach(var row in file.Errors)
                {
                    string[] lwRow = { file.FilePath, row.Location, row.Description, row.Criticality.ToString(), row.Suggestions };

                    var listView = new ListViewItem(lwRow);
                    listView.UseItemStyleForSubItems = false;

                    switch (row.Criticality)
                    {
                        case (ListViewErrors.Criticality.Низкий):
                            listView.SubItems[3].BackColor = Color.DarkGray;
                            break;

                        case (ListViewErrors.Criticality.Средний):
                            listView.SubItems[3].BackColor = Color.Yellow;
                            break;
                        case (ListViewErrors.Criticality.Высокий):
                            listView.SubItems[3].BackColor = Color.Orange;
                            break;
                        case (ListViewErrors.Criticality.Критический):
                            listView.SubItems[3].BackColor = Color.Tomato;
                            break;
                    }

                    lwReport.Items.Add(listView);
                }
            }
        }
    }
}

using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FORM
{
    public partial class FRM_DELI_STATUS_V6 : Form
    {
        public FRM_DELI_STATUS_V6()
        {
            InitializeComponent();
            tmrDate.Stop();
            tmrAnimation.Stop();
        }
        #region Variable
        int cCount = 0, cAnimated = 0;
        Random r = new Random();
        #endregion

        private void BindingLabelRData(Label lbl, int min, int max)
        {
            lbl.Text = string.Format("{0:n0}", r.Next(min, max));
        }

        private async Task BindingData4Chart(string Qtype)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                DatabaseTMS db = new DatabaseTMS();
                DataTable dt = new DataTable();
                lblOutgoing.Text = "OUTGOING TODAY";
                lblPlan.Text = "TOTAL PLAN";
                lblInventory.Text = "INVENTORY";
                lblShortage.Text = "SHORTAGE";
                if (ComVar.Var._strValue1.Equals("ALL"))
                    dt = db.TMS_DELIVERY_STATUS_DETAIL_V6("MES.PKG_TMS_DASHBOARD.TMS_GET_DELI_STATUS_V6", Qtype, ComVar.Var._strValue3, ComVar.Var._strValue4, DateTime.Now.ToString("yyyyMMdd"), ComVar.Var._strValue1);

                switch (Qtype)
                {
                    case "INVENTORY":
                        lblInventory_Total.Text = "0";
                        int InvQty = 0;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!string.IsNullOrEmpty(dr["LINE_NM"].ToString()))
                                    InvQty += Convert.ToInt32(dr["WIP_QTY"]);
                            }
                            lblInventory_Total.Text = string.Format("{0:n0}", InvQty);
                        }

                        chartINV.DataSource = dt;
                        chartINV.Series[0].ArgumentDataMember = "LINE_NM";
                        chartINV.Series[0].ValueDataMembers.AddRange(new string[] { "WIP_QTY" });
                        chartINV.Titles[0].Text = "Inventory Status By Plant";
                        ((DevExpress.XtraCharts.XYDiagram)chartINV.Diagram).AxisX.Title.Text = "Plant";
                        ((DevExpress.XtraCharts.XYDiagram)chartINV.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                        break;
                    case "OUTGOING":
                        lblOutgoing_Total.Text = "0";
                        if (dt.Rows.Count > 0)
                            lblOutgoing_Total.Text = string.Format("{0:n0}", Convert.ToInt32(dt.Compute("SUM(O_QTY)", "")));
                        chartOutgoing.DataSource = dt;
                        chartOutgoing.Series[0].ArgumentDataMember = "LINE_NM";
                        chartOutgoing.Series[0].ValueDataMembers.AddRange(new string[] { "O_QTY" });
                        ((DevExpress.XtraCharts.XYDiagram)chartOutgoing.Diagram).AxisX.Title.Text = "Plant";
                        chartOutgoing.Titles[0].Text = "Outgoing Status By Plant";
                        ((DevExpress.XtraCharts.XYDiagram)chartOutgoing.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                        break;
                    case "SHORTAGE":

                        int shrQty = 0, planQty = 0;
                        lblShortage_Total.Text = "0";
                        lblPlan_Total.Text = "0";
                        DataTable dtTemp = new DataTable();
                        if (dt.Rows.Count > 0)
                        {
                            dtTemp = dt.Select("SHR_QTY>0","LINE_CD").CopyToDataTable();
                            foreach (DataRow dr in dt.Rows)
                            {
                                shrQty += Convert.ToInt32(dr["SHR_QTY"]);
                                planQty += Convert.ToInt32(dr["PLAN_QTY"]);
                            }
                            lblShortage_Total.Text = string.Format("{0:n0}", shrQty);
                            lblPlan_Total.Text = string.Format("{0:n0}", planQty);
                        }
                        chartShortage.DataSource = dtTemp;
                        chartShortage.Series[0].ArgumentDataMember = "LINE_NM";
                        chartShortage.Series[0].ValueDataMembers.AddRange(new string[] { "PLAN_QTY" });
                        chartShortage.Series[1].ArgumentDataMember = "LINE_NM";
                        chartShortage.Series[1].ValueDataMembers.AddRange(new string[] { "SHR_QTY" });

                        ((XYDiagram)chartShortage.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;
                        ((XYDiagram)chartShortage.Diagram).AxisY.Title.Text = "Prs";
                        chartShortage.Titles[0].Text = "Shortage Status By Plant";
                        ((DevExpress.XtraCharts.XYDiagram)chartShortage.Diagram).AxisX.Title.Text = "Plant";
                        ((DevExpress.XtraCharts.XYDiagram)chartShortage.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                        if (dt.Rows.Count >= 11)
                        {
                            ((XYDiagram)chartShortage.Diagram).AxisX.VisualRange.SetMinMaxValues(dt.Rows[0]["LINE_NM"], dt.Rows[10]["LINE_NM"]);
                        }
                        break;
                }
                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                //MessageBox.Show(ex.Message);
            }
        }

        private async void BindingData()
        {

            var Task1 = BindingData4Chart("INVENTORY");
            var Task2 = BindingData4Chart("OUTGOING");
            var Task3 = BindingData4Chart("SHORTAGE");

            await Task.WhenAll(Task1, Task2, Task3);
        }

        private async void BindingData2()
        {
            var Task1 = BindingData4Chart2();
            await Task.WhenAll(Task1);
        }

        private async Task BindingData4Chart2()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                DatabaseTMS db = new DatabaseTMS();
                DataTable dt = new DataTable();
                DataTable dtTemp = new DataTable();

                lblOutgoing.Text = "TOTAL OUTGOING";
                lblPlan.Text = "TOTAL PLAN";
                lblInventory.Text = "INVENTORY";
                lblShortage.Text = "TOTAL SHORTAGE";

                DataSet ds = db.TMS_DELIVERY_DETAIL("MES.PKG_TMS_DASHBOARD.TMS_DELIVERY_DETAIL_V1", "VJ", ComVar.Var._strValue3, ComVar.Var._strValue4, DateTime.Now.ToString("yyyyMMdd"), string.IsNullOrEmpty(ComVar.Var._strValue1) ? "ALL" : ComVar.Var._strValue1);
                dt = ds.Tables[1];
                lblOutgoing_Total.Text = "0"; chartOutgoing.DataSource = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Columns.Remove(dt.Columns["STYLE_CD"]);
                    dt.Columns.Remove(dt.Columns["MODEL"]);
                    var result = from tab in dt.AsEnumerable()
                                 group tab by tab["DAYDAY"]
                    into groupDt
                                 select new
                                 {
                                     DAYDAY = groupDt.Key,
                                     O_QTY = groupDt.Sum((r) => decimal.Parse(r["O_QTY"].ToString()))
                                 };
                    DataTable boundTable = LINQResultToDataTable(result);
                    //Binding Outgoing
                    lblOutgoing_Total.Text = "0";
                    if (dt.Rows.Count > 0)
                        lblOutgoing_Total.Text = string.Format("{0:n0}", Convert.ToInt32(boundTable.Compute("SUM(O_QTY)", "")));
                    chartOutgoing.DataSource = boundTable;
                    chartOutgoing.Series[0].ArgumentDataMember = "DAYDAY";
                    chartOutgoing.Series[0].ValueDataMembers.AddRange(new string[] { "O_QTY" });
                    chartOutgoing.Titles[0].Text = "Shortage Status By Assembly Day";
                    ((DevExpress.XtraCharts.XYDiagram)chartOutgoing.Diagram).AxisX.Title.Text = "Assembly Day";

                    ((DevExpress.XtraCharts.XYDiagram)chartOutgoing.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                }
                //Inventory
                dt = ds.Tables[2];
                lblInventory_Total.Text = "0"; chartINV.DataSource = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblInventory_Total.Text = "0";
                    int InvQty = 0;
                    if (dt.Select("STYLE_CD = 'TOTAL'").Count() > 0)
                    {
                        dtTemp = dt.Select("STYLE_CD = 'TOTAL'").CopyToDataTable();
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            InvQty += Convert.ToInt32(dr["WIP_QTY"]);
                        }
                        lblInventory_Total.Text = string.Format("{0:n0}", InvQty);
                    }

                    chartINV.DataSource = dt.Select("STYLE_CD <> 'TOTAL'").CopyToDataTable();
                    chartINV.Series[0].ArgumentDataMember = "STYLE_CD";
                    chartINV.Series[0].ValueDataMembers.AddRange(new string[] { "WIP_QTY" });
                    chartINV.Titles[0].Text = "Inventory Status By Style Code";
                    ((DevExpress.XtraCharts.XYDiagram)chartINV.Diagram).AxisX.Title.Text = "Style Code";
                    ((DevExpress.XtraCharts.XYDiagram)chartINV.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                }
                //Shortage
                lblShortage_Total.Text = "0";
                lblPlan_Total.Text = "0";
                chartShortage.DataSource = null;
                if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {
                    dt = Pivot(ds.Tables[5], ds.Tables[5].Columns["DIV"], ds.Tables[5].Columns["QTY"]).Select("", "ASY_YMD").CopyToDataTable();

                    int shrQty = 0, planQty = 0;
                    lblShortage_Total.Text = "0";
                    lblPlan_Total.Text = "0";
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            shrQty += Convert.ToInt32(dr["SHR"]);
                            planQty += Convert.ToInt32(dr["PLAN"]);
                        }
                        lblShortage_Total.Text = string.Format("{0:n0}", shrQty);
                        lblPlan_Total.Text = string.Format("{0:n0}", planQty);
                    }
                    chartShortage.DataSource = dt;
                    chartShortage.Series[0].ArgumentDataMember = "ASY_YMD";
                    chartShortage.Series[0].ValueDataMembers.AddRange(new string[] { "PLAN" });
                    chartShortage.Series[1].ArgumentDataMember = "ASY_YMD";
                    chartShortage.Series[1].ValueDataMembers.AddRange(new string[] { "SHR" });

                    ((XYDiagram)chartShortage.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;
                    ((XYDiagram)chartShortage.Diagram).AxisY.Title.Text = "Prs";
                    chartShortage.Titles[0].Text = "Inventory Status By Assembly Day";
                    ((DevExpress.XtraCharts.XYDiagram)chartShortage.Diagram).AxisX.Title.Text = "Style Code";
                    ((DevExpress.XtraCharts.XYDiagram)chartShortage.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                }
                splashScreenManager1.CloseWaitForm();

            }
            catch//(Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                //  MessageBox.Show(ex.Message);
            }
        }
        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = Record.GetType().GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
        DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
            .Select(c => c.ColumnName)
            .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
            .Select(r => r[pivotColumn.ColumnName].ToString())
            .Distinct().ToList()
            .ForEach(c => result.Columns.Add(c, pivotValue.DataType));
            //.ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                pkColumnNames
                .Select(c => row[c])
                .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];


            }

            return result;
        }


        private void tmrDate_Tick(object sender, EventArgs e)
        {
            cCount++;
            lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd\nHH:mm:ss")); //Gán dữ liệu giờ cho label ngày giờ
            if (cCount >= 60)
            {
                cCount = 0;
                //Binding Data
                tmrAnimation.Start();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ComVar.Var.callForm = "back";
            tmrDate.Stop();
        }

        private void FRM_DELI_STATUS_V6_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd\nHH:mm:ss")); //Gán dữ liệu giờ cho label ngày giờ
                if (!string.IsNullOrEmpty(ComVar.Var._strValue2))
                    lblTitle.Text = ComVar.Var._strValue5.ToUpper() + " TMS - " + ComVar.Var._strValue2.ToUpper() + " DELIVERY STATUS";
                else
                    lblTitle.Text = ComVar.Var._strValue5.ToUpper() + " TMS - WORKSHOPS DELIVERY STATUS";
                cCount = 60;
                tmrDate.Start();
            }
            else
            {
                tmrDate.Stop();
            }
        }

        private void tmrAnimation_Tick(object sender, EventArgs e)
        {
            cAnimated++;

            #region Annimation
            BindingLabelRData(lblOutgoing_Total, 10000, 999999);
            BindingLabelRData(lblInventory_Total, 10000, 999999);
            BindingLabelRData(lblPlan_Total, 10000, 999999);
            BindingLabelRData(lblShortage_Total, 10000, 999999);

            #endregion

            if (cAnimated >= 10)
            {
                cAnimated = 0;
                tmrAnimation.Stop();

                BindingData(); //Get Data for All Factory

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using System.Globalization;

namespace FORM
{
    public partial class FRM_DELI_STATUS_V5 : Form
    {
        public FRM_DELI_STATUS_V5()
        {
            InitializeComponent();
        }
        int iCount = 0;
        DataSet ds = new DataSet();
        private void FRM_DELI_STATUS_V3_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                splashScreenManager1.ShowWaitForm();
                if (!string.IsNullOrEmpty(ComVar.Var._strValue2))
                    lblTitle.Text = ComVar.Var._strValue5.ToUpper() + " TMS - " + ComVar.Var._strValue2.ToUpper() + " DELIVERY STATUS";
                else
                    lblTitle.Text = ComVar.Var._strValue5.ToUpper() + " TMS - WORKSHOPS DELIVERY STATUS";
                lblComp_Inv.Text = string.Concat(ComVar.Var._strValue5, " Inventory");
                lblComp_Outgoing.Text = string.Concat(ComVar.Var._strValue5, " Outgoing");
                lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd\nHH:mm:ss")); //Gán dữ liệu giờ cho label ngày giờ
                iCount = 60;
                tmrDate.Start();
                splashScreenManager1.CloseWaitForm();
            }
            else
            { tmrDate.Stop(); }
        }

        private void GetDataGridSetBalanceChart()
        {
            DatabaseTMS db = new DatabaseTMS();
            try
            {

            }
            catch (Exception ex) { }
        }

        private void GetDataOutgoing_Chart()
        {
            try
            {
                lblOS_Outgoing.Text = "0 Prs";
                if (ds.Tables[1].Rows.Count > 0)
                    lblOS_Outgoing.Text = string.Concat(string.Format("{0:n0}", Convert.ToInt32(ds.Tables[1].Compute("SUM(O_QTY)", ""))), " Prs");
                chartControl1.DataSource = ds.Tables[1];
                chartControl1.Series[0].ArgumentDataMember = "LINE_NM";
                chartControl1.Series[0].ValueDataMembers.AddRange(new string[] { "O_QTY" });
                ((DevExpress.XtraCharts.XYDiagram)chartControl1.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;

                ((XYDiagram)chartControl1.Diagram).AxisX.VisualRange.SetMinMaxValues(ds.Tables[1].Rows[0]["LINE_NM"], ds.Tables[1].Rows[5]["LINE_NM"]);
            }
            catch { }
        }


        private void GetDataOutgoing_Grid()
        {
            try
            {
                DataTable dt = ds.Tables[1];
                gridControl1.DataSource = dt;
                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    gridView1.Columns[i].OptionsColumn.ReadOnly = true;
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    if (i < 1)
                        gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    else
                        gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    if (i > 1)
                    {
                        gridView1.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView1.Columns[i].DisplayFormat.FormatString = "#,#";
                    }
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                }
                gridView1.Columns["LINE_NM"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }
            catch { }
        }

        private void GetDataINV_Chart()
        {
            try
            {
                DatabaseTMS db = new DatabaseTMS();
                DataTable dtTemp = new DataTable();
                DataTable dtLabel = new DataTable();
                lblOS_INV.Text = "0 Prs";
                int InvQty = 0;
                DataTable dt = ds.Tables[2];
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                        InvQty += Convert.ToInt32(dt.Rows[i]["WIP_QTY"]);
                    lblOS_INV.Text = string.Format("{0:n0}", InvQty) + " Prs";
                }
                chartControl2.DataSource = dt;
                chartControl2.Series[0].ArgumentDataMember = "LINE_NM";
                chartControl2.Series[0].ValueDataMembers.AddRange(new string[] { "WIP_QTY" });
                ((DevExpress.XtraCharts.XYDiagram)chartControl2.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;

              

                gridControl2.DataSource = dt;
                for (int i = 0; i < gridView2.Columns.Count; i++)
                {
                    gridView2.Columns[i].OptionsColumn.ReadOnly = true;
                    gridView2.Columns[i].OptionsColumn.AllowEdit = false;
                    if (i < 2)
                        gridView2.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    else
                        gridView2.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    if (i > 0)
                    {
                        gridView2.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView2.Columns[i].DisplayFormat.FormatString = "#,#";
                    }
                    gridView2.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                }
                gridView2.Columns["LINE_NM"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;

                ((XYDiagram)chartControl2.Diagram).AxisX.VisualRange.SetMinMaxValues(dt.Rows[0]["LINE_NM"], dt.Rows[5]["LINE_NM"]);
            }
            catch (Exception ex) { chartControl2.DataSource = null; gridControl2.DataSource = null; }
        }

        private void GetChartShortageByComponent()
        {
            try
            {
                gridControl3.DataSource = ds.Tables[4];
                for (int i = 0; i < gridView3.Columns.Count; i++)
                {
                    gridView3.Columns[i].OptionsColumn.ReadOnly = true;
                    gridView3.Columns[i].OptionsColumn.AllowEdit = false;
                    if (i < 2)
                        gridView3.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    else
                        gridView3.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    if (i > 1)
                    {
                        gridView3.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView3.Columns[i].DisplayFormat.FormatString = "#,#";
                    }
                    gridView3.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                }
                gridView3.Columns["LINE_NM"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                lblPlan_Qty.Text = "0 Prs";
                lblSHR_Qty.Text = "0 Prs";
                if (ds.Tables[4].Rows.Count > 0)
                {
                    lblPlan_Qty.Text = string.Format("{0:n0}", ds.Tables[4].Rows[ds.Tables[4].Rows.Count - 1]["PLAN_QTY"]) + " Prs";
                    lblSHR_Qty.Text = string.Format("{0:n0}", ds.Tables[4].Rows[ds.Tables[4].Rows.Count - 1]["SHR_QTY"]) + " Prs";
                }
            }
            catch (Exception ex)
            { }
        }
        private void GetChartShortageByAsyDate()
        {
            try
            {
                chartControl5.DataSource = ds.Tables[5];

                chartControl5.SeriesDataMember = "DIV";
                chartControl5.SeriesTemplate.ArgumentDataMember = "LINE_NM";
                chartControl5.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "QTY" });
                chartControl5.SeriesTemplate.Label.TextPattern = "{V:#,#}";
                chartControl5.SeriesTemplate.CrosshairLabelPattern = "{V:#,#}";
                chartControl5.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl5.SeriesTemplate.Label.TextOrientation = TextOrientation.BottomToTop;

                chartControl5.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
                ((XYDiagram)chartControl5.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;
                ((XYDiagram)chartControl5.Diagram).AxisY.Title.Text = "Prs";
                ((DevExpress.XtraCharts.XYDiagram)chartControl5.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;

                ((XYDiagram)chartControl5.Diagram).AxisX.VisualRange.SetMinMaxValues(ds.Tables[5].Rows[0]["LINE_NM"], ds.Tables[5].Rows[5]["LINE_NM"]);

            }
            catch { }
        }


        private void GetChartOverall()
        {
            try
            {
                lblAVG_BTS.Text = "0%";
                chartControl3.DataSource = ds.Tables[3];
                chartControl3.Series[0].ArgumentDataMember = "LINE_NM";
                chartControl3.Series[0].ValueDataMembers.AddRange(new string[] { "RATIO" });
                ((DevExpress.XtraCharts.XYDiagram)chartControl3.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
               
                gridControl4.DataSource = ds.Tables[3];
                for (int i = 0; i < gridView4.Columns.Count; i++)
                {
                    gridView4.Columns[i].OptionsColumn.ReadOnly = true;
                    gridView4.Columns[i].OptionsColumn.AllowEdit = false;
                    if (i < 1)
                    {
                        gridView4.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gridView4.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                    }
                    else
                        gridView4.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    if (i > 0 && i < 3)
                    {

                        gridView4.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                        gridView4.Columns[i].DisplayFormat.FormatString = "#,#";
                        gridView4.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                    }
                    else
                        gridView4.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;


                }
                lblAVG_BTS.Text = string.Concat(Math.Round(Convert.ToDouble(ds.Tables[3].Compute("AVG(RATIO)", "")), 1), "%");

                ((XYDiagram)chartControl3.Diagram).AxisX.VisualRange.SetMinMaxValues(ds.Tables[3].Rows[0]["LINE_NM"], ds.Tables[3].Rows[5]["LINE_NM"]);
            }
            catch (Exception ex) { }
        }
        private void GetData()
        {
            DatabaseTMS db = new DatabaseTMS();
            ds = db.TMS_DELIVERY_DETAIL("MES.PKG_TMS_DASHBOARD.TMS_DELIVERY_DETAIL_V2", "VJ", ComVar.Var._strValue3, ComVar.Var._strValue4, DateTime.Now.ToString("yyyyMMdd"), string.IsNullOrEmpty(ComVar.Var._strValue1) ? "ALL" : ComVar.Var._strValue1);
            GetDataOutgoing_Chart(); GetDataOutgoing_Grid();
            GetDataINV_Chart();
            GetChartOverall();
            GetChartShortageByAsyDate(); GetChartShortageByComponent();
        }

        private void tmrDate_Tick(object sender, EventArgs e)
        {
            try
            {
                lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd\nHH:mm:ss")); //Gán dữ liệu giờ cho label ngày giờ
                iCount++;
                if (iCount >= 60)
                {
                    splashScreenManager1.ShowWaitForm();
                    splashScreenManager1.SetWaitFormCaption("Load " + ComVar.Var._strValue2);
                    GetData();
                    iCount = 0;
                    splashScreenManager1.CloseWaitForm();

                }
            }
            catch { iCount = 0; }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (ComVar.Var._bValue1)
                ComVar.Var.callForm = "342";
            else
                ComVar.Var.callForm = "223";
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }

        private void gridView3_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }
    }
}

using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
                DataTable dt = db.TMS_DELIVERY_STATUS_DETAIL_V6("MES.PKG_TMS_DASHBOARD.TMS_GET_DELI_STATUS_V6", Qtype, ComVar.Var._strValue3, ComVar.Var._strValue4, DateTime.Now.ToString("yyyyMMdd"), ComVar.Var._strValue1);
                switch (Qtype)
                {
                    case "INVENTORY":
                        lblInventory_Total.Text = "0";
                        int InvQty = 0;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                InvQty += Convert.ToInt32(dr["WIP_QTY"]);
                            }
                            lblInventory_Total.Text = string.Format("{0:n0}", InvQty);
                        }
                        chartINV.DataSource = dt;
                        chartINV.Series[0].ArgumentDataMember = "LINE_NM";
                        chartINV.Series[0].ValueDataMembers.AddRange(new string[] { "WIP_QTY" });
                        ((DevExpress.XtraCharts.XYDiagram)chartINV.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                        break;
                    case "OUTGOING":
                        lblOutgoing_Total.Text = "0";
                        if (dt.Rows.Count > 0)
                            lblOutgoing_Total.Text = string.Format("{0:n0}", Convert.ToInt32(dt.Compute("SUM(O_QTY)", "")));
                        chartOutgoing.DataSource = dt;
                        chartOutgoing.Series[0].ArgumentDataMember = "LINE_NM";
                        chartOutgoing.Series[0].ValueDataMembers.AddRange(new string[] { "O_QTY" });
                        ((DevExpress.XtraCharts.XYDiagram)chartOutgoing.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                        break;
                    case "SHORTAGE":

                        int shrQty = 0,planQty=0;
                        lblShortage_Total.Text = "0";
                        lblPlan_Total.Text = "0";
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                shrQty += Convert.ToInt32(dr["SHR_QTY"]);
                                planQty += Convert.ToInt32(dr["PLAN_QTY"]);
                            }
                            lblShortage_Total.Text = string.Format("{0:n0}", shrQty);
                            lblPlan_Total.Text = string.Format("{0:n0}", planQty);
                        }
                        chartShortage.DataSource = dt;
                        chartShortage.Series[0].ArgumentDataMember = "LINE_NM";
                        chartShortage.Series[0].ValueDataMembers.AddRange(new string[] { "PLAN_QTY" });
                        chartShortage.Series[1].ArgumentDataMember = "LINE_NM";
                        chartShortage.Series[1].ValueDataMembers.AddRange(new string[] { "SHR_QTY" });

                        ((XYDiagram)chartShortage.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;
                        ((XYDiagram)chartShortage.Diagram).AxisY.Title.Text = "Prs";
                        ((DevExpress.XtraCharts.XYDiagram)chartShortage.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
                        if (dt.Rows.Count >= 11)
                        {
                            ((XYDiagram)chartShortage.Diagram).AxisX.VisualRange.SetMinMaxValues(dt.Rows[0]["LINE_NM"], dt.Rows[10]["LINE_NM"]);
                        }
                        break;
                }
                splashScreenManager1.CloseWaitForm();
            }
            catch(Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show(ex.Message);
            }
        }

        private async void BindingData()
        {
         
            var Task1 = BindingData4Chart("INVENTORY");
            var Task2 = BindingData4Chart("OUTGOING");
            var Task3 = BindingData4Chart("SHORTAGE");

            await Task.WhenAll(Task1, Task2, Task3);
        }

        private void tmrDate_Tick(object sender, EventArgs e)
        {
            cCount++;
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
                BindingData();
            }
        }
    }
}

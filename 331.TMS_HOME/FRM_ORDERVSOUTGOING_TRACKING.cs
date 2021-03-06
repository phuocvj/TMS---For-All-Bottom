﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;

namespace FORM
{
    public partial class FRM_ORDERVSOUTGOING_TRACKING : Form
    {
        public FRM_ORDERVSOUTGOING_TRACKING()
        {
            InitializeComponent();
        }
        #region Variable
        string VDateF, VDateT, VLine, VTrip;
        int currhour = 0;
        private MyCellMergeHelper _Helper;
        #endregion
        private DataSet SELECT_DATA(string WorkType, string DateF, string DateT, string Line, string Trip)
        {
            System.Data.DataSet retDS;
            COM.OraDB MyOraDB = new COM.OraDB(); //1.LMES , 2.SEPHIROTH
            MyOraDB.ConnectName = COM.OraDB.ConnectDB.LMES;
            MyOraDB.ReDim_Parameter(14);
            MyOraDB.Process_Name = "P_GMES0262_Q_8";

            MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[3] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[4] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[5] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[6] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[7] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[8] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[9] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[10] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[11] = (char)OracleType.VarChar;
            MyOraDB.Parameter_Type[12] = (char)OracleType.Cursor;
            MyOraDB.Parameter_Type[13] = (char)OracleType.Cursor;

            MyOraDB.Parameter_Name[0] = "V_P_WORK_TYPE";
            MyOraDB.Parameter_Name[1] = "V_P_DATEF";
            MyOraDB.Parameter_Name[2] = "V_P_DATET";
            MyOraDB.Parameter_Name[3] = "V_P_LINE_CD";
            MyOraDB.Parameter_Name[4] = "V_P_TRIP";
            MyOraDB.Parameter_Name[5] = "V_P_ERROR_CODE";
            MyOraDB.Parameter_Name[6] = "V_P_ROW_COUNT";
            MyOraDB.Parameter_Name[7] = "V_P_ERROR_NOTE";
            MyOraDB.Parameter_Name[8] = "V_P_RETURN_STR";
            MyOraDB.Parameter_Name[9] = "V_P_ERROR_STR";
            MyOraDB.Parameter_Name[10] = "V_ERRORSTATE";
            MyOraDB.Parameter_Name[11] = "V_ERRORPROCEDURE";
            MyOraDB.Parameter_Name[12] = "OUT_CURSOR";
            MyOraDB.Parameter_Name[13] = "OUT_CURSOR1";

            MyOraDB.Parameter_Values[0] = WorkType;
            MyOraDB.Parameter_Values[1] = DateF;
            MyOraDB.Parameter_Values[2] = DateT;
            MyOraDB.Parameter_Values[3] = Line;
            MyOraDB.Parameter_Values[4] = Trip;
            MyOraDB.Parameter_Values[5] = "";
            MyOraDB.Parameter_Values[6] = "";
            MyOraDB.Parameter_Values[7] = "";
            MyOraDB.Parameter_Values[8] = "";
            MyOraDB.Parameter_Values[9] = "";
            MyOraDB.Parameter_Values[10] = "";
            MyOraDB.Parameter_Values[11] = "";
            MyOraDB.Parameter_Values[12] = "";
            MyOraDB.Parameter_Values[13] = "";

            MyOraDB.Add_Select_Parameter(true);
            retDS = MyOraDB.Exe_Select_Procedure();
            if (retDS == null) return null;
            return retDS;
        }

        public DataTable GET_LINE(string ARG_QTYPE)
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.PKG_TMS_HOME.TMS_HOME_GET_LINE";
                MyOraDB.ReDim_Parameter(2);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "ARG_QTYPE";
                MyOraDB.Parameter_Name[1] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = ARG_QTYPE;
                MyOraDB.Parameter_Values[1] = "";

                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();
                if (ds_ret == null) return null;
                return ds_ret.Tables[0];
            }
            catch
            {
                return null;
            }
        }
        private DataTable GetDataCboPlant()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PLANT_NM", typeof(string));
            dt.Columns.Add("PLANT_CD", typeof(string));

            DataRow dr = dt.NewRow();
            dr["PLANT_NM"] = "LD";
            dr["PLANT_CD"] = "201";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["PLANT_NM"] = "LE";
            dr["PLANT_CD"] = "202";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["PLANT_NM"] = "Plant L";
            dr["PLANT_CD"] = "018";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["PLANT_NM"] = "Plant H";
            dr["PLANT_CD"] = "014";
            dt.Rows.Add(dr);
            return dt;
        }
        private void MarkTripButton()
        {
            foreach (DevExpress.XtraEditors.SimpleButton btn in pnTrip.Controls)
            {
                if (btn.Tag.ToString().Equals(VTrip))
                    btn.Appearance.ForeColor = Color.Blue;
            }
        }
        //private void Trip_set()
        //{
        //    foreach (DevExpress.XtraEditors.SimpleButton btn in pnTrip.Controls)
        //    {
        //        btn.Appearance.ForeColor = Color.Black;
        //    }
        //    currhour = Int32.Parse(DateTime.Now.ToString("HHmm"));
        //    if ((currhour >= 545) && (currhour < 745))
        //    {
        //        VTrip = "001";
        //        lblTripTime.Text = "05:45";

        //    }
        //    else if ((currhour >= 730) && (currhour < 945))
        //    {
        //        VTrip = "002";
        //        lblTripTime.Text = "07:30";
        //    }
        //    else if ((currhour >= 945) && (currhour < 1145))
        //    {
        //        VTrip = "003";
        //        lblTripTime.Text = "09:45";

        //    }
        //    else if (currhour >= 1145)
        //    {
        //        VTrip = "004";
        //        lblTripTime.Text = "11:45";
        //    }
        //    MarkTripButton();
        //}
        private void Trip_set()
        {
            foreach (DevExpress.XtraEditors.SimpleButton btn in pnTrip.Controls)
            {
                btn.Appearance.ForeColor = Color.Black;
            }
            currhour = Int32.Parse(DateTime.Now.ToString("HHmm"));
            //-- load trip capa--

            if ((VLine == "201") || (VLine == "202"))
            {

                if ((currhour >= 545) && (currhour < 745))
                {
                    VTrip = "001";
                    lblTripTime.Text = "05:45";

                }
                else if ((currhour >= 730) && (currhour < 945))
                {
                    VTrip = "002";
                    lblTripTime.Text = "07:30";
                }
                else if ((currhour >= 945) && (currhour < 1145))
                {
                    VTrip = "003";
                    lblTripTime.Text = "09:45";
                }
                else if (currhour >= 1145)
                {
                    VTrip = "004";
                    lblTripTime.Text = "11:45";
                }

            }
            else  //vj1
            {
                if ((currhour >= 600) && (currhour < 900))
                {
                   VTrip = "001";
                    lblTripTime.Text = "06:00";

                }
                else if ((currhour >= 900) && (currhour <= 1030))
                {
                    VTrip = "002";
                    lblTripTime.Text = "09:00";
                }
                else if ((currhour >= 1100) && (currhour <= 1245))
                {
                   VTrip= "003";
                    lblTripTime.Text = "11:00";
                }
                else if ((currhour >= 1315) && (currhour <= 1445))
                {
                    VTrip= "004";
                    lblTripTime.Text = "13:15";
                }
                else
                {
                    VTrip = "000";
                    lblTripTime.Text = "";
                }
            }
            MarkTripButton();
        }
        private void LoadControl()
        {
            VDateF = DateTime.Now.AddDays(-6).ToString("yyyyMMdd");
            VDateT = DateTime.Now.ToString("yyyyMMdd");
            VLine = "201";
            VTrip = "001";
        }

        private bool loadchart(DataTable dtchart)
        {
            try
            {
                chart.DataSource = dtchart;
                chart.Series[0].ArgumentDataMember = "YMD";
                chart.Series[0].ValueDataMembers.AddRange(new string[] { "PER1" });
                chart.Series[1].ArgumentDataMember = "YMD";
                chart.Series[1].ValueDataMembers.AddRange(new string[] { "PER2" });
                chart.Series[2].ArgumentDataMember = "YMD";
                chart.Series[2].ValueDataMembers.AddRange(new string[] { "PER4" });
                chart.Series[3].ArgumentDataMember = "YMD";
                chart.Series[3].ValueDataMembers.AddRange(new string[] { "PER3" });
                ((XYDiagram)chart.Diagram).AxisX.Label.Staggered = false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private void loadgrid(DataTable dtgrid)
        {
            grdBase.DataSource = dtgrid;
            Format_Grid();
        }
        //private void Format_Grid()
        //{
        //    try
        //    {
        //        gvwBase.BeginUpdate();
        //        #region replace
        //        for (int i = 0; i <= gvwBase.RowCount - 1; i++)
        //        {
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "SET_ORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Order By Set (Prs)");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "SET_ORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "Order By Set (Prs)");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "TOTAL_OUTGOING_PRS")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Total Outgoing (Prs)");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "TOTAL_OUTGOING_PRS")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "Total Outgoing (Prs)");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "PER_TOTAL")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "%");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "PER_TOTAL")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "%");
        //            }

        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "IN_ORDER_BY_SET")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Outgoing In Order");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "OUTGOING_PRS1")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "By Set (Prs)");

        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "OUTGOING_PRS1.2")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "UnSet/2 (Prs)");
        //            }

        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "PER_INORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Outgoing In Order");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "PER_INORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "%");
        //            }

        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "NOT_IN_ORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Outgoing Not In Order");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "OUTGOING_PRS2")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "Sum/2 (Prs)");
        //            }

        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "PER_NOTORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Outgoing Not In Order");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString() == "PER_NOTORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], "%");
        //            }

        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "OUT_IN_ORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Order In Detail");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP1"]).ToString() == "OUT_NOT_ORDER")
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP1"], "Not Order In Detail");
        //            }
        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString().Contains("Y"))
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString().Replace("-Y", ""));
        //            }

        //            if (gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString().Contains("N"))
        //            {
        //                gvwBase.SetRowCellValue(i, gvwBase.Columns["CMP2"], gvwBase.GetRowCellValue(i, gvwBase.Columns["CMP2"]).ToString().Replace("-N", ""));
        //            }
        //        }

        //        #endregion replace
        //        _Helper = new MyCellMergeHelper(gvwBase);
        //        _Helper.AddMergedCell(0, 0, 1, "Order By Set (Prs)");
        //        _Helper.AddMergedCell(1, 0, 1, "Total Outgoing (Prs)");
        //        _Helper.AddMergedCell(2, 0, 1, "%");


        //        for (int i = 0; i < gvwBase.Columns.Count; i++)
        //        {
        //            gvwBase.Columns[i].AppearanceCell.Options.UseTextOptions = true;
        //            gvwBase.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        //            gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
        //            gvwBase.Columns[i].OptionsFilter.AllowFilter = false;
        //            gvwBase.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

        //            gvwBase.ColumnPanelRowHeight = 25;
        //            gvwBase.RowHeight = 25;
        //            gvwBase.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

        //            if (i <= 1)
        //            {
        //                gvwBase.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        //                gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
        //            }
        //            else
        //            {
        //                gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
        //                gvwBase.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //                gvwBase.Columns[i].DisplayFormat.FormatString = "#,###.##";
        //            }
        //            gvwBase.Columns[i].Caption = gvwBase.Columns[i].FieldName.ToString().Replace("'", "");
        //        }
        //        gvwBase.Columns[0].Caption = gvwBase.Columns[0].FieldName.ToString().Replace("CMP1", "Item");
        //        gvwBase.Columns[1].Caption = gvwBase.Columns[1].FieldName.ToString().Replace("CMP2", "Item");

        //        gvwBase.Appearance.Row.Font = new System.Drawing.Font("DotumChe", 10F, System.Drawing.FontStyle.Regular);
        //        gvwBase.BestFitColumns();
        //        gvwBase.EndUpdate();
        //    }
        //    catch { }
        //}
        private void Format_Grid()
        {
            gvwBase.BeginUpdate();
            #region replace


            for (int i = 0; i <= gvwBase.RowCount - 1; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (gvwBase.GetRowCellValue(i, gvwBase.Columns[j]).ToString() == "TOTAL_ORDER")
                    {
                        gvwBase.SetRowCellValue(i, gvwBase.Columns[j], "Total Order");
                    }
                    if (gvwBase.GetRowCellValue(i, gvwBase.Columns[j]).ToString() == "OUT_IN_ORDER")
                    {
                        gvwBase.SetRowCellValue(i, gvwBase.Columns[j], "Outgoing In Order");
                    }
                    if (gvwBase.GetRowCellValue(i, gvwBase.Columns[j]).ToString() == "OUT_IN_ORDER_IN_TIME")
                    {
                        gvwBase.SetRowCellValue(i, gvwBase.Columns[j], "On Time");
                    }
                    if (gvwBase.GetRowCellValue(i, gvwBase.Columns[j]).ToString() == "OUT_IN_ORDER_NOT_IN_TIME")
                    {
                        gvwBase.SetRowCellValue(i, gvwBase.Columns[j], "Not On Time");
                    }
                    if (gvwBase.GetRowCellValue(i, gvwBase.Columns[j]).ToString() == "OUT_WITHOUT_ORDER")
                    {
                        gvwBase.SetRowCellValue(i, gvwBase.Columns[j], "Outgoing Without Order");
                    }
                    if (gvwBase.GetRowCellValue(i, gvwBase.Columns[j]).ToString() == "TOTAL_OUT_IN_TRIP")
                    {
                        gvwBase.SetRowCellValue(i, gvwBase.Columns[j], "Total Outgoing");
                    }
                }


            }





            //}

            #endregion replace

            _Helper = new MyCellMergeHelper(gvwBase);
            _Helper.AddMergedCell(0, 0, 1, "Total Order");

            //_Helper.AddMergedCell(4, 0, 1, "Outgoing Without Order");
            //_Helper.AddMergedCell(5, 0, 1, "Outgoing Without Order");
            //_Helper.AddMergedCell(6, 0, 1, "Total Outgoing");
            //_Helper.AddMergedCell(7, 0, 1, "Total Outgoing");








            for (int i = 0; i < gvwBase.Columns.Count; i++)
            {
                gvwBase.Columns[i].AppearanceCell.Options.UseTextOptions = true;
                gvwBase.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                gvwBase.Columns[i].OptionsFilter.AllowFilter = false;
                gvwBase.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                gvwBase.Columns[i].OptionsColumn.AllowEdit = false;
                gvwBase.Columns[i].OptionsColumn.ReadOnly = true;
                gvwBase.ColumnPanelRowHeight = 25;
                gvwBase.RowHeight = 30;
                gvwBase.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                if (i <= 2)
                {
                    gvwBase.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                    if (i < 2)
                    {
                        gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    }
                    else
                        gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
                else
                {
                    gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    gvwBase.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gvwBase.Columns[i].DisplayFormat.FormatString = "#,###.##";
                }



                gvwBase.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvwBase.Columns[i].Caption = gvwBase.Columns[i].FieldName.ToString().Replace("'", "");





            }
            gvwBase.Columns[0].Caption = gvwBase.Columns[0].FieldName.ToString().Replace("CMP1", "Item");
            gvwBase.Columns[1].Caption = gvwBase.Columns[1].FieldName.ToString().Replace("CMP2", "Item");
            gvwBase.Columns[2].Caption = gvwBase.Columns[2].FieldName.ToString().Replace("CMP3", "Unit");

            gvwBase.Appearance.Row.Font = new System.Drawing.Font("DotumChe", 10F, System.Drawing.FontStyle.Regular);
            gvwBase.BestFitColumns();

            // gvwBase.OptionsView.ColumnAutoWidth = false;
            gvwBase.EndUpdate();
        }
        private void gvwBase_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            double temp = 0.0;


            if ((gvwBase.GetRowCellDisplayText(e.RowHandle, gvwBase.Columns["CMP3"]).ToString() == "Prs") && (gvwBase.GetRowCellDisplayText(e.RowHandle, gvwBase.Columns["CMP2"]).ToString() == "Total Order"))
            {
                e.Appearance.BackColor = Color.LightYellow;

            }

            if ((gvwBase.GetRowCellDisplayText(e.RowHandle, gvwBase.Columns["CMP3"]).ToString() == "%") && (gvwBase.GetRowCellDisplayText(e.RowHandle, gvwBase.Columns["CMP2"]).ToString() == "%"))
            {
                if (e.Column.AbsoluteIndex >= 2 && e.CellValue != null)
                {
                    double.TryParse(e.CellValue.ToString(), out temp); //out

                    if (temp > 0 && temp < 30)
                    {
                        e.Appearance.BackColor = Color.Black;
                        e.Appearance.ForeColor = Color.White;
                    }

                    else if (temp >= 30 && temp < 70)
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                    }
                    else if (temp >= 70 && temp < 90)
                    {

                        e.Appearance.BackColor = Color.Yellow;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (temp >= 90)
                    {

                        e.Appearance.BackColor = Color.LightGreen;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }

            }
        }
        private void FRM_ORDERVSOUTGOING_TRACKING_Load(object sender, EventArgs e)
        {

            splashScreenManager1.ShowWaitForm();
            try
            {
                LoadControl();
                btnLD.BackColor = Color.Yellow;
                DataTable dtLine = GET_LINE("Q");
                cboPlant.DataSource = dtLine; //GetDataCboPlant();
                cboPlant.DisplayMember = "LINE_NAME";
                cboPlant.ValueMember = "LINE_CD";
                if (dtLine == null) return;
                VLine = dtLine.Rows[0][0].ToString();
                Trip_set();
                DataSet ds = SELECT_DATA("Q", VDateF, VDateT, VLine, VTrip);
                DataTable dtGrid = new DataTable();
                DataTable dtChart = new DataTable();
                dtGrid = ds.Tables[0];
                loadgrid(dtGrid);
                dtChart = ds.Tables[1];
                loadchart(dtChart);
                splashScreenManager1.CloseWaitForm();
            }
            catch { splashScreenManager1.CloseWaitForm(); }

        }

        private void btnTrip_Click(object sender, EventArgs e)
        {
            VTrip = ((DevExpress.XtraEditors.SimpleButton)sender).Tag.ToString();
            if ((cboPlant.SelectedValue.ToString() == "201") || (cboPlant.SelectedValue.ToString() == "202"))
            {
                switch (VTrip)
                {
                    case "001":
                        lblTripTime.Text = "05:45";
                        break;
                    case "002":
                        lblTripTime.Text = "07:30";
                        break;
                    case "003":
                        lblTripTime.Text = "09:45";
                        break;
                    case "004":
                        lblTripTime.Text = "11:45";
                        break;
                }
            }
            else //vj1
            {
                switch (VTrip)
                {
                    case "001":
                        lblTripTime.Text = "06:00";
                        break;
                    case "002":
                        lblTripTime.Text = "09:00";
                        break;
                    case "003":
                        lblTripTime.Text = "11:00";
                        break;
                    case "004":
                        lblTripTime.Text = "13:15";
                        break;
                }
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                splashScreenManager1.ShowWaitForm();
                DataSet ds = SELECT_DATA("Q", VDateF, VDateT, VLine, VTrip);
                DataTable dtGrid = new DataTable();
                DataTable dtChart = new DataTable();
                dtGrid = ds.Tables[0];
                loadgrid(dtGrid);
                dtChart = ds.Tables[1];
                loadchart(dtChart);
                splashScreenManager1.CloseWaitForm();
                this.Cursor = Cursors.Default;
            }
            catch { splashScreenManager1.CloseWaitForm(); this.Cursor = Cursors.Default; }
            try
            {
                foreach (DevExpress.XtraEditors.SimpleButton btn in pnTrip.Controls)
                {
                    btn.Appearance.ForeColor = Color.Black;
                }
                ((DevExpress.XtraEditors.SimpleButton)sender).Appearance.ForeColor = Color.Blue;
            }
            catch { }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                btnLD.BackColor = Color.Silver;
                btnLE.BackColor = Color.Silver;
                this.Cursor = Cursors.WaitCursor;
                splashScreenManager1.ShowWaitForm();
                ((Button)sender).BackColor = Color.Yellow;
                VLine = ((Button)sender).Tag.ToString();
                DataSet ds = SELECT_DATA("Q", VDateF, VDateT, VLine, VTrip);
                DataTable dtGrid = new DataTable();
                DataTable dtChart = new DataTable();
                dtGrid = ds.Tables[0];
                loadgrid(dtGrid);
                dtChart = ds.Tables[1];
                loadchart(dtChart);
                splashScreenManager1.CloseWaitForm();
                this.Cursor = Cursors.Default;
            }
            catch
            {
                splashScreenManager1.CloseWaitForm();
                this.Cursor = Cursors.Default;
            }
        }
        private void Trip_capa_time()
        {
            if ((cboPlant.SelectedValue.ToString() == "201") || (cboPlant.SelectedValue.ToString() == "202"))
            {
                if (VTrip == "001")
                {
                    lblTripTime.Text = "05:45";
                    //   LblScantime.Text = "06:00 ~ 07:44";

                }
                else if (VTrip == "002")
                {
                    lblTripTime.Text = "07:30";
                    //  LblScantime.Text = "07:45 ~ 09:00";
                }

                else if (VTrip == "003")
                {
                    lblTripTime.Text = "09:45";
                    // LblScantime.Text = "09:45 ~ 11:59";
                }
                else if (VTrip == "004")
                {
                    lblTripTime.Text = "11:45";
                    // LblScantime.Text = "12:00 ~ 13:30";
                }


            }
            else //vj1
            {
                if (VTrip == "001")
                {
                    lblTripTime.Text = "06:00";
                }
                else if (VTrip == "002")
                {
                    lblTripTime.Text = "09:00";
                    //  LblScantime.Text = "09:00 ~ 10:30";

                }
                else if (VTrip == "003")
                {
                    lblTripTime.Text = "11:00";
                    //LblScantime.Text = "11:00 ~ 12:45";
                }
                else if (VTrip == "004")
                {
                    lblTripTime.Text = "13:15";
                    // LblScantime.Text = "13:15 ~ 14:45";
                }

            }
        }

        private void cboPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!cboPlant.SelectedValue.ToString().Equals("System.Data.DataRowView"))
                {

                    this.Cursor = Cursors.WaitCursor;
                    splashScreenManager1.ShowWaitForm();
                    Trip_capa_time();
                    VLine = cboPlant.SelectedValue.ToString();
                    DataSet ds = SELECT_DATA("Q", VDateF, VDateT, VLine, VTrip);
                    DataTable dtGrid = new DataTable();
                    DataTable dtChart = new DataTable();
                    dtGrid = ds.Tables[0];
                    loadgrid(dtGrid);
                    dtChart = ds.Tables[1];
                    loadchart(dtChart);
                    splashScreenManager1.CloseWaitForm();
                    this.Cursor = Cursors.Default;
                }
            }
            catch
            {
                splashScreenManager1.CloseWaitForm();
                this.Cursor = Cursors.Default;
            }
        }


    }
}

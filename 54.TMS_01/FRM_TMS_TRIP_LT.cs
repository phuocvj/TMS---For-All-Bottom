using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace FORM
{
    public partial class FRM_TMS_TRIP_LT : Form
    {
        int angle = 0;
        int rotSpeed = 1;
        Point origin = new Point(550, 65);  // my origin
        // Point origin = new Point(847, 622);  // my origin
        Point Lighting = new Point(556, 1011);
        int distance = 20;
        int _iStartText = 0;
        int count = 0;
      
        string MLINE = ComVar.Var._strValue2;
        public FRM_TMS_TRIP_LT()
        {
            InitializeComponent();
        }

        private void FRM_TMS_CAR_LT_NEW_Load(object sender, EventArgs e)
        {
            //   lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //    cbdate.DateTime = DateTime.Now;
        //    GoFullscreen();
            //   initForm();
            //  _Loc_X = pic_Car.Location.X;
            load_Data();
           // runTextModel();
        }
        private void GoFullscreen()
        {
            //this.WindowState = FormWindowState.Normal;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.Bounds = Screen.PrimaryScreen.Bounds;

        }
        //#region DB


        private void load_Data()
        {
            try
            {
               
                splashScreenManager1.ShowWaitForm();
                Cursor = Cursors.WaitCursor;
                DataTable data = null;
                data = Select_Ora_Grid().Tables[0];
                CreateSizeGrid(grdBase, gvwBase1, data);
                grdBase.DataSource = data;
                gvwBase1.Columns[0].OwnerBand.Width = 80;
                gvwBase1.Columns[1].OwnerBand.Width = 250;
                gvwBase1.Columns[2].OwnerBand.Width = 150;
                gvwBase1.Columns[3].OwnerBand.Width = 80;
             
            //    gvwBase1.Columns[1].OwnerBand.Width = 100;


                //for (int row = 0; row < gvwBase1.RowCount ; row++)
                //{
                    for (int i = 0; i < gvwBase1.Columns.Count; i++)
                    {
                        gvwBase1.Columns[i].OptionsColumn.AllowEdit = false;
                        gvwBase1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwBase1.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gvwBase1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwBase1.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gvwBase1.Columns[i].OwnerBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwBase1.Columns[i].OwnerBand.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gvwBase1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;



                        gvwBase1.Columns["LINE"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gvwBase1.Columns["MODEL"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gvwBase1.Columns["STYLE"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        //gvwBase1.Columns["LINE"].Fixed=DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        //gvwBase1.Columns["MODEL"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                 
                        //gvwBase1.Columns["STYLE"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        //gvwBase1.Columns["QTY"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        
                        if (i > 3)
                        {
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.BackColor = Color.FromArgb(57, 190, 29);
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.BackColor2 = Color.FromArgb(57, 190, 29);
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.ForeColor = Color.White;


                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.BackColor = Color.FromArgb(255, 127, 0);
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.BackColor2 = Color.FromArgb(255, 127, 0);
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.ForeColor = Color.White;


                            //  gvwBase1.Columns[i].OwnerBand.


                            gvwBase1.Columns[i].Width = (grdBase.Width - gvwBase1.Columns[0].OwnerBand.Width - gvwBase1.Columns[1].OwnerBand.Width) / (gvwBase1.Columns.Count - 2);
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                            gvwBase1.Columns[i].OwnerBand.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                        }
                }

                    Cursor = Cursors.Default;
                    splashScreenManager1.CloseWaitForm();


              //  }
                    
              //  gvwBase1.OptionsView.AllowCellMerge = true;
            }
            catch
            { splashScreenManager1.CloseWaitForm(); }
        }

        public void CreateSizeGrid(DevExpress.XtraGrid.GridControl gridControl, BandedGridView gridView, DataTable dt)
        {
            //gridControl.Hide();
            gridView.BeginDataUpdate();
            try
            {
                bool flag = false;
                gridView.OptionsView.ShowGroupPanel = false;
                gridView.OptionsView.AllowCellMerge = true;
                gridView.Columns.Clear();
                gridView.Bands.Clear();
                gridView.OptionsView.ShowColumnHeaders = false;
                gridView.OptionsView.ColumnAutoWidth = false;
                DevExpress.XtraGrid.Views.BandedGrid.GridBand[] band_parent = new DevExpress.XtraGrid.Views.BandedGrid.GridBand[dt.Columns.Count];
                DevExpress.XtraGrid.Views.BandedGrid.GridBand[] band_child = new DevExpress.XtraGrid.Views.BandedGrid.GridBand[dt.Columns.Count - 2];
                int i_arr = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (i<=1)
                    //{
                    band_parent[i] = new GridBand() { Caption = dt.Columns[i].ColumnName.ToString() };
                    gridView.Bands.Add(band_parent[i]);
                    band_parent[i].Columns.Add(new BandedGridColumn() { FieldName = dt.Columns[i].ColumnName.ToString(), Visible = true, Caption = band_parent[i].Caption });
                    //}
                   // else
                    ////{
                    //    if (!flag)
                    //    {
                    //        band_parent[i] = new GridBand() { Caption = "COMPONENT INCOMING" };
                    //        gridView.Bands.Add(band_parent[i]);
                    //        band_parent[i].Children.Add(new GridBand() { Caption = dt.Columns[i].ColumnName.ToString() });
                    //        band_parent[i].Children[i_arr].Columns.Add(new BandedGridColumn() { FieldName = dt.Columns[i].ColumnName.ToString(), Visible = true, Caption = dt.Columns[i].ColumnName.ToString() });
                    //        i_arr++;
                    //        band_parent[i].Children[0].RowCount = 2;
                    //        flag = true;
                    //    }
                    //    else
                    //    {
                    //        band_parent[band_parent.Count() - 1].Children.Add(new GridBand() { Caption = dt.Columns[i].ColumnName.ToString() });
                    //        band_parent[band_parent.Count() - 1].Children[i_arr].Columns.Add(new BandedGridColumn() { FieldName = dt.Columns[i].ColumnName.ToString(), Visible = true, Caption = dt.Columns[i].ColumnName.ToString() });
                    //        i_arr++;
                    //    }
                    ////}

                }
                band_parent[0].RowCount = 2;
                gridView.OptionsView.ColumnAutoWidth = false;
                
            }
            catch (Exception EX)
            {
                //throw EX;
            }
            gridView.EndDataUpdate();
            gridView.ExpandAllGroups();
        }
        


        private void gvwBase1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
           

            if (e.RowHandle % 2 == 0)
            {
               // e.Appearance.BackColor = Color.FromArgb(185, 217, 249);
                e.Appearance.BackColor = Color.FromArgb(219, 251, 255);
            }
           
                if (gvwBase1.GetRowCellValue(e.RowHandle, "LINE").ToString().ToUpper() == "TOTAL")
                {
                    e.Appearance.BackColor = Color.LightSkyBlue;
                    e.Appearance.ForeColor = Color.Black;
                }
                if (e.RowHandle == gvwBase1.RowCount - 1)
                {
                    e.Appearance.BackColor = Color.Lime;
                    e.Appearance.ForeColor = Color.Black;
                }
            
          
        }

        private void bgw_ER_Check_DoWork(object sender, DoWorkEventArgs e)
        {
            //if (pic_Car.InvokeRequired == true)
            //    pic_Car.Invoke((MethodInvoker)delegate
            //    {
            //        try
            //        {
            //            angle += rotSpeed;
            //         //   int x = (int)(origin.X + distance * Math.Sin(angle * Math.PI / 100));
            //          //  int y = (int)(origin.Y + distance * Math.Cos(angle * Math.PI / 300f));
            //            int x = (int)(origin.X + distance + angle/200 );
            //            int y = (int)(origin.Y  );
            //            pic_Car.Location = new Point(x, y);
            //            if (angle == 6000)
            //            {
            //                angle = 0;
            //            }

            //        }
            //        catch { }
            //    });
        }

        private void cbdate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                  load_Data();
            }
            catch
            {

            }
        }

        private void tmr_rotate_er_check_Tick(object sender, EventArgs e)
        {
            //if (!bgw_ER_Check.IsBusy)
            //{
            //    bgw_ER_Check.RunWorkerAsync();
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //   runTextModel();
        }

        private void runTextModel()
        {
            //string blank = "          ";
            //addTextGauge("Long Thanh going to Vinh Cuu" + blank, dtgestimate);
            //_iStartText++;
        }

        private void addTextGauge(string arg_str, DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge gauge)
        {

            //if (arg_str.Length <= 20)
            //{
            //    arg_str = arg_str.PadRight(20, ' ');
            //}

            //if (_iStartText + 1 > arg_str.Length)
            //{
            //    _iStartText = 0;
            //}

            //gauge.Text += arg_str.Substring(_iStartText, 1);
        }

        private void splMain_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public string v_Trip;
        public string v_CMP_CD_IN;
        public string v_date;
        public string LINE;
        public string v_p_location;


        private System.Data.DataSet Select_Ora_Grid()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_TRIP_LT";
                //ARGMODE
                MyOraDB.ReDim_Parameter(5);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_LINE";
                MyOraDB.Parameter_Name[1] = "V_P_YMD";
                MyOraDB.Parameter_Name[2] = "V_P_LOCATION";
                MyOraDB.Parameter_Name[3] = "V_P_TRIP";
               
            //    MyOraDB.Parameter_Name[4] = "V_P_CMP_CD";
                MyOraDB.Parameter_Name[4] = "CV_1";
                // MyOraDB.Parameter_Name[3] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[3] = (char)OracleType.VarChar;
         
              //  MyOraDB.Parameter_Type[4] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[4] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = LINE;
                MyOraDB.Parameter_Values[1] = v_date;
                MyOraDB.Parameter_Values[2] = v_p_location;
                MyOraDB.Parameter_Values[3] = v_Trip;
                
              //  MyOraDB.Parameter_Values[4] = v_CMP_CD_IN;
                MyOraDB.Parameter_Values[4] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();
                  if (ds_ret == null)
                   return null;
                 return ds_ret;
            }
            catch
            {
                  return null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FRM_TMS_CAR_LT_POP_VisibleChanged(object sender, EventArgs e)
        {
           // if (this.Visible)
           // {
           //     LINE = ComVar.Var._strValue1;
           //     MLINE = ComVar.Var._strValue2;
           //     count = 60;
           //     timer1.Start();
           // }
           //else
           //     timer1.Stop();
        }

       

      







    }

}

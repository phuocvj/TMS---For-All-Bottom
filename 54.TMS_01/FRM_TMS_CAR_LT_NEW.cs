using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;


namespace FORM
{
    public partial class FRM_TMS_CAR_LT_NEW : Form
    {
        int angle = 0;
        int rotSpeed = 1;
        string MLINE = "";
        string LINE = "";
        Point origin = new Point(550, 65);  // my origin
        Point origin_lb = new Point(560, 91);  // my origin
        // Point origin = new Point(847, 622);  // my origin
        Point Lighting = new Point(556, 1011);
        int distance = 20;
        int _iStartText = 0;
        int count = 0;
        int count_car = 0;
        int time_depart = 0;
        string v_loc = "";
        string v_thread = "";
        string v_p_location = "";
        string V_currentime = "";
        string v_trip_time = "";
        int isPriority = 0;
        public FRM_TMS_CAR_LT_NEW()
        {
            InitializeComponent();
        }

        private void FRM_TMS_CAR_LT_NEW_Load(object sender, EventArgs e)
        {
            DataTable _dtXML = null;
            isBack = ComVar.Var._IsBack;
            cmdBack.Visible = isBack;
            if (!ComVar.Var._IsBack)
            {
                _dtXML = ComVar.Func.ReadXML(Application.StartupPath + "\\Config.XML", "MAIN");

                FullScreen(Convert.ToInt16(_dtXML.Rows[0]["Monitor"]));


                ComVar.Var._strValue1 = v_loc = _dtXML.Rows[0]["Loc"].ToString();
                v_thread = _dtXML.Rows[0]["Thread"].ToString();


                if (v_loc == "FTY01")
                {
                    v_p_location = "FTY01";
                    LINE = "";
                    MLINE = "";
                }
                else if (v_loc == "VJ1")
                {
                    v_p_location = "VJ1";
                    LINE = "";
                    MLINE = "";
                }
                else
                {
                    v_p_location = "099";
                    LINE = "099";
                    MLINE = "001";
                }
            }


            lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //cbdate.DateTime = DateTime.Now;
            //GoFullscreen();
            //   initForm();
            //  _Loc_X = pic_Car.Location.X;

            runTextModel();
            V_currentime = DateTime.Now.ToString("HHmmss");


            Label[] lbl = {Lb_TL_trip1,Lb_TL_trip2,Lb_TL_trip3,Lb_TL_trip4,Lb_TL_trip5,
                           Lb_VC_trip1,Lb_VC_trip2,Lb_VC_trip3,Lb_VC_trip4,Lb_VC_trip5};

            for (int i = 0; i < lbl.Length; i++)
            {
                lbl[i].BackColor = Color.FromArgb(128, 128, 255);
                lbl[i].ForeColor = Color.White;
            }

            if (Convert.ToDouble(V_currentime) > 081500 && Convert.ToDouble(V_currentime) <= 094459)
            {

                Lb_TL_trip1.BackColor = Color.Yellow;
                Lb_TL_trip1.ForeColor = Color.White;
                DtwTrip.Text = "01";
                //dtwStart.Text = "08:18";
            }
            if (Convert.ToDouble(V_currentime) > 094500 && Convert.ToDouble(V_currentime) <= 121459)
            {
                Lb_TL_trip2.BackColor = Color.Yellow;
                Lb_TL_trip2.ForeColor = Color.White;
                DtwTrip.Text = "02";
                // dtwStart.Text = "09:50";

            }
            if (Convert.ToDouble(V_currentime) > 121500 && Convert.ToDouble(V_currentime) <= 135959)
            {
                Lb_TL_trip3.BackColor = Color.Yellow;
                Lb_TL_trip3.ForeColor = Color.White;
                DtwTrip.Text = "03";
                // dtwStart.Text = "12:16";

            }
            if (Convert.ToDouble(V_currentime) > 140000 && Convert.ToDouble(V_currentime) <= 154459)
            {
                Lb_TL_trip4.BackColor = Color.Yellow;
                Lb_TL_trip4.ForeColor = Color.White;
                DtwTrip.Text = "04";
                // dtwStart.Text = "14:05";

            }
            if (Convert.ToDouble(V_currentime) > 154500)
            {
                Lb_TL_trip5.BackColor = Color.Yellow;
                Lb_TL_trip5.ForeColor = Color.White;
                DtwTrip.Text = "05";
                // dtwStart.Text = "15:50";
                gvwBase.TopRowIndex = gvwBase.RowCount - 15;
            }

            // VC

            if (Convert.ToDouble(V_currentime) > 081500 && Convert.ToDouble(V_currentime) <= 094459)
            {
                Lb_VC_trip1.BackColor = Color.Yellow;
                Lb_VC_trip1.ForeColor = Color.White;
                // dtwArrival.Text = "09:20";
            }
            if (Convert.ToDouble(V_currentime) > 094500 && Convert.ToDouble(V_currentime) <= 121459)
            {
                Lb_VC_trip2.BackColor = Color.Yellow;
                Lb_VC_trip2.ForeColor = Color.White;
                //  dtwArrival.Text = "11:10";
            }
            if (Convert.ToDouble(V_currentime) > 121500 && Convert.ToDouble(V_currentime) <= 135959)
            {
                Lb_VC_trip3.BackColor = Color.Yellow;
                Lb_VC_trip3.ForeColor = Color.White;
                //  dtwArrival.Text = "13:18";
            }
            if (Convert.ToDouble(V_currentime) > 140000 && Convert.ToDouble(V_currentime) <= 154459)
            {
                Lb_VC_trip4.BackColor = Color.Yellow;
                Lb_VC_trip4.ForeColor = Color.White;
                //   dtwArrival.Text = "15:10";
            }
            if (Convert.ToDouble(V_currentime) > 154500 && Convert.ToDouble(V_currentime) < 200000)
            {
                Lb_VC_trip5.BackColor = Color.Yellow;
                Lb_VC_trip5.ForeColor = Color.White;
                //   dtwArrival.Text = "17:50";
            }



        }
        private void FullScreen(int ArgMonitor)
        {
            this.WindowState = FormWindowState.Normal;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;\
            Screen[] S = Screen.AllScreens;
            if (S.Length > 1)
            {
                this.Bounds = S[ArgMonitor - 1].Bounds;
                this.Height = S[ArgMonitor - 1].WorkingArea.Height + 70;
                this.Width = S[ArgMonitor - 1].WorkingArea.Width + 17;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = S[0].Bounds;
                this.Width = S[0].WorkingArea.Width;
            }
        }
        private void GoFullscreen()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;

        }
        #region DB
        private System.Data.DataSet Select_Ora_Grid()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_LT";
                //ARGMODE
                MyOraDB.ReDim_Parameter(5);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_LINE";
                MyOraDB.Parameter_Name[1] = "V_P_MLINE";
                MyOraDB.Parameter_Name[2] = "V_P_YMD";
                MyOraDB.Parameter_Name[3] = "V_P_LOCATION";
                // MyOraDB.Parameter_Name[3] = "CV_1";

                MyOraDB.Parameter_Name[4] = "CV_1";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[3] = (char)OracleType.VarChar;
                // MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;    
                MyOraDB.Parameter_Type[4] = (char)OracleType.Cursor;
                // MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = LINE;
                MyOraDB.Parameter_Values[1] = MLINE;
                MyOraDB.Parameter_Values[2] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[3] = v_p_location;
                MyOraDB.Parameter_Values[4] = "";
                // MyOraDB.Parameter_Values[3] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);

                if (v_loc == "VJ1")
                {
                    grpVC.Text = "VINH CUU";
                }

                else if (v_loc == "FTY01")
                {
                    grpVC.Text = "FACTORY 1";
                }

                else
                {
                    grpVC.Text = "PLANT N";
                }






                ds_ret = MyOraDB.Exe_Select_Procedure();
                if (ds_ret == null) return null;
                return ds_ret;


            }
            catch
            {
                return null;
            }
        }

        private System.Data.DataSet Select_Ora_Grid_Total()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_LT_TOTAL";
                //ARGMODE
                MyOraDB.ReDim_Parameter(6);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_LINE";
                MyOraDB.Parameter_Name[1] = "V_P_MLINE";
                MyOraDB.Parameter_Name[2] = "V_P_YMD";
                MyOraDB.Parameter_Name[3] = "V_P_TRIP";
                MyOraDB.Parameter_Name[4] = "V_P_LOCATION";
                MyOraDB.Parameter_Name[5] = "CV_1";

                // MyOraDB.Parameter_Name[3] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[3] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[4] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[5] = (char)OracleType.Cursor;
                // MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = LINE;
                MyOraDB.Parameter_Values[1] = MLINE;
                MyOraDB.Parameter_Values[2] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[3] = DtwTrip.Text;
                MyOraDB.Parameter_Values[4] = v_p_location;
                MyOraDB.Parameter_Values[5] = "";
                // MyOraDB.Parameter_Values[3] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();
                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }

        private System.Data.DataSet Select_Ora_Grid_Asy_Total()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_LT_ASY_TOTAL";
                //ARGMODE
                MyOraDB.ReDim_Parameter(6);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_LINE";
                MyOraDB.Parameter_Name[1] = "V_P_MLINE";
                MyOraDB.Parameter_Name[2] = "V_P_YMD";
                MyOraDB.Parameter_Name[3] = "V_P_TRIP";
                MyOraDB.Parameter_Name[4] = "V_P_LOCATION";
                MyOraDB.Parameter_Name[5] = "CV_1";

                // MyOraDB.Parameter_Name[3] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[3] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[4] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[5] = (char)OracleType.Cursor;
                // MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = LINE;
                MyOraDB.Parameter_Values[1] = MLINE;
                MyOraDB.Parameter_Values[2] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[3] = DtwTrip.Text;
                MyOraDB.Parameter_Values[4] = v_p_location;
                MyOraDB.Parameter_Values[5] = "";
                // MyOraDB.Parameter_Values[3] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();
                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }

        private System.Data.DataSet Select_Ora_Grid_Ratio()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_LT_VC_RATIO";
                //ARGMODE
                MyOraDB.ReDim_Parameter(6);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_LINE";
                MyOraDB.Parameter_Name[1] = "V_P_MLINE";
                MyOraDB.Parameter_Name[2] = "V_P_YMD";
                MyOraDB.Parameter_Name[3] = "V_P_TRIP";
                MyOraDB.Parameter_Name[4] = "V_P_LOCATION";
                // MyOraDB.Parameter_Name[3] = "CV_1";

                MyOraDB.Parameter_Name[5] = "CV_1";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[3] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[4] = (char)OracleType.VarChar;
                // MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;    
                MyOraDB.Parameter_Type[5] = (char)OracleType.Cursor;
                // MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = LINE;
                MyOraDB.Parameter_Values[1] = MLINE;
                MyOraDB.Parameter_Values[2] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[3] = DtwTrip.Text;
                MyOraDB.Parameter_Values[4] = v_p_location;
                MyOraDB.Parameter_Values[5] = "";
                // MyOraDB.Parameter_Values[3] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);




                ds_ret = MyOraDB.Exe_Select_Procedure();
                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }


        private System.Data.DataSet Select_Ora_Grid_Train()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_TRAIN_TIME";
                //ARGMODE
                MyOraDB.ReDim_Parameter(2);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_YMD";
                MyOraDB.Parameter_Name[1] = "CV_1";

                // MyOraDB.Parameter_Name[3] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.Cursor;
                // MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[1] = "";
                // MyOraDB.Parameter_Values[3] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();



                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }

        private System.Data.DataSet Select_Train_Real_Time()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_TRAIN_REAL_TIME";
                //ARGMODE
                MyOraDB.ReDim_Parameter(2);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_YMD";
                MyOraDB.Parameter_Name[1] = "CV_1";

                // MyOraDB.Parameter_Name[3] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.Cursor;
                // MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[1] = "";
                // MyOraDB.Parameter_Values[3] = "";
                // MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();



                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }
        private System.Data.DataSet Select_Total_Trip()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.SP_TMS_LT_TOTAL_TRIP";
                //ARGMODE
                MyOraDB.ReDim_Parameter(3);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "V_P_YMD";
                MyOraDB.Parameter_Name[1] = "V_P_LOCATION";
                MyOraDB.Parameter_Name[2] = "CV_1";



                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.Cursor;


                MyOraDB.Parameter_Values[0] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[1] = v_p_location;
                MyOraDB.Parameter_Values[2] = "";



                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();



                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }
        private System.Data.DataSet Select_Ora_Gauge()
        {
            try
            {
                COM.OraDB MyOraDB = new COM.OraDB();
                System.Data.DataSet ds_ret;

                string process_name = "MES.PKG_TMS.TMS_SEQ_SUM_SEL";
                //ARGMODE
                MyOraDB.ReDim_Parameter(4);
                MyOraDB.Process_Name = process_name;
                MyOraDB.Parameter_Name[0] = "ARG_DATE";
                MyOraDB.Parameter_Name[1] = "ARG_LINE";
                MyOraDB.Parameter_Name[2] = "ARG_SEQ";
                MyOraDB.Parameter_Name[3] = "OUT_CURSOR";

                MyOraDB.Parameter_Type[0] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[1] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[2] = (char)OracleType.VarChar;
                MyOraDB.Parameter_Type[3] = (char)OracleType.Cursor;

                MyOraDB.Parameter_Values[0] = DateTime.Now.ToString("yyyyMMdd");
                MyOraDB.Parameter_Values[1] = "014";
                MyOraDB.Parameter_Values[2] = "1";
                MyOraDB.Parameter_Values[3] = "";


                MyOraDB.Add_Select_Parameter(true);
                ds_ret = MyOraDB.Exe_Select_Procedure();
                if (ds_ret == null) return null;
                return ds_ret;
            }
            catch
            {
                return null;
            }
        }

        public static Dictionary<string, Tuple<string, string, string, string, string, string>> getInitForm2(string dll_name, string class_name)
        {
            COM.OraDB MyOraDB = new COM.OraDB();
            DataSet ds_ret;
            DataTable dt;
            Dictionary<string, Tuple<string, string, string, string, string, string>> dtn = null;
            string process_name = "SEPHIROTH.PROC_STB_GET_FORM_INIT";

            MyOraDB.ReDim_Parameter(3);
            MyOraDB.Process_Name = process_name;

            MyOraDB.Parameter_Name[0] = "ARG_DLL_NM";
            MyOraDB.Parameter_Name[1] = "ARG_CLASS_NM";
            MyOraDB.Parameter_Name[2] = "OUT_CURSOR";

            MyOraDB.Parameter_Type[0] = (int)OracleType.VarChar;
            MyOraDB.Parameter_Type[1] = (int)OracleType.VarChar;
            MyOraDB.Parameter_Type[2] = (int)OracleType.Cursor;

            MyOraDB.Parameter_Values[0] = dll_name;
            MyOraDB.Parameter_Values[1] = class_name;
            MyOraDB.Parameter_Values[2] = "";

            MyOraDB.Add_Select_Parameter(true);
            ds_ret = MyOraDB.Exe_Select_Procedure();

            if (ds_ret != null)
            {
                dt = ds_ret.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtn = new Dictionary<string, Tuple<string, string, string, string, string, string>>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtn.Add(dt.Rows[i]["COM_NM"].ToString()
                               , new Tuple<string, string, string, string, string, string>(dt.Rows[i]["COM_VL"].ToString(), dt.Rows[i]["VALUE2"].ToString(), dt.Rows[i]["VALUE3"].ToString()
                                                                                         , dt.Rows[i]["VALUE4"].ToString(), dt.Rows[i]["VALUE5"].ToString(), dt.Rows[i]["VALUE6"].ToString()));
                    }
                }
            }
            return dtn;
        }
        #endregion DB


        private void load_Data()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                DataTable data = null;
                DataTable data_total = null;
                DataTable data_ratio = null;
                DataTable data_train = null;
                DataTable data_train_Rtime = null;
                DataTable data_asy_total = null;
                DataTable data_trip_total = null;
                data = Select_Ora_Grid().Tables[0];
                if (v_thread.Equals("DEPART"))
                    data = data.Select("COMPONENT = 'UPPER'").CopyToDataTable();
                else
                    data = Select_Ora_Grid().Tables[0];
                // data_total = Select_Ora_Grid_Total().Tables[0];
                data_ratio = Select_Ora_Grid_Ratio().Tables[0];
                data_train = Select_Ora_Grid_Train().Tables[0];
                data_train_Rtime = Select_Train_Real_Time().Tables[0];
                data_asy_total = Select_Ora_Grid_Asy_Total().Tables[0];
                data_trip_total = Select_Total_Trip().Tables[0];

                if (data.Rows.Count < 1)
                {
                    v_trip_time = "";
                }
                else
                {
                    v_trip_time = data.Rows[data.Rows.Count - 1]["TRIP"].ToString();
                }

                lb_total.Text = data_asy_total.Rows[0]["TOTAL"].ToString();
                lb_DD.Text = data_asy_total.Rows[0]["QTY_DD"].ToString();
                lb_D1.Text = data_asy_total.Rows[0]["QTY_D1"].ToString();
                lb_D2.Text = data_asy_total.Rows[0]["QTY_D2"].ToString();
                lb_D3.Text = data_asy_total.Rows[0]["QTY_D3"].ToString();



                Lb_TL_trip1.Text = data_train_Rtime.Rows[0]["DP1"].ToString();
                Lb_TL_trip2.Text = data_train_Rtime.Rows[0]["DP2"].ToString();
                Lb_TL_trip3.Text = data_train_Rtime.Rows[0]["DP3"].ToString();
                Lb_TL_trip4.Text = data_train_Rtime.Rows[0]["DP4"].ToString();
                Lb_TL_trip5.Text = data_train_Rtime.Rows[0]["DP5"].ToString();
                if (Lb_TL_trip1.Text.Length == 5 && data_trip_total.Rows.Count >= 1)
                {
                    Lb_TL_trip1.Text = data_train_Rtime.Rows[0]["DP1"].ToString() + " (" + data_trip_total.Rows[0][0].ToString() + ")";
                }
                if (Lb_TL_trip2.Text.Length == 5 && data_trip_total.Rows.Count >= 2)
                {
                    Lb_TL_trip2.Text = data_train_Rtime.Rows[0]["DP2"].ToString() + " (" + data_trip_total.Rows[1][0].ToString() + ")";
                }
                if (Lb_TL_trip3.Text.Length == 5 && data_trip_total.Rows.Count >= 3)
                {
                    Lb_TL_trip3.Text = data_train_Rtime.Rows[0]["DP3"].ToString() + " (" + data_trip_total.Rows[2][0].ToString() + ")";
                }
                if (Lb_TL_trip4.Text.Length == 5 && data_trip_total.Rows.Count >= 4)
                {
                    Lb_TL_trip4.Text = data_train_Rtime.Rows[0]["DP4"].ToString() + " (" + data_trip_total.Rows[3][0].ToString() + ")";
                }
                if (Lb_TL_trip5.Text.Length == 5 && data_trip_total.Rows.Count >= 5)
                {
                    Lb_TL_trip5.Text = data_train_Rtime.Rows[0]["DP5"].ToString() + " (" + data_trip_total.Rows[4][0].ToString() + ")";
                }



                if (v_loc == "FTY01")
                {
                    Lb_VC_trip1.Text = data_train_Rtime.Rows[0]["AR_F1"].ToString();
                    Lb_VC_trip2.Text = data_train_Rtime.Rows[0]["AR_F2"].ToString();
                    Lb_VC_trip3.Text = data_train_Rtime.Rows[0]["AR_F3"].ToString();
                    Lb_VC_trip4.Text = data_train_Rtime.Rows[0]["AR_F4"].ToString();
                    Lb_VC_trip5.Text = data_train_Rtime.Rows[0]["AR_F5"].ToString();
                }
                else if (v_loc == "VJ1")
                {
                    Lb_VC_trip1.Text = data_train_Rtime.Rows[0]["LT_AR1"].ToString();
                    Lb_VC_trip2.Text = data_train_Rtime.Rows[0]["LT_AR2"].ToString();
                    Lb_VC_trip3.Text = data_train_Rtime.Rows[0]["LT_AR3"].ToString();
                    Lb_VC_trip4.Text = data_train_Rtime.Rows[0]["LT_AR4"].ToString();
                    Lb_VC_trip5.Text = data_train_Rtime.Rows[0]["LT_AR5"].ToString();
                }
                else
                {
                    Lb_VC_trip1.Text = data_train_Rtime.Rows[0]["AR_N1"].ToString();
                    Lb_VC_trip2.Text = data_train_Rtime.Rows[0]["AR_N2"].ToString();
                    Lb_VC_trip3.Text = data_train_Rtime.Rows[0]["AR_N3"].ToString();
                    Lb_VC_trip4.Text = data_train_Rtime.Rows[0]["AR_N4"].ToString();
                    Lb_VC_trip5.Text = data_train_Rtime.Rows[0]["AR_N5"].ToString();
                }



                if (data_train == null || data_train.Rows.Count < 1 || Convert.ToInt32(data_train.Rows[0]["TIME_GUESS"].ToString()) == 0 || Convert.ToInt32(data_train.Rows[0]["TIME_GUESS"].ToString()) > 59 || data_train.Rows[0]["LT_DEPART"].ToString() == "")
                {
                    time_depart = 0;
                }
                else
                {
                    time_depart = Convert.ToInt32(data_train.Rows[0]["TIME_GUESS"].ToString());
                }

                if (data_train != null)
                {
                    //dtwStart.Text = data_train.Rows[0]["LT_DEPART"].ToString();
                    //if (v_loc == "VJ1")
                    //{
                    //    dtwArrival.Text = data_train.Rows[0]["VC_ARRIVAL"].ToString();
                    //}
                    //else if (v_loc == "FTY01")
                    //{
                    //    dtwArrival.Text = data_train.Rows[0]["F1_ARRIAVAL"].ToString();
                    //}
                    //else
                    //{
                    //    dtwArrival.Text = data_train.Rows[0]["NOSN_ARRIVAL"].ToString();
                    //}
                }


                //if (data_total.Rows.Count == 0)
                //{
                //    lbTotal.Text = "";
                //}
                //else
                //{
                //    lbTotal.Text = data_total.Rows[0][0].ToString();
                //}
                if (data_ratio.Rows.Count < 1)
                {
                    dgG_Ratio.Text = "0";
                }
                else
                {
                    dgG_Ratio.Text = data_ratio.Rows[0][0].ToString();
                }

                grdBase.DataSource = data;
                gvwBase.Columns[0].AppearanceHeader.BackColor = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[0].AppearanceHeader.BackColor2 = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[0].AppearanceHeader.ForeColor = Color.White;




                gvwBase.Columns[3].AppearanceHeader.BackColor = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[3].AppearanceHeader.BackColor2 = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[3].AppearanceHeader.ForeColor = Color.White;
                gvwBase.Columns[4].AppearanceHeader.BackColor = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[4].AppearanceHeader.BackColor2 = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[4].AppearanceHeader.ForeColor = Color.White;
                gvwBase.Columns[5].AppearanceHeader.BackColor = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[5].AppearanceHeader.BackColor2 = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[5].AppearanceHeader.ForeColor = Color.White;
                gvwBase.Columns[6].AppearanceHeader.BackColor = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[6].AppearanceHeader.BackColor2 = Color.FromArgb(128, 128, 128);
                gvwBase.Columns[6].AppearanceHeader.ForeColor = Color.White;
                gvwBase.Columns[7].AppearanceHeader.BackColor = Color.FromArgb(57, 190, 29);
                gvwBase.Columns[7].AppearanceHeader.BackColor2 = Color.FromArgb(57, 190, 29);
                gvwBase.Columns[7].AppearanceHeader.ForeColor = Color.White;
                gvwBase.Columns[8].AppearanceHeader.BackColor = Color.FromArgb(255, 127, 0);
                gvwBase.Columns[8].AppearanceHeader.BackColor2 = Color.FromArgb(255, 127, 0);
                gvwBase.Columns[8].AppearanceHeader.ForeColor = Color.White;
                for (int i = 0; i < gvwBase.Columns.Count; i++)
                {
                    //if (i == 0 || i == 3 || i == 4 || i == 5 || i == 8)
                    //{
                    //    gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    //}
                    //else
                    //{
                    //    gvwBase.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    //}
                    gvwBase.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwBase.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gvwBase.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwBase.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;



                    gvwBase.Columns["TRIP"].Width = 100;
                    gvwBase.Columns["LINE"].Width = 100;
                    gvwBase.Columns["SET"].Width = 250;
                    gvwBase.Columns["MODEL"].Width = 500;
                    // gvwBase.Columns["START TIME"].Width = 50;
                    // gvwBase.Columns["ARRIVAL TIME"].Width = 50;
                    gvwBase.Columns["START TIME"].Visible = false;
                    gvwBase.Columns["ARRIVAL TIME"].Visible = false;
                    gvwBase.Columns["LINE_CD"].Visible = false;
                    gvwBase.Columns["MLINE_CD"].Visible = false;
                    gvwBase.Columns["SET_RATIO"].Visible = false;
                    if (v_thread.Equals("DEPART"))
                    {
                        gvwBase.Columns["COMPONENT"].Visible = false;
                        gvwBase.Columns["INCOMING QTY"].Caption = "UPPER OUTGOING";
                    }
                    else
                    {
                        gvwBase.Columns["COMPONENT"].Visible = true;
                        gvwBase.Columns["INCOMING QTY"].Caption = "INCOMING QTY";
                    }
                }

                gvwBase.OptionsView.AllowCellMerge = true;
                // gvwBase.TopRowIndex = gvwBase.RowCount - 15;

                //for (int i = 0; i < gvwBase.RowCount; i++)
                //{
                //    if (i % 2==0)
                //        gvwBase.Rows
                //}

                if (Convert.ToDouble(V_currentime) > 081500 && Convert.ToDouble(V_currentime) <= 094459)
                {
                    for (int i = 0; i < gvwBase.RowCount; i++)
                    {
                        if (i < gvwBase.RowCount - 1)
                        {
                            if (gvwBase.GetRowCellValue(i, "TRIP").ToString().Equals("1") && gvwBase.GetRowCellValue(i + 1, "TRIP").ToString().Equals("2"))
                            {
                                gvwBase.TopRowIndex = i - 18;
                                break;
                            }
                        }
                        else
                        {
                            gvwBase.TopRowIndex = gvwBase.RowCount - 18;
                            break;
                        }
                    }
                }
                if (Convert.ToDouble(V_currentime) > 094500 && Convert.ToDouble(V_currentime) <= 121459)
                {
                    for (int i = 0; i < gvwBase.RowCount; i++)
                    {
                        if (i < gvwBase.RowCount - 1)
                        {
                            if (gvwBase.GetRowCellValue(i, "TRIP").ToString().Equals("2") && gvwBase.GetRowCellValue(i + 1, "TRIP").ToString().Equals("3"))
                            {
                                gvwBase.TopRowIndex = i - 18;
                                break;
                            }
                        }
                        else
                        {
                            gvwBase.TopRowIndex = gvwBase.RowCount - 18;
                            break;
                        }
                    }
                }
                if (Convert.ToDouble(V_currentime) > 121500 && Convert.ToDouble(V_currentime) <= 135959)
                {
                    for (int i = 0; i < gvwBase.RowCount; i++)
                    {
                        if (i < gvwBase.RowCount - 1)
                        {
                            if (gvwBase.GetRowCellValue(i, "TRIP").ToString().Equals("3") && gvwBase.GetRowCellValue(i + 1, "TRIP").ToString().Equals("4"))
                            {
                                gvwBase.TopRowIndex = i - 18;
                                break;
                            }
                        }
                        else
                        {
                            gvwBase.TopRowIndex = gvwBase.RowCount - 18;
                            break;
                        }
                    }
                }
                if (Convert.ToDouble(V_currentime) > 140000 && Convert.ToDouble(V_currentime) <= 154459)
                {
                    for (int i = 0; i < gvwBase.RowCount; i++)
                    {
                        if (i < gvwBase.RowCount - 1)
                        {
                            if (gvwBase.GetRowCellValue(i, "TRIP").ToString().Equals("4") && gvwBase.GetRowCellValue(i + 1, "TRIP").ToString().Equals("5"))
                            {
                                gvwBase.TopRowIndex = i - 15;
                                break;
                            }
                        }
                        else
                        {
                            gvwBase.TopRowIndex = gvwBase.RowCount - 15;
                            break;
                        }
                    }
                }
                Label[] lbl = {Lb_TL_trip1,Lb_TL_trip2,Lb_TL_trip3,Lb_TL_trip4,Lb_TL_trip5,
                           Lb_VC_trip1,Lb_VC_trip2,Lb_VC_trip3,Lb_VC_trip4,Lb_VC_trip5};

                for (int i = 0; i < lbl.Length; i++)
                {
                    lbl[i].BackColor = Color.FromArgb(128, 128, 255);
                    lbl[i].ForeColor = Color.White;
                }

                if (Convert.ToDouble(V_currentime) > 154500)
                {
                    gvwBase.TopRowIndex = gvwBase.RowCount - 15;
                }

                /// lay trip cao set truoc, neu co thi lay neu ko co thi lay trip be hon
                /// 

                if ((Convert.ToDouble(V_currentime) > 154500 && Convert.ToDouble(V_currentime) < 200000) ||
                    (Convert.ToDouble(V_currentime) >= 153000 && Convert.ToDouble(V_currentime) <= 154500 && Lb_TL_trip5.Text.Substring(2, 1) == ":"))
                {

                    Lb_TL_trip5.BackColor = Color.Yellow;
                    Lb_TL_trip5.ForeColor = Color.White;
                    DtwTrip.Text = "05";
                    //  isPriority = 5;
                    Lb_VC_trip5.BackColor = Color.Yellow;
                    Lb_VC_trip5.ForeColor = Color.White;

                    if (Lb_TL_trip5.Text.Substring(2, 1) == ":")
                    {
                        dtwStart.Text = Lb_TL_trip5.Text.Substring(0, 5);
                    }
                    else
                    {
                        dtwStart.Text = "";
                    }


                    // dtwStart.Text = "15:50";
                    gvwBase.TopRowIndex = gvwBase.RowCount - 15;

                }
                else if ((Convert.ToDouble(V_currentime) > 140000 && Convert.ToDouble(V_currentime) <= 154459) ||
                    (Convert.ToDouble(V_currentime) >= 134500 && Convert.ToDouble(V_currentime) <= 140000 && Lb_TL_trip4.Text.Substring(2, 1) == ":")
                       )
                {
                    {

                        Lb_TL_trip4.BackColor = Color.Yellow;
                        Lb_TL_trip4.ForeColor = Color.White;
                        DtwTrip.Text = "04";
                        //   isPriority = 4;
                        Lb_VC_trip4.BackColor = Color.Yellow;
                        Lb_VC_trip4.ForeColor = Color.White;
                        // dtwStart.Text = "14:05";

                        if (Lb_TL_trip4.Text.Substring(2, 1) == ":")
                        {
                            dtwStart.Text = Lb_TL_trip4.Text.Substring(0, 5);
                        }
                        else
                        {
                            dtwStart.Text = "";
                        }

                    }
                }
                else if ((Convert.ToDouble(V_currentime) > 121500 && Convert.ToDouble(V_currentime) <= 135959) ||
                    (Convert.ToDouble(V_currentime) >= 120000 && Convert.ToDouble(V_currentime) <= 121500 && Lb_TL_trip3.Text.Substring(2, 1) == ":")
                    )
                {

                    Lb_TL_trip3.BackColor = Color.Yellow;
                    Lb_TL_trip3.ForeColor = Color.White;
                    DtwTrip.Text = "03";
                    //isPriority = 3;
                    Lb_VC_trip3.BackColor = Color.Yellow;
                    Lb_VC_trip3.ForeColor = Color.White;

                    if (Lb_TL_trip3.Text.Substring(2, 1) == ":")
                    {
                        dtwStart.Text = Lb_TL_trip3.Text.Substring(0, 5);
                    }
                    else
                    {
                        dtwStart.Text = "";
                    }

                    // dtwStart.Text = "12:16";

                }
                else if ((Convert.ToDouble(V_currentime) > 094500 && Convert.ToDouble(V_currentime) <= 121459) ||
                     (Convert.ToDouble(V_currentime) >= 093000 && Convert.ToDouble(V_currentime) <= 094500 && Lb_TL_trip2.Text.Substring(2, 1) == ":")
                    )
                {

                    Lb_TL_trip2.BackColor = Color.Yellow;
                    Lb_TL_trip2.ForeColor = Color.White;
                    DtwTrip.Text = "02";
                    // isPriority = 2;
                    Lb_VC_trip2.BackColor = Color.Yellow;
                    Lb_VC_trip2.ForeColor = Color.White;

                    if (Lb_TL_trip2.Text.Substring(2, 1) == ":")
                    {
                        dtwStart.Text = Lb_TL_trip2.Text.Substring(0, 5);
                    }
                    else
                    {
                        dtwStart.Text = "";
                    }
                    // dtwStart.Text = "09:50";


                }
                else if ((Convert.ToDouble(V_currentime) > 081500 && Convert.ToDouble(V_currentime) <= 094459) ||
                        (Convert.ToDouble(V_currentime) >= 075000 && Convert.ToDouble(V_currentime) <= 081500 && Lb_TL_trip1.Text.Substring(2, 1) == ":")
                        )
                {

                    Lb_TL_trip1.BackColor = Color.Yellow;
                    Lb_TL_trip1.ForeColor = Color.White;
                    DtwTrip.Text = "01";
                    //   isPriority = 1;
                    Lb_VC_trip1.BackColor = Color.Yellow;
                    Lb_VC_trip1.ForeColor = Color.White;
                    if (Lb_TL_trip1.Text.Substring(2, 1) == ":")
                    {
                        dtwStart.Text = Lb_TL_trip1.Text.Substring(0, 5);
                    }

                    else
                    {
                        dtwStart.Text = "";
                    }

                    if (v_loc == "VJ1")
                    {
                        dtwArrival.Text = data_train.Rows[0]["VC_ARRIVAL"].ToString();
                    }

                }
                else
                {
                }

                data_total = Select_Ora_Grid_Total().Tables[0];

                if (data_total.Rows.Count == 0)
                {
                    lbTotal.Text = "";
                }
                else
                {
                    lbTotal.Text = data_total.Rows[0][0].ToString();
                }




                if (v_loc == "VJ1")
                {
                    if (DtwTrip.Text == "01" && data_train_Rtime.Rows[0]["LT_AR1"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["LT_AR1"].ToString();
                    }
                    else if (DtwTrip.Text == "02" && data_train_Rtime.Rows[0]["LT_AR2"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["LT_AR2"].ToString();
                    }
                    else if (DtwTrip.Text == "03" && data_train_Rtime.Rows[0]["LT_AR3"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["LT_AR3"].ToString();
                    }
                    else if (DtwTrip.Text == "04" && data_train_Rtime.Rows[0]["LT_AR4"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["LT_AR4"].ToString();
                    }
                    else if (DtwTrip.Text == "05" && data_train_Rtime.Rows[0]["LT_AR5"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["LT_AR5"].ToString();
                    }
                    else
                    {
                        dtwArrival.Text = "";
                    }


                }

                if (v_loc == "FTY01")
                {
                    if (DtwTrip.Text == "01" && data_train_Rtime.Rows[0]["AR_F1"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_F1"].ToString();
                    }
                    else if (DtwTrip.Text == "02" && data_train_Rtime.Rows[0]["AR_F2"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_F2"].ToString();
                    }
                    else if (DtwTrip.Text == "03" && data_train_Rtime.Rows[0]["AR_F3"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_F3"].ToString();
                    }
                    else if (DtwTrip.Text == "04" && data_train_Rtime.Rows[0]["AR_F4"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_F4"].ToString();
                    }
                    else if (DtwTrip.Text == "05" && data_train_Rtime.Rows[0]["AR_F5"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_F5"].ToString();
                    }
                    else
                    {
                        dtwArrival.Text = "";
                    }


                }

                if (v_loc == "099")
                {
                    if (DtwTrip.Text == "01" && data_train_Rtime.Rows[0]["AR_N1"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_N1"].ToString();
                    }
                    else if (DtwTrip.Text == "02" && data_train_Rtime.Rows[0]["AR_N2"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_N2"].ToString();
                    }
                    else if (DtwTrip.Text == "03" && data_train_Rtime.Rows[0]["AR_N3"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_N3"].ToString();
                    }
                    else if (DtwTrip.Text == "04" && data_train_Rtime.Rows[0]["AR_N4"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_N4"].ToString();
                    }
                    else if (DtwTrip.Text == "05" && data_train_Rtime.Rows[0]["AR_N5"].ToString().Length == 5)
                    {
                        dtwArrival.Text = data_train_Rtime.Rows[0]["AR_N5"].ToString();
                    }
                    else
                    {
                        dtwArrival.Text = "";
                    }


                }
                splashScreenManager1.CloseWaitForm();
            }
            catch
            { splashScreenManager1.CloseWaitForm(); }
        }

        private void gvwBase_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "SET")
                {
                    if (!string.IsNullOrEmpty(gvwBase.GetRowCellValue(e.RowHandle, "SET_RATIO").ToString()))
                    {
                        double Ratio = Convert.ToDouble(gvwBase.GetRowCellValue(e.RowHandle, "SET_RATIO"));
                        if (Ratio >= 98)
                        {
                            e.Appearance.BackColor = Color.Green;
                            e.Appearance.ForeColor = Color.White;
                        }
                        else if (Ratio >= 95)
                        {
                            e.Appearance.BackColor = Color.Yellow;
                            e.Appearance.ForeColor = Color.Black;
                        }
                        else
                        {
                            e.Appearance.BackColor = Color.Red;
                            e.Appearance.ForeColor = Color.White;
                        }
                    }
                    //if (gvwBase.GetRowCellValue(e.RowHandle, "SET").ToString().Contains("100%"))
                    //{

                    //}
                    //else
                    //{
                    //    e.Appearance.BackColor = Color.Red;
                    //    e.Appearance.ForeColor = Color.White;
                    //}
                }
                if (e.Column.FieldName == "COMPONENT" || e.Column.FieldName == "INCOMING QTY")
                {
                    if (!v_thread.Equals("DEPART"))
                        if (gvwBase.GetRowCellValue(e.RowHandle, "COMPONENT").ToString().ToUpper().Contains("UPPER"))
                        {
                            e.Appearance.BackColor = Color.LightSkyBlue;
                            e.Appearance.ForeColor = Color.White;
                        }
                }
            }
            catch (Exception ex)
            {

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
            //            int x = (int)(origin.X +  angle );
            //            int y = (int)(origin.Y  );
            //            pic_Car.Location = new Point(x, y);
            //            if (angle == 662)
            //            {
            //                angle = 0;
            //            }

            //        }
            //        catch { }
            //    }
            //    );

            //if  (lbTotal.InvokeRequired == true)
            //    lbTotal.Invoke((MethodInvoker)delegate
            //    {
            //        try
            //        {
            //            angle += rotSpeed;
            //            //   int x = (int)(origin.X + distance * Math.Sin(angle * Math.PI / 100));
            //            //  int y = (int)(origin.Y + distance * Math.Cos(angle * Math.PI / 300f));
            //            int x = (int)(origin_lb.X  + angle );
            //            int y = (int)(origin_lb.Y);
            //            lbTotal.Location = new Point(x, y);
            //            if (angle == 662)
            //            {
            //                angle = 0;
            //            }

            //        }
            //        catch { }
            //    }
            //    );
        }

        private void cbdate_EditValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    load_Data();
            //}
            //catch
            //{

            //}
        }

        int location_car = 0;
        int location_lb = 0;
        int minutes = 0;
        private void tmr_rotate_er_check_Tick(object sender, EventArgs e)
        {

            angle += rotSpeed;
            DateTime.Now.ToString("HHmm");


            //if (DateTime.Now.ToString("HHmm") == "0818" || DateTime.Now.ToString("HHmm") == "0950" || DateTime.Now.ToString("HHmm") == "1216" ||
            //    DateTime.Now.ToString("HHmm") == "1405" || DateTime.Now.ToString("HHmm") == "1550" )
            if (dtwStart.Text == "")
            {

                pic_Car.Location = new Point(pic_Car.Location.X + 1, pic_Car.Location.Y);
            }
            //if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) < 0818)
            //{
            //    pic_Car.Location = new Point(pic_Car.Location.X + 1, pic_Car.Location.Y);
            //}
            else
            {
                DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(dtwStart.Text.Substring(0, 2)), Convert.ToInt32(dtwStart.Text.Substring(3, 2)), 00);
                DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), 00);

                TimeSpan span = endTime.Subtract(startTime);
                minutes = Convert.ToInt32(span.TotalMinutes);
            }



            //if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) >= 0818 && Convert.ToInt32(DateTime.Now.ToString("HHmm")) <= 0950)
            //{

            //    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 08, 18, 00);
            //    DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //    Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), 00);

            //    TimeSpan span = endTime.Subtract(startTime);
            //    minutes = Convert.ToInt32(span.TotalMinutes);
            //}
            //if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) > 0950 && Convert.ToInt32(DateTime.Now.ToString("HHmm")) <= 1216)
            //{

            //    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 09, 50, 00);
            //    DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //    Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), 00);

            //    TimeSpan span = endTime.Subtract(startTime);
            //    minutes = Convert.ToInt32(span.TotalMinutes);
            //}
            //if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) > 1216 && Convert.ToInt32(DateTime.Now.ToString("HHmm")) <= 1405)
            //{

            //    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 06, 00);
            //    DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //    Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), 00);

            //    TimeSpan span = endTime.Subtract(startTime);
            //    minutes = Convert.ToInt32(span.TotalMinutes);
            //}
            //if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) > 1405 && Convert.ToInt32(DateTime.Now.ToString("HHmm")) <= 1550)
            //{

            //    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 05, 00);
            //    DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //    Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), 00);

            //    TimeSpan span = endTime.Subtract(startTime);
            //    minutes = Convert.ToInt32(span.TotalMinutes);
            //}
            //if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) > 1550)
            //{

            //    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 50, 00);
            //    DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //    Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), 00);

            //    TimeSpan span = endTime.Subtract(startTime);
            //    minutes = Convert.ToInt32(span.TotalMinutes);
            //}
            if (minutes >= 0 && minutes < 15)
            {
                Time_pass1.BorderColor = Color.LightGreen;
                Time_pass1.FillColor = Color.LightGreen;
                Time_pass1.FillGradientColor = Color.LightGreen;

                Time_pass2.BorderColor = Color.Orange;
                Time_pass2.FillColor = Color.Orange;
                Time_pass2.FillGradientColor = Color.Orange;

                Time_pass3.BorderColor = Color.Orange;
                Time_pass3.FillColor = Color.Orange;
                Time_pass3.FillGradientColor = Color.Orange;

                Time_pass4.BorderColor = Color.Orange;
                Time_pass4.FillColor = Color.Orange;
                Time_pass4.FillGradientColor = Color.Orange;

                Time_pass5.BorderColor = Color.Orange;
                Time_pass5.FillColor = Color.Orange;
                Time_pass5.FillGradientColor = Color.Orange;

            }
            if (minutes >= 15 && minutes < 30)
            {
                Time_pass1.BorderColor = Color.LightGreen;
                Time_pass1.FillColor = Color.LightGreen;
                Time_pass1.FillGradientColor = Color.LightGreen;

                Time_pass2.BorderColor = Color.LightGreen;
                Time_pass2.FillColor = Color.LightGreen;
                Time_pass2.FillGradientColor = Color.LightGreen;

                Time_pass3.BorderColor = Color.Orange;
                Time_pass3.FillColor = Color.Orange;
                Time_pass3.FillGradientColor = Color.Orange;

                Time_pass4.BorderColor = Color.Orange;
                Time_pass4.FillColor = Color.Orange;
                Time_pass4.FillGradientColor = Color.Orange;

                Time_pass5.BorderColor = Color.Orange;
                Time_pass5.FillColor = Color.Orange;
                Time_pass5.FillGradientColor = Color.Orange;
            }
            if (minutes >= 30 && minutes < 45)
            {
                Time_pass1.BorderColor = Color.LightGreen;
                Time_pass1.FillColor = Color.LightGreen;
                Time_pass1.FillGradientColor = Color.LightGreen;

                Time_pass2.BorderColor = Color.LightGreen;
                Time_pass2.FillColor = Color.LightGreen;
                Time_pass2.FillGradientColor = Color.LightGreen;

                Time_pass3.BorderColor = Color.LightGreen;
                Time_pass3.FillColor = Color.LightGreen;
                Time_pass3.FillGradientColor = Color.LightGreen;

                Time_pass4.BorderColor = Color.Orange;
                Time_pass4.FillColor = Color.Orange;
                Time_pass4.FillGradientColor = Color.Orange;

                Time_pass5.BorderColor = Color.Orange;
                Time_pass5.FillColor = Color.Orange;
                Time_pass5.FillGradientColor = Color.Orange;
            }
            if (minutes >= 45 && minutes < 60)
            {
                Time_pass1.BorderColor = Color.LightGreen;
                Time_pass1.FillColor = Color.LightGreen;
                Time_pass1.FillGradientColor = Color.LightGreen;

                Time_pass2.BorderColor = Color.LightGreen;
                Time_pass2.FillColor = Color.LightGreen;
                Time_pass2.FillGradientColor = Color.LightGreen;

                Time_pass3.BorderColor = Color.LightGreen;
                Time_pass3.FillColor = Color.LightGreen;
                Time_pass3.FillGradientColor = Color.LightGreen;

                Time_pass4.BorderColor = Color.LightGreen;
                Time_pass4.FillColor = Color.LightGreen;
                Time_pass4.FillGradientColor = Color.LightGreen;

                Time_pass5.BorderColor = Color.Orange;
                Time_pass5.FillColor = Color.Orange;
                Time_pass5.FillGradientColor = Color.Orange;
            }

            if (minutes >= 60)
            {
                pic_Car.Location = new Point(1220, pic_Car.Location.Y);
                lbTotal.Location = new Point(1220, 91);

                Time_pass1.BorderColor = Color.LightGreen;
                Time_pass1.FillColor = Color.LightGreen;
                Time_pass1.FillGradientColor = Color.LightGreen;

                Time_pass2.BorderColor = Color.LightGreen;
                Time_pass2.FillColor = Color.LightGreen;
                Time_pass2.FillGradientColor = Color.LightGreen;

                Time_pass3.BorderColor = Color.LightGreen;
                Time_pass3.FillColor = Color.LightGreen;
                Time_pass3.FillGradientColor = Color.LightGreen;

                Time_pass4.BorderColor = Color.LightGreen;
                Time_pass4.FillColor = Color.LightGreen;
                Time_pass4.FillGradientColor = Color.LightGreen;

                Time_pass5.BorderColor = Color.LightGreen;
                Time_pass5.FillColor = Color.LightGreen;
                Time_pass5.FillGradientColor = Color.LightGreen;
            }
            else
            {
                location_car = 563 + minutes * 10;
                location_lb = 563 + minutes * 10;
                pic_Car.Location = new Point(location_car, pic_Car.Location.Y);
                lbTotal.Location = new Point(location_lb + 10, 91);
            }
            //    pic_Car.Location = new Point(location_car , pic_Car.Location.Y);


            //if (angle == 662)
            //{
            //    angle = 0;
            //    pic_Car.Location = new Point(pic_Car.Location.X - 662, pic_Car.Location.Y);
            //}


            //   lbTotal.Location = new Point(lbTotal.Location.X + 1, lbTotal.Location.Y);   
            if (angle == 662)
            {
                angle = 0;
                lbTotal.Location = new Point(lbTotal.Location.X - 662, lbTotal.Location.Y);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            V_currentime = DateTime.Now.ToString("HHmmss");
            //    load_Data();
            count++;
            if (count >= 60)
            {
                load_Data();
                count = 0;
            }
            runTextModel();
        }
        string v_guess = "";
        string V_cur_starttime = DateTime.Now.ToString("HHmm");
        private void runTextModel()
        {


            string blank = "          ";
            if (time_depart == 0 || dtwStart.Text == "" || dtwStart.Text == null)
            {
                v_guess = "";
                addTextGauge(blank, dtgestimate);
            }
            else
            {
                v_guess = time_depart.ToString();
                addTextGauge(v_guess + " minutes left to arrive to Vinh Cuu" + blank, dtgestimate);

                _iStartText++;
            }


        }

        private void addTextGauge(string arg_str, DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge gauge)
        {

            if (arg_str.Length <= 20)
            {
                arg_str = arg_str.PadRight(20, ' ');
            }

            if (_iStartText + 1 > arg_str.Length)
            {
                _iStartText = 0;
            }

            gauge.Text += arg_str.Substring(_iStartText, 1);
        }

        private void gvwBase_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void groupBoxEx4_Enter(object sender, EventArgs e)
        {
            //FRM_TMS_DASH DASH = new FRM_TMS_DASH();
            //DASH.Show();

        }

        private void gvwBase_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            try
            {
                if (e.RowHandle1 < 0 || gvwBase.RowCount == 0)
                    return;

                e.Merge = false;
                e.Handled = true;


                if (e.Column.FieldName == "TRIP")
                {
                    string line1 = gvwBase.GetRowCellValue(e.RowHandle1, "TRIP").ToString().Trim();
                    string line2 = gvwBase.GetRowCellValue(e.RowHandle2, "TRIP").ToString().Trim();
                    if (line1 == line2)
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }
                if (e.Column.FieldName == "LINE")
                {
                    string line1 = gvwBase.GetRowCellValue(e.RowHandle1, "LINE").ToString().Trim();
                    string line2 = gvwBase.GetRowCellValue(e.RowHandle2, "LINE").ToString().Trim();

                    if (line1 == line2)
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }
                if (e.Column.FieldName == "SET")
                {
                    string line1 = gvwBase.GetRowCellValue(e.RowHandle1, "LINE").ToString().Trim();
                    string line2 = gvwBase.GetRowCellValue(e.RowHandle2, "LINE").ToString().Trim();
                    string set1 = gvwBase.GetRowCellValue(e.RowHandle1, "SET").ToString().Trim();
                    string set2 = gvwBase.GetRowCellValue(e.RowHandle2, "SET").ToString().Trim();
                    string trip1 = gvwBase.GetRowCellValue(e.RowHandle1, "TRIP").ToString().Trim();
                    string trip2 = gvwBase.GetRowCellValue(e.RowHandle2, "TRIP").ToString().Trim();
                    string STYLE1 = gvwBase.GetRowCellValue(e.RowHandle1, "STYLE").ToString().Trim();
                    string STYLE2 = gvwBase.GetRowCellValue(e.RowHandle2, "STYLE").ToString().Trim();
                    if (set1 == set2 && trip1 == trip2 && STYLE1 == STYLE2 && line1 == line2)
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }



                if (e.Column.FieldName == "MODEL")
                {
                    string trip1 = gvwBase.GetRowCellValue(e.RowHandle1, "LINE").ToString().Trim();
                    string trip2 = gvwBase.GetRowCellValue(e.RowHandle2, "LINE").ToString().Trim();

                    string model1 = gvwBase.GetRowCellValue(e.RowHandle1, "MODEL").ToString().Trim();
                    string model2 = gvwBase.GetRowCellValue(e.RowHandle2, "MODEL").ToString().Trim();
                    if (trip1 == trip2 && model1 == model2)
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }
                if (e.Column.FieldName == "STYLE")
                {
                    string STYLE1 = gvwBase.GetRowCellValue(e.RowHandle1, "STYLE").ToString().Trim();
                    string STYLE2 = gvwBase.GetRowCellValue(e.RowHandle2, "STYLE").ToString().Trim();
                    string model1 = gvwBase.GetRowCellValue(e.RowHandle1, "MODEL").ToString().Trim();
                    string model2 = gvwBase.GetRowCellValue(e.RowHandle2, "MODEL").ToString().Trim();
                    string line1 = gvwBase.GetRowCellValue(e.RowHandle1, "LINE").ToString().Trim();
                    string line2 = gvwBase.GetRowCellValue(e.RowHandle2, "LINE").ToString().Trim();

                    string trip1 = gvwBase.GetRowCellValue(e.RowHandle1, "TRIP").ToString().Trim();
                    string trip2 = gvwBase.GetRowCellValue(e.RowHandle2, "TRIP").ToString().Trim();
                    if (trip1 == trip2 && STYLE1 == STYLE2 && model1 == model2 && line1 == line2)
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // this.MessageBoxW("gvwBase_CellMerge() \n " + ex.Message);
            }


        }



        private void gaugeControl4_Click(object sender, EventArgs e)
        {

        }

        private void gvwBase_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //   this.Cursor = Cursors.WaitCursor;
            if (e.Clicks < 2 || e.Column.FieldName != "SET")
                return;


            FRM_TMS_CAR_LT_POP pop = new FRM_TMS_CAR_LT_POP();
            pop.v_date = DateTime.Now.ToString("yyyyMMdd");
            pop.v_Trip = gvwBase.GetRowCellValue(e.RowHandle, "TRIP").ToString();
            pop.v_STYLE_CD = gvwBase.GetRowCellValue(e.RowHandle, "STYLE").ToString();
            //    pop.LINE = LINE;
            pop.v_LINE_CD = gvwBase.GetRowCellValue(e.RowHandle, "LINE_CD").ToString();
            pop.v_MLINE_CD = gvwBase.GetRowCellValue(e.RowHandle, "MLINE_CD").ToString();
            pop.v_p_location = v_p_location;

            //   pop.v_CMP_CD_IN = gvwBase.GetRowCellValue(e.RowHandle, "DIVISION").ToString();
            pop.ShowDialog();
            //   this.Cursor = Cursors.Default;
        }

        private void Lb_TL_trip1_VisibleChanged(object sender, EventArgs e)
        {

        }
        bool isBack = false;
        private void FRM_TMS_CAR_LT_NEW_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {

              

                if (ComVar.Var._strValue1 == "FTY01")
                {
                    v_p_location = "FTY01";
                    LINE = "";
                    MLINE = "";
                }
                else if (ComVar.Var._strValue1 == "VJ1")
                {
                    v_p_location = "VJ1";
                    LINE = "";
                    MLINE = "";
                }
                else
                {
                    v_p_location = "099";
                    LINE = "099";
                    MLINE = "001";
                }


                count = 60;
                timer1.Start();
            }
            else
                timer1.Stop();
        }

        private void timerblink_Tick(object sender, EventArgs e)
        {
            Label[] lbl = {Lb_TL_trip1,Lb_TL_trip2,Lb_TL_trip3,Lb_TL_trip4,Lb_TL_trip5,
                           Lb_VC_trip1,Lb_VC_trip2,Lb_VC_trip3,Lb_VC_trip4,Lb_VC_trip5};

            for (int i = 0; i < lbl.Length; i++)
            {
                if (lbl[i].BackColor == Color.Yellow)
                { lbl[i].BackColor = Color.Transparent; lbl[i].ForeColor = Color.Black; }
                else if (lbl[i].BackColor == Color.Transparent)
                { lbl[i].BackColor = Color.Yellow; lbl[i].ForeColor = Color.Black; }
            }

        }

        private void lblDate_DoubleClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void cmdBack_Click(object sender, EventArgs e)
        {
            //if (!isBack)
            //    ComVar.Var.callForm = "342";
            //else
                ComVar.Var.callForm = "back";
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            ////  WindowState = FormWindowState.Minimized ;
            //  ComVar.Var.callForm = "Minimized";
        }

        private void Lb_TL_trip1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (v_trip_time.ToString() == "")
            {
                this.Cursor = Cursors.Default;
                return;
            }
            else
            {
                FRM_TMS_TRIP_LT pop = new FRM_TMS_TRIP_LT();
                pop.v_date = DateTime.Now.ToString("yyyyMMdd");
                pop.v_Trip = "1";
                pop.LINE = LINE;
                pop.v_p_location = v_p_location;

                //   pop.v_CMP_CD_IN = gvwBase.GetRowCellValue(e.RowHandle, "DIVISION").ToString();
                pop.ShowDialog();
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void Lb_TL_trip2_Click(object sender, EventArgs e)
        {
            if (v_trip_time.ToString() == "" || Convert.ToInt32(v_trip_time.ToString()) < 2)
            {
                return;
            }

            FRM_TMS_TRIP_LT pop = new FRM_TMS_TRIP_LT();
            pop.v_date = DateTime.Now.ToString("yyyyMMdd");
            pop.v_Trip = "2";
            pop.LINE = LINE;
            pop.v_p_location = v_p_location;

            //   pop.v_CMP_CD_IN = gvwBase.GetRowCellValue(e.RowHandle, "DIVISION").ToString();
            pop.ShowDialog();
        }

        private void Lb_TL_trip3_Click(object sender, EventArgs e)
        {
            if (v_trip_time.ToString() == "" || Convert.ToInt32(v_trip_time.ToString()) < 3)
            {
                return;
            }
            FRM_TMS_TRIP_LT pop = new FRM_TMS_TRIP_LT();
            pop.v_date = DateTime.Now.ToString("yyyyMMdd");
            pop.v_Trip = "3";
            pop.LINE = LINE;
            pop.v_p_location = v_p_location;

            //   pop.v_CMP_CD_IN = gvwBase.GetRowCellValue(e.RowHandle, "DIVISION").ToString();
            pop.ShowDialog();
        }

        private void Lb_TL_trip4_Click(object sender, EventArgs e)
        {
            if (v_trip_time.ToString() == "" || Convert.ToInt32(v_trip_time.ToString()) < 4)
            {
                return;
            }
            FRM_TMS_TRIP_LT pop = new FRM_TMS_TRIP_LT();
            pop.v_date = DateTime.Now.ToString("yyyyMMdd");
            pop.v_Trip = "4";
            pop.LINE = LINE;
            pop.v_p_location = v_p_location;

            //   pop.v_CMP_CD_IN = gvwBase.GetRowCellValue(e.RowHandle, "DIVISION").ToString();
            pop.ShowDialog();
        }

        private void Lb_TL_trip5_Click(object sender, EventArgs e)
        {
            if (v_trip_time.ToString() == "" || Convert.ToInt32(v_trip_time.ToString()) < 5)
            {
                return;
            }
            FRM_TMS_TRIP_LT pop = new FRM_TMS_TRIP_LT();
            pop.v_date = DateTime.Now.ToString("yyyyMMdd");
            pop.v_Trip = "5";
            pop.LINE = LINE;
            pop.v_p_location = v_p_location;

            //   pop.v_CMP_CD_IN = gvwBase.GetRowCellValue(e.RowHandle, "DIVISION").ToString();
            pop.ShowDialog();
        }

        private void GawRatio_DoubleClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //ComVar.Var._strValue1 = "099";
                //   ComVar.Var._strValue1 = "";
                //   MessageBox.Show("_strValue1: " + ComVar.Var._strValue1 + "\n_strValue2: " + ComVar.Var._strValue2);
                ComVar.Var.callForm = "178";
                this.Cursor = Cursors.Default;
            }

            catch (Exception)
            {


            }
        }

        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            //  WindowState = FormWindowState.Minimized ;
            ComVar.Var.callForm = "Minimized";
        }




        //   webBrowser1.Navigate("https://www.tracksolid.com/mainFrame");


        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);



        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);




        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;

        private void label7_Click(object sender, EventArgs e)
        {
            UnMinimize(FindWindow(IntPtr.Zero, @"https://www.tracksolid.com/mainFrame - Google Chrome"));


        }
        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_MAXIMIZE = 1;
        const int SW_RESTORE = 9;

        public static void UnMinimize(IntPtr handle)
        {
            WINDOWPLACEMENT WinPlacement = new WINDOWPLACEMENT();
            GetWindowPlacement(handle, out WinPlacement);
            if (WinPlacement.flags.HasFlag(WINDOWPLACEMENT.Flags.WPF_RESTORETOMAXIMIZED))
            {
                ShowWindow(handle, SW_MAXIMIZE);
            }
            else
            {
                ShowWindow(handle, (int)SW_RESTORE);
            }
        }



        [StructLayout(LayoutKind.Sequential)]
        //public struct RECT
        //{
        //    public Int32 Left;
        //    public Int32 Top;
        //    public Int32 Right;
        //    public Int32 Bottom;
        //}

        public struct POINT
        {
            public int x;
            public int y;
        }

        public struct WINDOWPLACEMENT
        {

            [Flags]
            public enum Flags : uint
            {
                WPF_ASYNCWINDOWPLACEMENT = 0x0004,
                WPF_RESTORETOMAXIMIZED = 0x0002,
                WPF_SETMINPOSITION = 0x0001
            }


            /// <summary>
            /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
            /// </summary>
            public uint length;
            /// <summary>
            /// The flags that control the position of the minimized window and the method by which the window is restored. This member can be one or more of the following values.
            /// </summary>
            /// 
            public Flags flags;//uint flags;
            /// <summary>
            /// The current show state of the window. This member can be one of the following values.
            /// </summary>
            public uint showCmd;
            /// <summary>
            /// The coordinates of the window's upper-left corner when the window is minimized.
            /// </summary>
            public POINT ptMinPosition;
            /// <summary>
            /// The coordinates of the window's upper-left corner when the window is maximized.
            /// </summary>
            public POINT ptMaxPosition;
            /// <summary>
            /// The window's coordinates when the window is in the restored position.
            /// </summary>
            public RECT rcNormalPosition;
        }

        private void pic_Car_DoubleClick(object sender, EventArgs e)
        {
            //Process a = Process.Start("https://www.tracksolid.com/mainFrame");

            //if (v_thread.ToString() == "HOP")
            //{
            //    Thread.Sleep(3000);
            //}
            //else if (v_thread.ToString() == "SPT")
            //{
            //    Thread.Sleep(6000);
            //}
            //else
            //{
            //    Thread.Sleep(10000);
            //}
            //UnMinimize(FindWindow(IntPtr.Zero, @"https://www.tracksolid.com/mainFrame - Google Chrome"));
            //SetWindowPos(FindWindow(IntPtr.Zero, @"https://www.tracksolid.com/mainFrame - Google Chrome"), 0, 500, 120, 1000, 800, int.Parse((SWP_NOZORDER | SWP_SHOWWINDOW).ToString()));
        }

        private void GawRatio_Click(object sender, EventArgs e)
        {

        }

        private void pic_Car_Click(object sender, EventArgs e)
        {
            Process a = Process.Start("https://www.tracksolid.com/mainFrame");

            if (v_thread.ToString() == "HOP")
            {
                Thread.Sleep(3000);
            }
            else if (v_thread.ToString() == "SPT")
            {
                Thread.Sleep(6000);
            }
            else
            {
                Thread.Sleep(10000);
            }
            UnMinimize(FindWindow(IntPtr.Zero, @"https://www.tracksolid.com/mainFrame - Google Chrome"));
            SetWindowPos(FindWindow(IntPtr.Zero, @"https://www.tracksolid.com/mainFrame - Google Chrome"), 0, 500, 120, 1000, 800, int.Parse((SWP_NOZORDER | SWP_SHOWWINDOW).ToString()));
        }


    }


}

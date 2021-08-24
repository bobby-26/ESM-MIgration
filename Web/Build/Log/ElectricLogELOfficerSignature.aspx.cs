using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Log_ElectricLogELOfficerSignature : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    string username = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
        if (IsPostBack == false)
        {
            ViewState["dutyengname"] = string.Empty;
            ViewState["dutyengrank"] = string.Empty;
            ViewState["rankshortcode"] = string.Empty;


        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string script = "closeTelerikWindow('engineersign')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {


            DataSet ds = PhoenixElog.UserLogin(username, txtPassword.Text);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var rankId = GetRankId();
                string watch = Request.QueryString["watch"].Trim();
                string watchno = Request.QueryString["watchno"].Trim();
                string excepetedRank = GetRankBasedOnWatch(watchno);
                DateTime date = DateTime.Now;
                if (string.IsNullOrEmpty(Request.QueryString["date"]) == false)
                {
                    DateTime currentDate = DateTime.Now;
                    date = DateTime.Parse(Request.QueryString["date"]);
                    date = date.AddHours(currentDate.Hour);
                    date = date.AddMinutes(currentDate.Minute);
                    date = date.AddSeconds(currentDate.Second);
                }
                if(rankId == null)
                {
                    ucError.ErrorMessage = "You are not authorized to sign.";
                    ucError.Visible = true;
                }

                PhoenixEngineLog.MainEngineLogHistoryInsert(usercode, vesselId, watch, int.Parse(rankId), "ALL", false, date, null, null, null, null, null, General.GetNullableString( watchno));

                 DataSet ds1 = null;
                if (string.IsNullOrWhiteSpace(Request.QueryString["date"]) == false)
                {
                    ds1 = PhoenixEngineLog.MainEngineLogSearch(usercode, vesselId, DateTime.Parse(Request.QueryString["date"]));
                }
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds1.Tables[0].Rows)
                    {
                        string exceljson = row["FLDEXCELJSON"].ToString();
                        if(!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "meparam")
                        {
                           string json =  setcolourmeparam(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 1, json, DateTime.Parse(Request.QueryString["date"]), "meparam");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "metemp")
                        {
                            string json = setcolourmetemp(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 2, json, DateTime.Parse(Request.QueryString["date"]), "metemp");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "turbocharger")
                        {
                            string json = setcolourturbocharger(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 3, json, DateTime.Parse(Request.QueryString["date"]), "turbocharger");
                        }

                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "meparam2")
                        {
                            string json = setcolourmeparam(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 4, json, DateTime.Parse(Request.QueryString["date"]), "meparam2");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "metemp2")
                        {
                            string json = setcolourmetemp(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 5, json, DateTime.Parse(Request.QueryString["date"]), "metemp1");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "turbocharger2")
                        {
                            string json = setcolourturbocharger(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 6, json, DateTime.Parse(Request.QueryString["date"]), "turbocharger2");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "generator")
                        {
                            string json = setcolourgenerator(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 7, json, DateTime.Parse(Request.QueryString["date"]), "generator");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "misc")
                        {
                            string json = setcolourmisc(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 8, json, DateTime.Parse(Request.QueryString["date"]), "misc");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "aircond")
                        {
                            string json = setcolouraircond(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 8, json, DateTime.Parse(Request.QueryString["date"]), "aircond");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "noonreport" && watchno == "watch4")
                        {
                            string json = setcolournoonreport(exceljson, watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 8, json, DateTime.Parse(Request.QueryString["date"]), "noonreport");
                        }
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "remarks")
                        {
                            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                            Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];

                            if (watchno == "watch1")
                            {
                                worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengname"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengrank"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 6).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            }
                            if (watchno == "watch2")
                            {
                                worksheet.Rows.FirstOrDefault(r => r.Index == 9).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengname"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengrank"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            }
                            if (watchno == "watch3")
                            {
                                worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengname"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengrank"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 16).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            }
                            if (watchno == "watch4")
                            {
                                worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengname"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengrank"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            }
                            if (watchno == "watch5")
                            {
                                worksheet.Rows.FirstOrDefault(r => r.Index == 24).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengname"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 25).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengrank"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 26).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            }
                            if (watchno == "watch6")
                            {
                                worksheet.Rows.FirstOrDefault(r => r.Index == 29).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengname"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 30).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["dutyengrank"].ToString();
                                worksheet.Rows.FirstOrDefault(r => r.Index == 31).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            }
                            var json = setcolourremarks(workbook.ToJson(), watchno);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 11, json, DateTime.Parse(Request.QueryString["date"]), "remarks");
                        }
                    }
                }
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("watch", watch);
                nvc.Add("name", ViewState["dutyengname"].ToString());
                nvc.Add("rank", ViewState["dutyengrank"].ToString());
                nvc.Add("date", date.ToString("dd-MM-yyyy"));
                Filter.CurrentEngineLogFilterCriteria = nvc;


                // close the popup
                string script = "closeTelerikWindow('engineersign', 'EngineLog')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string setcolourmeparam(string json,string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
       int watch = int.Parse( watchno.Substring(watchno.Length - 1));
        int[] rows = { 4, 5, 7, 8, 10, 11, 13, 14, 16, 17, 18 };
        int[] columns = { 2, 3, 4, 5, 6, 7 };
        foreach (var column in columns)
        {
            if (column != watch+1) continue;

            foreach (var row in rows)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }
        return workbook.ToJson();
    }

    private string setcolourmetemp(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int watch = int.Parse(watchno.Substring(watchno.Length - 1));
        int[] rows = { 4, 5,6 ,7, 8,9, 10, 11,12, 13, 14,15, 16, 17, 18,19,20 };
        int[] watch1 = { 2, 3, 4 };
        int[] watch2 = { 5, 6, 7 };
        int[] watch3 = { 8, 9, 10 };
        int[] watch4 = { 11, 12, 13 };
        int[] watch5 = { 14, 15, 16 };
        int[] watch6 = { 17, 18, 19 };

        int[] columns = new int[3];

        columns = watch == 1 ? watch1 : watch == 2 ? watch2 : watch == 3 ? watch3 : watch == 4 ? watch4 : watch == 5 ? watch5 : watch6;
        int i = 0;
        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                if (i == 0) { i = i + 1; continue; }
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") { i = i + 1; continue; }
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";

                i = i + 1;
            }
        }
        return workbook.ToJson();
    }
    private string setcolourturbocharger(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int watch = int.Parse(watchno.Substring(watchno.Length - 1));
        int[] rows = { 3, 5, 6, 7, 8, 9, 10, 11, 13, 15, 16, 17};
        int[] watch1 = { 3, 9, 15, 21 };
        int[] watch2 = { 4, 10, 16, 22 };
        int[] watch3 = { 5, 11, 17, 23 };
        int[] watch4 = { 6, 12, 18, 24 };
        int[] watch5 = { 7, 13, 19, 25 };
        int[] watch6 = { 8, 14, 20, 26 };

        int[] columns = new int[5];

        columns = watch == 1 ? watch1 : watch == 2 ? watch2 : watch == 3 ? watch3 : watch == 4 ? watch4 : watch == 5 ? watch5 : watch6;
        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue; 
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }
        return workbook.ToJson();
    }
    private string setcolourgenerator(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int watch = int.Parse(watchno.Substring(watchno.Length - 1));
        int[] rows = { 3, 5, 6, 7, 8, 9, 10, 11,12, 14, 15, 16, 17,19,20,21 };
        int[] watch1 = { 3, 9, 15, 21 };
        int[] watch2 = { 4, 10, 16, 22 };
        int[] watch3 = { 5, 11, 17, 23 };
        int[] watch4 = { 6, 12, 18, 24 };
        int[] watch5 = { 7, 13, 19, 25 };
        int[] watch6 = { 8, 14, 20, 26 };

        int[] columns = new int[5];

        columns = watch == 1 ? watch1 : watch == 2 ? watch2 : watch == 3 ? watch3 : watch == 4 ? watch4 : watch == 5 ? watch5 : watch6;
        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }
        return workbook.ToJson();
    }
    private string setcolourmisc(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int watch = int.Parse(watchno.Substring(watchno.Length - 1));
        int[] rows = { 4, 5, 6, 7, 8, 9, 11,12, 14, 15, 17,18, 19, 20, 21,22};
        int[] watch1 = { 2};
        int[] watch2 = { 3 };
        int[] watch3 = { 4 };
        int[] watch4 = { 5 };
        int[] watch5 = { 6 };
        int[] watch6 = { 7 };

        int[] columns = new int[3];

        columns = watch == 1 ? watch1 : watch == 2 ? watch2 : watch == 3 ? watch3 : watch == 4 ? watch4 : watch == 5 ? watch5 : watch6;
        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        int[] rows2 = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
        int[] watch12 = { 12 };
        int[] watch22 = { 13 };
        int[] watch32 = { 14 };
        int[] watch42 = { 15 };
        int[] watch52 = { 16 };
        int[] watch62 = { 17 };

        int[] columns2 = new int[3];

        columns2 = watch == 1 ? watch12 : watch == 2 ? watch22 : watch == 3 ? watch32 : watch == 4 ? watch42 : watch == 5 ? watch52 : watch6;
        foreach (var column in columns2)
        {
            foreach (var row in rows2)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        return workbook.ToJson();
    }
    private string setcolouraircond(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int watch = int.Parse(watchno.Substring(watchno.Length - 1));
        int[] rows = { 4, 5, 6, 7, 9, 10, 11, 12, 16, 18, 19, 20,21, 22, 24, 25, 26, 27};
        int[] watch1 = { 3};
        int[] watch2 = { 4 };
        int[] watch3 = { 5 };
        int[] watch4 = { 6 };
        int[] watch5 = { 7 };
        int[] watch6 = { 8 };

        int[] columns = new int[3];

        columns = watch == 1 ? watch1 : watch == 2 ? watch2 : watch == 3 ? watch3 : watch == 4 ? watch4 : watch == 5 ? watch5 : watch6;
        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        int[] rows2 = { 4, 5, 6, 7, 9, 10, 11, 12, 16,17, 18, 22, 23, 24, 25, 26};
        int[] watch12 = { 13 };
        int[] watch22 = { 14 };
        int[] watch32 = { 15 };
        int[] watch42 = { 16 };
        int[] watch52 = { 17 };
        int[] watch62 = { 18 };

        int[] columns2 = new int[3];

        columns2 = watch == 1 ? watch12 : watch == 2 ? watch22 : watch == 3 ? watch32 : watch == 4 ? watch42 : watch == 5 ? watch52 : watch62;
        foreach (var column in columns2)
        {
            foreach (var row in rows2)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }
        return workbook.ToJson();
    }
    private string setcolournoonreport(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int[] rows = { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21 };
        int[] columns = { 3,8 };
        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        int[] rows2 = {  3, 4, 5,6, 7 };
        int[] columns2 = { 14, 18 };
        foreach (var column in columns2)
        {
            foreach (var row in rows2)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        int[] rows3 = { 10, 11, 12, 13, 14, 15, 18, 19, 20, 21, 22 };
        int[] columns3 = { 14, 15, 16, 17, 18 };
        foreach (var column in columns3)
        {
            foreach (var row in rows3)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        int[] rows4 = { 28, 29, 30, 31, 32, 33, 34, 35 };
        int[] columns4 = { 15, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        foreach (var column in columns4)
        {
            foreach (var row in rows4)
            {
                if (worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color == "#ffa64d") continue;
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }

        return workbook.ToJson();
    }
    private string setcolourremarks(string json, string watchno)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(json);
        Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];
        int watch = int.Parse(watchno.Substring(watchno.Length - 1));
        int[] rows = new int[5];
        int[] watch1 = { 2, 3, 4, 5, 6 };
        int[] watch2 = { 7, 8, 9, 10, 11 };
        int[] watch3 = { 12, 13, 14, 15, 16 };
        int[] watch4 = { 17, 18, 19, 20, 21 };
        int[] watch5 = { 22, 23, 24, 25, 26 };
        int[] watch6 = { 27, 28, 29, 30, 31 };

        rows = watch == 1 ? watch1 : watch == 2 ? watch2 : watch == 3 ? watch3 : watch == 4 ? watch4 : watch == 5 ? watch5 : watch6;
        int[] columns = { 1 };

        foreach (var column in columns)
        {
            foreach (var row in rows)
            {
                worksheet.Rows.FirstOrDefault(r => r.Index == row).Cells.FirstOrDefault(c => c.Index == column).Color = "Black";
            }
        }
        return workbook.ToJson();
    }
    private string GetRankBasedOnWatch(string watch)
    {
        string rank = string.Empty;
        switch (watch)
        {
            case "watch4":
                rank = "3E";
                break;
            case "watch5":
                rank = "2E";
                break;
            case "watch6":
                rank = "4E";
                break;
            case "watch1":
                rank = "3E";
                break;
            case "watch2":
                rank = "2E";
                break;
            case "watch3":
                rank = "4E";
                break;
        }

        return rank;
    }

    private string GetRankId()
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            ViewState["dutyengname"] = row["FLDFIRSTNAME"].ToString() + row["FLDLASTNAME"].ToString();
            ViewState["dutyengrank"] = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
            ViewState["rankshortcode"] = ds.Tables[0].Rows[0]["FLDRANKCODE"].ToString();

            return row["FLDRANKID"].ToString();
        }
        return string.Empty;
    }
    
}
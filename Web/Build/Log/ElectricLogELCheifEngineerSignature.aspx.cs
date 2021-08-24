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

public partial class Log_ElectricLogELCheifEngineerSignature : System.Web.UI.Page
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
            ViewState["name"] = string.Empty;
            ViewState["rank"] = string.Empty;
            ViewState["date"] = string.Empty;
            ViewState["rankshortcode"] = string.Empty;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string script = "closeTelerikWindow('engineersign')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
    }

    private string GetRankId()
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            ViewState["name"] = row["FLDFIRSTNAME"].ToString() + row["FLDLASTNAME"].ToString();
            ViewState["rank"] = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
            ViewState["rankshortcode"] = ds.Tables[0].Rows[0]["FLDRANKCODE"].ToString();

            return row["FLDRANKID"].ToString();
        }
        return string.Empty;
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
                string error = Request.QueryString["error"].Trim();
                string excepetedRank = "CE";
                DateTime date = DateTime.Now;
                if (string.IsNullOrEmpty(Request.QueryString["date"]) == false)
                {
                    date = DateTime.Parse(Request.QueryString["date"]);
                }


                if (string.IsNullOrWhiteSpace(excepetedRank) || (string)ViewState["rankshortcode"] != excepetedRank)
                {
                    ucError.ErrorMessage = "Only valid officer can able to sign";
                    ucError.Visible = true;
                    return;
                }
                //if (error == "false")
                //{
                //    ucError.ErrorMessage = "Coplete the log for signing watch and try again.";
                //    ucError.Visible = true;
                //    return;
                //}
                int? islock = Request.QueryString["lock"] != null ? int.Parse( Request.QueryString["lock"].ToString()) : 0;
                // update the status && need to do the status part
                PhoenixEngineLog.MainEngineCheifEngineerSignature(usercode, vesselId, date, islock);
                if (Request.QueryString["lock"] == null)
                    PhoenixEngineLog.MainEngineLogHistoryInsert(usercode, vesselId, watch, int.Parse(rankId), "All", false, date, null, null, null, null, null);

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
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "remarks")
                        {
                            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                            Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];

                            worksheet.Rows.FirstOrDefault(r => r.Index == 36).Cells.FirstOrDefault(c => c.Index == 3).Value = ViewState["name"].ToString();
                            worksheet.Rows.FirstOrDefault(r => r.Index == 37).Cells.FirstOrDefault(c => c.Index == 3).Value = date.ToString("dd-MM-yyyy");
                            var json = workbook.ToJson();
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, 11, json, DateTime.Parse(Request.QueryString["date"]), "remarks");
                        }
                    }
                }
                //nvc
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("watch", watch);
                nvc.Add("name", ViewState["name"].ToString());
                nvc.Add("isCheifEngineer", "true");
                nvc.Add("date", date.ToString("dd-MM-yyyy"));
                nvc.Add("islock", islock.ToString());
                Filter.CheifEngineerSignatureFilterCriteria = nvc;


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
}
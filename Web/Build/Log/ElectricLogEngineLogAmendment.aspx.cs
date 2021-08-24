using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;

public partial class Log_ElectricLogEngineLogAmendment : PhoenixBasePage
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
        if (string.IsNullOrWhiteSpace(Request.QueryString["value"]) == false)
        {
            txtOrgValue.Text = Request.QueryString["value"];
        }
        
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["parameter"] != null)
            {
                string value = General.GetNullableString(txtRevisedValue.Text);
                string sheet = Request.QueryString["sheetname"].Trim();
                string address = Request.QueryString["address"].Trim();
                string rank = GetRankId();
                DateTime date = DateTime.Now;
                date = DateTime.Parse(Request.QueryString["date"].Trim());

                PhoenixEngineLog.MainEngineLogHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       , Request.QueryString["watch"].Trim()
                       , int.Parse(rank)
                       , Request.QueryString["parameter"].Trim()
                       , true
                       , date
                       , Request.QueryString["value"].Trim()
                       , value
                       , General.GetNullableString(txtReason.Text)
                       , null
                       , null);

                int? cellrow = General.GetNullableInteger(Request.QueryString["row"]);
                int? cellcol = General.GetNullableInteger(Request.QueryString["col"]);
                DataSet ds1 = PhoenixEngineLog.MainEngineLogSearch(usercode, vesselId, date);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds1.Tables[0].Rows)
                    {
                        string exceljson = row["FLDEXCELJSON"].ToString();
                        if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == sheet)
                        {
                            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                            Telerik.Web.Spreadsheet.Worksheet worksheet = workbook.Sheets[0];

                            worksheet.Rows.FirstOrDefault(r => r.Index == cellrow).Cells.FirstOrDefault(c => c.Index == cellcol).Value = value;
                            worksheet.Rows.FirstOrDefault(r => r.Index == cellrow).Cells.FirstOrDefault(c => c.Index == cellcol).Color = "Red";

                            var json = workbook.ToJson();
                            int isortorder = sortorder(sheet);
                            PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, isortorder, json, date, sheet);
                        }
                    }
                }

                    NameValueCollection nvc = new NameValueCollection();
                nvc.Add("address", address);
                nvc.Add("value", value);
                nvc.Add("sheet", sheet);
                Filter.AmendFilterCriteria = nvc;

                string script = string.Format("closeTelerikWindow('EngineLogAmend','EngineLog')");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private int sortorder(string sheetname)
    {
        int sortorder=0;
       switch(sheetname)
        {
            case "meparam": sortorder = 1; break;
            case "metemp": sortorder = 2; break;
            case "turbocharger": sortorder = 3; break;
            case "meparam2": sortorder = 4; break;
            case "metemp2": sortorder = 5; break;
            case "turbocharger2": sortorder = 6; break;
            case "generator": sortorder = 7; break;
            case "misc": sortorder = 8; break;
            case "aircond": sortorder = 9; break;
            case "noonreport": sortorder = 10; break;
            case "remarks": sortorder = 11; break;
        }
        return sortorder;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string script = string.Format("closeTelerikWindow('EngineLogAmend')");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CloseWindow", script, true);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string script = string.Format("javascript: parent.openNewWindow('engineersign', '', 'Log/ElectricLogELCheifEngineerSignatureAmend.aspx?watchno="+ Request.QueryString["watchno"] + "', 'false', '405', '180', null, null)");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenWindow", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
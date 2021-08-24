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
    
public partial class ElectricLogELCheifEngineerSignatureAmend : System.Web.UI.Page
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
                string watchno = Request.QueryString["watchno"].Trim();
                string excepetedRank =  GetRankBasedOnWatch(watchno);

                //if (string.IsNullOrWhiteSpace(excepetedRank) || (string)ViewState["rankshortcode"] != excepetedRank)
                //{
                //    ucError.ErrorMessage = "Only valid officer can able to sign";
                //    ucError.Visible = true;
                //    return;
                //}


                // update the status && need to do the status part
                //PhoenixEngineLog.MainEngineCheifEngineerSignature(usercode, vesselId, date);
                //PhoenixEngineLog.MainEngineLogHistoryInsert(usercode, vesselId, watch, int.Parse(rankId), "All", false, date, null, null, null, null, null);

                ////nvc
                //NameValueCollection nvc = new NameValueCollection();
                //nvc.Add("watch", watch);
                //nvc.Add("name", ViewState["name"].ToString());
                //nvc.Add("isCheifEngineer", "true");
                //nvc.Add("date", date.ToString("dd-MM-yyyy"));
                //Filter.CheifEngineerSignatureFilterCriteria = nvc;


                // close the popup
                string script = "closeTelerikWindow('engineersign', 'EngineLogAmend')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
            }
        }
        catch (Exception)
        {
            
        }
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
}
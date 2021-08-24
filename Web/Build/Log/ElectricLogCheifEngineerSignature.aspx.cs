using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;

public partial class Log_ElectricLogCheifEngineerSignature :  PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid txid;
    string popupname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (String.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["id"]);
        }

        if (IsPostBack == false)
        {
            ViewState["RANKID"] = string.Empty;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string script = "closeTelerikWindow('OfficerSign')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            if (isValidSignInput() == false)
            {
                ucError.Visible = true;
                return;
            }

            // check user login 
            if (CheckUserLogin() == false)
            {
                throw new ArgumentException("Please enter a valid password");
            }

            ds = PhoenixElog.UserDetails(usercode);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                
                string rank = GetRankName(usercode);
                if (PhoenixElog.validCheifEngineer(rank) == false)
                {
                    ucError.ErrorMessage = "Only valid chief engineer can able to sign";
                    ucError.Visible = true;
                    return;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                string officerName = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString();

                int officerId = Convert.ToInt32(dr["FLDUSERCODE"]);
                int officerRankId = Convert.ToInt32(ViewState["RANKID"]);
                string officerRankName = rank;

                string signature = string.Format("{0} - {1}", officerRankName, officerName);

                PhoenixElog.LogOfiicerSignature(usercode, txid, officerId, officerRankId, officerName, officerRankName, DateTime.Now, true, signature);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('OfficerSign', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool CheckUserLogin()
    {
        bool isLoggedIn = false;
        DataSet userData = PhoenixElog.UserLogin(PhoenixSecurityContext.CurrentSecurityContext.UserName, txtPassword.Text);

        if (userData != null && userData.Tables[0].Rows.Count > 0)
        {
            isLoggedIn = true;
        }
        return isLoggedIn;
    }

    private string GetRankName(int usercode)
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ViewState["RANKID"] = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
            return ds.Tables[0].Rows[0]["FLDRANKCODE"].ToString();
        }
        return "";
    }

    private bool isValidSignInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            ucError.ErrorMessage = "Password is required";
        }

        return (!ucError.IsError);
    }
}
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBBookEntryDelete : PhoenixBasePage
{
    Guid txid;
    int usercode = 0;
    int vesselID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselID = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (String.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = new Guid(Request.QueryString["TxnId"]);
        }
        ShowToolBar();
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
    }

    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidSignInput() == false)
            {
                ucError.Visible = true;
                return;
            }
            DataSet ds = PhoenixElog.UserLogin(txtUsername.Text, txtPassword.Text);
            //
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int RankID = 0;
                string rank = GetRankName(Convert.ToInt32(dr["FLDUSERCODE"]), ref RankID);

                if (rank == "MST")
                {
                    throw new ArgumentException("Master cannot delete the record");
                }

                txtUserCode.Text = (string)dr["FLDUSERCODE"].ToString();
                lblName.Text = dr["FLDFIRSTNAME"].ToString() + "   " + dr["FLDLASTNAME"].ToString();
                lblRank.Text = rank;
                lblRankId.Text = RankID.ToString();
                lblLoggedUser.Text = String.Format("Logged in as {0}", dr["FLDFIRSTNAME"].ToString() + "   " + dr["FLDLASTNAME"].ToString());
                txtUsername.Visible = false;
                txtPassword.Visible = false;
                lblUsername.Visible = false;
                lblPassword.Visible = false;
                btnLogin.Visible = false;
                lblheading.Visible = false;
                lblLoggedUser.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string GetRankName(int usercode, ref int rankID)
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            rankID = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDRANKID"]);
            return ds.Tables[0].Rows[0]["FLDRANKCODE"].ToString();
        }
        return "";
    }

    private bool isValidSignInput()
    {
        ucError.HeaderMessage = "Please enter the following details";
        if (String.IsNullOrWhiteSpace(txtUsername.Text))
        {
            ucError.Text = "Username is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            ucError.Text = "Password is mandatory";
        }
        return !ucError.IsError;
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                int rankId = Convert.ToInt32(lblRankId.Text);

                DataTable dt = PhoenixMarpolLogCRB.LogBookCRBBookEntryRecord(Convert.ToInt32(txtUserCode.Text), txid);
                if (dt.Rows.Count > 0)
                {

                DataRow row = dt.Rows[0];

                PhoenixMarpolLogCRB.DeletEBookEntryCRBRecord(Convert.ToInt32(txtUserCode.Text), txid, rankId, lblRank.Text, lblName.Text);

                PhoenixMarpolLogCRB.LogCRBHistoryDelete(usercode
                                   , vesselID
                                   , General.GetNullableInteger(row["FLDLOGID"].ToString())
                                   , txid
                                   , row["FLDCODE"].ToString()
                                   , row["FLDITEMNO"].ToString()
                                   , 1
                                   , row["FLDRECORD"].ToString()
                                   , row["FLDLINE1"].ToString()
                                   , row["FLDLINE2"].ToString()
                                   , row["FLDLINE3"].ToString()
                                   , row["FLDLINE4"].ToString()
                                   , row["FLDLINE5"].ToString()
                                   , row["FLDLINE6"].ToString()
                                   , row["FLDLINE7"].ToString()
                                   , row["FLDLINE8"].ToString()
                             );
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('LogDelete', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool isValidInput()
    {
        ucError.HeaderMessage = "Please enter the following details";
        if (string.IsNullOrWhiteSpace(txtUserCode.Text))
        {
            ucError.Text = "Login has to be made to delete the log";
        }
        return !ucError.IsError;
    }
}
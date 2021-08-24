using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ElectronicLog_ElectronicLogOperationApproval : PhoenixBasePage
{
    string ReportCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuOperationApproval.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["OffName"] = "";
            ViewState["IncName"] = "";
            ReportCode = Request.QueryString["ReportCode"].ToString();
            ViewState["TransactionID"] = Request.QueryString["TxnId"].ToString();
            if (ViewState["TransactionID"].ToString() != null && ViewState["TransactionID"].ToString() != "")
            {
                BindData();
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            if (ViewState["TransactionID"].ToString() != null && ViewState["TransactionID"].ToString() != "")
            {
                DataSet ds = new DataSet();

                ds = PhoenixElog.GetOperationRecord(
                                                    new Guid(ViewState["TransactionID"].ToString())
                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                    , ReportCode
                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    lblOperationDate.Text = Convert.ToDateTime(dr["FLDDATE"].ToString()).ToShortDateString();
                    txtFromLocation.Text = dr["FLDFROMLOCATION"].ToString();
                    txtToLocation.Text = dr["FLDTOLOCATION"].ToString();
                    txtbfrFromRob.Text =Convert.ToInt16(dr["FLDFROMROB"]).ToString() + " m3";
                    txtbfrtoRob.Text = Convert.ToInt16(dr["FLDTOROB"]).ToString()+ " m3";
                    txtaftfrmROB.Text = (Convert.ToInt16(dr["FLDFROMROB"]) - Convert.ToInt16(dr["FLDTXQTY"])).ToString() + " m3";
                    txtafttorob.Text = (Convert.ToInt16(dr["FLDTOROB"]) + Convert.ToInt16(dr["FLDTXQTY"])).ToString() + " m3";
                    txtDate.Text = Convert.ToDateTime(dr["FLDDATE"].ToString()).ToShortDateString();
                    txtItemNo.Text = dr["FLDITEMNO"].ToString();
                    txtCode.Text = dr["FLDCODE"].ToString();
                    lblsignonedate.Text = Convert.ToDateTime(dr["FLDDATE"].ToString()).ToShortDateString();
                    lblsigntwodate.Text = Convert.ToDateTime(dr["FLDDATE"].ToString()).ToShortDateString();
                    lblRecord.Text = dr["FLDFROMDESCRIPTION"].ToString();
                    lbltorecord.Text = dr["FLDTODESCRIPTION"].ToString();
                    lblincsign.Text = dr["FLDINCHARGENAME"].ToString();
                    lblChiefSign.Text = dr["FLDCHIEFNAME"].ToString();

                    if (string.IsNullOrWhiteSpace(lbltorecord.Text))
                    {
                        rowTwo.Style.Add("display", "none");
                    }
                    if (lblincsign.Text != "" && lblincsign.Text != null)
                    {
                        lblincsign.Visible = true;
                        txtUserName.Visible = false;
                        txtPassword.Visible = false;
                        btnIncharge.Visible = false;
                    }

                    if (lblChiefSign.Text != "" && lblChiefSign.Text != null)
                    {
                        lblChiefSign.Visible = true;
                        txtCheifUserName.Visible = false;
                        txtCheifPassword.Visible = false;
                        btnOfficer.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                ViewState["CmdName"] = "SAVE";

                if (!isValidInput())
                {
                    ucError.Visible = true;
                    return;
                }

                //PhoenixElog.OfficerSigned(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                          , new Guid(ViewState["TransactionID"].ToString())
                //                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                //                          , lblChiefSign.Text == "" ? ViewState["OffName"].ToString() : lblChiefSign.Text
                //                          , lblincsign.Text == "" ? ViewState["IncName"].ToString() : lblincsign.Text
                //                          , ViewState["Signby"].ToString()
                //                          );

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool isValidSignInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["Signby"].ToString() == "OFFICERINCHARGE")
        {
            if (General.GetNullableString(txtUserName.Text) == null)
                ucError.ErrorMessage = "UserName is required";
            if (General.GetNullableString(txtPassword.Text) == null)
                ucError.ErrorMessage = "Password is required";
        }
        if (ViewState["Signby"].ToString() == "OFFICER")
        {
            if (General.GetNullableString(txtCheifUserName.Text) == null)
                ucError.ErrorMessage = "UserName is required";
            if (General.GetNullableString(txtCheifPassword.Text) == null)
                ucError.ErrorMessage = "Password is required";
        }
     
        return (!ucError.IsError);

    }
    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if(ViewState["CmdName"].ToString() == "SAVE")
        {
            if ((ViewState["OffName"].ToString() == null || ViewState["OffName"].ToString() == "") && (ViewState["IncName"].ToString() == null || ViewState["IncName"].ToString() == ""))
            {
                ucError.ErrorMessage = "Officer Incharge or Cheif Officer Signature is required";
            }
        }
        return (!ucError.IsError);
    }

    protected void btnOfficer_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Signby"] = "OFFICER";

            DataSet ds = new DataSet();

            if (!isValidSignInput())
            {
                ucError.Visible = true;
                return;
            }

            ds = PhoenixElog.UserLogin(txtCheifUserName.Text, txtCheifPassword.Text);

            if (ds.Tables[0].Columns.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblChiefSign.Text = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();

                ViewState["OffName"] = lblChiefSign.Text;

                lblChiefSign.Visible = true;
                txtCheifUserName.Visible = false;
                txtCheifPassword.Visible = false;
                btnOfficer.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnIncharge_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Signby"] = "OFFICERINCHARGE";

            DataSet ds = new DataSet();

            if (!isValidSignInput())
            {
                ucError.Visible = true;
                return;
            }

            ds = PhoenixElog.UserLogin(txtUserName.Text, txtPassword.Text);

            if (ds.Tables[0].Columns.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblincsign.Text = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();

                ViewState["IncName"] = lblincsign.Text;

                lblincsign.Visible = true;
                txtUserName.Visible = false;
                txtPassword.Visible = false;
                btnIncharge.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
}
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPNIMedicalCaseClose : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                if (Request.QueryString["pniid"] != null && Request.QueryString["pniid"] != "")
                {
                    ViewState["PNIID"] = Request.QueryString["pniid"];
                }
                else
                    ViewState["PNIID"] = "";

                ViewState["STATUS"] = "";

                ddlReason.HardTypeCode = "241";
                ddlReason.bind();
                EditOperation();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["STATUS"].ToString() == "1")
                toolbar.AddButton("Reopen", "REOPEN",ToolBarDirection.Right);
            else
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuClose.AccessRights = this.ViewState;
            MenuClose.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditOperation()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.PNIDoctorReportEdit(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlReason.SelectedHard = dr["FLDREASONCLOSED"].ToString();
                txtClosingRemarks.Text = dr["FLDCLOSINGREMARKS"].ToString();
                txtReasonReopening.Text = dr["FLDREOPENREMARKS"].ToString();

                ViewState["STATUS"] = dr["FLDSTATUS"].ToString();

                if (dr["FLDSTATUS"].ToString() == "1")
                {
                    txtReasonReopening.Enabled = true;
                    txtReasonReopening.CssClass = "input_mandatory";
                    txtReasonReopening.Visible = true;
                    lblReasonReopening.Visible = true;
                    ddlReason.Enabled = false;
                    txtClosingRemarks.Enabled = false;
                }
                else
                {
                    txtReasonReopening.Enabled = false;
                    txtReasonReopening.Visible = false;
                    lblReasonReopening.Visible = false;
                    txtReasonReopening.CssClass = "input";

                    ddlReason.Enabled = true;
                    txtClosingRemarks.Enabled = true;
                }
                
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["STATUS"].ToString() == "1")
                toolbar.AddButton("Reopen", "REOPEN",ToolBarDirection.Right);
            else
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);

            MenuClose.AccessRights = this.ViewState;
            MenuClose.MenuList = toolbar.Show();

        }
    }
    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidateClose())
                {
                    ucError.Visible = true;
                    return;
                }
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "You will not be able to reopen the case,kindly confirm you want to close the case";
                RadWindowManager1.RadConfirm("You will not be able to reopen the case,kindly confirm you want to close the case", "confirm", 320, 150, null, "Confirm");

            }
            else if (CommandName.ToUpper().Equals("REOPEN"))
            {
                if (!IsValidateReOpen())
                {
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(ViewState["PNIID"].ToString()) != null)
                {
                    PhoenixInspectionPNI.PNICaseReopen(new Guid(ViewState["PNIID"].ToString())
                                                        , txtReasonReopening.Text
                                                        );
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                }
                EditOperation(); 
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidateClose()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlReason.SelectedHard) == null)
            ucError.ErrorMessage = "Reason for Closing is required for proceeding.";

        if (string.IsNullOrEmpty(txtClosingRemarks.Text))
            ucError.ErrorMessage = "Remarks is required for proceeding.";

        return (!ucError.IsError);
    }
    private bool IsValidateReOpen()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtReasonReopening.Text))
            ucError.ErrorMessage = "Reason for reopening is required for proceeding.";

        return (!ucError.IsError);
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
           
                if (General.GetNullableGuid(ViewState["PNIID"].ToString()) != null)
                {
                    PhoenixInspectionPNI.PNICaseClose(new Guid(ViewState["PNIID"].ToString())
                                                    , int.Parse(ddlReason.SelectedHard)
                                                    , txtClosingRemarks.Text
                                                    );
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                }
            
            EditOperation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

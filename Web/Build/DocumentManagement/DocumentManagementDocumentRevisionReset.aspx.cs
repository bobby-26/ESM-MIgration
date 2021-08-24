using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementDocumentRevisionReset : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        confirmReset.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Reset All", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Change Published Date", "PUBLISHEDDATE", ToolBarDirection.Right);


        MenuDocument.AccessRights = this.ViewState;
        MenuDocument.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            
            //PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Change Published Date", "PUBLISHEDDATE");
            //toolbar.AddButton("Reset All", "SAVE");
            //toolbar.AddButton("Clear", "CLEAR");

            //MenuDocument.AccessRights = this.ViewState;
            //MenuDocument.MenuList = toolbar.Show();
            BindUser();
            BindDocument();
            ViewState["RESETALLYN"] = "0";
        }
    }

    protected void DocumentEdit(string documentid)
    {
        if (General.GetNullableGuid(documentid) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(
                new Guid(documentid));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //txtDocumentName.Text = ds.Tables[0].Rows[0]["FLDDOCUMENTFULLNAME"].ToString();
                //txtRevision.Text = ds.Tables[0].Rows[0]["FLDREVISION"].ToString();                                
            }
        }
    }

    protected void BindUser()
    {
        ddlApprovedBy.DataTextField = "FLDDESIGNATIONNAME";
        ddlApprovedBy.DataValueField = "FLDUSERCODE";
        ddlApprovedBy.DataSource = PhoenixDocumentManagementDocumentAdmin.DMSUserList();
        ddlApprovedBy.DataBind();
        ddlApprovedBy.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void BindDocument()
    {
        ddlDocument.DataTextField = "FLDDOCUMENTNAME";
        ddlDocument.DataValueField = "FLDDOCUMENTID";
        ddlDocument.DataSource = PhoenixDocumentManagementDocument.DocumentList(1, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlDocument.DataBind();
        ddlDocument.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidReset())
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["RESETALLYN"] = "1";
                RadWindowManager1.RadConfirm("Do you want to reset the revisions.?", "confirmReset", 320, 150, null, "Confirm");
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlDocument.SelectedValue = "";
                txtRevisionDate.Text = "";
                chkApprovedByYN.Checked = false;
                ddlApprovedBy.SelectedValue = "";
                ddlApprovedBy.Enabled = false;
                ddlApprovedBy.CssClass = "readonlytextbox";
            }
            else if (CommandName.ToUpper().Equals("PUBLISHEDDATE"))
            {
                if (!IsValidReset())
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Do you want to reset the revisions.?", "confirmReset", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void confirmReset_Click(object sender, EventArgs e)
    {
        try
        {
            string approvedyn = "";
            if (chkApprovedByYN.Checked == true)
                approvedyn = ddlApprovedBy.SelectedValue;

            PhoenixDocumentManagementDocumentAdmin.DocumentRevisionReset(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ddlDocument.SelectedValue),
                General.GetNullableDateTime(txtRevisionDate.Text),
                General.GetNullableInteger(approvedyn),
                General.GetNullableInteger(ViewState["RESETALLYN"].ToString()));

            ucStatus.Text = "Revisions have been reset.";

            ddlDocument.SelectedValue = "";
            txtRevisionDate.Text = "";

            chkApprovedByYN.Checked = false;
            ddlApprovedBy.SelectedValue = "";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidReset()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlDocument.SelectedValue) == null)
            ucError.ErrorMessage = "Document is required.";


        if (General.GetNullableDateTime(txtRevisionDate.Text) == null)
            ucError.ErrorMessage = "Revision date is required.";
        else
        {
            if (DateTime.Parse(txtRevisionDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Revision date should not be the future date.";
        }


        if (chkApprovedByYN.Checked == true)
        {
            if (General.GetNullableInteger(ddlApprovedBy.SelectedValue) == null)
                ucError.ErrorMessage = "Approved by is required.";
        }

        return (!ucError.IsError);
    }

    protected void ddlDocument_Changed(object sender, EventArgs e)
    {
        DocumentEdit(ddlDocument.SelectedValue);
    }

    protected void chkApprovedByYN_Changed(object sender, EventArgs e)
    {
        if (chkApprovedByYN.Checked == true)
        {
            ddlApprovedBy.Enabled = true;
            ddlApprovedBy.CssClass = "dropdown_mandatory";
        }
        else
        {
            ddlApprovedBy.Enabled = false;
            ddlApprovedBy.CssClass = "readonlytextbox";
        }
    }
}

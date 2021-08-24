using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsInvoiceQueryQuestions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInvoiceQuestion.AccessRights = this.ViewState;
            MenuInvoiceQuestion.Title = "Invoice Query";
            MenuInvoiceQuestion.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SelectedReasons"] = "";
                ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
                BindReasons();
                BindInvoicePic();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindInvoicePic()
    {
        DataSet ds = PhoenixAccountsInvoiceQuery.InvoiceStatusPicList(new Guid(ViewState["invoicecode"].ToString()));
        ddlInvoicePIC.DataSource = ds;
        ddlInvoicePIC.DataTextField = "FLDUSERNAME";
        ddlInvoicePIC.DataValueField = "FLDUSERCODE";
        ddlInvoicePIC.DataBind();
        ddlInvoicePIC.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }

    private void BindReasons()
    {
        DataSet ds = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148);
        chkReasons.DataSource = ds;
        chkReasons.DataTextField = "FLDQUICKNAME";
        chkReasons.DataValueField = "FLDQUICKCODE";
        chkReasons.DataBind();
        chkReasons.Items.Add(new ListItem("Others(Remarks Mandatory)", "0"));
    }


    protected void MenuInvoiceQuestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                ViewState["SelectedReasons"] = "";
                foreach (ListItem listItem in chkReasons.Items)
                {
                    if (listItem.Selected)
                    {
                        ViewState["SelectedReasons"] = ViewState["SelectedReasons"].ToString() + listItem.Value.ToString() + ",";
                    }
                }
                if (!IsValidData(ViewState["SelectedReasons"].ToString(), ddlInvoicePIC.SelectedValue.Equals("") ? txtuserid.Text : ddlInvoicePIC.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoiceQuery.InvoiceQueryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["invoicecode"].ToString())
                                                                , "Q"
                                                                , txtQuestion.Text
                                                                , ViewState["SelectedReasons"].ToString()
                                                                , General.GetNullableInteger(ddlInvoicePIC.SelectedValue.Equals("") ? txtuserid.Text : ddlInvoicePIC.SelectedValue));
                ucStatus.Text = "Question Inserted.";
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string reasons, string pic)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (reasons.Trim().Equals(""))
            ucError.ErrorMessage = "Reasons is required.";
        if (pic.Trim().Equals(""))
            ucError.ErrorMessage = "Invoice PIC is required.";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void ddlInvoicePIC_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoicePIC.SelectedValue != "")
        {
            txtMentorName.Enabled = false;
            txtMentorName.CssClass = "readonlytextbox";
            imguser.Enabled = false;
        }
        else
        {
            txtMentorName.Enabled = true;
            txtMentorName.CssClass = "input_mandatory";
            imguser.Enabled = true;
        }
    }

    protected void txtMentorName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtMentorName.Text != "")
        {
            ddlInvoicePIC.Enabled = false;
            ddlInvoicePIC.CssClass = "readonlytextbox";
        }
        else
        {
            ddlInvoicePIC.Enabled = true;
            ddlInvoicePIC.CssClass = "input_mandatory";
        }
    }

}

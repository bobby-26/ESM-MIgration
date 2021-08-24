using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsCashOutPaidDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Post", "POST",ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["cashpaymentid"] = "";
                if ((Request.QueryString["cashpaymentid"] != null) && (Request.QueryString["cashpaymentid"] != ""))
                {
                    ViewState["cashpaymentid"] = Request.QueryString["cashpaymentid"].ToString();
                    BindHeader(ViewState["cashpaymentid"].ToString());
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindHeader(string cashpaymentid)
    {
        DataSet ds = PhoenixAccountsCashOut.CashOutEdit(cashpaymentid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtPaidDate.Text = dr["FLDPAYMENTDATE"].ToString();
        }
    }
    private void BindData()
    {
       
        DataSet ds = new DataSet();
      
        {
            ds = PhoenixAccountsCashOut.CashOutgetDraft(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["cashpaymentid"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvLineItem.DataSource = ds.Tables[0];
                gvLineItem.DataBind();


            }
           
        }
    }
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidCashOut(txtPaidDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["cashpaymentid"].ToString() != "")
                {
                    //PhoenixAccountsCashOut.CashOutPaidUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, new Guid(ViewState["cashpaymentid"].ToString()), DateTime.Parse(txtPaidDate.Text));
                    PhoenixAccountsCashOut.CashOutPostUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, new Guid(ViewState["cashpaymentid"].ToString()),General.GetNullableDateTime(txtPaidDate.Text));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null);", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    void Reset()
    {
        txtPaidDate.Text = "";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCashOut(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Date is required.";
        }

        return (!ucError.IsError);
    }
}

using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class Accounts_AccountAdvancePaymentApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                dgAdvancePaymentApproval.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UserName_Changed(object sender, EventArgs e)
    {

        Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsAdvancePayment.AdvancePaymentApprovalLimitSearch(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , dgAdvancePaymentApproval.CurrentPageIndex+1
                                                           , dgAdvancePaymentApproval.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , General.GetNullableInteger(ddlUserAdd.SelectedUser));

        dgAdvancePaymentApproval.DataSource = ds;
        dgAdvancePaymentApproval.VirtualItemCount = iRowCount;



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void dgAdvancePaymentApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgAdvancePaymentApproval.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgAdvancePaymentApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT") return;
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName == "EDIT")
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string Advance = ((UserControlDecimal)e.Item.FindControl("ucNumberAmount")).Text;
                if (!IsValidData(Advance))
                {
                    ucError.Visible = true;
                    return;
                }
                string Amount = ((RadLabel)e.Item.FindControl("lblApprovalIdEdit")).Text;
               
                if (Amount == "")
                {
                    PhoenixAccountsAdvancePayment.AdvancePaymentApprovalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(((RadLabel)e.Item.FindControl("lblUserIdEdit")).Text), decimal.Parse(((UserControlDecimal)e.Item.FindControl("ucNumberAmount")).Text));

                }
                else
                {
                    PhoenixAccountsAdvancePayment.AdvancePaymentApprovalUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(((RadLabel)e.Item.FindControl("lblApprovalIdEdit")).Text), decimal.Parse(((UserControlDecimal)e.Item.FindControl("ucNumberAmount")).Text));
                    ucStatus.Text = "Updated Successfully.";
                    //BindData();
                    dgAdvancePaymentApproval.Rebind();
                }

            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(String Advance)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Advance.Trim().Equals(""))
            ucError.ErrorMessage = "Advance Amount is required";
        
        return (!ucError.IsError);
    }
    private void Rebind()
    {
        dgAdvancePaymentApproval.EditIndexes.Clear();
        dgAdvancePaymentApproval.SelectedIndexes.Clear();
        dgAdvancePaymentApproval.DataSource = null;
        dgAdvancePaymentApproval.Rebind();
    }
    protected void dgAdvancePaymentApproval_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        try
        {
            if (e.Item is GridDataItem)
            {
                ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsVesselVisitTravelClaimAddGST : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            if (Request.QueryString["VisitId"] != "")
                ViewState["VisitId"] = Request.QueryString["visitId"];
            else
                ViewState["VisitId"] = null;
            if (Request.QueryString["ClaimId"] != "")
                ViewState["TravelClaimId"] = Request.QueryString["ClaimId"];
            else
                ViewState["TravelClaimId"] = null;

            ViewState["PAGENUMBERLINE"] = 1;
            gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        //  BindLineItem();
    }

    public void BindLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemListForGST(new Guid(ViewState["VisitId"].ToString()), General.GetNullableGuid(ViewState["TravelClaimId"].ToString()),
                    (int)ViewState["PAGENUMBERLINE"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTLINE"] = iRowCount;
            ViewState["TOTALPAGECOUNTLINE"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblRejected = (RadLabel)e.Item.FindControl("lblRejected");
                RadLabel lblMarkup = (RadLabel)e.Item.FindControl("lblMarkup");
                ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
                if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                if (lblRejected != null && lblRejected.Text == "1")
                {
                    e.Item.Font.Strikeout = true;
                    if (cmdEdit != null) cmdEdit.Visible = false;
                }
            }
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblRejected = (RadLabel)e.Item.FindControl("lblRejected");
                RadLabel lblCurrencyId = (RadLabel)e.Item.FindControl("lblCurrencyId");
                if (lblCurrencyId != null && lblCurrencyId.Text != "")
                {
                    UserControlCurrency uc = ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit"));
                    uc.SelectedCurrency = lblCurrencyId.Text.ToString();
                }

                ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
                if (ib1 != null && ViewState["VESSELID"] != null)
                {
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                    ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
                }

                TextBox tb = (TextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
                tb = (TextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
                tb = (TextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

                TextBox txtBudgetIdEdit = (TextBox)e.Item.FindControl("txtBudgetIdEdit");
                ImageButton imgShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("imgShowOwnerBudgetEdit");
                if (imgShowOwnerBudgetEdit != null && txtBudgetIdEdit != null)
                {
                    imgShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + Convert.ToString(ViewState["PRINCIPAL"]) + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");
                    imgShowOwnerBudgetEdit.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudgetEdit.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindLineItem();
    }
    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemUpdateForGST(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(((RadLabel)e.Item.FindControl("lblClaimLineitemId")).Text)
                     , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtGSTAmountEdit")).Text));

                ucStatus.Text = "Travel claim line item updated";
                BindLineItem();
            }

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
        BindLineItem();
    }
}

using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class PurcahsePendingApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurcahsePendingApproval.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPendingApproval')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Purchase/PurcahsePendingApproval.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuPendingApproval.AccessRights = this.ViewState;
            MenuPendingApproval.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucVessel.bind();
                ucVessel.DataBind();
   
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() != "0")
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

                VesselConfiguration();
                gvPendingApproval.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPendingApproval_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPendingApproval.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDFORMNO", "FLDVESSELNAME", "FLDNAME", "FLDQUOTEDPRICE"};
            string[] alCaptions = { "Number", "Vessel Name", "Vendor", "Quoted Price"};
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixPurchaseQuotation.PendingApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableString(txtnumber.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Pending Approval", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNO", "FLDVESSELNAME", "FLDNAME", "FLDQUOTEDPRICE" };
        string[] alCaptions = { "Number", "Vessel Name", "Vendor", "Quoted Price" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixPurchaseQuotation.PendingApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucVessel.SelectedVessel),General.GetNullableString(txtnumber.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvPendingApproval.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvPendingApproval", "Pending Approval", alCaptions, alColumns, ds);

        gvPendingApproval.DataSource = ds;
        gvPendingApproval.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["orderid"] == null)
            {
                ViewState["orderid"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
            }
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    private void BindPageURL(int rowindex)
    {
        try
        {

            ViewState["orderid"] = ((Label)gvPendingApproval.Items[rowindex].FindControl("lblorderid")).Text;
            Filter.CurrentPurchaseVesselSelection = int.Parse(((Label)gvPendingApproval.Items[rowindex].FindControl("lblVesselid")).Text);
            PhoenixPurchaseOrderForm.FormNumber = ((Label)gvPendingApproval.Items[rowindex].FindControl("lblFormNumberName")).Text;
            Filter.CurrentPurchaseStockType = ((Label)gvPendingApproval.Items[rowindex].FindControl("lblStockType")).Text;

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtNumber", PhoenixPurchaseOrderForm.FormNumber.ToString());
            criteria.Add("ucVessel", ((Label)gvPendingApproval.Items[rowindex].FindControl("lblVesselid")).Text);
            Filter.CurrentOrderFormFilterCriteria = criteria;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["orderid"] = null;
        BindData();
        gvPendingApproval.Rebind();
    }
    protected string PrepareApprovalText(DataTable dt, int approved)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        if (approved == 1)
            sbemailbody.AppendLine("Purchase approval is cancelled.");
        else
            sbemailbody.AppendLine("Purchase order is approved");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDAPPROVEDBY"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of Executive Ship Management Pte Ltd.");
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }
    private void UpdateBudgetCode(Guid orderid, int vesselid, string budgetid, Guid? ownerbudgetid)
    {
        PhoenixPurchaseQuotation.UpdateBudgetDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , orderid
                                                                , vesselid
                                                                , General.GetNullableInteger(budgetid)
                                                                , ownerbudgetid
                                                                );
    }


    
    protected void gvPendingApproval_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPendingApproval.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvPendingApproval_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
        }
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlBudgetCode ucBudget = (UserControlBudgetCode)e.Item.FindControl("ucBudgetCodeEdit");
            if (ucBudget != null)
            {
                ucBudget.BudgetCodeList = PhoenixRegistersBudget.ListBudget();
                ucBudget.DataBind();
                ucBudget.SelectedBudgetSubAccount = drv["FLDBUDGETCODE"].ToString();
            }
            Label vesselid = (Label)e.Item.FindControl("lblVesselid");
            UserControlOwnerBudgetCode ucOwnerBudget = (UserControlOwnerBudgetCode)e.Item.FindControl("ucOwnerBudgetCodeEdit");
            if (ucOwnerBudget != null)
            {
                ucOwnerBudget.BudgetCodeList = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger("757"), General.GetNullableInteger(vesselid.Text), General.GetNullableInteger(ucBudget.SelectedBudgetCode));
                ucOwnerBudget.DataBind();

                ucOwnerBudget.SelectedValue = drv["FLDOWNERBUDGETID"].ToString();
            }
        }

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton cmdApprove = (LinkButton)item.FindControl("cmdApprove");
            if (cmdApprove != null)
            {
                if (General.GetNullableInteger(drv["FLDISBUDGETED"].ToString()) == 1)
                    cmdApprove.Attributes.Add("onclick", "openNewWindow('approval', '', '" + Session["sitepath"] + "/purchase/PurchaseRemainingBudget.aspx?quotationid=" + drv["FLDQUOTATIONID"].ToString() + "&type=" + drv["FLDQUOTATIONAPPROVAL"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&currentorder=" + drv["FLDQUOTEDPRICE"].ToString() + "&directapprovalyn=Y&stocktype=" + drv["FLDSTOCKTYPE"] + "&vesselid=" + drv["FLDVESSELID"] + "&formno=" + drv["FLDFORMNO"] + "');return false;");

                cmdApprove.Visible = General.GetNullableInteger(drv["FLDFORMBUDGETCODE"].ToString()) != null ? SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName) : false;
                cmdApprove.Width = 16;
                cmdApprove.Height = 16;

            }
            TextBox tb1 = (TextBox)item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");

            TextBox txtBudgetIdEdit = (TextBox)item.FindControl("txtBudgetIdEdit");
            if (txtBudgetIdEdit != null)
                txtBudgetIdEdit.Attributes.Add("style", "display:none");

            tb1 = (TextBox)item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            ImageButton ib1 = (ImageButton)item.FindControl("btnShowBudgetEdit");
            Label lblVesselId = (Label)item.FindControl("lblVesselid");
            if (ib1 != null && lblVesselId != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, "BUDGETCODE"))
                    ib1.Visible = false;
            }
            tb1 = (TextBox)item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (TextBox)item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (TextBox)item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            ImageButton ib2 = (ImageButton)item.FindControl("btnShowOwnerBudgetEdit");
            if (ib2 != null && txtBudgetIdEdit != null)
            {
                ib2.Visible = SessionUtil.CanAccess(this.ViewState, "OWNERBUDGETCODE");
                ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");
            }
        }
    }

    protected void gvPendingApproval_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            string orderid = ((Label)item.FindControl("lblorderid")).Text;
            string StockType = ((Label)item.FindControl("lblStockType")).Text;
            string StockClass = ((Label)item.FindControl("lblStockId")).Text;
            string FormNo = ((Label)item.FindControl("lblFormNumberName")).Text;
            string quotationid = ((Label)item.FindControl("lblQuotationid")).Text;
            string vesselid = ((Label)e.Item.FindControl("lblVesselid")).Text;

            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                BindPageURL(item.ItemIndex);
                Filter.CurrentPurchaseFormNumberSelection = FormNo;
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + orderid +
                    "&VesselId=" + vesselid + "&StockType=" + StockType + "&StockClass=" + StockClass + "&FormNo=" + FormNo + "');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                Filter.CurrentPurchaseStockType = StockType;
                BindPageURL(item.ItemIndex);
                try
                {


                    PhoenixCommonPurchase.UpdateQuotationApproval(new Guid(quotationid), 0);
                    ucStatus.Text = "Approved";
                    ucStatus.Visible = true;
                    if (Session["POQAPPROVE"] != null && ((DataSet)Session["POQAPPROVE"]).Tables.Count > 0)
                    {
                        DataSet ds = (DataSet)Session["POQAPPROVE"];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string emailbodytext = PrepareApprovalText(ds.Tables[0], 0);
                            DataRow dr = ds.Tables[0].Rows[0];
                            PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                null,
                                dr["FLDSUBJECT"].ToString() + "     " + dr["FLDFORMNO"].ToString(),
                                emailbodytext,
                                true,
                                System.Net.Mail.MailPriority.Normal,
                                "",
                                null,
                                null);
                        }
                        Session["POQAPPROVE"] = null;
                    }
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                BindData();
                gvPendingApproval.Rebind();
            }
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvPendingApproval_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvPendingApproval_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            GridEditableItem edit = (GridEditableItem)e.Item;

            Label lblorderid = (Label)item.FindControl("lblorderid");
            if (!string.IsNullOrEmpty(lblorderid.Text))
            {
                Label lblVesselId = ((Label)item.FindControl("lblVesselid"));
                int vesselid = int.Parse(lblVesselId.Text);
                
                UpdateBudgetCode(new Guid(lblorderid.Text)
                                    , vesselid
                                    , ((UserControlBudgetCode)edit.FindControl("ucBudgetCodeEdit")).SelectedBudgetCode
                                    , General.GetNullableGuid(((UserControlOwnerBudgetCode)edit.FindControl("ucOwnerBudgetCodeEdit")).SelectedBudgetCode)
                                    );
            }
            BindData();
            gvPendingApproval.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucBudgetCode_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlBudgetCode ucBudget = (UserControlBudgetCode)sender;
        GridEditableItem row = (GridEditableItem)ucBudget.NamingContainer;
        if (row != null)
        {
            UserControlOwnerBudgetCode ddl = (UserControlOwnerBudgetCode)row.FindControl("ucOwnerBudgetCodeEdit");
            Label vesselid = (Label)row.FindControl("lblVesselid");
            if (General.GetNullableGuid(ddl.SelectedBudgetCode) != null)
                ViewState["OwnerBudgetId"] = ddl.SelectedBudgetCode;
            ddl.BudgetCodeList = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger("757"), General.GetNullableInteger(vesselid.Text), General.GetNullableInteger(ucBudget.SelectedBudgetCode));
            ddl.DataBind();

            if (ViewState["OwnerBudgetId"] != null && General.GetNullableGuid(ViewState["OwnerBudgetId"].ToString()) != null)
                ddl.SelectedBudgetCode = ViewState["OwnerBudgetId"].ToString();
        }
    }
}

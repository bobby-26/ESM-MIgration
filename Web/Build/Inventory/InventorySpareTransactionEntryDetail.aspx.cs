using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InventorySpareTransactionEntryDetail : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem item in gvSpareEntryDetail.Items)
        {
            if (item is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (item.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (item.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH");
            toolbarmain.AddButton("General", "GENERAL");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarmain.AddButton("Vendors", "VENDORS");
            }
            toolbarmain.AddButton("Transaction", "STOCKTRANSACTION");
            toolbarmain.AddButton("Components", "COMPONENTS");
            toolbarmain.AddButton("Details", "DETAILS");
            toolbarmain.AddButton("Attachment", "ATTACHMENT");

            MenuInventoryStockInOut.AccessRights = this.ViewState; 
            MenuInventoryStockInOut.MenuList = toolbarmain.Show();
            MenuInventoryStockInOut.SelectedMenuIndex = 3 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);

            txtComponentID.Attributes.Add("style", "display:none");
            txtItemId.Attributes.Add("style", "display:none");
            txtLocationID.Attributes.Add("style", "display:none");

            txtOrderName.Attributes.Add("style", "display:none");
            txtOrderId.Attributes.Add("style", "display:none");

            txtOrderNumber.Attributes.Add("onkeydown", "return false;");
            txtOrderName.Attributes.Add("onkeydown", "return false;");
            txtOrderId.Attributes.Add("onkeydown", "return false;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareTransactionEntryDetail.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareEntryDetail')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuGridSpareInOut.AccessRights = this.ViewState;  
            MenuGridSpareInOut.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvSpareEntryDetail.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SPAREITEMDISPOSITIONID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                txtDispositionDate.Text = General.GetDateTimeToString(System.DateTime.Now.ToString());
                ddlDispositionType.HardTypeCode = ((int)PhoenixHardTypeCode.TRANSACTIONTYPE).ToString();
                ddlWorkOrder.HardTypeCode = ((int)PhoenixHardTypeCode.WORKORDERTYPE).ToString();

                if ((Request.QueryString["SPAREITEMID"] != null) && (Request.QueryString["SPAREITEMID"] != ""))
                {
                    if (ViewState["SPAREITEMDISPOSITIONID"] != null)
                        SpareItemTransactionDetailList(Request.QueryString["SPAREITEMID"].ToString(), ViewState["SPAREITEMDISPOSITIONID"].ToString());
                    else
                        SpareItemTransactionDetailList(Request.QueryString["SPAREITEMID"].ToString(), "");
                } 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        UpdateSpareItemTransactionEntryDetails();
        gvSpareEntryDetail.Rebind();
    }


    protected void MenuInventoryStockInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inventory/InventorySpareItemFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("InventorySpareItem.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"].ToString()+ "&SETCURRENTNAVIGATIONTAB=../Inventory/InventorySpareItemGeneral.aspx?SPAREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("VENDORS"))
            {
                Response.Redirect("InventorySpareItem.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Inventory/InventorySpareItemVendor.aspx?SPAREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("COMPONENTS"))
            {
                Response.Redirect("InventorySpareItem.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Inventory/InventorySpareItemComponent.aspx?SPAREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("InventorySpareItem.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Inventory/InventorySpareItemDetail.aspx?SPAREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("InventorySpareItem.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Common/CommonFileAttachment.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void UpdateSpareItemTransactionEntryDetails()
    {
        try
        {
            int iMessageCode = 0;
            string iMessageText = "";


            PhoenixInventorySpareItemTransaction.UpdateSpareItemTransactionEntryDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["SPAREITEMDISPOSITIONHEADERID"].ToString()),
                new Guid(ViewState["SPAREITEMDISPOSITIONID"].ToString()), new Guid(ViewState["SPAREITEMID"].ToString()), Convert.ToDecimal(txtDispositionQuantity.Text),
                Convert.ToInt32(ddlDispositionType.SelectedHard), Convert.ToDateTime(txtDispositionDate.Text),
                General.GetNullableInteger(ddlWorkOrder.SelectedHard), txtDispositionReference.Text, txtOrderId.Text, ucConfirm.confirmboxvalue,
                ref  iMessageCode, ref iMessageText);

            if (iMessageCode == 1)
                throw new ApplicationException(iMessageText);


        }
        catch (ApplicationException aex)
        {
            ucConfirm.HeaderMessage = "Please Confirm";
            ucConfirm.ErrorMessage = aex.Message;
            ucConfirm.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGridSpareInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
            string[] alColumns = { "FLDDISPOSITIODATE",  "TRANSACTIONTYPENAME", "FLDNAME",  "FLDDISPOSITIONQUANTITY", "FLDROB", "FLDPURCHASEPRICE", "WORKORDERTYPENAME", "REPORTEDBY" };
            string[] alCaptions = { "Transaction Date",  "Transaction Type",  "Item name",  "Quantity", "ROB", "Purchase Price", "Work order", "Reported By" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            //else
                //iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string dispositionheaderid = "";

            string spareitemid = (Request.QueryString["SPAREITEMID"] == null) ? null : (Request.QueryString["SPAREITEMID"].ToString());

            if (Filter.CurrentSpareItemDispositionHeaderId != null)
            {
                NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;
                dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();
            }
            DataSet ds = PhoenixInventorySpareItemTransaction.SpareItemDispositionTransactionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                         null, spareitemid, null, null,
                        null, null,
                        sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);
            General.ShowExcel("Inventory Spare Item Transaction Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SpareItemTransactionDetailList(string spareitemid, string spareitemdispositionid)
    {
        DataSet ds = PhoenixInventorySpareItemTransaction.SpareItemTransactionDetailList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
          new Guid(spareitemid), spareitemdispositionid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtItemNumber.Text = dr["FLDNUMBER"].ToString();
            txtItemName.Text = dr["FLDNAME"].ToString();
            txtItemId.Text = dr["FLDSPAREITEMID"].ToString();
            txtComponentNumber.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponentID.Text = dr["FLDCOMPONENTID"].ToString();
            txtLocationCode.Text = dr["FLDLOCATIONCODE"].ToString();
            txtLocationName.Text = dr["FLDLOCATIONNAME"].ToString();
            txtLocationID.Text = dr["FLDLOCATIONID"].ToString();
            //ddlWorkOrder.SelectedHard = dr["FLDWORKORDERID"].ToString();
            txtDispositionDate.Text = General.GetDateTimeToString(dr["FLDDISPOSITIODATE"].ToString());
            txtDispositionReference.Text = dr["FLDDISPOSITIOREFERENCE"].ToString();
            ddlDispositionType.SelectedHard = dr["FLDDISPOSITIOTYPE"].ToString();
            txtReportedBy.Text = dr["FLDUSERNAME"].ToString();
            txtDispositionQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDDISPOSITIONQUANTITY"]));
            txtOrderNumber.Text = dr["FLDFORMNO"].ToString();
            txtOrderName.Text = dr["FLDTITLE"].ToString();
            txtOrderId.Text = dr["FLDORDERID"].ToString();
            ViewState["SPAREITEMID"] = dr["FLDSPAREITEMID"].ToString();
            ViewState["SPAREITEMDISPOSITIONID"] = dr["FLDSPAREITEMDISPOSITIONID"].ToString();
            ViewState["SPAREITEMDISPOSITIONHEADERID"] = dr["FLDSPAREITEMDISPOSITIONHEADERID"].ToString();
        }
    }
    private void DeleteSpareItemDisposition(string spareitemid, string spareitemdispositionheaderid, string spareitemdispositionid, string spareitemdispositionquantity)
    {
        try
        {
            PhoenixInventorySpareItemTransaction.SpareItemTransactionEntryDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , new Guid(spareitemid)
                , new Guid(spareitemdispositionheaderid)
                , new Guid(spareitemdispositionid)
                , Convert.ToDecimal(spareitemdispositionquantity));
            gvSpareEntryDetail.Rebind();
            SpareItemTransactionDetailList(ViewState["SPAREITEMID"].ToString(), null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvSpareEntryDetail.Rebind();
            SpareItemTransactionDetailList(Request.QueryString["SPAREITEMID"].ToString(), "");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSpareEntryDetail_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            if (e.CommandName == "RowClick")
            {
                ViewState["SPAREITEMID"] = ((RadLabel)item.FindControl("lblSpareItemId")).Text;
                ViewState["SPAREITEMDISPOSITIONID"] = ((RadLabel)item.FindControl("lblSpareItemDispositionId")).Text;
                ViewState["SPAREITEMDISPOSITIONHEADERID"] = ((RadLabel)item.FindControl("lblSpareItemDispositionHeaderId")).Text;
                SpareItemTransactionDetailList(ViewState["SPAREITEMID"].ToString(), ViewState["SPAREITEMDISPOSITIONID"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSpareItemDisposition(((RadLabel)item.FindControl("lblSpareItemId")).Text
                    , ((RadLabel)item.FindControl("lblSpareItemDispositionHeaderId")).Text
                    , ((RadLabel)item.FindControl("lblSpareItemDispositionId")).Text
                    , ((RadLabel)item.FindControl("lblDispositionQuantity")).Text);
                BindData();
                gvSpareEntryDetail.Rebind();
            }
        }
    }

    protected void gvSpareEntryDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDDISPOSITIODATE", "TRANSACTIONTYPENAME", "FLDNAME", "FLDDISPOSITIONQUANTITY", "FLDROB", "FLDPURCHASEPRICE", "WORKORDERTYPENAME", "REPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Transaction Type", "Item name", "Quantity", "ROB", "Purchase Price", "Work order", "Reported By" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string spareitemid = (Request.QueryString["SPAREITEMID"] == null) ? null : (Request.QueryString["SPAREITEMID"].ToString());
            DataSet ds = PhoenixInventorySpareItemTransaction.SpareItemDispositionTransactionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                          null, spareitemid, null, null,
                          null, null,
                          sortexpression, sortdirection,
                          gvSpareEntryDetail.CurrentPageIndex + 1,
                          gvSpareEntryDetail.PageSize,
                          ref iRowCount,
                          ref iTotalPageCount);

            General.SetPrintOptions("gvSpareEntryDetail", "Inventory Stock Item Transaction Entry", alCaptions, alColumns, ds);

            gvSpareEntryDetail.DataSource = ds;
            gvSpareEntryDetail.VirtualItemCount = iRowCount;

            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareTransactionEntryDetail.aspx?SPAREITEMID=";
            }

            SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemFilter.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemGeneral.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemVendor.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 2;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareTransactionEntryDetail.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 3 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemComponent.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 4 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemDetail.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 5 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 6 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSpareEntryDetail_SortCommand(object sender, GridSortCommandEventArgs e)
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
}

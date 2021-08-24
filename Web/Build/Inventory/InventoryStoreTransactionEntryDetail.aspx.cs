using System;
using System.Web.UI;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreTransactionEntryDetail : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem item in gvStoreEntryDetail.Items)
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
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            SessionUtil.PageAccessRights(this.ViewState);
            toolbarmain.AddButton("Search", "SEARCH");
            toolbarmain.AddButton("General", "GENERAL");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarmain.AddButton("Vendors", "VENDORS");
            }
            toolbarmain.AddButton("Transaction", "STOCKTRANSACTION");
            toolbarmain.AddButton("Details", "DETAILS");
            toolbarmain.AddButton("Attachment", "ATTACHMENT");

            MenuInventoryStockInOut.AccessRights = this.ViewState;  
            MenuInventoryStockInOut.MenuList = toolbarmain.Show();
            MenuInventoryStockInOut.SelectedMenuIndex = 3 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
         
            txtItemId.Attributes.Add("style", "display:none");
            txtLocationID.Attributes.Add("style", "display:none");

            txtOrderName.Attributes.Add("style", "display:none");
            txtOrderId.Attributes.Add("style", "display:none");

            txtOrderNumber.Attributes.Add("onkeydown", "return false;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreTransactionEntryDetail.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreEntryDetail')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuGridStoreInOut.AccessRights = this.ViewState;  
            MenuGridStoreInOut.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvStoreEntryDetail.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["STOREITEMDISPOSITIONID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                txtDispositionDate.Text = General.GetDateTimeToString(System.DateTime.Now.ToString());
                ddlDispositionType.HardTypeCode = ((int)PhoenixHardTypeCode.TRANSACTIONTYPE).ToString();
                ddlWorkOrder.HardTypeCode = ((int)PhoenixHardTypeCode.WORKORDERTYPE).ToString();

                if ((Request.QueryString["STOREITEMID"] != null) && (Request.QueryString["STOREITEMID"] != ""))
                {
                    if (ViewState["STOREITEMDISPOSITIONID"] != null)
                        StoreItemTransactionDetailList(Request.QueryString["STOREITEMID"].ToString(), ViewState["STOREITEMDISPOSITIONID"].ToString());
                    else
                        StoreItemTransactionDetailList(Request.QueryString["STOREITEMID"].ToString(), "");
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
        UpdateStoreItemTransactionEntryDetails();
        gvStoreEntryDetail.Rebind();
    }

    protected void MenuInventoryStockInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inventory/InventoryStoreItemFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["StoreITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Inventory/InventoryStoreItemGeneral.aspx?STOREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("VENDORS"))
            {
                Response.Redirect("InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["StoreITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Inventory/InventoryStoreItemVendor.aspx?STOREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["StoreITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Inventory/InventoryStoreItemDetail.aspx?STOREITEMID=");
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["StoreITEMID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../Common/CommonFileAttachment.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void UpdateStoreItemTransactionEntryDetails()
    {
        try
        {
            int iMessageCode = 0;
            string iMessageText = "";


            PhoenixInventoryStoreItemTransaction.UpdateStoreItemTransactionEntryDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["STOREITEMDISPOSITIONHEADERID"].ToString()),
                new Guid(ViewState["STOREITEMDISPOSITIONID"].ToString()), new Guid(ViewState["STOREITEMID"].ToString()),
                Convert.ToDecimal(txtDispositionQuantity.Text),
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

    protected void MenuGridStoreInOut_TabStripCommand(object sender, EventArgs e)
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
            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "TRANSACTIONTYPENAME", "FLDDISPOSITIONQUANTITY", "FLDFORMNUMBER", "REPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Number", "Item name", "Transaction type", "Quantity", "Order Number", "Reported By" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string dispositionheaderid = "";


            string storeitemid = (Request.QueryString["STOREITEMID"] == null) ? null : (Request.QueryString["STOREITEMID"].ToString());

            if (Filter.CurrentStoreItemDispositionHeaderId != null)
            {
                NameValueCollection nvc = Filter.CurrentStoreItemDispositionHeaderId;
                dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();
            }
            DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemDispositionTransactionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                         null, storeitemid, null, null,
                        null, null,
                        sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);
            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemTransactionDetails.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory Store Item Transaction Details</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void StoreItemTransactionDetailList(string storeitemid, string storeitemdispositionid)
    {
        DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemTransactionDetailList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
          new Guid(storeitemid), storeitemdispositionid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtItemNumber.Text = dr["FLDNUMBER"].ToString();
            txtItemName.Text = dr["FLDNAME"].ToString();
            txtItemId.Text = dr["FLDSTOREITEMID"].ToString();           
            txtLocationCode.Text = dr["FLDLOCATIONCODE"].ToString();
            txtLocationName.Text = dr["FLDLOCATIONNAME"].ToString();
            txtLocationID.Text = dr["FLDLOCATIONID"].ToString();
            ddlWorkOrder.SelectedHard = dr["FLDWORKORDERID"].ToString();
            txtDispositionDate.Text = General.GetDateTimeToString(dr["FLDDISPOSITIODATE"].ToString());
            txtDispositionReference.Text = dr["FLDDISPOSITIOREFERENCE"].ToString();
            ddlDispositionType.SelectedHard = dr["FLDDISPOSITIOTYPE"].ToString();
            txtReportedBy.Text = dr["FLDUSERNAME"].ToString();
            txtDispositionQuantity.Text = string.Format(String.Format("{0:#####.00}", dr["FLDDISPOSITIONQUANTITY"]));
            txtOrderNumber.Text = dr["FLDFORMNO"].ToString();
            txtOrderName.Text = dr["FLDTITLE"].ToString();
            txtOrderId.Text = dr["FLDORDERID"].ToString();
            ViewState["STOREITEMID"] = dr["FLDSTOREITEMID"].ToString();
            ViewState["STOREITEMDISPOSITIONID"] = dr["FLDSTOREITEMDISPOSITIONID"].ToString();
            ViewState["STOREITEMDISPOSITIONHEADERID"] = dr["FLDSTOREITEMDISPOSITIONHEADERID"].ToString();
        }
    }

    private void DeleteStoreItemDisposition(string storeitemid,string storeitemdispositionheaderid,string storeitemdispositionid, string storeitemdispositionquantity)
    {
        try
        {
            PhoenixInventoryStoreItemTransaction.StoreItemTransactionEntryDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , new Guid(storeitemid)
                , new Guid(storeitemdispositionheaderid)  
                , new Guid(storeitemdispositionid)  
                , Convert.ToDecimal(storeitemdispositionquantity));
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
            gvStoreEntryDetail.Rebind();
            StoreItemTransactionDetailList(Request.QueryString["STOREITEMID"].ToString(), "");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreEntryDetail_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["STOREITEMID"] = ((RadLabel)item.FindControl("lblStoreItemId")).Text;
                ViewState["STOREITEMDISPOSITIONID"] = ((RadLabel)item.FindControl("lblStoreItemDispositionId")).Text;
                ViewState["STOREITEMDISPOSITIONHEADERID"] = ((RadLabel)item.FindControl("lblStoreItemDispositionHeaderId")).Text;
                StoreItemTransactionDetailList(ViewState["STOREITEMID"].ToString(), ViewState["STOREITEMDISPOSITIONID"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStoreItemDisposition(((RadLabel)item.FindControl("lblStoreItemId")).Text
                    , ((RadLabel)item.FindControl("lblStoreItemDispositionHeaderId")).Text
                    , ((RadLabel)item.FindControl("lblStoreItemDispositionId")).Text
                    , ((RadLabel)item.FindControl("lblDispositionQuantity")).Text);
                BindData();
                gvStoreEntryDetail.Rebind();
            }
        }
    }

    protected void gvStoreEntryDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "TRANSACTIONTYPENAME", "FLDDISPOSITIONQUANTITY", "FLDFORMNUMBER", "REPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name", "Transaction Type", "Quantity", "Order Number", "Reported By." };
            string strHeader = "Inventory Store Item Transaction Entry";

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            string storeitemid = (Request.QueryString["STOREITEMID"] == null) ? null : (Request.QueryString["STOREITEMID"].ToString());


            DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemDispositionTransactionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                           null, storeitemid, null, null,
                          null, null,
                          sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                          General.ShowRecords(null),
                          ref iRowCount,
                          ref iTotalPageCount);

            gvStoreEntryDetail.DataSource = ds;
            gvStoreEntryDetail.VirtualItemCount = iRowCount;

            General.SetPrintOptions("gvStoreEntryDetail", strHeader, alCaptions, alColumns, ds);
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemFilter.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemGeneral.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemVendor.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 2;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreTransactionEntryDetail.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 3 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemDetail.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 4 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuInventoryStockInOut.SelectedMenuIndex = 5 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreEntryDetail_SortCommand(object sender, GridSortCommandEventArgs e)
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

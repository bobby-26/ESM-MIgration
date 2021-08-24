using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InventoryStoreItemInOutTransaction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Transaction", "TRANSACTION", ToolBarDirection.Right);
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
            MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemInOutTransaction.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreItemInOutTransaction')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inventory/InventoryStoreItemTransactionFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuGridStoreItemInOutTransaction.AccessRights = this.ViewState;  
            MenuGridStoreItemInOutTransaction.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvStoreItemInOutTransaction.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["STOREITEMID"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void StoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (CommandName.ToUpper().Equals("TRANSACTION"))
            //{
            //    Response.Redirect("InventoryStoreInOut.aspx");
            //}

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvStoreItemInOutTransaction.Rebind();
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
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "LOCATIONNAME", "FLDDISPOSITIONQUANTITY", "PURCHASEDUNIT" };
            string[] alCaptions = { "Number", "Name", "Product Code", "Location Name", "Quantity", "Unit" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
          
            NameValueCollection nvc = Filter.CurrentStoreItemDispositionHeaderId;
            NameValueCollection nvcStore = Filter.CurrentStoreItemFilterCriteria;

            DataSet ds = PhoenixInventoryStoreItemDisposition.StoreItemDispositionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("DISPOSITIONHEADERID").ToString(),
                    (nvcStore == null ? "" : nvcStore.Get("txtNumber")), (nvcStore == null ? "" : nvcStore.Get("txtName")),
                    null, null, sortexpression, sortdirection,
                    1, iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);


            Response.AddHeader("Content-Disposition", "attachment; filename= StoreItemTransaction.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Store Item In Out Transaction</h3></td>");
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
    protected void MenuGridStoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvStoreItemInOutTransaction.Rebind();
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

    protected void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;
         
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "LOCATIONNAME", "FLDDISPOSITIONQUANTITY", "PURCHASEDUNIT" };
            string[] alCaptions = { "Number", "Name", "Product Code","Location Name","Quantity","Unit" };         

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
       
            NameValueCollection nvc = Filter.CurrentStoreItemDispositionHeaderId;
            NameValueCollection nvcStore = Filter.CurrentStoreItemFilterCriteria;

            DataSet ds = PhoenixInventoryStoreItemDisposition.StoreItemDispositionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("DISPOSITIONHEADERID").ToString(),
                    General.GetNullableString(string.IsNullOrEmpty(txtStoreItemNumber.Text) ? txtStoreItemNumber.Text : txtStoreItemNumber.TextWithLiterals),
                    General.GetNullableString(txtStoreItemName.Text),
                    null, null, sortexpression, sortdirection,
                   int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvStoreItemInOutTransaction.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.SetPrintOptions("gvStoreItemInOutTransaction", "Store Item In Out Transaction", alCaptions, alColumns, ds);

            gvStoreItemInOutTransaction.DataSource = ds;
            gvStoreItemInOutTransaction.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindFooterItemDetails(string storeitemid)
    {
        DataSet ds = PhoenixInventoryStoreItemDisposition.StoreItemDispositionList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                       new Guid(storeitemid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrentStockQuantity.Text = string.Format(String.Format("{0:#####.00}", dr["CURRENTQUANTITY"]));
            txtLocation.Text = string.Format(String.Format("{0:#####.00}",dr["LOCATIONNAME"]));
            txtTotalStockQuantity.Text = string.Format(String.Format("{0:#####.00}",dr["TOTALSTOCKQUANTITY"]));
            txtOnOrderQuantity.Text = "0";
        }
    }
    private bool IsValidStoreItemDispositionEdit(string dispositionquantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvStoreItemInOutTransaction;
        decimal result;

        if (dispositionquantity.Trim('.') != "")
        {
            if (decimal.TryParse(dispositionquantity, out result) == false)
                ucError.ErrorMessage = "Item wanted quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }

    private void InsertStoreItemDisposition(string storeitemid, string dispositionquantity, string storeitemlocationid)
    {
        try
        {
            if (!IsValidStoreItemDispositionEdit(dispositionquantity))
            {
                ucError.Visible = true;
                return;
            }
            NameValueCollection nvc = Filter.CurrentStoreItemDispositionHeaderId;

            int iMessageCode = 0;
            string iMessageText = "";

            PhoenixInventoryStoreItemDisposition.InsertStoreItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(storeitemlocationid),
                new Guid(nvc.Get("DISPOSITIONHEADERID").ToString()),
                new Guid(storeitemid),
                General.GetNullableDecimal(dispositionquantity), null, "",
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText
                );

            if (iMessageCode == 1)
                throw new ApplicationException(iMessageText);

            gvStoreItemInOutTransaction.Rebind();

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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvStoreItemInOutTransaction.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        InsertStoreItemDisposition(ViewState["LBLSTOREITEMID"].ToString(), ViewState["DISPOSITIONQUANTITY"].ToString(), ViewState["STOREITEMLOCATIONID"].ToString());
        BindData();
        gvStoreItemInOutTransaction.Rebind();
    }

    protected void gvStoreItemInOutTransaction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStoreItemInOutTransaction.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvStoreItemInOutTransaction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName=="RowClick")
            {
                ViewState["STOREITEMID"] = ((RadLabel)e.Item.FindControl("lblStoreItemId")).Text;
                BindFooterItemDetails(ViewState["STOREITEMID"].ToString());
            }
            else if (e.CommandName == "Page")
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

    protected void gvStoreItemInOutTransaction_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvStoreItemInOutTransaction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdUpdate");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }
        }
    }

    protected void gvStoreItemInOutTransaction_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ViewState["LBLSTOREITEMID"] = ((RadLabel)eeditedItem.FindControl("lblStoreItemId")).Text;
            ViewState["DISPOSITIONQUANTITY"] = ((RadTextBox)eeditedItem.FindControl("txtStockItemDispositionQuantityEdit")).Text;
            ViewState["STOREITEMLOCATIONID"] = eeditedItem.GetDataKeyValue("FLDSTOREITEMLOCATIONID").ToString();

            InsertStoreItemDisposition(ViewState["LBLSTOREITEMID"].ToString(), ViewState["DISPOSITIONQUANTITY"].ToString(), ViewState["STOREITEMLOCATIONID"].ToString());
            BindData();
            gvStoreItemInOutTransaction.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

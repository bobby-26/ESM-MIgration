using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InventorySpareItemInOutTransaction : PhoenixBasePage
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
            MenuSpareItemInOutTransaction.AccessRights = this.ViewState;
            MenuSpareItemInOutTransaction.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemInOutTransaction.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareItemInOutTransaction')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '"+ Session["sitepath"] + "/Inventory/InventorySpareItemTransactionFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuGridSpareItemInOutTransaction.AccessRights = this.ViewState;
            MenuGridSpareItemInOutTransaction.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvSpareItemInOutTransaction.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SPAREITEMID"] = null;
                ViewState["ComponentId"] = "";

                if (Request.QueryString["componentid"] !=null && Request.QueryString["componentid"] !="")
                {
                    ViewState["ComponentId"] = Request.QueryString["componentid"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SpareItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (CommandName.ToUpper().Equals("TRANSACTION"))
            //{
            //    Response.Redirect("InventorySpareInOut.aspx");
            //}
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                    if (ViewState["framename"] != null)
                        Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnReloadList('codehelp1');";
                    Script += "</script>" + "\n";
                }
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSpareItemInOutTransaction.Rebind();
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
            string[] alCaptions = { "Number", "Name", "Maker Reference", "Location Name", "Quantity", "Unit" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;
            NameValueCollection nvcSpare = Filter.CurrentSpareItemFilterCriteria;

            DataSet ds = PhoenixInventorySpareItemDisposition.SpareItemDispositionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("DISPOSITIONHEADERID").ToString(),
                    General.GetNullableString(string.IsNullOrEmpty(txtSpareItemNumber.Text) ? txtSpareItemNumber.Text : txtSpareItemNumber.TextWithLiterals),
                    General.GetNullableString(txtSpareItemName.Text),
                    null, null, sortexpression, 1,
                    1, iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StockItemWanted.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Spare Item In Out Transaction</h3></td>");
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
    protected void MenuGridSpareItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSpareItemInOutTransaction.Rebind();
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
            string[] alCaptions = { "Number", "Name", "Maker Reference", "Location Name", "Quantity", "Unit" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;
            NameValueCollection nvcSpare = Filter.CurrentSpareItemFilterCriteria;

            DataSet ds = PhoenixInventorySpareItemDisposition.SpareItemDispositionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("DISPOSITIONHEADERID").ToString(),
                    General.GetNullableString(string.IsNullOrEmpty(txtSpareItemNumber.Text) ? txtSpareItemNumber.Text : txtSpareItemNumber.TextWithLiterals),
                    General.GetNullableString(txtSpareItemName.Text),
                    null, null, sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvSpareItemInOutTransaction.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                    ,General.GetNullableGuid(ViewState["ComponentId"].ToString()));

            General.SetPrintOptions("gvSpareItemInOutTransaction", "Spare Item InOut Transaction", alCaptions, alColumns, ds);

            gvSpareItemInOutTransaction.DataSource = ds;
            gvSpareItemInOutTransaction.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindFooterItemDetails(string spareitemid)
    {
        DataSet ds = PhoenixInventorySpareItemDisposition.SpareItemDispositionList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                       new Guid(spareitemid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrentStockQuantity.Text = string.Format(String.Format("{0:#######}", dr["CURRENTQUANTITY"]));
            txtLocation.Text = string.Format(String.Format("{0:#######}", dr["LOCATIONNAME"]));
            txtTotalStockQuantity.Text = string.Format(String.Format("{0:#######}", dr["TOTALSTOCKQUANTITY"]));
            txtOnOrderQuantity.Text = "0";
        }
    }
    private bool IsValidSpareItemDispositionEdit(string dispositionquantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvSpareItemInOutTransaction;
        decimal result;

        if(string.IsNullOrEmpty(dispositionquantity.Trim()) || dispositionquantity.Trim() == "0")
        {
            ucError.ErrorMessage = "Item quantity should be a valid numeric value greater than 0.";
        }

        if (dispositionquantity.Trim() != "")
        {
            if (decimal.TryParse(dispositionquantity, out result) == false)
                ucError.ErrorMessage = "Item quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }

    private void InsertSpareItemDisposition(string spareitemid, string dispositionquantity, string spareitemlocationid)
    {
        try
        {
            if (!IsValidSpareItemDispositionEdit(dispositionquantity))
            {
                ucError.Visible = true;
                return;
            }
            NameValueCollection nvc = Filter.CurrentSpareItemDispositionHeaderId;

            int iMessageCode = 0;
            string iMessageText = "";

            PhoenixInventorySpareItemDisposition.InsertSpareItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(spareitemlocationid),
                new Guid(nvc.Get("DISPOSITIONHEADERID").ToString()),
                new Guid(spareitemid),
                General.GetNullableDecimal(dispositionquantity), null, "",
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText
                );

            if (iMessageCode == 1)
                throw new ApplicationException(iMessageText);

            gvSpareItemInOutTransaction.Rebind();

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
            gvSpareItemInOutTransaction.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        InsertSpareItemDisposition(ViewState["LBLSPAREITEMID"].ToString(), ViewState["DISPOSITIONQUANTITY"].ToString() , ViewState["SPAREITEMLOCATIONID"].ToString());
        BindData();
        gvSpareItemInOutTransaction.Rebind();
    }

    protected void gvSpareItemInOutTransaction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSpareItemInOutTransaction.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvSpareItemInOutTransaction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName=="RowClick")
            {
                ViewState["SPAREITEMID"] = ((RadLabel)e.Item.FindControl("lblSpareItemId")).Text;
                BindFooterItemDetails(ViewState["SPAREITEMID"].ToString());
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

    protected void gvSpareItemInOutTransaction_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvSpareItemInOutTransaction_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvSpareItemInOutTransaction_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            ViewState["LBLSPAREITEMID"] = ((RadLabel)eeditedItem.FindControl("lblSpareItemId")).Text;
            ViewState["DISPOSITIONQUANTITY"] = ((RadTextBox)eeditedItem.FindControl("txtStockItemDispositionQuantityEdit")).Text;
            ViewState["SPAREITEMLOCATIONID"] = eeditedItem.GetDataKeyValue("FLDSPAREITEMLOCATIONID").ToString();
            InsertSpareItemDisposition(ViewState["LBLSPAREITEMID"].ToString(), ViewState["DISPOSITIONQUANTITY"].ToString(), ViewState["SPAREITEMLOCATIONID"].ToString());
            BindData();
            gvSpareItemInOutTransaction.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
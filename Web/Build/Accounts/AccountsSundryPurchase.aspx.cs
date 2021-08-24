using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsSundryPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsSundryPurchase.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvBondReq')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsSundryPurchaseFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsSundryPurchase.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
            MenuOrderForm.Title = "Sundry Purchase";
            //  MenuBondReq.SetTrigger(pnlComponent);

            if (!IsPostBack)
            {

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["ORDERID"] = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvBondReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();           
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
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDSTOCKTYPE", "FLDORDERSTATUS" };
            string[] alCaptions = { "Order No", "Order Date", "Stock Type", "Order Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentSundryPurchaseFilter;
            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchase (null 
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStockType"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Sundry Purchase", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["ORDERID"] = null;
                Filter.CurrentSundryPurchaseFilter  = null;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
          
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["CURRENTTAB"] = "../Accounts/AccountsSundryPurchaseGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                ViewState["CURRENTTAB"] = "../Accounts/AccountsSundryPurchaseLineItem.aspx";
            }
            ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?orderid=" + ViewState["ORDERID"];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDSTOCKTYPE","FLDORDERSTATUS" };
            string[] alCaptions = { "Order No", "Order Date", "Stock Type", "Order Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentSundryPurchaseFilter;
            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchase(null 
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStockType"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvBondReq", "Sundry Purchase", alCaptions, alColumns, ds);
            gvBondReq.DataSource = ds;
            gvBondReq.VirtualItemCount = iRowCount;
          
            if (ViewState["CURRENTTAB"] == null)
                ViewState["CURRENTTAB"] = "../Accounts/AccountsSundryPurchaseGeneral.aspx";


            if (ds.Tables[0].Rows.Count > 0)
            {                
                
                if (ViewState["ORDERID"] == null)
                {
                    ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                  
                }
                ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?orderid=" + ViewState["ORDERID"];
            }
            else
            {
                ViewState["ORDERID"] = null;
               
             
                ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString();
            }
            SetTabHighlight();
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
         
            ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvBondReq_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
      
        gvBondReq.SelectedIndexes.Add(se.NewSelectedIndex);
     
        string orderid = ((RadLabel)gvBondReq.Items[se.NewSelectedIndex].FindControl("lblOrderId")).Text;
        ViewState["ORDERID"] = orderid;
        BindData();
    }

    
    protected void gvBondReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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

    protected void gvBondReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBondReq.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton db = (ImageButton)e.Item.FindControl("cmdApprove");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
               // db.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','AccountsSundryPurchaseGeneral.aspx?ORDERID=" + drv["FLDORDERID"].ToString() + "&r=1'); return false;");
                db.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsSundryPurchaseGeneral.aspx?ORDERID=" + drv["FLDORDERID"].ToString() + "&r=1" +" ',false);");
      
                if (drv["FLDACTIVEYN"].ToString() == "0") db.Visible = false;


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
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


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["ORDERID"] = null;
            //BindData();
            //ResetMenu();
            //for (int i = 0; i < gvBondReq.DataKeyNames.Length; i++)
            //{
            //    if (gvBondReq.DataKeyNames[i] == ViewState["ORDERID"].ToString())
            //    {
            //        gvBondReq.SelectedIndex = i;
            //        break;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["ORDERID"] != null)
        { 
            toolbar.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
        }
        toolbar.AddButton("General", "GENERAL",ToolBarDirection.Right);
      
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbar.Show();
        MenuOrderForm.SelectedMenuIndex = 1;
    }
    protected void SetTabHighlight()
    {
        try
        {
           
            RadToolBar dl = (RadToolBar)MenuOrderForm.FindControl("dlstTabs");
          
            if (dl.Items.Count > 0)
          {
                if (ViewState["CURRENTTAB"].ToString().Trim().Contains("AccountsSundryPurchaseGeneral.aspx"))
                {
                    MenuOrderForm.SelectedMenuIndex = 1;
                }
                else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("AccountsSundryPurchaseLineItem.aspx"))
                {
                    MenuOrderForm.SelectedMenuIndex = 0;
                }                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
}

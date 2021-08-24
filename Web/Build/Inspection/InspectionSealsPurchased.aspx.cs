using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionSealsPurchased : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealsPurchased.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "OExcel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "OPRINT");
            MenuSealExport.AccessRights = this.ViewState;
            MenuSealExport.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealsPurchased.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "SExcel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealNumber')", "Print Grid", "<i class=\"fas fa-print\"></i>", "SPRINT");
            MenuSealNumberExport.AccessRights = this.ViewState;
            MenuSealNumberExport.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["LOCATIONID"] = Request.QueryString["LOCATIOND"];
                ViewState["STOREITEMID"] = Request.QueryString["STOREITEMID"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTSELECTEDORDERID"] = null;

                ViewState["PAGENUMBERSL"] = 1;
                ViewState["SORTEXPRESSIONSL"] = null;
                ViewState["SORTDIRECTIONSL"] = null;

              

                gvOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvSealNumber.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();

            //BindDataSL();
            //SetPageNavigatorSL();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("OEXCEL"))
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
            string[] alColumns = { "FLDFORMNUMBER", "FLDVENDORCODE", "FLDVENDORNAME", "FLDQUANTITY" };
            string[] alCaptions = { "Order Number", "Supplier Code", "Supplier Name", "Purchased Qty" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealPurchasedSearch(new Guid(ViewState["STOREITEMID"].ToString())
                            , new Guid(ViewState["LOCATIONID"].ToString())
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Bulk PO List", ds.Tables[0], alColumns, alCaptions, null, null);
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
            string[] alColumns = { "FLDFORMNUMBER", "FLDVENDORCODE", "FLDVENDORNAME", "FLDQUANTITY" };
            string[] alCaptions = { "Order Number", "Supplier Code", "Supplier Name", "Purchased Qty" };

            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealPurchasedSearch(new Guid(ViewState["STOREITEMID"].ToString())
                           , new Guid(ViewState["LOCATIONID"].ToString())
                           , gvOrder.CurrentPageIndex+1
                           , gvOrder.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvOrder", "Bulk PO List", alCaptions, alColumns, ds);
          
          

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOrder.DataSource = ds;
                if (ViewState["CURRENTSELECTEDORDERID"] == null)
                {
                    ViewState["CURRENTSELECTEDORDERID"] = ds.Tables[0].Rows[0]["FLDBULKORDERID"].ToString();
                    //gvOrder.SelectedIndex = 0;
                }
                //SetRowSelection();
            }
            else
            {
                gvOrder.DataSource = ds;
                //DataTable dt = ds.Tables[0];
                // ShowNoRecordsFound(dt, gvOrder);
            }
            gvOrder.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOrder_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvOrder_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            BindPageURL(nCurrentRow);
    //            SetRowSelection();
    //            BindDataSL();
    //            SetPageNavigatorSL();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvOrder_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;
        BindPageURL(nCurrentRow);
        ViewState["PAGENUMBERSL"] = 1;
        BindDataSL();
        //SetPageNavigatorSL();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
           
            RadLabel lblorderid = (RadLabel)gvOrder.Items[rowindex].FindControl("lblorderid");
            if (lblorderid != null)
            {
                ViewState["CURRENTSELECTEDORDERID"] = lblorderid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetRowSelection()
    //{
    //    //gvOrder.SelectedIndex = -1;
    //    foreach (GridDataItem i in gvOrder.MasterTableView.Items)
    //    {
    //        if (ViewState["CURRENTSELECTEDORDERID"] != null)
    //        {
    //            if (gvOrder.MasterTableView.Items[0].GetDataKeyValue("FLDBULKORDERID").ToString().Equals(ViewState["CURRENTSELECTEDORDERID"].ToString()))
    //            {
                    
    //            }
    //        }
    //    }
    //}

    //protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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
            ViewState["PAGENUMBER"] = 1;
            BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //Seal number grid
    
    private void BindDataSL()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEALNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Seal Number", "Status" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSIONSL"] == null) ? null : (ViewState["SORTEXPRESSIONSL"].ToString());

            if (ViewState["SORTDIRECTIONSL"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSL"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealNumberPurchasedSearch(new Guid(ViewState["CURRENTSELECTEDORDERID"].ToString())
                           , new Guid(ViewState["STOREITEMID"].ToString())
                           , sortexpression, sortdirection
                           , gvSealNumber.CurrentPageIndex+1
                           , gvSealNumber.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvSealNumber", "Seal Numbers", alCaptions, alColumns, ds);
            gvSealNumber.DataSource = ds;
            gvSealNumber.VirtualItemCount = iRowCount;
          
            ViewState["ROWCOUNTSL"] = iRowCount;
            ViewState["TOTALPAGECOUNTSL"] = iTotalPageCount;
            //SetPageNavigatorSL();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuSealNumberExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEXCEL"))
            {
                ShowExcelSL();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelSL()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEALNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Seal Number", "Status" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSIONSL"] == null) ? null : (ViewState["SORTEXPRESSIONSL"].ToString());

            if (ViewState["SORTDIRECTIONSL"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSL"].ToString());

            if (ViewState["ROWCOUNTSL"] == null || Int32.Parse(ViewState["ROWCOUNTSL"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNTSL"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealNumberPurchasedSearch(new Guid(ViewState["CURRENTSELECTEDORDERID"].ToString())
                           , new Guid(ViewState["STOREITEMID"].ToString())
                           , sortexpression, sortdirection
                           , Convert.ToInt16(ViewState["PAGENUMBERSL"].ToString())
                           , iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Seal Numbers", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["SORTEXPRESSIONSL"] = se.SortExpression;

            if (ViewState["SORTDIRECTIONSL"] != null && ViewState["SORTDIRECTIONSL"].ToString() == "0")
                ViewState["SORTDIRECTIONSL"] = 1;
            else
                ViewState["SORTDIRECTIONSL"] = 0;
            BindDataSL();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSIONSL"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONSL"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTIONSL"] == null || ViewState["SORTDIRECTIONSL"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvOrder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                BindPageURL(nCurrentRow);
                //SetRowSelection();
                BindDataSL();
                gvSealNumber.Rebind();
                //SetPageNavigatorSL();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataSL();
    }

    protected void gvSealNumber_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}

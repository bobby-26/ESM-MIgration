using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inventory;

public partial class InventoryStoreItemTransactionEntry : PhoenixBasePage
{

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvStoreTransactionEntry.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH");
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Vendors", "VENDORS");
            toolbarmain.AddButton("Transaction", "STOCKTRANSACTION");
            toolbarmain.AddButton("Trns. Details", "STOCKTRANSACTIONDETAILS");
            toolbarmain.AddButton("Components", "COMPONENTS");
            toolbarmain.AddButton("Details", "DETAILS");
            toolbarmain.AddButton("Attachment", "ATTACHMENT");
            MenuStoreItemTransactionEntry.AccessRights = this.ViewState; 
            MenuStoreItemTransactionEntry.MenuList = toolbarmain.Show();
            MenuStoreItemTransactionEntry.SetTrigger(pnlStoreItemTransactionEntry);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemTransactionEntry.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreTransactionEntry')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuGridStoreItemTransactionEntry.AccessRights = this.ViewState;  
            MenuGridStoreItemTransactionEntry.MenuList = toolbargrid.Show();
            MenuGridStoreItemTransactionEntry.SetTrigger(pnlStoreItemTransactionEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                BindData();

            }
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuStoreItemTransactionEntry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inventory/InventoryStoreItemFilter.aspx");
            }

            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../Inventory/InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("VENDORS"))
            {
                Response.Redirect("../Inventory/InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("VENDORS"))
            {
                Response.Redirect("../Inventory/InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("STOCKTRANSACTION"))
            {
                Response.Redirect("../Inventory/InventoryStoreItemTransactionEntry.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("STOCKTRANSACTIONDETAILS"))
            {
                Response.Redirect("../Inventory/InventoryStoreTransactionEntryDetail.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("COMPONENTS"))
            {
                Response.Redirect("../Inventory/InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Inventory/InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }
            if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("../Inventory/InventoryStoreItem.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"]);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
 
    protected void StoreItemTransactionEntry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "TRANSACTIONTYPENAME", "COMPONENTNUMBER", "WORKORDERTYPENAME", "REPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name", "Transaction Type", "Component Number", "Work Order", "Reported By." };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemDispositionTransactionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
               null, Request.QueryString["STOREITEMID"].ToString(), null, null,
              null, null,
              sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
              General.ShowRecords(null),
              ref iRowCount,
              ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemVendor.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory Stock Item Transaction Entry</h3></td>");
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "TRANSACTIONTYPENAME", "COMPONENTNUMBER", "WORKORDERTYPENAME","REPORTEDBY"  };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name","Transaction Type" ,"Component Number", "Work Order", "Reported By." };
            string strHeader = "Inventory Stock Item Transaction Entry";

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemDispositionTransactionSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                           null,Request.QueryString["STOREITEMID"].ToString(),null , null,  
                          null, null,
                          sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                          General.ShowRecords(null),
                          ref iRowCount,
                          ref iTotalPageCount);



            General.SetPrintOptions("gvStoreTransactionEntry", "Store Transaction Entry", alCaptions, alColumns, ds);
       
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvStoreTransactionEntry.DataSource = ds;
                gvStoreTransactionEntry.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvStoreTransactionEntry);
            }

           General.SetPrintOptions("gvStoreTransactionEntry", strHeader, alCaptions, alColumns, ds); 
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvStoreTransactionEntry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvStoreTransactionEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStockItemTransactionEntry
                    (
                      ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStoreItemDispositionId")).Text
                    );
            }
            else
            {
                _gridView.EditIndex = -1;
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreTransactionEntry_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvStoreTransactionEntry_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvStoreTransactionEntry_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    }

                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
                string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
                e.Row.Attributes["ondblclick"] = _jsDouble;
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
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {


            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {

            gvStoreTransactionEntry.SelectedIndex = -1;
            gvStoreTransactionEntry.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void SetPageNavigator()
    {

        try
        {

            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void DeleteStockItemTransactionEntry(string StoreItemDispositionId)
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
  
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();
            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    

}

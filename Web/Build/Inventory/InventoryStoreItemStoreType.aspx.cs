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
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;

public partial class InventoryStoreItemStoreType : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvStoreType.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvStoreType.UniqueID, "Edit$" + r.RowIndex.ToString());            
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemStoreType.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListStoreType.aspx?mode=custom', true);", "Component", "component.png", "ADDCOMPONENT");
            MenuGridStoreItemStoreType.AccessRights = this.ViewState;  
            MenuGridStoreItemStoreType.MenuList = toolbargrid.Show();
            MenuGridStoreItemStoreType.SetTrigger(pnlStoreItemStoreType);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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

    protected void MenuGridStoreItemStoreType_TabStripCommand(object sender, EventArgs e)
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
            string[] alColumns = { "FLDSTORETYPENUMBER", "FLDSTORETYPENAME", "FLDQUANTITYINSTORETYPE", "FLDDRAWINGNUMBER", "FLDPOSITION", "FLDOLDPARTNUMBER" };
            string[] alCaptions = { "Number", "Name", "In Use", "Drawing Number", "Position", "Alternative Number" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInventoryStoreItemStoreType.StoreItemStoreTypeSearch(Request.QueryString["STOREITEMID"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , sortexpression, sortdirection
                            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                            , ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemStoreType.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory StoreItem StoreType</h3></td>");
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

            string[] alColumns = { "FLDSTORETYPENUMBER", "FLDSTORETYPENAME", "FLDQUANTITYINSTORETYPE", "FLDDRAWINGNUMBER", "FLDPOSITION", "FLDOLDPARTNUMBER" };
            string[] alCaptions = { "Number", "Name", "In Use", "Drawing Number", "Position", "Alternative Number" };
         
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInventoryStoreItemStoreType.StoreItemStoreTypeSearch(Request.QueryString["STOREITEMID"].ToString(),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                sortexpression, sortdirection,
               (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvStoreType", "Inventory Store Item Store Type", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvStoreType.DataSource = ds;
                gvStoreType.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvStoreType);
            }
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

    protected void gvStoreType_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvStoreType.SelectedIndex = -1;
        gvStoreType.EditIndex = -1;
        
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }



    protected void gvStoreType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvStoreType, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }



    protected void gvStoreType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            UpdateStockItemComponent
             (
                 ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentId")).Text,
                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuantityUseInComponentEdit")).Text,
                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDrawingNumberEdit")).Text,
                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemPositionEdit")).Text,
                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemOldNumberEdit")).Text

            );
            ucStatus.Text = "Component information updated";

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvStoreType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void gvStoreType_RowCommand(object sender, GridViewCommandEventArgs e)
    {


        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateStockItemComponent
            (
                ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentId")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuantityUseInComponentEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDrawingNumberEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemPositionEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemOldNumberEdit")).Text

           );
            _gridView.EditIndex = -1;
            BindData();
           ucStatus.Text = "Component information updated";
           
        }
        else if  (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteStockItemComponent
            (
                ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentId")).Text

            );
        }
      
        SetPageNavigator();
    }

    protected void gvStoreType_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvStoreType_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        
        
        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantityUseInComponentEdit")).Focus();
        SetPageNavigator();
    }

    protected void gvStoreType_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        gvStoreType.SelectedIndex = -1;
        gvStoreType.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvStoreType.SelectedIndex = -1;
        gvStoreType.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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


    private void UpdateStockItemComponent(string stockitemcomponentid, string quantityincomponent, string drawingnumber, string position, string oldpartnumber)
    {
        if (!IsValidComponentEdit(quantityincomponent))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInventoryStoreItemStoreType.UpdateStoreItemStoreType
        (
              PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
              , new Guid(Request.QueryString["STOREITEMID"]), General.GetNullableDecimal(quantityincomponent), drawingnumber, position, oldpartnumber
        );
    }

    private void DeleteStockItemComponent(string stockitemcomponentid)
    {
        PhoenixInventoryStoreItemStoreType.DeleteStoreItemStoreType
        (
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
        );
    }

    private bool IsValidComponentEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        GridView _gridView = gvStoreType;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string componentid = nvc.Get("lblStoreTypeId").ToString();
            Guid guidcomponentid = new Guid(componentid);

            if ((Request.QueryString["STOREITEMID"] != null) && (Request.QueryString["STOREITEMID"] != ""))
            {
                PhoenixInventoryStoreItemStoreType.InsertStoreItemStoreType
                (
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , guidcomponentid
                     , new Guid(Request.QueryString["STOREITEMID"])
                     , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                );
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

}

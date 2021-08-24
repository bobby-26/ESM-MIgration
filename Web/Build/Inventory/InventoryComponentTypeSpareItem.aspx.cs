using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;

public partial class InventoryComponentTypeSpareItem : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in dgComponentType.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(dgComponentType.UniqueID, "Edit$" + r.RowIndex.ToString());      
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
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dgComponentType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListSpareType.aspx?mode=custom', true);", "Part", "part.png", "ADDPART");
            MenuGridComponentTypeStockItem.AccessRights = this.ViewState;   
            MenuGridComponentTypeStockItem.MenuList = toolbargrid.Show();
            MenuGridComponentTypeStockItem.SetTrigger(pnlStockItemComponentType);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                BindComponentTypeData();
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


    private void BindComponentTypeData()
    {
        if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
        {
            DataSet ds = PhoenixInventoryComponentType.ListComponentType(new Guid(Request.QueryString["COMPONENTTYPEID"]));
            DataRow dr = ds.Tables[0].Rows[0];
            Title1.Text += "    (ComponentType Number - " + dr["FLDCOMPONENTNUMBER"].ToString() + " ,  " + "ComponentType Name -" + dr["FLDCOMPONENTNAME"].ToString() + ")";
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;


            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION" };
            string[] alCaptions = { "Number", "Name", "Maker's Reference", "In use", "Drawing No.", "Position" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixInventoryComponentTypeSpareItem.ComponentTypeStockItemSearch(Request.QueryString["COMPONENTTYPEID"].ToString()
                            , sortexpression, sortdirection
                            , (int)ViewState["PAGENUMBER"], iRowCount
                            , ref iRowCount, ref iTotalPageCount);



            General.SetPrintOptions("dgComponentType", "Parts", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dgComponentType.DataSource = ds;
                dgComponentType.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, dgComponentType);
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

    protected void MenuGridComponentTypeStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void dgComponentType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgComponentType, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void dgComponentType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            UpdateStockItemComponentType
                  (
                      ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentTypeId")).Text,
                      ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuantityUseInComponentTypeEdit")).Text,
                      ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDrawingNumberEdit")).Text,
                      ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemPositionEdit")).Text
                 );
            ucStatus.Text = "Part information updated";

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

    protected void dgComponentType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void dgComponentType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateStockItemComponentType
                (
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentTypeId")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuantityUseInComponentTypeEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDrawingNumberEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemPositionEdit")).Text
               );
                _gridView.EditIndex = -1;
                BindData();

                ucStatus.Text = "Part information updated";  

            }
           else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStockItemComponentType
                (
                   ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentTypeId")).Text

                );
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponentType_RowDeleting(object sender, GridViewDeleteEventArgs de)
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

    protected void dgComponentType_Sorting(object sender, GridViewSortEventArgs se)
    {

        dgComponentType.SelectedIndex = -1;
        dgComponentType.EditIndex = -1;
        
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }


    protected void dgComponentType_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {

            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;


            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantityUseInComponentTypeEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponentType_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
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
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
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
            dgComponentType.SelectedIndex = -1;
            dgComponentType.EditIndex = -1;

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

            dgComponentType.SelectedIndex = -1;
            dgComponentType.EditIndex = -1;
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


    private void UpdateStockItemComponentType(string stockitemcomponentid, string quantityincomponent, string drawingnumber, string position)
    {
        try
        {


            if (!IsValidComponentTypeStockItemEdit(quantityincomponent))
            {
                ucError.Visible = true;
                return;
            }
            if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
            {
                PhoenixInventoryComponentTypeSpareItem.UpdateComponentTypeSpareItem
                (
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
                      , Convert.ToDecimal(quantityincomponent), drawingnumber, position
                );
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void DeleteStockItemComponentType(string stockitemcomponentid)
    {
        try
        {

            PhoenixInventoryComponentTypeSpareItem.DeleteComponentTypeSpareItem
            (
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
            );

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidComponentTypeStockItemEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        GridView _gridView = dgComponentType;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Enter numeric decimal";
        }
        return (!ucError.IsError);
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("lblStockItemId").ToString();
            Guid guidstockitemid = new Guid(stockitemid);

            if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
            {
                PhoenixInventoryComponentTypeSpareItem.InsertComponentTypeStockItem
                (
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(Request.QueryString["COMPONENTTYPEID"])
                     , guidstockitemid
                     
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

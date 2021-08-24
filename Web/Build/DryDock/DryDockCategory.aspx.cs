using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;

public partial class DryDockCategory : PhoenixBasePage
{
	
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DryDock/DryDockCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DryDock/DryDockCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuDryDockCategory.AccessRights = this.ViewState;
            MenuDryDockCategory.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                                       
                gvCategory.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
           // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Category Code", "Category Name", "Frequency(Months)","Active Y/N" };
        string[] alColumns = { "FLDCATEGORYCODE", "FLDCATEGORYNAME","FLDFREQUENCY", "FLDACTIVEYNNAME" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDryDockCategory.DryDockCategorySearch(txtCode.Text, txtName.Text, (byte?)General.GetNullableInteger(ddlActive.SelectedValue),
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);


        General.ShowExcel("Docking Category", dt, alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void DryDockCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                
                ViewState["PAGENUMBER"] = 1;
                gvCategory.CurrentPageIndex = 0;
                //BindData();                
                gvCategory.Rebind();
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

    private void BindData()
    {        
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Category Code", "Category Name","Frequency(Months)", "Active Y/N" };
        string[] alColumns = { "FLDCATEGORYCODE", "FLDCATEGORYNAME","FLDFREQUENCY", "FLDACTIVEYNNAME" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixDryDockCategory.DryDockCategorySearch(txtCode.Text, txtName.Text, (byte?)General.GetNullableInteger(ddlActive.SelectedValue),
                   gvCategory.CurrentPageIndex+1,
                   gvCategory.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvCategory", "Docking Category", alCaptions, alColumns, ds);
        gvCategory.DataSource = dt;
        gvCategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        //SetPageNavigator();
    }
  

    protected void gvCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

   
    //protected void gvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            string code = ((TextBox)_gridView.FooterRow.FindControl("txtCodeAdd")).Text;
    //            string name = ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text;
    //            string frequency = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtFrequencyAdd")).Text;
    //            if (!IsValidCategory(code, name, frequency))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            PhoenixDryDockCategory.InsertDryDockJobCategory(code.Trim(), name.Trim(), (byte?)General.GetNullableInteger(frequency));
    //            BindData();
    //            ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Focus();
    //        }                                  
    //    }

    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            int id = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
            PhoenixDryDockCategory.DeleteDryDockJobCategory(id);
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvCategory_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtNameEdit")).Focus();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        int id = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
    //        string code = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCodeEdit")).Text;
    //        string name = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text;
    //        string activeyn = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYN")).Checked ? "1" : "0";
    //        string frequency = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtFrequencyEdit")).Text;
    //        if (!IsValidCategory(code, name, frequency))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixDryDockCategory.UpdateDryDockJobCategory(id, code.Trim(), name.Trim(), byte.Parse(activeyn), (byte?)General.GetNullableInteger(frequency));
    //        _gridView.EditIndex = -1;
    //        BindData();     
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }


    //}
    //protected void gvCategory_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //    if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //    ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //    if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //    ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //    if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //    ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //    if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }


    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null)
    //        {
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //        }

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //    }
    //}
   
    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvCategory.SelectedIndex = -1;
    //    gvCategory.EditIndex = -1;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();        
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCategory.SelectedIndex = -1;
    //    gvCategory.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();        
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private bool IsValidCategory(string code, string name, string frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        if (name.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Category Name is required.";
        }
        if (code.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Category Code is required.";
        }
        if (int.TryParse(frequency, out result) && result > 60 || result < 0)
        {
            ucError.ErrorMessage = "Frequency should be between 0 and 60 months.";
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

    protected void gvCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string code = ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text;
                string name = ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text;
                string frequency = ((UserControlNumber)e.Item.FindControl("txtFrequencyAdd")).Text;
                if (!IsValidCategory(code, name, frequency))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDryDockCategory.InsertDryDockJobCategory(code.Trim(), name.Trim(), (byte?)General.GetNullableInteger(frequency));
                // BindData();
                gvCategory.Rebind();
                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                try
                {

                    int id = int.Parse(gvCategory.MasterTableView.Items[0].GetDataKeyValue("FLDCATEGORYID").ToString());
                    PhoenixDryDockCategory.DeleteDryDockJobCategory(id);

                    gvCategory.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton  edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


       


        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }

    protected void gvCategory_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            int id = int.Parse(gvCategory.MasterTableView.Items[0].GetDataKeyValue("FLDCATEGORYID").ToString());
            string code = ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text;
            string name = ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text;
            string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked.Value ? "1" : "0";
            string frequency = ((UserControlNumber)e.Item.FindControl("txtFrequencyEdit")).Text;
            if (!IsValidCategory(code, name, frequency))
            {
                ucError.Visible = true;
                return;
            }
         
            PhoenixDryDockCategory.UpdateDryDockJobCategory(id, code.Trim(), name.Trim(), byte.Parse(activeyn), (byte?)General.GetNullableInteger(frequency));
           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

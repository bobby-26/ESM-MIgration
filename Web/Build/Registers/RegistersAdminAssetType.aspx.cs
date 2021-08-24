using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAdminAssetType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAdminAssetType.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAssetType')", "Print Grid", "icon_print.png", "Print");
            toolbar.AddImageButton("../Registers/RegistersAdminAssetType.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersAdminAssetType.aspx", "Reset", "clear-filter.png", "RESET");
            MenuRegistersAdminAssetType.AccessRights = this.ViewState;
            MenuRegistersAdminAssetType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindCategory();
                BindType();
                BindAssembly();
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
    protected void BindType()
    {
        ddlType.Items.Clear();
        ListItem li = new ListItem("--SELECT--", "");
        DataTable dt = PhoenixRegistersAssetType.ListItemType();
        ddlType.DataTextField = "FLDNAME";
        ddlType.DataValueField = "FLDADMITEMTYPEID";
        ddlType.DataSource = dt;
        ddlType.DataBind();
       // ddlType.Items.Insert(0, li);
    }
    protected void BindAssembly()
    {
        ddlAssembly.Items.Clear();
        ListItem li = new ListItem("--SELECT--", "");
        DataTable dt = PhoenixRegistersAssetType.ListAssembly(General.GetNullableInteger(ddlCategory.SelectedValue));
        ddlAssembly.DataTextField = "FLDNAME";
        ddlAssembly.DataValueField = "FLDASSETTYPEID";
        ddlAssembly.DataSource = dt;
        ddlAssembly.DataBind();
      //  ddlAssembly.Items.Insert(0, li);
    }
    protected void BindCategory()
    {
        ddlCategory.Items.Clear();
        //ListItem li = new ListItem("--SELECT--", "");
        DataTable dt = PhoenixRegistersAssetType.ListCategory(null);
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDTYPEID";
        ddlCategory.DataSource = dt;
        ddlCategory.DataBind();
      //  ddlCategory.Items.Insert(0, li);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAME", "FLDDESCRIPTION", "FLDITEMTYPENAME", "FLDASSEMBLYNAME" };
        string[] alCaptions = { "Name", "Description", "Type", "Assembly" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersAssetType.SearchAssetType(txtSearch.Text, General.GetNullableInteger(ddlCategory.SelectedValue)
            , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null)
            , ref iRowCount, ref iTotalPageCount
            , General.GetNullableInteger(ddlType.SelectedValue)
            , General.GetNullableInteger(ddlAssembly.SelectedValue)
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=AssetCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Asset Category </h3></td>");
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
    protected void MenuRegistersAdminAssetType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (IsValidSearch(
                    ddlCategory.SelectedValue
                    ))
                {
                    gvAssetType.SelectedIndex = -1;
                    gvAssetType.EditIndex = -1;
                    ViewState["PAGENUMBER"] = 1;
                    BindData();
                    SetPageNavigator();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (dce.CommandName.ToUpper().Equals("RESET"))
            {
                ddlCategory.SelectedValue = null;
                ddlType.SelectedValue = null;
                ddlAssembly.SelectedValue = null;
                txtSearch.Text = "";
                gvAssetType.SelectedIndex = -1;
                gvAssetType.EditIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
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
    private bool IsValidSearch(string CategoryId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(CategoryId) == null)
            ucError.ErrorMessage = "Category is Required.";
        return (!ucError.IsError);
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression;
        int? sortdirection = null;

        string[] alColumns = { "FLDNAME", "FLDDESCRIPTION", "FLDITEMTYPENAME", "FLDASSEMBLYNAME" };
        string[] alCaptions = { "Name", "Description", "Type", "Assembly" };

        DataSet ds = new DataSet();

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersAssetType.SearchAssetType(txtSearch.Text, General.GetNullableInteger(ddlCategory.SelectedValue)
            , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null)
            , ref iRowCount, ref iTotalPageCount
            , General.GetNullableInteger(ddlType.SelectedValue)
            , General.GetNullableInteger(ddlAssembly.SelectedValue)
            );
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAssetType.DataSource = ds;
            gvAssetType.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAssetType);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvAssetType", "Asset Category", alCaptions, alColumns, ds);
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
    }
    protected void gvAssetType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAssetType, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvAssetType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        _gridview.EditIndex = -1;
        BindData();
    }
    protected void gvAssetType_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvAssetType_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridview = (GridView)sender;
            _gridview.EditIndex = de.NewEditIndex;
            _gridview.SelectedIndex = de.NewEditIndex;
            BindData();
            ((RadTextBox)_gridview.Rows[de.NewEditIndex].FindControl("txtAssetTypeNameEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAssetType_SelectedIndexChanging(object sender, GridViewSelectEventArgs de)
    {
        gvAssetType.SelectedIndex = de.NewSelectedIndex;
    }

    protected void gvAssetType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (!IsValidAssembly(ddlCategory.SelectedValue
                    ))
            {
                ucError.Visible = true;
                return;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidTeamName(((RadTextBox)_gridView.FooterRow.FindControl("txtAssetTypeNameAdd")).Text
                    , ((RadTextBox)_gridView.FooterRow.FindControl("txtAssetTypeDesAdd")).Text
                    , ((RadComboBox)_gridView.FooterRow.FindControl("ddlItemTypeAdd")).SelectedValue
                    , ((RadComboBox)_gridView.FooterRow.FindControl("ddlAssetAssemblyAdd")).SelectedValue
                    )
                   )
                {
                    ucError.Visible = true;
                    return;
                }
                InsertAssetType(
                    ((RadTextBox)_gridView.FooterRow.FindControl("txtAssetTypeNameAdd")).Text
                    , ((RadTextBox)_gridView.FooterRow.FindControl("txtAssetTypeDesAdd")).Text
                    , General.GetNullableInteger(ddlCategory.SelectedValue)
                    , General.GetNullableInteger(((RadComboBox)_gridView.FooterRow.FindControl("ddlItemTypeAdd")).SelectedValue)
                    , General.GetNullableInteger(((RadComboBox)_gridView.FooterRow.FindControl("ddlAssetAssemblyAdd")).SelectedValue)
                    );
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTeamName(((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeNameEdit")).Text
                    , ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeDesEdit")).Text
                    , ((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlItemTypeEdit")).SelectedValue
                    , ((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlAssetAssemblyEdit")).SelectedValue
                    )
                )
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateAssetType(
                    Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetTypeIDEdit")).Text)
                    ,((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeNameEdit")).Text
                    , ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeDesEdit")).Text
                    ,General.GetNullableInteger(ddlCategory.SelectedValue)
                    ,General.GetNullableInteger(((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlItemTypeEdit")).SelectedValue)
                    ,General.GetNullableInteger(((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlAssetAssemblyEdit")).SelectedValue)
                    );
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteAssetType(
                 Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetTypeID")).Text));
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAssetType_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAssetType.EditIndex = -1;
        gvAssetType.SelectedIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }
    protected void gvAssetType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            if (!IsValidTeamName(((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeNameEdit")).Text
                    , ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeDesEdit")).Text
                    , ((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlItemTypeEdit")).SelectedValue
                    , ((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlAssetAssemblyEdit")).SelectedValue
                    )
                )
            {
                ucError.Visible = true;
                return;
            }
            UpdateAssetType(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssetTypeIDEdit")).Text),
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeNameEdit")).Text
                     ,((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssetTypeDesEdit")).Text
                     ,General.GetNullableInteger(ddlCategory.SelectedValue)
                     ,General.GetNullableInteger(((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlItemTypeEdit")).SelectedValue)
                     ,General.GetNullableInteger(((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlAssetAssemblyEdit")).SelectedValue)
                      );

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
    protected void gvAssetType_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                Label lblItemTypeEdit = (Label)e.Row.FindControl("lblItemTypeEdit");
                RadComboBox ddlItemTypeEdit = (RadComboBox)e.Row.FindControl("ddlItemTypeEdit");
                if (ddlItemTypeEdit != null)
                {
                    ddlItemTypeEdit.Items.Clear();
                    ListItem li = new ListItem("--SELECT--", "");
                    DataTable dt = PhoenixRegistersAssetType.ListItemType();
                    ddlItemTypeEdit.DataTextField = "FLDNAME";
                    ddlItemTypeEdit.DataValueField = "FLDADMITEMTYPEID";
                    ddlItemTypeEdit.DataSource = dt;
                    ddlItemTypeEdit.DataBind();
                   // ddlItemTypeEdit.Items.Insert(0, li);

                    if (lblItemTypeEdit != null)
                        ddlItemTypeEdit.SelectedValue = lblItemTypeEdit.Text;
                }

                Label lblParentId = (Label)e.Row.FindControl("lblParentId");
                RadComboBox ddlAssetAssemblyEdit = (RadComboBox)e.Row.FindControl("ddlAssetAssemblyEdit");
                if (ddlAssetAssemblyEdit != null)
                {
                    ddlAssetAssemblyEdit.Items.Clear();
                    ListItem li = new ListItem("--SELECT--", "");
                    DataTable dt = PhoenixRegistersAssetType.ListAssembly(General.GetNullableInteger(ddlCategory.SelectedValue));
                    ddlAssetAssemblyEdit.DataTextField = "FLDNAME";
                    ddlAssetAssemblyEdit.DataValueField = "FLDASSETTYPEID";
                    ddlAssetAssemblyEdit.DataSource = dt;
                    ddlAssetAssemblyEdit.DataBind();
                    //ddlAssetAssemblyEdit.Items.Insert(0, li);

                    if (lblParentId != null)
                        ddlAssetAssemblyEdit.SelectedValue = lblParentId.Text;
                }
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                RadComboBox ddlItemTypeAdd = (RadComboBox)e.Row.FindControl("ddlItemTypeAdd");
                if (ddlItemTypeAdd != null)
                {
                    ddlItemTypeAdd.Items.Clear();
                    ListItem li = new ListItem("--SELECT--", "");
                    DataTable dt = PhoenixRegistersAssetType.ListItemType();
                    ddlItemTypeAdd.DataTextField = "FLDNAME";
                    ddlItemTypeAdd.DataValueField = "FLDADMITEMTYPEID";
                    ddlItemTypeAdd.DataSource = dt;
                    ddlItemTypeAdd.DataBind();
                  //  ddlItemTypeAdd.Items.Insert(0, li);
                }
                RadComboBox ddlAssetAssemblyAdd = (RadComboBox)e.Row.FindControl("ddlAssetAssemblyAdd");
                if (ddlAssetAssemblyAdd != null)
                {
                    ddlAssetAssemblyAdd.Items.Clear();
                    ListItem li = new ListItem("--SELECT--", "");
                    DataTable dt = PhoenixRegistersAssetType.ListAssembly(General.GetNullableInteger(ddlCategory.SelectedValue));
                    ddlAssetAssemblyAdd.DataTextField = "FLDNAME";
                    ddlAssetAssemblyAdd.DataValueField = "FLDASSETTYPEID";
                    ddlAssetAssemblyAdd.DataSource = dt;
                    ddlAssetAssemblyAdd.DataBind();
                   // ddlAssetAssemblyAdd.Items.Insert(0, li);
                }
                ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
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
        gvAssetType.SelectedIndex = -1;
        gvAssetType.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvAssetType.SelectedIndex = -1;
        gvAssetType.EditIndex = -1;

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
        gvAssetType.SelectedIndex = -1;
        gvAssetType.EditIndex = -1;
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
    private void InsertAssetType(string AssetTypeName,string AssetTypeDescription, int? type, int? ItemType, int? Assembly)
    {
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixRegistersAssetType.InsertAssetType(rowusercode, AssetTypeName, AssetTypeDescription, type, ItemType, Assembly);
        ucStatus.Text = "Category Added";
    }
    private void UpdateAssetType(int AssetTypeID, string AssetTypeName, string AssetTypeDescription, int? type, int? ItemType, int? Assembly)
    {
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixRegistersAssetType.UpdateAssetType(rowusercode, AssetTypeID, AssetTypeName, AssetTypeDescription, type, ItemType, Assembly);
        ucStatus.Text = "Category updated";
    }
    private bool IsValidTeamName(string AssetTypeName, string AssetTypeDescription, string AssetType, string Assembly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvAssetType;

        if (AssetTypeName.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        if (AssetTypeDescription.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (General.GetNullableInteger(AssetType) == null)
            ucError.ErrorMessage = "Type is Required";
        //if (General.GetNullableInteger(AssetType) == 2 && General.GetNullableInteger(Assembly) == null)
        //    ucError.ErrorMessage = "Assembly is Required";
        if (General.GetNullableInteger(AssetType) == 1 && General.GetNullableInteger(Assembly) != null)
            ucError.ErrorMessage = "Assembly should not map for Assembly itself";

        return (!ucError.IsError);
    }

    private bool IsValidAssembly(string Assembly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvAssetType;

        if (General.GetNullableInteger(Assembly) == null)
            ucError.ErrorMessage = "Category is Required.";
        return (!ucError.IsError);
    }
    private void DeleteAssetType(int AssetTypeID)
    {
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixRegistersAssetType.DeleteAssetType(rowusercode, AssetTypeID);
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
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAssembly();
        ViewState["PAGENUMBER"] = 1;
        gvAssetType.SelectedIndex = -1;
        gvAssetType.EditIndex = -1;
        BindData();
        SetPageNavigator(); 
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}


using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.AdminAssetSpareItems;
public partial class Registers_RegistersAdminAssetSpareItems : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarlist = new PhoenixToolbar();
            toolbarlist.AddImageLink("javascript:parent.Openpopup('code1','','RegistersAdminAssetAdd.aspx?AssetId=" + null + "'); return true;", "Add", "add.png", "ADD");
            toolbarlist.AddImageLink("../Registers/RegistersAdminAssetSpareItems.aspx", "Asset Search", "search.png", "SEARCH");
            toolbarlist.AddImageButton("../Registers/RegistersAdminAssetSpareItems.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarlist.AddImageLink("javascript:CallPrint('gvAdminAssetItems')", "Print Grid", "icon_print.png", "PRINT");
            toolbarlist.AddImageButton("../Registers/RegistersAdminAssetSpareItems.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuRegistersAdminAsset.AccessRights = this.ViewState;
            MenuRegistersAdminAsset.MenuList = toolbarlist.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ASSETID"] = String.Empty;
                
                if (Request.QueryString["ASSETID"] != String.Empty)
                {
                    ViewState["ASSETPARENTID"] = Request.QueryString["ASSETID"];
                }
                BindAssetType();
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
    protected void BindAssetType()
    {
        ddlItemType.Items.Clear();
        DataTable dt = PhoenixRegistersAssetType.ListAssetType(1, 2);
        ddlItemType.DataSource = dt;
        ddlItemType.DataTextField = "FLDNAME";
        ddlItemType.DataValueField = "FLDASSETTYPEID";
        ddlItemType.DataBind();
        ddlItemType.Items.Insert(0, new ListItem("--Select--", ""));
    }
    protected void MenuRegistersAdminAsset_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            if (IsValidSearch(
                    ddlLocation.SelectedZone
                    ))
            {
                gvAdminAssetItems.SelectedIndex = -1;
                gvAdminAssetItems.EditIndex = -1;
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
            ddlLocation.SelectedZone = "";
            txtSerialNo.Text = "";
            ddlItemType.SelectedValue = null;
            gvAdminAssetItems.SelectedIndex = -1;
            gvAdminAssetItems.EditIndex = -1;
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    private bool IsValidSearch(string ZoneId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ZoneId) == null)
            ucError.ErrorMessage = "Zone is Required.";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDASSETTYPENAME",  "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER", "FLDSTATUSNAME" };
            string[] alCaptions = { "Item", "Serial Number", "Name", "Maker", "Model", "Status" };

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixAdministrationAssetSpareItems.SearchAssetSpareItems(1, General.GetNullableInteger(ddlItemType.SelectedValue), txtSerialNo.Text, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                             General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlLocation.SelectedZone));

            General.SetPrintOptions("gvAdminAssetItems", "Spare", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdminAssetItems.DataSource = ds;
                gvAdminAssetItems.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAdminAssetItems);
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDASSETTYPENAME", "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER", "FLDSTATUSNAME" };
        string[] alCaptions = { "Item", "Serial Number", "Name", "Maker", "Model", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAdministrationAssetSpareItems.SearchAssetSpareItems(1, General.GetNullableInteger(ddlItemType.SelectedValue), txtSerialNo.Text, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                             General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlLocation.SelectedZone));

        Response.AddHeader("Content-Disposition", "attachment; filename=AdminAssetsHardware.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Spare</h3></td>");
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
    protected void gvAdminAssetItems_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton cd = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cd != null)
            {
                cd.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cd.Visible = SessionUtil.CanAccess(this.ViewState, cd.CommandName);
            }
            
            Label AssetId = (Label)e.Row.FindControl("lblAdminAssetID");
            Label LocationId = (Label)e.Row.FindControl("lblLocationId");
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            {
                cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                cmdEdit.Attributes.Add("onclick", "javascript:parent.Openpopup('Code1', '', '../Registers/RegistersAdminAssetAdd.aspx?AssetId=" + AssetId.Text + "&ZoneId=" + LocationId.Text + "');return true;");
                cmdEdit.Visible = true;
            }
        }
    }

    protected void gvAdminAssetItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        _gridview.EditIndex = -1;
        BindData();
    }

    protected void gvAdminAssetItems_Rowupdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

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
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvAdminAssetItems_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdminAssetItems.EditIndex = -1;
        gvAdminAssetItems.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
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
            return true;

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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvAdminAssetItems.EditIndex = -1;
        gvAdminAssetItems.SelectedIndex = -1;
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
        gvAdminAssetItems.SelectedIndex = -1;
        gvAdminAssetItems.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void gvAdminAssetItems_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void gvAdminAssetItems_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAdminAssetItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            gvAdminAssetItems.SelectedIndex = nCurrentRow;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string AssetId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAssetID")).Text;
                PhoenixAdministrationAsset.DeleteAssetType(General.GetNullableGuid(AssetId));
                _gridView.SelectedIndex = -1;
                _gridView.EditIndex = -1;
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
}

using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class RegisterAdminAsset : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarlist = new PhoenixToolbar();
            toolbarlist.AddImageButton("../Registers/RegistersAdminAsset.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarlist.AddImageButton("../Registers/RegistersAdminAsset.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuRegistersAdminAsset.AccessRights = this.ViewState;
            MenuRegistersAdminAsset.MenuList = toolbarlist.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
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

    protected void MenuRegistersAdminAsset_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }
    public void ClearFilter()
    {
        BindData();
        SetPageNavigator();
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
            string[] alColumns = { "FLDASSETTYPENAME", "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER", "FLDQUANTITY" };
            string[] alCaptions = { "Asset Type", "Serial Number", "Name", "Maker", "Model", "Quantity" };

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixAdministrationAsset.SearchAsset(null, null, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, 1,
                                                            null, null, null, null, null);


            General.SetPrintOptions("gvAdminAsset", "Hardware", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAdminAsset.DataSource = ds;
                gvAdminAsset.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAdminAsset);
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
    private bool IsVaildDate(string date)
    {
        if (Convert.ToDateTime(date) != null && Convert.ToDateTime(date) > DateTime.Now)
        {
            // ucError.ErrorMessage = "Logged From date can't be greater than Today";
            // return (!ucError.IsError);
        }
        return (ucError.IsError);
    }
    private bool IsValidToDate(DateTime date)
    {
        if (date != null && date > DateTime.Now)
        {
            ucError.ErrorMessage = "Logged To Date can't be greater than Today.";
        }
        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDASSETTYPENAME", "FLDSERIALNO", "FLDNAME", "FLDMAKER", "FLDIDENTIFICATIONNUMBER", "FLDQUANTITY" };
        string[] alCaptions = { "Asset Type", "Serial Number", "Name", "Maker", "Model", "Quantity" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAdministrationAsset.SearchAsset(null, null, sortdirection, sortexpression, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                        General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, 1,
                                                        null, null, null, null, null);

        General.SetPrintOptions("gvAdminAsset", "Hardware", alCaptions, alColumns, ds);

        Response.AddHeader("Content-Disposition", "attachment; filename=AdminAssetsHardware.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Asset Hardware</h3></td>");
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
    protected void gvAdminAsset_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            Label lblassetid = (Label)e.Row.FindControl("lblAdminAssetID");
            Label lblassettypeid = (Label)e.Row.FindControl("lblAssetTypeId");
            ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "Openpopup('code1', '', '../Registers/RegistersAdminAssetAdd.aspx?Assetid=" + lblassetid.Text + "&AssetTypeId=" + lblassettypeid.Text + "'); return false;");
            }

            //LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkAdminAssetName");
            //lbtn.Attributes.Add("onclick", "Openpopup('code1', '', '../Registers/RegistersAdminAssetAdd.aspx?Assetid=" + lblassetid.Text + "'); return false;");

            //UserControlToolTip ucname = (UserControlToolTip)e.Row.FindControl("ucName");
            //lbtn.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucname.ToolTip + "', 'visible');");
            //lbtn.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucname.ToolTip + "', 'hidden');");

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

            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton cmdMap = (ImageButton)e.Row.FindControl("cmdSoftwareMapping");
            if (cmdMap != null)
            {
                if (drv["FLDITEMTYPE"].ToString() == "1")
                {
                    cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                    cmdMap.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Registers/RegistersAdminAssetMapping.aspx?ASSETID=" + drv["FLDASSETID"].ToString() + "&ASSETNAME=" + drv["FLDNAME"] + "');return false;");
                    cmdMap.Visible = true;
                }
                else
                {
                    cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                    cmdMap.Visible = false;
                }
            }
        }
    }

    protected void gvAdminAsset_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        _gridview.EditIndex = -1;
        BindData();
    }

    protected void gvAdminAsset_Rowupdating(object sender, GridViewUpdateEventArgs e)
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

    protected void gvAdminAsset_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdminAsset.EditIndex = -1;
        gvAdminAsset.SelectedIndex = -1;
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
        gvAdminAsset.EditIndex = -1;
        gvAdminAsset.SelectedIndex = -1;
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
        gvAdminAsset.SelectedIndex = -1;
        gvAdminAsset.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void gvAdminAsset_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        //((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtAdminAssetNameEdit")).Focus();
        SetPageNavigator();

    }
    protected void gvAdminAsset_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAdminAsset_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAdministrationAsset.DeleteAssetType(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAssetID")).Text));
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

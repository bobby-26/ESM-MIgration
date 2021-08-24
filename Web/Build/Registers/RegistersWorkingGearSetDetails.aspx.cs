using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Text;

public partial class RegistersWorkingGearSetDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            FillMappedRanks();
            FillMappedVessels();
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            if (!IsPostBack)
            {
                ViewState["SETID"] = Filter.CurrentWorkingGearSetSelection;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                cblRank.DataSource = PhoenixRegistersRank.ListRank();
                cblRank.DataBind();

                cblVessel.DataSource = PhoenixRegistersVessel.ListVessel();
                cblVessel.DataBind();
                BindData();
                SetPageNavigator();


            }

            toolbar.AddImageButton("../Registers/RegistersWorkingGearSetDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWorkingGearSetItems')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:showPickList('spnPickListStore', 'codehelp1', '', 'RegistersWorkingGearItemSelection.aspx', true); return false;", "Add Items", "add.png", "ADD");
            MenuWorkingGearSetDetails.AccessRights = this.ViewState;
            MenuWorkingGearSetDetails.MenuList = toolbar.Show();
            MenuWorkingGearSetDetails.SetTrigger(pnlWorkingGearSetDetails);


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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDWORKINGGEARITEMNAME", "FLDNOOFUNITS" };
        string[] alCaptions = { "S.No", "Item Name", "No of Units" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearSetDetails.WorkingGearSetItemSearch(General.GetNullableInteger(ViewState["SETID"].ToString()), sortexpression, sortdirection,
                                (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=WorkingGearSetItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap><h4><center>Working Gear Set Item Details</center></h4></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        DataRow drHeader = ds.Tables[0].Rows[0];

        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'><tr>");
        HttpContext.Current.Response.Write("<td colspan=" + 3 + " align='left' nowrap ><b>Set Name : </b>" + drHeader["FLDWORKINGGEARSETNAME"].ToString() + "</td>");
        HttpContext.Current.Response.Write("<tr>");

        HttpContext.Current.Response.Write("</TABLE>");

        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td nowrap>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td nowrap>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }

    protected void MenuWorkingGearSetDetails_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDWORKINGGEARITEMNAME", "FLDNOOFUNITS" };
        string[] alCaptions = { "S.No", "Item Name", "No of Units" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersWorkingGearSetDetails.WorkingGearSetItemSearch(General.GetNullableInteger(ViewState["SETID"].ToString()), sortexpression, sortdirection,
                                (int)ViewState["PAGENUMBER"], 10, ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWorkingGearSetItems.DataSource = ds;
            gvWorkingGearSetItems.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvWorkingGearSetItems);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        StringBuilder title = new StringBuilder();
        title.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        title.Append("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap><h4><center>Working Gear Set Item Details</center></h4></td></tr>");
        title.Append("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap>&nbsp;</td>");
        title.Append("</tr>");
        title.Append("</TABLE>");
        DataRow drHeader = ds.Tables[0].Rows[0];
        title.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'><tr>");
        title.Append("<td colspan=" + 3 + " align='left' nowrap ><b>Set Name : </b>" + drHeader["FLDWORKINGGEARSETNAME"].ToString() + "</td>");
        title.Append("<tr>");
        title.Append("</TABLE>");

        General.SetPrintOptions("gvWorkingGearSetItems", title.ToString(), alCaptions, alColumns, ds);
    }

    private void FillMappedRanks()
    {
        if (ViewState["SETID"] != null && General.GetNullableInteger(ViewState["SETID"].ToString()).HasValue)
        {
            DataSet ds = PhoenixRegistersWorkingGearSet.EditWorkingGearSet(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["SETID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ChkboxFillValues(cblRank, dr["FLDAPPLICABLERANKS"].ToString());
            }

        }
    }

    private void FillMappedVessels()
    {
        if (ViewState["SETID"] != null && General.GetNullableInteger(ViewState["SETID"].ToString()).HasValue)
        {
            DataSet ds = PhoenixRegistersWorkingGearSet.EditWorkingGearSet(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["SETID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ChkboxFillValues(cblVessel, dr["FLDAPPLICABLEVESSELS"].ToString());
            }

        }
    }

    protected void gvWorkingGearSetItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvWorkingGearSetItems_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvWorkingGearSetItems.EditIndex = -1;
        gvWorkingGearSetItems.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }

    protected void gvWorkingGearSetItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateWorkingGearSetItem(
                     ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSetItemIdEdit")).Text,
                     ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucNoOfUnitEdit")).Text
                 );
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteWorkingGearSetItem(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSetItemId")).Text);
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkingGearSetItems_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvWorkingGearSetItems_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gv = (GridView)sender;
        _gv.EditIndex = de.NewEditIndex;
        BindData();
        SetPageNavigator();
    }

    protected void gvWorkingGearSetItems_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete()");
            }
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
        int result;
        try
        {
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
            gvWorkingGearSetItems.SelectedIndex = -1;
            gvWorkingGearSetItems.EditIndex = -1;
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

    private void UpdateWorkingGearSetItem(string SetItemId, string NoofUnits)
    {
        if (!IsValidWorkingGearSetItem(NoofUnits))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearSetDetails.UpdateWorkingGearSetItemDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(SetItemId), int.Parse(NoofUnits));
    }

    private bool IsValidWorkingGearSetItem(string NoOfUnits)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(NoOfUnits))
            ucError.ErrorMessage = "No of Units is required.";

        return (!ucError.IsError);
    }

    private void DeleteWorkingGearSetItem(string SetItemId)
    {
        PhoenixRegistersWorkingGearSetDetails.DeleteWorkingGearSetItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(SetItemId));
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
    }

    private string ChkboxSelectedValue(CheckBoxList cbl)
    {
        StringBuilder str = new StringBuilder();
        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                str.Append(item.Value.ToString());
                str.Append(",");
            }
        }
        if (str.Length > 1)
        {
            str.Remove(str.Length - 1, 1);
        }
        return str.ToString();
    }

    private void ChkboxFillValues(CheckBoxList cbl, string strlist)
    {
        strlist = "," + strlist + ",";
        foreach (ListItem item in cbl.Items)
        {
            item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
        }

    }

    protected void ApplicableRanksSelection(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(General.GetNullableString(cblRank.SelectedValue)))
        {
            int setid = int.Parse(ViewState["SETID"].ToString());
            PhoenixRegistersWorkingGearSetDetails.UpdateWorkingGearSetRanks(PhoenixSecurityContext.CurrentSecurityContext.UserCode, setid, ChkboxSelectedValue(cblRank));
            FillMappedRanks();
        }
    }

    protected void ApplicableVesselsSelection(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(General.GetNullableString(cblVessel.SelectedValue)))
        {
            int setid = int.Parse(ViewState["SETID"].ToString());
            PhoenixRegistersWorkingGearSetDetails.UpdateWorkingGearSetVessels(PhoenixSecurityContext.CurrentSecurityContext.UserCode, setid, ChkboxSelectedValue(cblVessel));
            FillMappedVessels();
        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
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
}

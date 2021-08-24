using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAdminITStatus : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvITStatus.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvITStatus.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAdminITStatus.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvITStatus')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersAdminITStatus.aspx", "Find", "search.png", "FIND");
            MenuRegistersITStatus.AccessRights = this.ViewState;
            MenuRegistersITStatus.MenuList = toolbar.Show();
           // MenuRegistersITStatus.SetTrigger(pnlITStatusEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTCODE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Short Code", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersITStatus .SearchITStatus (txtSearch.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
             General.ShowRecords (null),
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=ITStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Status</h3></td>");
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
    protected void RegistersITStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvITStatus.SelectedIndex = -1;
                gvITStatus.EditIndex = -1;

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

        string[] alColumns = { "FLDSHORTCODE","FLDSTATUSNAME" };
        string[] alCaptions = { "Short Code","Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersITStatus.SearchITStatus(txtSearch.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords (null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvITStatus", "IT Status", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvITStatus.DataSource = ds;
            gvITStatus.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvITStatus);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }
    protected void gvITStatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvITStatus_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvITStatus, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvITStatus_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvITStatus.SelectedIndex = -1;
        gvITStatus.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvITStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            if (!IsValidStatusName(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblITSTatusIDEdit")).Text),
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusNameEdit")).Text,
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusShortNameEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateITStatus(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblITSTatusIDEdit")).Text),
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusNameEdit")).Text,
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusShortNameEdit")).Text
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

    protected void gvITStatus_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvITStatus.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvITStatus_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((RadTextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtITStatusShortNameEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvITStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidStatusName(((RadTextBox)_gridView.FooterRow.FindControl("txtITStatusAdd")).Text,
                    ((RadTextBox)_gridView.FooterRow.FindControl("txtITStatusShortCodeAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertITStatus(
                    ((RadTextBox)_gridView.FooterRow.FindControl("txtITStatusAdd")).Text,
                    ((RadTextBox)_gridView.FooterRow.FindControl("txtITStatusShortCodeAdd")).Text);
                BindData();
                ((RadTextBox)_gridView.FooterRow.FindControl("txtITStatusShortCodeAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidStatusName(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblITSTatusIDEdit")).Text),
                    ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusNameEdit")).Text,
                    ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusShortNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateITStatus(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblITSTatusIDEdit")).Text),
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusNameEdit")).Text,
                     ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtITStatusShortNameEdit")).Text
                     );
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteITStatus(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblITStatusID")).Text));
            }
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void DeleteITStatus(int p)
    //{
    //    throw new NotImplementedException();
    //}

    protected void gvITStatus_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvITStatus_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            /*check the new,closed,new status and hide the action button*/
            //Label  lblID = (Label)e.Row.FindControl("lblITStatusID");
            //string StatusID =lblID.Text ;
            //if (StatusID == "1" || StatusID == "2" || StatusID == "3")
            //{
            //    Label lbl = (Label)e.Row.FindControl("lblITStatusName");
            //    if (lbl != null) lbl.Visible = true;
            //    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkITStausName");
            //    if (lnk != null) lnk.Visible = false;
            //    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            //    if (db != null)
            //    {
            //        //db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            //        db.Visible = false;
            //    }

            //    ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            //    if (eb != null) eb.Visible = false;

            //    ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            //    if (sb != null) sb.Visible = false;

            //    ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            //    if (cb != null) cb.Visible = false;
            //}
            //else
            //{
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
            //}

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvITStatus.SelectedIndex = -1;
        gvITStatus.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvITStatus.SelectedIndex = -1;
        gvITStatus.EditIndex = -1;

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
        gvITStatus.SelectedIndex = -1;
        gvITStatus.EditIndex = -1;
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

    private void InsertITStatus(string ITStautsName, string ITshortcode)
    {
       
        PhoenixRegistersITStatus.InsertITStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ITStautsName,ITshortcode);
    }

    private void UpdateITStatus(int ITStatusID, string ITStausName, string ITShortCode)
    {
        
        PhoenixRegistersITStatus .UpdateITStatus (0, ITStatusID, ITStausName,ITShortCode);
        ucStatus.Text = "ITSupport Status updated";
    }

    private bool IsValidStatusName(int ITStatusID,string ITStausName, string ITShortCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvITStatus;

        if (ITStausName.Trim().Equals(""))
            ucError.ErrorMessage = "Status name is required.";
        if (ITShortCode.Trim().Equals(""))
            ucError.ErrorMessage = "Short code is required.";
      

        return (!ucError.IsError);
    }
    private bool IsValidStatusName(string ITStausName, string ITshortcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvITStatus;

        if (ITStausName.Trim().Equals(""))
            ucError.ErrorMessage = "Status name is required.";
        if (ITshortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Short code is required.";

        return (!ucError.IsError);
    }
    private void DeleteITStatus(int ITStatusID)
    {
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixRegistersITStatus.DeleteITStatus(rowusercode, ITStatusID);
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


}

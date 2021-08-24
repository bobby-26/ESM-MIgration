using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersCorrectiveActionForNC : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvQuick.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvQuick.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersCorrectiveActionForNC.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvQuick')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["QuickCodeType"] = null;
                string module = Request.QueryString["module"].ToString();
                //ucQuickType.QuickTypeGroup = module;
                //if (Request.QueryString["quickcodetype"] != null)
                //{
                //    ViewState["QuickCodeType"] = Request.QueryString["quickcodetype"].ToString();
                //    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                //}
                //if (Request.QueryString["quickcodetype"] != null)
                //{
                //    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                //    ucQuickType.QuickTypeShowYesNo = "0";
                //    CaptionChange(module, ViewState["QuickCodeType"].ToString());
                //    ucQuickType.Visible = false;
                //    lblRegister.Visible = false;

                //}
                //else
                //{
                //    ucQuickType.bind();
                //}                
            }
            //BindFirstValue();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void BindFirstValue()
    //{
    //    string quicktype = null;
    //    if (ViewState["QuickCodeType"] == null || ViewState["QuickCodeType"].ToString() == "")
    //    {
    //        if (ucQuickType.SelectedQuickType == "")
    //        {
    //            ucQuickType.QuickTypeShowYesNo = "1";
    //            string yesno = ucQuickType.QuickTypeShowYesNo;
    //            DataSet ds1 = PhoenixRegistersQuick.ListQuickType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                ucQuickType.QuickTypeGroup, Convert.ToInt32(yesno));
    //            if (ds1.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow drActivity = ds1.Tables[0].Rows[0];
    //                quicktype = drActivity["FLDQUICKTYPECODE"].ToString();
    //                ucQuickType.SelectedQuickType = drActivity["FLDQUICKTYPECODE"].ToString();
    //                ucQuickType.bind();
    //                ViewState["QuickCodeType"] = drActivity["FLDQUICKTYPECODE"].ToString();
    //                ViewState["QuickCodeTypeName"] = drActivity["FLDQUICKTYPENAME"].ToString();
    //            }
    //        }
    //    }
    //}

    //public void CaptionChange(string module,string quicktypecode)
    //{
    //    ucQuickType.Enabled = "false";
    //    DataSet dsedit = new DataSet();
    //    dsedit = PhoenixRegistersQuick.ListQuickTypeEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, module, 
    //                                                    Convert.ToInt32(quicktypecode));

    //    if (dsedit.Tables.Count > 0)
    //    {
    //        DataRow drquick = dsedit.Tables[0].Rows[0];
    //        ucTitle.Text = General.GetMixedCase(drquick["FLDQUICKTYPENAME"].ToString());
    //        ViewState["MODULENAME"] = ucTitle.Text;
    //    }
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTNAME", "FLDQUICKNAME", "FLDNCLEVELNAME" };
        string[] alCaptions = { "Code", "Name", "Level" };

        //      if (ViewState["QuickCodeType"].ToString() == "65")
        //{
        //	alCaptions[0] =  "Approval Code";
        //	alCaptions[1] =  "Approval Authority";
        //}		
        //else
        //{
        //	alCaptions[0] = "Code";
        //	alCaptions[1] = "Name";
        //}
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCorrectiveAction.CorrectiveActionSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["QUICKTYPENAME"] = Convert.ToString(dr["FLDQUICKTYPENAME"]);
        }

        if (ViewState["MODULENAME"] != null)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + "Corrective_Actions" + ".xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='https://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>" + ViewState["MODULENAME"].ToString() + "</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

        }
        else
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + "Corrective Actions" + ".xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>" + "Corrective Actions" + "</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

        }
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

    protected void RegistersQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvQuick.EditIndex = -1;
                gvQuick.SelectedIndex = -1;
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
        string[] alColumns = { "FLDSHORTNAME", "FLDQUICKNAME","FLDNCLEVELNAME" };
        string[] alCaptions = { "Code", "Name", "Level" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersCorrectiveAction.CorrectiveActionSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                sortexpression, sortdirection,
                 (int)ViewState["PAGENUMBER"],
                 General.ShowRecords(null),
                 ref iRowCount,
                 ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
        }
        General.SetPrintOptions("gvQuick", "Corrective Actions", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuick.DataSource = ds;
            gvQuick.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvQuick);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    protected void gvQuick_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvQuick_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvQuick.EditIndex = -1;
        gvQuick.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvQuick_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidQuick(((TextBox)_gridView.FooterRow.FindControl("txtQuickNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertQuick(
                    ((TextBox)_gridView.FooterRow.FindControl("txtQuickNameAdd")).Text.Trim(),
                    ((TextBox)_gridView.FooterRow.FindControl("txtShortNameAdd")).Text.Trim(),
                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlLevelAdd")).SelectedValue)
                );
                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtQuickNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteQuick(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuickCode")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("RESEND"))
            {
                PhoenixRegistersCorrectiveAction.ResendCorrectiveAction(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuickCode")).Text));
                //PhoenixRegistersCorrectiveActionLevel.ResendLevels(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMaritalStatusCode")).Text));
            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuick_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvQuick_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvQuick, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvQuick_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtQuickNameEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvQuick_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            UpdateQuick(General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblHardTypeCodeEdit")).Text),
                        Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuickCodeEdit")).Text),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuickNameEdit")).Text.Trim(),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortNameEdit")).Text.Trim(),
                     General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlLevel")).SelectedValue)
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
    protected void gvQuick_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


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
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Request.QueryString["quickcodetype"] != null)
            {
                if (Request.QueryString["quickcodetype"].ToString() == "64")
                {
                    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    {
                        LinkButton lnkCode = (LinkButton)e.Row.FindControl("lnkQuickCodeHeader");
                        if (lnkCode != null) lnkCode.Text = "Cancel Code";

                        LinkButton lnkName = (LinkButton)e.Row.FindControl("lblQuickNameHeader");
                        if (lnkName != null) lnkName.Text = "Cancel Reason";
                    }
                }
                else if (Request.QueryString["quickcodetype"].ToString() == "65")
                {
                    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    {
                        LinkButton lnkCode = (LinkButton)e.Row.FindControl("lnkQuickCodeHeader");
                        if (lnkCode != null) lnkCode.Text = "Approval Code";

                        LinkButton lnkName = (LinkButton)e.Row.FindControl("lblQuickNameHeader");
                        if (lnkName != null) lnkName.Text = "Approval Authority";
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            }
            DropDownList ddlLevel = (DropDownList)e.Row.FindControl("ddlLevel");
            if (ddlLevel != null)
            {
                ddlLevel.DataSource = PhoenixRegistersCorrectiveAction.NCLevellist();
                ddlLevel.DataTextField = "FLDNCLEVELNAME";
                ddlLevel.DataValueField = "FLDNCLEVELID";
                ddlLevel.DataBind();

                ddlLevel.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                if (General.GetNullableInteger(drv["FLDNCLEVELID"].ToString()) != null)
                    ddlLevel.SelectedValue = drv["FLDNCLEVELID"].ToString();

            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlLevelAdd = (DropDownList)e.Row.FindControl("ddlLevelAdd");
            
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            ddlLevelAdd.DataSource = PhoenixRegistersCorrectiveAction.NCLevellist();
            ddlLevelAdd.DataTextField = "FLDNCLEVELNAME";
            ddlLevelAdd.DataValueField = "FLDNCLEVELID";
            ddlLevelAdd.DataBind();

            ddlLevelAdd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        //ViewState["QuickCodeType"] = ucQuickType.SelectedQuickType;
        // CaptionChange(Request.QueryString["module"].ToString(), ViewState["QuickCodeType"].ToString());
        gvQuick.SelectedIndex = -1;
        gvQuick.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvQuick.SelectedIndex = -1;
        gvQuick.EditIndex = -1;
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
        gvQuick.SelectedIndex = -1;
        gvQuick.EditIndex = -1;
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

    private void InsertQuick(string Quickname, string Shortname,int? levelid)
    {

        PhoenixRegistersCorrectiveAction.InsertCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Quickname, Shortname, levelid);
        //PhoenixRegistersQuick.InsertQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickname, Shortname);
    }

    private void UpdateQuick(string Quicktypecode, int Quickcode, string Quickname, string shortname, int? levelid)
    {
        if (!IsValidQuick(Quickname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCorrectiveAction.UpdateCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickcode, Quickname, shortname, levelid);
        //PhoenixRegistersQuick.UpdateQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickcode, Quickname, shortname);
        ucStatus.Text = "Information updated";
    }
    private bool IsValidQuick(string Quickname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvQuick;

        if (Quickname.Trim().Equals(""))
        {

            if (ViewState["QuickCodeType"] != null && ViewState["QuickCodeType"].ToString() != "")
            {
                if (ViewState["QuickCodeType"].ToString() == "64")
                    ucError.ErrorMessage = "Cancel Reason is required.";
                if (ViewState["QuickCodeType"].ToString() == "65")
                    ucError.ErrorMessage = "Approval Status is required.";
                else
                    ucError.ErrorMessage = "Name is required.";
            }
            else
            {
                ucError.ErrorMessage = "Name is required.";
            }
        }


        return (!ucError.IsError);
    }

    private void DeleteQuick(int Quickcode)
    {

        PhoenixRegistersCorrectiveAction.DeleteCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Quickcode);
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


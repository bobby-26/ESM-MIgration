using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class CrewOffshoreCourseCostCBT : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCourseCost.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCourseCost.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Course", "COURSE");
            toolbarmain.AddButton("Cost of Course", "COSTOFCOURSE");
            MnuCourseCost.AccessRights = this.ViewState;
            MnuCourseCost.MenuList = toolbarmain.Show();
            MnuCourseCost.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../CrewOffshore/CrewOffshoreCourseCost.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvCourseCost')", "Print Grid", "icon_print.png", "PRINT");
                MenuCourseCost.AccessRights = this.ViewState;
                MenuCourseCost.MenuList = toolbar.Show();
                MenuCourseCost.SetTrigger(pnlCourseCostEntry);

                BindCourses(ddlCourse);    
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void CourseCost_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;

        if (dce.CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../Registers/RegistersDocumentCourse.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("COSTOFCOURSE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCourseCost.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINSTITUTENAME", "FLDCURRENCYNAME", "FLDCOST", "FLDDURATION" };
        string[] alCaptions = { "Institute", "Currency", "Cost", "Duration (In Days)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreCourseCost.CourseCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(ddlCourse.SelectedValue), null,                               
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CourseCost.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Course Cost</h3></td>");        
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");        
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

    protected void MenuCourseCost_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINSTITUTENAME", "FLDCURRENCYNAME", "FLDCOST", "FLDDURATION" };
        string[] alCaptions = { "Institute","Currency", "Cost","Duration (In Days)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreCourseCost.CourseCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               General.GetNullableInteger(ddlCourse.SelectedValue), null,          
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvCourseCost", "Course Cost", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCourseCost.DataSource = ds;
            gvCourseCost.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCourseCost);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvCourseCost_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCourseCost_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCourseCost.EditIndex = -1;
        gvCourseCost.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        //BindData();
    }

    protected void gvCourseCost_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCourseCost, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvCourseCost_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseCostId")).Text) != null)
                    DeleteCourseCost(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseCostId")).Text));

                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCourseCost(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtCostAdd")).Text
                    , ((UserControlAddressType)_gridView.FooterRow.FindControl("ucInstitutionAdd")).SelectedAddress
                    , ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyAdd")).SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertCourseCost(
                   General.GetNullableInteger(((UserControlAddressType)_gridView.FooterRow.FindControl("ucInstitutionAdd")).SelectedAddress),
                    General.GetNullableInteger(ddlCourse.SelectedValue),
                    General.GetNullableInteger(((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyAdd")).SelectedCurrency),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtCostAdd")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtDurationAdd")).Text)
                    );

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

    protected void gvCourseCost_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCourseCost_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvCourseCost_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidCourseCost(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtCost")).Text
                    , ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress
                    , ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency))
            {
                ucError.Visible = true;
                return;
            }

            UpdateCourseCost(
                Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseCostIdEdit")).Text),
                General.GetNullableInteger(((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress),                
                General.GetNullableInteger(ddlCourse.SelectedValue),
                General.GetNullableInteger(((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency),
                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtCost")).Text),
                General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtDuration")).Text)
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

    protected void gvCourseCost_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlCurrency ucCurrency = (UserControlCurrency)e.Row.FindControl("ucCurrencyEdit");            
            DataRowView drview = (DataRowView)e.Row.DataItem;

            if (ucCurrency != null)
            {
                string CID = drview["FLDCURRENCYID"].ToString();
                ucCurrency.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(1, true);
                ucCurrency.SelectedCurrency = CID; //default : USD
            }

            //DropDownList ddlCourseEdit = (DropDownList)e.Row.FindControl("ddlCourseEdit");
            //if (ddlCourseEdit != null)
            //{
            //    BindCourses(ddlCourseEdit);
            //    ddlCourseEdit.SelectedValue = drview["FLDDOCUMENTID"].ToString();
            //}

            UserControlAddressType ucIns = (UserControlAddressType)e.Row.FindControl("ucInstitutionEdit");
            if (ucIns != null)
            {
                string Ins = drview["FLDADDRESSID"].ToString();
                ucIns.AddressList = PhoenixRegistersAddress.ListAddress("138");
                ucIns.SelectedAddress = Ins;
            }
            LinkButton lb = (LinkButton)e.Row.FindControl("lnkInstituteName");
            //if (lb != null) lb.Text = lb.Text.TrimEnd(trimChar);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {

            //DropDownList ddlCourseAdd = (DropDownList)e.Row.FindControl("ddlCourseAdd");
            //if (ddlCourseAdd != null) BindCourses(ddlCourseAdd);

            UserControlAddressType ucInstitute = (UserControlAddressType)e.Row.FindControl("ucInstitutionAdd");
            if(ucInstitute != null)
            {

            }           
            UserControlCurrency ucCurrencyAdd = (UserControlCurrency)e.Row.FindControl("ucCurrencyAdd");
            if (ucCurrencyAdd != null)
            {
                ucCurrencyAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(1, true);
                ucCurrencyAdd.SelectedCurrency = "10"; //USD
            }
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void BindCourses(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(451); //CBT
        ddl.DataTextField = "FLDDOCUMENTNAME";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvCourseCost.EditIndex = -1;
        gvCourseCost.SelectedIndex = -1;
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
        gvCourseCost.SelectedIndex = -1;
        gvCourseCost.EditIndex = -1;
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

    private void InsertCourseCost(int? Addressid, int? courseid, int? currencyid, decimal? cost, int? duration)
    {
       PhoenixCrewOffshoreCourseCost.InsertCourseCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Addressid, courseid, currencyid, cost, duration);
    }

    private void UpdateCourseCost(int coursecostid,int? Addressid,int? courseid, int? currencyid, decimal? cost, int? duration)
    {
        PhoenixCrewOffshoreCourseCost.UpdateCourseCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , coursecostid, Addressid,courseid, currencyid, cost,duration);
    }

    private bool IsValidCourseCost(string cost, string institute, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlCourse.SelectedValue) == null)
            ucError.ErrorMessage = "Course is required.";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required.";
        
        if (cost == "0.00" || General.GetNullableDecimal(cost) == null)
            ucError.ErrorMessage = "Cost is required.";

        if (General.GetNullableInteger(institute) == null)
            ucError.ErrorMessage = "Institute is required.";

        return (!ucError.IsError);
    }

    private void DeleteCourseCost(int Coursecostid)
    {
       PhoenixCrewOffshoreCourseCost.DeleteCourseCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Coursecostid);
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

    //protected void ucConsulate_TextChanged(object sender, EventArgs e)
    //{
    //    DataSet ds = PhoenixRegistersAddress.EditAddress(1, long.Parse(ucConsulate.SelectedAddress));

    //    ucCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDCLINICCURRENCY"].ToString();
    //}
}

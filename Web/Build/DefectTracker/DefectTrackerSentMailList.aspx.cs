using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DefectTrackerSentMailList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Mapped Items", "MAPPED");
        toolbar.AddButton("Sent Items", "MAILSENT");
        MenuMailSent.AccessRights = this.ViewState;
        MenuMailSent.MenuList = toolbar.Show();

        PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
        toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerSentMailList.aspx", "Search", "search.png", "SEARCH");

        MenuMailManager.AccessRights = this.ViewState;
        MenuMailManager.MenuList = toolbarbuglist.Show();

        if (!IsPostBack)
        {
            ucSentMails.Text = "Mapped Mails";
            ViewState["MAPPEDONLY"] = "1";
            MenuMailSent.SelectedMenuIndex = 0;

            if (Request.QueryString["mappedonly"] == "0")
            {
                ucSentMails.Text = "Sent Mails";              
                ViewState["MAPPEDONLY"] = "0";
                MenuMailSent.SelectedMenuIndex = 1;
            }

            if (Request.QueryString["patchdtkey"] != null)
            {
                ViewState["PATCHDTKEY"] = Request.QueryString["patchdtkey"].ToString();
            }

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

        }
        
        BindData();
        SetPageNavigator();
    }

    protected void MenuMailManager_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindData();
            SetPageNavigator();
        }
    }
    protected void Vessel_Changed(object sender, EventArgs e)
    {
        int result = 0;
        Int32.TryParse(UcVessel.SelectedVessel, out result);

        DataTable dt = PhoenixMailManager.MailIdofVessel(result);
        txtTo.Text = "";
        if (dt.Rows.Count > 0)
            txtTo.Text = dt.Rows[0]["FLDEMAIL"].ToString();

        BindData();
        SetPageNavigator();
    }
    protected void Filter_Changed(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void Module_Changed(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixMailManager.PatchMailSentList(
            General.GetNullableString("SEP"),
            General.GetNullableString(""),
            General.GetNullableString(txtTo.Text),
            General.GetNullableString(""),
            General.GetNullableString(""),
            General.GetNullableString(txtSubjectText.Text),
            General.GetNullableString(txtBodyText.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            (0),
            General.GetNullableString(""),
            General.GetNullableGuid((ViewState["PATCHDTKEY"] == null) ? "" : ViewState["PATCHDTKEY"].ToString()),
            General.GetNullableInteger((ViewState["VESSELID"] == null) ? "" : ViewState["VESSELID"].ToString()),
            short.Parse(ViewState["MAPPEDONLY"].ToString()),
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            MailManager.DataSource = ds;
            MailManager.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, MailManager);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
    protected void MailManager_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            Label lbl = (Label)e.Row.FindControl("lblUniqueID");
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);                  

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucSubject");
            LinkButton lb = (LinkButton)e.Row.FindControl("lnkSubject");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

            UserControlToolTip lblToList = (UserControlToolTip)e.Row.FindControl("lblToList");
            Label lblTo = (Label)e.Row.FindControl("lblTo");
            lblTo.Attributes.Add("onmouseover", "showTooltip(ev,'" + lblToList.ToolTip + "', 'visible');");
            lblTo.Attributes.Add("onmouseout", "showTooltip(ev,'" + lblToList.ToolTip + "', 'hidden');");

            UserControlToolTip lblCcList = (UserControlToolTip)e.Row.FindControl("lblCcList");
            Label lblCc = (Label)e.Row.FindControl("lblCc");
            lblCc.Attributes.Add("onmouseover", "showTooltip(ev,'" + lblCcList.ToolTip + "', 'visible');");
            lblCc.Attributes.Add("onmouseout", "showTooltip(ev,'" + lblCcList.ToolTip + "', 'hidden');");
        }

    }
    protected void MailManager_Sorting(object sender, GridViewSortEventArgs se)
    {
        MailManager.EditIndex = -1;
        MailManager.SelectedIndex = -1;
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
        MailManager.EditIndex = -1;
        MailManager.SelectedIndex = -1;
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
        MailManager.SelectedIndex = -1;
        MailManager.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void MailManager_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();
    }
    protected void MailManager_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }

    protected void MailManager_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            Label lblMailOutId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID");


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixMailManager.MailDelete(new Guid(lblMailOutId.Text));
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Response.Redirect("DefectTrackerMailRead.aspx?sentitem=1&mailid=" + lblMailOutId.Text, false);
            }

            if (e.CommandName.ToUpper().Equals("MAP"))
            {
                
                PhoenixDefectTracker.MapPatchToVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        new Guid(ViewState["PATCHDTKEY"].ToString()),
                                                        lblMailOutId.Text,
                                                        int.Parse(ViewState["VESSELID"].ToString())
                                                        );
                ucStatus.Text = (ViewState["MAPPEDONLY"].ToString() == "1") ? "Mail unmapped from patch" : "Mail mapped to patch";
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

    private void ClearFiter()
    {
        txtBodyText.Text = "";
        txtSubjectText.Text = "";
        ucFromDate.Text = "";
        txtTo.Text = "";
        ucToDate.Text = "";
        UcVessel.SelectedVessel = "";
    }

    protected void MenuMailSent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("MAPPED"))
            {
                Response.Redirect("DefectTrackerSentMailList.aspx?mappedonly=1&patchdtkey=" + ViewState["PATCHDTKEY"].ToString() + "&vesselid=" + ViewState["VESSELID"].ToString());
            }
            if (dce.CommandName.ToUpper().Equals("MAILSENT"))
            {
                Response.Redirect("DefectTrackerSentMailList.aspx?mappedonly=0&patchdtkey=" + ViewState["PATCHDTKEY"].ToString() + "&vesselid=" + ViewState["VESSELID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}

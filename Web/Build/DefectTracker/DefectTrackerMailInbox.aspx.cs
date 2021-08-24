using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DefectTrackerMailInbox : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in MailManager.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(MailManager.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Inbox", "INBOX");
        toolbar.AddButton("Sent Items", "MAILSENT");
        MenuMailInbox.AccessRights = this.ViewState;
        MenuMailInbox.MenuList = toolbar.Show();
        MenuMailInbox.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
        toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerMailInbox.aspx", "Search", "search.png", "SEARCH");
        toolbarbuglist.AddImageLink("javascript:parent.Openpopup('MoreInfo','','DefectTrackerMailCallLog.aspx?'); return false;", "Add", "61.png", "ADD");

        MenuMailManager.AccessRights = this.ViewState;
        MenuMailManager.MenuList = toolbarbuglist.Show();

        NameValueCollection lastmail = Filter.CurrentMailSelectionFilter;

        if (Request.QueryString["showlastmail"] != null)
            ViewState["PAGENUMBER"] = General.GetNullableInteger(lastmail.Get("PAGENUMBER").ToString());
        else
            ViewState["PAGENUMBER"] = 1;

        if (!IsPostBack)
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
            Filter.CurrentMailSelectionFilter = criteria;
        }

        if (Filter.CurrentMailSelectionFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentMailSelectionFilter;
            ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
        }
        else
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();
            criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
            Filter.CurrentMailSelectionFilter = criteria;
        }

        BindData();
        SetPageNavigator();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentMailSelectionFilter;

        try
        {
            ViewState["PAGENUMBER"] = int.Parse(nvc.Get("PAGENUMBER"));
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMailManager_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
            Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
        }
    }

    protected void Vessel_Changed(object sender, EventArgs e)
    {
        int result = 0;
        Int32.TryParse(UcVessel.SelectedVessel, out result);

        DataTable dt = PhoenixMailManager.MailIdofVessel(result);
        txtFrom.Text = "";
        if (dt.Rows.Count > 0)
        {
            txtFrom.Text = dt.Rows[0]["FLDEMAIL"].ToString();
        }

        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
        Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
    }

    protected void VesselTo_Changed(object sender, EventArgs e)
    {
        int result = 0;
        Int32.TryParse(ucVesselTo.SelectedVessel, out result);

        DataTable dt = PhoenixMailManager.MailIdofVessel(result);
        txtTo.Text = "";
        if (dt.Rows.Count > 0)
            txtTo.Text = dt.Rows[0]["FLDEMAIL"].ToString();

        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
        Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
    }

    protected void MailManager_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(MailManager, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Filter_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
        Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
    }

    protected void Module_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
        Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if ((int)ViewState["PAGENUMBER"] > 0)
            Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());

        NameValueCollection nvc = Filter.CurrentMailSelectionFilter;

        DataSet ds = PhoenixMailManager.MailList(
            General.GetNullableString(ucSEPMailUsername.SelectedValue),
            General.GetNullableString(txtFrom.Text),
            General.GetNullableString(txtTo.Text),
            General.GetNullableString(txtCc.Text),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString(txtSubjectText.Text),
            General.GetNullableString(txtBodyText.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            (chkArchived.Checked ? 0 : 1),
            (chkResponse.Checked ? 1 : 0),
            General.GetNullableString(txtCallNumber.Text),
            (chkphoenixit.Checked ? 1 : 0),
            int.Parse(nvc.Get("PAGENUMBER").ToString()),
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
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton attachments = (ImageButton)e.Row.FindControl("Attachments");
            if (drv["FLDATTACHMENTCOUNT"].ToString() == "0")
                attachments.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                attachments.ImageUrl = Session["images"] + "/attachment.png";

            ImageButton CallStatus = (ImageButton)e.Row.FindControl("CallStatus");
            if (drv["FLDACTIVEYN"].ToString() == "1")
            {
                CallStatus.ImageUrl = Session["images"] + "/green.png";
                CallStatus.ToolTip = "Call Status : Closed";
            }
            else
            {
                CallStatus.ImageUrl = Session["images"] + "/red.png";
                CallStatus.ToolTip = "Call Status : Open";
            }

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?mailid=" + lbl.Text + "'); return false;");
            }

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAttachment");
            if (ab != null) ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
            if (ab != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                ab.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugAttachment.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            LinkButton lb = (LinkButton)e.Row.FindControl("lnkSubject");
            if (lb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?mailid=" + lbl.Text + "'); return false;");
            }

            ImageButton anl = (ImageButton)e.Row.FindControl("cmdArcheiveHistory");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                anl.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerArchievedEmail.aspx?mailid=" + lbl.Text + "'); return false;");
            }

            ImageButton log = (ImageButton)e.Row.FindControl("cmdLog");
            if (log != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (log != null)
            {
                Label callnumber = (Label)e.Row.FindControl("lblCallNumber");
                log.Attributes.Add("onclick", "javascript:parent.Openpopup('MoreInfo','', 'DefectTrackerMailCallLog.aspx?callnumber=" + callnumber.Text + "'); return false;");
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucSubject");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");


            ((Label)e.Row.FindControl("lblMessageOut")).Visible = true;
            ((LinkButton)e.Row.FindControl("lnkMessageOut")).Visible = false;
            if (!drv["FLDRESPONSECOUNT"].ToString().Equals("0"))
            {
                ((Label)e.Row.FindControl("lblMessageOut")).Visible = false;
                ((LinkButton)e.Row.FindControl("lnkMessageOut")).Visible = true;
            }

            if (!drv["FLDREADYN"].ToString().Equals("1"))
            {
                LinkButton lbsubject = (LinkButton)e.Row.FindControl("lnkSubject");
                lbsubject.Font.Bold = true;
            }

            ImageButton arc = (ImageButton)e.Row.FindControl("cmdDelete");
            if (arc != null) arc.Visible = SessionUtil.CanAccess(this.ViewState, arc.CommandName);
            if (arc != null)
            {
                Label lblmailid = (Label)e.Row.FindControl("lblUniqueID");
                arc.Attributes.Add("onclick", "Openpopup('codehelp1','','DefectTrackerArchievedEmail.aspx?mailid=" + lblmailid.Text + "&savemode=1');return false;");
            }
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
        //Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
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

        Filter.CurrentMailSelectionFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
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

            if (e.CommandName.ToUpper().Equals("MAILSENT"))
            {
                string messageid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMessageId")).Text;
                Response.Redirect("DefectTrackerMailSent.aspx?messageid=" + messageid, false);
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //PhoenixMailManager.MailDelete(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID")).Text));
                //PhoenixDefectTracker.InsertArcheiveHistory(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID")).Text),Request.ServerVariables["REMOTE_ADDR"].ToString(), Guid.NewGuid());
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

    protected void MenuMailInbox_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("MAILSENT"))
                Response.Redirect("DefectTrackerMailSent.aspx", false);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}

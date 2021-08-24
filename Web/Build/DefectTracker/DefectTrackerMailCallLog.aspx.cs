using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;

public partial class DefectTrackerMailCallLog : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in MailCallLog.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(MailCallLog.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void rbtnlstImport_TextChanged(object sender, EventArgs e)
    {
        UpdateCallNumber();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuCallNumber.AccessRights = this.ViewState;
        MenuCallNumber.MenuList = toolbar.Show();

        PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
        toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerMailCallLog.aspx", "Search", "search.png", "SEARCH");

        MenuMailCallLog.AccessRights = this.ViewState;
        MenuMailCallLog.MenuList = toolbarbuglist.Show();
        ViewState["CALLNUMBER"] = txtCallNumber.Text;
        if (!IsPostBack)
        {
            if (Request.QueryString["callnumber"] != null)
                ViewState["CALLNUMBER"] = Request.QueryString["callnumber"].ToString();
            txtCallNumber.Text = ViewState["CALLNUMBER"].ToString();
            BindImportance();
            BindData();
        }
    }

    protected void MenuCallNumber_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateCallNumber();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void UpdateCallNumber()
    {
        PhoenixDefectTracker.CallImportInsertUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                           , ViewState["CALLNUMBER"].ToString()
                                           , rbtnlstImportance.SelectedValue.ToString()
                                           , short.Parse(rbtnCallStatus.SelectedValue.ToString())
                                           , txtRemarks.Text
                                           , txtClosedBy.Text
                                           );
        BindData();
    }
    private void BindImportance()
    {
        DataTable dt = new DataTable();
        dt = PhoenixDefectTracker.CallImportanceSearch(ViewState["CALLNUMBER"].ToString());
        if (dt.Rows.Count > 0)
        {
            rbtnlstImportance.SelectedValue = dt.Rows[0]["FLDIMPORTANCE"].ToString();
            rbtnCallStatus.SelectedValue = dt.Rows[0]["FLDACTIVEYN"].ToString();
            txtClosedBy.Text = dt.Rows[0]["FLDCALLCLOSEDBY"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void MenuMailCallLog_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["CALLNUMBER"] = txtCallNumber.Text;
            BindData();
        }
    }

    protected void MailCallLog_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(MailCallLog, "Select$" + e.Row.RowIndex.ToString(), false);
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
        int vesselid = -1;
        if (Request.QueryString["vesselid"] != null)
            vesselid = int.Parse(Request.QueryString["vesselid"].ToString());

        DataSet ds = new DataSet();
        if (vesselid <= 0)
        {
            ds = PhoenixDefectTracker.CallNumberList(ViewState["CALLNUMBER"].ToString());
        }
        else
        {
            ds = PhoenixDefectTracker.CallNumberList(ViewState["CALLNUMBER"].ToString(), vesselid);
        }

            if (ds.Tables[0].Rows.Count > 0)
            {
                MailCallLog.DataSource = ds;
                MailCallLog.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, MailCallLog);
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

    protected void MailCallLog_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            ImageButton attachments = (ImageButton)e.Row.FindControl("cmdAttachment");
            if (drv["FLDATTACHMENTCOUNT"].ToString() == "0")
            {
                attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                attachments.Enabled = false;
            }
            else
                attachments.ImageUrl = Session["images"] + "/attachment.png";


            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                if (drv["FLDSENTORRECEIVE"].ToString().ToUpper() == "SENT")
                    eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?sentitem=1&mailid=" + lbl.Text + "'); return false;");
                else
                    eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?mailid=" + lbl.Text + "'); return false;");
            }

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAttachment");
            if (ab != null) ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
            if (ab != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                ab.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailAttachments.aspx?mailinoutid=" + drv["FLDMAILINOUTID"].ToString() + "'); return false;");
            }

            LinkButton lb = (LinkButton)e.Row.FindControl("lnkSubject");
            if (lb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    if (drv["FLDFROM"].ToString() == "SEP")
                        lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?sentitem=1&mailid=" + lbl.Text + "'); return false;");
                    else
                        lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?mailid=" + lbl.Text + "'); return false;");
            }

            ImageButton anl = (ImageButton)e.Row.FindControl("cmdArcheiveHistory");
            anl.Visible = false;
            if (drv["FLDARCHIVEORNOT"].ToString().ToUpper().Equals("YES"))
                anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                anl.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerArchievedEmail.aspx?mailid=" + lbl.Text + "'); return false;");
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucSubject");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

            if (!drv["FLDREADSENTYN"].ToString().Equals("1"))
            {
                LinkButton lbsubject = (LinkButton)e.Row.FindControl("lnkSubject");
                lbsubject.Font.Bold = true;
            }

            ImageButton arc = (ImageButton)e.Row.FindControl("cmdDelete");
            if (arc != null) arc.Visible = SessionUtil.CanAccess(this.ViewState, arc.CommandName);
            if (arc != null)
            {
                Label lblmailid = (Label)e.Row.FindControl("lblUniqueID");
                arc.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','DefectTrackerArchievedEmail.aspx?mailid=" + lblmailid.Text + "&savemode=1');");
            }
        }
    }

    protected void MailCallLog_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }

    protected void MailCallLog_RowCommand(object sender, GridViewCommandEventArgs e)
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
                //PhoenixMailCallLog.MailDelete(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID")).Text));
                //PhoenixDefectTracker.InsertArcheiveHistory(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID")).Text),Request.ServerVariables["REMOTE_ADDR"].ToString(), Guid.NewGuid());
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

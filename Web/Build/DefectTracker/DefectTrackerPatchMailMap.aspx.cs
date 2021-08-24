using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;
using System.Drawing;

public partial class DefectTrackerPatchMailMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");

        PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
        toolbarbuglist.AddImageButton(General.RedirectTo("../DefectTracker/DefectTrackerPatchMailMap.aspx"), "Search", "search.png", "SEARCH");

        MenuMailCallLog.AccessRights = this.ViewState;
        MenuMailCallLog.MenuList = toolbarbuglist.Show();
        if (!IsPostBack)
            txtCallNumber.Text = (Request.QueryString["callnumber"] != null) ? Request.QueryString["callnumber"].ToString() : txtCallNumber.Text;

        BindData();
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
            BindData();
        }
    }


    private void BindData()
    {
        int vesselid = -1;
        vesselid = (Request.QueryString["vesselid"] != null) ? int.Parse(Request.QueryString["vesselid"].ToString()) : vesselid;

        DataSet ds = new DataSet();
        ds = PhoenixDefectTracker.CallNumber2Map(txtCallNumber.Text, 
                                                 txtvesselmailsearch.Text,
                                                 vesselid);
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


            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucSubject");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

            ImageButton arc = (ImageButton)e.Row.FindControl("cmdDelete");
            if (arc != null) arc.Visible = SessionUtil.CanAccess(this.ViewState, arc.CommandName);
            if (arc != null)
            {
                Label lblmailid = (Label)e.Row.FindControl("lblUniqueID");
                arc.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','DefectTrackerArchievedEmail.aspx?mailid=" + lblmailid.Text + "&savemode=1');");
            }

            Label lblitemfrom = (Label)e.Row.FindControl("lblitemfrom");
            UserControlToolTip ucFromtooltip = (UserControlToolTip)e.Row.FindControl("ucitemfrom");
            lblitemfrom.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucFromtooltip.ToolTip + "', 'visible');");
            lblitemfrom.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucFromtooltip.ToolTip + "', 'hidden');");

            Label lblitemto = (Label)e.Row.FindControl("lblitemto");
            UserControlToolTip ucTotooltip = (UserControlToolTip)e.Row.FindControl("ucitemto");
            lblitemto.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucTotooltip.ToolTip + "', 'visible');");
            lblitemto.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucTotooltip.ToolTip + "', 'hidden');");
                                      
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

            if (e.CommandName.ToUpper().Equals("MAPMAIL"))
            {
                string messageid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMessageId")).Text;
                int vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
                string dtkey = Request.QueryString["dtkey"].ToString();
                string callnumber = txtCallNumber.Text;
                PhoenixPatchTracker.MapMail2Patch(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(dtkey), messageid, vesselid, callnumber);
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
}

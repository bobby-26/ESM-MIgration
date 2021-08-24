using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersMiscellaneousBriefingTopic : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousBriefingTopic.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBriefingTopics')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersBriefingTopics.AccessRights = this.ViewState;
        MenuRegistersBriefingTopics.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvBriefingTopics.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDSUBJECT", "FLDGROUP" };
        string[] alCaptions = { "Subject", "Group/Category" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersMiscellaneousBriefingTopic.MiscellaneousBriefingTopicSearch("", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvBriefingTopics.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BriefingTopic.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Briefing Topics</h3></td>");
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

    protected void RegistersBriefingTopics_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvBriefingTopics.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        string[] alColumns = { "FLDSUBJECT", "FLDGROUP" };
        string[] alCaptions = { "Subject", "Group/Category" };
       
        DataSet ds = PhoenixRegistersMiscellaneousBriefingTopic.MiscellaneousBriefingTopicSearch("", "", sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], gvBriefingTopics.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvBriefingTopics", "Briefing Topics", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBriefingTopics.DataSource = ds;
            gvBriefingTopics.VirtualItemCount = iRowCount;
        }
        else
        {
            gvBriefingTopics.DataSource = "";
        }
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvBriefingTopics.Rebind();
    }

    private void InsertBriefingTopics(string Subject, string group)
    {
        if (!IsValidBriefing(Subject, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousBriefingTopic.InsertMiscellaneousBriefingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Subject, group);
    }
    private void UpdateBriefingTopics(int Subjectid, string Subject, string group)
    {
        if (!IsValidBriefing(Subject, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousBriefingTopic.UpdateMiscellaneousBriefingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Subjectid, Subject, group);
        ucStatus.Text = "Briefing Topics information updated";
    }
    private void DeleteBriefingTopics(int Subjectid)
    {
        PhoenixRegistersMiscellaneousBriefingTopic.DeleteMiscellaneousBriefingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Subjectid);       
    }


    private bool IsValidBriefing(string Subject, string group)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (Subject.Trim().Equals(""))
            ucError.ErrorMessage = "Subject is required.";
        if (group.Trim().Equals(""))
            ucError.ErrorMessage = "Group is required.";

        return (!ucError.IsError);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvBriefingTopics_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertBriefingTopics(
                ((RadTextBox)e.Item.FindControl("txtSubjectAdd")).Text,
                ((RadTextBox)e.Item.FindControl("txtGroupAdd")).Text);
            BindData();
            gvBriefingTopics.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateBriefingTopics(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblSubjectIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtSubjectEdit")).Text,
                ((RadTextBox)e.Item.FindControl("txtGroupEdit")).Text);          
            BindData();
            gvBriefingTopics.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteBriefingTopics(Int32.Parse(((RadLabel)e.Item.FindControl("lblSubjectId")).Text));
            BindData();
            gvBriefingTopics.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            UpdateBriefingTopics(
               Int16.Parse(((RadLabel)e.Item.FindControl("lblSubjectIdEdit")).Text),
               ((RadTextBox)e.Item.FindControl("txtSubjectEdit")).Text,
               ((RadTextBox)e.Item.FindControl("txtGroupEdit")).Text);
            BindData();
            gvBriefingTopics.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvBriefingTopics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBriefingTopics.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBriefingTopics_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (((LinkButton)e.Item.FindControl("cmdViewContent") == null) ? new LinkButton() : (LinkButton)e.Item.FindControl("cmdViewContent"));
            RadLabel lblSubjectId = (RadLabel)e.Item.FindControl("lblSubjectId");
            string subjectid = string.Empty;
            string subjectname=string.Empty;
            if (lblSubjectId != null)
                subjectid = lblSubjectId.Text;
            RadLabel lblSubjectName = (((RadLabel)e.Item.FindControl("lblSubject") == null) ? new RadLabel() : (RadLabel)e.Item.FindControl("lblSubject"));
            if (lblSubjectName != null)
                subjectname = lblSubjectName.Text; 
            db.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersMiscellaneousBriefingContent.aspx?SubjectId=" + subjectid + "&SubjectName=" + subjectname + "');return false;");
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }        
    }

    protected void gvBriefingTopics_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        gvBriefingTopics.Rebind();
    }
}

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersMiscellaneousOnBoardTrainingTopic : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousOnBoardTrainingTopic.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOnBoardTrainingTopics')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersOnBoardTrainingTopics.AccessRights = this.ViewState;
        MenuRegistersOnBoardTrainingTopics.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvOnBoardTrainingTopics.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        DataSet ds = PhoenixRegistersMiscellaneousOnBoardTrainingTopic.MiscellaneousOnBoardTrainingTopicSearch("", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvOnBoardTrainingTopics.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MiscellaneousOnBoardTrainingTopic.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Onboard Training</h3></td>");
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

    protected void RegistersOnBoardTrainingTopics_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvOnBoardTrainingTopics.Rebind();
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

        string[] alColumns = { "FLDSUBJECT", "FLDGROUP" };
        string[] alCaptions = { "Subject", "Group/Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        DataSet ds = PhoenixRegistersMiscellaneousOnBoardTrainingTopic.MiscellaneousOnBoardTrainingTopicSearch("", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvOnBoardTrainingTopics.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOnBoardTrainingTopics", "Onboard Training", alCaptions, alColumns, ds);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOnBoardTrainingTopics.DataSource = ds;
            gvOnBoardTrainingTopics.VirtualItemCount = iRowCount;
        }
        else
        {
            gvOnBoardTrainingTopics.DataSource = "";
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        gvOnBoardTrainingTopics.Rebind();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvOnBoardTrainingTopics.Rebind();
    }

    private void InsertOnBoardTrainingTopics(string Subject, string group)
    {
        if (!IsValidOnBoardTraining(Subject, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousOnBoardTrainingTopic.InsertMiscellaneousOnBoardTrainingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Subject, group);
    }

    private void UpdateOnBoardTrainingTopics(int Subjectid, string Subject, string group)
    {
        if (!IsValidOnBoardTraining(Subject, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousOnBoardTrainingTopic.UpdateMiscellaneousOnBoardTrainingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Subjectid, Subject, group);
        ucStatus.Text = "Onboard Training information updated";
    }

    private void DeleteOnBoardTrainingTopics(int Subjectid)
    {
        PhoenixRegistersMiscellaneousOnBoardTrainingTopic.DeleteMiscellaneousOnBoardTrainingTopic(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Subjectid);
    }

    private bool IsValidOnBoardTraining(string Subject, string group)
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

    protected void gvOnBoardTrainingTopics_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertOnBoardTrainingTopics(
                ((RadTextBox)e.Item.FindControl("txtSubjectAdd")).Text,
                ((RadTextBox)e.Item.FindControl("txtGroupAdd")).Text);
            BindData();
            gvOnBoardTrainingTopics.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateOnBoardTrainingTopics(
                Int16.Parse(((RadLabel)e.Item.FindControl("lblSubjectIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtSubjectEdit")).Text,
                ((RadTextBox)e.Item.FindControl("txtGroupEdit")).Text);
            BindData();
            gvOnBoardTrainingTopics.Rebind();
            ucStatus.Text = "Onboard Training information Saved";
        }

        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteOnBoardTrainingTopics(Int32.Parse(((RadLabel)e.Item.FindControl("lblSubjectId")).Text));
            BindData();
            gvOnBoardTrainingTopics.Rebind();
            ucStatus.Text = "Onboard Training information Deleted";
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            UpdateOnBoardTrainingTopics(
                      Int16.Parse(((RadLabel)e.Item.FindControl("lblSubjectIdEdit")).Text),
                      ((RadTextBox)e.Item.FindControl("txtSubjectEdit")).Text,
                      ((RadTextBox)e.Item.FindControl("txtGroupEdit")).Text);
            BindData();
            gvOnBoardTrainingTopics.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvOnBoardTrainingTopics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOnBoardTrainingTopics.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOnBoardTrainingTopics_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db1 = (((LinkButton)e.Item.FindControl("cmdViewContent") == null) ? new LinkButton() : (LinkButton)e.Item.FindControl("cmdViewContent"));
            RadLabel lblSubjectId = (((RadLabel)e.Item.FindControl("lblSubjectId") == null) ? (RadLabel)e.Item.FindControl("lblSubjectIdEdit") : (RadLabel)e.Item.FindControl("lblSubjectId"));
            string subjectid = lblSubjectId.Text;
            RadLabel lblSubjectName = (((RadLabel)e.Item.FindControl("lblSubject") == null) ? new RadLabel() : (RadLabel)e.Item.FindControl("lblSubject"));
            string subjectname = lblSubjectName.Text;
            if (db1 != null)
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersMiscellaneousOnBoardTrainingContent.aspx?SubjectId=" + subjectid + "&SubjectName=" + subjectname + "');return false;");
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
        
    protected void gvOnBoardTrainingTopics_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();

    }
}

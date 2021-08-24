using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Southnests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderPostponementQuestion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPostponementQuestion.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrderQuestions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','PlannedMaintenance/PlannedMaintenanceWorkOrderPostponementQuestionAdd.aspx?type=ADD&QuestionId=')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRAPostponementQuestions.AccessRights = this.ViewState;
            MenuRAPostponementQuestions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkOrderQuestions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrderQuestions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrderQuestions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvWorkOrderQuestions.SelectedIndexes.Clear();
        gvWorkOrderQuestions.EditIndexes.Clear();
        gvWorkOrderQuestions.DataSource = null;
        gvWorkOrderQuestions.Rebind();
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDQUESTIONNO", "FLDQUESTIONNAME", "FLDISCOMMENTYNSTATUS", "FLDISACTIVEYNSTATUS" };
        string[] alCaptions = { "S.No", "Question", "Comments Y/N", "Active Y/N" };

        DataSet ds = PhoenixPlannedMaintenanceWOPostponementQuestion.RAPostponementQuestions(
            sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvWorkOrderQuestions.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWorkOrderQuestions.DataSource = ds;
            gvWorkOrderQuestions.VirtualItemCount = iRowCount;
        }
        else
        {
            gvWorkOrderQuestions.DataSource = "";
        }

        General.SetPrintOptions("gvWorkOrderQuestions", "Work Order Postponement Questions", alCaptions, alColumns, ds);
    }
    protected void MenuRAPostponementQuestions_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDQUESTIONNO", "FLDQUESTIONNAME", "FLDISCOMMENTYNSTATUS", "FLDISACTIVEYNSTATUS" };
        string[] alCaptions = { "S.No", "Question", "Comments Y/N", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceWOPostponementQuestion.RAPostponementQuestions(
            sortexpression
            , sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=WorkOrderPostponementQuestions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Work Order Postponement Questions</h3></td>");
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

    protected void gvWorkOrderQuestions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceWOPostponementQuestion.DeleteRAPostponementQuestions(new Guid(((RadLabel)e.Item.FindControl("lblQuestionId")).Text));
                //ucStatus.Text = "Question deleted successfully.";
                RadWindowManager1.RadAlert("Question deleted successfully.", 320, 150, null, "");
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                e.Item.Selected = true; 
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                e.Item.Selected = true;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrderQuestions_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string sQuestionId = ((RadLabel)e.Item.FindControl("lblQuestionId")).Text;
            string sQuestionNo = ((RadLabel)e.Item.FindControl("lblOrder")).Text;
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderPostponementQuestionAdd.aspx?type=EDIT&QuestionId=" + sQuestionId + "');");
            }
            LinkButton cmdOption = (LinkButton)e.Item.FindControl("cmdOption");
            if (cmdOption != null)
            {
                cmdOption.Visible = SessionUtil.CanAccess(this.ViewState, cmdOption.CommandName);
                cmdOption.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderPostponementQuestionOption.aspx?QuestionId=" + sQuestionId + "&QuestionNo=" + sQuestionNo + "');");
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrderQuestions_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
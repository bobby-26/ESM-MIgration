using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionRAActivityExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAActivityExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAActivityAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRAActivity.AccessRights = this.ViewState;
            MenuRAActivity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRAActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindGrid()
    {
        gvRAActivity.SelectedIndexes.Clear();
        gvRAActivity.EditIndexes.Clear();
        gvRAActivity.DataSource = null;
        gvRAActivity.Rebind();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAME", "FLDCATEGORYNAME", "FLDEVENTLIST" };
        string[] alCaptions = { "Name", "Process", "Event" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentActivityExtn.RiskAssessmentActivitySearch(General.GetNullableString(null),
            General.GetNullableInteger(ucCategory.SelectedCategory),
            sortexpression, sortdirection
            , 1
            , iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(null));
        Response.AddHeader("Content-Disposition", "attachment; filename=Activity.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Activity</h3></td>");
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

    protected void RAActivity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvRAActivity.Rebind();
            }
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDCATEGORYNAME", "FLDEVENTLIST" };
        string[] alCaptions = { "Name", "Process", "Event" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentActivityExtn.RiskAssessmentActivitySearch(General.GetNullableString(null),
            General.GetNullableInteger(ucCategory.SelectedCategory),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRAActivity.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(null));


        General.SetPrintOptions("gvRAActivity", "Activity", alCaptions, alColumns, ds);
        gvRAActivity.DataSource = ds;
        gvRAActivity.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount ;
    }

    protected void gvRAActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteRAActivity(Int32.Parse(((RadLabel)e.Item.FindControl("lblActivityId")).Text));
                BindGrid();
            }

            if (e.CommandName == "Page")
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

    protected void gvRAActivity_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvRAActivity.Rebind();
    }

    protected void gvRAActivity_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindGrid();
    }
    protected void gvRAActivity_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadLabel lblActivityId = (RadLabel)e.Item.FindControl("lblActivityId");

            if (eb != null)
                eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAActivityAdd.aspx?ACTIVITYID=" + lblActivityId.Text + "'); return true;");


            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton cmdRankGroupMapping = (LinkButton)e.Item.FindControl("cmdRankGroupMapping");
            if (cmdRankGroupMapping != null) cmdRankGroupMapping.Visible = SessionUtil.CanAccess(this.ViewState, cmdRankGroupMapping.CommandName);

            if (cmdRankGroupMapping != null)
            {
                cmdRankGroupMapping.Visible = SessionUtil.CanAccess(this.ViewState, cmdRankGroupMapping.CommandName);
                cmdRankGroupMapping.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionProcessRATeamComposition.aspx?Activityid=" + drv["FLDACTIVITYID"].ToString() + "&ActivityName=" + drv["FLDNAME"].ToString() + "');return false;");
            }
        }
    }

    private void DeleteRAActivity(int Activityid)
    {
        PhoenixInspectionRiskAssessmentActivityExtn.DeleteRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Activityid);
    }

    protected void gvRAActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAActivity.CurrentPageIndex + 1;
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRAActivity.Rebind();
    }

    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindGrid();
    }
}
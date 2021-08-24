using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionInvestigationQuestions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionInvestigationQuestions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInvestigationQuestions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionIncidentInvestigationQuestionAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuInvestigationQuestions.AccessRights = this.ViewState;
            MenuInvestigationQuestions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvInvestigationQuestions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDQUESTION", "FLDFINDINGTEXT", "FLDACCIDENTCATEGORYLIST", "FLDNEARMISSCATEGORYLIST" };
        string[] alCaptions = { "Question", "Finding Text", "Accident Category", "Near Miss Category" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionInvestigationQuestions.InvestigationQuestionSearch(null, null, sortexpression, sortdirection,
                                    1,
                                    iRowCount,
                                    ref iRowCount,
                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionInvestigationQuestions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Investigation Questions</h3></td>");
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

    protected void MenuInvestigationQuestions_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvInvestigationQuestions.SelectedIndexes.Clear();
        gvInvestigationQuestions.EditIndexes.Clear();
        gvInvestigationQuestions.DataSource = null;
        gvInvestigationQuestions.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUESTION", "FLDFINDINGTEXT", "FLDACCIDENTCATEGORYLIST", "FLDNEARMISSCATEGORYLIST" };
        string[] alCaptions = { "Question", "Finding Text", "Accident Category", "Near Miss Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixInspectionInvestigationQuestions.InvestigationQuestionSearch(null, null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvInvestigationQuestions", "Investigation Questions", alCaptions, alColumns, ds);

        gvInvestigationQuestions.DataSource = ds;
        gvInvestigationQuestions.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvInvestigationQuestions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionInvestigationQuestions.InvestigationQuestionDelete(new Guid(((RadLabel)e.Item.FindControl("lblQuestionId")).Text.ToString()));
                ucStatus.Text = "Information Deleted.";
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvInvestigationQuestions_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");

            if (eb != null)
                eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionIncidentInvestigationQuestionAdd.aspx?QUESTIONID=" + lblQuestionId.Text + "'); return true;");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvInvestigationQuestions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvestigationQuestions.CurrentPageIndex + 1;

        BindData();
    }
}

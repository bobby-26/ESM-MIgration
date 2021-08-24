using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionPEARSRAConsequenceImpact : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRAConsequenceImpact.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAImpact')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRAConsequenceImpact.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRAConsequenceImpact.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Impact','Consequence Impact Add','" + Session["sitepath"] + "/Inspection/InspectionPEARSRAConsequenceImpactAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuRAImpact.AccessRights = this.ViewState;
            MenuRAImpact.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvRAImpact.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindHazardCategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDHAZARDCATEGORYNAME", "FLDCONSEQUENCEIMPACT", "FLDSEVERITYNAME", "FLDSEVERITYSCORE", "FLDACTIVEYN" };
        string[] alCaptions = { "Hazard Category", "Consequence Impact", "Severity", "Score", "ActiveYN" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPEARSRiskassessmentConsequenceImpact.RAConsequenceImpactSearch(General.GetNullableInteger(ddlHazard.SelectedValue)
            , General.GetNullableString(txtName.Text)
            , sortdirection
            , sortexpression
            , gvRAImpact.CurrentPageIndex + 1
            , gvRAImpact.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvRAImpact", "ConsequenceImpact", alCaptions, alColumns, ds);

        gvRAImpact.DataSource = ds;
        gvRAImpact.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvRAImpact.SelectedIndexes.Clear();
        gvRAImpact.EditIndexes.Clear();
        gvRAImpact.DataSource = null;
        gvRAImpact.Rebind();
    }

    protected void BindHazardCategory()
    {
        ddlHazard.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRAHazardCategory();
        ddlHazard.DataBind();
        ddlHazard.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDHAZARDCATEGORYNAME", "FLDCONSEQUENCEIMPACT", "FLDSEVERITYNAME", "FLDSEVERITYSCORE", "FLDACTIVEYN" };
        string[] alCaptions = { "Hazard Category", "Consequence Impact", "Severity", "Score", "ActiveYN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPEARSRiskassessmentConsequenceImpact.RAConsequenceImpactSearch(General.GetNullableInteger(ddlHazard.SelectedValue)
            , General.GetNullableString(txtName.Text)
            , sortdirection
            , sortexpression
            , gvRAImpact.CurrentPageIndex + 1
            , gvRAImpact.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=ConsequenceImpact.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Process</h3></td>");
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
    protected void MenuRAImpact_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtName.Text = "";
                ddlHazard.ClearSelection();
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvRAImpact_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAImpact.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRAImpact_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblConsequenceID = (RadLabel)e.Item.FindControl("lblConsequenceID");
                PhoenixInspectionPEARSRiskassessmentConsequenceImpact.DeleteRAConsequenceImpact(new Guid(lblConsequenceID.Text));
                ucStatus.Text = "Successfully Deleted";
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvRAImpact_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblConsequenceID = (RadLabel)e.Item.FindControl("lblConsequenceID");

            if (cmdEdit != null)
            {
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('Impact','Consequence Impact Edit','" + Session["sitepath"] + "/Inspection/InspectionPEARSRAConsequenceImpactAdd.aspx?IMPACTID=" + lblConsequenceID.Text + "'); return false;");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}
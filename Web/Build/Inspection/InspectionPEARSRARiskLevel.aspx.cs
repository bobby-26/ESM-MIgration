using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionPEARSRARiskLevel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRARiskLevel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRARiskLevel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRARiskLevel.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRARiskLevel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('RiskLevel','Risk Level Add','" + Session["sitepath"] + "/Inspection/InspectionPEARSRARiskLevelAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuRARiskLevel.AccessRights = this.ViewState;
            MenuRARiskLevel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvRARiskLevel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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
        string[] alColumns = { "FLDSHORTCODE", "FLDMINRANGE", "FLDMAXRANGE", "FLDRISKLEVEL", "FLDCOLOR", "FLDREMARKS", "FLDACTIVEYN" };
        string[] alCaptions = { "Code", "Min Range", "Max Range", "Risk Level", "Color", "Remarks", "ActiveYN" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPEARSRiskLevel.RARiskLevelSearch(General.GetNullableString(txtName.Text)
            , sortdirection
            , sortexpression
            , gvRARiskLevel.CurrentPageIndex + 1
            , gvRARiskLevel.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvRARiskLevel", "RiskLevel", alCaptions, alColumns, ds);

        gvRARiskLevel.DataSource = ds;
        gvRARiskLevel.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvRARiskLevel.SelectedIndexes.Clear();
        gvRARiskLevel.EditIndexes.Clear();
        gvRARiskLevel.DataSource = null;
        gvRARiskLevel.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTCODE", "FLDMINRANGE", "FLDMAXRANGE", "FLDRISKLEVEL", "FLDCOLOR", "FLDREMARKS", "FLDACTIVEYN" };
        string[] alCaptions = { "Code", "Min Range", "Max Range", "Risk Level", "Color", "Remarks", "ActiveYN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPEARSRiskLevel.RARiskLevelSearch(General.GetNullableString(txtName.Text)
            , sortdirection
            , sortexpression
            , gvRARiskLevel.CurrentPageIndex + 1
            , gvRARiskLevel.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=RiskLevel.xls");
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
    protected void MenuRARiskLevel_TabStripCommand(object sender, EventArgs e)
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

    protected void gvRARiskLevel_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblRiskLevelID = (RadLabel)e.Item.FindControl("lblRiskLevelID");
                PhoenixInspectionPEARSRiskLevel.DeleteRARiskLevel(int.Parse(lblRiskLevelID.Text));
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

    protected void gvRARiskLevel_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblRiskLevelID = (RadLabel)e.Item.FindControl("lblRiskLevelID");

            if(cmdEdit != null)
            {
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('RiskLevel','Risk Level Edit','" + Session["sitepath"] + "/Inspection/InspectionPEARSRARiskLevelAdd.aspx?RISKLEVELID=" + lblRiskLevelID.Text + "'); return false;");
            }
        }
    }

    protected void gvRARiskLevel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRARiskLevel.CurrentPageIndex + 1;
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}
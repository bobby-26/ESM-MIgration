using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionPEARSRALOH : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRALOH.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRALOH')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRALOH.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRALOH.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('LOH','LOH Add','" + Session["sitepath"] + "/Inspection/InspectionPEARSRALOHAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuRALOH.AccessRights = this.ViewState;
            MenuRALOH.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvRALOH.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        string[] alColumns = { "FLDSHORTCODE", "FLDNAME", "FLDSCORE", "FLDREMARKS", "FLDACTIVEYN" };
        string[] alCaptions = { "Code", "Name", "Score", "Remarks", "ActiveYN" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPEARSRiskassessmentLOH.RALOHSearch(General.GetNullableString(txtName.Text)
            , sortdirection
            , sortexpression
            , gvRALOH.CurrentPageIndex + 1
            , gvRALOH.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvRALOH", "LOH", alCaptions, alColumns, ds);

        gvRALOH.DataSource = ds;
        gvRALOH.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvRALOH.SelectedIndexes.Clear();
        gvRALOH.EditIndexes.Clear();
        gvRALOH.DataSource = null;
        gvRALOH.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTCODE", "FLDNAME", "FLDSCORE", "FLDREMARKS", "FLDACTIVEYN" };
        string[] alCaptions = { "Code", "Name", "Score", "Remarks", "ActiveYN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPEARSRiskassessmentLOH.RALOHSearch(General.GetNullableString(txtName.Text)
            , sortdirection
            , sortexpression
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=LOH.xls");
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
    protected void gvRALOH_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRALOH.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRALOH_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblLOHID = (RadLabel)e.Item.FindControl("lblLOHID");
                PhoenixInspectionPEARSRiskassessmentLOH.DeleteRALOH(Int32.Parse(lblLOHID.Text));
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

    protected void gvRALOH_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblLOHID = (RadLabel)e.Item.FindControl("lblLOHID");

            if (cmdEdit != null)
            {
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('LOH','LOH Edit','" + Session["sitepath"] + "/Inspection/InspectionPEARSRALOHAdd.aspx?LOHID=" + lblLOHID.Text + "'); return false;");
            }
        }
    }

    protected void MenuRALOH_TabStripCommand(object sender, EventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}
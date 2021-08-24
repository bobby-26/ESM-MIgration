using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Options_OptionsVesselPhoenixImportError : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionsVesselPhoenixImportError.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvImportError')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Options/OptionsVesselPhoenixImportError.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Options/OptionsVesselPhoenixImportError.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersHoliday.AccessRights = this.ViewState;
            MenuRegistersHoliday.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvImportError.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBASETABLE", "FLDERRORDATE", "FLDERRORMESSAGE", "FLDXML", "FLDSQL", "FLDERRORNUMBER", "FLDERRORSEVERITY", "FLDERRORSTATE" };
        string[] alCaptions = { "Base Table", "Error Date", "Error Message", "XML", "SQL", "Error Number", "Error Severity", "Error State" };

        ds = PhoenixDataTransferImportError.PhoenixVesselImportError(General.GetNullableString(txtBaseTable.Text), General.GetNullableDateTime(txtDate.Text));

        Response.AddHeader("Content-Disposition", "attachment; filename=OfficeImportError.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Office Import Error</h3></td>");
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

    protected void RegistersHoliday_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvImportError.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtBaseTable.Text = "";
                txtDate.Text = "";
                BindData();
                gvImportError.Rebind();
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBASETABLE", "FLDERRORDATE", "FLDERRORMESSAGE", "FLDXML", "FLDSQL",  "FLDERRORNUMBER",  "FLDERRORSEVERITY", "FLDERRORSTATE"};
        string[] alCaptions = { "Base Table", "Error Date", "Error Message", "XML", "SQL", "Error Number", "Error Severity", "Error State"};

        ds = PhoenixDataTransferImportError.PhoenixVesselImportError(General.GetNullableString(txtBaseTable.Text), General.GetNullableDateTime(txtDate.Text));

        General.SetPrintOptions("gvImportError", "Import Error", alCaptions, alColumns, ds);

        gvImportError.DataSource = ds;
        gvImportError.VirtualItemCount = iRowCount;

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvImportError.Rebind();
    }

    protected void gvImportError_ItemCommand(object sender, GridCommandEventArgs e)
    {
    }

    protected void gvImportError_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvImportError_ItemDataBound(object sender, GridItemEventArgs e)
    {
        UserControlToolTip ucErrorMessage = (UserControlToolTip)e.Item.FindControl("ucErrorMessage");
        RadLabel lblErrorMessage = (RadLabel)e.Item.FindControl("lblErrorMessage");
        if (lblErrorMessage != null)
        {
            ucErrorMessage.Position = ToolTipPosition.BottomCenter;
            ucErrorMessage.TargetControlId = lblErrorMessage.ClientID;
        }

        UserControlToolTip ucXML = (UserControlToolTip)e.Item.FindControl("ucXML");
        RadLabel lblXML = (RadLabel)e.Item.FindControl("lblXML");
        if (lblXML != null)
        {
            ucXML.Position = ToolTipPosition.BottomCenter;
            ucXML.TargetControlId = lblXML.ClientID;
        }

        UserControlToolTip ucSQL = (UserControlToolTip)e.Item.FindControl("ucSQL");
        RadLabel lblSQL = (RadLabel)e.Item.FindControl("lblSQL");
        if (lblSQL != null)
        {
            ucSQL.Position = ToolTipPosition.BottomCenter;
            ucSQL.TargetControlId = lblSQL.ClientID;
        }

    }
}

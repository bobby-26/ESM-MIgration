using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Registers_RegistersSeaPortVisaDocuments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersSeaPortVisaDocuments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCountryVisaDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersSeaPortVisaDocuments.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuRegistersCountryVisaDocument.AccessRights = this.ViewState;
        MenuRegistersCountryVisaDocument.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Seaport", "SEAPORT" ,ToolBarDirection.Right);
        MenuRegistersCountryVisa.AccessRights = this.ViewState;
        MenuRegistersCountryVisa.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["countryid"] != null)
                ViewState["countryid"] = Request.QueryString["countryid"];
            else
                ViewState["countryid"] = "";

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvCountryVisaDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void RegistersCountryVisaDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvCountryVisaDocument.DataSource = null;
                gvCountryVisaDocument.Rebind();
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

    protected void gvCountryVisaDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName == "Page")
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

    protected void gvCountryVisaDocument_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblDocumentSpecification = (RadLabel)e.Item.FindControl("lblDocumentSpecification");
            UserControlToolTip ucDocumentSpecTT = (UserControlToolTip)e.Item.FindControl("ucDocumentSpecTT");
            if (lblDocumentSpecification != null)
            {
                lblDocumentSpecification.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDocumentSpecTT.ToolTip + "', 'visible');");
                lblDocumentSpecification.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDocumentSpecTT.ToolTip + "', 'hidden');");
            }
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHARDNAME", "FLDDOCUMENTSPECIFICATION" };
        string[] alCaptions = { "Document Name", "Document Specification" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCountryVisaDocument.CountryVisaRequiredDocumentsSearch(
            General.GetNullableInteger(ViewState["countryid"].ToString()),
            General.GetNullableInteger(ucHard.SelectedHard), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"], gvCountryVisaDocument.PageSize,
            ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvCountryVisaDocument", "Country Visa Document", alCaptions, alColumns, ds);

        gvCountryVisaDocument.DataSource = ds;
        gvCountryVisaDocument.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDHARDNAME", "FLDDOCUMENTSPECIFICATION" };
        string[] alCaptions = { "Document Name", "Document Specification" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCountryVisaDocument.CountryVisaRequiredDocumentsSearch(
          General.GetNullableInteger(ViewState["countryid"].ToString()),
          General.GetNullableInteger(ucHard.SelectedHard), sortexpression, sortdirection,
          1, iRowCount,
          ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CountryVisa.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Country Visa Document</h3></td>");
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
    protected void MenuRegistersCountryVisa_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEAPORT"))
        {
            Response.Redirect("../Registers/RegistersSeaport.aspx?countryid=" + ViewState["countryid"].ToString(), true);
        }
    }

    protected void gvCountryVisaDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = (ViewState["PAGENUMBER"] != null) ? ViewState["PAGENUMBER"] : (gvCountryVisaDocument.CurrentPageIndex + 1);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

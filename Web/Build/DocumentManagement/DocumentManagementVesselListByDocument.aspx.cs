using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class DocumentManagementVesselListByDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementVesselListByDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselListByDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");
            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["DOCUMENTID"] = "";
                ViewState["Type"] ="";
                ViewState["FORMID"] = "";
                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                {
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                    GetDocumentName();
                }
                if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() != "")
                    ViewState["Type"] = Request.QueryString["type"].ToString();
                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                {
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                    GetFormName();
                }
                gvVesselListByDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void GetDocumentName()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                titlecontent.Text =" Distributed Vessel List [ " + dr["FLDDOCUMENTNAME"].ToString() + " ] ";
            }
        }
    }
    private void GetFormName()
    {
        if (ViewState["FORMID"] != null && ViewState["FORMID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementForm.FormEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                ,new Guid(ViewState["FORMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                titlecontent.Text = " Distributed Vessel List [ " + dr["FLDCAPTION"].ToString() + " ] ";
            }
        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDVESSELNAME" };
        string[] alCaptions = { "S.No", "Vessel [ID]"};

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDocumentManagementDocument.DocumentManagementVesselListByDocument(General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel List</h3></td>");
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

    protected void gvVesselListByDocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSERIALNUMBER", "FLDVESSELNAME" };
        string[] alCaptions = { "S.No", "Vessel [ID]" };

        DataSet ds = new DataSet();

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["Type"].ToString() == "Document")
        {
            ds = PhoenixDocumentManagementDocument.DocumentManagementVesselListByDocument(General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount);
        }
        else if (ViewState["Type"].ToString() == "Form")
        {
            ds = PhoenixDocumentManagementDocument.DocumentManagementVesselListByForm(General.GetNullableGuid(ViewState["FORMID"].ToString())
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount);
        }
        
        General.SetPrintOptions("gvVesselListByDocument", "Vessel List", alCaptions, alColumns, ds);
        gvVesselListByDocument.DataSource = ds;
        gvVesselListByDocument.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

}

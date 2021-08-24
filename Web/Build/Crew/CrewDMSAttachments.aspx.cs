using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewDMSAttachments : PhoenixBasePage
{

    int employeeId, companyid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewDMSAttachments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDMSDocuments')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewDMSAttachments.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewDMSAttachments.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.MenuList = toolbar.Show();
            employeeId = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvDMSDocuments.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvDMSDocuments.SelectedIndexes.Clear();
        gvDMSDocuments.EditIndexes.Clear();
        gvDMSDocuments.DataSource = null;
        gvDMSDocuments.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER","FLDDOCUMENTNAME" };
        string[] alCaptions = { "S.NO", "Document Name" };

       string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = CrewDmsDocumentsAttachment.CrewDMSAttachmentsSearch(
                null
                , txtSearch.Text
                , sortexpression
                , sortdirection
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvDMSDocuments.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvDMSDocuments.DataSource = dt;
            gvDMSDocuments.VirtualItemCount = iRowCount;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvDMSDocuments", "DMS Documents", alCaptions, alColumns, ds);

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDMSDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDMSDocuments.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDMSDocuments_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string documentId = ((RadTextBox)e.Item.FindControl("txtDocumentId")).Text;
                string documentName = ((RadTextBox)e.Item.FindControl("txtDocumentName")).Text;
                if (!IsValidDMSDocument(documentId))
                {
                    ucError.Visible = true;
                    return;
                }
                CrewDmsDocumentsAttachment.DMSAttachmentsInsert(employeeId, documentId);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string crewDocumentId = ((RadLabel)e.Item.FindControl("lblCrewDocumentId")).Text;
                CrewDmsDocumentsAttachment.CrewDMSAttachmentsDelete(employeeId, crewDocumentId);
                Rebind();
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
    private bool IsValidDMSDocument(string documentName)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(General.GetNullableString(documentName)))
            ucError.ErrorMessage = "Document Name is required.";

        return (!ucError.IsError);
    }
    protected void gvDMSDocuments_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridFooterItem)
        {

            LinkButton btnAdd = e.Item.FindControl("btnShowDocuments") as LinkButton;
            btnAdd.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + companyid + "', true); ");
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentMedicalRequestFilter = null;
                txtSearch.Text = "";
                gvDMSDocuments.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = new DataTable();

        string[] alColumns = { "FLDROWNUMBER", "FLDDOCUMENTNAME" };
        string[] alCaptions = { "S.NO", "Document Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        dt = CrewDmsDocumentsAttachment.CrewDMSAttachmentsSearch(
                null,
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DMSDocuments.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Crew DMS Documents</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;


public partial class OptionsDocumentFields : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsDocumentFields.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentFields')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Options/OptionsDocumentFields.aspx", "Find", "search.png", "FIND");

            MenuDocumentFields.MenuList = toolbar.Show();
        
            if (!IsPostBack)
            {
                ucDocType.DocmentsTypeList = PhoenixCommanDocuments.ListDocumentType();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDocumentFields.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void Rebind()
    {
        gvDocumentFields.SelectedIndexes.Clear();
        gvDocumentFields.EditIndexes.Clear();
        gvDocumentFields.DataSource = null;
        gvDocumentFields.Rebind();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDFIELDNAME", "FLDDESCRIPTION" };
        string[] alCaptions = { "Document Type","Field Name", "Description"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommanDocuments.DocumentNumberFieldsSearch(ucDocType.SelectedDocumentType.ToString(), sortexpression, sortdirection,
           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentFields.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentFields.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Fields</h3></td>");
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

    protected void DocumentFields_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDFIELDNAME", "FLDDESCRIPTION" };
        string[] alCaptions = { "Document Type","Field Name", "Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null)? null: (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
      
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds= PhoenixCommanDocuments.DocumentNumberFieldsSearch(ucDocType.SelectedDocumentType.ToString() , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentFields.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentFields", "Document Fields", alCaptions, alColumns, ds);
        gvDocumentFields.DataSource = ds;
        gvDocumentFields.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

  
    protected void gvDocumentFields_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocumentFields(((RadTextBox)e.Item.FindControl("txtDocumentFieldsAdd")).Text,
                       ucDocType.SelectedDocumentType.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentFields(
                    ((RadTextBox)e.Item.FindControl("txtDocumentFieldsAdd")).Text,
                   int.Parse(ucDocType.SelectedDocumentType.ToString()),
                    ((RadTextBox)(e.Item.FindControl("txtDescriptionAdd"))).Text
                );
                ((RadTextBox)e.Item.FindControl("txtDocumentFieldsAdd")).Focus();
                BindData();
                Rebind();
               
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDocumentFields(((RadTextBox)e.Item.FindControl("txtDocumentFieldsEdit")).Text,
                    ucDocType.SelectedDocumentType.ToString()))

                {
                    ucError.Visible = true;
                    return;
                }

                UpdateDocumentFields(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentFieldsEdit")).Text),
                          ((RadTextBox)e.Item.FindControl("txtDocumentFieldsEdit")).Text,
                        int.Parse(ucDocType.SelectedDocumentType.ToString()),
                        ((RadTextBox)(e.Item.FindControl("txtDescriptionEdit"))).Text
                     );
                BindData();
                Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentFields(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentFieldsId")).Text));
                BindData();
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

 
    private void InsertDocumentFields(string fieldsname, int documenttypeid, string description)
    {

        PhoenixCommanDocuments.InsertDocumentNumberFields(PhoenixSecurityContext.CurrentSecurityContext.UserCode,documenttypeid,
            fieldsname, description);
        
    }

    private void UpdateDocumentFields(int fieldsid, string fieldsname,int documenttypeid, string description)
    {
        PhoenixCommanDocuments.UpdateDocumentNumberFields(PhoenixSecurityContext.CurrentSecurityContext.UserCode, fieldsid,documenttypeid,
            fieldsname, description);
        ucStatus.Text = "Document Field information updated";
        
    }

    private bool IsValidDocumentFields(string DocumentFields, string DocumentType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (DocumentFields.Trim().Equals(""))
            ucError.ErrorMessage = "Field is required.";

        if (DocumentType.Equals(""))
            ucError.ErrorMessage = "Document type is required.";       
        return (!ucError.IsError);
    }

    private void DeleteDocumentFields(int DocumentFieldsid)
    {
        PhoenixCommanDocuments.DeleteDocumentNumberFields(PhoenixSecurityContext.CurrentSecurityContext.UserCode, DocumentFieldsid);
    }

  
    protected void gvDocumentFields_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
        if (e.Item is GridDataItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvDocumentFields_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentFields.CurrentPageIndex + 1;
        BindData();
    }
}

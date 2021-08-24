using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class OptionsDocumentType : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsDocumentType.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentType')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Options/OptionsDocumentType.aspx", "Find", "search.png", "FIND");

            MenuDocumentType.MenuList = toolbar.Show();
        
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDocumentType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch(Exception ex)
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

        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDDESCRIPTION"};
        string[] alCaptions = { "Document Type", "Description"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommanDocuments.DocumentTypeSearch(txtDocumentType.Text, txtDescription.Text, null, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),gvDocumentType.PageSize,ref iRowCount,ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=documenttype.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Type</h3></td>");
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
        protected void MenuDocumentType_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Document Type", "Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null)? null: (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
      
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds= PhoenixCommanDocuments.DocumentTypeSearch(txtDocumentType.Text, txtDescription.Text, null, sortexpression, sortdirection,
              Int32.Parse(ViewState["PAGENUMBER"].ToString()),
              gvDocumentType.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentType", "Document Type", alCaptions, alColumns, ds);

        gvDocumentType.DataSource = ds;
        gvDocumentType.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    
    }
    
    protected void gvDocumentType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocumentType(((RadTextBox)e.Item.FindControl("txtDocumentTypeAdd")).Text))
                   
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentType(
                    ((RadTextBox)e.Item.FindControl("txtDocumentTypeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text,
                    ((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true ? 1 : 0
                     
                    
                );
                ((RadTextBox)e.Item.FindControl("txtDocumentTypeAdd")).Focus();
                BindData();
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDocumentType(((RadTextBox)e.Item.FindControl("txtDocumentTypeEdit")).Text))

                {
                    ucError.Visible = true;
                    return;
                }

                UpdateDocumentType(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentTypeEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtDocumentTypeEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text,
                         ((RadCheckBox)e.Item.FindControl("chkActiveYNedit")).Checked == true ? 1 : 0
                     );

                BindData();
                Rebind();
            }


            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentType(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentTypeId")).Text));
             
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

    private void InsertDocumentType(string documenttype, string description, int vesselid)
    {

        PhoenixCommanDocuments.InsertDocumentType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            documenttype, description, vesselid);
        
    }

    private void UpdateDocumentType(int documenttypeid, string documenttype, string description,int vesselid)
    {
        PhoenixCommanDocuments.UpdateDocumentType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,documenttypeid,
            documenttype, description, vesselid);
        ucStatus.Text = "Country information updated";
        
    }
 
    private bool IsValidDocumentType(string documenttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (documenttype.Trim().Equals(""))
            ucError.ErrorMessage = "Document type is required.";
        return (!ucError.IsError);
    }

    private void DeleteDocumentType(int documenttypeid)
    {
        PhoenixCommanDocuments.DeleteDocumentType(PhoenixSecurityContext.CurrentSecurityContext.UserCode, documenttypeid);
    }

   
  
    protected void gvDocumentType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
    protected void Rebind()
    {
        gvDocumentType.SelectedIndexes.Clear();
        gvDocumentType.EditIndexes.Clear();
        gvDocumentType.DataSource = null;
        gvDocumentType.Rebind();
    }

    protected void gvDocumentType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentType.CurrentPageIndex + 1;
        BindData();
    }
}

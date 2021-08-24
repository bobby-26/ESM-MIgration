using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class OptionsDocumentNumberFormat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsDocumentNumberFormat.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentNumberFormat')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Options/OptionsDocumentNumberFormat.aspx", "Find", "search.png", "FIND");

            MenuDocumentNumberFormat.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucDocType.DocmentsTypeList = PhoenixCommanDocuments.ListDocumentType();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDocumentNumberFormat.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvDocumentNumberFormat.SelectedIndexes.Clear();
        gvDocumentNumberFormat.EditIndexes.Clear();
        gvDocumentNumberFormat.DataSource = null;
        gvDocumentNumberFormat.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDFIELDNAME", "FLDVALUE", "FLDORDER" };
        string[] alCaptions = { "Document Type", "Field Name", "Value", "Order" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommanDocuments.DocumentNumberFormatSearch(General.GetNullableInteger(ucDocType.SelectedDocumentType.ToString()), "", null, sortexpression, sortdirection,
           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentNumberFormat.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentFormat.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Format</h3></td>");
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

    protected void MenuDocumentNumberFormat_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDFIELDNAME", "FLDVALUE", "FLDORDER" };
        string[] alCaptions = { "Document Type", "Field Name", "Value", "Order" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixCommanDocuments.DocumentNumberFormatSearch(General.GetNullableInteger(ucDocType.SelectedDocumentType.ToString()), "", "", sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentNumberFormat.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentNumberFormat", "Document Format", alCaptions, alColumns, ds);
        gvDocumentNumberFormat.DataSource = ds;
        gvDocumentNumberFormat.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvDocumentNumberFormat_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocumentFormat(((UserControlDocmentFields)(e.Item.FindControl("ucFieldsAdd"))).SelectedDocmentFields,
                       ((RadTextBox)e.Item.FindControl("txtValuesAdd")).Text,
                       ((UserControlMaskNumber)e.Item.FindControl("txtorderAdd")).Text,
                       ucDocType.SelectedDocumentType.ToString()))

                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentNumberFormat(
                   ucDocType.SelectedDocumentType,
                    ((UserControlDocmentFields)(e.Item.FindControl("ucFieldsAdd"))).SelectedDocmentFields,
                    ((RadTextBox)e.Item.FindControl("txtValuesAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtorderAdd")).Text
                );
                ((RadTextBox)e.Item.FindControl("txtValuesAdd")).Focus();

                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDocumentFormat(
                      ((UserControlDocmentFields)e.Item.FindControl("ucFieldsEdit")).SelectedDocmentFields.ToString(),
                      // ucFieldsEdit.SelectedDocmentFields.ToString()
                      ((RadTextBox)e.Item.FindControl("txtValuesEdit")).Text,
                      ((UserControlMaskNumber)e.Item.FindControl("txtorderEdit")).Text,
                       ucDocType.SelectedDocumentType.ToString()))

                {
                    ucError.Visible = true;
                    return;
                }

                UpdateDocumentNumberFormat(
                         ((RadLabel)e.Item.FindControl("lblValues")).Text,
                        ((RadLabel)e.Item.FindControl("lblDocumentTypeID")).Text,
                        ((UserControlDocmentFields)e.Item.FindControl("ucFieldsEdit")).SelectedDocmentFields,
                         ((RadTextBox)e.Item.FindControl("txtValuesEdit")).Text,
                          ((UserControlMaskNumber)e.Item.FindControl("txtorderEdit")).Text
                     );

                Rebind();
            }


            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentNumberFormat(Int32.Parse(((RadLabel)e.Item.FindControl("lblValuesId")).Text));
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
    private void InsertDocumentNumberFormat(string documenttypeid, string fieldid, string fieldvalues, string fielorder)
    {

        PhoenixCommanDocuments.InsertDocumentNumberFormat(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
           int.Parse(documenttypeid), fieldid, fieldvalues, int.Parse(fielorder));

    }

    private void UpdateDocumentNumberFormat(string valuesid, string documenttypeid, string fieldid, string fieldvalues, string fielorder)
    {
        PhoenixCommanDocuments.UpdateDocumentNumberFormat(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(valuesid),
             int.Parse(documenttypeid), fieldid, fieldvalues, int.Parse(fielorder));
        ucStatus.Text = "Document Format updated";

    }

    //  private bool IsValidDocumentType(string fildvalues, string serial)
    //  {
    //      ucError.HeaderMessage = "Please provide the following required information";
    //
    //      if (serial.Equals("") || General.GetNullableInteger(serial) == null)
    //          ucError.ErrorMessage = "Field order is required.";
    //
    //      return (!ucError.IsError);
    //  }

    private bool IsValidDocumentFormat(string Fieldname, string Value, string order, string DocumentType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        // if (Fieldname.Equals(""))
        //    ucError.ErrorMessage = "Field Name is required.";
        //  if (General.GetNullableInteger(Fieldname) == null)
        //     ucError.ErrorMessage = "Field Name is required.";
        if (Fieldname.Equals("") || General.GetNullableInteger(Fieldname) == null)
            ucError.ErrorMessage = "Field Name is required.";

        if (Value.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";
        if (order.Trim().Equals(""))
            ucError.ErrorMessage = "order is required.";

        if (DocumentType.Equals(""))
            ucError.ErrorMessage = "Document type is required.";
        return (!ucError.IsError);
    }

    private void DeleteDocumentNumberFormat(int valuesid)
    {
        PhoenixCommanDocuments.DeleteDocumentNumberFormat(PhoenixSecurityContext.CurrentSecurityContext.UserCode, valuesid);
    }

    protected void gvDocumentNumberFormat_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
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

            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
        if (e.Item is GridEditableItem)
        {
            UserControlDocmentFields dc = (UserControlDocmentFields)e.Item.FindControl("ucFieldsEdit");
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblDocumentTypeID");

            if (dc != null)
            {
                    dc.DocmentFieldsList = PhoenixCommanDocuments.ListDocumentNumberFields(int.Parse(lbl.Text));
                    dc.SelectedDocmentFields = drv["FLDFIELDID"].ToString();
            }
        }
        if (e.Item is GridDataItem)
        {

        }
        if(e.Item is GridFooterItem)
        {
            ((UserControlDocmentFields)(e.Item.FindControl("ucFieldsAdd"))).DocmentFieldsList = PhoenixCommanDocuments.ListDocumentNumberFields(General.GetNullableInteger(ucDocType.SelectedDocumentType));
        }
    }

    protected void gvDocumentNumberFormat_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentNumberFormat.CurrentPageIndex + 1;
        BindData();
    }
}

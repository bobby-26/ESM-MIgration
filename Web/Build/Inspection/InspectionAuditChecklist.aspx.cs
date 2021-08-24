using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionAuditChecklist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionAuditChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvChecklist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuChecklist.AccessRights = this.ViewState;
            MenuChecklist.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvChecklist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGENUMBER"] = 1;
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

        string[] alColumns = { "FLDSHORTCODE", "FLDCHECKLIST", "FLDORDER" };
        string[] alCaptions = { "Code", "Checklist", "Order" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionAuditChecklist.InspectionAuditChecklistSearch(null, null, sortexpression, sortdirection,
             1,
             iRowCount,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ReviewChecklist.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Review Checklist</h3></td>");
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

    protected void Checklist_TabStripCommand(object sender, EventArgs e)
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

    protected void Rebind()
    {
        gvChecklist.SelectedIndexes.Clear();
        gvChecklist.EditIndexes.Clear();
        gvChecklist.DataSource = null;
        gvChecklist.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTCODE", "FLDCHECKLIST", "FLDORDER" };
        string[] alCaptions = { "Code", "Checklist", "Order" };
        DataSet ds = new DataSet();
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionAuditChecklist.InspectionAuditChecklistSearch(null, null, sortexpression, sortdirection,
           gvChecklist.CurrentPageIndex + 1,
            gvChecklist.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvChecklist", "Review Checklist", alCaptions, alColumns, ds);

        gvChecklist.DataSource = ds;
        gvChecklist.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvChecklist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
    protected void gvChecklist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidChecklist(((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                                      ((RadTextBox)e.Item.FindControl("txtChecklistAdd")).Text,
                                      ((UserControlMaskNumber)e.Item.FindControl("txtOrderAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionAuditChecklist.InsertInspectionAuditChecklist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text.Trim()),
                                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistAdd")).Text.Trim()),
                                        int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtOrderAdd")).Text));
                    ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Focus();
                    Rebind();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    GridDataItem item = e.Item as GridDataItem;
                    Guid checklistid  = new Guid(item.GetDataKeyValue("FLDCHECKLISTID").ToString());

                    PhoenixInspectionAuditChecklist.DeleteInspectionAuditChecklist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    checklistid);
                    Rebind();
                }
                else if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidChecklist(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                                       ((RadTextBox)e.Item.FindControl("txtChecklistEdit")).Text,
                                       ((UserControlMaskNumber)e.Item.FindControl("txtOrderEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    GridDataItem item = e.Item as GridDataItem;
                    Guid checklistid = new Guid(item.GetDataKeyValue("FLDCHECKLISTID").ToString());

                    PhoenixInspectionAuditChecklist.UpdateInspectionAuditChecklist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            checklistid,
                                                            General.GetNullableString(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim()),
                                                            General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistEdit")).Text.Trim()),
                                                            int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtOrderEdit")).Text));
                    Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidChecklist(string shortcode, string checklist, string order)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(shortcode) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableString(checklist) == null)
            ucError.ErrorMessage = "Checklist is required.";

        if (General.GetNullableInteger(order) == null)
            ucError.ErrorMessage = "Order is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvChecklist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvChecklist.CurrentPageIndex + 1;
        BindData();
    }
}

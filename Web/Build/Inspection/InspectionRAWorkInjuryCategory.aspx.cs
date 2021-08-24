using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAWorkInjuryCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAWorkInjuryCategory.aspx?" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAWorkInjuryCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAWorkInjuryCategory.aspx?type=" + Request.QueryString["type"], "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRAWorkInjuryCategory.AccessRights = this.ViewState;
            MenuRAWorkInjuryCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
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
        string[] alColumns = { "FLDWORKINJURYCATEGORYNAME", "FLDSEVERITY", "FLDSCORE", "FLDCATEGORY" };
        string[] alCaptions = { "Work Injury Category", "Severity", "Score", "Consequence Category" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentWorkInjuryCategory.RiskAssessmentWorkInjuryCategorySearch(
            General.GetNullableString(txtName.Text),
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=WorkInjuryCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Work Injury Category</h3></td>");
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

    protected void RAWorkInjuryCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDWORKINJURYCATEGORYNAME", "FLDSEVERITY", "FLDSCORE", "FLDCATEGORY" };
        string[] alCaptions = { "Work Injury Category", "Severity", "Score", "Consequence Category" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentWorkInjuryCategory.RiskAssessmentWorkInjuryCategorySearch(
            General.GetNullableString(txtName.Text),
            gvRAWorkInjuryCategory.CurrentPageIndex + 1,
            gvRAWorkInjuryCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvRAWorkInjuryCategory", "Work Injury Category", alCaptions, alColumns, ds);

        gvRAWorkInjuryCategory.DataSource = ds;
        gvRAWorkInjuryCategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        BindData();
    }

    protected void gvRAWorkInjuryCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidRAWorkInjuryCategory(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                             ((RadTextBox)e.Item.FindControl("txtCategoryAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRAWorkInjuryCategory(
                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                        ((UserControlRASeverity)e.Item.FindControl("ucSeverityAdd")).SelectedSeverity,
                        ((RadTextBox)e.Item.FindControl("txtCategoryAdd")).Text
                    );
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteWorkInjuryCategory(new Guid(((RadLabel)e.Item.FindControl("lblWorkInjuryCategoryId")).Text));
                }

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidRAWorkInjuryCategory(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateRAWorkInjuryCategory(
                            (((RadLabel)e.Item.FindControl("lblWorkInjuryCategoryIdEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                             ((UserControlRASeverity)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity,
                             ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text
                         );
                }
            }
            BindData();
            gvRAWorkInjuryCategory.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAWorkInjuryCategory_ItemDataBound(object sender, GridItemEventArgs e)
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
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlRASeverity ucSeverity = (UserControlRASeverity)e.Item.FindControl("ucSeverityEdit");
            DataRowView drvSeverity = (DataRowView)e.Item.DataItem;
            if (ucSeverity != null) ucSeverity.SelectedSeverity = drvSeverity["FLDSEVERITYID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private void InsertRAWorkInjuryCategory(string name, string severityid, string category)
    {

        PhoenixInspectionRiskAssessmentWorkInjuryCategory.InsertRiskAssessmentWorkInjuryCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              name, General.GetNullableInteger(severityid), General.GetNullableString(category));
    }

    private void UpdateRAWorkInjuryCategory(string WorkInjuryCategoryid, string name, string severityid, string category)
    {

        PhoenixInspectionRiskAssessmentWorkInjuryCategory.UpdateRiskAssessmentWorkInjuryCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(WorkInjuryCategoryid), name, General.GetNullableInteger(severityid), General.GetNullableString(category));
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRAWorkInjuryCategory(string name, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        // GridView _gridView = gvRAWorkInjuryCategory;
        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Work Injury Category is required.";

        if (General.GetNullableString(category) == null)
            ucError.ErrorMessage = "Consequence Category is required.";

        return (!ucError.IsError);
    }

    private void DeleteWorkInjuryCategory(Guid WorkInjuryCategoryid)
    {
        PhoenixInspectionRiskAssessmentWorkInjuryCategory.DeleteRiskAssessmentWorkInjuryCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, WorkInjuryCategoryid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRAWorkInjuryCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersVesselSurveyTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSurveyTemplate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurveyTemplate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersSurveyTemplate.AccessRights = this.ViewState;
            MenuRegistersSurveyTemplate.MenuList = toolbar.Show();
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                gvSurveyTemplate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDSHORTCODE", "FLDTEMPLATENAME", "FLDDURATION", "FLDAUDITREQUIREDYN" };
        string[] alCaptions = { "Code", "Name", "Validity(M)", "Is Audit Required Y/N" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselSurveyTemplate.SurveyTemplateSearch(
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Validity Cycle", ds.Tables[0], alColumns, alCaptions, null, "");
    }

    protected void MenuRegistersSurveyTemplate_TabStripCommand(object sender, EventArgs e)
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

        string[] alColumns = { "FLDSHORTCODE", "FLDTEMPLATENAME", "FLDDURATION", "FLDAUDITREQUIREDYN" };
        string[] alCaptions = { "Code", "Name", "Validity(M)", "Is Audit Required Y/N" };

        DataSet ds = PhoenixRegistersVesselSurveyTemplate.SurveyTemplateSearch(
            (int)ViewState["PAGENUMBER"],
            gvSurveyTemplate.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvSurveyTemplate", "Validity Cycle", alCaptions, alColumns, ds);


        gvSurveyTemplate.DataSource = ds;
        gvSurveyTemplate.VirtualItemCount = iRowCount;
 

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void InsertSurveyTemplate(string ShortCode, string Template, int? Duration, string AuditRequiredyn)
    {
        PhoenixRegistersVesselSurveyTemplate.InsertSurveyTemplate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             ShortCode
             , Template
             , Duration
             , int.Parse(AuditRequiredyn));
    }

    private void UpdateSurveyTemplate(string Template, int? Duration, Guid? TemplateId, string AuditRequiredyn)
    {
        PhoenixRegistersVesselSurveyTemplate.UpdateSurveyTemplate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Template
             , Duration
             , TemplateId
             , int.Parse(AuditRequiredyn));
    }
    private void DeleteSurveyTemplate(Guid? TemplateId)
    {
        PhoenixRegistersVesselSurveyTemplate.DeleteSurveyTemplate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, TemplateId);
    }

    private bool IsValidSurveyTemplate(string ShortCode, string Template, string Duration)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ShortCode == "")
            ucError.ErrorMessage = "Code is required.";

        if (Template == "")
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(Duration)==null)
            ucError.ErrorMessage = "Validity(M) is required.";

        if (General.GetNullableInteger(Duration) == 0)
            ucError.ErrorMessage = "Please enter valid Validity(M) .";
        return (!ucError.IsError);
    }

    protected void gvSurveyTemplate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSurveyTemplate.CurrentPageIndex + 1;
        BindData();
    }
    protected void Rebind()
    {
        gvSurveyTemplate.SelectedIndexes.Clear();
        gvSurveyTemplate.EditIndexes.Clear();
        gvSurveyTemplate.DataSource = null;
        gvSurveyTemplate.Rebind();
    }
    protected void gvSurveyTemplate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
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

    protected void gvSurveyTemplate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvSurveyTemplate.MasterTableView.GetItems(GridItemType.Footer)[0];
                if (!IsValidSurveyTemplate(((RadTextBox)footerItem.FindControl("txtShortCodeAdd")).Text.Trim()
                , ((RadTextBox)footerItem.FindControl("txtTemplateAdd")).Text.Trim()
                , ((UserControlMaskNumber)footerItem.FindControl("ucDurationAdd")).Text
                ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSurveyTemplate(
                    ((RadTextBox)footerItem.FindControl("txtShortCodeAdd")).Text.Trim()
                    , ((RadTextBox)footerItem.FindControl("txtTemplateAdd")).Text.Trim()
                     , General.GetNullableInteger(((UserControlMaskNumber)footerItem.FindControl("ucDurationAdd")).Text)
                     , (((RadCheckBox)footerItem.FindControl("chkAuditRequiredYNAdd")).Checked == true ? "1" : "0").ToString()
                );
                ucStatus.Text = "Validity Cycle added sucessfully";
                ucStatus.Visible = true;
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["TemplatesId"] = ((RadLabel)e.Item.FindControl("lblTemplatesId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadCheckBox chkAuditRequiredYN = ((RadCheckBox)e.Item.FindControl("chkAuditRequiredYN"));
                if (chkAuditRequiredYN.Checked == true)
                {
                    Response.Redirect("../Registers/RegistersVesselAuditSurveyPlanConfiguration.aspx?TemplateId=" + ((RadLabel)e.Item.FindControl("lblTemplatesId")).Text);
                }
                else
                {
                    Response.Redirect("../Registers/RegistersVesselSurveyPlanConfiguration.aspx?TemplateId=" + ((RadLabel)e.Item.FindControl("lblTemplatesId")).Text);
                }
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

    protected void gvSurveyTemplate_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidSurveyTemplate(((RadLabel)e.Item.FindControl("lblShortCodeEdit")).Text.Trim()
                , ((RadTextBox)e.Item.FindControl("txtTemplateEdit")).Text.Trim()
                , ((UserControlMaskNumber)e.Item.FindControl("ucDurationEdit")).Text
                ))
            {
                ucError.Visible = true;
                return;
            }

            UpdateSurveyTemplate(
                     ((RadTextBox)e.Item.FindControl("txtTemplateEdit")).Text.Trim()
                     , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucDurationEdit")).Text)
                      , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTemplateId")).Text)
                      , (((RadCheckBox)e.Item.FindControl("chkAuditRequiredYNEdit")).Checked == true ? "1" : "0").ToString()
                 );
            gvSurveyTemplate.SelectedIndexes.Clear();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyTemplate_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteSurveyTemplate(General.GetNullableGuid(ViewState["TemplatesId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

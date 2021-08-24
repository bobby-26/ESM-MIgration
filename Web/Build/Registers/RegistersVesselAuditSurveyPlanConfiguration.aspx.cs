using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselAuditSurveyPlanConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselAuditSurveyPlanConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurveyCycles')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersSurveyCycles.AccessRights = this.ViewState;
            MenuRegistersSurveyCycles.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Validity Cycle", "BACK",ToolBarDirection.Right);
            MainMenuRegistersSurveyCycles.AccessRights = this.ViewState;
            MainMenuRegistersSurveyCycles.MenuList = toolbarmain.Show();
            SessionUtil.PageAccessRights(this.ViewState);


            if (!IsPostBack)
            {
                ViewState["TemplateId"] = string.IsNullOrEmpty(Request.QueryString["TemplateId"]) ? "" : Request.QueryString["TemplateId"];
                ViewState["PAGENUMBER"] = 1;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                gvSurveyCycles.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        if (!string.IsNullOrEmpty(ViewState["TemplateId"].ToString()))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDSHORTCODE", "FLDTEMPLATENAME", "FLDDURATION" };
            string[] alCaptions = { "Short Code", "Template", "Duration(Months)" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixRegistersVesselSurveyPlanConfiguration.AuditSurveyCycleSearch(new Guid(ViewState["TemplateId"].ToString()),
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

            General.ShowExcel("Survey Cycle", ds.Tables[0], alColumns, alCaptions, null, "");
        }
    }

    protected void MenuRegistersSurveyCycles_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvSurveyCycles.SelectedIndexes.Clear();
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSurveyCycles.Rebind();
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
    protected void MainMenuRegistersSurveyCycles_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Registers/RegistersVesselSurveyTemplate.aspx");
        }
    }
    private void BindData()
    {
        if (!string.IsNullOrEmpty(ViewState["TemplateId"].ToString()))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSHORTCODE", "FLDTEMPLATENAME", "FLDDURATION" };
            string[] alCaptions = { "Short Code", "Template", "Duration(Months)" };

            DataSet ds = PhoenixRegistersVesselSurveyPlanConfiguration.AuditSurveyCycleSearch(new Guid(ViewState["TemplateId"].ToString()),
                (int)ViewState["PAGENUMBER"],
                gvSurveyCycles.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvSurveyCycles", "Survey Cycle", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTemplate.Text = ds.Tables[0].Rows[0]["FLDTEMPLATENAME"].ToString();
            }

            gvSurveyCycles.DataSource = ds;
            gvSurveyCycles.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            
            
        }
    }

    private void InsertSurveyTemplate(Guid? TemplateId, Guid? AuditId, int? Sequence, int? Frequency, Guid? NextAuditId, int? NextSequence)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.InsertAuditSurveyCycle(            
               TemplateId
             , AuditId
             , Sequence
             , Frequency
             , NextAuditId
             , NextSequence);
    }

    private void UpdateSurveyTemplate(Guid ConfigurationId, Guid? AuditId, int? Sequence, int? Frequence, Guid? NextAuditId, int? NextSequence)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.UpdateAuditSurveyCycle(ConfigurationId                        
            , AuditId
            , Sequence
            , Frequence
            , NextAuditId
            , NextSequence);
    }
    private void DeleteSurveyTemplate(Guid? ConfigurationId)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.DeleteSurveyCycle(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ConfigurationId);
    }

    private bool IsValidSurveyTemplate(string AuditId, string Sequence, string Frequency, string NextAuditId, string NextSequence)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (AuditId.Equals("Dummy"))
            ucError.ErrorMessage = "Audit is required.";

        if (General.GetNullableInteger(Sequence) == null)
            ucError.ErrorMessage = "Sequence is required.";

        if (General.GetNullableInteger(Sequence) == 0)
            ucError.ErrorMessage = "Please enter valid Sequence.";

        if (General.GetNullableInteger(Frequency) == null)
            ucError.ErrorMessage = "'Months to add for Next Due Date' is required.";

        if (General.GetNullableInteger(Frequency) == 0)
            ucError.ErrorMessage = "Please enter valid 'Months to add for Next Due Date'.";

        if (NextAuditId.Equals("Dummy"))
            ucError.ErrorMessage = "Next Audit is required.";

        if (General.GetNullableInteger(Sequence) == null)
            ucError.ErrorMessage = "Next Sequence is required.";

        return (!ucError.IsError);
    }

    protected void gvSurveyCycles_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSurveyCycles.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvSurveyCycles_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'DAre you sure to delete the record?'); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadComboBox ddlNextAuditEdit = (RadComboBox)e.Item.FindControl("ddlNextAuditEdit");
            RadComboBox ddlAuditEdit = ((RadComboBox)e.Item.FindControl("ddlAuditEdit"));
            RadLabel lblNextAuditId = (RadLabel)e.Item.FindControl("lblNextAuditId");
            RadLabel lblAuditId = (RadLabel)e.Item.FindControl("lblAuditId");

            //if (ddlAuditEdit != null && lblNextAuditId != null)
            //    ddlAuditEdit.SelectedValue = lblNextAuditId.Text.Trim();
            //if (ddlNextAuditEdit != null && lblNextAuditId != null)
            //    ddlNextAuditEdit.SelectedValue = lblNextAuditId.Text.Trim();

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
                string Inspectiontype = PhoenixCommonRegisters.GetHardCode(1, 144, "EXT");

                if (ddlAuditEdit != null) 
                {
                    DataSet ds = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                                   , General.GetNullableInteger(Inspectiontype)
                                                   , null
                                                   , null
                                                   , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                    if (ds.Tables[0].Rows.Count>0)
                    {
                        ddlAuditEdit.DataSource = ds;
                        ddlAuditEdit.DataBind();
                    }
                    //ddlAuditEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlAuditEdit.SelectedValue = lblAuditId.Text.Trim();
                }
                if (ddlNextAuditEdit != null)
                {
                    DataSet ds = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                                   , General.GetNullableInteger(Inspectiontype)
                                                   , null
                                                   , null
                                                   , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlNextAuditEdit.DataSource = ds;
                        ddlNextAuditEdit.DataBind();
                    }
                    //ddlNextAuditEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNextAuditEdit.SelectedValue = lblNextAuditId.Text.Trim();
                }
            }
        }


        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
            string Inspectiontype = PhoenixCommonRegisters.GetHardCode(1, 144, "EXT");
            RadComboBox ddlAuditAdd = ((RadComboBox)e.Item.FindControl("ddlAuditAdd"));
            RadComboBox ddlNextAuditAdd = ((RadComboBox)e.Item.FindControl("ddlNextAuditAdd"));
            ddlAuditAdd.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                            , General.GetNullableInteger(Inspectiontype)
                                            , null
                                            , null
                                            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
            ddlAuditAdd.DataBind();
            ddlAuditAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));


            ddlNextAuditAdd.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                            , General.GetNullableInteger(Inspectiontype)
                                            , null
                                            , null
                                            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
            ddlNextAuditAdd.DataBind();
            ddlNextAuditAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
    }

    protected void gvSurveyCycles_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvSurveyCycles.MasterTableView.GetItems(GridItemType.Footer)[0];
                string type = ((RadComboBox)footerItem.FindControl("ddlAuditAdd")).SelectedValue.Trim();
                string sequence = ((UserControlMaskNumber)footerItem.FindControl("ucSequenceAdd")).Text.Trim();
                string nextdue = ((UserControlMaskNumber)footerItem.FindControl("ucNextDueAdd")).Text;
                string nexttype = ((RadComboBox)footerItem.FindControl("ddlNextAuditAdd")).SelectedValue.Trim();
                string nextsequence = ((UserControlMaskNumber)footerItem.FindControl("ucNextSequenceAdd")).Text.Trim();

                if (!IsValidSurveyTemplate(type, sequence, nextdue, nexttype, nextsequence))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSurveyTemplate(General.GetNullableGuid(ViewState["TemplateId"].ToString())
                        , General.GetNullableGuid(type)
                        , General.GetNullableInteger(sequence)
                        , General.GetNullableInteger(nextdue)
                        , General.GetNullableGuid(nexttype)
                        , General.GetNullableInteger(nextsequence)
                );
                ucStatus.Text = "Template added sucessfully";
                ucStatus.Visible = true;
                BindData();
                gvSurveyCycles.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSurveyTemplate(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblConfigurationId")).Text));
                BindData();
                gvSurveyCycles.Rebind();
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

    protected void gvSurveyCycles_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string type = ((RadComboBox)e.Item.FindControl("ddlAuditEdit")).SelectedValue;
            string sequence = ((UserControlMaskNumber)e.Item.FindControl("ucSequenceEdit")).Text.Trim();
            string nextdue = ((UserControlMaskNumber)e.Item.FindControl("ucNextDueEdit")).Text;
            string nexttype = ((RadComboBox)e.Item.FindControl("ddlNextAuditEdit")).SelectedValue;
            string nextsequence = ((UserControlMaskNumber)e.Item.FindControl("ucNextSequenceEdit")).Text.Trim();

            if (!IsValidSurveyTemplate(type, sequence, nextdue, nexttype, nextsequence))
            {
                ucError.Visible = true;
                return;
            }

            UpdateSurveyTemplate(new Guid(((RadLabel)e.Item.FindControl("lblConfigurationId")).Text.Trim())
                        , General.GetNullableGuid(type)
                        , General.GetNullableInteger(sequence)
                        , General.GetNullableInteger(nextdue)
                        , General.GetNullableGuid(nexttype)
                        , General.GetNullableInteger(nextsequence)
                 );
            gvSurveyCycles.SelectedIndexes.Clear();
            BindData();
            gvSurveyCycles.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyCycles_SortCommand(object sender, GridSortCommandEventArgs e)
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
}

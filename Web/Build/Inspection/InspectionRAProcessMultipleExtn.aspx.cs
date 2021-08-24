using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using System.Web;

public partial class InspectionRAProcessMultipleExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            BindData();
            ViewState["RISKASSESSMENTPROCESSID"] = null;
            Filter.CurrentMultipleRASelection = null;
            ViewState["STATUS"] = null;
            ViewState["VESSELID"] = "";

            if (Request.QueryString["processid"] != null)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["processid"].ToString();
                RiskAssessmentProcessEdit();
            }
            if (Request.QueryString["processmultipleid"] != null && Request.QueryString["processmultipleid"].ToString() != string.Empty)
            {
                Filter.CurrentMultipleRASelection = Request.QueryString["processmultipleid"].ToString();
                RiskAssessmentProcessMultipleEdit();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW");
            toolbar.AddButton("Save", "SAVE");
            MenuProcess.AccessRights = this.ViewState;
            if ((ViewState["STATUS"] != null && ViewState["STATUS"].ToString() != "3") || ViewState["STATUS"] == null)
                MenuProcess.MenuList = toolbar.Show();
            MenuProcess.SetTrigger(pnlProcess);
        }
        BindGrid();

        DropDownList ddlHealthSubHazardType = (DropDownList)gvHealthSafetyRisk.FooterRow.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();

        DropDownList ddllEnvSubHazardType = (DropDownList)gvEnvironmentalRisk.FooterRow.FindControl("ddlSubHazardType");
        ddllEnvSubHazardType.DataTextField = "FLDNAME";
        ddllEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddllEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddllEnvSubHazardType.DataBind();

        DropDownList ddlEcoSubHazardType = (DropDownList)gvEconomicRisk.FooterRow.FindControl("ddlSubHazardType");
        ddlEcoSubHazardType.DataTextField = "FLDNAME";
        ddlEcoSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEcoSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEcoSubHazardType.DataBind();
    }

    protected void BindGrid()
    {
        BindGridEconomicRisk();
        BindGridEnvironmentalRisk();
        BindGridHealthSafetyRisk();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        RiskAssessmentProcessEdit();
        RiskAssessmentProcessMultipleEdit();
    }

    private void RiskAssessmentProcessEdit()
    {
        DataSet dsProcess = PhoenixInspectionRiskAssessmentProcessExtn.EditInspectionRiskAssessmentProcess(
            (ViewState["RISKASSESSMENTPROCESSID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString())));

        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsProcess.Tables[0].Rows[0];
            txtCategory.Text = dr["FLDCATEGORYNAME"].ToString();
            txtProcess.Text = dr["FLDJOBACTIVITY"].ToString();
            ViewState["STATUS"] = dr["FLDSTATUS"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
        }
    }

    private void RiskAssessmentProcessMultipleEdit()
    {
        DataSet dsProcess = PhoenixInspectionRiskAssessmentProcessExtn.EditInspectionRAProcessMultiple(
            General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()));

        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsProcess.Tables[0].Rows[0];
            BindCheckBoxList(cblOtherRisk, dr["FLDOTHERRISK"].ToString());
            txtOtherRisk.Content = dr["FLDOTHERRISKPROPOSEDCONTROL"].ToString();
            //txtSequenceNumber.Text = dr["FLDSERIALNUMBER"].ToString();
            txtActivityCondition.Text = dr["FLDACTIVITYCONDITION"].ToString();
            if (rblOtherRiskControl.Items.FindByValue(dr["FLDRISKCONTROL"].ToString()) != null)
                rblOtherRiskControl.Items.FindByValue(dr["FLDRISKCONTROL"].ToString()).Selected = true;
            ucCompetencyLevel.SelectedQuick = dr["FLDCOMPETENCYLEVEL"].ToString();
            txtRiskAspects.Content = dr["FLDRISKASPECTS"].ToString();
        }
    }

    private void BindData()
    {
        cblOtherRisk.DataTextField = "FLDNAME";
        cblOtherRisk.DataValueField = "FLDHAZARDID";
        cblOtherRisk.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(3, 0);
        cblOtherRisk.DataBind();

        rblOtherRiskControl.DataTextField = "FLDNAME";
        rblOtherRiskControl.DataValueField = "FLDFREQUENCYID";
        rblOtherRiskControl.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
        rblOtherRiskControl.DataBind();
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
            {
                ucError.ErrorMessage = "Please go back and save the template and then try adding multiple RA.";
                ucError.Visible = true;
                return;
            }
            SaveRiskAssessmentProcessMultiple();
        }
        else if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            txtOtherRisk.Content = "";
            txtActivityCondition.Text = "";
            ucCompetencyLevel.SelectedQuick = "";
            txtRiskAspects.Content = "";
            //txtSequenceNumber.Text = "";
            Filter.CurrentMultipleRASelection = null;
            BindData();
            BindGrid();
        }
    }

    private void SaveRiskAssessmentProcessMultiple()
    {
        try
        {
            string otherrisk = ReadCheckBoxList(cblOtherRisk);
            string otherriskproposed = HttpUtility.HtmlDecode(txtOtherRisk.Content);

            if (!IsValidProcessTemplate())
            {
                ucError.Visible = true;
                return;
            }
            Guid? riskassessmentprocessmultipleidout = null;
            if (Filter.CurrentMultipleRASelection != null && Filter.CurrentMultipleRASelection.ToString() != string.Empty)
                riskassessmentprocessmultipleidout = new Guid(Filter.CurrentMultipleRASelection.ToString());

            PhoenixInspectionRiskAssessmentProcessExtn.InsertInspectionRAProcessMultiple(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                ref riskassessmentprocessmultipleidout,
                new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableString(otherrisk),
                General.GetNullableString(otherriskproposed),
                txtActivityCondition.Text,
                rblOtherRiskControl.SelectedValue,
                General.GetNullableInteger(ucCompetencyLevel.SelectedQuick),
                General.GetNullableString(HttpUtility.HtmlDecode(txtRiskAspects.Content)));

            Filter.CurrentMultipleRASelection = riskassessmentprocessmultipleidout.ToString();
            ucStatus.Text = "RA updated.";
            RiskAssessmentProcessMultipleEdit();

            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidProcessTemplate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtActivityCondition.Text) == null)
            ucError.ErrorMessage = "Activity/Condition is required.";

        return (!ucError.IsError);
    }

    private void BindCheckBoxList(CheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindByValue(item) != null)
                    cbl.Items.FindByValue(item).Selected = true;
            }
        }
    }

    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    private void BindGridHealthSafetyRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentProcessExtn.ListProcessCategory(1,
            (Filter.CurrentMultipleRASelection == null ? null : General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()))
            , General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvHealthSafetyRisk.DataSource = ds;
            gvHealthSafetyRisk.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvHealthSafetyRisk);
        }
    }

    private void BindGridEnvironmentalRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentProcessExtn.ListProcessCategory(2,
            (Filter.CurrentMultipleRASelection == null ? null : General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()))
            , General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvEnvironmentalRisk.DataSource = ds;
            gvEnvironmentalRisk.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvEnvironmentalRisk);
        }
    }

    private void BindGridEconomicRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentProcessExtn.ListProcessCategory(4,
            (Filter.CurrentMultipleRASelection == null ? null : General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()))
            , General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvEconomicRisk.DataSource = ds;
            gvEconomicRisk.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvEconomicRisk);
        }
    }

    protected void gvEconomicRisk_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton cmdAdd = (ImageButton)ge.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvEconomicRisk_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(gce.CommandArgument.ToString());
            if (gce.CommandName.ToUpper().Equals("CADD"))
            {
                if (!IsValidHazard(ucEconomicHazardType.SelectedHazardType,
                                        ((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue, null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcessExtn.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                        new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(Filter.CurrentMultipleRASelection.ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)_gridView.FooterRow.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucEconomicHazardType.SelectedHazardType = "";
                BindGridEconomicRisk();

            }
            else if (gce.CommandName.ToUpper().Equals("CDELETE"))
            {
                string categoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcessExtn.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridEconomicRisk();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(gce.CommandArgument.ToString());
            if (gce.CommandName.ToUpper().Equals("EADD"))
            {
                if (!IsValidHazard(ucEnvHazardType.SelectedHazardType,
                                        ((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue, 2,
                                        ((UserControlRAMiscellaneous)_gridView.FooterRow.FindControl("ucImpactType")).SelectedMiscellaneous))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcessExtn.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                        new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(Filter.CurrentMultipleRASelection.ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)_gridView.FooterRow.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtProposedControlAdd")).Text),
                        General.GetNullableInteger(((UserControlRAMiscellaneous)_gridView.FooterRow.FindControl("ucImpactType")).SelectedMiscellaneous)
                        );

                ucEnvHazardType.SelectedHazardType = "";
                BindGridEnvironmentalRisk();

            }
            else if (gce.CommandName.ToUpper().Equals("EDELETE"))
            {
                string categoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcessExtn.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridEnvironmentalRisk();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Row.RowType == DataControlRowType.Footer)
        {
            UserControlRAMiscellaneous ucImpactType = (UserControlRAMiscellaneous)ge.Row.FindControl("ucImpactType");
            if (ucImpactType != null)
            {
                ucImpactType.MiscellaneousList = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(3, 0);
                ucImpactType.DataBind();
            }

            ImageButton cmdAdd = (ImageButton)ge.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton cmdAdd = (ImageButton)ge.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk__RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("HADD"))
            {
                if (!IsValidHazard(ucHazardType.SelectedHazardType,
                                        ((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue, null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcessExtn.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(Filter.CurrentMultipleRASelection.ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)_gridView.FooterRow.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucHazardType.SelectedHazardType = "";
                BindGridHealthSafetyRisk();

            }
            else if (e.CommandName.ToUpper().Equals("HDELETE"))
            {
                string categoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcessExtn.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridHealthSafetyRisk();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Filter.CurrentMultipleRASelection == null)
            ucError.ErrorMessage = "Please save the RA first and then try adding the Hazards.";

        if (Filter.CurrentMultipleRASelection != null && Filter.CurrentMultipleRASelection.ToString() != string.Empty)
        {
            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Please select the Hazard Type and then add the Impact";

            if (type != null && type.ToString() == "2")
            {
                if (General.GetNullableInteger(impacttype) == null)
                    ucError.ErrorMessage = "Impact Type is required.";
            }

            if (General.GetNullableGuid(subhazardid) == null)
                ucError.ErrorMessage = "Impact is required.";

        }

        return (!ucError.IsError);

    }
}

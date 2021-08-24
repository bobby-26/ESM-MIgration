using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobGeneral : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuPlannedMaintenanceJobGeneral.AccessRights = this.ViewState;
            MenuPlannedMaintenanceJobGeneral.MenuList = toolbarmain.Show();
            //MenuPlannedMaintenanceJobGeneral.SetTrigger(pnlPlannedMaintenanceJobGeneral);
            if ((Request.QueryString["JOBID"] != null) && (Request.QueryString["JOBID"] != ""))
            {
                ViewState["JOBID"] = Request.QueryString["JOBID"].ToString();
            }
            if ((Request.QueryString["HIDETOOLBAR"] != null) && (Request.QueryString["HIDETOOLBAR"].ToString() == "1"))
            {
                MenuPlannedMaintenanceJobGeneral.Visible = false;
            }
            if (!IsPostBack)
            {
                BindFields();
                chkPostOverHaulCheck_CheckedChanged(null, null);
                //ucCounterType.HardTypeCode = "111";
                //ucFrequency.HardTypeCode = ((int)(PhoenixHardTypeCode.PERIODICFREQUENCY)).ToString();// "7";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["JOBID"] != null) && (Request.QueryString["JOBID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceJob.EditJob(new Guid((ViewState["JOBID"].ToString())));
                DataRow dr = ds.Tables[0].Rows[0];
                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtJobTitle.Text = dr["FLDJOBTITLE"].ToString();
                ucJobClass.SelectedQuick = dr["FLDJOBCLASS"].ToString();
                ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
                //ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
                //txtFrequency.Text = dr["FLDFREQUENCY"].ToString();
                //ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
                //ucCounterFrequency.Text = dr["FLDCOUNTERVALUES"].ToString();
                ucCategory.SelectedQuick = dr["FLDCATEGORY"].ToString();
                ucRank.SelectedRank = dr["FLDVESSELVERIFYREQUIRED"].ToString();
                chkSupntVerification.Checked = dr["FLDOFFICEVERIFYREQUIRED"].ToString() == "1" ? true : false;
                chkPostOverHaulCheck.Checked = dr["FLDISPOSTOVERHAULCHECK"].ToString() == "1" ? true : false;
                rblInterval.SelectedValue = dr["FLDOVERHAULINTERVAL"].ToString();
                txtDetilsofCheck.Text = dr["FLDPOSTOVERHAULCHECKDESC"].ToString();
                txtInterValue.Text = dr["FLDOVERHAULINTERVALVALUE"].ToString();
                chkIsAttReq.Checked = dr["FLDISATTACHMENTREQUIRED"].ToString() == "1" ? true : false;
                txtInstructions.Visible = dr["FLDISATTACHMENTREQUIRED"].ToString() == "1" ? true : false;
                txtInstructions.Text = dr["FLDATTACHMENTINSTRUCTION"].ToString();
                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private bool IsValidJob()
    {

        ucError.HeaderMessage = "Please provide the following required information.";

        int result;

        if (txtJobCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Job Code can not be blank.";

        if (txtJobTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Job Title can not be blank.";

        if (int.TryParse(ucJobClass.SelectedQuick, out result) == false)
            ucError.ErrorMessage = "Job Class is required.";

        //if (General.GetNullableInteger(ucCounterType.SelectedHard)== null && ucCounterFrequency.Text != "")
        //    ucError.ErrorMessage = "Counter Type is required.";        

        if(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value)
        {
            if (General.GetNullableInteger(txtInterValue.Text) == null)
                ucError.ErrorMessage = "Interval after the Overhaul is required.";

            if (General.GetNullableInteger(rblInterval.SelectedValue) == null)
                ucError.ErrorMessage = "Interval is required.";

            if (General.GetNullableString(txtDetilsofCheck.Text) == null)
                ucError.ErrorMessage = "Details of Check is required.";
        }

        return (!ucError.IsError);
    }


    protected void PlannedMaintenanceJobGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidJob())
                {
                    ucError.Visible = true;
                    return;
                }

                string officeVerify = chkSupntVerification.Checked == true ? "1" : "0";

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixPlannedMaintenanceJob.UpdateJob(new Guid(Request.QueryString["JOBID"])
                        , txtJobCode.Text.Trim()
                        , txtJobTitle.Text.Trim()
                        , Convert.ToInt32(ucJobClass.SelectedQuick)
                        , General.GetNullableInteger(ucWTOApproval.SelectedHard)
                        , null
                       , null
                       , null
                    , null
                    , General.GetNullableInteger(ucCategory.SelectedQuick)
                    , General.GetNullableInteger(ucRank.SelectedRank)
                    , General.GetNullableInteger(officeVerify)
                    , General.GetNullableInteger(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value ? "1" : "0")
                    , txtDetilsofCheck.Text
                    , General.GetNullableInteger(rblInterval.SelectedValue)
                    , General.GetNullableInteger(txtInterValue.Text)
                    , chkIsAttReq.Checked == true ? 1 : 0
                    , chkIsAttReq.Checked == true ?  General.GetNullableString(txtInstructions.Text) : null
                    );
                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {

                    PhoenixPlannedMaintenanceJob.InsertJob(txtJobCode.Text.Trim()
                        , txtJobTitle.Text.Trim()
                        , Convert.ToInt32(ucJobClass.SelectedQuick)
                       , General.GetNullableInteger(ucWTOApproval.SelectedHard)
                       , null
                       , null
                       , null
                    , null
                    , General.GetNullableInteger(ucCategory.SelectedQuick)
                    , General.GetNullableInteger(ucRank.SelectedRank)
                    , General.GetNullableInteger(officeVerify)
                    , General.GetNullableInteger(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value ? "1" : "0")
                    , txtDetilsofCheck.Text
                    , General.GetNullableInteger(rblInterval.SelectedValue)
                    , General.GetNullableInteger(txtInterValue.Text)
                    , chkIsAttReq.Checked == true ? 1 : 0
                    , chkIsAttReq.Checked == true ? General.GetNullableString(txtInstructions.Text) : null
                    );
                }
                //String script = String.Format("javascript:fnReloadList('code1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, false);
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {

                txtJobCode.Text = "";
                txtJobTitle.Text = "";
                ucJobClass.SelectedQuick = "";
                //txtFrequency.Text = "";
                //ucCounterFrequency.Text = "";
                txtInterValue.Text = "";
                rblInterval.SelectedIndex = -1;
                txtDetilsofCheck.Text = "";
                chkPostOverHaulCheck.Checked = false;
                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void chkPostOverHaulCheck_CheckedChanged(object sender, EventArgs e)
    {
        txtDetilsofCheck.Enabled = false;
        rblInterval.Enabled = false;
        lblIntervalValue.Enabled = false;
        if (chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value)
        {
            txtDetilsofCheck.Enabled = true;
            rblInterval.Enabled = true;
            lblIntervalValue.Enabled = true;
        }
    }

    protected void chkIsAttReq_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsAttReq.Checked == true)
        {
            txtInstructions.Visible = true;
        }
        else txtInstructions.Visible = false;
    }
}

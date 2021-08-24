using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewNewApplicantOnboardTrainingList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddButton("Save", "SAVE");
            //MenuCrewOnboardTrainingList.AccessRights = this.ViewState;
            //MenuCrewOnboardTrainingList.MenuList = toolbar.Show();

            if (Request.QueryString["CrewOnboardTrainingId"] != null)
            {
                EditCrewOnboardTraining(new Guid(Request.QueryString["CrewOnboardTrainingId"].ToString()));
            }
            else
                ResetCrewOnboardTraining();
        }
    }

    protected void EditCrewOnboardTraining(Guid employeeonboardtrainingid)
    {
        DataSet ds = PhoenixCrewOnboardTraining.EditOnboardTraining(employeeonboardtrainingid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucSubject.SelectedOnboardTrainingTopic = dr["FLDSUBJECTID"].ToString();
            ucRank.SelectedRank = dr["FLDTRAINERRANKID"].ToString();

            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            txtTrainerName.Text = dr["FLDTRAINERNAME"].ToString();
            txtSubjectName.Text = dr["FLDSUBJECTNAME"].ToString();
            txtDuration.Text = dr["FLDDURATION"].ToString();
        }
    }

    protected void ResetCrewOnboardTraining()
    {
        ucVessel.SelectedVessel = "";
        ucSubject.SelectedOnboardTrainingTopic = "";
        ucRank.SelectedRank = "";

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtRemarks.Text = "";
        txtTrainerName.Text = "";
    }

    //protected void CrewOnboardTrainingList_TabStripCommand(object sender, EventArgs e)
    //{
    //    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
    //    String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //    if (dce.CommandName.ToUpper().Equals("SAVE"))
    //    {
    //        if (IsValidCrewOnboardTraining())
    //        {
    //            if (Request.QueryString["CrewOnboardTrainingId"] != null)
    //            {
    //                PhoenixCrewOnboardTraining.UpdateOnboardTraining(
    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                    , new Guid(Request.QueryString["CrewOnboardTrainingId"].ToString())
    //                    , Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
    //                    , Int32.Parse(ucVessel.SelectedVessel)
    //                    , DateTime.Parse(txtFromDate.Text)
    //                    , DateTime.Parse(txtToDate.Text)
    //                    , Int32.Parse(ucSubject.SelectedOnboardTrainingTopic)
    //                    , General.GetNullableInteger(ucRank.SelectedRank)
    //                    , txtTrainerName.Text
    //                    , txtRemarks.Text);

    //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
    //            }
    //            else
    //            {
    //                PhoenixCrewOnboardTraining.InsertOnboardTraining(
    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                    , Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
    //                    , Int32.Parse(ucVessel.SelectedVessel)
    //                    , DateTime.Parse(txtFromDate.Text)
    //                    , DateTime.Parse(txtToDate.Text)
    //                    , Int32.Parse(ucSubject.SelectedOnboardTrainingTopic)
    //                    , General.GetNullableInteger(ucRank.SelectedRank)
    //                    , txtTrainerName.Text
    //                    , txtRemarks.Text);

    //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
    //                ResetCrewOnboardTraining();
    //            }
    //        }
    //        else
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //    }
    //}

    private bool IsValidCrewOnboardTraining()
    {
        DateTime resultDate;
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if ((!Int32.TryParse(ucVessel.SelectedVessel, out resultInt)) || ucVessel.SelectedVessel == "0")
            ucError.ErrorMessage = "Vessel is required.";
        if ((!Int32.TryParse(ucSubject.SelectedOnboardTrainingTopic, out resultInt)) || ucSubject.SelectedOnboardTrainingTopic == "0")
            ucError.ErrorMessage = "Subject is required.";
        if (!DateTime.TryParse(txtFromDate.Text, out resultDate))
            ucError.ErrorMessage = "Valid From Date is required.";
        if (!DateTime.TryParse(txtToDate.Text, out resultDate))
            ucError.ErrorMessage = "Valid To Date is required.";

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtToDate.Text, out resultDate)))
                if ((DateTime.Parse(txtFromDate.Text)) >= (DateTime.Parse(txtToDate.Text)))
                    ucError.ErrorMessage = "'To Date' should be greater than 'From Date'";
        }

        return (!ucError.IsError);

    }
}

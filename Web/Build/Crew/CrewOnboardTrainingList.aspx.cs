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
public partial class CrewOnboardTrainingList : PhoenixBasePage
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

            if (Request.QueryString["CrewOnboardTrainingId"] != null)
            {
                EditCrewOnboardTraining(new Guid(Request.QueryString["CrewOnboardTrainingId"].ToString()));
            }
            else
            {
                ResetCrewOnboardTraining();

            }
        }
    }

    protected void EditCrewOnboardTraining(Guid employeeonboardtrainingid)
    {
        DataSet ds = PhoenixCrewOnboardTraining.EditOnboardTraining(employeeonboardtrainingid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucVessel.SelectedVessel = "";
            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucSubject.SelectedOnboardTrainingTopic = dr["FLDSUBJECTID"].ToString();            
            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            txtTrainerName.Text = dr["FLDTRAINERNAME"].ToString();
            txtSubjectName.Text = dr["FLDSUBJECTNAME"].ToString();
            txtDuration.Text = dr["FLDDURATION"].ToString();
            txtTrainerRank.Text = dr["FLDRANKNAME"].ToString();
            txtTrainerNameNotinList.Text = dr["FLDTRAINERNAMENOTINLIST"].ToString();
            
        }
    }

    protected void ResetCrewOnboardTraining()
    {
        ucVessel.SelectedVessel = "";
        ucSubject.SelectedOnboardTrainingTopic = "";        
        txtTrainerNameNotinList.Text = "";
        txtTrainerRank.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtRemarks.Text = "";
        txtTrainerName.Text = "";
    }
    
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

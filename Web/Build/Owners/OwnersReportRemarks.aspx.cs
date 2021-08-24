using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Owners_OwnersReportRemarks : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    DateTime currentDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = int.Parse(Filter.SelectedOwnersReportVessel);

        if (Filter.SelectedOwnersReportDate != null)
        {
            currentDate = Convert.ToDateTime(Filter.SelectedOwnersReportDate);
        }
        else
        {
            currentDate = DateTime.Now;
        }

        if (IsPostBack == false)
        {
            loadRemarks();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        // on signature save and retrive the data
        loadRemarks();
    }


    private void loadRemarks()
    {
        DataTable dt = PhoenixOwnerReport.OwnersReportRemarksSearch(vesselId, currentDate);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            txtTechnicalRemarks.Text = row["FLDTECHREMARKS"] == DBNull.Value ? string.Empty : (string)row["FLDTECHREMARKS"];
            txtMarineRemarks.Text = row["FLDMARINEREMARKS"] == DBNull.Value ? string.Empty : (string)row["FLDMARINEREMARKS"];
            txtFleetRemarks.Text = row["FLDFLEETREMARKS"] == DBNull.Value ? string.Empty : (string)row["FLDFLEETREMARKS"];
            txtowner.Text = row["FLDOWNERREMARKS"] == DBNull.Value ? string.Empty : (string)row["FLDOWNERREMARKS"];


            lblTechnicalSignName.Text = row["FLDTECHSUPTNAME"] == DBNull.Value ? string.Empty : (string)row["FLDTECHSUPTNAME"];
            lblMarineSignName.Text = row["FLDMARINESUPTNAME"] == DBNull.Value ? string.Empty : (string)row["FLDMARINESUPTNAME"];
            lblFleetName.Text = row["FLDFLEETSUPTNAME"] == DBNull.Value ? string.Empty : (string)row["FLDFLEETSUPTNAME"];
            lblownername.Text = row["FLDOWNERNAME"] == DBNull.Value ? string.Empty : (string)row["FLDOWNERNAME"];

            lblTechnicalDate.Text = row["FLDTECHSIGNEDDATE"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["FLDTECHSIGNEDDATE"]).ToString("dd-MM-yyyy");
            lblMarineSignDate.Text = row["FLDMARINESIGNEDDATE"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["FLDMARINESIGNEDDATE"]).ToString("dd-MM-yyyy");
            lblFleetDate.Text = row["FLDFLEETSIGNEDDATE"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["FLDFLEETSIGNEDDATE"]).ToString("dd-MM-yyyy");
            lblownerdate.Text = row["FLDOWNERSIGNEDDATE"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["FLDOWNERSIGNEDDATE"]).ToString("dd-MM-yyyy");

            // if signed need to disable the textarea
            txtTechnicalRemarks.Enabled = !Convert.ToBoolean(row["FLDTECHSIGNEDYN"]);
            txtMarineRemarks.Enabled = !Convert.ToBoolean(row["FLDMARINESIGNEDYN"]);
            txtFleetRemarks.Enabled = !Convert.ToBoolean(row["FLDFLEETSIGNEDYN"]);
            txtowner.Enabled = !Convert.ToBoolean(row["FLDOWNERSIGNEDYN"]);
        }
    }

    private void SetPopupForSignature(string commandName)
    {
        string popupName = string.Empty;
        string userdesignation = commandName;
        string remarks = string.Empty;
        switch (commandName)
        {
            case "TECHNICAL":
                popupName = "Technical SuperIntedent";
                remarks = txtTechnicalRemarks.Text;
                break;
            case "MARINE":
                popupName = "Marine SuperIntedent";
                remarks = txtMarineRemarks.Text;
                break;
            case "FLEETMANAGER":
                popupName = "Fleet Manager";
                remarks = txtFleetRemarks.Text;
                break;
            case "OWNER":
                popupName = "Owner";
                remarks = txtFleetRemarks.Text;
                break;
        }

        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Owners/OwnersReportSignature.aspx?popuptitle={0}&designation={1}&remarks={2}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, userdesignation, remarks);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUpClose", script, true);
    }

    private void verifyAlreadySigned(string commandName)
    {
        DataTable dt = PhoenixOwnerReport.OwnersReportRemarksSearch(vesselId, currentDate);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            if (commandName == "TECHNICAL" && Convert.ToBoolean(row["FLDTECHSIGNEDYN"]))
            {
                throw new ArgumentException("Technical SuperIntedent is already signed");
            }

            if (commandName == "MARINE" && Convert.ToBoolean(row["FLDMARINESIGNEDYN"]))
            {
                throw new ArgumentException("Marine SuperIntedent is already signed");
            }

            if (commandName == "FLEETMANAGER" && Convert.ToBoolean(row["FLDFLEETSIGNEDYN"]))
            {
                throw new ArgumentException("Fleet Manager is already signed");
            }

            if (commandName == "OWNER" && Convert.ToBoolean(row["FLDOWNERSIGNEDYN"]))
            {
                throw new ArgumentException("Owner is already signed");
            }


        }

        if (currentDate.Month == DateTime.Now.Month && currentDate.Year == DateTime.Now.Year)
        {
            throw new ArgumentException("signature not allowed for current month");
        }

        if (commandName == "TECHNICAL" && string.IsNullOrWhiteSpace(txtTechnicalRemarks.Text))
        {
            throw new ArgumentException("Remarks is required");
        }

        if (commandName == "MARINE" && string.IsNullOrWhiteSpace(txtMarineRemarks.Text))
        {
            throw new ArgumentException("Remarks is required");
        }

        if (commandName == "FLEETMANAGER" && string.IsNullOrWhiteSpace(txtFleetRemarks.Text))
        {
            throw new ArgumentException("Remarks is required");
        }

        if (commandName == "OWNER" && string.IsNullOrWhiteSpace(txtowner.Text))
        {
            throw new ArgumentException("Remarks is required");
        }
    }

    protected void btnSign_Command(object sender, CommandEventArgs e)
    {
        try
        {
            verifyAlreadySigned(e.CommandName);
            SetPopupForSignature(e.CommandName);
        }
        catch (Exception ex) 
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnTechincalSign_Click(object sender, EventArgs e)
    {
        try
        {
            var button = (Button)sender;
            verifyAlreadySigned(button.CommandName);
            SetPopupForSignature(button.CommandName);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
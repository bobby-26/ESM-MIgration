using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogFuelChangeOver : PhoenixBasePage
{
    int usercode;
    int vesselid;
    Guid logBookId = Guid.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        ShowToolBar();

        if (string.IsNullOrWhiteSpace(Request.QueryString["LogBookId"]) == false)
        {
            logBookId = Guid.Parse(Request.QueryString["LogBookId"]);
        }

        LoadTankDetails();

        if (IsPostBack == false)
        {
            ViewState["TankCount"] = 0;
            LoadEntryExit();
            BindData();
            dtDateTimeStart.SelectedDate = DateTime.Now;
            dtDateTimeStart.MaxDate = DateTime.Now;
            dtDateTimeCompleted.SelectedDate = DateTime.Now;
            dtDateTimeCompleted.MaxDate = DateTime.Now;
            dtEntryExitDate.SelectedDate = DateTime.Now;
            dtEntryExitDate.MaxDate = DateTime.Now;

        }
    }

    private void BindData()
    {
        if (logBookId != Guid.Empty)
        {
            DataSet ds = PhoenixLogFuelChangeOver.FuelOilChangeOverTransactionEdit(vesselid, logBookId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                RadComboBoxItem item = ddlEntry.Items.FindItem(x => x.Value == row["FLDTRANSACTIONTYPE"].ToString());
                if (item != null)
                {
                    item.Selected = true;
                }

                txtBDNBefore.Text = row["FLDBEFOREFUELBDN"].ToString();
                txtBDNAfter.Text = row["FLDAFTERFUELBDN"].ToString();
                txtSulphurBefore.Text = row["FLDBEFOREFUELSULPHUR"].ToString();
                txtSulphurAfter.Text = row["FLDAFTERFUELSULPHUR"].ToString();

                txtTypeBefore.Text = row["FLDBEFOREFUELTYPE"].ToString();
                txtTypeAfter.Text = row["FLDAFTERFUELTYPE"].ToString();

                dtDateTimeStart.SelectedDate = Convert.ToDateTime(row["FLDSTARTDATE"]);
                txtStartTime.SelectedDate = Convert.ToDateTime(row["FLDSTARTDATE"]);

                dtDateTimeCompleted.SelectedDate = Convert.ToDateTime(row["FLDCOMPLETEDATE"]);
                txtCompletedTime.SelectedDate = Convert.ToDateTime(row["FLDCOMPLETEDATE"]);

                dtEntryExitDate.SelectedDate = Convert.ToDateTime(row["FLDENTRYDATE"]);
                txtEntryExitTime.SelectedDate = Convert.ToDateTime(row["FLDENTRYDATE"]);

                txtStartPosistion.Text = row["FLDSTARTLOCATION"].ToString();
                txtCompletedPosistion.Text = row["FLDCOMPLETELOCATION"].ToString();
                txtEntryPosistion.Text = row["FLDENTRYLOCATION"].ToString();

                txtMachinery.Text = row["FLDMACHINERY"].ToString();
            }
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    private void LoadEntryExit()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Entry", "ENTRY");
        data.Add("Exit", "EXIT");
        ddlEntry.DataSource = data;
        ddlEntry.DataTextField = "Key";
        ddlEntry.DataValueField = "Value";
        ddlEntry.DataBind();
    }

    private void LoadTankDetails()
    {
        DataTable dt = PhoenixLogFuelChangeOver.FuelOilChangeOverTankROBList(vesselid, logBookId);
        if (dt.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt.Rows.Count;
            rptrTank.DataSource = dt;
            rptrTank.DataBind();
        }
        
    }

    private void OnDutyEngineerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;

            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincsign.Text = string.Format("{0}-{1} {2}", nvc.Get("rank"), nvc.Get("name"), signedDate.ToString("dd-MM-yyyy"));
            lblinchName.Text = nvc.Get("name");
            lblincRank.Text = nvc.Get("rank");
            lblinchId.Text = nvc.Get("id");
            lblincSignDate.Text = signedDate.ToString();
            Filter.DutyEngineerSignatureFilterCriteria = null;

            btnInchargeSign.Visible = false;
            lblincsign.Visible = true;

        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false || ValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                if (logBookId == Guid.Empty)
                {
                    logBookEntryInsert();
                }
                else
                {
                    logBookEntryUpdate();
                }

                // close the popup and refresh the list
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "EXCEED85")
                RadWindowManager1.RadConfirm("Warning. Tank Reached its 85 percentage capacity", "confirm", 320, 150, null, "Exceed");
            else
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void logBookEntryUpdate()
    {
        
        Decimal sulphurBefore = Convert.ToDecimal(txtSulphurBefore.Text);
        Decimal sulphurAfter = Convert.ToDecimal(txtSulphurAfter.Text);

        DateTime StartDateTime = dtDateTimeStart.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime CompletedDateTime = dtDateTimeCompleted.SelectedDate.Value.Add(txtCompletedTime.SelectedTime.Value);
        DateTime entryExitDateTime = dtEntryExitDate.SelectedDate.Value.Add(txtEntryExitTime.SelectedTime.Value);

        PhoenixLogFuelChangeOver.FuelChangeOverUpdate(vesselid, logBookId, txtTypeBefore.Text, sulphurBefore, txtBDNBefore.Text, txtTypeBefore.Text,
            sulphurAfter, txtBDNAfter.Text, StartDateTime, txtStartPosistion.Text, CompletedDateTime, txtCompletedPosistion.Text, entryExitDateTime,
            txtEntryPosistion.Text, txtMachinery.Text
            , General.GetNullableInteger(lblinchId.Text)
            , General.GetNullableString(lblinchName.Text)
            , General.GetNullableString(lblincRank.Text));


        foreach (RepeaterItem ri in rptrTank.Items)
        {
            RadLabel lblTankId = ri.FindControl("lblTankId") as RadLabel;
            RadLabel lblRobId = ri.FindControl("lblROBId") as RadLabel;
            RadNumericTextBox txtTankROB = ri.FindControl("txtTankROB") as RadNumericTextBox;
            Decimal rob = Convert.ToDecimal(txtTankROB.Text);

           PhoenixLogFuelChangeOver.FuelChangeOverRobUpdate(vesselid,  Guid.Parse(lblRobId.Text), logBookId, Guid.Parse(lblTankId.Text), rob);
        }

    }

    private void logBookEntryInsert()
    {

        Decimal sulphurBefore = Convert.ToDecimal(txtSulphurBefore.Text);
        Decimal sulphurAfter = Convert.ToDecimal(txtSulphurAfter.Text);

        DateTime StartDateTime = dtDateTimeStart.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime CompletedDateTime = dtDateTimeCompleted.SelectedDate.Value.Add(txtCompletedTime.SelectedTime.Value);
        DateTime entryExitDateTime = dtEntryExitDate.SelectedDate.Value.Add(txtEntryExitTime.SelectedTime.Value);

        Guid LogId = Guid.Empty;

        PhoenixLogFuelChangeOver.FuelChangeOverInsert(vesselid, ddlEntry.SelectedItem.Value, txtTypeBefore.Text, sulphurBefore, txtBDNBefore.Text, txtTypeBefore.Text, 
            sulphurAfter, txtBDNAfter.Text, StartDateTime, txtStartPosistion.Text, CompletedDateTime, txtCompletedPosistion.Text, entryExitDateTime,
            txtEntryPosistion.Text, txtMachinery.Text, ref LogId
            ,General.GetNullableInteger(lblinchId.Text)
            ,General.GetNullableString(lblinchName.Text)
            ,General.GetNullableString(lblincRank.Text));


        foreach (RepeaterItem ri in rptrTank.Items)
        {
            RadLabel lblTankId = ri.FindControl("lblTankId") as RadLabel;
            RadNumericTextBox txtTankROB = ri.FindControl("txtTankROB") as RadNumericTextBox;
            Decimal rob = Convert.ToDecimal(txtTankROB.Text);

            PhoenixLogFuelChangeOver.FuelChangeOverRobInsert(vesselid, LogId, Guid.Parse(lblTankId.Text), rob);
        }

    }

    private bool isValidInput()
    {
        

        if (string.IsNullOrWhiteSpace(txtTypeBefore.Text))
        {
            ucError.ErrorMessage = "Before Type is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtTypeAfter.Text))
        {
            ucError.ErrorMessage = "After type is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtSulphurBefore.Text))
        {
            ucError.ErrorMessage = "Sulphur before is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtSulphurAfter.Text))
        {
            ucError.ErrorMessage = "Sulphur after is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtBDNBefore.Text))
        {
            ucError.ErrorMessage = "BDN before is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtBDNAfter.Text))
        {
            ucError.ErrorMessage = "BDN after is Required ";
        }

        if (dtDateTimeStart.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start ChangeOver date is Required ";
        }

        if (dtDateTimeCompleted.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Completed ChangeOver date is Required ";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Changeover Time is Required ";
        }

        if (txtCompletedTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Completed Changeover is Required ";
        }

        if (String.IsNullOrWhiteSpace(txtStartPosistion.Text))
        {
            ucError.ErrorMessage = "Start Position is Required ";
        }

        if (String.IsNullOrWhiteSpace(txtCompletedPosistion.Text))
        {
            ucError.ErrorMessage = "Completed Position is Required ";
        }

        if (dtEntryExitDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtEntryPosistion.Text))
        {
            ucError.ErrorMessage = "Entry Exist Position is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtMachinery.Text))
        {
            ucError.ErrorMessage = "Machinery Which had FO is Required ";
        }

        foreach (RepeaterItem item in rptrTank.Items)
        {
            RadNumericTextBox txtcapacity = (RadNumericTextBox)item.FindControl("txtTankROB");
            if (string.IsNullOrWhiteSpace(txtcapacity.Text))
            {
                ucError.ErrorMessage = "Tank ROB is Required";
                break;
            }
        }

        return (!ucError.IsError);
    }

    private bool ValidateSignature()
    {
        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Incharge Sign is Required ";
        }

        return (!ucError.IsError);
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }
        string popupName = "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        btnInchargeSign.Attributes.Add("onclick", script);

    }
}
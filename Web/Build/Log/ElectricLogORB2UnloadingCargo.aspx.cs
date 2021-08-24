using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

public partial class Log_ElectricLogORB2UnloadingCargo : PhoenixBasePage
{
    string ReportCode;
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null; 
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "UnloadingCargo");
        confirm.Attributes.Add("style", "display:none;");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            txtOperationDate.Enabled = false;
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["isMissedOperation"]) == false)
        {
            isMissedOperation = Convert.ToBoolean(Request.QueryString["isMissedOperation"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationEdit"]) == false)
        {
            isMissedOperationEdit = Convert.ToBoolean(Request.QueryString["missedOperationEdit"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationDate"]) == false)
        {
            missedOperationDate = Convert.ToDateTime(Request.QueryString["missedOperationDate"]);
        }
        ShowToolBar();
        LoadTankDetails();

        if (IsPostBack == false)
        {
            ViewState["lastTranscationDate"] = null;
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            LoadNotes();
            BindData();
        }
    }

    private void LoadNotes()
    {
        DataTable dt = PhoenixMarbolLogORB2.ORB2LogRegisterEdit(usercode, General.GetNullableInteger(logId.ToString()));
        if (dt.Rows.Count > 0)
        {
            lblnotes.Text = dt.Rows[0]["FLDNOTES"].ToString();
        }
    }

    private void BindData()
    {
        if (txid != Guid.Empty)
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, txid);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 12)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 13)
                    {
                        txtUnloadTerminal.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 14)
                    {
                        txtTotalQtyUnloaded.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 15)
                    {
                        txtGradeOfCargo.Text = row["FLDVALUE"].ToString();
                    }

                }

                // load cargo on tank list
                SetDefaultData();
            }
        }
    }

    private void LoadTankDetails()
    {

        DataTable dt = PhoenixMarbolLogORB2.CargoLoadingQuantitySearch(usercode, vesselId, txid != Guid.Empty ? General.GetNullableGuid(txid.ToString()) : null);

        DataTable dtload = SelectTopDataRow(dt, 0, 9);

        if (dtload.Rows.Count > 0)
        {
            rptrTank.DataSource = dtload;
            rptrTank.DataBind();
        }

        DataTable dt2ndload = SelectTopDataRow(dt, 10, 19);

        if (dt2ndload.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt2ndload.Rows.Count;
            rptrTank20.DataSource = dt2ndload;
            rptrTank20.DataBind();
        }

        DataTable dt3ndload = SelectTopDataRow(dt, 20, 29);

        if (dt3ndload.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt3ndload.Rows.Count;
            rptrTank30.DataSource = dt3ndload;
            rptrTank30.DataBind();
        }

        DataTable dt4ndload = SelectTopDataRow(dt, 30, 39);

        if (dt4ndload.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt4ndload.Rows.Count;
            rptrTank40.DataSource = dt4ndload;
            rptrTank40.DataBind();
        }

    }

    public DataTable SelectTopDataRow(DataTable dt, int from, int to)
    {
        DataTable dtn = dt.Clone();
        for (int i = from; (i <= to && i < dt.Rows.Count); i++)
        {
            dtn.ImportRow(dt.Rows[i]);
        }

        return dtn;
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

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }

    private void SetDefaultData()
    {
        ReportCode = "C";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = "6";
        txtItemNo1.Text = "7";
        txtItemNo2.Text = "8";
        txtCode.Text = ReportCode;
        ShowCargoTank();

        lblRecord.Text = string.Format("<b>{0}</b>", txtUnloadTerminal.Text);
        lblrecord1.Text = string.Format("<b>{0}</b>", ViewState["TankName"].ToString().TrimEnd(','));
        if (ViewState["TankLoaded"].ToString() != "")
        {
            if (decimal.Parse(ViewState["SumQuantity"].ToString()) > 0)
            {
                lblrecord2.Text = string.Format("<span class='left'><b>NO </b></span>  <span class='right'><b> {0}</b></span>", ViewState["TankLoaded"].ToString().TrimEnd(','));
            }
            else
                lblrecord2.Text = string.Format("<span class='left'><b>YES</b></span>");
        }
        else
            lblrecord2.Text = string.Format("<span class='left'><b>YES</b></span>");
    }

    private void ShowCargoTank()
    {
        // form the dynamic tank
        ViewState["TankName"] = string.Empty;
        ViewState["TankLoaded"] = string.Empty;
        ShowCargoTankDetails(rptrTank);
        ShowCargoTankDetails(rptrTank20);
        ShowCargoTankDetails(rptrTank30);
        ShowCargoTankDetails(rptrTank30);
    }

    private void ShowCargoTankDetails(Repeater rptname)
    {
        if (rptname.Items.Count > 0)
        {
            ViewState["SumQuantity"] = 0;
            foreach (RepeaterItem ri in rptname.Items)
            {
                RadLabel lblTankName = ri.FindControl("lblTankName") as RadLabel;
                RadNumericTextBox txtLoadedQty = ri.FindControl("txtLoadedQty") as RadNumericTextBox;

                string loadedQty = string.IsNullOrWhiteSpace(txtLoadedQty.Text) ? "0" : txtLoadedQty.Text;
                string tankCargo = string.Format("{0} = {1} m3,", lblTankName.Text, loadedQty);

                if (loadedQty != "0")
                {
                    ViewState["TankName"] += lblTankName.Text + " ,";
                    ViewState["TankLoaded"] += " " + tankCargo;
                    ViewState["SumQuantity"] = decimal.Parse(ViewState["SumQuantity"].ToString()) + decimal.Parse(loadedQty);
                }
            }
        }

    }
    private void CargoTankCapcityValidate(Repeater rptname)
    {

        foreach (RepeaterItem ri in rptname.Items)
        {
            RadLabel lblTankId = ri.FindControl("lblTankId") as RadLabel;
            RadNumericTextBox txtLoadedQty = ri.FindControl("txtLoadedQty") as RadNumericTextBox;

            if (lblTankId.Text != "")
            {
                PhoenixMarbolLogORB2.InsertTransactionORB2Validation(usercode
                                    , new Guid(lblTankId.Text)
                                    , null
                                    , txtLoadedQty.Text
                                    , vesselId
                                    , General.GetNullableInteger(lblinchId.Text)
                                    );
            }
        }

    }


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false && isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                //Validate transaction Capacity
                TransactionValidation();
                SaveTransaction();

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

    private void TransactionValidation()
    {
        CargoTankCapcityValidate(rptrTank);
        CargoTankCapcityValidate(rptrTank20);
        CargoTankCapcityValidate(rptrTank30);
        CargoTankCapcityValidate(rptrTank30);
    }

    private void SaveTransaction()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        SetDefaultData();

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate(logId);
            MissedOperationalEntryTemplateUpdate(txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate(logId);
            LogBookUpdate(logId);
        }
        else
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);

            // book entry insert
            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            txtCode.Text,
                            txtItemNo.Text,
                            lblRecord.Text,
                            1
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo1.Text,
                            lblrecord1.Text,
                            2
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo2.Text,
                            lblrecord2.Text,
                            3
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            null,
                            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), false),
                            4,
                            true
                );

            // history insert
            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                            , vesselId
                            , logId
                            , logTxId
                            , General.GetNullableInteger(lblinchId.Text)
                            , General.GetNullableString(lblincRank.Text)
                            , General.GetNullableString(lblincsign.Text)
                            , General.GetNullableDateTime(lblincSignDate.Text)
                        );

        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidInput() == false)
            {
                ucError.Visible = true;
                return;
            }

            SaveTransaction();

            // close the popup and refresh the list
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "3");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void LogBookUpdate(int logId)
    {

        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord.Text,
            1
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord1.Text,
            2
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord2.Text,
            3
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            4
        );

        // history update
        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                        , vesselId
                        , logId
                        , txid
                        , General.GetNullableInteger(lblinchId.Text)
                        , General.GetNullableString(lblincRank.Text)
                        , General.GetNullableString(lblincsign.Text)
                        , General.GetNullableDateTime(lblincSignDate.Text)
                    );
    }

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(usercode,
                vesselId,
                logId,
                12,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarbolLogORB2.TransactionUpdate(usercode,
                vesselId,
                logId,
                13,
                txid,
                entrydate,
                txtUnloadTerminal.Text);


        PhoenixMarbolLogORB2.TransactionUpdate(usercode,
                vesselId,
                logId,
                14,
                txid,
                entrydate,
                txtTotalQtyUnloaded.Text);


        PhoenixMarbolLogORB2.TransactionUpdate(usercode,
                vesselId,
                logId,
                15,
                txid,
                entrydate,
                txtGradeOfCargo.Text);

        CargoLoadingTankUpdate(rptrTank);
        CargoLoadingTankUpdate(rptrTank20);
        CargoLoadingTankUpdate(rptrTank30);
        CargoLoadingTankUpdate(rptrTank40);
    }

    private void CargoLoadingTankUpdate(Repeater rptname)
    {
        foreach (RepeaterItem ri in rptname.Items)
        {
            RadLabel lblTankId = ri.FindControl("lblTankId") as RadLabel;
            RadNumericTextBox txtLoadedQty = ri.FindControl("txtLoadedQty") as RadNumericTextBox;

            if (lblTankId.Text != "")
            {
                PhoenixMarbolLogORB2.CargoLoadingUpdate(
                    usercode,
                    vesselId,
                    txid,
                    Guid.Parse(lblTankId.Text),
                    txtLoadedQty.Text == "" ? 0 : Convert.ToDecimal(txtLoadedQty.Text),
                    true
                );
            }
        }
    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            12,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            13,
                            logTxId,
                            entrydate,
                            txtUnloadTerminal.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            14,
                            logTxId,
                            entrydate,
                            txtTotalQtyUnloaded.Text
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            15,
                            logTxId,
                            entrydate,
                            txtGradeOfCargo.Text
            );

        // insert the tank list

        CargoLoadingTankInsert(rptrTank, logTxId);
        CargoLoadingTankInsert(rptrTank20, logTxId);
        CargoLoadingTankInsert(rptrTank30, logTxId);
        CargoLoadingTankInsert(rptrTank40, logTxId);

    }

    private void CargoLoadingTankInsert(Repeater rptname, Guid logTxId)
    {
        foreach (RepeaterItem ri in rptname.Items)
        {
            RadLabel lblTankId = ri.FindControl("lblTankId") as RadLabel;
            RadNumericTextBox txtLoadedQty = ri.FindControl("txtLoadedQty") as RadNumericTextBox;

            if (lblTankId.Text != "")
            {
                PhoenixMarbolLogORB2.CargoLoadingInsert(
                        usercode,
                        vesselId,
                        logTxId,
                        Guid.Parse(lblTankId.Text),
                        txtLoadedQty.Text == "" ? 0 : Convert.ToDecimal(txtLoadedQty.Text),
                        true
                    );
            }
        }
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        if (string.IsNullOrWhiteSpace(txtUnloadTerminal.Text))
        {
            ucError.ErrorMessage = "Unload Terminal is Required";
        }

        if (string.IsNullOrWhiteSpace(txtTotalQtyUnloaded.Text))
        {
            ucError.ErrorMessage = "Total Quantity Unloaded is Required";
        }

        if (string.IsNullOrWhiteSpace(txtGradeOfCargo.Text))
        {
            ucError.ErrorMessage = "Grade of Cargo is Required";
        }


        // validate tanks

        //foreach (RepeaterItem ri in rptrTank.Items)
        //{
        //    RadLabel lblTankId = ri.FindControl("lblTankId") as RadLabel;
        //    RadNumericTextBox txtLoadedQty = ri.FindControl("txtLoadedQty") as RadNumericTextBox;
        //    if (string.IsNullOrWhiteSpace(txtLoadedQty.Text))
        //    {
        //        ucError.ErrorMessage = "Cargo Tank value is Required Before Save";
        //        break;
        //    }
        //}


        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTransaction();
        }
    }

    protected void txtLoadedQty_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        SetDefaultData();
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        //string popupName = "Log";
        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string rank = "co";
        string popupTitle = "Chief Officer Signature";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}&isMissedOperation={3}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle, isMissedOperation);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }

    private bool isValidateSignature()
    {

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

}
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2DischargeOfCleaningBallast : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "58";
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
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "DischargeOfCleanBallast");        

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

        if (IsPostBack == false)
        {
            ViewState["lastTranscationDate"] = null;
            ddlCargoPopulate();
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 91) { txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]); }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 92) {
                        RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 93) { txtDischargeStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]); }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 94) { txtDischargeStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]); }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 95) { txtUTCStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]); }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 96) { txtUTCStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]); }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 97) { txtTankEmpty.Text = (string)row["FLDVALUE"]; }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 98) { txtStartPosistionLat.Text    =  (string)row["FLDVALUE"]; }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 99) { txtStartPosistionLog.Text = (string)row["FLDVALUE"]; }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 100) { txtStopPosistionLat.Text = (string)row["FLDVALUE"]; }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 133) { txtStopPosistionLog.Text = (string)row["FLDVALUE"]; }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 134) { txtRegularCheck.Text = (string)row["FLDVALUE"]; }

                }


                SetDefaultData();
            }
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

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ddlCargoPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
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
        if (ddlTransferFrom.SelectedItem == null) return;

        ReportCode = "K";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;

        txtItemNo.Text = ItemNo;
        txtItemNo1.Text = "59";
        txtItemNo2.Text = "60";
        txtItemNo3.Text = "61";
        txtItemNo4.Text = "62";

        //TimeSpan startTime = txtDischargeStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDischargeStartTime.SelectedTime.Value;
        //TimeSpan stopTime = txtDischargeStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDischargeStopTime.SelectedTime.Value;

        lblRecord.Text = string.Format("Start at <b>{0}, {1}</b> ", txtStartPosistionLat.Text, txtStartPosistionLog.Text);
        lblrecord1.Text = string.Format("<b>{0}</b> Cargo Tank", ddlTransferFrom.Text);
        //lblrecord2.Text = string.Format(" <b>{0}</b> Hours", Math.Abs(stopTime.Hours - startTime.Hours));
        lblrecord2.Text = string.Format(" <b>{0}</b> ", txtTankEmpty.Text);
        lblrecord3.Text = string.Format("Stop at <b>{0}, {1}</b> ", txtStopPosistionLat.Text, txtStopPosistionLog.Text);
        lblrecord4.Text = string.Format("<b>{0}</b>", txtRegularCheck.Text);
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                if (isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

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

    private void SaveTransaction()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logId, logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate(logId);
            MissedOperationalEntryTemplateUpdate(logId, txid);
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
                            txtItemNo3.Text,
                            lblrecord3.Text,
                            4
                );

            PhoenixMarbolLogORB2.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo4.Text,
                            lblrecord4.Text,
                            5
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
                            6,
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

    private void MissedOperationalEntryTemplateUpdate(int logId,Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);
        nvc.Add("Record4", lblrecord3.Text);
        nvc.Add("Record5", lblrecord4.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "5");
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
            lblrecord3.Text,
            4
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord4.Text,
            5
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            6
        );

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

        PhoenixMarbolLogORB2.TransactionUpdate(
                           usercode,
                           vesselId,
                           logId,
                           91,
                           txid,
                           entrydate,
                           entrydate.ToString()
           );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            92,
                            txid,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            93,
                            txid,
                            entrydate,
                            txtDischargeStartTime.SelectedDate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            94,
                            txid,
                            entrydate,
                            txtDischargeStopTime.SelectedDate.ToString()
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            95,
                            txid,
                            entrydate,
                            txtUTCStartTime.SelectedDate.ToString()
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            96,
                            txid,
                            entrydate,
                            txtUTCStopTime.SelectedDate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            97,
                            txid,
                            entrydate,
                            txtTankEmpty.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            98,
                            txid,
                            entrydate,
                            txtStartPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            99,
                            txid,
                            entrydate,
                            txtStartPosistionLog.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            100,
                            txid,
                            entrydate,
                            txtStopPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            133,
                            txid,
                            entrydate,
                            txtStopPosistionLog.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                    usercode,
                    vesselId,
                    logId,
                    134,
                    txid,
                    entrydate,
                    txtRegularCheck.Text
        );



    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            91,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            92,
                            logTxId,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            93,
                            logTxId,
                            entrydate,
                            txtDischargeStartTime.SelectedDate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            94,
                            logTxId,
                            entrydate,
                            txtDischargeStopTime.SelectedDate.ToString()
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            95,
                            logTxId,
                            entrydate,
                            txtUTCStartTime.SelectedDate.ToString()
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            96,
                            logTxId,
                            entrydate,
                            txtUTCStopTime.SelectedDate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            97,
                            logTxId,
                            entrydate,
                            txtTankEmpty.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            98,
                            logTxId,
                            entrydate,
                            txtStartPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            99,
                            logTxId,
                            entrydate,
                            txtStartPosistionLog.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            100,
                            logTxId,
                            entrydate,
                            txtStopPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            133,
                            logTxId,
                            entrydate,
                            txtStopPosistionLog.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                    usercode,
                    vesselId,
                    logId,
                    134,
                    logTxId,
                    entrydate,
                    txtRegularCheck.Text
        );

    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (txtDischargeStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Discharge Start Time is required";
        }

        if (txtDischargeStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Discharge Stop Time is required";
        }

        if (txtUTCStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Start Time is required";
        }

        if (txtUTCStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Stop Time is required";
        }

        if (string.IsNullOrWhiteSpace(txtTankEmpty.Text))
        {
            ucError.ErrorMessage = "Tank empty on completion is required";
        }

        if (string.IsNullOrWhiteSpace(txtRegularCheck.Text))
        {
            ucError.ErrorMessage = "Regular check carried out is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text))
        {
            ucError.ErrorMessage = "Start Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position Longitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLat.Text))
        {
            ucError.ErrorMessage = "Stop Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLog.Text))
        {
            ucError.ErrorMessage = "Stop Position Longitude is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtAfrTrnsROBFrom_TextChanged(object sender, EventArgs e)
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

    protected void ddlWaterTransferedFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
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


    protected void txtTankEmpty_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
}
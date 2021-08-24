using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class Log_ElectricLogCRBDischargeOfCleaningBallast : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "22";
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "DischargeBallastWater");

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

        ShowToolBar();

        if (IsPostBack == false)
        {
            ddlFromPopulate();
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
            DataTable dt = PhoenixMarpolLogCRB.TransactionEdit(usercode, txid);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 50)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 51)
                    {
                        RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 52)
                    {
                        ddldischargeballast.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 53)
                    {
                        ddldischargeport.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 54)
                    {
                        txtReceptionName.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 55)
                    {
                        txtDisStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 56)
                    {
                        txtDisStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 57)
                    {
                        txtUTCStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 58)
                    {
                        txtUTCStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }
                    
                    if (Convert.ToInt32(row["FLDITEMID"]) == 59)
                    {
                        txtspeed.Text = row["FLDVALUE"].ToString();
                    }
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
        ViewState["lastTranscationDate"] = PhoenixMarpolLogCRB.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarpolLogCRB.ElogLocationDropDown(vesselId, usercode, null);
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
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }

    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        TimeSpan disstartTime = txtDisStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDisStartTime.SelectedTime.Value;
        TimeSpan disstopTime = txtDisStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDisStopTime.SelectedTime.Value;
        TimeSpan utcstartTime = txtUTCStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStartTime.SelectedTime.Value;
        TimeSpan utcstopTime = txtUTCStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStopTime.SelectedTime.Value;

        ReportCode = "H";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "23.1";
        txtItemNo2.Text = "23.2";
        txtItemNo3.Text = "24";
        txtItemNo4.Text = "25";
        lblRecord.Text = string.Format("<b>{0}</b>", ddlTransferFrom.SelectedItem.Text);
        if (ddldischargeballast.SelectedValue == "Yes")
        {
            lblrecord1.Text = string.Format("<b>Discharged into Sea</b>");
        }
        else
        {
            lblrecord1.Text = "<b>NA</b>";
        }
        if (ddldischargeport.SelectedValue == "Yes")
        {
            lblrecord2.Text = string.Format("<b>To {0}</b>", txtReceptionName.Text);
        }
        else
        {
            lblrecord2.Text = "<b>NA</b>";
        }
        lblrecord3.Text = string.Format("<b> Commenced Pumping: {0} LT / {1} UTC, Completed: {2} LT / {3} UTC</b>", disstartTime.ToString(), utcstartTime.ToString(), disstopTime.ToString(), utcstopTime.ToString());
        lblrecord4.Text = string.Format("<b>{0} Knots</b>", txtspeed.Text);
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false || IsValidSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                SaveTxin();
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

    private void SaveTxin()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

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
            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
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

            PhoenixMarpolLogCRB.BookEntryInsert(
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
            PhoenixMarpolLogCRB.LogCRBBookEntryStatusInsert(usercode
                            , vesselId
                            , logId
                            , logTxId
                            , General.GetNullableInteger(lblinchId.Text)
                            , General.GetNullableString(lblincRank.Text)
                            , General.GetNullableString(lblincsign.Text)
                            , General.GetNullableDateTime(lblincSignDate.Text)
                        );


            PhoenixMarpolLogCRB.LogCRBHistoryInsert(usercode
                             , vesselId
                             , logId
                             , txid
                             , General.GetNullableString(lblincRank.Text)
                             , General.GetNullableDateTime(lblincSignDate.Text)
                             , txtCode.Text
                             , txtItemNo.Text
                             , 1
                             , lblRecord.Text
                             , lblRecord.Text
                             , lblrecord1.Text
                             , lblrecord2.Text
                             , lblrecord3.Text
                             , lblrecord4.Text
                             , ""
                             , ""
                             , ""
                       );

        }
    }

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // store transaction 
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

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord.Text,
            1
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord1.Text,
            2
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord2.Text,
            3
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord3.Text,
            4
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord4.Text,
            5
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            6
        );

        // history update
        PhoenixMarpolLogCRB.LogCRBBookEntryStatusUpdate(usercode
                        , vesselId
                        , logId
                        , txid
                        , General.GetNullableInteger(lblinchId.Text)
                        , General.GetNullableString(lblincRank.Text)
                        , General.GetNullableString(lblincsign.Text)
                        , General.GetNullableDateTime(lblincSignDate.Text)
                    );

        PhoenixMarpolLogCRB.LogCRBHistoryUpdate(usercode
                             , vesselId
                             , logId
                             , txid
                             , General.GetNullableString(lblincRank.Text)
                             , General.GetNullableDateTime(lblincSignDate.Text)
                             , txtCode.Text
                             , txtItemNo.Text
                             , 1
                             , lblRecord.Text
                             , lblRecord.Text
                             , lblrecord1.Text
                             , lblrecord2.Text
                             , lblrecord3.Text
                             , lblrecord4.Text
                             , ""
                             , ""
                             , ""
                       );
    }

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        TimeSpan disstartTime = txtDisStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDisStartTime.SelectedTime.Value;
        TimeSpan disstopTime = txtDisStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDisStopTime.SelectedTime.Value;
        TimeSpan utcstartTime = txtUTCStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStartTime.SelectedTime.Value;
        TimeSpan utcstopTime = txtUTCStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStopTime.SelectedTime.Value;

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                50,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                51,
                txid,
                entrydate,
                ddlTransferFrom.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                52,
                txid,
                entrydate,
                ddldischargeballast.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                53,
                txid,
                entrydate,
                ddldischargeport.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                54,
                txid,
                entrydate,
                txtReceptionName.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                55,
                txid,
                entrydate,
                disstartTime.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                56,
                txid,
                entrydate,
                disstopTime.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                57,
                txid,
                entrydate,
                utcstartTime.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                58,
                txid,
                entrydate,
                utcstopTime.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                59,
                txid,
                entrydate,
                txtspeed.Text);
    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        TimeSpan disstartTime = txtDisStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDisStartTime.SelectedTime.Value;
        TimeSpan disstopTime = txtDisStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtDisStopTime.SelectedTime.Value;
        TimeSpan utcstartTime = txtUTCStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStartTime.SelectedTime.Value;
        TimeSpan utcstopTime = txtUTCStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtUTCStopTime.SelectedTime.Value;

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            50,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            51,
                            logTxId,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            52,
                            logTxId,
                            entrydate,
                            ddldischargeballast.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            53,
                            logTxId,
                            entrydate,
                            ddldischargeport.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            54,
                            logTxId,
                            entrydate,
                            txtReceptionName.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            55,
                            logTxId,
                            entrydate,
                            disstartTime.ToString()
            );


        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            56,
                            logTxId,
                            entrydate,
                            disstopTime.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            57,
                            logTxId,
                            entrydate,
                            utcstartTime.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            58,
                            logTxId,
                            entrydate,
                            utcstopTime.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            59,
                            logTxId,
                            entrydate,
                            txtspeed.Text
            );
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (txtDisStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Discharge Start Time is required";
        }

        if (txtDisStopTime.SelectedDate.HasValue == false)
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

        if (string.IsNullOrWhiteSpace(txtspeed.Text))
        {
            ucError.ErrorMessage = "Ships Speed is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }


        return (!ucError.IsError);
    }

    private bool IsValidSignature()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
        SetDefaultData();
    }
    protected void Refresh_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlTransferFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void txtStartTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
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

        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string rank = "co";
        string popupTitle = "Chief Officer Sign";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }

    protected void ddldischargeballast_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
}
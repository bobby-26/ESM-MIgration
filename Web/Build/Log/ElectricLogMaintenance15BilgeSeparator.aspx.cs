using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Log_ElectricLogMaintenance15BilgeSeparator : PhoenixBasePage
{
    string ReportCode = "I";
    string ItemNo = null;
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    DateTime? missedOperationDate = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

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
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
        }
    }

    private void OnDutyEngineerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;
            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincRank.Text = nvc.Get("rank");
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

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }

    
    private void SetDefaultData()
    {
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;
        lblRecord.Text = string.Format("15ppm Bilge separator opened. Inspected and cleaned as per manufacturer’s instruction manual.");
        lblRecord1.Text = string.Format("{0}", txtMaintenance.Text);
        lbltorecord.Text = string.Format("Start Time <b>{0}</b> Finished  <b> {1}</b> ", startTime, stopTime);

    }
    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.BilgeSeparatorMaintenainceSearch(usercode, vesselId, txid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());
                txtMaintenance.Text = dr["FLDMAINTENANCE"].ToString();
                txtStartTime.SelectedDate = Convert.ToDateTime(dr["FLDSTARTTIME"].ToString());
                txtStopTime.SelectedDate = Convert.ToDateTime(dr["FLDSTOPTIME"].ToString());
                SetDefaultData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void MissedOperationInsert(Guid logId, string logBookName)
    {
        Guid TranscationId = Guid.Empty;
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixElog.MissedOperationalEntryInsert(usercode
                                            , vesselId
                                            , logId
                                            , logBookName
                                            , ref TranscationId
                                            , entrydate
                                            , Convert.ToDateTime(missedOperationDate)
                                            , false
                        );
    }

    private void SaveTxin()
    {
        string logName = "Maintenance of 15ppm bilge separators";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId, logId;
            TransactionInsert(out TranscationId, out logId);
            MissedOperationTemplateUpdate(logName, TranscationId, logId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate();
            MissedOperationTemplateUpdate(logName, null, txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate();
            LogBookUpdate();
        }
        else
        {


            DateTime logBookEntryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid TranscationId, logId;
            TransactionInsert(out TranscationId, out logId);

            PhoenixElog.LogBookEntryInsert(usercode
            , logBookEntryDate
            , ItemNo
            , lblRecord.Text
            , txtCode.Text
            , TranscationId
            , 1
            , null
            , null
            , General.GetNullableString(lblincRank.Text)
            , vesselId
            , false
            , null
            , logName
            , false
            , entryNo
            , logId
            );

            PhoenixElog.LogBookEntryInsert(usercode
            , logBookEntryDate
            , null
            , lblRecord1.Text
            , null
            , TranscationId
            , 2
            , null
            , null
            , General.GetNullableString(lblincRank.Text)
            , vesselId
            , false
            , null
            , logName
            , false
            , entryNo
            , logId
            );

            PhoenixElog.LogBookEntryInsert(usercode
            , logBookEntryDate
            , null
            , lbltorecord.Text
            , null
            , TranscationId
            , 3
            , null
            , null
            , General.GetNullableString(lblincRank.Text)
            , vesselId
            , false
            , null
            , logName
            , false
            , entryNo
            , logId
            );


            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , null
                                      , TranscationId
                                      , 4
                                      , false
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , false
                                      , null
                                      , logName
                                      , false
                                      , entryNo
                                      , logId
                                );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
                                      , PhoenixElog.GetSignatureName("", "", null, true)
                                      , null
                                      , TranscationId
                                      , 5
                                      , true
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , false
                                      , null
                                      , logName
                                      , false
                                      , entryNo
                                      , logId
                                );
        }
    }

    private void MissedOperationTemplateUpdate(string logName, Guid? TranscationId, Guid logId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lbltorecord.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        // add meta data for the log
        nvc.Add("logBookEntry", "3");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "false");
        nvc.Add("txId", txid == null ? "" : TranscationId.ToString());

        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TransactionInsert(out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        Guid LogBookDetailId = Guid.Empty;
        logId = Guid.NewGuid();
        PhoenixElog.BilgeSeparatorMaintenainceInsert(usercode
        , vesselId
        , txtOperationDate.SelectedDate.Value
        , txtStartTime.SelectedDate.Value
        , txtStopTime.SelectedDate.Value
        , txtMaintenance.Text
        , General.GetNullableInteger(lblinchId.Text)
        , General.GetNullableString(lblincRank.Text)
        , General.GetNullableString(lblincsign.Text)
        , General.GetNullableDateTime(lblincSignDate.Text)
        , General.GetNullableInteger(null)
        , General.GetNullableString(null)
        , General.GetNullableString(null)
        , General.GetNullableDateTime(null)
        , ref TranscationId
        , logId
        );

        PhoenixElog.LogBookEntryStatusInsert(usercode
                                      , vesselId
                                      , logId
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                      , General.GetNullableInteger(null)
                                      , General.GetNullableString(null)
                                      , General.GetNullableString(null)
                                      , General.GetNullableDateTime(null)
                                        );
    }

    private void LogBookUpdate()
    {
        

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord.Text
                        , txid
                        , 1
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord1.Text
                        , txid
                        , 2
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lbltorecord.Text
                        , txid
                        , 3
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                        , txid
                        , 4
                        , false);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName("", "", null, true)
                        , txid
                        , 5
                        , true);

    }

    private void TransactionUpdate()
    {
        PhoenixElog.BilgeSeparatorMaintenainceUpdate(usercode
                        , txid
                        , vesselId
                        , txtOperationDate.SelectedDate.Value
                        , txtStartTime.SelectedDate.Value
                        , txtStopTime.SelectedDate.Value
                        , txtMaintenance.Text
                        , General.GetNullableInteger(lblinchId.Text)
                        , General.GetNullableString(lblincRank.Text)
                        , General.GetNullableString(lblincsign.Text)
                        , General.GetNullableDateTime(lblincSignDate.Text)
                        , General.GetNullableInteger(null)
                        , General.GetNullableString(null)
                        , General.GetNullableString(null)
                        , General.GetNullableDateTime(null)
                        );
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(txtMaintenance.Text))
        {
            ucError.ErrorMessage = "Maintenance is required";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }

        if (txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Time is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false && isMissedOperation == false)
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
            ucError.ErrorMessage = "Duty Engineer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixElog.GetLogLastTranscationDate(vesselId, usercode);
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
   
    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
    }
    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txt_selectedchange(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
    }

    protected void txtStopTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
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
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }
}
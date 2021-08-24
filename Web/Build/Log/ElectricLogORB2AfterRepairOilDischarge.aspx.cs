using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2AfterRepairOilDischarge : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "70";
    // string ItemName = "Sludge";
    int usercode = 0;
    int vesselId = 0;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "AfterOilRepair");

        ViewState["TXID"] = "";

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            ViewState["TXID"] = Guid.Parse(Request.QueryString["TxnId"]);
            //ddlTransferFrom.Enabled = false;
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

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }
    private void SetDefaultData()
    {
        ReportCode = "M";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "71";

        TimeSpan failuretime = txtFailureTime.SelectedTime.HasValue == false ? new TimeSpan() : txtFailureTime.SelectedTime.Value;
        TimeSpan Repairtime = txtrepairtime.SelectedTime.HasValue == false ? new TimeSpan() : txtrepairtime.SelectedTime.Value;
        TimeSpan Utc1 = txtUTC.SelectedTime.HasValue == false ? new TimeSpan() : txtUTC.SelectedTime.Value;
        TimeSpan Utc2 = txtutc2.SelectedTime.HasValue == false ? new TimeSpan() : txtutc2.SelectedTime.Value;


        lblRecord.Text = string.Format(" <b> {0}</b> LT; <b> {1}</b> UTC", failuretime.ToString(), Utc1.ToString());
        lblrecord1.Text = string.Format(" <b> {0}</b> LT; <b> {1}</b> UTC", Repairtime.ToString(), Utc2.ToString());

    }
    private void BindData()
    {
        if (ViewState["TXID"].ToString() != "")
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, new Guid(ViewState["TXID"].ToString()));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 105)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 106)
                    {
                        txtFailureTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 107)
                    {
                        txtrepairtime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 108)
                    {
                        txtFailureReason.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 109)
                    {
                        txtUTC.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 110)
                    {
                        txtutc2.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                }
                SetDefaultData();
            }
        }

        DataTable dtODME = PhoenixMarbolLogORB2.ORB2LogIsFaliureODME(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (dtODME.Rows.Count > 0)
        {
            if (dtODME.Rows[0]["FLDFAILURE"].ToString() == "1")
            {
                if (dtODME.Rows[0]["FLDFAILUREDATE"].ToString() != "")
                {
                    txtFailureTime.SelectedDate = Convert.ToDateTime(dtODME.Rows[0]["FLDFAILUREDATE"].ToString());
                    txtUTC.SelectedDate = Convert.ToDateTime(dtODME.Rows[0]["FLDUTCDATE"].ToString());
                    txtFailureTime.Enabled = false;
                    txtUTC.Enabled = false;
                }
                else
                {
                    txtFailureTime.Enabled = true;
                    txtUTC.Enabled = true;
                }

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
        if (txtFailureTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Failure Time is required";
        }
        if (txtrepairtime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Repair Time is required";
        }
        if (txtUTC.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Time is required";
        }
        if (txtutc2.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Time is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && string.IsNullOrWhiteSpace(ViewState["TXID"].ToString()))
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }
    private void TranscationUpdate(int logId)
    {

        TimeSpan failuretime = txtFailureTime.SelectedTime.HasValue == false ? new TimeSpan() : txtFailureTime.SelectedTime.Value;
        TimeSpan Repairtime = txtrepairtime.SelectedTime.HasValue == false ? new TimeSpan() : txtrepairtime.SelectedTime.Value;
        TimeSpan Utc1 = txtUTC.SelectedTime.HasValue == false ? new TimeSpan() : txtUTC.SelectedTime.Value;
        TimeSpan Utc2 = txtutc2.SelectedTime.HasValue == false ? new TimeSpan() : txtutc2.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 105
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 106
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(failuretime.ToString())

                                );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                   , vesselId
                                   , logId
                                   , 107
                                   , General.GetNullableGuid(ViewState["TXID"].ToString())
                                   , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                   , General.GetNullableString(Repairtime.ToString())

                               );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                          , vesselId
                          , logId
                          , 108
                          , General.GetNullableGuid(ViewState["TXID"].ToString())
                          , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                          , General.GetNullableString(txtFailureReason.Text)
                      );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                   , vesselId
                                   , logId
                                   , 109
                                   , General.GetNullableGuid(ViewState["TXID"].ToString())
                                   , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                   , General.GetNullableString(Utc1.ToString())

                               );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                  , vesselId
                                  , logId
                                  , 110
                                  , General.GetNullableGuid(ViewState["TXID"].ToString())
                                  , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                  , General.GetNullableString(Utc2.ToString())

                              );


    }

    private void LogBookUpdate()
    {


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                  , vesselId
                                  , General.GetNullableGuid(ViewState["TXID"].ToString())
                                  , logId
                                  , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                  , lblRecord.Text
                                  , 1
                              );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord1.Text
                                 , 2
                             );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 3
                             );

        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                                      , vesselId
                                      , logId
                                      , General.GetNullableGuid(ViewState["TXID"].ToString())
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                        );

    }
    private void TranscationInsert(DateTime entrydate, Guid TranscationId, int logId)
    {

        TimeSpan failuretime = txtFailureTime.SelectedTime.HasValue == false ? new TimeSpan() : txtFailureTime.SelectedTime.Value;
        TimeSpan Repairtime = txtrepairtime.SelectedTime.HasValue == false ? new TimeSpan() : txtrepairtime.SelectedTime.Value;
        TimeSpan Utc1 = txtUTC.SelectedTime.HasValue == false ? new TimeSpan() : txtUTC.SelectedTime.Value;
        TimeSpan Utc2 = txtutc2.SelectedTime.HasValue == false ? new TimeSpan() : txtutc2.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 105
                                    , TranscationId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 106
                                    , TranscationId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(failuretime.ToString())

                                );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 107
                                    , TranscationId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(Repairtime.ToString())

                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 108
                            , TranscationId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtFailureReason.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 109
                            , TranscationId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(Utc1.ToString())

                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                           , vesselId
                           , logId
                           , 110
                           , TranscationId
                           , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                           , General.GetNullableString(Utc2.ToString())

                       );


    }

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);


        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);


        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "2");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }
    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string rank = PhoenixElog.GetRankName(usercode);

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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SaveTransaction()
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
            TranscationUpdate(logId);
            MissedOperationalEntryTemplateUpdate(new Guid(ViewState["TXID"].ToString()));
        }

        else if (ViewState["TXID"].ToString() != "" && isMissedOperationEdit == false)
        {
            TranscationUpdate(logId);
            LogBookUpdate();
        }
        else
        {

            Guid TranscationId = Guid.NewGuid();
            TranscationInsert(entrydate, TranscationId, logId);

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , lblRecord.Text
                                     , 1
                                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , null
                                     , txtItemNo1.Text
                                     , lblrecord1.Text
                                     , 2
                                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                      , vesselId
                                      , TranscationId
                                      , logId
                                      , entrydate
                                      , null
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 3
                                      , true
                                );

            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                                          , vesselId
                                          , logId
                                          , TranscationId
                                          , General.GetNullableInteger(lblinchId.Text)
                                          , General.GetNullableString(lblincRank.Text)
                                          , General.GetNullableString(lblincsign.Text)
                                          , General.GetNullableDateTime(lblincSignDate.Text)
                                            );

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


    protected void txt_selectedindexchaged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtFailureTime_SelectedDateChanged(object sender, EventArgs e)
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

    protected void txtFailureReason_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
}
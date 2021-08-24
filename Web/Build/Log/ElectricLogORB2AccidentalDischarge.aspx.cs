using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2AccidentalDischarge : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "73";
    //string ItemName = "Sludge";
    int usercode = 0;
    int vesselId = 0;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    Guid txid = Guid.Empty;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "AccidentalExceptional");

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
            LoadNotes();
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
        ReportCode = "N";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;

        txtItemNo1.Text = "74";
        txtItemNo2.Text = "75";
        txtItemNo3.Text = "76";

        lblRecord.Text = string.Format(" Start at <b> {0},{1}</b>", txtStartPosistionLat.Text, txtStartPosistionLog.Text);
        lblrecord1.Text = string.Format(" <b> {0}</b>  <b> {1},{2}</b> ", txtnameofport.Text, txtStartPosistionLat.Text, txtStartPosistionLog.Text);
        lblrecord2.Text = string.Format("Approximately <b> {0}</b> m3 of  <b> {1}</b>", txtquantity.Text, txttype.Text);
        lblrecord3.Text = string.Format(" <b> {0}</b>", txtremarks.Text);

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
        if (txid.ToString() != "")
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, new Guid(txid.ToString()));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 111)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 112)
                    {
                        txtaccidentTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 113)
                    {
                        txtquantity.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 114)
                    {
                        txttype.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 115)
                    {
                        txtUTC.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 116)
                    {
                        txtStartPosistionLat.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 117)
                    {
                        txtStartPosistionLog.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 118)
                    {
                        txtnameofport.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 132)
                    {
                        txtremarks.Text = row["FLDVALUE"].ToString();
                    }

                }
                SetDefaultData();
            }
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
            TranscationInsert(logTxId);
            MissedOperationalEntryTemplateUpdate(logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate();
            MissedOperationalEntryTemplateUpdate(txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate();
            LogBookUpdate();
        }
        else
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(logTxId);

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , logTxId
                                     , logId
                                     , entrydate
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , lblRecord.Text
                                     , 1
                                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , logTxId
                                     , logId
                                     , entrydate
                                     , null
                                     , txtItemNo1.Text
                                     , lblrecord1.Text
                                     , 2
                                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                    , vesselId
                                    , logTxId
                                    , logId
                                    , entrydate
                                    , null
                                    , txtItemNo2.Text
                                    , lblrecord2.Text
                                    , 3
                                );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , logTxId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo3.Text
                     , lblrecord3.Text
                     , 4
                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                      , vesselId
                                      , logTxId
                                      , logId
                                      , entrydate
                                      , null
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 5
                                      , true
                                );

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

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);
        nvc.Add("Record4", lblrecord3.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "4");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void LogBookUpdate()
    {

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                  , vesselId
                                  , General.GetNullableGuid(txid.ToString())
                                  , logId
                                  , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                  , lblRecord.Text
                                  , 1
                              );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(txid.ToString())
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord1.Text
                                 , 2
                             );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(txid.ToString())
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord2.Text
                                 , 3
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(txid.ToString())
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord3.Text
                                 , 4
                             );



        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(txid.ToString())
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 5
                             );

        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                                      , vesselId
                                      , logId
                                      , General.GetNullableGuid(txid.ToString())
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                        );

    }

    private void TransactionUpdate()
    {

        TimeSpan Accidenttime = txtaccidentTime.SelectedTime.HasValue == false ? new TimeSpan() : txtaccidentTime.SelectedTime.Value;
        TimeSpan utctime = txtUTC.SelectedTime.HasValue == false ? new TimeSpan() : txtUTC.SelectedTime.Value;
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 111
                                    , General.GetNullableGuid(txid.ToString())
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 112
                                    , General.GetNullableGuid(txid.ToString())
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(Accidenttime.ToString())
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 113
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtquantity.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 114
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txttype.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 115
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(utctime.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 116
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLat.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 117
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLog.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 118
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtnameofport.Text)

                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 132
                            , General.GetNullableGuid(txid.ToString())
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtremarks.Text)

                        );


    }

    private void TranscationInsert(Guid logTxId)
    {
        TimeSpan Accidenttime = txtaccidentTime.SelectedTime.HasValue == false ? new TimeSpan() : txtaccidentTime.SelectedTime.Value;
        TimeSpan utctime = txtUTC.SelectedTime.HasValue == false ? new TimeSpan() : txtUTC.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 111
                                    , logTxId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 112
                                    , logTxId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(Accidenttime.ToString())

                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 113
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtquantity.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 114
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txttype.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 115
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(utctime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 116
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLat.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 117
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 118
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtnameofport.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 132
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtremarks.Text)

                        );

    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation    is required";
        }
        if (string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position Longitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text))
        {
            ucError.ErrorMessage = "Start Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtquantity.Text))
        {
            ucError.ErrorMessage = "Approx Quantity is required";
        }

        if (string.IsNullOrWhiteSpace(txttype.Text))
        {
            ucError.ErrorMessage = "Type of Oil/ Cargo Discharged is required";
        }
        if (string.IsNullOrWhiteSpace(txtnameofport.Text))
        {
            ucError.ErrorMessage = "Name of the Port/Dock/Anchorage if applicable is required";
        }
        if (string.IsNullOrWhiteSpace(txtremarks.Text))
        {
            ucError.ErrorMessage = "Reason and General Remarks  is required";
        }

        if (txtaccidentTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = " Time of Accident Discharge is required";
        }
        if (txtUTC.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "UTC Time is required";
        }

        if (PhoenixMarbolLogORB2.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
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
    protected void txtQuantity_TextChangedEvent(object sender, EventArgs e)
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
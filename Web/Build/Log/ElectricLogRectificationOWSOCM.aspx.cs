using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogRectificationOWSOCM : PhoenixBasePage
{
    string ReportCode = "F";
    string ItemNo = "19";
    string ItemName = string.Empty;
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
            ViewState["sealinchargeSign"] = false;
            ViewState["inchargeSign"] = false;
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            ViewState["lastTranscationDate"] = null;
            GetLastTranscationDate();
            SetFailureTime();
            SetDefaultData();
            BindData();
        }
    }

    private void SetFailureTime()
    {
        DataTable dt = PhoenixElog.owsFaliureTime(usercode, vesselId);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            txtFailureTime.SelectedDate = Convert.ToDateTime(row["FLDFAILURETIME"]);
            txtUTCFailureTime.SelectedDate = Convert.ToDateTime(row["FLDUTCFAILURETIME"]);
            txtFailureTime.Enabled = false;
            txtUTCFailureTime.Enabled = false;
        }
    }

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.RectificationOWSOperationTransferSearch(usercode ,vesselId, txid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtDate.Text = Convert.ToDateTime(dr["FLDFAILURETIME"].ToString()).ToShortDateString();
                txtFailureTime.SelectedDate = Convert.ToDateTime(dr["FLDFAILURETIME"]);
                txtEstimatedTime.SelectedDate = Convert.ToDateTime(dr["FLDSYSTEMFUNCTIONALTIME"]);

                if (dr["FLDUTCFAILURETIME"] != DBNull.Value)
                {
                    txtUTCFailureTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFAILURETIME"]);
                }

                if (dr["FLDUTCSYSTEMFUNCTIONALTIME"] != DBNull.Value)
                {
                    txtUTCEstimatedTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCSYSTEMFUNCTIONALTIME"]);
                }

                txtFailureReason.Text = dr["FLDREASON"].ToString();
                txtOverboardValue.Text = dr["FLDOVERBOARDVALVENUMBER"].ToString();
                txtSealNo.Text = dr["FLDSEALNO"].ToString();

                txtFailureTime.Enabled = false;
                txtUTCFailureTime.Enabled = false;
                txtEstimatedTime.Enabled = false;
                txtUTCEstimatedTime.Enabled = false;

                SetDefaultData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            ViewState["inchargeSign"] = true;
        }

        if (Filter.SealEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.SealEngineerSignatureFilterCriteria;
            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblSealincRank.Text = nvc.Get("rank");
            lblSealinchName.Text = string.Format("{0}-{1} {2}", nvc.Get("rank"), nvc.Get("name"), signedDate.ToString("dd-MM-yyyy"));
            lblSealincRank.Text = nvc.Get("rank");
            lblSealinchId.Text = nvc.Get("id");
            lblSealincSignDate.Text = signedDate.ToString();
            Filter.SealEngineerSignatureFilterCriteria = null;

            btnSealInchargeSign.Visible = false;
            lblSealinchName.Visible = true;
            ViewState["sealinchargeSign"] = true;
        }

    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }
    private void SetDefaultData()
    {
        //txtDate.Text = txtOperationDate.SelectedDate.HasValue ? txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy");
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtDate1.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);

        txtCode.Text = ReportCode;
        txtCode1.Text = "I";
        lblItemNo1.Text = ItemNo;
        lblItemNo2.Text = "20";
        lblItemNo3.Text = "21";
        TimeSpan failureTimeSpan = txtFailureTime.SelectedTime.HasValue ? txtFailureTime.SelectedTime.Value : new TimeSpan();
        TimeSpan estimatedTime = txtEstimatedTime.SelectedTime.HasValue ? txtEstimatedTime.SelectedTime.Value : new TimeSpan();
        TimeSpan utcFailureTimeSpan = txtUTCFailureTime.SelectedTime.HasValue ? txtUTCFailureTime.SelectedTime.Value : new TimeSpan();
        TimeSpan utcEstimatedTime = txtUTCEstimatedTime.SelectedTime.HasValue ? txtUTCEstimatedTime.SelectedTime.Value : new TimeSpan();
        lblRecord1.Text = string.Format("Stopped due to failure  <b>{0}</b> LTC, <b>{1}</b> UTC", failureTimeSpan.ToString(), utcFailureTimeSpan.ToString());
        lblRecord2.Text = string.Format("Item repaired,Started <b>{0}</b>, <b>{1}</b> UTC", estimatedTime.ToString(), utcEstimatedTime.ToString());
        lblRecord3.Text = string.Format("Reason:<b>{0}</b>", txtFailureReason.Text);
        lblRecord4.Text = string.Format("Overboard Value no <b>{0}</b>  from 15 ppm bilge water separator unit unsealed from normal operation", txtOverboardValue.Text);
        lblRecord5.Text = string.Format("Seal No: <b>{0}</b>", txtSealNo.Text);
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
        string logName = "Rectification of OWS, OCM or Stopping Device";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId = Guid.Empty;
            Guid logId = TranscationInsert(ref TranscationId);
            MissedOperationtemplateUpdate(logName, TranscationId, logId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationtemplateUpdate(logName, null, txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {

            Guid TranscationId = Guid.Empty;
            DateTime logBookEntryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid logId = TranscationInsert(ref TranscationId);

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , ItemNo
                                      , lblRecord1.Text
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
                                      , lblItemNo2.Text
                                      , lblRecord2.Text
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
                                      , lblItemNo3.Text
                                      , lblRecord3.Text
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
                                      , lblRecord4.Text
                                      , txtCode1.Text
                                      , TranscationId
                                      , 6
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
                                      , lblRecord5.Text
                                      , null
                                      , TranscationId
                                      , 7
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
                                     , PhoenixElog.GetSignatureName(General.GetNullableString(lblSealinchName.Text), General.GetNullableString(lblSealincRank.Text), General.GetNullableDateTime(lblSealincSignDate.Text))
                                     , null
                                     , TranscationId
                                     , 8
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
                                      , 9
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

    private void MissedOperationtemplateUpdate(string logName, Guid? TranscationId, Guid logId)
    {
        NameValueCollection nvc = new NameValueCollection();
        //nvc["date"] = txtOperationDate.SelectedDate.Value.ToString();
        //nvc["failureTime"] = txtFailureTime.SelectedDate.Value.ToString();
        //nvc["failureReason"] = txtFailureReason.Text;
        //nvc["estimatedRescueTime"] = txtEstimatedTime.SelectedDate.Value.ToString();
        //nvc["overBoardValveNo"] = txtOverboardValue.Text;
        //nvc["sealNo"] = txtSealNo.Text;
        // add for logbook
        nvc.Add("Record1", lblRecord1.Text);
        nvc.Add("Record2", lblRecord2.Text);
        nvc.Add("Record3", lblRecord3.Text);
        nvc.Add("Record4", lblRecord4.Text);
        nvc.Add("Record5", lblRecord5.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ReportCode2", "");
        nvc.Add("ReportCode3", "");
        nvc.Add("ReportCode4", "");
        nvc.Add("ReportCode5", txtCode1.Text);
        nvc.Add("ItemNo1", lblItemNo1.Text);
        nvc.Add("ItemNo2", lblItemNo2.Text);
        nvc.Add("ItemNo3", lblItemNo3.Text);
        nvc.Add("ItemNo4", "");
        nvc.Add("ItemNo5", lblItemNo5.Text);
        nvc.Add("ItemName1", ItemName);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));


        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        nvc.Add("sealInchId", lblSealinchId.Text);
        nvc.Add("sealInchRank", lblSealincRank.Text);
        nvc.Add("sealInchName", lblSealinchName.Text);
        nvc.Add("sealInchSignDate", lblSealincSignDate.Text);
        nvc.Add("sealInchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblSealinchName.Text), General.GetNullableString(lblSealincRank.Text), General.GetNullableDateTime(lblSealincSignDate.Text)));

        // add meta data for the log
        nvc.Add("logBookEntry", "5");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "false");
        nvc.Add("txId", txid == null ? "" : TranscationId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private Guid TranscationInsert(ref Guid TranscationId)
    {
        Guid logId = Guid.NewGuid();

        PhoenixElog.RectificationOWSOperationTransferInsert(usercode,
                  vesselId,
                  txtFailureTime.SelectedDate.Value,
                  txtEstimatedTime.SelectedDate.Value,
                  txtFailureReason.Text,
                  txtOverboardValue.Text,
                  txtSealNo.Text,
                  Convert.ToInt32(lblinchId.Text),
                  lblincRank.Text,
                  lblincsign.Text,
                  Convert.ToDateTime(lblincSignDate.Text),
                  General.GetNullableInteger(null),
                  General.GetNullableString(null),
                  General.GetNullableString(null),
                  General.GetNullableDateTime(null),
                  Convert.ToInt32(lblSealinchId.Text),
                  lblSealinchName.Text,
                  lblSealincRank.Text,
                  Convert.ToDateTime(lblSealincSignDate.Text),
                  General.GetNullableInteger(null),
                  General.GetNullableString(null),
                  General.GetNullableString(null),
                  General.GetNullableDateTime(null),
                  logId,
                  ref TranscationId,
                  txtUTCFailureTime.SelectedDate.Value,
                  txtUTCEstimatedTime.SelectedDate.Value
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
                                      , isMissedOperation
                                        );

        PhoenixElog.FailureRectified(usercode, vesselId);

        return logId;
    }

    private void LogBookUpdate()
    {
        

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord1.Text
                        , txid
                        , 1
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord2.Text
                        , txid
                        , 2
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord3.Text
                        , txid
                        , 3
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                        , txid
                        , 4
                        , false);

        //PhoenixElog.LogBookEntryUpdate(usercode
        //                , PhoenixElog.GetSignatureName("", "", null, true)
        //                , txid
        //                , 5
        //                , true);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord4.Text
                        , txid
                        , 6
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord5.Text
                        , txid
                        , 7
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                       , PhoenixElog.GetSignatureName(General.GetNullableString(lblSealinchName.Text), General.GetNullableString(lblSealincRank.Text), General.GetNullableDateTime(lblSealincSignDate.Text))
                       , txid
                       , 8
                       , false);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName("", "", null, true)
                        , txid
                        , 9
                        , true);
    }

    private void TranscationUpdate()
    {
        PhoenixElog.RectificationOWSOperationTransferUpdate(usercode, txid, vesselId,
                                      txtFailureTime.SelectedDate.Value,
                                      txtEstimatedTime.SelectedDate.Value,
                                      txtFailureReason.Text,
                                      txtOverboardValue.Text,
                                      txtSealNo.Text,
                                      Convert.ToInt32(lblinchId.Text),
                                      lblincRank.Text,
                                      lblincsign.Text,
                                      Convert.ToDateTime(lblincSignDate.Text),
                                      Convert.ToInt32(lblSealinchId.Text),
                                      lblSealinchName.Text,
                                      lblSealincRank.Text,
                                      Convert.ToDateTime(lblSealincSignDate.Text),
                                      txtUTCFailureTime.SelectedDate.Value,
                                      txtUTCEstimatedTime.SelectedDate.Value
                    );
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixElog.GetLogLastTranscationDate(vesselId, usercode);
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

        if (txtEstimatedTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Estimated Time of rescue after repairs is required";
        }

        if (string.IsNullOrWhiteSpace(txtFailureReason.Text))
        {
            ucError.ErrorMessage = "Reason of Failure is required";
        }

        if (string.IsNullOrWhiteSpace(txtOverboardValue.Text))
        {
            ucError.ErrorMessage = "Overboard Value is required";
        }

        if (string.IsNullOrWhiteSpace(txtSealNo.Text))
        {
            ucError.ErrorMessage = "Seal No is required";
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

        if (string.IsNullOrWhiteSpace(lblSealinchName.Text))
        {
            ucError.ErrorMessage = "Seal Incharge sign is must";
        }

        return (!ucError.IsError);
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        if ((isMissedOperation || isMissedOperationEdit) && Convert.ToBoolean(ViewState["sealinchargeSign"]) && Convert.ToBoolean(ViewState["inchargeSign"]))
        {
            SaveTxin();
        }
    }

    protected void btnSealInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }
        string popupName = isMissedOperation ? "Log~iframe" : "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogSealEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        string popupName = isMissedOperation ? "Log~iframe" : "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }
}
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogSealingOverboardValve : PhoenixBasePage
{
    string ReportCode = "I";
    string ItemNo = null;
    string ItemName = null;
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
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            SetDefaultData();
            BindData();
        }
    }

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.SealingUnsealingSearch(usercode, vesselId, txid, false);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());
                txtSealNo.Text = dr["FLDSEALNO"].ToString();
                txtSealingOverBoard.Text = dr["FLDVALVENO"].ToString();

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
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
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
        String logName = "Sealing of Overboard Valve";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId, TranscationId;
            TransactionInsert(out logId, out TranscationId);
            MissedOperationTemplateUpdate(logName, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate();
            MissedOperationTemplateUpdate(logName, txid, null);
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
            Guid logId, TranscationId;
            TransactionInsert(out logId, out TranscationId);

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
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
                                      , lbltorecord.Text
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
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                        , null
                        , TranscationId
                        , 3
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
                                      , 4
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

    private void MissedOperationTemplateUpdate(string logName, Guid logId, Guid? TranscationId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("overBoardValveNo", txtSealingOverBoard.Text);
        nvc.Add("sealNo", txtSealNo.Text);
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lbltorecord.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        // add meta data for the log
        nvc.Add("logBookEntry", "2");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "false");
        nvc.Add("txId", txid == null ? "" : TranscationId.ToString());


        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TransactionInsert(out Guid logId, out Guid TranscationId)
    {
        logId = Guid.NewGuid();
        TranscationId = Guid.Empty;
        PhoenixElog.SealingUnsealingInsert(usercode
                                    , vesselId
                                    , General.GetNullableDecimal(ItemNo)
                                    , ItemName
                                    , ReportCode
                                    , txtSealingOverBoard.Text
                                    , txtSealNo.Text
                                    , false
                                    , General.GetNullableInteger(lblinchId.Text)
                                   , General.GetNullableString(lblincRank.Text)
                                   , General.GetNullableString(lblincsign.Text)
                                   , General.GetNullableDateTime(lblincSignDate.Text)
                                   , General.GetNullableInteger(null)
                                   , General.GetNullableString(null)
                                   , General.GetNullableString(null)
                                   , General.GetNullableDateTime(null)
                                   , ref TranscationId
                                   , txtOperationDate.SelectedDate.Value
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
        PhoenixElog.SealingUnsealingUpdate(usercode
                                                        , vesselId
                                                        , txid
                                                        , General.GetNullableDecimal(ItemNo)
                                                        , ItemName
                                                        , ReportCode
                                                        , txtSealingOverBoard.Text
                                                        , txtSealNo.Text
                                                        , General.GetNullableInteger(lblinchId.Text)
                                                       , General.GetNullableString(lblincRank.Text)
                                                       , General.GetNullableString(lblincsign.Text)
                                                       , General.GetNullableDateTime(lblincSignDate.Text)
                                                    );
    }

    private void SetDefaultData()
    {
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        lblRecord.Text = string.Format("Overboard value no <b>{0}</b> from 15 ppm bilge water separator unit sealed", txtSealingOverBoard.Text);
        lbltorecord.Text = string.Format("Seal No:  <b>{0}</b>", txtSealNo.Text);
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
        if (string.IsNullOrWhiteSpace(txtSealingOverBoard.Text))
        {
            ucError.ErrorMessage = "Overboard Value no is required";
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
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
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
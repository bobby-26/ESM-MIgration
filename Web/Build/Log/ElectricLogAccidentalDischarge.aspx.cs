using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogAccidentalDischarge : PhoenixBasePage
{
    string ReportCode = "G";
    string ItemNo = "22";
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

            ds = PhoenixElog.AccidentalDischargeSearch(usercode, vesselId, txid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDACCIDENTTIME"].ToString());
                txtExceptionTime.SelectedDate = Convert.ToDateTime(dr["FLDACCIDENTTIME"].ToString());
                txtStartPosistionLat.Text = dr["FLDSTARTLATITUDE"].ToString();
                txtStartPosistionLog.Text = dr["FLDSTARTLONGITUDE"].ToString();
                txtTypeQuantiy.Text = dr["FLDTYPE"].ToString();
                txtCircumstance.Text = dr["FLDCIRCUMSTANCE"].ToString();

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
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }

    private void SetDefaultData()
    {
        string date = DateTime.Now.ToString("dd-MM-yyyy");
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;
        lblItemNo1.Text = ItemNo;
        lblItemNo2.Text = "23";
        lblItemNo3.Text = "24";
        lblItemNo4.Text = "25";
        TimeSpan exceptionTime = txtExceptionTime.SelectedTime.HasValue ? txtExceptionTime.SelectedTime.Value : new TimeSpan();
        string StartPosistion = txtStartPosistionLat.Text + " ,  " + txtStartPosistionLog.Text;
        lblRecord1.Text = string.Format("<b>{0}</b>", exceptionTime.ToString());
        lblRecord2.Text = string.Format("Place or Position <b>{0}</b>", StartPosistion);
        lblRecord3.Text = string.Format("<b>{0}</b>", txtTypeQuantiy.Text);
        lblRecord4.Text = string.Format("<b>{0}</b>", txtCircumstance.Text);
    }
    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "SAVE")
            {
                if (isValidInput() == false || IsValidSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                SaveTxin();

            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
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
        string logName = "Accidental Discharge or other exceptional discharges of oil";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId = Guid.Empty;
            Guid logId = TranscationInsert(ref TranscationId);
            missedOperationTemplateUpdate(logName, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            missedOperationTemplateUpdate(logName, txid, null);
        }
        else if (txid != Guid.Empty)
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
                                      , lblItemNo4.Text
                                      , lblRecord4.Text
                                      , null
                                      , TranscationId
                                      , 4
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
                                      , 5
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
                                      , 6
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

    private Guid TranscationInsert(ref Guid TranscationId)
    {
        Guid logId = Guid.NewGuid();

        PhoenixElog.AccidentalDischargeInsert(usercode,
                          vesselId
                          , txtExceptionTime.SelectedDate.Value
                          , txtStartPosistionLat.Text
                          , txtStartPosistionLog.Text
                          , txtTypeQuantiy.Text
                          , txtCircumstance.Text
                          , General.GetNullableInteger(lblinchId.Text)
                          , General.GetNullableString(lblincRank.Text)
                          , General.GetNullableString(lblincsign.Text)
                          , General.GetNullableDateTime(lblincSignDate.Text)
                          , General.GetNullableInteger(null)
                          , General.GetNullableString(null)
                          , General.GetNullableString(null)
                          , General.GetNullableDateTime(null)
                          , logId
                          , ref TranscationId
                          , isMissedOperation
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
        return logId;
    }

    private void missedOperationTemplateUpdate(string logName, Guid logId, Guid? txId)
    {
        

        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("timeOfAccident", txtExceptionTime.SelectedDate.Value.ToString());
        nvc.Add("startPositionLat", txtStartPosistionLat.Text);
        nvc.Add("startPositionLong", txtStartPosistionLog.Text);
        nvc.Add("quantityOil", txtTypeQuantiy.Text);
        nvc.Add("circumstanceFailure", txtCircumstance.Text);
        // add for logbook
        nvc.Add("Record1", lblRecord1.Text);
        nvc.Add("Record2", lblRecord2.Text);
        nvc.Add("Record3", lblRecord3.Text);
        nvc.Add("Record4", lblRecord4.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", lblItemNo1.Text);
        nvc.Add("ItemNo2", lblItemNo2.Text);
        nvc.Add("ItemNo3", lblItemNo3.Text);
        nvc.Add("ItemNo4", lblItemNo4.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        // add meta data for the log
        nvc.Add("logBookEntry", "4");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "false");
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TranscationUpdate()
    {
        PhoenixElog.AccidentalDischargeUpdate(usercode
                                           , txid
                                           , vesselId
                                           , txtExceptionTime.SelectedDate.Value
                                           , txtStartPosistionLat.Text
                                           , txtStartPosistionLog.Text
                                           , txtTypeQuantiy.Text
                                           , txtCircumstance.Text
                                           , General.GetNullableInteger(lblinchId.Text)
                                         , General.GetNullableString(lblincRank.Text)
                                         , General.GetNullableString(lblincsign.Text)
                                         , General.GetNullableDateTime(lblincSignDate.Text)
                                         , isMissedOperation
                            );


        PhoenixElog.LogBookEntryStatusUpdate(usercode
                                                  , vesselId
                                                  , txid
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
        
        if (isMissedOperation  == false)
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
                            , lblRecord4.Text
                            , txid
                            , 4
                            , null);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                            , txid
                            , 5
                            , false);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName("", "", null, true)
                            , txid
                            , 6
                            , true);
        }
        
    }

    
    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (txtExceptionTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Time for Accident or Exceptional is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text) || string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position	is required";
        }

        if (string.IsNullOrWhiteSpace(txtTypeQuantiy.Text))
        {
            ucError.ErrorMessage = "Type and Quantity of Oil Residue is required";
        }

        if (string.IsNullOrWhiteSpace(txtCircumstance.Text))
        {
            ucError.ErrorMessage = "Circumstance of Failure	is required";
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

    protected void txt_SelectedDateChanged(object sender, EventArgs e)
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
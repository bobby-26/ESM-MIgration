using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;


public partial class Log_ElectricLogAutomaticTransferBilge : PhoenixBasePage
{
    string ReportCode = "E";
    string ItemNo = "17";
    int usercode = 0;
    int vesselId = 0;
    string ItemName = string.Empty;
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
            ddlTransferFrom.Enabled = false;
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
            ddlFromPopulate();
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
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }

    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        lblItemNo1.Text = "18";
        txtCode.Text = ReportCode;
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue ? txtStartTime.SelectedTime.Value : new TimeSpan();
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue ? txtStopTime.SelectedTime.Value : new TimeSpan();
        lblRecord.Text = string.Format("Transfer Start  <b>{0}</b> hrs LT <b>{1},{2}</b>, To", startTime, txtStartPosistionLat.Text, txtStartPosistionLog.Text);
        lblRecord1.Text = string.Format("{0}", ddlTransferFrom.SelectedItem.Text);
        lbltorecord.Text = string.Format("Transfer Stop <b>{0}</b> hrs LT, at <b>{1},{2}</b>", stopTime, txtStopPosistionLat.Text, txtStopPosistionLog.Text);
    }

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.AutomaticTransferBilgeSearch(usercode, vesselId, txid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDCREATEDDATE"].ToString());
                txtStartTime.SelectedDate = Convert.ToDateTime(dr["FLDSTARTTIME"].ToString());
                txtStopTime.SelectedDate = Convert.ToDateTime(dr["FLDSTOPTIME"].ToString());
                txtStartPosistionLat.Text = dr["FLDSTARTPOSITIONLAT"].ToString();
                txtStartPosistionLog.Text = dr["FLDSTARTPOSITIONLONG"].ToString();
                txtStopPosistionLog.Text = dr["FLDSTOPPOSITIONLAT"].ToString();
                txtStopPosistionLat.Text = dr["FLDSTOPPOSITIONLON"].ToString();

                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
                }
                SetDefaultData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void TranscationUpdate()
    {
        PhoenixElog.AutomaticTransferBilgeUpdate(usercode
                                                , txid
                                                , vesselId
                                                , txtStartTime.SelectedDate.Value
                                                , txtStopTime.SelectedDate.Value
                                                , txtStartPosistionLat.Text
                                                , txtStartPosistionLog.Text
                                                , txtStopPosistionLat.Text
                                                , txtStopPosistionLog.Text
                                                , General.GetNullableInteger(lblinchId.Text)
                                                , General.GetNullableString(lblincRank.Text)
                                                , General.GetNullableString(lblincsign.Text)
                                                , General.GetNullableDateTime(lblincSignDate.Text)
                                                , General.GetNullableInteger(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableDateTime(null)
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
                                                  , isMissedOperation
                                                    );
    }

    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
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
        Guid TranscationId = Guid.Empty;
        Guid LogBookDetailId = Guid.Empty;
        string logName = "Automatic Transfer of bilge water from engine-room bilge wells";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId = insertTranscation(ref TranscationId);
            MissedOperationTemplateUpdate(TranscationId, logName, logId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationTemplateUpdate(TranscationId, logName, txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {
            DateTime date = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);

            Guid logId = insertTranscation(ref TranscationId);

            PhoenixElog.LogBookEntryInsert(usercode
                                      , date
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
                                      , date
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
                                      , date
                                      , lblItemNo1.Text
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
                                      , date
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
                                      , date
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

    private void MissedOperationTemplateUpdate(Guid TranscationId, string logName, Guid logId)
    {
        NameValueCollection nvc = new NameValueCollection();
        //// store transaction 
        nvc.Add("FromTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("FromTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("pumpStart", txtStartTime.SelectedDate.Value.ToString());
        nvc.Add("pumpStop", txtStopTime.SelectedDate.Value.ToString());

        nvc.Add("startPositionLat", txtStartPosistionLat.Text);
        nvc.Add("startPositionLong", txtStartPosistionLog.Text);
        nvc.Add("stopPositionLat", txtStopPosistionLat.Text);
        nvc.Add("stopPositionLong", txtStopPosistionLog.Text);

        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lbltorecord.Text);
        nvc.Add("ItemName0", ItemName);


        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", "");
        nvc.Add("ItemNo3", lblItemNo1.Text);
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


    private Guid insertTranscation(ref Guid TranscationId)
    {
        Guid logId = Guid.NewGuid();

        PhoenixElog.AutomaticTransferBilgeInsert(usercode
                                                , ref TranscationId
                                                , vesselId
                                                , Guid.Parse(ddlTransferFrom.SelectedItem.Value)
                                                , ddlTransferFrom.SelectedItem.Text
                                                , txtStartTime.SelectedDate.Value
                                                , txtStopTime.SelectedDate.Value
                                                , txtStartPosistionLat.Text
                                                , txtStartPosistionLog.Text
                                                , txtStopPosistionLat.Text
                                                , txtStopPosistionLog.Text
                                                , General.GetNullableInteger(lblinchId.Text)
                                                , General.GetNullableString(lblincRank.Text)
                                                , General.GetNullableString(lblincsign.Text)
                                                , General.GetNullableDateTime(lblincSignDate.Text)
                                                , General.GetNullableInteger(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableDateTime(null)
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
                                      , isMissedOperation
                                        );
        return logId;
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position Longitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text))
        {
            ucError.ErrorMessage = "Start Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLat.Text))
        {
            ucError.ErrorMessage = "Stop Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLog.Text))
        {
            ucError.ErrorMessage = "Stop Position Longitude is required";
        }


        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false)
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

    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
    }

    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
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

    protected void txt_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
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

        string popupName = isMissedOperation ? "Log~iframe" : "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }
}
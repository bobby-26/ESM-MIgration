using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogOWSOperation : PhoenixBasePage
{
    string ReportCode = "D";
    string ItemNo = "13";
    string ItemName = "Bilge";
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
            DataSet owsDetail = new DataSet();
            DataSet bilgeDetail = new DataSet();

            ds = PhoenixElog.GetOperationRecord(txid
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , ReportCode
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());
                txtBfrTrnsROBFrom.Text = dr["FLDFROMROB"].ToString();
                txtAftrTrnsROBFrom.Text = dr["FLDAFTTXNFROM"].ToString();
                lblTranscationDetailID.Text = dr["FLDTRANSCATIONDETAILID"].ToString();
                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
                }

                owsDetail = PhoenixElog.OWSOperationTransferSearch(usercode, Guid.Parse(lblTranscationDetailID.Text));
                DataRow owsDetailRow = owsDetail.Tables[0].Rows[0];
                lblBilgeTranscationId.Text = owsDetailRow["FLDBILGEWATERTRANSCATIONID"].ToString();
                txtOperatingTime.Text = owsDetailRow["FLDOPERATINGTIME"].ToString();
                txtOWSCapacityRunTime.Text = owsDetailRow["FLDOWSCAPACITYRUNTIME"].ToString();
                txtStartPosistionLat.Text = owsDetailRow["FLDSTARTLATITUDE"].ToString();
                txtStartPosistionLog.Text = owsDetailRow["FLDSTARTLONGITUDE"].ToString();
                txtStopPosistionLat.Text = owsDetailRow["FLDSTOPLATITUDE"].ToString();
                txtStopPosistionLog.Text = owsDetailRow["FLDSTOPLONGITUDE"].ToString();
                txtOperationalTime.SelectedDate = Convert.ToDateTime(owsDetailRow["FLDOPERATIONALTIME"]);
                bilgeDetail = PhoenixElog.BilgeWellTransferSearch(usercode, Guid.Parse(lblBilgeTranscationId.Text), vesselId);
                DataRow bilgeDetailRow = bilgeDetail.Tables[0].Rows[0];
                txtStartTime.SelectedDate = Convert.ToDateTime(bilgeDetailRow["FLDSTARTTIME"]);
                txtStopTime.SelectedDate = Convert.ToDateTime(bilgeDetailRow["FLDENDTIME"]);

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
        string logName = "OWS Operations";
        Guid OwsProcessId = PhoenixElog.GetProcessId(usercode, "OWS");

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId, TranscationId;
            TranscationInsert(out logId, out TranscationId);
            MisedOperationTemplateUpdate(logName, OwsProcessId, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MisedOperationTemplateUpdate(logName, OwsProcessId, txid, null);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {

            DateTime logBookEntryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid logId, TranscationId;
            TranscationInsert(out logId, out TranscationId);

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
                                      , true
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
                                     , true
                                     , null
                                     , logName
                                     , false
                                     , entryNo
                                     , logId
                                 );

            PhoenixElog.LogBookEntryInsert(usercode
                                     , logBookEntryDate
                                     , null
                                     , lblRecord2.Text
                                     , null
                                     , TranscationId
                                     , 3
                                     , null
                                     , null
                                     , General.GetNullableString(lblincRank.Text)
                                     , vesselId
                                     , true
                                     , null
                                     , logName
                                     , false
                                     , entryNo
                                     , logId
                                 );

            PhoenixElog.LogBookEntryInsert(usercode
                                     , logBookEntryDate
                                     , txtItemNo3.Text
                                     , lblRecord3.Text
                                     , txtCode3.Text
                                     , TranscationId
                                     , 4
                                     , null
                                     , null
                                     , General.GetNullableString(lblincRank.Text)
                                     , vesselId
                                     , true
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
                                   , null
                                   , TranscationId
                                   , 5
                                   , null
                                   , null
                                   , General.GetNullableString(lblincRank.Text)
                                   , vesselId
                                   , true
                                   , null
                                   , logName
                                   , false
                                   , entryNo
                                   , logId
                               );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , txtItemNo5.Text
                                      , lblTimeRecord.Text
                                      , null
                                      , TranscationId
                                      , 6
                                      , null
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , false
                                      , entryNo
                                      , logId
                                );

            PhoenixElog.LogBookEntryInsert(usercode
                        , logBookEntryDate
                        , txtItemNo6.Text
                        , lblEquipment.Text
                        , null
                        , TranscationId
                        , 7
                        , null
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , false
                                      , entryNo
                                      , logId
                  );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
                                      , lblStartPos.Text
                                      , null
                                      , TranscationId
                                      , 8
                                      , null
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , false
                                      , entryNo
                                      , logId
               );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
                                      , lblStopPos.Text
                                      , null
                                      , TranscationId
                                      , 9
                                      , null
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
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
                                      , 10
                                      , false
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
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
                                      , 11
                                      , true
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , false
                                      , entryNo
                                      , logId
                                );


        }
    }

    private void MisedOperationTemplateUpdate(string logName, Guid OwsProcessId, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        string txQty = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAftrTrnsROBFrom.Text)).ToString();
        // store transaction 
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));
        nvc.Add("FromTankId", OwsProcessId.ToString());
        nvc.Add("FromTank", logName);
        nvc.Add("ToTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("ToTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("FromRob", txtBfrTrnsROBFrom.Text);
        nvc.Add("ToRob", txtAftrTrnsROBFrom.Text);
        nvc.Add("TransferQty", txQty);
        // add for logbook
        
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lblRecord2.Text);
        nvc.Add("Record4", lblRecord3.Text);
        nvc.Add("Record5", lblRecord4.Text);
        nvc.Add("Record6", lblTimeRecord.Text);
        nvc.Add("Record7", lblEquipment.Text);
        nvc.Add("Record8", lblStartPos.Text);
        nvc.Add("Record9", lblStopPos.Text);
        
        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ReportCode2", "");
        nvc.Add("ReportCode3", txtCode3.Text);

        nvc.Add("ItemName1", ItemName);

        nvc.Add("ItemNo1", "");
        nvc.Add("ItemNo2", "");
        nvc.Add("ItemNo3", txtItemNo3.Text);
        nvc.Add("ItemNo4", "");
        nvc.Add("ItemNo5", txtItemNo5.Text);
        nvc.Add("ItemNo6", txtItemNo6.Text);

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        nvc.Add("startPosistionLat", txtStartPosistionLat.Text);
        nvc.Add("startPosistionLong", txtStartPosistionLog.Text);
        nvc.Add("stopPosistionLat", txtStopPosistionLat.Text);
        nvc.Add("stopPosistionLong", txtStopPosistionLog.Text);

        nvc.Add("operatingTime", txtOperatingTime.Text);
        nvc.Add("operationalTime", txtOperationalTime.SelectedDate.Value.ToString());
        nvc.Add("owsCapacityRuntime", txtOWSCapacityRunTime.Text);

        // add meta data for the log
        nvc.Add("logBookEntry", "9");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : txId.ToString());

        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TranscationInsert(out Guid logId, out Guid TranscationId)
    {
        logId = Guid.NewGuid();
        TranscationId = Guid.Empty;
        Guid TranscationOtherId = Guid.Empty;
        Guid BilgeWaterTranscationId = Guid.Empty;

        PhoenixElog.BilgeWellTransferInsert(
                usercode,
                ref BilgeWaterTranscationId,
                txtStartTime.SelectedDate.Value,
                txtStopTime.SelectedDate.Value
            );


        PhoenixElog.OWSOperationTransferInsert(
                usercode,
                Convert.ToDecimal(txtOperatingTime.Text),
                Convert.ToDecimal(txtOWSCapacityRunTime.Text),
                txtStartPosistionLat.Text,
                txtStartPosistionLog.Text,
                txtStopPosistionLat.Text,
                txtStopPosistionLog.Text,
                BilgeWaterTranscationId,
                ref TranscationOtherId,
                txtOperationalTime.SelectedDate.Value
        );

        PhoenixElog.InsertTransactionNew(usercode
                                    , ReportCode
                                    , new Guid()
                                    , "Oil Water Separation"
                                    , new Guid(ddlTransferFrom.SelectedItem.Value)
                                    , ddlTransferFrom.SelectedItem.Text
                                    , txtCode.Text
                                    , ItemNo
                                    , ItemName
                                    , txtBfrTrnsROBFrom.Text
                                    , "0"
                                    , (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAftrTrnsROBFrom.Text)).ToString()
                                    , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                    , vesselId
                                    , General.GetNullableInteger(lblinchId.Text)
                                    , General.GetNullableString(lblincRank.Text)
                                    , General.GetNullableString(lblincsign.Text)
                                    , General.GetNullableDateTime(lblincSignDate.Text)
                                    , General.GetNullableInteger(null)
                                    , General.GetNullableString(null)
                                    , General.GetNullableString(null)
                                    , General.GetNullableDateTime(null)
                                    , null
                                    , ref TranscationId
                                    , TranscationOtherId
                                    , logId
                                    , txtAftrTrnsROBFrom.Text
                                    , "0"
                                    , "OWS Operations"
                                    , isMissedOperation
                                );
        ;
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
                        , lblRecord2.Text
                        , txid
                        , 3
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord3.Text
                        , txid
                        , 4
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord4.Text
                        , txid
                        , 5
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblTimeRecord.Text
                        , txid
                        , 6
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblEquipment.Text
                        , txid
                        , 7
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblStartPos.Text
                        , txid
                        , 8
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblStopPos.Text
                        , txid
                        , 9
                        , null);


        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                        , txid
                        , 10
                        , false);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName("", "", null, true)
                        , txid
                        , 11
                        , true);
    }

    private void TranscationUpdate()
    {
        PhoenixElog.BilgeWellTransferUpdate(
                                  usercode,
                                  Guid.Parse(lblBilgeTranscationId.Text),
                                  txtStartTime.SelectedDate.Value,
                                  txtStopTime.SelectedDate.Value
                              );


        PhoenixElog.OWSOperationTransferUpdate(
                usercode,
                Convert.ToDecimal(txtOperatingTime.Text),
                Convert.ToDecimal(txtOWSCapacityRunTime.Text),
                txtStartPosistionLat.Text,
                txtStartPosistionLog.Text,
                txtStopPosistionLat.Text,
                txtStopPosistionLog.Text,
                Guid.Parse(lblBilgeTranscationId.Text),
                Guid.Parse(lblTranscationDetailID.Text),
                txtOperationalTime.SelectedDate.Value
        );

        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , new Guid(ddlTransferFrom.SelectedItem.Value)
                                                , txtBfrTrnsROBFrom.Text
                                                , "0"
                                                , (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAftrTrnsROBFrom.Text)).ToString()
                                                , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                                , vesselId
                                                , General.GetNullableInteger(lblinchId.Text)
                                                , General.GetNullableString(lblincRank.Text)
                                                , General.GetNullableString(lblincsign.Text)
                                                , General.GetNullableDateTime(lblincSignDate.Text)
                                                , General.GetNullableInteger(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableDateTime(null)
                                                , null
                                                , txtAftrTrnsROBFrom.Text
                                                , "0"
                                            );
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        lblDate.Text = txtDate.Text;
        //txtDate.Text = txtOperationDate.SelectedDate.HasValue ? txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy");
        txtCode.Text = "I";
        txtCode3.Text = ReportCode;

        txtItemNo3.Text = ItemNo;
        txtItemNo5.Text = "14";
        txtItemNo6.Text = "15.1";
        string capacity = "50";
        string StartPos = txtStartPosistionLat.Text + " , " + txtStartPosistionLog.Text;
        string StopPos = txtStopPosistionLat.Text + " , " + txtStopPosistionLog.Text;
        decimal FromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        decimal FromTankAfter = Convert.ToDecimal(txtAftrTrnsROBFrom.Text == "" ? "0" : txtAftrTrnsROBFrom.Text);
        decimal transferQty = Math.Abs(FromTankAfter - FromTankBefore);
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;
        TimeSpan opeartionalTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtOperationalTime.SelectedTime.Value;
        lblRecord.Text = string.Format("<b>{0}</b> LT carried out operational tests as follows 1) Oil detection and alarm test of oil", opeartionalTime.ToString());
        lblRecord1.Text = string.Format("content monitor for 15ppm bilge separator and 2) Operation test of 3-way valve for overboard");
        lblRecord2.Text = string.Format("discharging and found them in good order.");
        lblRecord3.Text = string.Format("<b>{0}</b> m3 bilge water from {1}", transferQty, ddlTransferFrom.SelectedItem.Text);
        lblRecord4.Text = string.Format("capacity <b>{0}</b> m3, <b>{1}</b> m3 Retained", capacity, FromTankAfter);
        lblTimeRecord.Text = string.Format("Start:    <b>{0}</b>  ,  Stop:  <b>{1}</b> ", startTime.ToString(), stopTime.ToString());
        lblEquipment.Text = string.Format("Through 15 ppm equipment overboard");
        lblStartPos.Text = string.Format("Position Start: <b>{0}</b>", StartPos);
        lblStopPos.Text = string.Format("Position Stop: <b>{0}</b>", StopPos);
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

        if (string.IsNullOrWhiteSpace(ddlTransferFrom.Text))
        {
            ucError.ErrorMessage = "Bilge Water Transferred FROM is required";
        }

        if (string.IsNullOrWhiteSpace(txtAftrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After Transfer ROB is required";
        }
        else if (Convert.ToDecimal(txtAftrTrnsROBFrom.Text) > Convert.ToDecimal(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After ROB cannot be greater than Before ROB";
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

        if (txtOperationalTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Operational Time is required";
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

    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        ddlTransferFrom.DataBind();
        foreach (RadComboBoxItem item in ddlTransferFrom.Items)
        {
            item.Selected = true;
            break;
        }
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        SetDefaultData();
    }
    protected void txt_SelectedDateChanged(object sender, EventArgs e)
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
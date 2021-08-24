using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Log_ElectricLogQuarterlyTestingOWS : PhoenixBasePage
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

            ds = PhoenixElog.GetOperationRecord(
                                                  txid
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
                lblTranscationDetailId.Text= dr["FLDTRANSCATIONDETAILID"].ToString();
                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
                }

                owsDetail = PhoenixElog.OWSMaintenainceSearch(usercode, vesselId, Guid.Parse(lblTranscationDetailId.Text));
                if (owsDetail != null && owsDetail.Tables.Count > 0 && owsDetail.Tables[0].Rows.Count > 0)
                {
                    DataRow owsDetailRow = owsDetail.Tables[0].Rows[0];
                    txtSoundingBefore.Text = owsDetailRow["FLDSOUNDINGBEFORE"].ToString();
                    txtSoundingAfter.Text = owsDetailRow["FLDSOUNDINGAFTER"].ToString();
                    txtStartPosistionLat.Text = owsDetailRow["FLDSTARTPOSISTIONLAT"].ToString();
                    txtStartPosistionLog.Text = owsDetailRow["FLDSTARTPOSISTIONLON"].ToString();
                    txtStopPosistionLat.Text = owsDetailRow["FLDSTOPPOSISTIONLAT"].ToString();
                    txtStopPosistionLog.Text = owsDetailRow["FLDSTOPPOSISTIONLON"].ToString();
                    txtOperatingTime.Text = owsDetailRow["FLDOPERATINGHOUR"].ToString();
                    txtOWSCapacityRunTime.Text = owsDetailRow["FLDOWSCAPACITYRUN"].ToString();
                    txtStartTime.SelectedDate = Convert.ToDateTime(owsDetailRow["FLDSTARTTIME"].ToString());
                    txtStopTime.SelectedDate = Convert.ToDateTime(owsDetailRow["FLDSTOPTIME"].ToString());
                    lblBilgeTransferId.Text = owsDetailRow["FLDBILGEWATERTRANSCATIONID"].ToString();
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
        string logName = "Quarterly testing of OWS by actual overboard discharge";

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
                                      , null
                                      , lblRecord3.Text
                                      , null
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
                                      , null
                                      , lblRecord5.Text
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
                                      , null
                                      , lblRecord6.Text
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
                                      , lblRecord7.Text
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
                                      , lblRecord8.Text
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


            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , lblItemNo1.Text
                                      , lblRecord9.Text
                                      , lblCode1.Text
                                      , TranscationId
                                      , 12
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
                                      , lblRecord10.Text
                                      , null
                                      , TranscationId
                                      , 13
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
                                      , lblItemNo2.Text
                                      , lblRecord11.Text
                                      , null
                                      , TranscationId
                                      , 14
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
                                      , lblItemNo3.Text
                                      , lblRecord12.Text
                                      , null
                                      , TranscationId
                                      , 15
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
                                    , lblRecord13.Text
                                    , null
                                    , TranscationId
                                    , 16
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
                                    , lblRecord14.Text
                                    , null
                                    , TranscationId
                                    , 17
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
                                   , PhoenixElog.GetSignatureName(General.GetNullableString(lblSealinchName.Text), General.GetNullableString(lblSealincRank.Text), General.GetNullableDateTime(lblSealincSignDate.Text))
                                   , null
                                   , TranscationId
                                   , 18
                                   , false
                                   , null
                                   , General.GetNullableString(lblSealincRank.Text)
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
                                      , 19
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

    private void MissedOperationTemplateUpdate(string logName, Guid? TranscationId, Guid logId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lblRecord2.Text);
        nvc.Add("Record4", lblRecord3.Text);
        nvc.Add("Record5", lblRecord4.Text);
        nvc.Add("Record6", lblRecord5.Text);
        nvc.Add("Record7", lblRecord6.Text);
        nvc.Add("Record8", lblRecord7.Text);
        nvc.Add("Record9", lblRecord8.Text);
        nvc.Add("Record10", lblRecord9.Text);
        nvc.Add("Record11", lblRecord10.Text);
        nvc.Add("Record12", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        nvc.Add("Record13", lblRecord11.Text);
        nvc.Add("Record14", lblRecord12.Text);
        nvc.Add("Record15", lblRecord13.Text);
        nvc.Add("Record16", lblRecord14.Text);

        nvc.Add("ReportCode1",  txtCode.Text);
        nvc.Add("ReportCode12", lblCode1.Text);

        nvc.Add("ItemNo12", lblItemNo1.Text);
        nvc.Add("ItemNo14", lblItemNo2.Text);
        nvc.Add("ItemNo15", lblItemNo3.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        // add meta data for the log
        nvc.Add("logBookEntry", "16");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : TranscationId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TransactionInsert(out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        Guid TranscationOtherId = Guid.Empty;
        Guid transcationDetailId = Guid.Empty;
        Guid BilgeWaterTranscationId = Guid.Empty;
        Guid owsProcessID = PhoenixElog.GetProcessId(usercode, "OWS");
        logId = Guid.NewGuid();
        PhoenixElog.BilgeWellTransferInsert(
                usercode,
                ref BilgeWaterTranscationId,
                txtStartTime.SelectedDate.Value,
                txtStopTime.SelectedDate.Value
            );


        PhoenixElog.OWSMaintenainceInsert(
                usercode,
                logId,
                vesselId,
                txtOperationDate.SelectedDate.Value,
                txtStartTime.SelectedDate.Value,
                txtStopTime.SelectedDate.Value,
                txtStartPosistionLat.Text,
                txtStartPosistionLog.Text,
                txtStopPosistionLat.Text,
                txtStopPosistionLog.Text,
                Convert.ToDecimal(txtOperatingTime.Text),
                Convert.ToDecimal(txtOWSCapacityRunTime.Text),
                Convert.ToDecimal(txtSoundingBefore.Text),
                Convert.ToDecimal(txtSoundingAfter.Text),
                General.GetNullableInteger(lblinchId.Text),
                General.GetNullableString(lblincRank.Text),
                General.GetNullableString(lblincsign.Text),
                General.GetNullableDateTime(lblincSignDate.Text),
                General.GetNullableInteger(null),
                General.GetNullableString(null),
                General.GetNullableString(null),
                General.GetNullableDateTime(null),
                BilgeWaterTranscationId,
                ref transcationDetailId
        );


        PhoenixElog.InsertTransactionNew(usercode
                                    , ReportCode
                                    , new Guid(ddlTransferFrom.SelectedItem.Value)
                                    , ddlTransferFrom.SelectedItem.Text
                                    , owsProcessID
                                    , "Oil Water Separation"
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
                                    , transcationDetailId
                                    , logId
                                    , txtAftrTrnsROBFrom.Text
                                    , "0"
                                    , "OWS Operations"
                                    , isMissedOperation
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
                        , lblRecord5.Text
                        , txid
                        , 6
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord6.Text
                        , txid
                        , 7
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord7.Text
                        , txid
                        , 8
                        , null);


        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord8.Text
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

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord9.Text
                        , txid
                        , 12
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord10.Text
                        , txid
                        , 13
                        , null);


        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord11.Text
                        , txid
                        , 14
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord12.Text
                        , txid
                        , 15
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord13.Text
                        , txid
                        , 16
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord14.Text
                        , txid
                        , 17
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblSealinchName.Text), General.GetNullableString(lblSealincRank.Text), General.GetNullableDateTime(lblSealincSignDate.Text))
                        , txid
                        , 18
                        , false);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName("", "", null, true)
                        , txid
                        , 19
                        , false);

    }

    private void TransactionUpdate()
    {
        Guid owsProcessID = PhoenixElog.GetProcessId(usercode, "OWS");


        PhoenixElog.BilgeWellTransferUpdate(
                 usercode,
                 Guid.Parse(lblBilgeTransferId.Text),
                 txtStartTime.SelectedDate.Value,
                 txtStopTime.SelectedDate.Value
             );


        PhoenixElog.OWSMaintenainceUpdate(
                usercode,
                Guid.Parse(lblTranscationDetailId.Text),
                vesselId,
                txtOperationDate.SelectedDate.Value,
                txtStartTime.SelectedDate.Value,
                txtStopTime.SelectedDate.Value,
                txtStartPosistionLat.Text,
                txtStartPosistionLog.Text,
                txtStopPosistionLat.Text,
                txtStopPosistionLog.Text,
                Convert.ToDecimal(txtOperatingTime.Text),
                Convert.ToDecimal(txtOWSCapacityRunTime.Text),
                Convert.ToDecimal(txtSoundingBefore.Text),
                Convert.ToDecimal(txtSoundingAfter.Text),
                General.GetNullableInteger(lblinchId.Text),
                General.GetNullableString(lblincRank.Text),
                General.GetNullableString(lblincsign.Text),
                General.GetNullableDateTime(lblincSignDate.Text),
                General.GetNullableInteger(null),
                General.GetNullableString(null),
                General.GetNullableString(null),
                General.GetNullableDateTime(null),
                Guid.Parse(lblBilgeTransferId.Text)

        );



        //PhoenixElog.OWSOperationTransferUpdate(
        //        usercode,
        //        Convert.ToDecimal(txtOperatingTime.Text),
        //        Convert.ToDecimal(txtOWSCapacityRunTime.Text),
        //        txtStartPosistionLat.Text,
        //        txtStartPosistionLog.Text,
        //        txtStopPosistionLat.Text,
        //        txtStopPosistionLog.Text,
        //        Guid.Parse(lblBilgeTransferId.Text),
        //        Guid.Parse(lblTranscationDetailId.Text)
        //);


        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , owsProcessID
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

        lblCode1.Text = "D";
        txtItemNo.Text = ItemNo;
        lblItemNo1.Text = "13";
        lblItemNo2.Text = "14";
        lblItemNo3.Text = "15.3";
        txtCode.Text = ReportCode;
        txtDate.Text = PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value);
        lblDate1.Text = txtDate.Text;
        string capacity = "50";
        string StartPos = txtStartPosistionLat.Text + " , " + txtStartPosistionLog.Text;
        string StopPos = txtStopPosistionLat.Text + " , " + txtStopPosistionLog.Text;
        decimal FromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        decimal FromTankAfter = Convert.ToDecimal(txtAftrTrnsROBFrom.Text == "" ? "0" : txtAftrTrnsROBFrom.Text);
        decimal transferQty = Math.Abs(FromTankAfter - FromTankBefore);
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;
        lblRecord.Text = string.Format("Quarterly testing of OWS carried out as follows ( by actual discharge)");
        lblRecord1.Text = string.Format("1) Pump capacity verified by checking quantity discharged from bilge holding tank");
        lblRecord2.Text = string.Format("2) Oil content monitor alarm test done and found in order");
        lblRecord3.Text = string.Format("3) Sounding of bilge tank before test -  <b>{0}</b>  mtr", txtSoundingBefore.Text);
        lblRecord4.Text = string.Format("Sounding of bilge tank after test –  <b>{0}</b>  mtr", txtSoundingAfter.Text);
        lblRecord5.Text = string.Format("4) On activation of 15 ppm alarm the operation of 3 way valve checked and found satisfactory");
        lblRecord6.Text = string.Format("5) Pump shut off test carried out and found satisfactory");
        lblRecord7.Text = string.Format("6) Extension alarms in E/R (Audio/ Visual) and Records in ECR verified and found satisfactory");
        lblRecord8.Text = string.Format("Quarterly testing of OWS carried out as follows ( by actual discharge)");
        lblRecord9.Text = string.Format("{0}	m3 bilge water from		<b>{1}</b>", transferQty, ddlTransferFrom.SelectedItem.Text);
        lblRecord10.Text = string.Format("capacity	<b>{0}</b>	m3,	<b>{1}</b>	m3 Retained	", capacity, txtAftrTrnsROBFrom.Text);
        lblRecord11.Text = string.Format("Start:	<b>{0}</b>	, Stop:	<b>{1}</b>	", startTime, stopTime);
        lblRecord12.Text = string.Format("Through 15 ppm equipment overboard");
        lblRecord13.Text = string.Format("Position Start:	<b>{0} {1}</b>", txtStartPosistionLat.Text, txtStartPosistionLog.Text);
        lblRecord14.Text = string.Format("Position Stop:	<b>{0} {1}</b>", txtStopPosistionLat.Text, txtStopPosistionLog.Text);
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

        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }
}
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogBunkeringFuel : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    string ReportCode = "H";
    string itemName = string.Empty;
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
            ViewState["tankCounter"] = 1;
            ViewState["EXCEED98"] = "";
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            txtStartDate.SelectedDate = DateTime.Now;
            txtStopDate.SelectedDate = DateTime.Now;
            txtStartDate.MaxDate = DateTime.Now;
            txtStopDate.MaxDate = DateTime.Now;
            ddlFromPopulate();
            //ddlFromPopulate1();
            GetLastTranscationDate();
            setDefaultData();
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


    private void setDefaultData()
    {
        if (txtStartDate.SelectedDate == null) return;
        if (txtStopDate.SelectedDate == null) return;

        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;
        txtItemNo.Text = "26.1";
        txtItemNo1.Text = "26.2";
        txtItemNo2.Text = "26.3";
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue ? txtStartTime.SelectedTime.Value : new TimeSpan();
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue ? txtStopTime.SelectedTime.Value : new TimeSpan();
        DateTime? StartDate = General.GetNullableDateTime(txtStartDate.SelectedDate.Value.ToString());  //txtStartDate.SelectedDate.HasValue ? txtStartDate.SelectedDate.Value : null;
        DateTime? StopDate = General.GetNullableDateTime(txtStopDate.SelectedDate.Value.ToString());
        lblRecord0.Text = string.Format("<b>{0}</b>", txtPort.Text);
        lblRecord1.Text = string.Format("Start: <b>{0}</b> <b>{1}</b>  Stop  <b>{2}</b>  <b>{3}</b>", PhoenixElog.GetElogDateFormat(StartDate.Value).ToString(), startTime.ToString(), PhoenixElog.GetElogDateFormat(StopDate.Value).ToString(), stopTime.ToString());
        lblRecord2.Text = string.Format("<b>{0}</b> MT of ISO {1}   <b>{2}</b>  <b>{3}</b> Sulphur bunkered in tanks:", txtBunkerQty.Text, txtBunkerStandard.Text, txtBunkerType.Text, txtbunkerSulphur.Text);
        //lblTankRecord.Text = string.Format("<b>{0}</b> MT added to {1}   now containing  <b>{2}</b>  MT", txtbfrBunkerQty.Text, ddlTransferFrom.SelectedItem.Text, txtTotalQty.Text);
        //lblTankRecord1.Text = string.Format("<b>{0}</b> MT added to {1}   now containing  <b>{2}</b>  MT", txtbfrBunkerQty1.Text, ddlTransferFrom1.SelectedItem.Text, txtTotalQty1.Text);
        // loop
        int tankCount = Convert.ToInt32(ViewState["tankCounter"]);
        for (int i = 0; i <= tankCount; i++)
        {
            string controlName = "lblTankRecord" + i;
            string bfrBunkerQty = "txtbfrBunkerQty" + i;
            string tranferFrom = "ddlTransferFrom" + i;
            string totalQty = "txtTotalQty" + i;

            RadLabel ctrlRecord = (RadLabel)tblTemplate.FindControl(controlName);
            RadNumericTextBox ctrlbfrBunkerQty = (RadNumericTextBox)tblTemplate.FindControl(bfrBunkerQty);
            RadComboBox ctrlTransferFrom = (RadComboBox)tblTemplate.FindControl(tranferFrom);
            RadTextBox ctrlTotalQty = (RadTextBox)tblTemplate.FindControl(totalQty);
            if (ctrlTransferFrom.SelectedItem != null)
            {
                ctrlRecord.Text = string.Format("<b>{0}</b> MT added to {1}   now containing  <b>{2}</b>  MT", ctrlbfrBunkerQty.Text, ctrlTransferFrom.SelectedItem.Text, ctrlTotalQty.Text);
            }
        }
    }

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.OperationRecordByIdSearch(
                                                  usercode
                                                , vesselId
                                                , txid
                                                );
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["tankCounter"] = (ds.Tables[0].Rows.Count - 1); // zero based template
                int i = 0;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    RadComboBox ddlTankList = (RadComboBox)tankList.FindControl("ddlTransferFrom" + i);
                    RadNumericTextBox txtBftTrnsROB = (RadNumericTextBox)tankList.FindControl("txtBftTrnsROBFrom" + i);
                    RadNumericTextBox txtbfrBunker = (RadNumericTextBox)tankList.FindControl("txtbfrBunkerQty" + i);
                    RadTextBox txtTotal = (RadTextBox)tankList.FindControl("txtTotalQty" + i);
                    RadLabel lblRecordId = (RadLabel)tblTemplate.FindControl("lblRecordId" + i);

                    RadComboBoxItem fromItem = ddlTankList.Items.FindItem(x => x.Text == row["FLDTOLOCATION"].ToString());
                    if (fromItem != null)
                    {
                        fromItem.Selected = true;
                    }
                    ddlTankList.Enabled = false;

                    lblRecordId.Text = row["FLDLOGITEMBYTANKTXID"].ToString();
                    txtBftTrnsROB.Text = row["FLDFROMROB"].ToString();
                    txtbfrBunker.Text = row["FLDTXQTY"].ToString();
                    txtTotal.Text = (Convert.ToDecimal(txtbfrBunker.Text) + Convert.ToDecimal(txtBftTrnsROB.Text)).ToString();
                    
                    string bfrTransfer = "txtBftTrnsROBFrom" + i;
                    string transferFrom = "ddlTransferFrom" + i;
                    string rowName = "rowTankRecord" + i;

                    string controlName = "lblTankRecord" + i;
                    string bfrBunkerQty = "txtbfrBunkerQty" + i;
                    string tranferFrom = "ddlTransferFrom" + i;
                    string totalQty = "txtTotalQty" + i;

                    RadNumericTextBox txtbfrTransnfer = (RadNumericTextBox)tankList.FindControl(bfrTransfer);
                    RadComboBox ddlTranferFrom = (RadComboBox)tankList.FindControl(transferFrom);
                    RadNumericTextBox txtbfrBunkerQty = (RadNumericTextBox)tankList.FindControl(bfrBunkerQty);
                    RadTextBox txtTotalQty = (RadTextBox)tankList.FindControl(totalQty);
                    RadLabel ctrlRecord = (RadLabel)tblTemplate.FindControl(controlName);
                    RadNumericTextBox ctrlbfrBunkerQty = (RadNumericTextBox)tblTemplate.FindControl(bfrBunkerQty);
                    RadComboBox ctrlTransferFrom = (RadComboBox)tblTemplate.FindControl(tranferFrom);
                    RadTextBox ctrlTotalQty = (RadTextBox)tblTemplate.FindControl(totalQty);

                    txtbfrTransnfer.Visible = true;
                    ddlTranferFrom.Visible = true;
                    txtbfrBunkerQty.Visible = true;
                    txtTotalQty.Visible = true;
                    ctrlRecord.Text = string.Format("<b>{0}</b> MT added to {1}   now containing  <b>{2}</b>  MT", ctrlbfrBunkerQty.Text, ctrlTransferFrom.Text, ctrlTotalQty.Text);
                    HtmlTableRow tblrow = (HtmlTableRow)tblTemplate.FindControl(rowName);
                    tblrow.Attributes.Add("style", "display:table-row;");

                }

                //ViewState["tankCounter"] = i;
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());
                lblBunkeringId.Text = dr["FLDTRANSCATIONDETAILID"].ToString();
                Guid bunkerDetailId = Guid.Parse(lblBunkeringId.Text);

                DataSet bunkeringDetail = PhoenixElog.BunkeringFuelSearch(usercode, vesselId, bunkerDetailId);
                DataRow bunkerDetailRow = bunkeringDetail.Tables[0].Rows[0];
                txtPort.Text = (string)bunkerDetailRow["FLDBUNKERPORTNAME"].ToString();
                txtStartDate.SelectedDate = Convert.ToDateTime(bunkerDetailRow["FLDSTARDATETTIME"]);
                txtStopDate.SelectedDate = Convert.ToDateTime(bunkerDetailRow["FLDSTOPDATETIME"]);
                txtStartTime.SelectedDate = Convert.ToDateTime(bunkerDetailRow["FLDSTARDATETTIME"]);
                txtStopTime.SelectedDate = Convert.ToDateTime(bunkerDetailRow["FLDSTOPDATETIME"]);
                txtBunkerQty.Text = (string)bunkerDetailRow["FLDBUNKERQUA"].ToString();
                txtBunkerStandard.Text = (string)bunkerDetailRow["FLDBUNKERISO"].ToString();
                txtbunkerSulphur.Text = (string)bunkerDetailRow["FLDBUNKERSULPHUR"].ToString();
                txtBunkerType.Text = bunkerDetailRow["FLDBUNKERTYPE"].ToString();

                setDefaultData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        string logName = "Bunkering Fuel";
        DateTime StartDateTime = txtStartDate.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime StopDateTime = txtStopDate.SelectedDate.Value.Add(txtStopTime.SelectedTime.Value);

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId, TranscationId;
            int tankCount;
            TranscationInsert(StartDateTime, StopDateTime, out logId, out TranscationId, out tankCount);
            MissedOperationEntryTemplateUpdate(logName, StartDateTime, StopDateTime, logId, tankCount, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            int tankCount = TranscationUpdate();
            MissedOperationEntryTemplateUpdate(logName, StartDateTime, StopDateTime, txid, tankCount, null);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {
            Guid IncinerationId = PhoenixElog.GetProcessId(usercode, "BUKFUE");
            DateTime logBookEntryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);

            Guid logId, TranscationId;
            int tankCount;
            TranscationInsert(StartDateTime, StopDateTime, out logId, out TranscationId, out tankCount);

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , txtItemNo.Text
                                      , lblRecord0.Text
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
                                      , true
                                      , entryNo
                                      , logId
                                  );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , txtItemNo1.Text
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
                                      , true
                                      , entryNo
                                      , logId
                                );

            PhoenixElog.LogBookEntryInsert(usercode
                                  , logBookEntryDate
                                  , txtItemNo2.Text
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
                                      , true
                                      , entryNo
                                      , logId
                            );

            // here need to loop to get all the additional tanks
            int logCount = 4;
            for (int i = 0; i <= tankCount; i++)
            {
                string record = "lblTankRecord" + i;
                RadLabel ctrlRecord = (RadLabel)tblTemplate.FindControl(record);
                PhoenixElog.LogBookEntryInsert(usercode
                                         , logBookEntryDate
                                         , null
                                         , ctrlRecord.Text
                                         , null
                                         , TranscationId
                                         , logCount
                                           , null
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , true
                                      , entryNo
                                      , logId
                 );
                logCount++;
            }

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , null
                                      , TranscationId
                                      , logCount
                                      , false
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , true
                                      , entryNo
                                      , logId
                                );

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , null
                                      , PhoenixElog.GetSignatureName("", "", null, true)
                                      , null
                                      , TranscationId
                                      , logCount + 1
                                      , true
                                        , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , true
                                      , null
                                      , logName
                                      , true
                                      , entryNo
                                      , logId
                                );

        }
    }

    private void MissedOperationEntryTemplateUpdate(string logName, DateTime StartDateTime, DateTime StopDateTime, Guid logId, int tankCount, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("portOfBunker", txtPort.Text);
        nvc.Add("startDate", StartDateTime.ToString());
        nvc.Add("stopDate", StopDateTime.ToString());
        nvc.Add("bunkerQuantity", txtBunkerQty.Text);
        nvc.Add("typeOfBunker", txtBunkerType.Text);
        nvc.Add("isoStandarad", txtBunkerStandard.Text);
        nvc.Add("bunkerSulphur", txtbunkerSulphur.Text);
        // add for logbook
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));
        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("Record1", lblRecord0.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lblRecord2.Text);

        // loop and get the value
        for (int i = 0; i <= tankCount; i++)
        {
            string bfrTransfer = "txtBftTrnsROBFrom" + i;
            string transferFrom = "ddlTransferFrom" + i;
            string bfrBunkerQty = "txtbfrBunkerQty" + i;
            string totalQty = "txtTotalQty" + i;
            string controlName = "lblTankRecord" + i;
            string tranferFrom = "ddlTransferFrom" + i;

            RadLabel lblTankRecord = (RadLabel)tankList.FindControl(controlName);

            nvc.Add("Record" + (i + 4), lblTankRecord.Text);


        }
        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        // add meta data for the log
        nvc.Add("logBookEntry", (4 + tankCount) .ToString());
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        nvc.Add("isAttachmentRequired", "true");

        Filter.MissedOperationalEntryCriteria = nvc;
    }


    private void TranscationInsert(DateTime StartDateTime, DateTime StopDateTime, out Guid logId, out Guid TranscationId, out int tankCount)
    {
        logId = Guid.NewGuid();
        TranscationId = Guid.Empty;
        Guid BunkerId = Guid.Empty;

        PhoenixElog.BunkeringFuelInsert(usercode
                                    , vesselId
                                    , txtPort.Text
                                    , StartDateTime
                                    , StopDateTime
                                    , Convert.ToDecimal(txtBunkerQty.Text)
                                    , txtBunkerStandard.Text
                                    , txtbunkerSulphur.Text
                                    , txtBunkerType.Text
                                    , ref BunkerId
                                );


        tankCount = Convert.ToInt32(ViewState["tankCounter"]);
        for (int i = 0; i <= tankCount; i++)
        {
            string bfrTransfer = "txtBftTrnsROBFrom" + i;
            string transferFrom = "ddlTransferFrom" + i;
            string bfrBunkerQty = "txtbfrBunkerQty" + i;
            string totalQty = "txtTotalQty" + i;
            string controlName = "lblTankRecord" + i;
            string tranferFrom = "ddlTransferFrom" + i;

            RadNumericTextBox ctrlbfrTransnfer = (RadNumericTextBox)tankList.FindControl(bfrTransfer);
            RadComboBox ctrlTranferFrom = (RadComboBox)tankList.FindControl(transferFrom);
            RadTextBox ctrlTotalQty = (RadTextBox)tankList.FindControl(totalQty);
            RadNumericTextBox ctrlBunkerQty = (RadNumericTextBox)tblTemplate.FindControl(bfrBunkerQty);

            PhoenixElog.InsertTransactionNew(usercode
                                          , ReportCode
                                          , BunkerId
                                          , "Bunkering"
                                          , new Guid(ctrlTranferFrom.SelectedItem.Value)
                                          , ctrlTranferFrom.SelectedItem.Text
                                          , null
                                          , null
                                          , itemName
                                          , ctrlbfrTransnfer.Text.ToString()
                                          , ctrlTotalQty.Text
                                          , ctrlBunkerQty.Text.ToString()
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
                                          , BunkerId
                                          , logId
                                          , null
                                          , null
                                          , "Bunkering Fuel"
                                          , isMissedOperation
                                      );

        }
    }

    private void LogBookUpdate()
    {
        int tankCount = Convert.ToInt32(ViewState["tankCounter"]);


        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord0.Text
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


        int logCount = 4;
        for (int i = 0; i <= tankCount; i++)
        {
            string record = "lblTankRecord" + i;
            RadLabel ctrlRecord = (RadLabel)tblTemplate.FindControl(record);
            PhoenixElog.LogBookEntryUpdate(usercode
                        , ctrlRecord.Text
                        , txid
                        , logCount
                        , null);
            logCount++;
        }

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                        , txid
                        , logCount 
                        , false);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName("", "", null, true)
                        , txid
                        , logCount + 1
                        , true);
    }

    private int TranscationUpdate()
    {
        //bunkering update has to be done
        Guid BunkeringId = PhoenixElog.GetProcessId(usercode, "BUKFUE");
        DateTime StartDateTime = txtStartDate.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime StopDateTime = txtStopDate.SelectedDate.Value.Add(txtStopTime.SelectedTime.Value);

        PhoenixElog.BunkeringFuelUpdate(usercode
                            , vesselId
                            , txtPort.Text
                            , StartDateTime
                            , StopDateTime
                            , Convert.ToDecimal(txtBunkerQty.Text)
                            , txtBunkerStandard.Text
                            , txtbunkerSulphur.Text
                            , txtBunkerType.Text
                            , Guid.Parse(lblBunkeringId.Text)
                        );



        //// here loop and insert based on the tank list
        int tankCount = Convert.ToInt32(ViewState["tankCounter"]);
        int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);

        for (int i = 0; i <= tankCount; i++)
        {
            string bfrTransfer = "txtBftTrnsROBFrom" + i;
            string transferFrom = "ddlTransferFrom" + i;
            string bfrBunkerQty = "txtbfrBunkerQty" + i;
            string totalQty = "txtTotalQty" + i;
            string controlName = "lblTankRecord" + i;
            string tranferFrom = "ddlTransferFrom" + i;

            RadNumericTextBox ctrlbfrTransnfer = (RadNumericTextBox)tankList.FindControl(bfrTransfer);
            RadComboBox ctrlTranferFrom = (RadComboBox)tankList.FindControl(transferFrom);
            RadTextBox ctrlTotalQty = (RadTextBox)tankList.FindControl(totalQty);
            RadNumericTextBox ctrlbfrBunkerQty = (RadNumericTextBox)tblTemplate.FindControl(bfrBunkerQty);
            RadLabel lblTxid = (RadLabel)tblTemplate.FindControl("lblRecordId" + i);


            PhoenixElog.UpdateTransaction(Guid.Parse(lblTxid.Text),
                                           usercode
                                          , BunkeringId
                                          , ctrlbfrTransnfer.Text.ToString()
                                          , ctrlbfrBunkerQty.Text.ToString()
                                          , ctrlTotalQty.Text
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
                                      );

        }

        return tankCount;
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateROB();
        setDefaultData();
    }
    protected void ddlTransferFrom1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateROB();
        setDefaultData();
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixElog.GetLogLastTranscationDate(vesselId, usercode);
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(txtPort.Text))
        {
            ucError.ErrorMessage = "Port of Bunker is required";
        }

        if (txtStartDate.SelectedDate.HasValue == false || txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Date is required";
        }

        if (txtStopDate.SelectedDate.HasValue == false || txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Date is required";
        }

        if (string.IsNullOrWhiteSpace(txtBunkerQty.Text))
        {
            ucError.ErrorMessage = "Bunker Quantity is required";
        }

        if (string.IsNullOrWhiteSpace(txtBunkerType.Text))
        {
            ucError.ErrorMessage = "Bunker Type is required";
        }

        if (string.IsNullOrWhiteSpace(txtBunkerStandard.Text))
        {
            ucError.ErrorMessage = "Bunker Standard is required";
        }

        if (string.IsNullOrWhiteSpace(txtbunkerSulphur.Text))
        {
            ucError.ErrorMessage = "Bunker Sulphur is required";
        }

        if(ViewState["EXCEED98"].ToString() == "1")
        {
            ucError.ErrorMessage = "'Cannot perform transaction. Exceeds capacity.'";
            ViewState["EXCEED98"] = "";
        }

        //if (string.IsNullOrWhiteSpace(ddlTransferFrom.SelectedValue))
        //{
        //    ucError.ErrorMessage = "Tank is required";
        //}
        //else if (string.IsNullOrWhiteSpace(ddlTransferFrom1.SelectedValue))
        //{
        //    ucError.ErrorMessage = "Tank is required";
        //}
        //else if (ddlTransferFrom1.SelectedItem.Text == ddlTransferFrom.SelectedItem.Text)
        //{
        //    ucError.ErrorMessage = "Tanks cannot be the same";
        //}

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        //if (false)
        //{
        //    ucError.ErrorMessage = "Total bunker quantity should be equal to two tanks";
        //}

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
        DataSet ds = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= 9; i++)
            {
                string controlName = "ddlTransferFrom" + i;
                RadComboBox dllTransfer = (RadComboBox)tankList.FindControl(controlName);
                dllTransfer.DataSource = ds;
                dllTransfer.DataBind();
                dllTransfer.Items[0].Selected = true;
            }
        }
    }
    private void ddlFromPopulate1()
    {
        ddlTransferFrom1.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        ddlTransferFrom1.DataBind();
        foreach (RadComboBoxItem item in ddlTransferFrom1.Items)
        {
            item.Selected = true;
            break;
        }
    }
    
    protected void txtOperationDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        setDefaultData();
    }
    protected void txtbfrBunkerQty_TextChanged(object sender, EventArgs e)
    {
        CalculateROB();
        setDefaultData();
    }
    protected void txtbfrBunkerQty1_TextChanged(object sender, EventArgs e)
    {
        CalculateROB();
        setDefaultData();
    }

    private void CalculateROB()
    {
        int tankCount = Convert.ToInt32(ViewState["tankCounter"]);
        for (int i = 0; i <= tankCount; i++)
        {
            string bfrTransfer = "txtBftTrnsROBFrom" + i;
            string bfrBunkerQty = "txtbfrBunkerQty" + i;
            string totalQty = "txtTotalQty" + i;
            string transferFrom = "ddlTransferFrom" + i;
            string capacityerror = "lblcapacityexceed" + i;
            decimal tankcapacity = 0;

            RadNumericTextBox ctrlbfrTransnfer = (RadNumericTextBox)tankList.FindControl(bfrTransfer);
            RadNumericTextBox ctrlbfrBunkerQty = (RadNumericTextBox)tankList.FindControl(bfrBunkerQty);
            RadTextBox ctrlTotalQty = (RadTextBox)tankList.FindControl(totalQty);
            RadComboBox ctrlTranferFrom = (RadComboBox)tankList.FindControl(transferFrom);
            RadLabel capacityexceed = (RadLabel)tankList.FindControl(capacityerror);

            DataSet ds = PhoenixElog.GetTankCapacity(usercode, vesselId, new Guid(ctrlTranferFrom.SelectedItem.Value));

            tankcapacity = decimal.Parse(ds.Tables[0].Rows[0]["FLDCAPACITY"].ToString());

            if (string.IsNullOrWhiteSpace(ctrlbfrTransnfer.Text) == false && string.IsNullOrWhiteSpace(ctrlbfrBunkerQty.Text) == false)
            {
                ctrlTotalQty.Text = (Convert.ToDecimal(ctrlbfrTransnfer.Text) + Convert.ToDecimal(ctrlbfrBunkerQty.Text)).ToString();
                if (Convert.ToDecimal(ctrlTotalQty.Text) > Convert.ToDecimal((tankcapacity / 100) * 85) && Convert.ToDecimal(ctrlTotalQty.Text) < Convert.ToDecimal((tankcapacity / 100) * 98))
                {
                    capacityexceed.Text = "<span style='color: Red; '>*Exceed 85% of tank capacity</span>";
                    ViewState["EXCEED98"] = "";
                }
                else if (Convert.ToDecimal(ctrlTotalQty.Text) > Convert.ToDecimal((tankcapacity / 100) * 98))
                {
                    capacityexceed.Text = "<span style='color: Red; '>* Exceed 98% of tank capacity</span>";                    
                    ViewState["EXCEED98"] = "1";
                }
                else
                { 
                    capacityexceed.Text = "";                    
                }
            }
        }
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        CalculateROB();
        setDefaultData();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["tankCounter"] == null) return;
            if (Convert.ToInt32(ViewState["tankCounter"]) >=  9) throw new ArgumentException("Maximum tank added");
            if (txid != Guid.Empty) throw new ArgumentException("Cannot add tank on edit mode");

            string bfrTransfer = "txtBftTrnsROBFrom" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);
            string transferFrom = "ddlTransferFrom" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);
            string rowName = "rowTankRecord" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);

            string controlName = "lblTankRecord" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);
            string bfrBunkerQty = "txtbfrBunkerQty" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);
            string tranferFrom = "ddlTransferFrom" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);
            string totalQty = "txtTotalQty" + (Convert.ToInt32(ViewState["tankCounter"]) + 1);

            RadNumericTextBox txtbfrTransnfer = (RadNumericTextBox)tankList.FindControl(bfrTransfer);
            RadComboBox ddlTranferFrom = (RadComboBox)tankList.FindControl(transferFrom);
            RadNumericTextBox txtbfrBunkerQty = (RadNumericTextBox)tankList.FindControl(bfrBunkerQty);
            RadTextBox txtTotalQty = (RadTextBox)tankList.FindControl(totalQty);
            RadLabel ctrlRecord = (RadLabel)tblTemplate.FindControl(controlName);
            RadNumericTextBox ctrlbfrBunkerQty = (RadNumericTextBox)tblTemplate.FindControl(bfrBunkerQty);
            RadComboBox ctrlTransferFrom = (RadComboBox)tblTemplate.FindControl(tranferFrom);
            RadTextBox ctrlTotalQty = (RadTextBox)tblTemplate.FindControl(totalQty);

            txtbfrTransnfer.Visible = true;
            ddlTranferFrom.Visible = true;
            txtbfrBunkerQty.Visible = true;
            txtTotalQty.Visible = true;
            ctrlRecord.Text = string.Format("<b>{0}</b> MT added to {1}   now containing  <b>{2}</b>  MT", ctrlbfrBunkerQty.Text, ctrlTransferFrom.Text, ctrlTotalQty.Text);
            HtmlTableRow row = (HtmlTableRow)tblTemplate.FindControl(rowName);
            row.Attributes.Add("style", "display:table-row;");


            ViewState["tankCounter"] = Convert.ToInt32(ViewState["tankCounter"]) + 1;

            setDefaultData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["tankCounter"] == null) return;
            if (Convert.ToInt32(ViewState["tankCounter"]) <= 1) throw new ArgumentException("Minimum tank reached");
            if (txid != Guid.Empty) throw new ArgumentException("Cannot Delete tank on edit mode");
            
            int tankCount = (Convert.ToInt32(ViewState["tankCounter"]));
            string bfrTransfer = "txtBftTrnsROBFrom" + tankCount;
            string transferFrom = "ddlTransferFrom" + tankCount;
            string bfrBunkerQty = "txtbfrBunkerQty" + tankCount;
            string totalQty = "txtTotalQty" + tankCount;
            string rowName = "rowTankRecord" + tankCount;

            RadNumericTextBox txtbfrTransnfer = (RadNumericTextBox)tankList.FindControl(bfrTransfer);
            RadComboBox ddlTranferFrom = (RadComboBox)tankList.FindControl(transferFrom);
            RadNumericTextBox txtbfrBunkerQty = (RadNumericTextBox)tankList.FindControl(bfrBunkerQty);
            RadTextBox txtTotalQty = (RadTextBox)tankList.FindControl(totalQty);


            txtbfrTransnfer.Visible = false;
            ddlTranferFrom.Visible = false;
            txtbfrBunkerQty.Visible = false;
            txtTotalQty.Visible = false;
            HtmlTableRow row = (HtmlTableRow)tblTemplate.FindControl(rowName);
            row.Attributes.Add("style", "display:none;");

            ViewState["tankCounter"] = Convert.ToInt32(ViewState["tankCounter"]) - 1;
            setDefaultData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
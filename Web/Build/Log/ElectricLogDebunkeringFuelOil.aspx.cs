using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogDebunkeringFuelOil : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    string ReportCode = "I";
    string itemNo = null;
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
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            txtStartDate.SelectedDate = DateTime.Now;
            txtStopDate.SelectedDate = DateTime.Now;
            ddlFromPopulate();
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

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.GetOperationRecord(
                                                  txid
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , ReportCode
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                );

            //

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblDebunkeringId.Text = dr["FLDTRANSCATIONDETAILID"].ToString();
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());

                DataSet DebunkeringDataSet = PhoenixElog.DeBunkeringSearch(usercode, vesselId,Guid.Parse(lblDebunkeringId.Text));
                if (DebunkeringDataSet != null && DebunkeringDataSet.Tables[0].Rows.Count  > 0)
                {
                    DataRow DebunkeringRow = DebunkeringDataSet.Tables[0].Rows[0];

                    txtBfrTrnsROBFrom.Text = DebunkeringRow["FLDFROMROB"].ToString();
                    txtTruck.Text = DebunkeringRow["FLDFACILITYNAME"].ToString();
                    txtPort.Text = DebunkeringRow["FLDDEBUNKERPORTNAME"].ToString();
                    txtStartTime.SelectedDate = Convert.ToDateTime(DebunkeringRow["FLDSTARDATETTIME"].ToString());
                    txtStopTime.SelectedDate = Convert.ToDateTime(DebunkeringRow["FLDSTOPDATETIME"].ToString());
                    txtStartDate.SelectedDate = Convert.ToDateTime(DebunkeringRow["FLDSTARDATETTIME"].ToString());
                    txtStopDate.SelectedDate = Convert.ToDateTime(DebunkeringRow["FLDSTOPDATETIME"].ToString());
                    txtDebunkerQty.Text = DebunkeringRow["FLDDEBUNKERQTY"].ToString();
                    txtBunkerQty.Text = DebunkeringRow["FLDTXQTY"].ToString();
                    txtBunkerType.Text = DebunkeringRow["FLDBUNKERTYPE"].ToString();
                    txtBunkerStandard.Text = DebunkeringRow["FLDISOSTANDARD"].ToString();
                    txtbunkerSulphur.Text = DebunkeringRow["FLDBUNKERSULPHUR"].ToString();
                    txtRob.Text = DebunkeringRow["FLDTOROB"].ToString();
                    RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                    if (fromItem != null)
                    {
                        fromItem.Selected = true;
                    }
                }
                setDefaultData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void setDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;
        if (txtStartDate.SelectedDate == null) return;
        if (txtStopDate.SelectedDate == null) return;

        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;
        txtItemNo.Text = "";
        txtItemNo1.Text = "";
        txtItemNo2.Text = "";
        txtBunkerQty.Text  = txtDebunkerQty.Text;
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue ? txtStartTime.SelectedTime.Value : new TimeSpan();
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue ? txtStopTime.SelectedTime.Value : new TimeSpan();
        lblRecord.Text = string.Format("<b>{0}</b> MT of ISO  <b>{1}</b>  HFO  <b>{2}</b>  Sulphur de-bunkered from tanks", txtDebunkerQty.Text, txtBunkerStandard.Text, txtbunkerSulphur.Text);
        lblRecord1.Text = string.Format("<b>{0}</b> MT removed from <b>{1}</b>  now containing  <b>{2}</b> MT", txtDebunkerQty.Text, ddlTransferFrom.SelectedItem.Text, txtRob.Text);
        lblRecord2.Text = string.Format("De-bunkered to <b>{0}</b> in port <b>{1}</b>", txtTruck.Text, txtPort.Text);
        lblRecord3.Text = string.Format("start <b>{0}</b>  <b>{1}</b> Stop <b>{2}</b>  <b>{3}</b>", PhoenixElog.GetElogDateFormat(txtStartDate.SelectedDate.Value).ToString(), startTime.ToString(), PhoenixElog.GetElogDateFormat(txtStopDate.SelectedDate.Value).ToString(), stopTime.ToString());
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }

    private void CalculateROB()
    {
        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBFrom.Text) == false && string.IsNullOrWhiteSpace(txtRob.Text) == false)
        {
            //txtDebunkerQty.Text = (Convert.ToInt32(Convert.ToDecimal(txtBfrTrnsROBFrom.Text)) - Convert.ToInt32(Convert.ToDecimal(txtRob.Text))).ToString();
            txtDebunkerQty.Text = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtRob.Text)).ToString();
        }
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
        string logName = "De-Bunkering of Fuel Oil";
        DateTime StartDateTime = txtStartDate.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime StopDateTime = txtStopDate.SelectedDate.Value.Add(txtStopTime.SelectedTime.Value);

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId, logId;
            TransactionInsert(StartDateTime, StopDateTime, out TranscationId, out logId);
            MissedOperationTemplateUpdate(logName, StartDateTime, StopDateTime, TranscationId, logId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate();
            MissedOperationTemplateUpdate(logName, StartDateTime, StopDateTime, null, txid);
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
            TransactionInsert(StartDateTime, StopDateTime, out TranscationId, out logId);

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
                                      , null
                                      , null
                                      , logName
                                      , true
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
                                      , null
                                      , null
                                      , logName
                                      , true
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
                                      , null
                                      , null
                                      , logName
                                      , true
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
                                      , null
                                      , null
                                      , logName
                                      , true
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
                                      , null
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
                                      , 6
                                      , true
                                      , null
                                      , General.GetNullableString(lblincRank.Text)
                                      , vesselId
                                      , null
                                      , null
                                      , logName
                                      , true
                                      , entryNo
                                      , logId
                                );
        }
    }

    private void MissedOperationTemplateUpdate(string logName, DateTime StartDateTime, DateTime StopDateTime, Guid? TranscationId, Guid logId)
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
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lblRecord2.Text);
        nvc.Add("Record4", lblRecord3.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
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
        nvc.Add("isTransaction", "true");
        nvc.Add("isAttachmentRequired", "true");
        nvc.Add("txId", txid == null ? "" : TranscationId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TransactionInsert(DateTime StartDateTime, DateTime StopDateTime, out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        Guid DebunkerId = Guid.Empty;
        logId = Guid.NewGuid();
        PhoenixElog.DeBunkeringInsert(usercode
                                    , vesselId
                                    , txtPort.Text
                                    , txtTruck.Text
                                    , StartDateTime
                                    , StopDateTime
                                    , txtBunkerQty.Text
                                    , txtBunkerStandard.Text
                                    , txtbunkerSulphur.Text
                                    , txtBunkerType.Text
                                    , ref DebunkerId
                                );


        PhoenixElog.InsertTransactionNew(usercode
                                      , ReportCode
                                      , new Guid(ddlTransferFrom.SelectedItem.Value)
                                      , ddlTransferFrom.SelectedItem.Text
                                      , new Guid()
                                      , "De-Bunkering"
                                      , txtCode.Text
                                      , itemNo
                                      , itemName
                                      , txtBfrTrnsROBFrom.Text
                                      , txtRob.Text
                                      , txtDebunkerQty.Text
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
                                      , DebunkerId
                                      , logId
                                      , null
                                      , null
                                      , "De-Bunkering of Fuel Oil"
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

    private void TransactionUpdate()
    {
        DateTime StartDateTime = txtStartDate.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime StopDateTime = txtStopDate.SelectedDate.Value.Add(txtStopTime.SelectedTime.Value);

        PhoenixElog.DeBunkeringUpdate(usercode, vesselId, Guid.Parse(lblDebunkeringId.Text), txtPort.Text, txtTruck.Text,
                StartDateTime, StopDateTime , txtDebunkerQty.Text, txtBunkerStandard.Text, txtbunkerSulphur.Text, txtBunkerType.Text
            );

        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , new Guid(ddlTransferFrom.SelectedItem.Value)
                                                , txtBfrTrnsROBFrom.Text
                                                , txtRob.Text
                                                , txtDebunkerQty.Text
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

    protected void txtStartDate_TextChangedEvent(object sender, EventArgs e)
    {
        setDefaultData();
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

        if (string.IsNullOrWhiteSpace(ddlTransferFrom.SelectedValue))
        {
            ucError.ErrorMessage = "Tank is required";
        }
        
        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false && isMissedOperation == false  )
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transcation date and not greater than current date";
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
    
    
    protected void txt_TextChanged(object sender, EventArgs e)
    {
        CalculateROB();
        setDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        setDefaultData();
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
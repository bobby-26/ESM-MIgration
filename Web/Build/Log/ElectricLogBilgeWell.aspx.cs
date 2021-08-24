using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogBilgeWell : PhoenixBasePage
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
            DataSet BilgeDetail = new DataSet();

            ds = PhoenixElog.GetOperationRecord(
                                                  //new Guid(ViewState["TransactionID"].ToString())
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
                lblTranscationDetailId.Text = dr["FLDTRANSCATIONDETAILID"].ToString();

                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
                }

                BilgeDetail = PhoenixElog.BilgeWellTransferSearch(usercode, Guid.Parse(lblTranscationDetailId.Text), vesselId);
                DataRow BilgeDetailRow = BilgeDetail.Tables[0].Rows[0];
                txtStartTime.SelectedDate = Convert.ToDateTime(BilgeDetailRow["FLDSTARTTIME"].ToString());
                txtStopTime.SelectedDate = Convert.ToDateTime(BilgeDetailRow["FLDENDTIME"].ToString());

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
        string logName = "Bilge Well";
        Guid bilgeWellId = PhoenixElog.GetProcessId(usercode, "BLG");

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId, logId;
            TranscationInsert(bilgeWellId, out TranscationId, out logId);
            MissedOperationTemplateUpdate(logName, bilgeWellId, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationTemplateUpdate(logName, bilgeWellId, txid, null);
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
            Guid TranscationId, logId;
            TranscationInsert(bilgeWellId, out TranscationId, out logId);

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
                                      , txtcode2.Text
                                      , lblTimeRecord.Text
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
                                  , txtcode3.Text
                                  , lbltorecord.Text
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
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , null
                                      , TranscationId
                                      , 4
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
                                      , 5
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

    private void MissedOperationTemplateUpdate(string logName, Guid bilgeWellId, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        string txQty = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAftrTrnsROBFrom.Text)).ToString();
        // store transaction 
        nvc.Add("FromTankId", bilgeWellId.ToString());
        nvc.Add("FromTank", "rcom Bilge Wells");
        nvc.Add("ToTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("ToTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("FromRob", txtBfrTrnsROBFrom.Text);
        nvc.Add("ToRob", txtAftrTrnsROBFrom.Text);
        nvc.Add("TransferQty", txQty);
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblTimeRecord.Text);
        nvc.Add("Record3", lbltorecord.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemName1", ItemName);

        nvc.Add("ItemNo2", txtcode2.Text);
        nvc.Add("ItemNo3", txtcode3.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        nvc.Add("startTime", txtStartTime.SelectedDate.Value.ToString());
        nvc.Add("endTime", txtStopTime.SelectedDate.Value.ToString());
        // add meta data for the log
        nvc.Add("logBookEntry", "3");
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        nvc.Add("logBookName", logName);
        Filter.MissedOperationalEntryCriteria = nvc;
    }


    private void TranscationInsert(Guid bilgeWellId, out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        Guid TranscationOtherId = Guid.Empty;
        logId = Guid.NewGuid();
        PhoenixElog.BilgeWellTransferInsert(
                usercode,
                ref TranscationOtherId,
                txtStartTime.SelectedDate.Value,
                txtStopTime.SelectedDate.Value
        );


        PhoenixElog.InsertTransactionNew(usercode
                                    , ReportCode
                                    , bilgeWellId
                                    , "rcom bilge wells"
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
                                    , "Bilge Well"
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
                        , lblTimeRecord.Text
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
        PhoenixElog.BilgeWellTransferUpdate(
                                    usercode,
                                    Guid.Parse(lblTranscationDetailId.Text),
                                    txtStartTime.SelectedDate.Value,
                                    txtStopTime.SelectedDate.Value
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
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtcode2.Text = "14";
        txtcode3.Text = "15.3";
        decimal FromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        decimal FromTankAfter = Convert.ToDecimal(txtAftrTrnsROBFrom.Text == "" ? "0" : txtAftrTrnsROBFrom.Text);
        decimal transferQty = Math.Abs(FromTankAfter - FromTankBefore);
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;
        lblRecord.Text = string.Format("<b>{0}</b> m3 bilge water from engine room bilge wells", transferQty);
        lblTimeRecord.Text = string.Format("Start:    <b>{0}</b>  ,  Stop:  <b>{1}</b> ", startTime.ToString(), stopTime.ToString());
        lbltorecord.Text = string.Format("To    <b>{0}</b>  ,  Retained in tank    <b>{1}</b>   m3", ddlTransferFrom.SelectedItem.Text, FromTankAfter);
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
            ucError.ErrorMessage = "Bilge Well Transfer is required";
        }
        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }
        if (txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "End Time is required";
        }

        if (string.IsNullOrWhiteSpace(txtAftrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After Transfer ROB is required";
        }

        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "Before Transfer ROB is required";
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
    
    protected void txtBfrTrnsROBFrom_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
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
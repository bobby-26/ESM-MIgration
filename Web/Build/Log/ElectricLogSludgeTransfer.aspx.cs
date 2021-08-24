using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogSludgeTransfer : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "12.2";
    string ItemName = "Sludge";
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
        confirm.Attributes.Add("style", "display:none;");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTransferFrom.Enabled = false;
            ddlTransferTo.Enabled = false;
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
            ddlToPopulate();
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
        if (ddlTransferFrom.SelectedItem == null)
        {
            return;
        }
        ReportCode = "C";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = "12.2";
        txtCode.Text = ReportCode;

        Decimal fromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        Decimal fromTankAfter = Convert.ToDecimal(txtAfrTrnsROBFrom.Text == "" ? "0" : txtAfrTrnsROBFrom.Text);
        Decimal toTankBefore = Convert.ToDecimal(txtBfrTrnsROBTo.Text == "" ? "0" : txtBfrTrnsROBTo.Text);
        Decimal toTankAfter = Convert.ToDecimal(txtAfrTrnsROBTo.Text == "" ? "0" : txtAfrTrnsROBTo.Text);
        Decimal transcatQtyFrom = Math.Abs(Convert.ToDecimal(fromTankAfter) - Convert.ToDecimal(fromTankBefore));
        Decimal transcatQtyTo = Math.Abs(Convert.ToDecimal(toTankAfter) - Convert.ToDecimal(toTankBefore));

        lblRecord.Text = string.Format("<b> {0}</b>   m3 Sludge Transfered from  <b>{1}</b>, <br/>  <b> {2}</b> m3 retained", transcatQtyFrom, ddlTransferFrom.SelectedItem.Text, fromTankAfter);
        lbltorecord.Text = string.Format("to  <b>{0}</b> retained in  tank <br/>  <b> {1}</b> m3", ddlTransferTo.SelectedItem.Text, toTankAfter);
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString());
                txtDate.Text = Convert.ToDateTime(dr["FLDDATE"].ToString()).ToShortDateString();
                txtBfrTrnsROBFrom.Text = dr["FLDFROMROB"].ToString();
                txtBfrTrnsROBTo.Text = dr["FLDTOROB"].ToString();
                txtAfrTrnsROBFrom.Text = dr["FLDAFTTXNFROM"].ToString();
                txtAfrTrnsROBTo.Text = dr["FLDAFTTXNTO"].ToString();

                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
                }
                RadComboBoxItem toItem = ddlTransferTo.Items.FindItem(x => x.Text == dr["FLDTOLOCATION"].ToString());
                if (toItem != null)
                {
                    toItem.Selected = true;
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
                            , lbltorecord.Text
                            , txid
                            , 2
                            , null);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                            , txid
                            , 3
                            , false);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName("", "", null, true)
                            , txid
                            , 4
                            , true);
    }

    private void TranscationUpdate()
    {
        PhoenixElog.UpdateTransaction(txid
                                                        , usercode
                                                        , new Guid(ddlTransferTo.SelectedItem.Value)
                                                        , txtBfrTrnsROBFrom.Text
                                                        , txtBfrTrnsROBTo.Text
                                                        , (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAfrTrnsROBFrom.Text)).ToString()
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
                                                        , txtAfrTrnsROBFrom.Text
                                                        , txtAfrTrnsROBTo.Text
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

                if (isValidInput() == false && isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                TransactionValidation();
                saveTxin();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "EXCEED85")
                RadWindowManager1.RadConfirm("Warning. Tank Reached its 85 percentage capacity", "confirm", 320, 150, null, "Exceed");
            else
            { 
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
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

    private void saveTxin()
    {
        Guid TranscationId = Guid.Empty;
        Guid LogBookDetailId = Guid.Empty;
        string logName = "Sludge Transfer";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId = TranscationInsert(ref TranscationId);
            MissedOperationInsert(logId, logName);
            MissedOperationalEntryTemplateUpdate(logName, logId, TranscationId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationalEntryTemplateUpdate(logName, txid, null);
        }
        else if (txid != Guid.Empty & isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {
            DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid logId = TranscationInsert(ref TranscationId);

            PhoenixElog.LogBookEntryInsert(usercode
                                      , entrydate
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
                                      , entrydate
                                      , null
                                      , lbltorecord.Text
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
                                      , entrydate
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , null
                                      , TranscationId
                                      , 3
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
                                      , entrydate
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(""), General.GetNullableString(""), General.GetNullableDateTime(""), true)
                                      , null
                                      , TranscationId
                                      , 4
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

    private void TransactionValidation()
    {
        PhoenixElog.InsertTransactionValidation(usercode
                                        , new Guid(ddlTransferTo.SelectedItem.Value)
                                        , txtBfrTrnsROBFrom.Text
                                        , txtBfrTrnsROBTo.Text
                                        , (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAfrTrnsROBFrom.Text)).ToString()
                                        , vesselId
                                        , General.GetNullableInteger(lblinchId.Text)
                                        , General.GetNullableInteger(null)
                                        , txtAfrTrnsROBFrom.Text
                                        , txtAfrTrnsROBTo.Text
                                        );
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        saveTxin();
    }

    private void MissedOperationalEntryTemplateUpdate(string logName, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        string txQty = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAfrTrnsROBFrom.Text)).ToString();
        // store transaction 
        nvc.Add("FromTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("FromTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("ToTankId", ddlTransferTo.SelectedItem.Value);
        nvc.Add("ToTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("FromRob", txtBfrTrnsROBFrom.Text);
        nvc.Add("ToRob", txtBfrTrnsROBTo.Text);
        nvc.Add("TransferQty", txQty);
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lbltorecord.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemName1", ItemName);
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
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        nvc.Add("isTransaction", "true");
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private Guid TranscationInsert(ref Guid TranscationId)
    {
        Guid logId = Guid.NewGuid();

        PhoenixElog.InsertTransactionNew(usercode
                                        , ReportCode
                                        , new Guid(ddlTransferFrom.SelectedItem.Value)
                                        , ddlTransferFrom.SelectedItem.Text
                                        , new Guid(ddlTransferTo.SelectedItem.Value)
                                        , ddlTransferTo.SelectedItem.Text
                                        , txtCode.Text
                                        , ItemNo
                                        , ItemName
                                        , txtBfrTrnsROBFrom.Text
                                        , txtBfrTrnsROBTo.Text
                                        , (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAfrTrnsROBFrom.Text)).ToString()
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
                                        , null
                                        , logId
                                        , txtAfrTrnsROBFrom.Text
                                        , txtAfrTrnsROBTo.Text
                                        , "Sludge Transfer"
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
        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "From Tank Before Transfer ROB is required";
        }

        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBTo.Text))
        {
            ucError.ErrorMessage = "To Tank Before Transfer ROB is required";
        }


        if (string.IsNullOrWhiteSpace(txtAfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "From Tank After Transfer ROB is required";
        }

        if (ddlTransferFrom.SelectedItem.Text == ddlTransferTo.SelectedItem.Text)
        {
            ucError.ErrorMessage = "Transfer From and To Tank cannot be the same";
        }
        else if (string.IsNullOrWhiteSpace(txtAfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "To Tank After Transfer ROB is required";
        }
        else if (Convert.ToDecimal(txtAfrTrnsROBFrom.Text) > Convert.ToDecimal(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After ROB cannot be greater than Before ROB";
        }

        if (string.IsNullOrWhiteSpace(txtAfrTrnsROBTo.Text))
        {
            ucError.ErrorMessage = "To Tank After Transfer ROB is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    private bool isValidateSignature()
    {
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
    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void to_selectedindexchaged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txt_selectedindexchaged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    private void CalculateROB()
    {
        if (txtBfrTrnsROBFrom.Text != "" && txtAfrTrnsROBFrom.Text != "" && txtBfrTrnsROBTo.Text != "")
        {
            SetDefaultData();
        }
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
    private void ddlToPopulate()
    {
        ddlTransferTo.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "TO", vesselId, usercode);
        ddlTransferTo.DataBind();
        if (ddlTransferTo.Items.Count > 0)
        {
            ddlTransferTo.Items[0].Selected = true;
        }
    }

    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        if (isMissedOperation || isMissedOperationEdit)
        {
            saveTxin();
        }
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log"; ;
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}&isMissedOperation={1}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, isMissedOperation);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }
}
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Log_ElectricLogDisposingSludgeCleaning : PhoenixBasePage
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
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }
    
    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null)
        {
            return;
        }
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtItemNo1.Text = "12.3";
        txtCode.Text = ReportCode;
        txtCode1.Text = "C";
        lblRecord.Text = string.Format("<b> {0}</b>   m3 from cleaning of   <b>{1}</b>", txtQuantity.Text, ddlTransferFrom.SelectedItem.Text);
        lblRecord1.Text = string.Format("Burned in Incinerator for  <b>{0}</b> hours", txtIncinerationHours.Text);
    }

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();
            ds = PhoenixElog.GetOperationRecord(
                                      txid
                                    , vesselId
                                    , ReportCode
                                    , usercode
                                    );
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Guid disposingId = Guid.Parse(dr["FLDTRANSCATIONDETAILID"].ToString());
                lblDisposingId.Text = disposingId.ToString();
                DataSet disposingSludge = PhoenixElog.DisposingSludgeSearch(usercode, vesselId, disposingId);
                DataRow disposingSludgeRow = disposingSludge.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(disposingSludgeRow["FLDDATE"].ToString());
                txtIncinerationHours.Text = disposingSludgeRow["FLDINCINERATIONTIME"].ToString();
                txtQuantity.Text = dr["FLDTXQTY"].ToString();

                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == disposingSludgeRow["FLDLOCATIONNAME"].ToString());
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

    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
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
        string logName = "Disposing sludge from cleaning of oil tanks";

        if (isMissedOperation && isMissedOperationEdit == false)
        {

            Guid TranscationId, logId;
            TransactionInsert(logName, out TranscationId, out logId);
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


            DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid TranscationId, logId;
            TransactionInsert(logName, out TranscationId, out logId);

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
                                      , PhoenixElog.GetSignatureName("", "", null, true)
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

    private void MissedOperationTemplateUpdate(string logName, Guid? TranscationId, Guid logId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ReportCode2", txtCode1.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo1", txtItemNo1.Text);
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
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : TranscationId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TransactionInsert(string logName, out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        Guid TranscationDetailID = Guid.Empty;
        logId = Guid.NewGuid();
        Guid DisposalId = PhoenixElog.GetProcessId(usercode, "DSCOT");

        PhoenixElog.DisposingSludgeInsert(usercode
                        , vesselId
                        , txtOperationDate.SelectedDate.Value
                        , Convert.ToInt32(txtIncinerationHours.Text)
                        , Guid.Parse(ddlTransferFrom.SelectedItem.Value)
                        , ddlTransferFrom.SelectedItem.Text
                        , Convert.ToDecimal(txtQuantity.Text)
                        , ref TranscationDetailID
            );

        PhoenixElog.InsertTransactionNew(usercode
                                        , ReportCode
                                        , new Guid(ddlTransferFrom.SelectedItem.Value)
                                        , ddlTransferFrom.SelectedItem.Text
                                        , DisposalId
                                        , "Cleaning Sludge"
                                        , txtCode.Text
                                        , ItemNo
                                        , ItemName
                                        , "0"
                                        , "0"
                                        , txtQuantity.Text
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
                                        , TranscationDetailID
                                        , logId
                                        , null
                                        , null
                                        , logName
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

    private void TransactionUpdate()
    {
        PhoenixElog.DisposingSludgeUpdate(usercode
                                            , Guid.Parse(lblDisposingId.Text)
                                            , vesselId
                                            , txtOperationDate.SelectedDate.Value
                                            , Convert.ToInt32(txtIncinerationHours.Text)
                                            , Guid.Parse(ddlTransferFrom.SelectedItem.Value)
                                            , ddlTransferFrom.SelectedItem.Text
                                            , Convert.ToDecimal(txtQuantity.Text)
                                );

        PhoenixElog.UpdateTransaction(txid
                                            , usercode
                                            , new Guid(ddlTransferFrom.SelectedItem.Value)
                                            , "0"
                                            , "0"
                                            , txtQuantity.Text
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

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(ddlTransferFrom.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Sludge removed from is required";
        }

        if (string.IsNullOrWhiteSpace(txtIncinerationHours.Text))
        {
            ucError.ErrorMessage = "Incineration Hours is required";
        }

        if (string.IsNullOrWhiteSpace(txtQuantity.Text))
        {
            ucError.ErrorMessage = "Quantity of Sludge is required";
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


    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
        CalculateROB();
    }
    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
        CalculateROB();
    }
    private void CalculateROB()
    {
        //if (txtBfrTrnsROBFrom.Text != "" && txtAfrTrnsROBFrom.Text != "" && txtBfrTrnsROBTo.Text != "")
        //{
        //    txtAfrTrnsROBTo.Text = ((Convert.ToInt32(txtBfrTrnsROBFrom.Text) - Convert.ToInt32(txtAfrTrnsROBFrom.Text)) + Convert.ToInt32(txtBfrTrnsROBTo.Text)).ToString();
        //    SetDefaultData();
        //}
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
    
    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
        CalculateROB();
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

        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }
}
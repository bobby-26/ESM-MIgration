using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogOilSludgeToIOPP : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "11.1";
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
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            ddlFromPopulate();
            ddlToPopulate();
            SetDefaultData();
            GetLastTranscationDate();
            OnDutyEngineerSign();
            BindData();
        }
    }

    private void GetTankCapacity()
    {
        Guid tankId = Guid.Parse(ddlTransferTo.SelectedItem.Value);
        DataSet ds = PhoenixElog.GetTankCapacity(usercode, vesselId, tankId); 
        DataRow row = ds.Tables[0].Rows[0];
        lbltorecord.Text = string.Format("Capacity <b>{0}</b> m3", row["FLDCAPACITY"].ToString());
    }

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();

            ds = PhoenixElog.GetOperationRecord( txid
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , ReportCode
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());
                txtBfrTrnsROBTo.Text = dr["FLDTOROB"].ToString();
                txtTransferQuantity.Text = dr["FLDTXQTY"].ToString(); 
                txtAftrTrnsROBTo.Text = dr["FLDAFTTXNTO"].ToString(); ;

                ddlTransferFrom.Text = dr["FLDFROMLOCATION"].ToString();
                //RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                //if (fromItem != null)
                //{
                //    fromItem.Selected = true;
                //}
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


    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixElog.GetLogLastTranscationDate(vesselId, usercode);
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
                if (!isValidInput() || IsValidSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                TransactionValidation();
                SaveTxin();
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

    private void TransactionValidation()
    {
        PhoenixElog.InsertTransactionValidation(usercode
                                            , new Guid(ddlTransferTo.SelectedItem.Value)
                                            , "0"
                                            , txtBfrTrnsROBTo.Text
                                            , Convert.ToDecimal(txtTransferQuantity.Text).ToString()
                                            , vesselId
                                            , General.GetNullableInteger(lblinchId.Text)
                                            , General.GetNullableInteger(null)
                                            , "0"
                                            , txtAftrTrnsROBTo.Text
                                            );
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
        string logName = "Oil Sludge to IOPP";

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId, logId;
            TranscationInsert(out TranscationId, out logId);
            MissedOperationTemplateUpdate(logName, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationTemplateUpdate(logName, txid, null);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {
            DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid TranscationId, logId;
            TranscationInsert(out TranscationId, out logId);

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
                                      , txtcode1.Text
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
                                     , txtcode2.Text
                                     , lblAfterROB.Text
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
                                    , entrydate
                                    , txtcode3.Text
                                    , lblFromRob.Text
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
                                          , entrydate
                                          , null
                                          , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                          , null
                                          , TranscationId
                                          , 5
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
                                      , 6
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

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (!isValidInput() || IsValidSignature() == false)
            {
                ucError.Visible = true;
                return;
            }

            SaveTxin();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    

    private void MissedOperationTemplateUpdate(string logName, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        string txQty = txtTransferQuantity.Text;
        Guid ManualOilSLudgeId = PhoenixElog.GetProcessId(usercode, "INC");
        // store transaction 
        nvc.Add("FromTankId", ManualOilSLudgeId.ToString());
        nvc.Add("FromTank", ddlTransferFrom.Text);
        nvc.Add("ToTankId", ddlTransferTo.SelectedItem.Value);
        nvc.Add("ToTank", ddlTransferTo.SelectedItem.Text);
        nvc.Add("ToRob", txtBfrTrnsROBTo.Text);
        nvc.Add("TransferQty", txQty);
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", ItemNo);
        nvc.Add("ItemName1", ItemName);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("Record2", lbltorecord.Text);
        nvc.Add("ItemNo2", txtcode1.Text);

        nvc.Add("Record3", lblAfterROB.Text);
        nvc.Add("ItemNo3", txtcode2.Text);

        nvc.Add("Record4", lblFromRob.Text);
        nvc.Add("ItemNo4", txtcode3.Text);

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        // add meta data for the log
        nvc.Add("logBookEntry", "4");
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        nvc.Add("logBookName", logName);

        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TranscationInsert(out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        logId = Guid.NewGuid();
        Guid ManualOilSLudgeId = PhoenixElog.GetProcessId(usercode, "INC");

        PhoenixElog.InsertTransactionNew(usercode
                                        , ReportCode
                                        , ManualOilSLudgeId
                                        , ddlTransferFrom.Text
                                        , new Guid(ddlTransferTo.SelectedItem.Value)
                                        , ddlTransferTo.SelectedItem.Text
                                        , txtCode.Text
                                        , ItemNo
                                        , ItemName
                                        , "0"
                                        , txtBfrTrnsROBTo.Text
                                        , Convert.ToDecimal(txtTransferQuantity.Text).ToString()
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
                                        , "0"
                                        , txtAftrTrnsROBTo.Text
                                        , "Oil Sludge to IOPP"
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
                        , lbltorecord.Text
                        , txid
                        , 2
                        , null);


        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblAfterROB.Text
                        , txid
                        , 3
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblFromRob.Text
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

    private void TranscationUpdate()
    {
        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , new Guid(ddlTransferTo.SelectedItem.Value)
                                                , "0"
                                                , txtBfrTrnsROBTo.Text
                                                , Convert.ToDecimal(txtTransferQuantity.Text).ToString()
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
                                                 , "0"
                                                 , txtAftrTrnsROBTo.Text
                                            );
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }
        if (string.IsNullOrWhiteSpace(txtTransferQuantity.Text))
        {
            ucError.ErrorMessage = "Transfer Quantity is required";
        }
        if (string.IsNullOrWhiteSpace(ddlTransferFrom.Text))
        {
            ucError.ErrorMessage = "Sludge Transferred From is required";
        }
        if (string.IsNullOrWhiteSpace(ddlTransferTo.Text))
        {
            ucError.ErrorMessage = "Sludge Transferred to tank is required";
        }
        if (ddlTransferFrom.Text == ddlTransferTo.Text)
        {
            ucError.ErrorMessage = "Transfer From and To Tank cannot be the same";
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

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
        CalculateROB();
    }
    private void SetDefaultData()
    {
        if (ddlTransferTo.SelectedItem == null) return;

        GetTankCapacity();       
        ReportCode = "C";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        decimal toTankBefore = Convert.ToDecimal(txtBfrTrnsROBTo.Text == "" ? "0" : txtBfrTrnsROBTo.Text);
        decimal toTankAfter = Convert.ToDecimal(txtAftrTrnsROBTo.Text == "" ? "0" : txtAftrTrnsROBTo.Text);
        decimal toTankqty = Convert.ToDecimal(txtTransferQuantity.Text == "" ? "0" : txtTransferQuantity.Text);
        lblRecord.Text = string.Format("Name of the Tank: <b> {0}</b> ", ddlTransferTo.SelectedItem.Text);
        txtcode1.Text = "11.2";
        //lbltorecord.Text = "<b>1</b> m3";
        txtcode2.Text = "11.3";
        lblAfterROB.Text = string.Format("Total retained on board <b>{0}</b> m3", toTankAfter);
        txtcode3.Text = "11.4";
        lblFromRob.Text = string.Format("<b>{0}</b> m3 Collected from  <b>{1}</b>", toTankqty, ddlTransferFrom.Text);
    }
    private void CalculateROB()
    {
        if (txtTransferQuantity.Text != "" && txtBfrTrnsROBTo.Text != "" )
        {
            //txtAftrTrnsROBTo.Text = (Convert.ToDecimal(txtTransferQuantity.Text) + Convert.ToDecimal(txtBfrTrnsROBTo.Text)).ToString();
            SetDefaultData();
        }
    }
    private void ddlFromPopulate()
    {
        //ddlTransferFrom.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        //ddlTransferFrom.DataBind();
        //foreach (RadComboBoxItem item in ddlTransferFrom.Items)
        //{
        //    item.Selected = true;
        //    break;
        //}
    }
    private void ddlToPopulate()
    {
        ddlTransferTo.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "TO", vesselId, usercode);
        ddlTransferTo.DataBind();
        foreach (RadComboBoxItem item in ddlTransferTo.Items)
        {
            item.Selected = true;
            break;
        }
    }

    private void ddlToLoadData()
    {
        ddlTransferTo.Items.Clear();
        //ddlTransferTo.DataSource = PhoenixElog.ddlTrasnferToLocation(ReportCode);
        ddlTransferTo.DataSource = PhoenixElog.ElogLocationDropDown(null, null, "FROM", vesselId, usercode);
        ddlTransferTo.DataTextField = "FLDNAME";
        ddlTransferTo.DataValueField = "FLDLOCATIONID";
        ddlTransferTo.DataBind();
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void txtTransferQuantity_TextChangedEvent(object o, EventArgs e)
    {
        SetDefaultData();
        CalculateROB();
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
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogIncineration : PhoenixBasePage
{
    string ReportCode = string.Empty;
    string ItemNo   = "12.3";
    string ItemName = "Sludge";
    int usercode;
    int vesselId;
    Guid txid = Guid.Empty;
    Guid IncinerationTranscationId = Guid.Empty;
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
                DataSet incinerationDetail = new DataSet();

            

            ds = PhoenixElog.GetOperationRecord(txid
                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                    , ReportCode
                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtDate.Text = Convert.ToDateTime(dr["FLDDATE"].ToString()).ToShortDateString();
                    txtBfrTrnsROBFrom.Text = dr["FLDFROMROB"].ToString();
                    txtAfrTrnsROBFrom.Text = dr["FLDAFTTXNFROM"].ToString();
                    lblIncinerationDetailID.Text = dr["FLDTRANSCATIONDETAILID"].ToString();
                    IncinerationTranscationId = Guid.Parse(lblIncinerationDetailID.Text);

                    RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                    if (fromItem != null)
                    {
                        fromItem.Selected = true;
                    }

                incinerationDetail = PhoenixElog.IncinerationTransactionSearch(usercode, IncinerationTranscationId);
                DataRow incinerationDetailRow = incinerationDetail.Tables[0].Rows[0];
                txtLtrsHrs.Text = incinerationDetailRow["FLDINCINERATIONLITRESHOURS"].ToString();
                txtTimeInHrs.Text = incinerationDetailRow["FLDINCINERATIONHOURS"].ToString();
                SetDefaultData();

                }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }

    private void LogBookUpdate()
    {
        if (string.IsNullOrWhiteSpace(lblIncinerationDetailID.Text)) throw new ArgumentException("Transaction Detail Id is  not Supplied");


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
        Guid IncinerationId = PhoenixElog.GetProcessId(usercode, "INC");

        IncinerationTranscationId = Guid.Parse(lblIncinerationDetailID.Text);

        PhoenixElog.IncinerationTransactionUpdate(usercode, IncinerationTranscationId, Convert.ToInt32(txtTimeInHrs.Text), Convert.ToInt32(txtLtrsHrs.Text));

        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , IncinerationId
                                                , txtBfrTrnsROBFrom.Text
                                                , txtAfrTrnsROBFrom.Text
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
                                            );
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
        string logName = "Incineration";

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

    private void TranscationInsert(out Guid TranscationId, out Guid logId)
    {
        TranscationId = Guid.Empty;
        Guid IncinerationTranscationId = Guid.Empty;
        logId = Guid.NewGuid();
        Guid IncinerationId = PhoenixElog.GetProcessId(usercode, "INC");

        PhoenixElog.IncinerationTransactionInsert(usercode, ref IncinerationTranscationId, Convert.ToInt32(txtTimeInHrs.Text), Convert.ToInt32(txtLtrsHrs.Text));

        PhoenixElog.InsertTransactionNew(usercode
                                        , ReportCode
                                        , new Guid(ddlTransferFrom.SelectedItem.Value)
                                        , ddlTransferFrom.SelectedItem.Text
                                        , IncinerationId
                                        , "Incineration"
                                        , txtCode.Text
                                        , ItemNo
                                        , ItemName
                                        , txtBfrTrnsROBFrom.Text
                                        , txtAfrTrnsROBFrom.Text
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
                                        , IncinerationTranscationId
                                        , logId
                                        , txtAfrTrnsROBFrom.Text
                                        , null
                                        , "Incineration"
                                        , isMissedOperation
                                    );
    }

    private void MissedOperationTemplateUpdate(string logName,Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        string txQty = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAfrTrnsROBFrom.Text)).ToString();
        Guid IncinerationId = PhoenixElog.GetProcessId(usercode, "INC");
        // store transaction 
        nvc.Add("FromTankId", IncinerationId.ToString());
        nvc.Add("FromTank", logName);
        nvc.Add("ToTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("ToTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("FromRob", txtBfrTrnsROBFrom.Text);
        nvc.Add("ToRob", txtAfrTrnsROBFrom.Text);
        nvc.Add("TransferQty", txQty);
        nvc.Add("incinerationTime", txtTimeInHrs.Text);
        nvc.Add("incinerationHrs", txtLtrsHrs.Text);
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lbltorecord.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", ItemNo);
        nvc.Add("ItemName1", ItemName);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));

        nvc.Add("chiefEngId", lblinchId.Text);
        nvc.Add("chiefEngRank", lblincRank.Text);
        nvc.Add("chiefEngName", lblinchName.Text);
        nvc.Add("chiefEngSignDate", lblincSignDate.Text);
        nvc.Add("CheifEngineerSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        // add meta data for the log
        nvc.Add("logBookEntry", "2");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTransaction", "true");
        nvc.Add("txId", txid == null ? "" : txId.ToString());

        Filter.MissedOperationalEntryCriteria = nvc;
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(ddlTransferFrom.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Incineration Tank is required";
        }

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }
       
        if (string.IsNullOrWhiteSpace(txtTimeInHrs.Text))
        {
            ucError.ErrorMessage = "Incineration Time is required";
        }
        if (string.IsNullOrWhiteSpace(txtLtrsHrs.Text))
        {
            ucError.ErrorMessage = "Incineration Ltrs/Hour is required";
        }

        if (string.IsNullOrWhiteSpace(txtAfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After Transfer ROB is required";
        }
        else if (Convert.ToDecimal(txtAfrTrnsROBFrom.Text) > Convert.ToDecimal(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After ROB cannot be greater than Before ROB";
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

    protected void bfr_selectedindexchange(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtAfrTrnsROBFrom_TextChangedEvent(object o, EventArgs e)
    {
        SetDefaultData();
        CalculateROB();
    }
    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = "C";
        decimal fromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        decimal fromTankAfter = Convert.ToDecimal(txtAfrTrnsROBFrom.Text == "" ? "0" : txtAfrTrnsROBFrom.Text);
        decimal transferQty = Math.Abs(fromTankAfter - fromTankBefore);
        lblRecord.Text = string.Format("<b> {0}</b>   m3 sludge from  <b>{1}</b>, <br/> <b>{2}</b> m3 Retained", transferQty, ddlTransferFrom.SelectedItem.Text, fromTankAfter);
        lbltorecord.Text = string.Format("Burned in Incinerator for  {0}  hrs", txtTimeInHrs.Text);
    }
    private void CalculateROB()
    {
        SetDefaultData();
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

    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txt_TextChangedEvent(object sender, EventArgs e)
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
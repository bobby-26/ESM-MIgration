using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogEvaporationFromSludge : PhoenixBasePage
{
    string ItemNo = "12.4";
    int usercode = 0;
    int vesselId = 0;
    string ItemName = "Sludge";
    string ReportCode = string.Empty;
    Guid txid = Guid.Empty;
    bool isDutyEngineer = false;
    bool isCheifEngineer = false;
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
            SetDefaultData();
            GetLastTranscationDate();
            BindData();
        }
    }

    private void BindData()
    {
        if (txid == Guid.Empty) return;

        DataSet ds = new DataSet();

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
            txtOperationDate.SelectedDate = Convert.ToDateTime(dr["FLDDATE"]);
            txtBfrTrnsROBFrom.Text = dr["FLDFROMROB"].ToString();
            txtAfrTrnsROBFrom.Text = dr["FLDAFTTXNFROM"].ToString();

            RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
            if (fromItem != null)
            {
                fromItem.Selected = true;
            }

            SetDefaultData();
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

        PhoenixElog.LogBookEntryUpdate(usercode
                        , lblRecord.Text
                        , txid
                        , 1
                        , null);

        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                        , txid
                        , 2
                        , null);


        PhoenixElog.LogBookEntryUpdate(usercode
                        , PhoenixElog.GetSignatureName("", "", null, true)
                        , txid
                        , 3
                        , true);

    }

    private void TranscationUpdate()
    {
        Guid EvaporationId = PhoenixElog.GetProcessId(usercode, "EVP");

        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , EvaporationId
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

    //private Guid GetProcessId()
    //{
    //    Guid evaportationProcessId;
    //    DataSet ds = PhoenixElog.GeLocationProcessId(usercode, "EVP");
    //    DataRow row = ds.Tables[0].Rows[0];
    //    evaportationProcessId = Guid.Parse(row["FLDPROCESSID"].ToString());
    //    return evaportationProcessId;
    //}

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string rank = PhoenixElog.GetRankName(usercode);
                isDutyEngineer = PhoenixElog.validDutyEngineer(rank);
                isCheifEngineer = PhoenixElog.validCheifEngineer(rank);

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
        string logName = "Evaporation From Sludge";
        Guid EvaporationId = PhoenixElog.GetProcessId(usercode, "EVP");


        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid TranscationId = Guid.Empty;
            Guid logId = TranscationInsert(EvaporationId, ref TranscationId);
            MissedOperationEntryTemplateUpdate(logName, EvaporationId, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationEntryTemplateUpdate(logName, EvaporationId, txid, null);
        }
        else if (txid != Guid.Empty && isMissedOperation == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {
            DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
            Guid TranscationId = Guid.Empty;
            Guid logId = TranscationInsert(EvaporationId, ref TranscationId);

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
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , null
                                      , TranscationId
                                      , 2
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
                                      , 3
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

    private void MissedOperationEntryTemplateUpdate(string logName, Guid EvaporationId, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        string txQty = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) - Convert.ToDecimal(txtAfrTrnsROBFrom.Text)).ToString();
        // store transaction 
        nvc.Add("FromTankId", EvaporationId.ToString());
        nvc.Add("FromTank", "Evaporation");
        nvc.Add("ToTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("ToTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("FromRob", txtBfrTrnsROBFrom.Text);
        nvc.Add("ToRob", txtAfrTrnsROBFrom.Text);
        nvc.Add("TransferQty", txQty);
        // add for logbook
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", ItemNo);
        nvc.Add("ItemName1", ItemName);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        // add meta data for the log
        nvc.Add("logBookEntry", "1");
        nvc.Add("logBookName", logName);
        nvc.Add("logId", logId.ToString());
        nvc.Add("isTranscation", "false");
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private Guid TranscationInsert(Guid EvaporationId, ref Guid TranscationId)
    {
        Guid logId = Guid.NewGuid();

        PhoenixElog.InsertTransactionNew(usercode
                                        , ReportCode
                                        , new Guid(ddlTransferFrom.SelectedItem.Value)
                                        , ddlTransferFrom.SelectedItem.Text
                                        , EvaporationId
                                        , "EVAPORATION"
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
                                        , null
                                        , logId
                                        , txtAfrTrnsROBFrom.Text
                                        , null
                                        , "Evaporation From Sludge"
                                        , isMissedOperation
                                    );
        return logId;
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        decimal AfrTrns = txtAfrTrnsROBFrom.Text == "" ? 0 : Convert.ToDecimal(txtAfrTrnsROBFrom.Text);
        decimal BfrTrns = txtBfrTrnsROBFrom.Text == "" ? 0 : Convert.ToDecimal(txtBfrTrnsROBFrom.Text);

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation    is required";
        }
        if (string.IsNullOrWhiteSpace(txtAfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After Transfer ROB is required";
        }
        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "Before Transfer ROB is required";
        }
        if (AfrTrns > BfrTrns)
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
    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = "C";
        string From = ddlTransferFrom.SelectedItem == null ? "" : ddlTransferFrom.SelectedItem.Text;
        Decimal fromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        Decimal fromTankAfter = Convert.ToDecimal(txtAfrTrnsROBFrom.Text == "" ? "0" : txtAfrTrnsROBFrom.Text);
        decimal transferQty = Math.Abs(fromTankAfter - fromTankBefore);
        lblRecord.Text = string.Format("<b> {0}</b>   m3 Water Evaporated from  <b>{1}</b>, <br/>  <b> {2}</b> m3 retained", transferQty, From, fromTankAfter);
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

    protected void txtAfrTrnsROBFrom_TextChangedEvent(object o, EventArgs e)
    {
        SetDefaultData();
        CalculateROB();
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
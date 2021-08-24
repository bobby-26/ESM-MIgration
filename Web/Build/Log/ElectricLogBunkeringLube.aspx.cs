using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogBunkeringLube : PhoenixBasePage
{
    int usercode    =   0;
    int vesselId    =   0;
    string itemNo   = "26.1";
    string itemName = string.Empty;
    string ReportCode = "H";
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
            txtStartDate.SelectedDate = DateTime.Now;
            txtStopDate.SelectedDate = DateTime.Now;
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            txtStartDate.MaxDate = DateTime.Now;
            txtStopDate.MaxDate = DateTime.Now;
            ddlFromPopulate();
            GetLastTranscationDate();
            setDefaultData();
            BindData();
        }
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
                lblBunkeringLubeId.Text = dr["FLDTRANSCATIONDETAILID"].ToString();


                DataSet bunkeringLubeDetail = PhoenixElog.BunkeringLubeSearch(usercode, vesselId, Guid.Parse(lblBunkeringLubeId.Text));
                DataRow bunkeringDetail = bunkeringLubeDetail.Tables[0].Rows[0];
                txtBfrTrnsROBFrom.Text = dr["FLDFROMROB"].ToString();
                txtTotalQty.Text = dr["FLDTOROB"].ToString();
                txtbfrBunkerQty.Text = dr["FLDTXQTY"].ToString();
                txtStartDate.SelectedDate = Convert.ToDateTime(bunkeringDetail["FLDSTARDATETTIME"]);
                txtStartTime.SelectedDate = Convert.ToDateTime(bunkeringDetail["FLDSTARDATETTIME"]);
                txtStopDate.SelectedDate = Convert.ToDateTime(bunkeringDetail["FLDSTOPDATETIME"]);
                txtStopTime.SelectedDate = Convert.ToDateTime(bunkeringDetail["FLDSTOPDATETIME"]);
                txtBunkerQty.Text = bunkeringDetail["FLDBUNKERQUA"].ToString();
                txtBunkerType.Text = bunkeringDetail["FLDBUNKERTYPE"].ToString();
                txtPort.Text = bunkeringDetail["FLDBUNKERPORTNAME"].ToString();

                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
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

    private void CalculateROB()
    {
        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBFrom.Text) == false && string.IsNullOrWhiteSpace(txtbfrBunkerQty.Text) == false)
        {
            txtTotalQty.Text = (Convert.ToDecimal(txtBfrTrnsROBFrom.Text) + Convert.ToDecimal(txtbfrBunkerQty.Text)).ToString();
        }
    }

    private void setDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = "H";
        txtItemNo.Text = "26.1";
        txtItemNo1.Text = "26.2";
        txtItemNo2.Text = "26.4";
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue ? txtStartTime.SelectedTime.Value : new TimeSpan();
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue ? txtStopTime.SelectedTime.Value : new TimeSpan();
        lblRecord.Text = string.Format("<b>{0}</b>", txtPort.Text);
        lblRecord1.Text = string.Format("Start: <b>{0}</b> <b>{1}</b>  Stop  <b>{2}</b>  <b>{3}</b>", PhoenixElog.GetElogDateFormat(txtStartDate.SelectedDate.Value).ToString(), startTime.ToString(), PhoenixElog.GetElogDateFormat(txtStopDate.SelectedDate.Value).ToString(), stopTime.ToString());
        lblRecord2.Text = string.Format("<b>{0}</b> MT  <b>{1}</b> bunkered in tanks:", txtBunkerQty.Text, txtBunkerType.Text);
        lblRecord3.Text = string.Format("<b>{0}</b> MT added to <b>{1}</b>   now containing  <b>{2}</b>  MT", txtbfrBunkerQty.Text, ddlTransferFrom.SelectedItem.Text, txtTotalQty.Text);
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
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
                            , new Guid(ddlTransferFrom.SelectedItem.Value)
                            , txtBfrTrnsROBFrom.Text
                            , txtTotalQty.Text
                            , txtbfrBunkerQty.Text
                            , vesselId
                            , General.GetNullableInteger(lblinchId.Text)
                            , General.GetNullableInteger(null)
                            , null
                            , null
                            );
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidInput() == false)
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
        string logName = "Bunkering Lube";
        DateTime StartDateTime = txtStartDate.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime StopDateTime = txtStopDate.SelectedDate.Value.Add(txtStopTime.SelectedTime.Value);
        Guid bunkeringLubeId = PhoenixElog.GetProcessId(usercode, "BUKLUB");
        Guid TranscationId = Guid.Empty;
        Guid BunkerId = Guid.Empty;

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId = TranscationInsert(logName, StartDateTime, StopDateTime, bunkeringLubeId, ref TranscationId, ref BunkerId);
            MissedOperationEntryTemplateUpdate(logName, StartDateTime, StopDateTime, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationEntryTemplateUpdate(logName, StartDateTime, StopDateTime, txid, null);
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
            Guid logId = TranscationInsert(logName, StartDateTime, StopDateTime, bunkeringLubeId, ref TranscationId, ref BunkerId);

            PhoenixElog.LogBookEntryInsert(usercode
                                      , logBookEntryDate
                                      , txtItemNo.Text
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
                                      , 6
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

    private void MissedOperationEntryTemplateUpdate(string logName, DateTime StartDateTime, DateTime StopDateTime, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblRecord1.Text);
        nvc.Add("Record3", lblRecord2.Text);
        nvc.Add("Record4", lblRecord3.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
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
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        nvc.Add("isAttachmentRequired", "true");
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private Guid TranscationInsert(string logName, DateTime StartDateTime, DateTime StopDateTime, Guid bunkeringLubeId, ref Guid TranscationId, ref Guid BunkerId)
    {
        Guid logId = Guid.NewGuid();

        PhoenixElog.BunkeringLubeInsert(usercode
                                , vesselId
                                , Convert.ToDecimal(itemNo)
                                , itemName
                                , ReportCode
                                , txtPort.Text
                                , StartDateTime
                                , StopDateTime
                                , Convert.ToDecimal(txtBunkerQty.Text)
                                , txtBunkerType.Text
                                , ref BunkerId
                            );

        PhoenixElog.InsertTransactionNew(usercode
                                      , ReportCode
                                      , bunkeringLubeId
                                      , "Bunkering"
                                      , new Guid(ddlTransferFrom.SelectedItem.Value)
                                      , ddlTransferFrom.SelectedItem.Text
                                      , txtCode.Text
                                      , itemNo
                                      , itemName
                                      , txtBfrTrnsROBFrom.Text
                                      , txtTotalQty.Text
                                      , txtbfrBunkerQty.Text
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
                                      , logName
                                      , isMissedOperation
                                  );
        return logId;
    }

    private void LogBookUpdate()
    {
        

        if (isMissedOperation == false)
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
    }

    private void TranscationUpdate()
    {

        DateTime StartDateTime = txtStartDate.SelectedDate.Value.Add(txtStartTime.SelectedTime.Value);
        DateTime StopDateTime = txtStopDate.SelectedDate.Value.Add(txtStopTime.SelectedTime.Value);

        PhoenixElog.BunkeringLubeUpdate(usercode
                                        , vesselId
                                        , txtPort.Text
                                        , StartDateTime
                                        , StopDateTime
                                        , Convert.ToDecimal(txtBunkerQty.Text)
                                        , txtBunkerType.Text
                                        , Guid.Parse(lblBunkeringLubeId.Text)
                    );


        PhoenixElog.UpdateTransaction(txid
                                             , usercode
                                             , new Guid(ddlTransferFrom.SelectedItem.Value)
                                             , txtBfrTrnsROBFrom.Text
                                              , txtTotalQty.Text
                                              , txtbfrBunkerQty.Text
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
   
    protected void txt_TextChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }
    protected void txtBunkerQty_TextChanged(object sender, EventArgs e)
    {
        CalculateROB();
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
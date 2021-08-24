using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogShoreSludgeDisposal : PhoenixBasePage
{
    string ReportCode = "C";
    string ItemNo = "12.1";
    int usercode = 0;
    int vesselId = 0;
    string ItemName = "Sludge";
    Guid txid;
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
            ddlFromPopulate();
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
    

    public void BindData()
    {
        try
        {
            if (txid == Guid.Empty) return;

            DataSet ds = new DataSet();
            DataSet shoreSludgeDetail = new DataSet();

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
                lblTranscationDetailId.Text = dr["FLDTRANSCATIONDETAILID"].ToString();


                RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == dr["FLDFROMLOCATION"].ToString());
                if (fromItem != null)
                {
                    fromItem.Selected = true;
                }

                shoreSludgeDetail = PhoenixElog.ShoreSludgeTransferSearch(usercode, Guid.Parse(lblTranscationDetailId.Text));
                DataRow shoreSludgeDetailRow = shoreSludgeDetail.Tables[0].Rows[0];
                txtShoreFacility.Text = shoreSludgeDetailRow["FLDFACILITYNAME"].ToString();
                txtPortName.Text = shoreSludgeDetailRow["FLDPORTNAME"].ToString();


                SetDefaultData();
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
        string logName = "Shore Sludge Disposal";


        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logId, TranscationId;
            TranscationInsert(out logId, out TranscationId);
            MissedOperationEntryTemplateUpdate(logName, logId, TranscationId);
            MissedOperationInsert(logId, logName);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate();
            MissedOperationEntryTemplateUpdate(logName, txid, null);
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
            Guid logId, TranscationId;
            TranscationInsert(out logId, out TranscationId);

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
                                      , true
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
                                      , true
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
                                      , true
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
                                      , true
                                      , entryNo
                                      , logId
                                );
        }
    }

    private void MissedOperationEntryTemplateUpdate(string logName, Guid logId, Guid? txId)
    {
        NameValueCollection nvc = new NameValueCollection();
        Guid shoreId = PhoenixElog.GetProcessId(usercode, "SHS");

        nvc.Add("FromTankId", ddlTransferFrom.SelectedItem.Value);
        nvc.Add("FromTank", ddlTransferFrom.SelectedItem.Text);
        nvc.Add("ToTankId", shoreId.ToString());
        nvc.Add("ToTank", "");
        nvc.Add("FromRob", txtBfrTrnsROBFrom.Text);
        nvc.Add("ToRob", txtAftrTrnsROBFrom.Text);
        // add for logbook
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
        nvc.Add("isTransaction", "true");
        nvc.Add("isAttachmentRequired", "true");
        nvc.Add("txId", txid == null ? "" : txId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TranscationInsert(out Guid logId, out Guid TranscationId)
    {
        logId = Guid.NewGuid();
        TranscationId = Guid.Empty;
        Guid DetailTranscationId = Guid.Empty;
        Guid shoreId = PhoenixElog.GetProcessId(usercode, "SHS");


        PhoenixElog.ShoreSludgeTransferInsert(usercode, ref DetailTranscationId, txtShoreFacility.Text, txtPortName.Text);

        PhoenixElog.InsertTransactionNew(usercode
                                        , ReportCode
                                        , new Guid(ddlTransferFrom.SelectedItem.Value)
                                        , ddlTransferFrom.SelectedItem.Text
                                        , shoreId
                                        , "SHORE"
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
                                        , DetailTranscationId
                                        , logId
                                        , txtAftrTrnsROBFrom.Text
                                        , null
                                        , "Shore Sludge Disposal"
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
        Guid shoreId = PhoenixElog.GetProcessId(usercode, "SHS");

        PhoenixElog.ShoreSludgeTransferUpdate(usercode, Guid.Parse(lblTranscationDetailId.Text), txtShoreFacility.Text, txtPortName.Text);

        PhoenixElog.UpdateTransaction(txid
                                                , usercode
                                                , shoreId
                                                , txtBfrTrnsROBFrom.Text
                                                , txtAftrTrnsROBFrom.Text
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
                                            );
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFromROB();
        SetDefaultData();
    }
    protected void bfr_onselctedindexchanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        decimal FromTankBefore = Convert.ToDecimal(txtBfrTrnsROBFrom.Text == "" ? "0" : txtBfrTrnsROBFrom.Text);
        decimal FromTankAfter = Convert.ToDecimal(txtAftrTrnsROBFrom.Text == "" ? "0" : txtAftrTrnsROBFrom.Text);
        decimal transferQty = Math.Abs(FromTankAfter - FromTankBefore);
        lblRecord.Text = string.Format("<b>{0}</b> m3 sludge from <b>{1}</b> <br/> <b>{2}</b>m3 Retained", transferQty, ddlTransferFrom.SelectedItem.Text, FromTankAfter);
        lbltorecord.Text = string.Format("To <b>{0}</b> during Port Stay  <b>{1}</b>", txtShoreFacility.Text, txtPortName.Text);
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
            ucError.ErrorMessage = "Discharge from tank is required";
        }
        if (string.IsNullOrWhiteSpace(txtAftrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After Discharge ROB is required";
        }
        else if (Convert.ToDecimal(txtAftrTrnsROBFrom.Text) > Convert.ToDecimal(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After ROB cannot be greater than Before ROB";
        }

        if (string.IsNullOrWhiteSpace(txtShoreFacility.Text))
        {
            ucError.ErrorMessage = "Shore Facility is required";
        }
        if (string.IsNullOrWhiteSpace(txtPortName.Text))
        {
            ucError.ErrorMessage = "Port name is required";
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

    private string GetRankName(int usercode)
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["FLDRANKCODE"].ToString();
        }
        return "";
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
    public void ddlFromROB()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        DataSet ds1 = PhoenixElog.TankCurrentROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                               , new Guid(ddlTransferFrom.SelectedItem.Value)
                                                );

        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = ds1.Tables[0].Rows[0];

            if (dr1["FLDROB"].ToString() == null || dr1["FLDROB"].ToString() == "")
            {

                DataSet ds = PhoenixElog.VesselROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , ddlTransferFrom.SelectedItem.Value
                                                  );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtBfrTrnsROBFrom.Text = dr["FLDROB"].ToString();
                }
            }
            else
            {
                txtBfrTrnsROBFrom.Text = dr1["FLDROB"].ToString();
            }
        }
        else
        {
            txtBfrTrnsROBFrom.Text = "";
        }
        DataSet ds2 = PhoenixElog.VesselROB(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , ddlTransferFrom.SelectedItem.Value
                                             );
        if (ds2.Tables[1].Rows.Count > 0)
        {
            DataRow dr2 = ds2.Tables[1].Rows[0];
            ViewState["ItemNObyTank"] = dr2["FLDITEMNO"].ToString();
            ViewState["ItemNamebyTank"] = dr2["FLDITEMNAME"].ToString();
        }
    }
    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtAftrTrnsROBFrom_TextChanged(object sender, EventArgs e)
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
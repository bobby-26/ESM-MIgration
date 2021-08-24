using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2AdditionalProcedure : PhoenixBasePage
{
    string ReportCode;
    int usercode = 0;
    int vesselId = 0;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "AdditionalOperational");

        ViewState["TXID"] = "";

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            ViewState["TXID"] = Guid.Parse(Request.QueryString["TxnId"]);
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
            ViewState["isMasterLoggedIn"] = false;            
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
            CheckMasterLoggedIn();
        }
    }

    private void CheckMasterLoggedIn()
    {
        DataSet userdetail = PhoenixElog.GetSeaFarerRankName(usercode);
        if (userdetail != null && userdetail.Tables.Count > 0 && userdetail.Tables[0].Rows.Count > 0)
        {
            DataRow userdetailRow = userdetail.Tables[0].Rows[0];
            if (userdetailRow["FLDRANKCODE"].ToString() == "MST")
            {
                ViewState["isMasterLoggedIn"] = true;
            }
        }
    }


    private void BindData()
    {
        if (ViewState["TXID"].ToString() != "")
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, new Guid(ViewState["TXID"].ToString()));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 119)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 120)
                    {
                        txtremark.Text = row["FLDVALUE"].ToString();
                    }
                   
                }
                SetDefaultData();
            }
        
        }
    }
         
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }
    
    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }
    private void SetDefaultData()
    {
        ReportCode = "O";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;
        lblRecord.Text = string.Format(" <b>{0}</b>", txtremark.Text);
       
    }
    private void TranscationUpdate(int logId)
    {

        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 119
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 120
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(txtremark.Text)

                                );



    }
     public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }
        if (string.IsNullOrWhiteSpace(txtremark.Text))
        {
            ucError.ErrorMessage = "Remarks is required";
        }

        if (PhoenixMarbolLogORB2.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && string.IsNullOrWhiteSpace(ViewState["TXID"].ToString()))
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
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    private void LogBookUpdate()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                  , vesselId
                                  , General.GetNullableGuid(ViewState["TXID"].ToString())
                                  , logId
                                  , entrydate
                                  , lblRecord.Text
                                  , 1
                              );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 2
                             );

        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                                      , vesselId
                                      , logId
                                      , General.GetNullableGuid(ViewState["TXID"].ToString())
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                        );

        PhoenixMarbolLogORB2.BookEntryHistoryUpdate(usercode
                                  , vesselId
                                  , General.GetNullableGuid(ViewState["TXID"].ToString())
                                  , logId
                                  , General.GetNullableString(lblincRank.Text)
                                  , General.GetNullableDateTime(lblincSignDate.Text)
                                  , txtCode.Text
                                  , txtItemNo.Text
                                  , 1
                                  , lblRecord.Text
                                  , lblRecord.Text
                                  , "", "", "", "", "", "", ""
                            );

    }

    private void TranscationInsert(DateTime entrydate, Guid TranscationId, int logId)
    {   

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 119
                                    , TranscationId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 120
                                    , TranscationId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtremark.Text)

                                );

       


    }

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("Record1", lblRecord.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
      


        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "1");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }
    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveTxin();
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SaveTxin()
    {

        if (isValidInput() == false || IsValidSignature() == false)
        {
            ucError.Visible = true;
            return;
        }


        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TranscationUpdate(logId);
            MissedOperationalEntryTemplateUpdate(new Guid(ViewState["TXID"].ToString()));
        }

        else if (ViewState["TXID"].ToString() != "" && isMissedOperationEdit == false)
        {
            TranscationUpdate(logId);
            LogBookUpdate();
        }
        else
        {

            Guid TranscationId = Guid.NewGuid();
            TranscationInsert(entrydate, TranscationId, logId);

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , lblRecord.Text
                                     , 1
                                 );



            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                      , vesselId
                                      , TranscationId
                                      , logId
                                      , entrydate
                                      , null
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 2
                                      , true
                                );

            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                                          , vesselId
                                          , logId
                                          , TranscationId
                                          , General.GetNullableInteger(lblinchId.Text)
                                          , General.GetNullableString(lblincRank.Text)
                                          , General.GetNullableString(lblincsign.Text)
                                          , General.GetNullableDateTime(lblincSignDate.Text)
                                            );

            PhoenixMarbolLogORB2.BookEntryHistoryInsert(usercode
                                      , vesselId
                                      , TranscationId
                                      , logId
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                      , txtCode.Text
                                      , txtItemNo.Text
                                      , 1
                                      , lblRecord.Text
                                      , lblRecord.Text
                                      , "", "", "","","","",""
                                );

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
   
    protected void txt_selectedindexchaged(object sender, EventArgs e)
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

   
    protected void txtFailureReason_TextChanged(object sender, EventArgs e)
    {
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
        string rank = "co";
        string popupTitle = "Chief Officer Signature";
        string masterPopup = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, "mst", "Master Signature");
        string chiefOfficerPopup = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle);
        string script = Convert.ToBoolean(ViewState["isMasterLoggedIn"]) ? masterPopup : chiefOfficerPopup;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "bookmark", script, true);
    }
}
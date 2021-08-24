using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2MissedOperation : PhoenixBasePage
{

    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    string ReportCode = String.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        ViewState["dtkey"] = null;

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);            
            ddlLogBookName.Enabled = false;
            txtOperationDate.Enabled = false;
            isMissedOperationEdit = true;
        }


        if (string.IsNullOrWhiteSpace(Request.QueryString["isMissedOperation"]) == false)
        {
            isMissedOperation = Convert.ToBoolean(Request.QueryString["isMissedOperation"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationEdit"]) == false)
        {
            isMissedOperationEdit = Convert.ToBoolean(Request.QueryString["missedOperationEdit"]);
        }


        if (IsPostBack == false)
        {            
            Filter.MissedOperationalEntryCriteria = null;
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            ShowToolBar();
            GetLastTranscationDate();
            setDefaultData();
            logbookPopulate();
            BindData();
        }
    }

    private void BindData()
    {
        if (txid == Guid.Empty) return;
        DataSet ds = PhoenixMarbolLogORB2.LogORB2BookEntrySelectByLogId(usercode, vesselId, txid);
        DataSet missoperationDataSet = PhoenixElog.MissedOperationEntrySearch(usercode, vesselId, txid);
        DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, txid);
        Filter.MissedOperationalEntryCriteria = null;

        if (ds != null && ds.Tables.Count > 0)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["FLDITEMID"]) == 121)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 122)
                    {
                        txtRemarks.Text = (String)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 136)
                    {
                        RadComboBoxItem log = ddlLogBookName.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (log != null)
                        {
                            log.Selected = true;
                        }
                    }
                }
            }
            LoadIframe(true);
        }
    }
 

    private void logbookPopulate()
    {

        ddlLogBookName.DataSource = PhoenixMarbolLogORB2.ORB2LogRegisterList(isMissedOperationEdit);
        ddlLogBookName.DataTextField = "FLDNAME";
        ddlLogBookName.DataValueField = "FLDURL";
        ddlLogBookName.DataBind();
    }

    private void setDefaultData()
    {
        lblTempRecord0.Text = string.Format("Entry Pertaining to an earlier missed operational entry");
        lblTempCode0.Text = "O";
        lblTempDate0.Text = txtOperationDate.SelectedDate.HasValue ? txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy");
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
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

                if (txid != Guid.Empty)
                {
                    TranscationUpdate();
                    LogBookUpdate();

                }
                else
                {
                    if (Filter.MissedOperationalEntryCriteria != null)
                    {
                        //MissedOperation
                        int logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "MissedOperation");
                        NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
                        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
                        int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselId);
                        DateTime templateDate = Convert.ToDateTime(nvc["Date"]);
                        Guid logTxId = Guid.Parse(nvc["logTxId"]);
                        int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);
                        int logoperationid = Convert.ToInt32(nvc["logId"]);
                        ViewState["dtkey"] = nvc["dtkey"] != null ? nvc["dtkey"] : string.Empty;

                        BindLogData();

                        //PhoenixMarbolLogORB2.TransactionUpdate(
                        //    usercode,
                        //    vesselId,
                        //    logId,
                        //    122,
                        //    logTxId,
                        //    templateDate,
                        //    txtRemarks.Text
                        //);

                        //PhoenixMarbolLogORB2.TransactionUpdate(usercode,
                        //            vesselId,
                        //            logId,
                        //            135,
                        //            logTxId,
                        //            templateDate,
                        //            "1");

                        PhoenixMarbolLogORB2.BookEntryInsert(
                                    usercode,
                                    vesselId,
                                    logTxId,
                                    logId,
                                    entrydate,
                                    lblTempCode0.Text,
                                    lblTempItemNo0.Text,
                                    lblTempRecord0.Text,
                                    1
                        );
                        
                        int counter = 2;
                        for (int i = 1; i <= logEntryCount; i++)
                        {
                            PhoenixMarbolLogORB2.BookEntryInsert(
                                    usercode,
                                    vesselId,
                                    logTxId,
                                    logId,
                                    General.GetNullableDateTime(nvc["Date"]),
                                    General.GetNullableString(nvc["ReportCode" + i]),
                                    General.GetNullableString(nvc["ItemNo" + i]),
                                    General.GetNullableString(nvc["Record" + i]),
                                    counter,
                                    false,
                                    General.GetNullableGuid(ViewState["dtkey"].ToString())
                        );
                            counter++;
                        }


                        PhoenixMarbolLogORB2.BookEntryInsert(
                                    usercode,
                                    vesselId,
                                    logTxId,
                                    logId,
                                    entrydate,
                                    null,
                                    null,
                                    PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"])),
                                    counter + 1,
                                    true
                        );


                        //PhoenixMarbolLogORB2.BookEntryInsert(
                        //            usercode,
                        //            vesselId,
                        //            logTxId,
                        //            logId,
                        //            entrydate,
                        //            null,
                        //            null,
                        //            PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"]), false),
                        //            counter + 2,
                        //            true
                        //);

                        // history insert
                        PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                                        , vesselId
                                        , logId
                                        , logTxId
                                        , General.GetNullableInteger(nvc["inchId"])
                                        , General.GetNullableString(nvc["inchRank"])
                                        , General.GetNullableString(nvc["InchargeSignature"])
                                        , General.GetNullableDateTime(nvc["inchSignDate"])
                                        , true                                          
                                    );

                        if(logoperationid == 12 || logoperationid == 11)
                        {
                            PhoenixMarbolLogORB2.LogORB2RepairUpdate(usercode                                            
                                            , logoperationid
                                            , vesselId
                                            , logTxId
                                            , true
                                            , false                                            
                                        );
                        }


                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void TranscationUpdate()
    {
        int logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "MissedOperation");
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            121,
                            txid,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                        usercode,
                        vesselId,
                        logId,
                        122,
                        txid,
                        entrydate,
                        txtRemarks.Text
        );
    }

    private void LogBookUpdate()
    {

        if (Filter.MissedOperationalEntryCriteria != null)
        {

            NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
            int logEntryCount = Convert.ToInt32(nvc["logBookEntry"]);
            int logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "MissedOperation");

            PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
                                vesselId,
                                txid,
                                logId,
                                General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString()),
                                lblTempRecord0.Text,
                                1
                            );

            int counter = 2;

            for (int i = 1; i <= logEntryCount; i++)
            {
                PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
                                vesselId,
                                txid,
                                logId,
                                General.GetNullableDateTime(nvc["Date"]),
                                General.GetNullableString(nvc["Record" + i]),
                                counter
                            );
                    counter++;
            }

            PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
                                vesselId,
                                txid,
                                logId,
                                General.GetNullableDateTime(nvc["Date"]),
                                PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"])),
                                counter + 1 
                            );

            //PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            //                    vesselId,
            //                    txid,
            //                    logId,
            //                    General.GetNullableDateTime(nvc["Date"]),
            //                    PhoenixElog.GetSignatureName(General.GetNullableString(nvc["inchName"]), General.GetNullableString(nvc["inchRank"]), General.GetNullableDateTime(nvc["inchSignDate"])),
            //                    counter + 2
            //                );

            // history update
            PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                            , vesselId
                            , logId
                            , txid
                            , General.GetNullableInteger(nvc["inchId"])
                            , General.GetNullableString(nvc["inchRank"])
                            , General.GetNullableString(nvc["InchargeSignature"])
                            , General.GetNullableDateTime(nvc["inchSignDate"])
                            , true
                        );

        }
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }


    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrWhiteSpace(lblinchId.Text))
        //{
        //    ucError.ErrorMessage = "Duty Engineer Signature is Required Before Save";
        //}

        //if (string.IsNullOrWhiteSpace(txtRemarks.Text))
        //{
        //    ucError.ErrorMessage = "Remarks is Required Before Save";
        //}

        if (ddlLogBookName.SelectedItem == null || string.IsNullOrWhiteSpace(ddlLogBookName.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Log has to be selected Before Save";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        if (Filter.MissedOperationalEntryCriteria == null && ddlLogBookName.SelectedItem != null && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Selected log is not completed. Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        setDefaultData();
    }

    protected void ddlLogBookName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        LoadIframe(false);
    }

    private void LoadIframe(bool isEdit)
    {
        if (ddlLogBookName.SelectedItem == null)
        {
            return;
        }
        string LogName = ddlLogBookName.SelectedItem.Value;
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        string iframeSrc = txid == Guid.Empty ? string.Format("{0}/Log/{1}?isMissedOperation=true&missedOperationDate={2}", Session["sitepath"], LogName, entrydate.ToString()) : string.Format("{0}/Log/{1}?isMissedOperation=true&missedOperationDate={2}&TxnId={3}&missedOperationEdit={4}", Session["sitepath"], LogName, entrydate.ToString(), txid, isEdit);
        iframe.Src = iframeSrc;
        iframe.Visible = true;
    }

    protected void iframe_OnLoad(object sender, EventArgs e)
    {
        iframe.Attributes.Add("onLoad", "resizeIframe(this);");
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixElog.GetLogLastTranscationDate(vesselId, usercode);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindLogData();
    }

    private void BindLogData()
    {
        if (Filter.MissedOperationalEntryCriteria != null)
        {
            NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;            

            int recordCount = Convert.ToInt32(nvc["logBookEntry"]);
            for (int i = 1; i <= recordCount; i++)
            {
                RadLabel ctrlLblTempRecord = (RadLabel)template.FindControl("lblTempRecord" + i);
                RadLabel ctrlLblTempCode = (RadLabel)template.FindControl("lblTempCode" + i);
                RadLabel ctrlLblItemNo = (RadLabel)template.FindControl("lblTempItemNo" + i);
                RadLabel ctrlLblDate = (RadLabel)template.FindControl("lblTempDate" + i);
                ctrlLblTempRecord.Text = nvc["Record" + i];
                ctrlLblTempCode.Text = nvc["ReportCode" + i];
                ctrlLblItemNo.Text = nvc["ItemNo" + i];
                if (ctrlLblDate != null)
                {
                    ctrlLblDate.Text = nvc["Date"];
                }
                HtmlTableRow row = (HtmlTableRow)template.FindControl("temprecord" + i);
                row.Visible = true;
            }

            TransactionInsert();
        }
    }

    private void TransactionInsert()
    {
        NameValueCollection nvc = Filter.MissedOperationalEntryCriteria;
        int logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "MissedOperation");
        Guid logTxId = Guid.Parse(nvc["logTxId"]);
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


        PhoenixMarbolLogORB2.TransactionInsert(
                        usercode,
                        vesselId,
                        logId,
                        121,
                        logTxId,
                        entrydate,
                        entrydate.ToString()
        );

        PhoenixMarbolLogORB2.TransactionInsert(
                        usercode,
                        vesselId,
                        logId,
                        122,
                        logTxId,
                        entrydate,
                        txtRemarks.Text
        );

        PhoenixMarbolLogORB2.TransactionInsert(
                        usercode,
                        vesselId,
                        logId,
                        135,
                        logTxId,
                        entrydate,
                        "0"
        );

        PhoenixMarbolLogORB2.TransactionInsert(
                        usercode,
                        vesselId,
                        logId,
                        136,
                        logTxId,
                        entrydate,
                        ddlLogBookName.SelectedItem.Text
        );
    }

    protected void btnEditLog_Click(object sender, EventArgs e)
    {
        string LogName = ddlLogBookName.SelectedItem.Value;
        string scriptpopup = String.Format("javascript:parent.openNewWindow('MissedOperationEntry','','{0}/Log/{1}?isMissedOperation=true&TxnId={2}&missedOperationEdit=true');", Session["sitepath"], LogName, txid.ToString());
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
}
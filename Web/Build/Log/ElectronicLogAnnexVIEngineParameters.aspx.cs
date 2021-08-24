using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;
using System.Web;
using SouthNests.Phoenix.Reports;
using System.Collections.Generic;

public partial class Log_ElectronicLogAnnexVIEngineParameters : PhoenixBasePage
{
    string url = "ElectronicLogAnnexVIEngineParameters.aspx";
    string logbook = "Annex-VI Diesel Engine Prameters Record";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            gvParameter.PageSize = 100;
            BindLogStatus();
            ViewState["PARAMETERID"] = "";
            ViewState["ENGINE"] = "";
            gvParameterRecord.PageSize = 12;
            ViewState["DELETE"] = "";
            ViewState["SIGN"] = "";
            SetLatestPage();
            showLoggedUser();
        }
        ShowToolBar();
    }
    private void showLoggedUser()
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataSet ds = PhoenixElog.GetSeaFarerRankName(usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            lblUsername.Text = string.Format("{0} - {1}", row["FLDFIRSTNAME"].ToString() + "   " + row["FLDLASTNAME"].ToString(), row["FLDRANKCODE"]);
        }
    }
    private void SetLatestPage()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataTable dt = PhoenixMarpolLogEngineParameters.EngineParameterRecordSearch(Status, From, To, General.GetNullableGuid(ViewState["PARAMETERID"].ToString())
                                                                          , gvParameterRecord.CurrentPageIndex + 1
                                                                          , gvParameterRecord.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount);
            if (iTotalPageCount == 0)
            {
                return;
            }
            gvParameterRecord.CurrentPageIndex = iTotalPageCount - 1;
            gvParameterRecord.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("", "Clear", "<i class=\"fa fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("", "PDF Export", "<i class=\"fa fa-file-pdf\"></i>", "PDF");
        toolbar.AddFontAwesomeButton("", "Add New Record", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("", "History", "<i class=\"fa fa-copy-requisition\"></i>", "TRANSCATIONHISTORY");
        toolbar.AddFontAwesomeButton("", "User Guide", "<i class=\"fas fa-question\"></i>", "HELP", ToolBarDirection.Right);
        toolbar.AddFontAwesomeButton("", "Event Log", "<i class=\"fas fa-administration\"></i>", "EVENTLOG");
        Tabstrip.MenuList = toolbar.Show();
        Tabstrip.AccessRights = this.ViewState;
    }
    protected void gvParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            DataTable dt = PhoenixMarpolLogEngineParameters.EngineParametersSearch();
            gvParameter.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    protected void gvParameter_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            Guid? ComponentId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("radcomponentid")).Text);
            Guid? ParameterId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("radid")).Text);
            string ComponentName = General.GetNullableString(((RadLabel)e.Item.FindControl("radlblenginename")).Text);
            string Make = General.GetNullableString(((RadTextBox)e.Item.FindControl("radtbenginemakeentry")).Text);
            string Model = General.GetNullableString(((RadTextBox)e.Item.FindControl("radlblenginemodelentry")).Text);
            string SerialNumber = General.GetNullableString(((RadTextBox)e.Item.FindControl("radlblengineserialentry")).Text);
            string CertificateNumber = General.GetNullableString(((RadTextBox)e.Item.FindControl("radlblengineeiappentry")).Text);

            PhoenixMarpolLogEngineParameters.EngineParameterInsert(ComponentId, ComponentName, Make, Model, SerialNumber, CertificateNumber, ParameterId);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddl_TextChanged(object sender, EventArgs e)
    {
        try
        {
            gvParameter.CurrentPageIndex = 0;
            gvParameter.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindLogStatus()
    {
        try
        {

            DataSet ds = PhoenixElog.LogStatusList();
            ddlStatus.DataSource = ds;
            ddlStatus.DataTextField = "FLDSTATUS";
            ddlStatus.DataValueField = "FLDLOGSTATUSID";
            ddlStatus.DataBind();
            DropDownListItem item = new DropDownListItem("All", string.Empty);
            item.Selected = true;
            ddlStatus.Items.Insert(0, item);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvParameter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                ViewState["PARAMETERID"] = ((RadLabel)e.Item.FindControl("radid")).Text;
                ViewState["ENGINE"] = ((RadLabel)e.Item.FindControl("radlblenginename")).Text;
                e.Item.Selected = true;
                gvParameterRecord.MasterTableView.ColumnGroups[0].HeaderText = ViewState["ENGINE"].ToString();

                gvParameterRecord.Rebind();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            string caption = dce.Item.ToolTip;

            Dictionary<string, bool> accessrights = GetAccessRights();

            if (accessrights[CommandName.ToUpper()] == false)
            {
                string errorMessage = string.Format("You don't have access rights", CommandName.ToLower());
                throw new Exception(errorMessage);
            }
            EventLog(url, caption, CommandName, logbook);
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvParameterRecord.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStatus.SelectedValue = "";
                gvParameterRecord.Rebind();

            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                if (General.GetNullableGuid(ViewState["PARAMETERID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Please select an Engine  / Enter relevant details of Engine.";
                    ucError.Visible = true;
                    return;
                }

                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogPdfExport','','{0}/Log/ElectricLogPDFExport.aspx?refreshWindowName=Log', 'false', '370', '300', null, null, {{ 'disableMinMax': true }})", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (General.GetNullableGuid(ViewState["PARAMETERID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Please select an Engine  / Enter relevant details of Engine.";
                    ucError.Visible = true;
                    return;
                }
                string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIEngineParametersRecord.aspx?Parameterid=" + ViewState["PARAMETERID"] + "&Engine=" + ViewState["ENGINE"] + "','false','600px','410px');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                if (General.GetNullableGuid(ViewState["PARAMETERID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Please select an Engine  / Enter relevant details of Engine.";
                    ucError.Visible = true;
                    return;
                }
                string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIEngineParametersRecordHistory.aspx?Parameterid=" + ViewState["PARAMETERID"] + "&Engine=" + ViewState["ENGINE"] + "','false','1100px','500px');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);


            }
            if (CommandName.ToUpper().Equals("HELP"))
            {
                string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIHelp.aspx?helpid=ep','false',null,null);");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("EVENTLOG"))
            {


                string scriptpopup = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogEventLogHistory.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvParameterRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataTable dt = PhoenixMarpolLogEngineParameters.EngineParameterRecordSearch(Status, From, To, General.GetNullableGuid(ViewState["PARAMETERID"].ToString())
                                                                          , gvParameterRecord.CurrentPageIndex + 1
                                                                          , gvParameterRecord.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount);
            gvParameterRecord.DataSource = dt;
            gvParameterRecord.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvParameterRecord_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "ROWCLICK")
            {
                return;
            }
            if (e.Item == null)
            {
                return;
            }
            string scriptpopup = "";
            ViewState["DELETE"] = "";
            ViewState["SIGN"] = "";
            RadLabel Id = (RadLabel)e.Item.FindControl("radrid");
            RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radoicsign");
           
            string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            string CommandName = e.CommandName.ToUpper();


            Dictionary<string, bool> accessrights = GetAccessRights();

            if (accessrights[CommandName.ToUpper()] == false)
            {
                string errorMessage = string.Format("You don't have access rights", CommandName.ToLower());
                throw new Exception(errorMessage);
            }
            if (e.CommandName.ToUpper() == "UNLOCK")
            {


               


                scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Cheif Engineer Signature" + "&rankName=" + "CE" + "&LogBookId=" + Id.Text + "&popupname=Log" + "','false','400','190');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper() == "SIGN")
            {
                ViewState["SIGN"] = "OIC";

                

                scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Officer in Charge Signature" + "&rankName=" + Rank + "&LogBookId=" + Id.Text + "&popupname=Log" + "','false','400','190');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper() == "CHIEFENGGSIGN" )
            {
                if (OICSignStatus.Text != "1")
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = "You cannot verify the record as it is not signed.";
                    ucError.Visible = true;
                    return;

                }

                


                scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Cheif Engineer Signature" + "&rankName=" + "CE" + "&LogBookId=" + Id.Text + "&popupname=Log" + "','false','400','190');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper() == "DELETE")
            {

                ViewState["DELETE"] = "1";
                


                scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Officer In Charge Signature" + "&rankName=" + Rank + "&LogBookId=" + Id.Text + "&popupname=Log" + "','false','400','190');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper() == "AMEND")
            {
                scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','Log/ElectronicLogAnnexVIEngineParametersRecord.aspx?Parameterid=" + ViewState["PARAMETERID"] + "&Engine=" + Server.UrlEncode(ViewState["ENGINE"].ToString()) + "&id=" + Id.Text + "','false','600px','410px');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvParameterRecord_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;
                RadLabel Id = (RadLabel)e.Item.FindControl("radrid");
                RadLabel Dtkey = (RadLabel)e.Item.FindControl("raddtkey");
                RadLabel OicSign = (RadLabel)e.Item.FindControl("radoicsign");
                RadLabel CESign = (RadLabel)e.Item.FindControl("radcesign");
                RadLabel OicSignt = (RadLabel)e.Item.FindControl("radoicsigntext");
                RadLabel CESignt = (RadLabel)e.Item.FindControl("radcesigntext");
                LinkButton OSign = (LinkButton)e.Item.FindControl("btnoicsign");
                LinkButton CSign = (LinkButton)e.Item.FindControl("btncesign");
                RadLabel Status = (RadLabel)e.Item.FindControl("radstatus");
                LinkButton Lock = (LinkButton)e.Item.FindControl("btnredlock");
                LinkButton Unlock = (LinkButton)e.Item.FindControl("btngreenlock");
                LinkButton Edit = (LinkButton)e.Item.FindControl("btnedit1");
                LinkButton Delete = (LinkButton)e.Item.FindControl("btndelete1");
                ImageButton Attachments = (ImageButton)e.Item.FindControl("btnattachments");
                RadLabel IsBackDatedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");

                LinkButton BackDatedEntry = (LinkButton)e.Item.FindControl("imgFlag");
                RadLabel radadjustment = (RadLabel)e.Item.FindControl("radadjustment");
                RadLabel radremarks = (RadLabel)e.Item.FindControl("radremarks");
                RadLabel raddate = (RadLabel)e.Item.FindControl("raddate");

                int fldIsDeleted = dv["FLDISDELETED"].ToString() == "" ? 0 : Convert.ToInt32(dv["FLDISDELETED"].ToString());

                if (fldIsDeleted == 1)
                {
                    radadjustment.Text = "<S>" + dv["FLDADJUSTMENT"].ToString() + "</S>";
                    radremarks.Text = "<S>" + dv["FLDREMARKS"].ToString() + "</S>";
                    raddate.Text = "<S>" + General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.Date) + "</S>";
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "1")
                {
                    radadjustment.Text = "<S>" + dv["FLDOLDADJUSTMENT"].ToString() + "</S></br>" + dv["FLDADJUSTMENT"].ToString();
                    radremarks.Text = "<S>" + dv["FLDOLDREMARKS"].ToString() + "</S></br>" + dv["FLDREMARKS"].ToString();
                    raddate.Text = General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.Date);


                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "")
                {
                    radadjustment.Text = dv["FLDADJUSTMENT"].ToString();
                    radremarks.Text = dv["FLDREMARKS"].ToString();
                    raddate.Text = General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.Date);
                }

                if (DataBinder.Eval(e.Item.DataItem, "FLDATTACHMENT").ToString() == "0")
                    Attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    Attachments.ImageUrl = Session["images"] + "/attachment.png";
                if (Status.Text == "2" || Status.Text == "5")
                {
                    Unlock.Visible = true;
                    Unlock.Enabled = false;

                    if (Status.Text == "2" )
                    {
                        CSign.Text = "Verify";
                    }
                    else
                    { CSign.Text = "Re-Verify"; }
                }
                else
                {
                    Lock.Visible = true;
                    Edit.Visible = false;
                    Delete.Visible = false;

                    if (Status.Text == "3")
                    { CESignt.Text = "Verified"; }
                    else
                    {
                        CESignt.Text = "Re-Verified";
                    } 
                }

                if (OicSign != null && CESign != null)
                {
                    if (OicSign.Text != "1")
                    {
                        OSign.Visible = true;
                        CSign.Enabled = false;
                        //OSign.Attributes.Add("onclick", "javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname=Officer Incharge Signature&rankName=Chief Officer&LogBookId="+ Id.Text + "', 'false', '370', '170', null, null, {{ 'disableMinMax': true }})');return false");
                       // OSign.Attributes.Add("onclick", "javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Officer Incharge Signature" + "&rankName=" + "CO" + "&LogBookId=" + Id.Text + "&popupname=Log" + "','false','400','190');return false");

                    }
                    else { OicSignt.Visible = true; }
                    if (CESign.Text != "1")
                    {
                        CSign.Visible = true;
                        //CSign.Attributes.Add("onclick", "javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Chief Engineer Signature" + "&rankName=" + "CE" + "&LogBookId=" + Id.Text + "&popupname=Log" + "','false','400','190');return false");
                    }
                    else { CESignt.Visible = true; }
                }
                if (Attachments != null && CESign.Text != "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + Dtkey.Text + "&MOD=LOG&RefreshWindowName=Log" + "');return false;");
                }
                if (Attachments != null && CESign.Text == "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + Dtkey.Text + "&MOD=LOG&RefreshWindowName=Log" + "&u=n" + "');return false;");
                }



                if (IsBackDatedEntry.Text == "1")
                {
                    BackDatedEntry.Visible = true;
                }

            }
            if (e.Item is GridFooterItem)
            {
                e.Item.Cells[2].ColumnSpan = ((RadGrid)sender).Columns.Count + 2;
                e.Item.Cells[0].Visible = false;
                e.Item.Cells[1].Visible = false;
                for (int i = 3; i < ((RadGrid)sender).Columns.Count + 2; i++)
                {
                    e.Item.Cells[i].Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.DutyEngineerSignatureFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;


                string Name = nvc.Get("name");
                string Rank = nvc.Get("rank");
                Guid? Id = General.GetNullableGuid(nvc.Get("LogBookId"));

                Filter.DutyEngineerSignatureFilterCriteria = null;

                if (ViewState["DELETE"].ToString() != "1")
                {
                    if (ViewState["SIGN"].ToString() == "OIC")
                    {
                        PhoenixMarpolLogEngineParameters.EngineParameterRecordSign(Id, Rank, Name, 1);

                    }
                    else
                    {
                        PhoenixMarpolLogEngineParameters.EngineParameterRecordSign(Id, Rank, Name);
                    }

                }
                else
                {
                    PhoenixMarpolLogEngineParameters.EngineParameterRecordDelete(Id, Rank, Name);
                }
                
            }

            if (Filter.LogBookPdfExportCriteria != null)
            {
                NameValueCollection nvc = Filter.LogBookPdfExportCriteria;
                DateTime? startDate = General.GetNullableDateTime(txtFromDate.Text);
                DateTime? endDate = General.GetNullableDateTime(txtToDate.Text);
                int? pageNo = gvParameterRecord.CurrentPageIndex + 1;
                
                if (nvc["isDateRange"] != null && nvc["isDateRange"].ToLower() == "true")
                {
                    startDate = General.GetNullableDateTime(nvc["startDate"]);
                    endDate = General.GetNullableDateTime(nvc["endDate"]);
                    pageNo = null;

                }
                Filter.LogBookPdfExportCriteria = null;
                ExportPDF(pageNo, startDate, endDate);
            }
            gvParameterRecord.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ExportPDF(int? pageNo, DateTime? startDate, DateTime? endDate)
    {
        string Tmpfilelocation = string.Empty;
        string[] reportfile = new string[0];
        int iTotalPageCount = 0;
        int iRowCount = 0;
        DataSet ds = GetSSRSData(ref iRowCount, ref iTotalPageCount, startDate, endDate, pageNo);
        string filename = "Engine Parameters Log Book.Pdf";
        
        if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("applicationcode", "24");
            nvc.Add("reportcode", "MARPOLENGINEPARAMRECORD");
            
            nvc.Add("CRITERIA", "");
            Session["PHOENIXREPORTPARAMETERS"] = nvc;

            Tmpfilelocation = HttpContext.Current.Request.MapPath("~/");

            Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);
            PhoenixReportsCommon.LoadLogo(ds);
            PhoenixSsrsReportsCommon.getLogo();
            PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
            Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
        }
    }

    private DataSet GetSSRSData(ref int iRowCount, ref int iTotalPageCount, DateTime? startDate, DateTime? endDate, int? pageNo)
    {
        DataSet ds =   PhoenixMarpolLogEngineParameters.EngineParameterRecordReportSearch(startDate, endDate, General.GetNullableGuid(ViewState["PARAMETERID"].ToString()), pageNo, gvParameterRecord.PageSize);
        int NoRows = ds.Tables[0].Rows.Count;

        if (NoRows != 0)
        {

            int Remainder = NoRows % 12;
            int RowsRequired = 12 - Remainder;

            if (RowsRequired > 0)
            {
                for (int i = 0; i < RowsRequired; i++)
                {
                    ds.Tables[0].Rows.Add();
                }
            }
        }
        return ds;
    }
    private Dictionary<string, bool> GetAccessRights()
    {
        // get it from db and form a dictionary
        Dictionary<string, bool> accessrights = new Dictionary<string, bool>();
        string rankShortCode = PhoenixElog.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string designaionShortCode = null;
        bool isVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? true : false;
        bool isChiefEngg = PhoenixElog.validCheifEngineer(rankShortCode);
        DataSet ds = PhoenixMarpolLogEngineParameters.GetMarpolLogEPAccessRights(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("VIEW", Convert.ToBoolean(row["FLDVIEW"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDENTRY"]));
            accessrights.Add("SIGN", Convert.ToBoolean(row["FLDSIGN"]));
            accessrights.Add("UNLOCK", Convert.ToBoolean(row["FLDUNLOCK"]));
            accessrights.Add("AMEND", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("DELETE", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("EDIT", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("TRANSCATIONHISTORY", Convert.ToBoolean(row["FLDHISTORY"]));

            if (isChiefEngg)
            {
                accessrights.Add("CHIEFENGGSIGN", Convert.ToBoolean(row["FLDSIGN"]));
            }
            else
            {
                accessrights.Add("CHIEFENGGSIGN", Convert.ToBoolean(false));
            }
        }
        else
        {
            accessrights.Add("VIEW", false);
            accessrights.Add("ADD", false);
            accessrights.Add("SIGN", false);
            accessrights.Add("UNLOCK", false);
            accessrights.Add("CHIEFENGGSIGN", false);
            accessrights.Add("AMEND", false);
            accessrights.Add("TRANSCATIONHISTORY", false);
            accessrights.Add("DELETE", false);
            accessrights.Add("EDIT", false);
        }
        accessrights.Add("PDF", true);
        accessrights.Add("FIND", true);
        accessrights.Add("CLEAR", true);
        accessrights.Add("EVENTLOG", true);
        accessrights.Add("HELP", true);

        return accessrights;
    }
    private void EventLog(string url, string Caption, string CommandName, string logbook)
    {
        PhoenixElogCommon.MarpolEventLogInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                url,
                                                CommandName,
                                                Caption,
                                                logbook,
                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                General.GetNullableDateTime(null), General.GetNullableDateTime(null));
    }
}
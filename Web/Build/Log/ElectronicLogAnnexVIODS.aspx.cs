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
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

public partial class Log_ElectronicLogAnnexVIODS : PhoenixBasePage
{
    string url = "ElectronicLogAnnexVIODS.aspx";
    string logbook = "Annex-VI Ozone-depleting Substances Record";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            BindLogStatus();
            gvODS.PageSize = 12;

            gvODSM.PageSize = 12;
            gvODSSD.PageSize = 12;
            SetLatestPage();
            ViewState["DELETE"] = "";
            ViewState["SIGN"] = "";
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

            DataTable dt = PhoenixMarpolLogODS.ODSRecordSearch(From, To, Status, gvODS.CurrentPageIndex + 1, gvODS.PageSize, ref iRowCount, ref iTotalPageCount);
            if (iTotalPageCount == 0)
            {
                return;
            }
            gvODS.CurrentPageIndex = iTotalPageCount - 1;
            gvODS.Rebind();


            DataTable dt1 = PhoenixMarpolLogODS.ODSRecordMaintSearch(From, To, Status, gvODSM.CurrentPageIndex + 1, gvODSM.PageSize, ref iRowCount, ref iTotalPageCount);
            if (iTotalPageCount == 0)
            {
                return;
            }
            gvODSM.CurrentPageIndex = iTotalPageCount - 1;
            gvODS.Rebind();


            DataTable dt2 = PhoenixMarpolLogODS.ODSRecordDSSearch(From, To, Status, gvODSSD.CurrentPageIndex + 1, gvODSSD.PageSize, ref iRowCount, ref iTotalPageCount);
            if (iTotalPageCount == 0)
            {
                return;
            }
            gvODSSD.CurrentPageIndex = iTotalPageCount - 1;
            gvODS.Rebind();


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
        MenuMain.MenuList = toolbar.Show();
        MenuMain.AccessRights = this.ViewState;
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptpopup = "";
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string caption = dce.Item.ToolTip;
            string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);



            Dictionary<string, bool> accessrights = GetAccessRights();

            if (accessrights[CommandName.ToUpper()] == false)
            {
                string errorMessage = string.Format("You don't have access rights", CommandName.ToLower());
                throw new Exception(errorMessage);
            }
            if (RadMultiPage1.SelectedIndex == 0)
            {
                logbook = "Annex-VI Ozone-depleting Substances Record";
            }
            if (RadMultiPage1.SelectedIndex == 1)
            {
                logbook = "Annex-VI Ozone-depleting Substances Maintenance";
            }
            if (RadMultiPage1.SelectedIndex == 2)
            {
                logbook = "Annex-VI Ozone-depleting Substances Supply and Discharge";
            }

            EventLog(url, caption, CommandName, logbook);
            if (CommandName.ToUpper().Equals("FIND"))
            {
                SetLatestPage();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStatus.SelectedValue = "";
                gvODS.Rebind();
                gvODSM.Rebind();
                gvODSSD.Rebind();
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                scriptpopup = String.Format("javascript:parent.openNewWindow('LogPdfExport','','{0}/Log/ElectricLogPDFExport.aspx?refreshWindowName=Log', 'false', '370', '300', null, null, {{ 'disableMinMax': true }})", Session["sitepath"]);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {

                if (RadMultiPage1.SelectedIndex == 0)
                {
                    scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIODSAdd.aspx','false','600px','460px');");

                }
                if (RadMultiPage1.SelectedIndex == 1)
                {
                    scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIODSMAdd.aspx','false','600px','460px');");

                }
                if (RadMultiPage1.SelectedIndex == 2)
                {
                    scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIODSSDAdd.aspx','false','600px','460px');");

                }

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("TRANSCATIONHISTORY"))
            {
                if (RadMultiPage1.SelectedIndex == 0)
                {
                    scriptpopup = String.Format("javascript:parent.openNewWindow('code1','Ozone Depleting Substances Record History','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIODSRecordHistory.aspx',null, null, null, null, null, {{model:true}} );");

                }
                if (RadMultiPage1.SelectedIndex == 1)
                {
                    scriptpopup = String.Format("javascript:parent.openNewWindow('code1','Ozone Depleting Substances Record Maintenace History','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIODSMainTRecordHistory.aspx',null, null, null, null, null, {{model:true}} );");

                }
                if (RadMultiPage1.SelectedIndex == 2)
                {
                    scriptpopup = String.Format("javascript:parent.openNewWindow('code1','Ozone Depleting Substances Record Supply And Discharge History','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIODSSDRecordHistory.aspx',null, null, null, null, null, {{model:true}} );");

                }
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("HELP"))
            {
                scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Log/ElectronicLogAnnexVIHelp.aspx?helpid=ods','false',null,null);");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("EVENTLOG"))
            {


                scriptpopup = String.Format("javascript:parent.openNewWindow('LogMasterSign','','" + Session["sitepath"] + "/Log/ElectricLogEventLogHistory.aspx',null, null,null,null,null,{{model:true}});");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

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


                if (RadMultiPage1.SelectedIndex == 0 && ViewState["DELETE"].ToString() != "1")
                {
                    if (ViewState["SIGN"].ToString() == "OIC")
                    {
                        PhoenixMarpolLogODS.ODSRecordSign(Id, Rank, Name, 1);
                    }
                    else
                    {
                        PhoenixMarpolLogODS.ODSRecordSign(Id, Rank, Name);

                    }

                }
                if (RadMultiPage1.SelectedIndex == 0 && ViewState["DELETE"].ToString() == "1")
                {
                    PhoenixMarpolLogODS.ODSRecordDelete(Id, Rank, Name);
                }
                if (RadMultiPage1.SelectedIndex == 1 && ViewState["DELETE"].ToString() != "1")
                {
                    if (ViewState["SIGN"].ToString() == "OIC")
                    {
                        PhoenixMarpolLogODS.ODSMaintRecordSign(Id, Rank, Name, 1);
                    }
                    else
                    {
                        PhoenixMarpolLogODS.ODSMaintRecordSign(Id, Rank, Name);
                    }

                }
                if (RadMultiPage1.SelectedIndex == 1 && ViewState["DELETE"].ToString() == "1")
                {
                    PhoenixMarpolLogODS.ODSMaintDelete(Id, Rank, Name);
                }

                if (RadMultiPage1.SelectedIndex == 2 && ViewState["DELETE"].ToString() != "1")
                {
                    if (ViewState["SIGN"].ToString() == "OIC")
                    {
                        PhoenixMarpolLogODS.ODSSDRecordSign(Id, Rank, Name, 1);
                    }
                    else
                    {
                        PhoenixMarpolLogODS.ODSSDRecordSign(Id, Rank, Name);
                    }

                }
                if (RadMultiPage1.SelectedIndex == 2 && ViewState["DELETE"].ToString() == "1")
                {
                    PhoenixMarpolLogODS.ODSSDDelete(Id, Rank, Name);
                }
                ViewState["SIGN"] = "";

            }
            if (Filter.LogBookPdfExportCriteria != null)
            {
                NameValueCollection nvc = Filter.LogBookPdfExportCriteria;
                DateTime? startDate = General.GetNullableDateTime(txtFromDate.Text);
                DateTime? endDate = General.GetNullableDateTime(txtToDate.Text);
                int? pageNo = 0;
                if (RadMultiPage1.SelectedIndex == 0)
                {
                    pageNo = gvODS.CurrentPageIndex + 1;
                }
                if (RadMultiPage1.SelectedIndex == 1)
                {
                    pageNo = gvODSM.CurrentPageIndex + 1;
                }
                if (RadMultiPage1.SelectedIndex == 2)
                {
                    pageNo = gvODSSD.CurrentPageIndex + 1;
                }
                if (nvc["isDateRange"] != null && nvc["isDateRange"].ToLower() == "true")
                {
                    startDate = General.GetNullableDateTime(nvc["startDate"]);
                    endDate = General.GetNullableDateTime(nvc["endDate"]);
                    pageNo = null;

                }
                Filter.LogBookPdfExportCriteria = null;
                ExportPDF(pageNo, startDate, endDate);
            }
            gvODS.Rebind();
            gvODSM.Rebind();
            gvODSSD.Rebind();
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
        string filename = "";
        if (RadMultiPage1.SelectedIndex == 0)
        {

            filename = string.Format("{0}.pdf", "ODS Record Book");

        }
        if (RadMultiPage1.SelectedIndex == 1)
        {
            filename = string.Format("{0}.pdf", "ODS Record Maintenance Book");
        }
        if (RadMultiPage1.SelectedIndex == 2)
        {
            filename = string.Format("{0}.pdf", "ODS Record Supply and Discharge Book");
        }
        if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("applicationcode", "24");
            if (RadMultiPage1.SelectedIndex == 0)
            {
                nvc.Add("reportcode", "MARPOLLOGODSRECORD");
            }
            if (RadMultiPage1.SelectedIndex == 1)
            {
                nvc.Add("reportcode", "MARPOLLOGODSMRECORD");
            }
            if (RadMultiPage1.SelectedIndex == 2)
            {
                nvc.Add("reportcode", "MARPOLLOGODSSDRECORD");
            }
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
        DataSet ds = new DataSet();

        if (RadMultiPage1.SelectedIndex == 0)
        {
            ds = PhoenixMarpolLogODS.ODSRecordReportSearch(startDate, endDate, null, pageNo, gvODS.PageSize);

        }
        if (RadMultiPage1.SelectedIndex == 1)

        {
            ds = PhoenixMarpolLogODS.ODSRecordMReportSearch(startDate, endDate, null, pageNo, gvODS.PageSize);
        }
        if (RadMultiPage1.SelectedIndex == 2)
        {
            ds = PhoenixMarpolLogODS.ODSRecordSDReportSearch(startDate, endDate, null, pageNo, gvODS.PageSize);
        }


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
    public void BindLogStatus()
    {
        try
        {

            DataSet ds = PhoenixMarpolLogODS.AnnexVILogStatusList();
            ddlStatus.DataSource = ds;
            ddlStatus.DataTextField = "FLDANNEXVISTATUS";
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

    protected void ddl_TextChanged(object sender, EventArgs e)
    {
        try
        {
            gvODS.CurrentPageIndex = 0;
            gvODSM.CurrentPageIndex = 0;
            gvODSSD.CurrentPageIndex = 0;
            gvODS.Rebind();
            gvODSM.Rebind();
            gvODSSD.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvODS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataTable dt = PhoenixMarpolLogODS.ODSRecordSearch(From, To, Status, gvODS.CurrentPageIndex + 1, gvODS.PageSize, ref iRowCount, ref iTotalPageCount);

            gvODS.DataSource = dt;
            gvODS.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvODS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "ROWCLICK" || e.CommandName.ToUpper() == "")
            {
                return;
            }
            if (e.Item == null)
            {
                return;
            }
            string scriptpopup = "";
            ViewState["DELETE"] = "";
            RadLabel Id = (RadLabel)e.Item.FindControl("radrid");
            RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radoicsignid");
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
            if (e.CommandName.ToUpper() == "CHIEFENGGSIGN")
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
                scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','Log/ElectronicLogAnnexVIODSAdd.aspx?recordid=" + Id.Text + "','false','610px','460px');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvODS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {


            if (e.Item is GridDataItem)
            {

                DataRowView dv = (DataRowView)e.Item.DataItem;

                LinkButton Lock = (LinkButton)e.Item.FindControl("btnredlock");
                LinkButton UnLock = (LinkButton)e.Item.FindControl("btngreenlock");
                RadLabel Id = (RadLabel)e.Item.FindControl("radrid");
                RadLabel DtKey = (RadLabel)e.Item.FindControl("raddtkey");
                RadLabel IsBackDatedEntry = (RadLabel)e.Item.FindControl("radisbackdatedentry");

                LinkButton BackDatedEntry = (LinkButton)e.Item.FindControl("imgFlag");
                LinkButton OICSign = (LinkButton)e.Item.FindControl("radoicsignlink");
                LinkButton CEVerifySign = (LinkButton)e.Item.FindControl("radcesignveifylink");
                LinkButton CEReVerifySign = (LinkButton)e.Item.FindControl("radcesignreverifylink");

                RadLabel Status = (RadLabel)e.Item.FindControl("radstatus");
                RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radoicsignid");
                RadLabel CESignStatus = (RadLabel)e.Item.FindControl("radcesignid");
                RadLabel OICSignLabel = (RadLabel)e.Item.FindControl("radoicsign");
                RadLabel CEVerifiedLabel = (RadLabel)e.Item.FindControl("radcesign");
                RadLabel CEReVerifiedLabel = (RadLabel)e.Item.FindControl("radcereverfiedsign");
                RadLabel IsDeleted = (RadLabel)e.Item.FindControl("radisdeleted");

                RadLabel raddate = (RadLabel)e.Item.FindControl("raddate");
                RadLabel radequipment = (RadLabel)e.Item.FindControl("radequipment");
                RadLabel radmaker = (RadLabel)e.Item.FindControl("radmaker");
                RadLabel radmodel = (RadLabel)e.Item.FindControl("radmodel");
                RadLabel radlocation = (RadLabel)e.Item.FindControl("radlocation");
                RadLabel radodsname = (RadLabel)e.Item.FindControl("radodsname");
                RadLabel radmass = (RadLabel)e.Item.FindControl("radmass");


                ImageButton Attachments = (ImageButton)e.Item.FindControl("btnattachments");
                LinkButton Edit = (LinkButton)e.Item.FindControl("btnedit");
                LinkButton Delete = (LinkButton)e.Item.FindControl("btndelete");

                int fldIsDeleted = dv["FLDISDELETED"].ToString() == "" ? 0 : Convert.ToInt32(dv["FLDISDELETED"].ToString());

                if (fldIsDeleted == 1)
                {
                    raddate.Text = "<S>" +General.GetDateTimeToString(dv["FLDDDATE"].ToString(),DateDisplayOption.DateTimeHR24) + "</S>";
                    radequipment.Text = "<S>" + dv["FLDEQUIPMENT"].ToString() + "</S>";
                    radmaker.Text = "<S>" + dv["FLDMAKER"].ToString() + "</S>";
                    radmodel.Text = "<S>" + dv["FLDMODEL"].ToString() + "</S>";
                    radlocation.Text = "<S>" + dv["FLDLOCATION"].ToString() + "</S>";
                    radodsname.Text = "<S>" + dv["FLDODSNAME"].ToString() + "</S>";
                    radmass.Text = "<S>" + dv["FLDMASS"].ToString() + "</S>";
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "1")
                {
                    raddate.Text = General.GetDateTimeToString(dv["FLDDDATE"].ToString(), DateDisplayOption.DateTimeHR24);
                    radequipment.Text = "<S>" + dv["FLDOLDEQUIPMENT"].ToString() + "</S></br>" + dv["FLDEQUIPMENT"].ToString();
                    radmaker.Text = "<S>" + dv["FLDOLDMAKER"].ToString() + "</S></br>" + dv["FLDMAKER"].ToString();
                    radmodel.Text = "<S>" + dv["FLDOLDMODEL"].ToString() + "</S></br>" + dv["FLDMODEL"].ToString();
                    radlocation.Text = "<S>" + dv["FLDOLDLOCATION"].ToString() + "</S></br>" + dv["FLDLOCATION"].ToString();
                    radodsname.Text = "<S>" + dv["FLDOLDODSNAME"].ToString() + "</S></br>" + dv["FLDODSNAME"].ToString();
                    radmass.Text = "<S>" + dv["FLDOLDMASS"].ToString() + "</S></br>" + dv["FLDMASS"].ToString();
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "")
                {
                    raddate.Text = General.GetDateTimeToString(dv["FLDDDATE"].ToString(), DateDisplayOption.DateTimeHR24);
                    radequipment.Text = dv["FLDEQUIPMENT"].ToString();
                    radmaker.Text = dv["FLDMAKER"].ToString();
                    radmodel.Text = dv["FLDMODEL"].ToString();
                    radlocation.Text = dv["FLDLOCATION"].ToString();
                    radodsname.Text = dv["FLDODSNAME"].ToString();
                    radmass.Text = dv["FLDMASS"].ToString();
                }

                if (DataBinder.Eval(e.Item.DataItem, "FLDATTACHMENT").ToString() == "0")
                    Attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    Attachments.ImageUrl = Session["images"] + "/attachment.png";

                if (Attachments != null && CESignStatus.Text != "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + DtKey.Text + "&MOD=LOG&RefreshWindowName=Log" + "');return false;");
                }
                if (Attachments != null && CESignStatus.Text == "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + DtKey.Text + "&MOD=LOG&RefreshWindowName=Log" + "&u=n" + "');return false;");
                }
                if (IsDeleted.Text == "1" && CESignStatus.Text == "1")
                {
                    Attachments.Visible = false;
                    Edit.Visible = false;
                    Delete.Visible = false;
                }

                if (IsDeleted.Text == "1" && CESignStatus.Text != "1")
                {

                    Delete.Visible = false;
                }
                if (CESignStatus.Text == "1" && OICSignStatus.Text == "1")
                {
                    Lock.Visible = true;
                }
                else
                {
                    UnLock.Visible = true;
                }

                if (OICSignStatus.Text != "1")
                {
                    OICSign.Visible = true;
                    if (Status.Text == "2")
                    {
                        CEVerifySign.Visible = true;
                        CEVerifySign.Enabled = false;
                    }
                    if (Status.Text == "5")
                    {
                        CEReVerifySign.Visible = true;
                        CEReVerifySign.Enabled = false;
                    }

                }
                if (OICSignStatus.Text == "1" && CESignStatus.Text != "1")
                {
                    OICSignLabel.Visible = true;
                    if (Status.Text == "2")
                    {
                        CEVerifySign.Visible = true;

                    }
                    if (Status.Text == "5")
                    {
                        CEReVerifySign.Visible = true;
                        CEReVerifySign.Text = "Re-Verify";
                    }

                }
                if (CESignStatus.Text == "1" && OICSignStatus.Text == "1")
                {
                    OICSignLabel.Visible = true;

                    if (Status.Text == "3")
                    {
                        CEVerifiedLabel.Visible = true;

                    }
                    if (Status.Text == "6")
                    {
                        CEReVerifiedLabel.Visible = true;
                        CEReVerifiedLabel.Text = "Re-Verified";
                    }

                    Edit.Visible = false;
                    Delete.Visible = false;
                }
                if (IsBackDatedEntry.Text == "1")
                {
                    BackDatedEntry.Visible = true;
                }
            }
            if (e.Item is GridFooterItem)
            {
                e.Item.Cells[2].ColumnSpan = ((RadGrid)sender).Columns.Count+2;
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


    protected void gvODSM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataTable dt = PhoenixMarpolLogODS.ODSRecordMaintSearch(From, To, Status, gvODSM.CurrentPageIndex + 1, gvODSM.PageSize, ref iRowCount, ref iTotalPageCount);

            gvODSM.DataSource = dt;
            gvODSM.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvODSM_ItemCommand(object sender, GridCommandEventArgs e)
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
            RadLabel Id = (RadLabel)e.Item.FindControl("radmrid");
            RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radmoicsignid");
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
            if (e.CommandName.ToUpper() == "CHIEFENGGSIGN")
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
                scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','Log/ElectronicLogAnnexVIODSMAdd.aspx?recordid=" + Id.Text + "','false','610px','460px');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvODSM_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;

                LinkButton Lock = (LinkButton)e.Item.FindControl("btnmredlock");
                LinkButton UnLock = (LinkButton)e.Item.FindControl("btnmgreenlock");
                RadLabel Id = (RadLabel)e.Item.FindControl("radmrid");
                RadLabel DtKey = (RadLabel)e.Item.FindControl("mraddtkey");
                RadLabel IsBackDatedEntry = (RadLabel)e.Item.FindControl("radmisbackdatedentry");

                LinkButton BackDatedEntry = (LinkButton)e.Item.FindControl("imgmFlag");
                LinkButton OICSign = (LinkButton)e.Item.FindControl("radmoicsignlink");
                LinkButton CEVerifySign = (LinkButton)e.Item.FindControl("radmcesignveifylink");
                LinkButton CEReVerifySign = (LinkButton)e.Item.FindControl("radmcesignreverifylink");

                RadLabel Status = (RadLabel)e.Item.FindControl("radmstatus");
                RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radmoicsignid");
                RadLabel CESignStatus = (RadLabel)e.Item.FindControl("radmcesignid");
                RadLabel OICSignLabel = (RadLabel)e.Item.FindControl("radmoicsign");
                RadLabel CEVerifiedLabel = (RadLabel)e.Item.FindControl("radmcesign");
                RadLabel CEReVerifiedLabel = (RadLabel)e.Item.FindControl("radmcereverfiedsign");

                RadLabel radmdate = (RadLabel)e.Item.FindControl("radmdate");
                RadLabel radmlocation = (RadLabel)e.Item.FindControl("radmlocation");
                RadLabel radmgas = (RadLabel)e.Item.FindControl("radmgas");
                RadLabel radmquantity = (RadLabel)e.Item.FindControl("radmquantity");
                RadLabel radmreason = (RadLabel)e.Item.FindControl("radmreason");
                RadLabel radmexplain = (RadLabel)e.Item.FindControl("radmexplain");

                int fldIsDeleted = dv["FLDISDELETED"].ToString() == "" ? 0 : Convert.ToInt32(dv["FLDISDELETED"].ToString());

                if (fldIsDeleted == 1)
                {
                    radmdate.Text = "<S>" + General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.DateTimeHR24)+"</S>";
                    radmlocation.Text = "<S>" + dv["FLDLOCATION"].ToString() + "</S>";
                    radmgas.Text = "<S>" + dv["FLDGASNAME"].ToString() + "</S>";
                    radmquantity.Text = "<S>" + dv["FLDQOREUSED"].ToString() + "</S>";
                    radmreason.Text = "<S>" + dv["FLDCHARGINGREASON"].ToString() + "</S>";
                    radmexplain.Text = "<S>" + dv["FLDMAINTDONE"].ToString() + "</S>";
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "1")
                {
                    radmdate.Text = General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.DateTimeHR24);
                    radmlocation.Text = "<S>" + dv["FLDOLDLOCATION"].ToString() + "</S></br>" + dv["FLDLOCATION"].ToString();
                    radmgas.Text = "<S>" + dv["FLDOLDGASNAME"].ToString() + "</S></br>" + dv["FLDGASNAME"].ToString();
                    radmquantity.Text = "<S>" + dv["FLDOLDQOREUSED"].ToString() + "</S></br>" + dv["FLDQOREUSED"].ToString();
                    radmreason.Text = "<S>" + dv["FLDOLDCHARGINGREASON"].ToString() + "</S></br>" + dv["FLDCHARGINGREASON"].ToString();
                    radmexplain.Text = "<S>" + dv["FLDOLDMAINTDONE"].ToString() + "</S></br>" + dv["FLDMAINTDONE"].ToString();
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "")
                {
                    radmdate.Text = General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.DateTimeHR24);
                    radmlocation.Text = dv["FLDLOCATION"].ToString();
                    radmgas.Text = dv["FLDGASNAME"].ToString();
                    radmquantity.Text = dv["FLDQOREUSED"].ToString();
                    radmreason.Text = dv["FLDCHARGINGREASON"].ToString();
                    radmexplain.Text = dv["FLDMAINTDONE"].ToString();
                }


                ImageButton Attachments = (ImageButton)e.Item.FindControl("btnmattachments");
                LinkButton Edit = (LinkButton)e.Item.FindControl("btnmedit");
                LinkButton Delete = (LinkButton)e.Item.FindControl("btnmdelete");
                RadLabel IsDeleted = (RadLabel)e.Item.FindControl("radmisdeleted");

                if (DataBinder.Eval(e.Item.DataItem, "FLDATTACHMENT").ToString() == "0")
                    Attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    Attachments.ImageUrl = Session["images"] + "/attachment.png";

                if (Attachments != null && CESignStatus.Text != "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + DtKey.Text + "&MOD=LOG&RefreshWindowName=Log" + "');return false;");
                }
                if (Attachments != null && CESignStatus.Text == "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + DtKey.Text + "&MOD=LOG&RefreshWindowName=Log" + "&u=n" + "');return false;");
                }

                if (IsDeleted.Text == "1" && CESignStatus.Text == "1")
                {
                    Attachments.Visible = false;
                    Edit.Visible = false;
                    Delete.Visible = false;
                }

                if (IsDeleted.Text == "1" && CESignStatus.Text != "1")
                {

                    Delete.Visible = false;
                }

                if (CESignStatus.Text == "1" && OICSignStatus.Text == "1")
                {
                    Lock.Visible = true;
                }
                else
                {
                    UnLock.Visible = true;

                }

                if (OICSignStatus.Text != "1")
                {
                    OICSign.Visible = true;
                    if (Status.Text == "2")
                    {
                        CEVerifySign.Visible = true;
                        CEVerifySign.Enabled = false;
                    }
                    if (Status.Text == "5")
                    {
                        CEReVerifySign.Visible = true;
                        CEReVerifySign.Enabled = false;
                    }

                }
                if (OICSignStatus.Text == "1" && CESignStatus.Text != "1")
                {
                    OICSignLabel.Visible = true;
                    if (Status.Text == "2")
                    {
                        CEVerifySign.Visible = true;

                    }
                    if (Status.Text == "5")
                    {
                        CEReVerifySign.Visible = true;

                    }

                }
                if (CESignStatus.Text == "1" && OICSignStatus.Text == "1")
                {
                    OICSignLabel.Visible = true;

                    if (Status.Text == "3")
                    {
                        CEVerifiedLabel.Visible = true;

                    }
                    if (Status.Text == "6")
                    {
                        CEReVerifiedLabel.Visible = true;

                    }

                    Edit.Visible = false;
                    Delete.Visible = false;
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

    protected void gvODSSD_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataTable dt = PhoenixMarpolLogODS.ODSRecordDSSearch(From, To, Status, gvODSSD.CurrentPageIndex + 1, gvODSSD.PageSize, ref iRowCount, ref iTotalPageCount);

            gvODSSD.DataSource = dt;
            gvODSSD.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvODSSD_ItemCommand(object sender, GridCommandEventArgs e)
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
            RadLabel Id = (RadLabel)e.Item.FindControl("radssdrid");
            RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radsdoicsignid");
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
            if (e.CommandName.ToUpper() == "CHIEFENGGSIGN")
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
                scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','Log/ElectronicLogAnnexVIODSSDAdd.aspx?recordid=" + Id.Text + "','false','610px','460px');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvODSSD_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;

                LinkButton Lock = (LinkButton)e.Item.FindControl("btnssdredlock");
                LinkButton UnLock = (LinkButton)e.Item.FindControl("btnssdgreenlock");
                RadLabel Id = (RadLabel)e.Item.FindControl("radssdrid");
                RadLabel DtKey = (RadLabel)e.Item.FindControl("sraddtkey");
                RadLabel IsBackDatedEntry = (RadLabel)e.Item.FindControl("radsdisbackdatedentry");

                LinkButton BackDatedEntry = (LinkButton)e.Item.FindControl("imgsdFlag");
                LinkButton OICSign = (LinkButton)e.Item.FindControl("radsdoicsignlink");
                LinkButton CEVerifySign = (LinkButton)e.Item.FindControl("radsdcesignveifylink");
                LinkButton CEReVerifySign = (LinkButton)e.Item.FindControl("radsdcesignreverifylink");

                RadLabel Status = (RadLabel)e.Item.FindControl("radssdstatus");
                RadLabel OICSignStatus = (RadLabel)e.Item.FindControl("radsdoicsignid");
                RadLabel CESignStatus = (RadLabel)e.Item.FindControl("radsdcesignid");
                RadLabel OICSignLabel = (RadLabel)e.Item.FindControl("radsdoicsign");
                RadLabel CEVerifiedLabel = (RadLabel)e.Item.FindControl("radsdcesign");
                RadLabel CEReVerifiedLabel = (RadLabel)e.Item.FindControl("radsdcereverfiedsign");
                RadLabel IsDeleted = (RadLabel)e.Item.FindControl("radsisdeleted");

                ImageButton Attachments = (ImageButton)e.Item.FindControl("btnsdattachments");
                LinkButton Edit = (LinkButton)e.Item.FindControl("btnsdedit");
                LinkButton Delete = (LinkButton)e.Item.FindControl("btnsddelete");

                RadLabel radssddate = (RadLabel)e.Item.FindControl("radssddate");
                RadLabel radsystemtype = (RadLabel)e.Item.FindControl("radsystemtype");
                RadLabel radsupply = (RadLabel)e.Item.FindControl("radsupply");
                RadLabel raddeli = (RadLabel)e.Item.FindControl("raddeli");
                RadLabel radnd = (RadLabel)e.Item.FindControl("radnd");
                RadLabel raddobs = (RadLabel)e.Item.FindControl("raddobs");
                RadLabel radsdremarks = (RadLabel)e.Item.FindControl("radsdremarks");

                int fldIsDeleted = dv["FLDISDELETED"].ToString() == "" ? 0 : Convert.ToInt32(dv["FLDISDELETED"].ToString());

                if (fldIsDeleted == 1)
                {
                    radssddate.Text = "<S>" + General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.DateTimeHR24) + "</S>";
                    radsystemtype.Text = "<S>" + dv["FLDSYSTYPE"].ToString() + "</S>";
                    radsupply.Text = "<S>" + dv["FLDODSSUPPLY"].ToString() + "</S>";
                    raddeli.Text = "<S>" + dv["FLDDELIBDIS"].ToString() + "</S>";
                    radnd.Text = "<S>" + dv["FLDNDELIBDIS"].ToString() + "</S>";
                    raddobs.Text = "<S>" + dv["FLDODSDISCHARGEATSHORE"].ToString() + "</S>";
                    radsdremarks.Text = "<S>" + dv["FLDREMARKS"].ToString() + "</S>";
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "1")
                {
                    radssddate.Text = General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.DateTimeHR24);
                    radsystemtype.Text = "<S>" + dv["FLDOLDSYSTYPE"].ToString() + "</S></br>" + dv["FLDSYSTYPE"].ToString();
                    radsupply.Text = "<S>" + dv["FLDOLDODSSUPPLY"].ToString() + "</S></br>" + dv["FLDODSSUPPLY"].ToString();
                    raddeli.Text = "<S>" + dv["FLDOLDDELIBDIS"].ToString() + "</S></br>" + dv["FLDDELIBDIS"].ToString();
                    radnd.Text = "<S>" + dv["FLDOLDNDELIBDIS"].ToString() + "</S></br>" + dv["FLDNDELIBDIS"].ToString();
                    raddobs.Text = "<S>" + dv["FLDOLDODSDISCHARGEATSHORE"].ToString() + "</S></br>" + dv["FLDODSDISCHARGEATSHORE"].ToString();
                    radsdremarks.Text = "<S>" + dv["FLDOLDREMARKS"].ToString() + "</S></br>" + dv["FLDREMARKS"].ToString();
                }
                else if (fldIsDeleted != 1 && dv["FLDISUPDATED"].ToString() == "")
                {
                    radssddate.Text = General.GetDateTimeToString(dv["FLDDATE"].ToString(), DateDisplayOption.DateTimeHR24);
                    radsystemtype.Text = dv["FLDSYSTYPE"].ToString();
                    radsupply.Text = dv["FLDODSSUPPLY"].ToString();
                    raddeli.Text = dv["FLDDELIBDIS"].ToString();
                    radnd.Text = dv["FLDNDELIBDIS"].ToString();
                    raddobs.Text = dv["FLDODSDISCHARGEATSHORE"].ToString();
                    radsdremarks.Text = dv["FLDREMARKS"].ToString();
                }

                if (DataBinder.Eval(e.Item.DataItem, "FLDATTACHMENT").ToString() == "0")
                    Attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    Attachments.ImageUrl = Session["images"] + "/attachment.png";

                if (Attachments != null && CESignStatus.Text != "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + DtKey.Text + "&MOD=LOG&RefreshWindowName=Log" + "');return false;");
                }
                if (Attachments != null && CESignStatus.Text == "1")
                {
                    Attachments.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + DtKey.Text + "&MOD=LOG&RefreshWindowName=Log" + "&u=n" + "');return false;");
                }
                if (IsDeleted.Text == "1" && CESignStatus.Text == "1")
                {
                    Attachments.Visible = false;
                    Edit.Visible = false;
                    Delete.Visible = false;
                }

                if (IsDeleted.Text == "1" && CESignStatus.Text != "1")
                {

                    Delete.Visible = false;
                }
                if (CESignStatus.Text == "1" && OICSignStatus.Text == "1")
                {
                    Lock.Visible = true;
                }
                else
                {
                    UnLock.Visible = true;

                }

                if (OICSignStatus.Text != "1")
                {
                    OICSign.Visible = true;
                    if (Status.Text == "2")
                    {
                        CEVerifySign.Visible = true;
                        CEVerifySign.Enabled = false;
                    }
                    if (Status.Text == "5")
                    {
                        CEReVerifySign.Visible = true;
                        CEReVerifySign.Enabled = false;
                    }

                }
                if (OICSignStatus.Text == "1" && CESignStatus.Text != "1")
                {
                    OICSignLabel.Visible = true;
                    if (Status.Text == "2")
                    {
                        CEVerifySign.Visible = true;

                    }
                    if (Status.Text == "5")
                    {
                        CEReVerifySign.Visible = true;

                    }

                }
                if (CESignStatus.Text == "1" && OICSignStatus.Text == "1")
                {
                    OICSignLabel.Visible = true;

                    if (Status.Text == "3")
                    {
                        CEVerifiedLabel.Visible = true;

                    }
                    if (Status.Text == "6")
                    {
                        CEReVerifiedLabel.Visible = true;

                    }

                    Edit.Visible = false;
                    Delete.Visible = false;

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

    private Dictionary<string, bool> GetAccessRights()
    {
        // get it from db and form a dictionary
        Dictionary<string, bool> accessrights = new Dictionary<string, bool>();
        string rankShortCode = PhoenixElog.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string designaionShortCode = null;
        bool isVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? true : false;
        bool isChiefEngg = PhoenixElog.validCheifEngineer(rankShortCode);
        DataSet ds = PhoenixMarpolLogODS.GetMarpolLogODSAccessRights(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID, rankShortCode, designaionShortCode, isVessel);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            accessrights.Add("VIEW", Convert.ToBoolean(row["FLDVIEW"]));
            accessrights.Add("ADD", Convert.ToBoolean(row["FLDENTRY"]));
            accessrights.Add("SIGN", Convert.ToBoolean(row["FLDSIGN"]));
            accessrights.Add("UNLOCK", Convert.ToBoolean(row["FLDUNLOCK"]));
            accessrights.Add("AMEND", Convert.ToBoolean(row["FLDAMEND"]));
            accessrights.Add("DELETE", Convert.ToBoolean(row["FLDAMEND"]));
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
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.CrewOffshore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SouthNests.Phoenix.Reports;
using System.Web;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewSignOn : PhoenixBasePage
{
    private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {try {document.getElementById('REQUEST_LASTFOCUS').focus();
            } catch (ex) {}}";
    int? fldSignOnId, fldSelectedSignOnId;
    string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                toolbar.AddButton("Send", "SAVE", ToolBarDirection.Right);
            else
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSignOn.AccessRights = this.ViewState;
            MenuSignOn.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOn.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewSignOn')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            HookOnFocus(this.Page as Control);

            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;


            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                CalculateReliefDue(null, null);
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                SetEmployeePrimaryDetails();
                txtSignOnOffId.Text = "";
                CrewSignOnEdit(null, null);
                ViewState["SIGNONID"] = 0;
                //ucConfirm.Visible = false;
                Page.ClientScript.RegisterStartupScript(typeof(CrewSignOn), "ScriptDoFocus", SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]), true);
                if (!string.IsNullOrEmpty(Request.QueryString["ds"]) && Request.QueryString["ds"] == "1")
                    ddlVessel.Enabled = true;

                ViewState["SAVE"] = "1";
                gvCrewSignOn.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void HookOnFocus(Control CurrentControl)
    {
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList))

            (CurrentControl as WebControl).Attributes.Add(
               "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        if (CurrentControl.HasControls())

            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    private void CrewSignOnEdit(int? signonid, int? status)
    {
        DataTable dt = PhoenixCrewSignOnOff.CrewSignOnEdit(General.GetNullableInteger(strEmployeeId).Value, signonid, status, General.GetNullableInteger(Request.QueryString["vslid"]));

        if (dt.Rows.Count > 0)
        {

            txtSignOnOffId.Text = dt.Rows[0]["FLDSIGNONOFFID"].ToString();
            ddlRank.SelectedRank = "";
            ddlRank.SelectedRank = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
            ddlVessel.SelectedVessel = "";
            ddlVessel.SelectedVessel = dt.Rows[0]["FLDSIGNONVESSELID"].ToString();
            ddlPort.SelectedSeaport = "";
            ddlPort.SelectedSeaport = dt.Rows[0]["FLDSIGNONSEAPORTID"].ToString();
            ddlCompany.SelectedAddress = "";
            ddlCompany.SelectedAddress = dt.Rows[0]["FLDMANAGERID"].ToString();
            ddlPool.SelectedPool = dt.Rows[0]["FLDPOOLID"].ToString();
            ddlSignOnReason.SelectedSignOnReason = "";
            ddlSignOnReason.SelectedSignOnReason = dt.Rows[0]["FLDSIGNONREASONID"].ToString();
            ddlExtraCrew.SelectedValue = "";
            ddlExtraCrew.SelectedValue = dt.Rows[0]["FLDEXTRACREWON"].ToString();
            if (General.GetNullableInteger(dt.Rows[0]["FLDAPPRAISALYN"].ToString()) == null)
                chkAppraisalYN.Checked = true;
            else if (dt.Rows[0]["FLDAPPRAISALYN"].ToString() == "1")
                chkAppraisalYN.Checked = true;
            else
                chkAppraisalYN.Checked = false;
            chkAppraisalYN.Enabled = SessionUtil.CanAccess(this.ViewState, "APPRAISALYN");
            txtBeginToD.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDBTOD"].ToString())));
            txtSignOnDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDSIGNONDATE"].ToString())));
            if (dt.Rows[0]["FLDDURATION"].ToString() != "")
            {
                txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
            }
            txtContract.Text = dt.Rows[0]["FLDCONTRACT"].ToString();
            txtReliefDueDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDRELIEFDUEDATE"].ToString())));
            txtSignonRemarks.Text = dt.Rows[0]["FLDSIGNONREMARKS"].ToString();
            ddlSignOn.Vessel = 0;
            ddlSignOn.Vessel = General.GetNullableInteger(dt.Rows[0]["FLDSIGNONVESSELID"].ToString());
            ddlSignOn.Rank = 0;
            ddlSignOn.Rank = General.GetNullableInteger(dt.Rows[0]["FLDSIGNONRANKID"].ToString());
            ddlSignOn.SelectedCrew = "";
            ddlSignOn.SelectedCrew = dt.Rows[0]["FLDRELIEVERID"].ToString();
            ucCountry.SelectedCountry = "";
            ucCountry.SelectedCountry = dt.Rows[0]["FLDCOUNTRYCODE"].ToString();
            if (dt.Rows[0]["FLDJOINWITHPREVIOUSMEDICAL"].ToString() == "1")
                chkJoinWithPrevMed.Checked = true;
            ViewState["status"] = dt.Rows[0]["FLDSTATUS"].ToString();
            if (ViewState["status"].ToString() == "1")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ds"]) && Request.QueryString["ds"] == "1")
                    ddlVessel.Enabled = true;
                else
                    ddlVessel.Enabled = false;
                ucCountry.Enabled = false;
                ddlRank.Enabled = false;
                txtDuration.ReadOnly = true;
                txtDuration.CssClass = "readonlytextbox";
                txtReliefDueDate.ReadOnly = true;
                txtReliefDueDate.CssClass = "readonlytextbox";
                ddlSignOnReason.Enabled = false;
                ddlSignOn.Enabled = false;
                ddlSignOn.Enabled = false;
                ddlExtraCrew.Enabled = false;
                txtSignonRemarks.ReadOnly = true;
                txtSignonRemarks.CssClass = "readonlytextbox";
                //   MenuSignOn.Visible = true;      
                chkJoinWithPrevMed.Enabled = false;
                ViewState["SAVE"] = 1;
            }
            else if (ViewState["status"].ToString() == "0")
            {
                //MenuSignOn.Visible = false;
                ViewState["SAVE"] = 0;
            }
            if (dt.Rows[0]["FLDFLAG"].ToString() == "1")
            {
                txtDuration.Text = dt.Rows[0]["FLDCONTRACTDURATION"].ToString();
                txtSignOnDate.Text = dt.Rows[0]["FLDCONTRACTSIGNONDATE"].ToString();
                CalculateReliefDue(null, null);
            }
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONRANK", "FLDSIGNONREASON", "FLDSIGNONDATE" };
        string[] alCaptions = { "Vessel", "Rank", "Reason", "Sign On Date" };

        DataSet ds;

        ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), null);

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew SignOn.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Sign On</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindData()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONRANK", "FLDSIGNONREASON", "FLDSIGNONDATE" };
        string[] alCaptions = { "Vessel", "Sign On Rank", "Sign On Reason", "Sign On Date" };

        DataSet ds;
        ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), null);
        General.SetPrintOptions("gvCrewSignOn", "Crew Sign On", alCaptions, alColumns, ds);
        gvCrewSignOn.DataSource = ds;
        gvCrewSignOn.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void ReasonExtraCrew(object sender, EventArgs e)
    {
        if (ddlExtraCrew.SelectedValue != "0")
            ddlReasonExtraCrew.CssClass = "dropdown_mandatory";
        else
            ddlReasonExtraCrew.CssClass = "input";
    }
    protected void FilterSeaport(object sender, EventArgs e)
    {
        ddlPort.SeaportList = PhoenixRegistersSeaport.EditSeaport(null, General.GetNullableInteger(ucCountry.SelectedCountry));
    }
    protected void OnBeginTravelClick(object sender, EventArgs e)
    {
        if (chkBeginTravel.Checked == true)
            txtBeginToD.Text = txtSignOnDate.Text;
        else txtBeginToD.Text = "";
    }
    protected void Rebind()
    {
        gvCrewSignOn.SelectedIndexes.Clear();
        gvCrewSignOn.EditIndexes.Clear();
        gvCrewSignOn.DataSource = null;
        gvCrewSignOn.Rebind();
    }
    protected void btnSignOn_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidCrewSignOn())
            {
                ucError.Visible = true;
                return;

            }
            SaveSignOn();
            CrewSignOnEdit(null, null);
            Rebind();

            if (Request.QueryString["r"] != null)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList(null,'ifMoreInfo','keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewSignOn_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCrewSignOn())
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to save the record?", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SaveSignOn()
    {
        try
        {
            if (txtSignOnOffId.Text != "")
            {
                fldSignOnId = int.Parse(txtSignOnOffId.Text);
            }
            if (!string.IsNullOrEmpty(ddlSignOn.SelectedCrew))
            {
                fldSelectedSignOnId = int.Parse(ddlSignOn.SelectedCrew);
            }
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
            {
                PhoenixCrewOffshoreCrewChange.CrewSignOnInsert(fldSignOnId,
                                                                    General.GetNullableInteger(strEmployeeId),
                                                                    int.Parse(ddlRank.SelectedRank),
                                                                    int.Parse(ddlVessel.SelectedVessel),
                                                                    fldSelectedSignOnId, int.Parse(ddlPort.SelectedSeaport)
                                                                    , General.GetNullableInteger(null), int.Parse(ddlSignOnReason.SelectedSignOnReason),
                                                                    DateTime.Parse(txtBeginToD.Text)
                                                                    , DateTime.Parse(txtSignOnDate.Text), decimal.Parse(txtDuration.Text), txtSignonRemarks.Text, int.Parse(ddlExtraCrew.SelectedValue)
                                                                    , null//chkAdditionalRank.Checked ? byte.Parse("1") : byte.Parse("0")
                                                                    , General.GetNullableDateTime(txtReliefDueDate.Text)
                                                                    , chkJoinWithPrevMed.Checked == true ? 1 : 0
                                                                    , General.GetNullableInteger(ddlReasonExtraCrew.SelectedValue)
                                                                    , General.GetNullableInteger(chkAppraisalYN.Checked == true ? "1" : "0")
                                                                    , General.GetNullableInteger(Request.QueryString["trainingmatrixid"].ToString())
                                                                    );
            }
            else
            {
                PhoenixCrewSignOnOff.CrewSignOnInsert(fldSignOnId,
                                                                    General.GetNullableInteger(strEmployeeId),
                                                                    int.Parse(ddlRank.SelectedRank),
                                                                    int.Parse(ddlVessel.SelectedVessel),
                                                                    fldSelectedSignOnId, int.Parse(ddlPort.SelectedSeaport)
                                                                    , General.GetNullableInteger(null), int.Parse(ddlSignOnReason.SelectedSignOnReason),
                                                                    DateTime.Parse(txtBeginToD.Text)
                                                                    , DateTime.Parse(txtSignOnDate.Text), decimal.Parse(txtDuration.Text), txtSignonRemarks.Text, int.Parse(ddlExtraCrew.SelectedValue)
                                                                    , null//chkAdditionalRank.Checked ? byte.Parse("1") : byte.Parse("0")
                                                                    , General.GetNullableDateTime(txtReliefDueDate.Text)
                                                                    , chkJoinWithPrevMed.Checked == true ? 1 : 0
                                                                    , General.GetNullableInteger(ddlReasonExtraCrew.SelectedValue)
                                                                    , General.GetNullableInteger(chkAppraisalYN.Checked == true ? "1" : "0")
                                                                    );
            }
            PhoenixVesselAccountsEmployee.SendCrewDataToVessel(General.GetNullableInteger(strEmployeeId).Value, int.Parse(ddlVessel.SelectedVessel), null);
            AttachPD(General.GetNullableInteger(strEmployeeId), int.Parse(ddlRank.SelectedRank), int.Parse(ddlVessel.SelectedVessel));
            ucStatus.Text = "Sign on Information Updated";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void AttachPD(int? EmployeeID, int Rank, int Vessel)
    {

        string Tmpfilelocation = string.Empty;
        string Tmpfilelocation1 = string.Empty;

        string reportcode = string.Empty;
        DataSet ds = new DataSet();
        ds = PhoenixCrewLicenceRequest.EmployeeCrewGet(Convert.ToInt64(EmployeeID), Rank, Vessel, ref reportcode);

        string reportcode1 = string.Empty;
        DataSet ds1 = new DataSet();
        ds1 = PhoenixCrewLicenceRequest.PDOwnerGet(Convert.ToInt64(EmployeeID), Rank, Vessel, ref reportcode1);

        string[] reportfile = new string[3];

        string[] reportfile1 = new string[2];

        if (reportcode.Equals("PDSENIOROFFICER"))
        {
            foreach (string s in reportfile)
            {
                reportfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportsCrewPDSeniorOfficer.rpt");
                reportfile[1] = "ReportsCrewPDSub.rpt";
                reportfile[2] = "ReportsCrewPDService.rpt";
            }
        }
        else if (reportcode.Equals("PDJUNIOROFFICER"))
        {
            reportfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportsCrewPDJuniorOfficer.rpt");
            reportfile[1] = "ReportsCrewPDSub.rpt";
            reportfile[2] = "ReportsCrewPDService.rpt";
        }
        else if (reportcode.Equals("PDRATINGS"))
        {
            reportfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportsCrewPDRatings.rpt");
            reportfile[1] = "ReportsCrewPDSub.rpt";
            reportfile[2] = "ReportsCrewPDService.rpt";
        }

        if (reportcode1.Equals("PDOWNERDECK"))
        {
            reportfile1[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsCrewPDDeck.rpt");
            reportfile1[1] = "ReportsCrewPDCoursesSub.rpt";
        }
        else if (reportcode1.Equals("PDOWNERENGINE"))
        {
            reportfile1[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsCrewPDEngine.rpt");
            reportfile1[1] = "ReportsCrewPDCoursesSub.rpt";
        }

        //Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
        //Tmpfilelocation1 = System.Web.HttpContext.Current.Request.MapPath("~/");

        Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
        Tmpfilelocation1 = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;

        string fileid = PhoenixCommonFileAttachment.GenerateDTKey();
        string fileid1 = PhoenixCommonFileAttachment.GenerateDTKey();

        string filename = fileid + ".pdf";
        string filename1 = fileid1 + ".pdf";

        Guid attachmentcode = new Guid(ds.Tables[0].Rows[0]["FLDINTERVIEWSUMMARYFLDDTKEY"].ToString());
        //Tmpfilelocation = Tmpfilelocation + "Attachments/Crew/" + filename;
        //Tmpfilelocation1 = Tmpfilelocation1 + "Attachments/Crew/" + filename1;

        Tmpfilelocation = Tmpfilelocation + "/Crew/" + filename;
        Tmpfilelocation1 = Tmpfilelocation1 + "/Crew/" + filename1;

        PhoenixReportClass.ExportReport(reportfile, Tmpfilelocation, ds);
        PhoenixReportClass.ExportReport(reportfile1, Tmpfilelocation1, ds1);

        FileInfo fi = new FileInfo(Tmpfilelocation);
        FileInfo fi1 = new FileInfo(Tmpfilelocation1);

        PhoenixCommonFileAttachment.InsertAttachment(attachmentcode, "PDFORM.PDF", "CREW/" + filename, fi.Length, 0, 0, "ASSESSMENT", new Guid(fileid));
        PhoenixCommonFileAttachment.InsertAttachment(attachmentcode, "OWNERPD.PDF", "CREW/" + filename1, fi1.Length, 0, 0, "ASSESSMENT", new Guid(fileid1));

    }
    protected void CalculateReliefDue(object sender, EventArgs e)
    {
        UserControlDate d = sender as UserControlDate;
        string id = string.Empty;
        if (sender != null)
            id = ((Control)sender).ID;
        if (e != null)
        {
            if (txtReliefDueDate.Text != null && txtSignOnDate.Text != null && id == "txtReliefDueDate")
            {
                DateTime sg = Convert.ToDateTime(txtSignOnDate.Text);
                DateTime rd = Convert.ToDateTime(txtReliefDueDate.Text);
                TimeSpan s = rd - sg;
                int isnegative = s.Days;

                double x = isnegative / 30.00;
                txtDuration.Text = Convert.ToString(x);

            }
            else if (txtDuration.Text != "" && txtSignOnDate.Text != null && id == "txtDuration")
            {
                double months = Convert.ToDouble(txtDuration.Text);
                int days = Convert.ToInt32(30 * months);
                txtReliefDueDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(txtSignOnDate.Text).AddDays(days));
            }
        }

    }
    protected void MappedManagerPool(object sender, EventArgs e)
    {
        UserControlCommonVessel vsl = sender as UserControlCommonVessel;
        if (vsl != null)
        {
            if (vsl.SelectedVessel.ToUpper() != "DUMMY")
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vsl.SelectedVessel));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drManagerPool = ds.Tables[0].Rows[0];
                    ddlCompany.SelectedAddress = drManagerPool["FLDPRIMARYMANAGER"].ToString();
                    ddlPool.SelectedPool = drManagerPool["FLDOFFICERPOOL"].ToString();
                }
            }
        }
        ddlSignOn.OnboardList = PhoenixCrewManagement.ListCrewOnboard(General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlRank.SelectedRank));
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    private bool IsValidCrewSignOn()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        int compareresult;
        if (ViewState["SAVE"].ToString() == "0")
        {
            ucError.ErrorMessage = "You cannot update this record";
        }
        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";
        }

        if (ddlRank.SelectedRank.Trim().Equals("Dummy") || ddlRank.SelectedRank.Trim().Equals(""))
            ucError.ErrorMessage = "Rank is required.";
        if (ddlVessel.SelectedVessel.Trim().Equals("Dummy") || ddlVessel.SelectedVessel.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel  is required.";

        if (ddlPort.SelectedSeaport.Trim().Equals("Dummy") || ddlPort.SelectedSeaport.Trim().Equals(""))
            ucError.ErrorMessage = "Sign-On Port is required.";

        if (ddlSignOnReason.SelectedSignOnReason.Trim().Equals("Dummy") || ddlSignOnReason.SelectedSignOnReason.Trim().Equals(""))
            ucError.ErrorMessage = "Reason on is required.";

        if (!DateTime.TryParse(txtSignOnDate.Text, out resultDate))
            ucError.ErrorMessage = "Sign-On Date is required.";
        else if (DateTime.TryParse(txtSignOnDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-On Date should be earlier than current date";
        }
        if (!DateTime.TryParse(txtBeginToD.Text, out resultDate))
            ucError.ErrorMessage = "Begin ToD/Travel to VSL. is required.";
        else if (DateTime.TryParse(txtSignOnDate.Text, out resultDate) && DateTime.TryParse(txtBeginToD.Text, out resultDate))
        {
            compareresult = DateTime.Compare(resultDate, DateTime.Parse(txtSignOnDate.Text));
            if (chkBeginTravel.Checked == true)
            {
                if (compareresult > 0)
                    ucError.ErrorMessage = "Begin ToD/Travel to VSL. be earlier or than SignOn Date";
            }
            else
            {
                if (compareresult > 0 || compareresult == 0)
                    ucError.ErrorMessage = "Begin ToD/Travel to VSL. be earlier SignOn Date";
            }
        }
        if (txtDuration.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Duration is required.";

        if (!string.IsNullOrEmpty(txtReliefDueDate.Text))
        {
            if (DateTime.TryParse(txtSignOnDate.Text, out resultDate) && DateTime.TryParse(txtReliefDueDate.Text, out resultDate))
                if (Convert.ToDateTime(txtSignOnDate.Text) > Convert.ToDateTime(txtReliefDueDate.Text))
                    ucError.ErrorMessage = "Relief Due Date should be later than sign on date.";
        }

        if (General.GetNullableInteger(ddlExtraCrew.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Extra Crew Onboard is Required.";
        }
        else if (General.GetNullableInteger(ddlExtraCrew.SelectedValue) != 0)
        {
            if (General.GetNullableInteger(ddlReasonExtraCrew.SelectedValue) == null)
            {
                ucError.ErrorMessage = "Reason for Extra Crew Onboard is Required.";
            }
        }
        return (!ucError.IsError);

    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString()));
                txtLastVesselRank.Text = dt.Rows[0]["FLDLASTVESSELRANK"].ToString();
                txtActivity.Text = dt.Rows[0]["FLDACTIVITYNAME"].ToString() + ' ' + dt.Rows[0]["FLDACTIVITYDATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSignOn_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            if (e.CommandName.ToUpper() == "REDIRECT")
            {
                string signonid = ((RadLabel)e.Item.FindControl("lblSignOnId")).Text;
                CrewSignOnEdit(General.GetNullableInteger(signonid), 1);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSignOn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }


}

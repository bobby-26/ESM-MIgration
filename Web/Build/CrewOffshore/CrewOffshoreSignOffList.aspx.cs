using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshoreSignOffList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSignOffList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSignOffList.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSignOffList.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../CrewOffshore/CrewOffshoreBulkSignOnOff.aspx?type=signon'); return false;", "Bulk Sign On", "bulk_save.png", "BULK");
            CrewSignOn.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSignOffList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSignOff')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../CrewOffshore/CrewOffshoreBulkSignOnOff.aspx?type=signoff'); return false;", "Bulk Sign Off", "bulk_save.png", "BULK");
            CrewSignOff.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERSO"] = 1;

                ViewState["SORTEXPRESSIONSO"] = null;
                ViewState["SORTDIRECTIONSO"] = null;

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvSignOff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigatorSO();
            //BindDataSignOff();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewSignOn_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelSO();

                //string[] alColumns = { "FLDNAME", "FLDNATIONALITYNAME", "FLDSIGNONRANKNAME", "FLDFILENO", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME" };
                //string[] alCaptions = { "Name", "Nationality", "Sign on Rank", "File No", "Passport No", "CDC No", "Sign on Date", "Sign-On Port" };

                //DataTable dt = PhoenixCrewOffshoreReliefRequest.ListVesselSignOn(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                //General.ShowExcel("Crew Sign-On List", dt, alColumns, alCaptions, null, string.Empty);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshoreSignOffCriteria = null;
                BindData();
                gvCrewSearch.Rebind();
                BindDataSignOff();
                gvSignOff.Rebind();
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreSignOffListFilter.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewSignOff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelSOff();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? vesselid = null;

            string[] alColumns = new string[0];
            string[] alCaptions = new string[0];
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO",
                                  "FLDSIGNONDATE","FLDSIGNONREASON", "FLDSIGNONSEAPORTNAME", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
                alCaptions = new string[] { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No",  "Sign On Date", "Sign-On Reason",
                                                    "Sign-On Port", "End of Contract", "Max Tour of Duty" };
            }
            else
            {
                alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDCURRENCYCODE",
                                 "FLDDAILYRATE", "FLDDPALLOWANCE", "FLDSIGNONDATE","FLDSIGNONREASON", "FLDSIGNONSEAPORTNAME", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
                alCaptions = new string[]  { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No","Currency" ,"Daily Rate", "Daily DP Allowance", "Sign On Date", "Sign-On Reason",
                                  "Sign-On Port", "End of Contract", "Max Tour of Duty" };
            }

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSIONSO"] == null) ? null : (ViewState["SORTEXPRESSIONSO"].ToString());
            if (ViewState["SORTDIRECTIONSO"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSO"].ToString());

            if (ViewState["ROWCOUNTSO"] == null || Int32.Parse(ViewState["ROWCOUNTSO"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNTSO"].ToString());

            NameValueCollection nvc = Filter.CurrentOffshoreSignOffCriteria;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                // nvc = null;
            }

            if (nvc != null)
            {
                if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                    vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
            }

            DataTable dt = PhoenixCrewOffshoreReliefRequest.SearchSignOnList(vesselid
                                                                , nvc != null ? nvc.Get("txtFileNo") : null
                                                                , nvc != null ? nvc.Get("txtName") : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucRank"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignonFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignonToDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueToDate"] : string.Empty)
                                                                , (int)ViewState["PAGENUMBERSO"], gvCrewSearch.PageSize
                                                                , ref iRowCount, ref iTotalPageCount
                                                                , sortexpression, sortdirection
                                                              );
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Sign-On List", alCaptions, alColumns, ds);


            if (dt.Rows.Count > 0)
            {

                gvCrewSearch.DataSource = dt;
               // CheckSelectedAllSignOn();
            }
            else
            {
                gvCrewSearch.DataSource = dt;
            }

            gvCrewSearch.VirtualItemCount = iRowCount;
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                gvCrewSearch.MasterTableView.GetColumn("Currency").Visible = false;
                gvCrewSearch.MasterTableView.GetColumn("DailyRate").Visible = false;
                // gvCrewSearch.Columns[8].Visible = false;
            }

            ViewState["ROWCOUNTSO"] = iRowCount;
            ViewState["TOTALPAGECOUNTSO"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtSignOnDate_TextChanged(object sender, EventArgs e)
    {
        UserControlDate txtSignondate = (UserControlDate)sender;
        GridDataItem row = (GridDataItem)txtSignondate.Parent.Parent;
        txtSignondate = (UserControlDate)row.FindControl("txtSignOnDate");
        RadLabel lbl90ReliefDateEdit = (RadLabel)row.FindControl("lbl90ReliefDateEdit");
        //Label lblInspectionIdEdit = (Label)row.FindControl("lbl90ReliefDate");

        if (txtSignondate != null && General.GetNullableDateTime(txtSignondate.Text) != null)
        {
            DateTime dtSignOn = Convert.ToDateTime(txtSignondate.Text);
            DateTime dt90Relief = dtSignOn.AddDays(90);
            lbl90ReliefDateEdit.Text = General.GetDateTimeToString(dt90Relief);
        }
    }
    protected void txtSignOffDate_TextChanged(object sender, EventArgs e)
    {
        UserControlDate txtSignondate = (UserControlDate)sender;
        GridDataItem row = (GridDataItem)txtSignondate.Parent.Parent;
        txtSignondate = (UserControlDate)row.FindControl("txtSignOnDate");
        RadLabel lbl90ReliefDateEdit = (RadLabel)row.FindControl("lbl90ReliefDate");
        //Label lblInspectionIdEdit = (Label)row.FindControl("lbl90ReliefDate");

        if (txtSignondate != null && General.GetNullableDateTime(txtSignondate.Text) != null)
        {
            DateTime dtSignOn = Convert.ToDateTime(txtSignondate.Text);
            DateTime dt90Relief = dtSignOn.AddDays(90);
            lbl90ReliefDateEdit.Text = General.GetDateTimeToString(dt90Relief);
        }
    }

    public void BindDataSignOff()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? vesselid = null;

            string[] alColumns = new string[0];
            string[] alCaptions = new string[0];
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO",
                                     "FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDSIGNOFFREASON", "FLDSIGNOFFSEAPORTNAME", "FLDETOD", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
                alCaptions = new string[] { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No",  "Sign On Date", "Sign-Off Date","Sign-Off Reason",
                                      "Sign-Off Port","Estimated Travel End Date", "End of Contract", "Max Tour of Duty" };
            }
            else
            {
                alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDCURRENCYCODE",
                                     "FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDSIGNOFFREASON", "FLDSIGNOFFSEAPORTNAME", "FLDETOD", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
                alCaptions = new string[]{ "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No","Currency", "Daily Rate", "DailyDPAllowance", "Sign On Date", "Sign-Off Date","Sign-Off Reason",
                                      "Sign-Off Port","Estimated Travel End Date", "End of Contract", "Max Tour of Duty" };
            }

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentOffshoreSignOffCriteria;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                // nvc = null;
            }

            if (nvc != null)
            {
                if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                    vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
            }

            DataTable dt = PhoenixCrewOffshoreReliefRequest.SearchSignOffList(vesselid
                                                                , nvc != null ? nvc.Get("txtFileNo") : null
                                                                , nvc != null ? nvc.Get("txtName") : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucRank"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignonFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignonToDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueToDate"] : string.Empty)
                                                                , (int)ViewState["PAGENUMBER"], gvSignOff.PageSize
                                                                , ref iRowCount, ref iTotalPageCount
                                                                , sortexpression, sortdirection
                                                              );
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvSignOff", "Sign-Off List", alCaptions, alColumns, ds);

            gvSignOff.DataSource = dt;
            gvSignOff.VirtualItemCount = iRowCount;

           

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                gvSignOff.MasterTableView.GetColumn("Currency").Visible = false;
                gvSignOff.MasterTableView.GetColumn("DailyRate").Visible = false;
                //gvSignOff.Columns[7].Visible = false;
                //gvSignOff.Columns[8].Visible = false;
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelSO()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO",
                                  "FLDSIGNONDATE","FLDSIGNONREASON", "FLDSIGNONSEAPORTNAME", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
            alCaptions = new string[] { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No",  "Sign On Date", "Sign-On Reason",
                                                    "Sign-On Port", "End of Contract", "Max Tour of Duty" };
        }
        else
        {
            alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO","FLDCURRENCYCODE",
                                 "FLDDAILYRATE", "FLDDPALLOWANCE", "FLDSIGNONDATE","FLDSIGNONREASON", "FLDSIGNONSEAPORTNAME", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
            alCaptions = new string[]  { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No","Currency", "Daily Rate", "Daily DP Allowance", "Sign On Date", "Sign-On Reason",
                                  "Sign-On Port", "End of Contract", "Max Tour of Duty" };
        }

        if (ViewState["ROWCOUNTSO"] == null || Int32.Parse(ViewState["ROWCOUNTSO"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTSO"].ToString());

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONSO"] == null) ? null : (ViewState["SORTEXPRESSIONSO"].ToString());
        if (ViewState["SORTDIRECTIONSO"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSO"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreSignOffCriteria;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreReliefRequest.SearchSignOnList(vesselid
                                                            , nvc != null ? nvc.Get("txtFileNo") : null
                                                            , nvc != null ? nvc.Get("txtName") : null
                                                            , General.GetNullableInteger(nvc != null ? nvc["ucRank"] : string.Empty)
                                                            , General.GetNullableDateTime(nvc != null ? nvc["txtSignonFromDate"] : string.Empty)
                                                            , General.GetNullableDateTime(nvc != null ? nvc["txtSignonToDate"] : string.Empty)
                                                            , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueFromDate"] : string.Empty)
                                                            , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueToDate"] : string.Empty)
                                                            , (int)ViewState["PAGENUMBER"], iRowCount
                                                            , ref iRowCount, ref iTotalPageCount
                                                            , sortexpression, sortdirection
                                                          );
        General.ShowExcel("Sign-On List", dt, alColumns, alCaptions, null, string.Empty);
    }

    protected void ShowExcelSOff()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO",
                                     "FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDSIGNOFFREASON", "FLDSIGNOFFSEAPORTNAME", "FLDETOD", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
            alCaptions = new string[] { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No",  "Sign On Date", "Sign-Off Date","Sign-Off Reason",
                                      "Sign-Off Port","Estimated Travel End Date", "End of Contract", "Max Tour of Duty" };
        }
        else
        {
            alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDCURRENCYCODE",
                                     "FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDSIGNOFFREASON", "FLDSIGNOFFSEAPORTNAME", "FLDETOD", "FLDRELIEFDUEDATE", "FLD90RELIEFDATE" };
            alCaptions = new string[]{ "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No","Currency", "Daily Rate", "DailyDPAllowance", "Sign On Date", "Sign-Off Date","Sign-Off Reason",
                                      "Sign-Off Port","Estimated Travel End Date", "End of Contract", "Max Tour of Duty" };
        }

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreSignOffCriteria;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreReliefRequest.SearchSignOffList(vesselid
                                                                , nvc != null ? nvc.Get("txtFileNo") : null
                                                                , nvc != null ? nvc.Get("txtName") : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucRank"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignonFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignonToDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtReliefDueToDate"] : string.Empty)
                                                                , 1, iRowCount
                                                                , ref iRowCount, ref iTotalPageCount
                                                                , sortexpression, sortdirection
                                                              );

        General.ShowExcel("Sign-Off List", dt, alColumns, alCaptions, null, string.Empty);
    }

  
    
 

    private bool IsValidSignOn(string date, string SeaPort, string reliefdate, string signonreason)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        DateTime signondate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-On Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(signonreason).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Reason is required.";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Sea Port is required.";
        }
        //if (General.GetNullableDateTime(reliefdate) == null)
        //    ucError.ErrorMessage = "Relief due date is required.";
        if (DateTime.TryParse(reliefdate, out resultDate) && DateTime.TryParse(date, out signondate) && DateTime.Compare(resultDate, signondate) < 0)
        {
            ucError.ErrorMessage = "Relief due date should be later than sign on date";
        }


        return (!ucError.IsError);
    }

    private bool IsValidSignOff(string date, string SeaPort, string signoffreason, string travelenddate, string signondate, string reliefdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        DateTime signdate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign-Off Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-Off Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(signoffreason).HasValue)
        {
            ucError.ErrorMessage = "Sign-Off Reason is required.";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign-Off Sea Port is required.";
        }
        //if (!General.GetNullableDateTime(travelenddate).HasValue)
        //{
        //    ucError.ErrorMessage = "Estimated Travel End Date is required.";
        //}
        if (!General.GetNullableDateTime(signondate).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Date is required.";
        }
        if (General.GetNullableDateTime(reliefdate) == null)
            ucError.ErrorMessage = "Relief due date is required.";
        else if (DateTime.TryParse(reliefdate, out resultDate) && DateTime.TryParse(signondate, out signdate) && DateTime.Compare(resultDate, signdate) < 0)
        {
            ucError.ErrorMessage = "Relief due date should be later than sign on date";
        }
        return (!ucError.IsError);
    }




    // Sign On

    //protected void CheckAllSignOn(Object sender, EventArgs e)
    //{
    //    string[] ctl = Request.Form.GetValues("__EVENTTARGET");

    //    if (ctl != null && ctl[0].ToString() == "gvCrewSearch$ctl01$chkAllSignOn")
    //    {
    //        CheckBox chkAll = (CheckBox)gvCrewSearch.HeaderRow.FindControl("chkAllSignOn");
    //        foreach (GridViewRow row in gvCrewSearch.Rows)
    //        {
    //            CheckBox cbSelected = (CheckBox)row.FindControl("chkSignOnSelect");
    //            if (cbSelected != null)
    //            {
    //                if (chkAll.Checked == true)
    //                {
    //                    cbSelected.Checked = true;
    //                }
    //                else
    //                {
    //                    cbSelected.Checked = false;
    //                }
    //            }
    //        }
    //    }
    //}

    //protected void SaveCheckedSignOnValues(Object sender, EventArgs e)
    //{
    //    ArrayList SelectedSignOn = new ArrayList();
    //    Int64 index;
    //    foreach (GridViewRow gvrow in gvCrewSearch.Rows)
    //    {
    //        bool result = false;
    //        index = Int64.Parse(gvCrewSearch.DataKeys[gvrow.RowIndex].Value.ToString());

    //        if (((CheckBox)(gvrow.FindControl("chkSignOnSelect"))).Checked == true)
    //        {
    //            result = true;// ((CheckBox)gvrow.FindControl("chkSignOnSelect")).Checked;
    //        }

    //        // Check in the Session
    //        if (Session["SIGNON_CHECKED_ITEMS"] != null)
    //            SelectedSignOn = (ArrayList)Session["SIGNON_CHECKED_ITEMS"];
    //        if (result)
    //        {
    //            if (!SelectedSignOn.Contains(index))
    //                SelectedSignOn.Add(index);
    //        }
    //        else
    //            SelectedSignOn.Remove(index);
    //    }
    //    if (SelectedSignOn != null && SelectedSignOn.Count > 0)
    //        Session["SIGNON_CHECKED_ITEMS"] = SelectedSignOn;
    //}

    //private void CheckSelectedAllSignOn()
    //{
    //    if (Session["SIGNON_CHECKED_ITEMS"] != null)
    //    {
    //        for (int i = 0; i < gvCrewSearch.Rows.Count; i++)
    //        {
    //            ArrayList SelectedSignOn = new ArrayList();
    //            SelectedSignOn = (ArrayList)Session["SIGNON_CHECKED_ITEMS"];
    //            if (SelectedSignOn != null && SelectedSignOn.Count > 0)
    //            {
    //                foreach (Int64 index in SelectedSignOn)
    //                {
    //                    if (gvCrewSearch.DataKeys[i].Value.ToString().Equals(index.ToString()))
    //                    {
    //                        CheckBox cbSelected = (CheckBox)gvCrewSearch.Rows[i].FindControl("chkSignOnSelect");
    //                        if (cbSelected != null)
    //                        {
    //                            cbSelected.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    // Sign Off

    //protected void CheckAllSignOff(Object sender, EventArgs e)
    //{
    //    string[] ctl = Request.Form.GetValues("__EVENTTARGET");

    //    if (ctl != null && ctl[0].ToString() == "gvSignOff$ctl01$chkAllSignOff")
    //    {
    //        CheckBox chkAll = (CheckBox)gvSignOff.HeaderRow.FindControl("chkAllSignOff");
    //        foreach (GridViewRow row in gvSignOff.Rows)
    //        {
    //            CheckBox cbSelected = (CheckBox)row.FindControl("chkSignOffSelect");
    //            if (cbSelected != null)
    //            {
    //                if (chkAll.Checked == true)
    //                {
    //                    cbSelected.Checked = true;
    //                }
    //                else
    //                {
    //                    cbSelected.Checked = false;
    //                }
    //            }
    //        }
    //    }
    //}

    //protected void SaveCheckedSignOffValues(Object sender, EventArgs e)
    //{
    //    ArrayList SelectedSignOn = new ArrayList();
    //    Int64 index;
    //    foreach (GridViewRow gvrow in gvSignOff.Rows)
    //    {
    //        bool result = false;
    //        index = Int64.Parse(gvSignOff.DataKeys[gvrow.RowIndex].Value.ToString());

    //        if (((CheckBox)(gvrow.FindControl("chkSignOffSelect"))).Checked == true)
    //        {
    //            result = true;// ((CheckBox)gvrow.FindControl("chkSignOffSelect")).Checked;
    //        }

    //        // Check in the Session
    //        if (Session["SIGNOFF_CHECKED_ITEMS"] != null)
    //            SelectedSignOn = (ArrayList)Session["SIGNOFF_CHECKED_ITEMS"];
    //        if (result)
    //        {
    //            if (!SelectedSignOn.Contains(index))
    //                SelectedSignOn.Add(index);
    //        }
    //        else
    //            SelectedSignOn.Remove(index);
    //    }
    //    if (SelectedSignOn != null && SelectedSignOn.Count > 0)
    //        Session["SIGNOFF_CHECKED_ITEMS"] = SelectedSignOn;
    //}

    //private void CheckSelectedAllSignOff()
    //{
    //    if (Session["SIGNOFF_CHECKED_ITEMS"] != null)
    //    {
    //        for (int i = 0; i < gvSignOff.Rows.Count; i++)
    //        {
    //            ArrayList SelectedSignOff = new ArrayList();
    //            SelectedSignOff = (ArrayList)Session["SIGNON_CHECKED_ITEMS"];
    //            if (SelectedSignOff != null && SelectedSignOff.Count > 0)
    //            {
    //                foreach (Int64 index in SelectedSignOff)
    //                {
    //                    if (gvSignOff.DataKeys[i].Value.ToString().Equals(index.ToString()))
    //                    {
    //                        CheckBox cbSelected = (CheckBox)gvSignOff.Rows[i].FindControl("chkSignOffSelect");
    //                        if (cbSelected != null)
    //                        {
    //                            cbSelected.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentSignOnOffSelection != null)
            {
                NameValueCollection nvc = Filter.CurrentSignOnOffSelection;

                if (nvc.Get("type").ToUpper().ToString() == "SIGNON")
                {
                    ArrayList SelectedSignOn = new ArrayList();
                    //string selectedsignonlist = ",";
                    if (Session["SIGNON_CHECKED_ITEMS"] != null)
                    {
                        SelectedSignOn = (ArrayList)Session["SIGNON_CHECKED_ITEMS"];
                        if (SelectedSignOn != null && SelectedSignOn.Count > 0)
                        {
                            foreach (Int64 index in SelectedSignOn)
                            {
                                //selectedsignonlist = selectedsignonlist + index + ","; 
                                PhoenixCrewOffshoreCrewList.UpdateVesselSignOn(int.Parse(index.ToString()), DateTime.Parse(nvc.Get("txtDate").ToString()), int.Parse(nvc.Get("ddlSeaPort").ToString()), 1);
                            }
                        }
                        Session["SIGNON_CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please Select Seafarer to Sign On";
                        ucError.Visible = true;
                        return;
                    }
                }

                if (nvc.Get("type").ToUpper().ToString() == "SIGNOFF")
                {
                    ArrayList SelectedSignOff = new ArrayList();
                    //string selectedsignonlist = ",";
                    if (Session["SIGNOFF_CHECKED_ITEMS"] != null)
                    {
                        SelectedSignOff = (ArrayList)Session["SIGNOFF_CHECKED_ITEMS"];
                        if (SelectedSignOff != null && SelectedSignOff.Count > 0)
                        {
                            foreach (Int64 index in SelectedSignOff)
                            {
                                //selectedsignonlist = selectedsignonlist + index + ","; 
                                PhoenixCrewOffshoreCrewList.UpdateVesselSignOff(int.Parse(index.ToString()), DateTime.Parse(nvc.Get("txtDate").ToString()), int.Parse(nvc.Get("ddlSeaPort").ToString()), 0);
                            }
                        }
                        Session["SIGNOFF_CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please Select Seafarer to Sign Off";
                        ucError.Visible = true;
                        return;
                    }
                }
            }

            gvCrewSearch.Rebind();
            gvSignOff.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  

    
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

      

        if (e.Item is GridDataItem)
        {
            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                de.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            LinkButton db = (LinkButton)e.Item.FindControl("cmdApprove");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm sign-on ?')");
            }
            RadLabel EmpId = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel SignOnOffId = (RadLabel)e.Item.FindControl("lblSignOnOffIdAdd");
            RadLabel Signondate = ((RadLabel)e.Item.FindControl("lblSignOnDate"));
            RadLabel Port = ((RadLabel)e.Item.FindControl("lblSeaPortId"));
            RadLabel crewplanid = ((RadLabel)e.Item.FindControl("lblCrewPlanId"));
            LinkButton cmdAppointmentLetter = (LinkButton)e.Item.FindControl("cmdAppointmentLetter");
            LinkButton cmdCancelAppointment = (LinkButton)e.Item.FindControl("cmdCancelAppointment");
            LinkButton ccmdDocChecklist = (LinkButton)e.Item.FindControl("cmdDocChecklist");

            LinkButton Course = (LinkButton)e.Item.FindControl("cmdTrainingCourse");
            if (Course != null)
            {
                Course.Visible = SessionUtil.CanAccess(this.ViewState, Course.CommandName);
                Course.Attributes.Add("onclick", "openNewWindow('course', '', '"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreTrainingMatrixRequirement.aspx?empid=" + EmpId.Text + "&signonoffid=" + SignOnOffId.Text + "');return false;");
            }
            if (cmdCancelAppointment != null)
            {
                cmdCancelAppointment.Attributes.Add("onclick", "openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + drv["FLDCREWPLANID"].ToString()
                    + "&APPOINTMENTLETTERID=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                    + "&EMPLOYEEID=" + EmpId.Text + "','medium'); return true;");
            }
            if (cmdAppointmentLetter != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdAppointmentLetter.Visible = false;

                cmdAppointmentLetter.Attributes.Add("onclick", "openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTERPDF&showmenu=0&showword=0&showexcel=0"
                    + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                    + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                    + "&popup=1" + "');return false;");
            }

            if (ccmdDocChecklist != null)
            {
                ccmdDocChecklist.Attributes.Add("onclick", "openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDocumentChecklist.aspx?signonoffid=" + SignOnOffId.Text + "'); return true;");
            }
            //LinkButton Approve = (LinkButton)e.Item.FindControl("cmdApprove");
            //if (Approve != null)
            //{
            //    Approve.Visible = SessionUtil.CanAccess(this.ViewState, Approve.CommandName);
            //    Approve.Attributes.Add("onclick", "parent.Openpopup('suitability','','../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + EmpId.Text + "&calledfrom=signonoff&signonid=" + SignOnOffId.Text + "&signondate=" + Signondate.Text + "&port=" + Port.Text + "&crewplanid=" + crewplanid.Text + "&personalmaster=1');return false;");

            //}
            UserControlSignOnReason ucSignOnReasonEdit = (UserControlSignOnReason)e.Item.FindControl("ucSignOnReasonEdit");
            if (ucSignOnReasonEdit != null)
            {
                ucSignOnReasonEdit.SelectedSignOnReason = drv["FLDSIGNONREASONID"].ToString();
            }

            UserControlSeaport ddlSeaPort = (UserControlSeaport)e.Item.FindControl("ddlSeaPort");
            if (ddlSeaPort != null) ddlSeaPort.SelectedSeaport = drv["FLDSIGNONSEAPORTID"].ToString();

            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && lblEmployeeid != null && SignOnOffId != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&signonId=" + SignOnOffId.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&signonId=" + SignOnOffId.Text + "&launchedfrom=offshore'); return false;");
            }

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && lblEmployeeid != null)
            {
                lnkEmployeeName.Visible = SessionUtil.CanAccess(this.ViewState, lnkEmployeeName.CommandName);
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
            }
            if (cmdCancelAppointment != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancelAppointment.CommandName)) cmdCancelAppointment.Visible = false;
            }
            if (cmdAppointmentLetter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAppointmentLetter.CommandName)) cmdAppointmentLetter.Visible = false;
            }
            if (ccmdDocChecklist != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ccmdDocChecklist.CommandName)) ccmdDocChecklist.Visible = false;
            }
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT") return;
      
        try
        {
            if (e.CommandName.ToString().ToUpper() == "APPROVE")
            {
                string signonid = ((RadLabel)e.Item.FindControl("lblSignOnOffIdAdd")).Text;
                string signondate = ((RadLabel)e.Item.FindControl("lblSignOnDate")).Text;
                string port = ((RadLabel)e.Item.FindControl("lblSeaPortId")).Text;
                string dailyrate = ((RadLabel)e.Item.FindControl("lblDailyRate")).Text;
                string reliefdate = ((RadLabel)e.Item.FindControl("lblReliefDate")).Text;
                string ninetyreliefdate = ((RadLabel)e.Item.FindControl("lbl90ReliefDate")).Text;
                string signonreason = ((RadLabel)e.Item.FindControl("lblSignonReasonId")).Text;
                if (!IsValidSignOn(signondate, port, reliefdate, signonreason))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOffshoreCrewList.ConfirmVesselSignOn(int.Parse(signonid));

                //Insert Task
                PhoenixCrewOffshoreCrewList.InertCompetencyTask(int.Parse(signonid));

                gvCrewSearch.Rebind();
                gvSignOff.Rebind();
            }
            if(e.CommandName.ToString().ToUpper()=="UPDATE")
            {
               
                try
                {
                    string signonid = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                    string signondate = ((UserControlDate)e.Item.FindControl("txtSignOnDate")).Text;
                    string port = ((UserControlSeaport)e.Item.FindControl("ddlSeaPort")).SelectedSeaport;
                    string reliefdate = ((UserControlDate)e.Item.FindControl("txtReliefDateEdit")).Text;
                    string signonreason = ((UserControlSignOnReason)e.Item.FindControl("ucSignOnReasonEdit")).SelectedSignOnReason;
                    string signondatecheckid = ((RadLabel)e.Item.FindControl("lblSignonDateCheckIdEdit")).Text;
                    if (!IsValidSignOn(signondate, port, reliefdate, signonreason))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewOffshoreCrewList.UpdateVesselSignOn(int.Parse(signonid), DateTime.Parse(signondate), int.Parse(port), null
                        , General.GetNullableDateTime(reliefdate), General.GetNullableInteger(signonreason), General.GetNullableGuid(signondatecheckid));
                    BindData();
                    gvCrewSearch.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if(e.CommandName.ToString().ToUpper()=="DELETE")
            {
               
                try
                {
                    int id = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDSIGNONOFFID"].ToString());
                    PhoenixCrewOffshoreCrewList.DeleteVesselSignOn(id);
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

                BindData();
                gvCrewSearch.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvSignOff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSignOff.CurrentPageIndex + 1;
            BindDataSignOff();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSignOff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT") return;
       
        try
        {
            if (e.CommandName.ToString().ToUpper() == "APPROVE")
            {
                int signonid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDSIGNONOFFID"].ToString());
                string signoffdate = ((RadLabel)e.Item.FindControl("lblSignOffDate")).Text;
                string port = ((RadLabel)e.Item.FindControl("lblSeaPortId")).Text;
                string signoffreason = ((RadLabel)e.Item.FindControl("lblSignoffReasonId")).Text;
                string travelenddate = ((RadLabel)e.Item.FindControl("lblEstimatedTravelDate")).Text;
                string signondate = ((RadLabel)e.Item.FindControl("lblDateJoined")).Text;
                string reliefdate = ((RadLabel)e.Item.FindControl("lblReliefDate")).Text;
                if (!IsValidSignOff(signoffdate, port, signoffreason, travelenddate, signondate, reliefdate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOffshoreCrewList.ConfirmVesselSignOff(signonid);
                BindDataSignOff();
                gvSignOff.Rebind();
            }
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                string signonoffid = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                string signoffdate = ((UserControlDate)e.Item.FindControl("txtSignOffDate")).Text;
                string port = ((UserControlSeaport)e.Item.FindControl("ddlSeaPort")).SelectedSeaport;
                string signoffreason = ((UserControlSignOffReason)e.Item.FindControl("ucSignOffReasonEdit")).SelectedSignOffReason;
                string estimatedtravelenddate = ((UserControlDate)e.Item.FindControl("txtEstimatedTravelEndDate")).Text;
                string signondate = ((UserControlDate)e.Item.FindControl("txtSignOnDate")).Text;
                string reliefdate = ((UserControlDate)e.Item.FindControl("txtReliefDateEdit")).Text;

                if (!IsValidSignOff(signoffdate, port, signoffreason, estimatedtravelenddate, signondate, reliefdate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOffshoreCrewList.UpdateVesselSignOff(int.Parse(signonoffid), DateTime.Parse(signoffdate), int.Parse(port), 1, General.GetNullableInteger(signoffreason)
                                                                , General.GetNullableDateTime(estimatedtravelenddate), General.GetNullableDateTime(signondate), General.GetNullableDateTime(reliefdate));
                BindDataSignOff();
                gvSignOff.Rebind();
            }
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {

            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSignOff_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvSignOff_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdApprove");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm sign-off ?')");
            }
            RadLabel EmpId = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel lblSignOnOffID = (RadLabel)e.Item.FindControl("lblSignOnOffid");

            LinkButton Course = (LinkButton)e.Item.FindControl("cmdTrainingCourse");
            if (Course != null)
            {
                Course.Visible = SessionUtil.CanAccess(this.ViewState, Course.CommandName);
                Course.Attributes.Add("onclick", "openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingMatrixRequirement.aspx?empid=" + EmpId.Text + "&signonoffid=" + lblSignOnOffID.Text + "');return false;");
            }
            UserControlSignOffReason ddlReason = (UserControlSignOffReason)e.Item.FindControl("ddlReason");
            if (ddlReason != null) ddlReason.SelectedSignOffReason = drv["FLDSIGNOFFREASONID"].ToString();

            CheckBox chk = (CheckBox)e.Item.FindControl("chkRecoverNegBal");
            if (chk != null)
            {
                if (General.GetNullableDecimal(drv["FLDCURRENTBALANCE"].ToString()).HasValue
                    && General.GetNullableDecimal(drv["FLDCURRENTBALANCE"].ToString()) < -1 && drv["FLDSIGNOFFDATE"].ToString() != string.Empty)
                {
                    chk.Visible = true;
                }
                else
                    chk.Visible = false;
            }

            LinkButton cmdCancelAppointment = (LinkButton)e.Item.FindControl("cmdCancelAppointment");
            if (cmdCancelAppointment != null)
            {
                cmdCancelAppointment.Attributes.Add("onclick", "openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + drv["FLDCREWPLANID"].ToString()
                    + "&APPOINTMENTLETTERID=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                    + "&EMPLOYEEID=" + EmpId.Text + "','medium'); return true;");
            }

            LinkButton ccmdDocChecklist = (LinkButton)e.Item.FindControl("cmdDocChecklist");
            if (ccmdDocChecklist != null)
            {
                ccmdDocChecklist.Attributes.Add("onclick", "openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDocumentChecklist.aspx?signonoffid=" + lblSignOnOffID.Text + "'); return true;");
            }

            LinkButton cmdRaiseReliefRequest = (LinkButton)e.Item.FindControl("cmdRaiseReliefRequest");
            if (cmdRaiseReliefRequest != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRaiseReliefRequest.CommandName)) cmdRaiseReliefRequest.Visible = false;
                cmdRaiseReliefRequest.Attributes.Add("onclick", "openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreReliefRemarks.aspx?SIGNONOFFID=" + lblSignOnOffID.Text + "','medium'); return true;");
            }

            UserControlSignOffReason ucSignOffReasonEdit = (UserControlSignOffReason)e.Item.FindControl("ucSignOffReasonEdit");
            if (ucSignOffReasonEdit != null) ucSignOffReasonEdit.SelectedSignOffReason = drv["FLDSIGNOFFREASONID"].ToString();

            UserControlSeaport ddlSeaPort = (UserControlSeaport)e.Item.FindControl("ddlSeaPort");
            if (ddlSeaPort != null) ddlSeaPort.SelectedSeaport = drv["FLDSIGNOFFSEAPORTID"].ToString();

            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
            }
            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                lnkEmployeeName.Visible = SessionUtil.CanAccess(this.ViewState, lnkEmployeeName.CommandName);
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "openNewWindow('chml','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
            }
            if (cmdCancelAppointment != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancelAppointment.CommandName)) cmdCancelAppointment.Visible = false;
            }
            if (ccmdDocChecklist != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ccmdDocChecklist.CommandName)) ccmdDocChecklist.Visible = false;
            }
        }
    }
}

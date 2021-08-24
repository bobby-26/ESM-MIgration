using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web.UI;

public partial class CrewOffshore_CrewOffshoreSignOnDateCorrection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Sign On/Off", "SIGNONOFFCONFIG");
            toolbarsub.AddButton("App. Letter", "APPLETTER");
            toolbarsub.AddButton("Signon Date", "SDC");
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            CrewQuery.SelectedMenuIndex = 2;
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSignOnDateCorrection.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSignOff')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../CrewOffshore/CrewOffshoreBulkSignOnOff.aspx?type=signoff'); return false;", "Bulk Sign Off", "bulk_save.png", "BULK");
            CrewSignOff.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERSO"] = 1;
                gvSignOff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           // BindDataSignOff();
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("APPLETTER"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreAppointmentLetterCorrection.aspx", true);
        }
        if (CommandName.ToUpper().Equals("SIGNONOFFCONFIG"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreSignOnOffConfiguration.aspx", true);
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

            string[] alColumns = new string[0];
            string[] alCaptions = new string[0];
            alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", 
                                     "FLDDAILYRATE","FLDDPALLOWANCE","FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDSIGNOFFREASON", "FLDSIGNOFFSEAPORTNAME"
                                     , "FLDRELIEFDUEDATE",
                                     "FLD90RELIEFDATE" };
            alCaptions = new string[] { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No",  
                                            "Daily Rate (USD)","Daily DP Allowance (USD)",
                                            "Sign On Date", "Sign-Off Date","Sign-Off Reason", "Sign-Off Port",
                                            "End of Contract", 
                                            "Max Tour of Duty" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.bind();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                    UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
            }


            DataTable dt = PhoenixCrewOffshoreReliefRequest.SearchSignOffList(General.GetNullableInteger(UcVessel.SelectedVessel)
                                                                ,  null
                                                                , null
                                                                , General.GetNullableInteger(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , gvSignOff.CurrentPageIndex+1, gvSignOff.PageSize
                                                                , ref iRowCount, ref iTotalPageCount
                                                                , sortexpression, sortdirection
                                                              );
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvSignOff", "Crew List", alCaptions, alColumns, ds);
            gvSignOff.DataSource = dt;
            gvSignOff.VirtualItemCount = iRowCount;
           

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                gvSignOff.Columns[7].Visible = false;
                gvSignOff.Columns[8].Visible = false;
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
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        BindDataSignOff();
        gvSignOff.Rebind();
        //SetPageNavigator();
    }
    protected void ShowExcelSOff()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
   
            alColumns = new string[] { "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", 
                                     "FLDDAILYRATE","FLDDPALLOWANCE","FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDSIGNOFFREASON", "FLDSIGNOFFSEAPORTNAME"
                                     , "FLDRELIEFDUEDATE",
                                     "FLD90RELIEFDATE" };
            alCaptions = new string[] { "Vessel", "File No", "Name", "Rank", "Nationality", "Passport No",  
                                            "Daily Rate (USD)","Daily DP Allowance (USD)",
                                            "Sign On Date", "Sign-Off Date","Sign-Off Reason", "Sign-Off Port",
                                            "End of Contract", 
                                            "Max Tour of Duty" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            UcVessel.bind();
            ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            UcVessel.Enabled = false;
        }
        else
        {
            if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
        }


        DataTable dt = PhoenixCrewOffshoreReliefRequest.SearchSignOffList(General.GetNullableInteger(UcVessel.SelectedVessel)
                                                                , null
                                                                , null
                                                                , General.GetNullableInteger(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , General.GetNullableDateTime(string.Empty)
                                                                , 1, iRowCount
                                                                , ref iRowCount, ref iTotalPageCount
                                                                , sortexpression, sortdirection
                                                              );

        General.ShowExcel("Crew List", dt, alColumns, alCaptions, null, string.Empty);
    }
    protected void gvSignOff_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT") return;
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void gvSignOff_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataSignOff();
    }
    protected void gvSignOff_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindDataSignOff();
    }
    
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSignOn(string signondate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDateTime(signondate).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Date is required.";
        }
        return (!ucError.IsError);
    }
  
    
    protected void gvSignOff_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindDataSignOff();
    }
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
           // BindDataSignOff();
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  

    protected void gvSignOff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataSignOff();
    }

    protected void gvSignOff_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
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

            ImageButton Course = (ImageButton)e.Item.FindControl("cmdTrainingCourse");
            if (Course != null)
            {
                Course.Visible = SessionUtil.CanAccess(this.ViewState, Course.CommandName);
                Course.Attributes.Add("onclick", "parent.Openpopup('course', '', '../CrewOffshore/CrewOffshoreTrainingMatrixRequirement.aspx?empid=" + EmpId.Text + "&signonoffid=" + lblSignOnOffID.Text + "');return false;");
            }
            UserControlSignOffReason ddlReason = (UserControlSignOffReason)e.Item.FindControl("ddlReason");
            if (ddlReason != null) ddlReason.SelectedSignOffReason = DataBinder.Eval(e.Item.DataItem, "FLDSIGNOFFREASONID").ToString();

            CheckBox chk = (CheckBox)e.Item.FindControl("chkRecoverNegBal");
            if (chk != null)
            {
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDCURRENTBALANCE").ToString()).HasValue
                    && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDCURRENTBALANCE").ToString()) < -1 && DataBinder.Eval(e.Item.DataItem, "FLDSIGNOFFDATE").ToString() != string.Empty)
                {
                    chk.Visible = true;
                }
                else
                    chk.Visible = false;
            }

            ImageButton cmdCancelAppointment = (ImageButton)e.Item.FindControl("cmdCancelAppointment");
            if (cmdCancelAppointment != null)
            {
                cmdCancelAppointment.Attributes.Add("onclick", "javascript:parent.Openpopup('CAPP','','../CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + DataBinder.Eval(e.Item.DataItem, "FLDCREWPLANID").ToString()
                    + "&APPOINTMENTLETTERID=" + DataBinder.Eval(e.Item.DataItem, "FLDAPPOINTMENTLETTERID").ToString()
                    + "&EMPLOYEEID=" + EmpId.Text + "','medium'); return true;");
            }

            ImageButton ccmdDocChecklist = (ImageButton)e.Item.FindControl("cmdDocChecklist");
            if (ccmdDocChecklist != null)
            {
                ccmdDocChecklist.Attributes.Add("onclick", "javascript:parent.Openpopup('CAPP','','../CrewOffshore/CrewOffshoreDocumentChecklist.aspx?signonoffid=" + lblSignOnOffID.Text + "'); return true;");
            }

            ImageButton cmdRaiseReliefRequest = (ImageButton)e.Item.FindControl("cmdRaiseReliefRequest");
            if (cmdRaiseReliefRequest != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRaiseReliefRequest.CommandName)) cmdRaiseReliefRequest.Visible = false;
                cmdRaiseReliefRequest.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../CrewOffshore/CrewOffshoreReliefRemarks.aspx?SIGNONOFFID=" + lblSignOnOffID.Text + "','medium'); return true;");
            }

            UserControlSignOffReason ucSignOffReasonEdit = (UserControlSignOffReason)e.Item.FindControl("ucSignOffReasonEdit");
            if (ucSignOffReasonEdit != null) ucSignOffReasonEdit.SelectedSignOffReason = DataBinder.Eval(e.Item.DataItem, "FLDSIGNOFFREASONID").ToString();

            UserControlSeaport ddlSeaPort = (UserControlSeaport)e.Item.FindControl("ddlSeaPort");
            if (ddlSeaPort != null) ddlSeaPort.SelectedSeaport = DataBinder.Eval(e.Item.DataItem, "FLDSIGNOFFSEAPORTID").ToString();

            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE") != null && General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
            }
            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                lnkEmployeeName.Visible = SessionUtil.CanAccess(this.ViewState, lnkEmployeeName.CommandName);
                if (DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE") != null && General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + DataBinder.Eval(e.Item.DataItem, "FLDCREWPLANID").ToString() + "'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + DataBinder.Eval(e.Item.DataItem, "FLDCREWPLANID").ToString() + "'); return false;");
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

    protected void gvSignOff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName.ToUpper()=="UPDATE")
        {
            try
            {
                string signonoffid = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                string signondate = ((UserControlDate)e.Item.FindControl("txtSignOnDate")).Text;
                string port = ((RadLabel)e.Item.FindControl("lblSeaPortId")).Text;
                string signonreason = ((RadLabel)e.Item.FindControl("lblSignonreason")).Text;




                if (!IsValidSignOn(signondate))
                {
                   
                    ucError.Visible = true;

                    return;
                }


                PhoenixCrewOffshoreCrewList.UpdateSignonDate(int.Parse(signonoffid), DateTime.Parse(signondate), int.Parse(port), null,
                   General.GetNullableInteger(signonreason));


                gvSignOff.Rebind();
            }
            catch (Exception ex)
            {
                e.Canceled = true;
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
              
            }
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

  
}

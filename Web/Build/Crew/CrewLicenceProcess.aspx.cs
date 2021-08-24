using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Drawing;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLicenceProcess : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvLicReq.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvLicReq.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!Page.IsPostBack)
            {       

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["CRPAGENUMBER"] = 1;
                ViewState["CRSORTEXPRESSION"] = null;
                ViewState["CRSORTDIRECTION"] = null;

                Filter.CurrentLicenceRequestFilter = null;
                Filter.CurrentLicenceReqCovLetterFilter = null;
                ViewState["PID"] = string.Empty;
				ViewState["PAGEURL"] = "CrewLicenceProcessGeneral.aspx";
                if (Request.QueryString["nl"] != null)
                {
                    ucError.ErrorMessage = "Application Form Not Available.";
                    ucError.Visible = true;
                }
                if (Request.QueryString["err"] != null)
                {
                    ucError.ErrorMessage = "Seperate Application Form for COC is not required for this flag.";
                    ucError.Visible = true;
                }
                if (Request.QueryString["cl"] != null)
                {
                    ucError.ErrorMessage = "Covering Letter Not Available.";
                    ucError.Visible = true;
                }
                if (Request.QueryString["serv"] != null)
                {
                    ucError.ErrorMessage = "Sea Service Not Available for this flag.";
                    ucError.Visible = true;
                }
               
            }

            BindData(); 
            SetPageNavigator();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
           
            toolbargrid.AddImageButton("../Crew/CrewLicenceProcess.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvLicReq')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:Openpopup('Filter','','CrewLicenceRequestFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Crew/CrewLicenceProcess.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageButton("../Crew/CrewLicenceProcess.aspx", "Covering Letter", "covering-letter.png", "COVERINGLETTER");
            toolbargrid.AddImageButton("../Crew/CrewLicenceProcess.aspx", "Application Form", "application-form.png", "APPLICATION");
            toolbargrid.AddImageButton("../Crew/CrewLicenceProcess.aspx", "CRA Form", "applying-cra.png", "CRAFORM");
            MenuLicenceList.AccessRights = this.ViewState;
            MenuLicenceList.MenuList = toolbargrid.Show();

			PhoenixToolbar toolbargridcancel = new PhoenixToolbar();
			toolbargridcancel.AddImageButton("../Crew/CrewLicenceProcess.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargridcancel.AddImageLink("javascript:CallPrint('gvLicReqCancel')", "Print Grid", "icon_print.png", "PRINT");
			MenuLicenceCancelList.AccessRights = this.ViewState;
			MenuLicenceCancelList.MenuList = toolbargridcancel.Show();

			BindCancelData();
            SetCRPageNavigator();
            MenuLicenceList.SetTrigger(pnlCrewCourseCertificateEntry);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuLicenceList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("COVERINGLETTER") && ViewState["PID"].ToString() != string.Empty)
            {
                if (gvLicReq.SelectedIndex < 0)
                {
                    ucError.ErrorMessage = "Select one Licence Request";
                    ucError.Visible = true;
                    return;
                }
                getSelectedReqForCovLetter();
                string processid="";
                if (Filter.CurrentLicenceReqCovLetterFilter != null)
                    processid = Filter.CurrentLicenceReqCovLetterFilter;
                else 
                    processid = ((Label)gvLicReq.Rows[gvLicReq.SelectedIndex].FindControl("lblProcessId")).Text;
                string flagid = ((Label)gvLicReq.Rows[gvLicReq.SelectedIndex].FindControl("lblFlagId")).Text;
                string strRefNo = string.Empty;
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=LICENCE&flagid=" + flagid + "&processid=" + processid + "&refno=" + strRefNo,false);
            }
            else if (dce.CommandName.ToUpper().Equals("APPLICATION") && ViewState["PID"].ToString() != string.Empty)
            {
                if (gvLicReq.SelectedIndex < 0)
                {
                    ucError.ErrorMessage = "Select one Licence Request";
                    ucError.Visible = true;
                    return;
                }
                string processid = ((Label)gvLicReq.Rows[gvLicReq.SelectedIndex].FindControl("lblProcessId")).Text;
                string flagid = ((Label)gvLicReq.Rows[gvLicReq.SelectedIndex].FindControl("lblFlagId")).Text;
                string strRefNo = string.Empty;
				PhoenixCrewLicenceRequest.UpdateLicenceProcessCRAStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid(processid));
				Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&showword=0&showexcel=0&reportcode=LICENCEAPP&flagid=" + flagid + "&processid=" + processid + "&refno=" + strRefNo);
            }
            else if (dce.CommandName.ToUpper().Equals("CRAFORM") && ViewState["PID"].ToString() != string.Empty)
            {
                if (gvLicReq.SelectedIndex < 0)
                {
                    ucError.ErrorMessage = "Select one Licence Request";
                    ucError.Visible = true;
                    return;
                }
                string processid = ((Label)gvLicReq.Rows[gvLicReq.SelectedIndex].FindControl("lblProcessId")).Text;
                string flagid = ((Label)gvLicReq.Rows[gvLicReq.SelectedIndex].FindControl("lblFlagId")).Text;
                string strRefNo = string.Empty;
                if (flagid.Equals("94"))
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=LICENCECRA&flagid=" + flagid + "&processid=" + processid + "&refno=" + strRefNo);
                }
                else
                {
                    ucError.ErrorMessage = "CRA Form Not Available";
                    ucError.Visible = true;
                    return;
                }
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentLicenceRequestFilterSelection = null;
                Filter.CurrentLicenceReqCovLetterFilter = null;
                //BindData();
                cmdHiddenSubmit_Click(null, null);
            } 
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDREFNUMBER", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCREWCHANGEDATE", "FLDCREATEDDATE", "FLDCRADATE", "FLDCOMPANYADDRINREPORT", "FLDAUTHORIZED", "FLDDESIGNATION", "FLDSTATUSNAME", "FLDBILLTOCOMPANY", "FLDAMOUNT", "FLDSUBACCOUNT" };
                string[] alCaptions = { "Request No", "Flag", "Vessel", "Crew Change", "Requested", "CRA Requested", "Company", "Authorized Representative", "Designation", "Status", "Bill To", "Amount", "Budget Code" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;
				DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																			, sortexpression, sortdirection
																			, (int)ViewState["PAGENUMBER"], iRowCount
																			, ref iRowCount, ref iTotalPageCount
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																			, null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null,1
																			, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                            , nvc != null ? General.GetNullableString(nvc.Get("ucRankList")) : null);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Licence Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLicReq_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = e.NewEditIndex;
			_gridView.SelectedIndex = e.NewEditIndex;
			Label lblPID = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblProcessId"));
			ViewState["PID"] = lblPID.Text;
			BindData();
			SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLicReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                 && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                string lblPaymentBy = ((Label)e.Row.FindControl("lblPaymentBy")).Text;
                string lblStatus = ((Label)e.Row.FindControl("lblStatus")).Text;
                string lblflagid = ((Label)e.Row.FindControl("lblFlagId")).Text;
                Label l = (Label)e.Row.FindControl("lblProcessId");
                CheckBox cb;
                cb = (CheckBox)e.Row.FindControl("chkSelect");
                if (cb != null && lblStatus != null && ViewState["FLAGID"] != null && !lblflagid.Equals(ViewState["FLAGID"].ToString()))
                    cb.Visible = false;

                CheckBox cb1;
                cb1 = (CheckBox)e.Row.FindControl("chkSelectCL");
                if (cb1 != null && ViewState["FLAGID"] != null && !lblflagid.Equals(ViewState["FLAGID"].ToString()))
                    cb1.Visible = false;

                if (cb != null && lblStatus != null && lblStatus == PhoenixCommonRegisters.GetHardCode(1, 123, "LPO"))
                    cb.Enabled = false;

                if (lblStatus != null && lblStatus == PhoenixCommonRegisters.GetHardCode(1, 123, "LPO"))
                {
                    UserControlMaskNumber t = ((UserControlMaskNumber)e.Row.FindControl("txtAmountEdit"));
                    if (t != null)
                    {
                        t.CssClass = "readonlytextbox";
                        t.ReadOnly = true;
                    }
                    UserControlBudgetCode b = ((UserControlBudgetCode)e.Row.FindControl("ucBudgetCodeEdit"));
                    if (b != null)
                    {
                        b.CssClass = "readonlytextbox";
                        b.Enabled = false;
                    }

                }

                UserControlBudgetCode ucBudgetCodeEdit = (UserControlBudgetCode)e.Row.FindControl("ucBudgetCodeEdit");
                DataRowView drv = (DataRowView)e.Row.DataItem;

                UserControlCompany ucCompany = (UserControlCompany)e.Row.FindControl("ddlCompany");
                if (ucCompany != null) ucCompany.SelectedCompany = drv["FLDCOMPANYADDR"].ToString();


                if (ucBudgetCodeEdit != null && drv["FLDSUBACCOUNT"].ToString() != "")
                    ucBudgetCodeEdit.SelectedBudgetSubAccount = drv["FLDSUBACCOUNT"].ToString();
                else if (ucBudgetCodeEdit != null && drv["FLDSUBACCOUNT"].ToString() == "")
                    ucBudgetCodeEdit.SelectedBudgetSubAccount = "2703";

                ImageButton lb = (ImageButton)e.Row.FindControl("cmdGeneratePO");
                lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'CrewLicenceProcessGeneral.aspx?pid=" + l.Text + "');return false;");
            }
           
           
        }
       
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
            Label lblRequestedDate = (Label)e.Row.FindControl("lblRequestedDate");
            UserControlToolTip ucToolTipRequestedDate = (UserControlToolTip)e.Row.FindControl("ucToolTipRequestedDate");
            if (ucToolTipRequestedDate != null)
            {
                lblRequestedDate.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipRequestedDate.ToolTip + "', 'visible');");
                lblRequestedDate.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipRequestedDate.ToolTip + "', 'hidden');");
            }
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
  
    protected void gvLicReq_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLicReq_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }

    protected void gvLicReq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string lblProcessId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId")).Text;
            string amt = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text;
            string budgetid = ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ucBudgetCodeEdit")).SelectedBudgetCode;
            string authorized = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAuthorisedRep")).Text;
            string designation = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDesignation")).Text;
            string compaddr = ((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompany")).SelectedCompany;

            if (!IsValidBudgetCode(amt, budgetid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateCrewLicenceProcessBudgetCode(
                new Guid(lblProcessId), int.Parse(budgetid), decimal.Parse(amt), authorized, designation,General.GetNullableInteger(compaddr.ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        _gridView.EditIndex = -1;
        SetPageNavigator();
        BindData();
    }

    protected void gvLicReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToString().ToUpper() == "SELECT")
			{
				_gridView.SelectedIndex = nCurrentRow;

                Label lblPID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId"));
                Label lblflagid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFlagId"));

                ViewState["PID"] = lblPID.Text;
                ViewState["FLAGID"] = lblflagid.Text;

                BindData();
                SetPageNavigator();
            }
          
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private bool IsValidBudgetCode(string amount, string budgetcode)
    {
        ucError.HeaderMessage = "Please provide the following information";

        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount is required";

        if (General.GetNullableInteger(budgetcode) == null)
            ucError.ErrorMessage = "Budget code is required";

        return (!ucError.IsError);
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFNUMBER", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCREWCHANGEDATE", "FLDCREATEDDATE", "FLDCRADATE", "FLDCOMPANYADDRINREPORT", "FLDAUTHORIZED", "FLDDESIGNATION", "FLDSTATUSNAME", "FLDBILLTOCOMPANY", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "Request No", "Flag", "Vessel", "Crew Change", "Requested", "CRA Requested", "Company", "Authorized Representative", "Designation", "Status", "Bill To", "Amount", "Budget Code" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
     
        try
        {
			NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;
			DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch( nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")):null
																		, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																		,	sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                        , ref iRowCount, ref iTotalPageCount
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																		, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                        , null
																		, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																		, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null,1
																		, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                        , nvc != null ? General.GetNullableString(nvc.Get("ucRankList")) : null);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvLicReq", "Licence Request", alCaptions, alColumns, ds); 

            if (dt.Rows.Count > 0)
            {
                if (ViewState["PID"].ToString() == string.Empty)
                {
                    ViewState["PID"] = dt.Rows[0]["FLDPROCESSID"].ToString();
                    // gvLicReq.SelectedIndex = 0;
                    ViewState["FLAGID"] = dt.Rows[0]["FLDFLAGID"].ToString();

                }               
                gvLicReq.DataSource = dt;
                gvLicReq.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvLicReq);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            getSelectedRequests();
            getSelectedReqForCovLetter(); 
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvLicReq.SelectedIndex = -1;
            gvLicReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PID"] = string.Empty;
        ViewState["PAGENUMBER"] = 1;
        ViewState["CRPAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
        BindCancelData();
        SetCRPageNavigator();
        //ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?pid=" + ViewState["PID"];
        txtnopage.Text = "";
        txtCRnopage.Text = "";
    }
    protected void chkSelect_checkedchanged(object sender, EventArgs e)
    {
        getSelectedRequests();
    }
    protected void getSelectedRequests()
    {
        string selitems = "";
        for (int i = 0; i < gvLicReq.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)gvLicReq.Rows[i].FindControl("chkSelect");
            Label lblproid = (Label)gvLicReq.Rows[i].FindControl("lblProcessId");
            if (cb != null && lblproid != null && (cb.Checked == true) && (cb.Visible == true) && (cb.Enabled == true))
            {
                selitems += lblproid.Text;
                selitems += ",";
            }
        }
        if (selitems.Length > 0)
        {
            selitems = selitems.Remove(selitems.Length - 1);
            Filter.CurrentLicenceRequestFilter = selitems.ToString();
        }
    }
    protected void chkSelectCL_checkedchanged(object sender, EventArgs e)
    {
        getSelectedReqForCovLetter();
    }
    protected void getSelectedReqForCovLetter()
    {
        string selitems = "";
        for (int i = 0; i < gvLicReq.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)gvLicReq.Rows[i].FindControl("chkSelectCL");
            Label lblproid = (Label)gvLicReq.Rows[i].FindControl("lblProcessId");
            if (cb != null && lblproid != null && (cb.Checked == true) && (cb.Visible == true))
            {
                selitems += lblproid.Text;
                selitems += ",";
            }
        }
        if (selitems.Length > 0)
        {
            selitems = selitems.Remove(selitems.Length - 1);
            Filter.CurrentLicenceReqCovLetterFilter = selitems.ToString();
        }
    }

	protected void BindCancelData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDREFNUMBER", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCREWCHANGEDATE", "FLDCREATEDDATE", "FLDCRADATE", "FLDCANCELDATE", "FLDCANCELLEDBY", "FLDSTATUSNAME" };
		string[] alCaptions = { "Request No", "Flag", "Vessel", "Crew Change", "Requested", "CRA Requested", "Cancelled On", " Cancelled By", "Status" };

		string sortexpression = (ViewState["CRSORTEXPRESSION"] == null) ? null : (ViewState["CRSORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["CRSORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["CRSORTDIRECTION"].ToString());

		try
		{
			NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;

			DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																			, sortexpression, sortdirection
                                                                            , (int)ViewState["CRPAGENUMBER"], General.ShowRecords(null)
																			, ref iRowCount, ref iTotalPageCount
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																			, null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null, 0
																			, nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                            , nvc != null ? General.GetNullableString(nvc.Get("ucRankList")) : null);


			DataSet ds = new DataSet();
			ds.Tables.Add(dt.Copy());

			General.SetPrintOptions("gvLicReqCancel", "Cancelled Requests", alCaptions, alColumns, ds);

			if (dt.Rows.Count > 0)
			{
				gvLicReqCancel.DataSource = dt;
				gvLicReqCancel.DataBind();
			}
			else
			{
                ShowNoRecordsFound(dt, gvLicReqCancel);
			}
            ViewState["CRROWCOUNT"] = iRowCount;
            ViewState["CRTOTALPAGECOUNT"] = iTotalPageCount;
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvLicReqCancel_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{

			Label l = (Label)e.Row.FindControl("lblProcessId");

			LinkButton lb = (LinkButton)e.Row.FindControl("lnkRefNo");
			lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'CrewLicenceRequestCancel.aspx?pid=" + l.Text + "');return false;");


		}
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (ViewState["SORTEXPRESSION"] != null)
			{
				HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
				if (img != null)
				{
					if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
						img.Src = Session["images"] + "/arrowUp.png";
					else
						img.Src = Session["images"] + "/arrowDown.png";

					img.Visible = true;
				}
			}
		}
	}


	protected void gvLicReqCancel_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		ViewState["CRSORTEXPRESSION"] = se.SortExpression;

		if (ViewState["CRSORTDIRECTION"] != null && ViewState["CRSORTDIRECTION"].ToString() == "0")
			ViewState["CRSORTDIRECTION"] = 1;
		else
			ViewState["CRSORTDIRECTION"] = 0;

		BindCancelData();
        SetCRPageNavigator();
	}

	protected void MenuLicenceCancelList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		try
		{
			
			if (dce.CommandName.ToUpper().Equals("CLEAR"))
			{
				Filter.CurrentLicenceRequestFilterSelection = null;
				BindCancelData();
                SetCRPageNavigator();
			}
			else if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;

                string[] alColumns = { "FLDREFNUMBER", "FLDFLAGNAME", "FLDVESSELNAME", "FLDCREWCHANGEDATE", "FLDCREATEDDATE", "FLDCRADATE", "FLDCANCELDATE", "FLDCANCELLEDBY", "FLDSTATUSNAME" };
                string[] alCaptions = { "Request No", "Flag", "Vessel", "Crew Change", "Requested", "CRA Requested", "Cancelled On", " Cancelled By", "Status" };

				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["CRSORTEXPRESSION"] == null) ? null : (ViewState["CRSORTEXPRESSION"].ToString());

				if (ViewState["CRSORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["CRSORTDIRECTION"].ToString());

				if (ViewState["CRROWCOUNT"] == null || Int32.Parse(ViewState["CRROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["CRROWCOUNT"].ToString());

				NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;

				DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ddlFlag")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("ddlVessel")) : null
																			, sortexpression, sortdirection
                                                                            , (int)ViewState["CRPAGENUMBER"], iRowCount
																			, ref iRowCount, ref iTotalPageCount
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
																			, nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
																			, null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
																			, nvc != null ? General.GetNullableString(nvc.Get("txtFileno")) : null, 0
                                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null
                                                                            , nvc != null ? General.GetNullableString(nvc.Get("ucRankList")) : null);
				DataSet ds = new DataSet();
				ds.Tables.Add(dt.Copy());

				if (ds.Tables.Count > 0)
					General.ShowExcel("Cancelled Requests", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    private Boolean IsCRPreviousEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["CRPAGENUMBER"];
        iTotalPageCount = (int)ViewState["CRTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;

    }
    private Boolean IsCRNextEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["CRPAGENUMBER"];
        iTotalPageCount = (int)ViewState["CRTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void SetCRPageNavigator()
    {
        try
        {
            cmdCRPrevious.Enabled = IsCRPreviousEnabled();
            cmdCRNext.Enabled = IsCRNextEnabled();
            lblCRPagenumber.Text = "Page " + ViewState["CRPAGENUMBER"].ToString();
            lblCRPages.Text = " of " + ViewState["CRTOTALPAGECOUNT"].ToString() + " Pages. ";
            lblCRRecords.Text = "(" + ViewState["CRROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdCRGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtCRnopage.Text, out result))
            {
                ViewState["CRPAGENUMBER"] = Int32.Parse(txtCRnopage.Text);

                if ((int)ViewState["CRTOTALPAGECOUNT"] < Int32.Parse(txtCRnopage.Text))
                    ViewState["CRPAGENUMBER"] = ViewState["CRTOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtCRnopage.Text))
                    ViewState["CRPAGENUMBER"] = 1;

                if ((int)ViewState["CRPAGENUMBER"] == 0)
                    ViewState["CRPAGENUMBER"] = 1;

                txtCRnopage.Text = ViewState["CRPAGENUMBER"].ToString();
            }
            BindCancelData();
            SetCRPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CRPagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvLicReqCancel.SelectedIndex = -1;
            gvLicReqCancel.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["CRPAGENUMBER"] = (int)ViewState["CRPAGENUMBER"] - 1;
            else
                ViewState["CRPAGENUMBER"] = (int)ViewState["CRPAGENUMBER"] + 1;

            BindCancelData();
            SetCRPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

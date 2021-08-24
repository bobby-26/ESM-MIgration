using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewEmployeeQualification : PhoenixBasePage
{
    StringBuilder straccounttype = new StringBuilder();

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gdEmployeeQualification.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Crew/CrewEmployeeQualification.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('divGrid')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageButton("../Crew/CrewEmployeeQualification.aspx", "<b>Find</b>", "search.png", "FIND");
		MenuCrewEmployeeQualification.AccessRights = this.ViewState;
        MenuCrewEmployeeQualification.MenuList = toolbargrid.Show();
        MenuCrewEmployeeQualification.SetTrigger(pnlEmployeeQualificationEntry);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["QUALIFICATIONID"] = null;
           
        }
        BindData();
        SetPageNavigator();
    }

    protected void CrewEmployeeQualification_TabStripCommand(object sender, EventArgs e)
    {

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDINSTITUTIONNAME", "FLDPLACE", "FLDCERTIFICATE", "FLDPERCENT","FLDGRADE","FLDFROM","FLDTO","FLDDURATION","FLDPASSEDDATE" };
        string[] alCaptions = { "Institution Name", "Place", "Certificate", "Percent","Grade","From Date","To Date","Duration","Passed Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixCrewEmployeeQualification.EmployeeQualificationSearch(1, null, null, null, null,
                                                        null, null, null, null, null, null, sortexpression, sortdirection,
                                                        (int)ViewState["PAGENUMBER"],
                                                        General.ShowRecords(null),
                                                        ref iRowCount,
                                                        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AcademicQualification.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>StockItem Register</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

   
   

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixCrewEmployeeQualification.EmployeeQualificationSearch(1, null, null, null, null,
                                                        null, null,null,null,null,null,sortexpression, sortdirection,
                                                        (int)ViewState["PAGENUMBER"],
                                                        General.ShowRecords(null),
                                                        ref iRowCount,
                                                        ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gdEmployeeQualification.DataSource = ds;
            gdEmployeeQualification.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gdEmployeeQualification);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    

   
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gdEmployeeQualification_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gdEmployeeQualification_RowEditing(object sender, GridViewEditEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
   

   

    protected void gdEmployeeQualification_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidEmployeeQualification(txtEmployeeNo.Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtInstitutionNameAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtPlaceAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtCertificateAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtPercentAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtGradeAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtFromAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtToAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtDurationAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtPassedDateAdd")).Text))
            {
                ucError.Visible = true;
                return;
            }
           
            DateTime fromdate = Convert.ToDateTime(((TextBox)_gridView.FooterRow.FindControl("txtFromAdd")).Text);
            DateTime todate=Convert.ToDateTime(((TextBox)_gridView.FooterRow.FindControl("txtToAdd")).Text);
            int diffyear = (todate.Year - fromdate.Year);
            int diffDate = diffyear*12+(todate.Month - fromdate.Month);
            ((TextBox)_gridView.FooterRow.FindControl("txtDurationAdd")).Text = diffDate.ToString();

          InsertEmployeeQualification(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        txtEmployeeNo.Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtInstitutionNameAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtPlaceAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtCertificateAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtPercentAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtGradeAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtFromAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtToAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtDurationAdd")).Text,
                                                       ((TextBox)_gridView.FooterRow.FindControl("txtPassedDateAdd")).Text);
            BindData();
            
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidEmployeeQualification(txtEmployeeNo.Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInstitutionNameEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCertificateEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPercentEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtGradeEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDurationEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPassedDateEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }


            DateTime fromdate = Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromEdit")).Text);
            DateTime todate = Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToEdit")).Text);
            int diffyear = (todate.Year - fromdate.Year);
            int diffDate = diffyear * 12 + (todate.Month - fromdate.Month);
            ((TextBox)_gridView.FooterRow.FindControl("txtDurationAdd")).Text = diffDate.ToString();

            UpdateEmployeeQualification(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQualificationidEdit")).Text),
                                                        txtEmployeeNo.Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInstitutionNameEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCertificateEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPercentEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtGradeEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDurationEdit")).Text,
                                                       ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPassedDateEdit")).Text);
            _gridView.EditIndex = -1;
            BindData();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteEmployeeQualification(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQualificationid")).Text));
        }
        else
        {
            _gridView.EditIndex = -1;
            BindData();
        }

    }
    public void InsertEmployeeQualification(int rowusercode,
        string employeeno,
        string institutionname,
        string place,
        string certificate,
        string percent,
        string grade,
        string from,
        string to,
        string duration,
        string passeddate)
    {

        PhoenixCrewEmployeeQualification.InsertEmployeeQualification(rowusercode,Convert.ToInt32(employeeno),
                                            institutionname, place, certificate, Convert.ToDecimal( percent), grade,
                                             Convert.ToDateTime(from), Convert.ToDateTime(to), Convert.ToInt32(duration),
                                             Convert.ToDateTime (passeddate));
    }

    public void UpdateEmployeeQualification(int rowusercode,
       int qualificationid,
       string employeeno,
       string institutionname,
       string place,
       string certificate,
       string percent,
       string grade,
       string from,
       string to,
       string duration,
       string passeddate)
    {
       
        PhoenixCrewEmployeeQualification.UpdateEmployeeQualification(rowusercode,qualificationid, Convert.ToInt32(employeeno),
                                            institutionname, place, certificate, Convert.ToDecimal(percent), grade,
                                             Convert.ToDateTime(from), Convert.ToDateTime(to), Convert.ToInt32(duration), 
                                             Convert.ToDateTime(passeddate));
    }

    public void DeleteEmployeeQualification(int rowusercode, int qualificationid)
    {
        PhoenixCrewEmployeeQualification.DeleteEmployeeQualification(rowusercode, qualificationid);
    }


    private bool IsValidEmployeeQualification(string employeeno,
        string institutionname,
        string place,
        string certificate,
        string percent,
        string grade,
        string from,
        string to,
        string duration,
        string passeddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gdEmployeeQualification;


        if (institutionname.Trim().Equals(""))
            ucError.ErrorMessage = "Institute Name is required.";

        if (place.Trim().Equals(""))
            ucError.ErrorMessage = "Place is required.";

        if (certificate.Trim().Equals(""))
            ucError.ErrorMessage = "Certificate is required.";

        if (percent.Trim().Equals(""))
            ucError.ErrorMessage = "Parent is required.";

        if (grade.Trim().Equals(""))
            ucError.ErrorMessage = "Grade is required.";

        if (from.Trim().Equals(""))
            ucError.ErrorMessage = "From Date is required.";

        if (to.Trim().Equals(""))
            ucError.ErrorMessage = "To is required.";

        if (duration.Trim().Equals(""))
            ucError.ErrorMessage = "Duration is required.";

        if (passeddate.Trim().Equals(""))
            ucError.ErrorMessage = "Passed Date is required.";


        return (!ucError.IsError);
    }

    protected void gdEmployeeQualification_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }



    protected void gdEmployeeQualification_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
				ImageButton cme = (ImageButton)e.Row.FindControl("cmdEdit");
				if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            }
        }
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton imgbt = (ImageButton)e.Row.FindControl("cmdAdd");
			if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;
		}				

    }

    protected void cmdGo_Click(object sender, EventArgs e)
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gdEmployeeQualification.SelectedIndex = -1;
        gdEmployeeQualification.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

   
   

}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewUserMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarMain = new PhoenixToolbar();
        toolbarMain.AddButton("Back", "BACK");
        CrewUserMain.AccessRights = this.ViewState;
        CrewUserMain.MenuList = toolbarMain.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Crew/CrewUserMapping.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgUser')", "Print Grid", "icon_print.png", "PRINT");
        //toolbar.AddImageButton("../Crew/CrewUserMapping.aspx", "Find", "search.png", "FIND");

        MenuCrewUsers.MenuList = toolbar.Show();
        MenuCrewUsers.SetTrigger(pnlUserEntry);
        MenuCrewUsers.AccessRights = this.ViewState;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["TOTALPAGECOUNT"] = 0;
            ViewState["ROWCOUNT"] = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["VESSEL"].ToString()))
            {
                ViewState["VESSELID"] = Request.QueryString["VESSEL"].ToString();
                ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                ddlVessel.bind();
                ddlVesselCode.VesselId = int.Parse(ViewState["VESSELID"].ToString());
                ddlVesselCode.bind();
                BindData();
                SetPageNavigator();
            }
            else
            {
                divPage.Visible = false;
            }
        }


        //imgOnBoardEmployee.Attributes.Add("onclick",
        //     "return showPickList('spnReportedByShip1', 'codehelp1', '','../Common/CommonPickListCrewOnboardWithRank.aspx?VesselId="
        //     + ucVessel.SelectedVessel + "&date=" + txtDateOfIncident.Text + "', true); ");
    }

    protected void CrewUserMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewUsers_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        //if (!IsValidFilter())
        //{
        //    ucError.Visible = true;
        //    return;
        //}
        //if (dce.CommandName.ToUpper().Equals("FIND"))
        //{
        //    ViewState["PAGENUMBER"] = 1;
        //    BindData();
        //    SetPageNavigator();
        //}
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDUSERCODE", "FLDUSERNAME", "FLDGROUPLIST", "FLDACTIVEYN", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDEFFECTIVEFROM" };
        string[] alCaptions = { "User code", "User Name", "Group List", "Active YN", "Firstname", "Lastname", "middlename", "Effective From" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewUserMapping.CrewUserSearch(int.Parse(ddlVessel.SelectedVessel)
            , ddlVesselCode.SelectedVesselCode,
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewUser.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>User Register</h3></td>");
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();

            string[] alColumns = { "FLDUSERCODE", "FLDUSERNAME", "FLDGROUPLIST", "FLDACTIVEYN", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDEFFECTIVEFROM" };
            string[] alCaptions = { "User code", "User Name", "Group List", "Active Y/N", "First Name", "Last Name", "Middle Name", "Effective From" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCrewUserMapping.CrewUserSearch(int.Parse(ddlVessel.SelectedVessel)
                , ddlVesselCode.SelectedVesselCode,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("dgUser", "User List", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dgUser.DataSource = ds;
                dgUser.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, dgUser);
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void dgUser_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (Label)e.Row.FindControl("lblUser");

                Label lb2 = (Label)e.Row.FindControl("lblUserName");

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);


                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }

                TextBox txtEmpId = (TextBox)e.Row.FindControl("txtEmployeeIdEdit");
                if (txtEmpId != null) txtEmpId.Attributes.Add("style", "visibility:hidden");

                Image imgEmp = (Image)e.Row.FindControl("imgOnBoardEmployeeEdit");
                if (imgEmp != null)
                    imgEmp.Attributes.Add("onclick", "return showPickList('spnOnBoardEmployee', 'codehelp1', '','../Common/CommonPickListCrewOnboardWithRank.aspx?VesselId=" + ViewState["VESSELID"].ToString() + "', false); ");

                ImageButton rb = (ImageButton)e.Row.FindControl("cmdReset");
                if (rb != null)
                {
                    rb.Attributes.Add("onclick", "Openpopup('codehelp2', '', 'CrewUserResetPassword.aspx?usercode=" + lbl.Text + "&username=" + lb2.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "')");
                    rb.Visible = SessionUtil.CanAccess(this.ViewState, rb.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgUser_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void dgUser_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void dgUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                string EmpId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmployeeIdEdit")).Text;
                string EffectDate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("EffectFromDateEdit")).Text;
                string userid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblUserCode")).Text;
                string shortcode = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblShortCode")).Text;

                if (!IsValidUserNameMapping(EmpId, EffectDate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewUserMapping.UpdateCrewUserNameMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , int.Parse(userid)
                                                                , General.GetNullableDateTime(EffectDate)
                                                                , int.Parse(EmpId)
                                                                , int.Parse(ViewState["VESSELID"].ToString())
                                                                , General.GetNullableString(shortcode));

                _gridView.EditIndex = -1;
                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
        dgUser.SelectedIndex = -1;
        dgUser.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        divPage.Visible = true;
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
        
        SetPageNavigator();
    }

    protected void ddlVesselChanged(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = ddlVessel.SelectedVessel;
        if (ddlVessel.SelectedVessel.ToUpper() != "DUMMY")
        {
            ddlVesselCode.VesselId = int.Parse(ddlVessel.SelectedVessel);
            ddlVesselCode.bind();

            BindData();
        }
    }

    protected void ddlVesselCodeChanged(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    //public bool IsValidFilter()
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";

    //    if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
    //        ucError.ErrorMessage = "Vessel is required.";

    //    if (General.GetNullableString(ddlVesselCode.SelectedVesselCode) == null)
    //        ucError.ErrorMessage = "VesselCode is required.";

    //    return (!ucError.IsError);
    //}

    public bool IsValidUserNameMapping(string empid, string effectDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(empid) == null)
            ucError.ErrorMessage = "Employee Name is required.";

        if (General.GetNullableDateTime(effectDate) == null)
            ucError.ErrorMessage = "Effective from date is required.";

        return (!ucError.IsError);
    }

}

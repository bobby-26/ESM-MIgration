using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

//personnel master
public partial class InspectionProsperScorePersonnelMaster : PhoenixBasePage
{
    string empid;

    string strPreviousRowID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                empid = Request.QueryString["empid"];
                bindcyclelist();
                BindTenureCycleList();
                gvProsperemplist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvEmployeeProsper.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                //ViewState["TAB"] = "YES";
                //if (Request.QueryString["notab"] != null)
                //{
                //    ViewState["TAB"] = "NO";
                //}
            }
            //if (ViewState["TAB"].ToString() == "YES")
            //{
                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                string id = filterprosper.CurrentSelectedProsperEmployee;
                toolbarmain.AddButton("Events", "EVENTS");
                toolbarmain.AddButton("OC 28", "OC28");
                toolbarmain.AddButton("Score", "SCORE");
                MenuProgress.AccessRights = this.ViewState;
                MenuProgress.MenuList = toolbarmain.Show();
                MenuProgress.SelectedMenuIndex = 2;
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void QualityProgress_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                SetSelectedTab("BACK");
            }
            else if (CommandName.ToUpper().Equals("OC28"))
            {
                SetSelectedTab("OC28");
            }
            else if (CommandName.ToUpper().Equals("EVENTS"))
            {
                SetSelectedTab("EVENTS");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void SetSelectedTab(string currenttab)
    {
        if (currenttab == "OC28")
        {
            Response.Redirect("../Crew/CrewEmployeeSuperintendentAssessment.aspx", false);
        }
        if (currenttab == "EVENTS")
        {
            Response.Redirect("../Crew/CrewInspectionSupdtConcernsList.aspx", false);
        }
        if (currenttab == "BACK")
        {

            ViewState["URL"] = "../Inspection/InspectionProsperReports.aspx";
            Response.Redirect(ViewState["URL"].ToString(), false);
        }
    }

    public void bindcyclelist()
    {
        DataTable dt = PhoenixProsper.ProsperCycleList(General.GetNullableInteger(Request.QueryString["empid"]));
        if (dt.Rows.Count > 0)
        {
            ddlcycle.DataSource = dt;
            ddlcycle.DataBind();
            ddlcycle.SelectedValue = dt.Rows[0]["FLDCYCLEID"].ToString();
            //ddlcycle.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            //ddlcycle.SelectedIndex = 0;
        }
        else
        {
            ddlcycle.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            ddlcycle.SelectedIndex = 0;
        }
    }

    public void BindTenureCycleList()
    {
        DataTable dt = PhoenixProsper.ProsperTenureCycleList(General.GetNullableInteger(ddlcycle.SelectedValue.ToString()));
        if (dt.Rows.Count > 0)
        {
            ddltenurecycle.DataSource = dt;
            ddltenurecycle.DataBind();
            ddltenurecycle.SelectedValue = dt.Rows[0]["FLDTENURECYCLEID"].ToString();
            BindDataScore();
        }
        else
        {
            ddltenurecycle.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            ddltenurecycle.SelectedIndex = 0;
        }
    }
    public void BindDataScore()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        try
        {
            DataSet ds = PhoenixProsper.ProsperEmployeescoresearch(
                            General.GetNullableInteger(Request.QueryString["empid"])
                            , General.GetNullableInteger(ddlcycle.SelectedValue.ToString())
                            , General.GetNullableGuid(ddltenurecycle.SelectedValue.ToString()));
            
            gvEmployeeProsper.DataSource = ds.Tables[0];

            gvEmployeeProsper.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
            gvEmployeeProsper.MasterTableView.GetColumn("ACTUAL").FooterText = "Total";
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEmployeeProsper.MasterTableView.GetColumn("CALCULATED").FooterText = ds.Tables[1].Rows[0]["FLDTOTALSCORE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmployeeProsper_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindDataScore();
    }

    protected void gvEmployeeProsper_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvProsperemplist_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProsperemplist.CurrentPageIndex + 1;

        BindData();
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixProsper.ProsperEmployeeList(General.GetNullableInteger(Request.QueryString["empid"]));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtfromdate.Text = dt.Rows[0]["FLDCYCLESTARTDATE"].ToString();
                txttodate.Text = dt.Rows[0]["FLDCYCLEENDDATE"].ToString();
                lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            }
            else
            {
                dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

                if (dt.Rows.Count > 0)
                {
                    txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                    txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                    txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                    txtfromdate.Text = "";
                    txttodate.Text = "";
                    lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();

                }

            }

            //int? cycleid = 0;
            //if (ddlcycle.SelectedIndex == 0)
            //    cycleid = 0;
            //else
            //    cycleid = Convert.ToInt32(ddlcycle.SelectedValue.ToString());

            DataSet ds = PhoenixProsper.ProsperEmployeeWiseReport(
                             General.GetNullableInteger(Request.QueryString["empid"])
                            , General.GetNullableInteger(ddlcycle.SelectedValue.ToString())
                            , General.GetNullableGuid(ddltenurecycle.SelectedValue.ToString())
                            , sortexpression
                            , sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            , gvProsperemplist.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);


            gvProsperemplist.DataSource = ds.Tables[0];
            gvProsperemplist.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvProsperemplist.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                gvProsperemplist.MasterTableView.GetColumn("PSCDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDPSCDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("PSCDET").FooterText = ds.Tables[1].Rows[0]["SUMFLDPSCDET"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("VETTINGDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDVETTINGDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("VETTINGREJ").FooterText = ds.Tables[1].Rows[0]["SUMFLDVETTINGREJ"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("INCIDENTACATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDINCIDENTACATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("INCIDENTBCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDINCIDENTBCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("INCIDENTCCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDINCIDENTCCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("HNSACATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDHNSACATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("HNSBCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDHNSBCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("HNSCCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDHNSCCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("FEEDBACKPOSITIVE").FooterText = ds.Tables[1].Rows[0]["SUMFLDFEEDBACKPOSITIVE"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("FEEDBACKNEGATIVE").FooterText = ds.Tables[1].Rows[0]["SUMFLDFEEDBACKNEGATIVE"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("FEEDBACKWARNING").FooterText = ds.Tables[1].Rows[0]["SUMFLDFEEDBACKWARNING"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("TPIDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDTPIDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("TPIDET").FooterText = ds.Tables[1].Rows[0]["SUMFLDTPIDET"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("EXTDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDEXTDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("EXTDET").FooterText = ds.Tables[1].Rows[0]["SUMFLDEXTDET"].ToString();
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

    protected void gvProsperemplist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }


    protected void Prosper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddlcycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["EDIT"] = "1";
        BindTenureCycleList();
        gvProsperemplist.Rebind();
    }
    protected void ddltenurecycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["EDIT"] = "1";
        gvProsperemplist.Rebind();
    }


}

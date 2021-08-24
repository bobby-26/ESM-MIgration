using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewCompanyExperience : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Crew/CrewCompanyExperience.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewCompanyExperience')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewCompanyExperienceList.aspx?type=p&empid=" + Filter.CurrentCrewSelection + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWCOMPANYEXPERIENCE");
        MenuCrewCompanyExperience.AccessRights = this.ViewState;
        MenuCrewCompanyExperience.MenuList = toolbar.Show();

        PhoenixToolbar toolbarMenu = new PhoenixToolbar();
        MenuCompanyExp.AccessRights = this.ViewState;
        MenuCompanyExp.MenuList = toolbarMenu.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;         
            SetEmployeePrimaryDetails();

            gvCrewCompanyExperience.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDDAILYRATEUSD", "FLDPRINCIPALNAME", "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type", "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration",
                                      "Gap", "Employer", "Last drawn salary/day(USD)", "Owner", "SignOff Reason", "Ice Experience" };
        }
        else
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDEMPLOYEENAME" , "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT", "FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY","FLDPRINCIPALNAME", "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type","Employee", "Rank", "DWT", "GRT", "TEU", "Engine Type / Model", "From", "To", "Duration",
                                      "Gap", "Employer", "Owner", "SignOff Reason", "Ice Experience" };
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewCompanyExperience.CrewCompanyExperienceSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        General.ShowExcel("Company Experience", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void CrewCompanyExperience_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME","FLDEMPLOYEENAME" , "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDDAILYRATEUSD", "FLDDPALLOWANCE", "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type", "Employee" , "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration",
                                      "Gap", "Employer", "Last drawn salary/day(USD)", "DP Allowance/day (USD)", "SignOff Reason", "Ice Experience" };
        }
        else
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDEMPLOYEENAME" , "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type","Employee" , "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "From", "To", "Duration",
                                      "Gap", "Employer", "SignOff Reason", "Ice Experience" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewCompanyExperience.CrewCompanyExperienceSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvCrewCompanyExperience.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("gvCrewCompanyExperience", "Company Experience", alCaptions, alColumns, ds);

        gvCrewCompanyExperience.DataSource = ds;
        gvCrewCompanyExperience.VirtualItemCount = iRowCount;

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            gvCrewCompanyExperience.MasterTableView.GetColumn("LASTSALARY").Display = true;
            gvCrewCompanyExperience.MasterTableView.GetColumn("OWNER").Display = true;
        }
        else
        {
            gvCrewCompanyExperience.MasterTableView.GetColumn("LASTSALARY").Display = false;
            gvCrewCompanyExperience.MasterTableView.GetColumn("OWNER").Display = false;
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCompanyExperience_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCompanyExperience.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvCrewCompanyExperience_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            RadLabel l = (RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkRank");
            RadLabel lblsignonoff = (RadLabel)e.Item.FindControl("lblsignonoff");

            if (lb != null)
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('NAFA', '', '" + Session["sitepath"] + "/Crew/CrewCompanyExperienceList.aspx?type=p&empid=" + Filter.CurrentCrewSelection + "&CrewCompanyExperienceId=" + l.Text + "&sid=" + lblsignonoff.Text + "');return false;");
                lb.Enabled = SessionUtil.CanAccess(this.ViewState, "EDIT");
            }

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdXAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.THREEAFORMYN + "&cmdname=THREEAFORMUPLOAD'); return false;");
            }

            LinkButton cmdCargoAdd = (LinkButton)e.Item.FindControl("cmdCargoAdd");
            if (cmdCargoAdd != null)
            {
                RadLabel lblFromDate = (RadLabel)e.Item.FindControl("lblFromDate");
                RadLabel lbToDate = (RadLabel)e.Item.FindControl("lbToDate");
                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");

                cmdCargoAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdCargoAdd.CommandName);

                cmdCargoAdd.Attributes.Add("onclick"
                , "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/VesselPosition/VesselPositionCargoExperienceList.aspx?vesselid="
                + lblVesselId.Text + "&fromdate=" + lblFromDate.Text + "&todate=" + lbToDate.Text + "');return false;");
            }

            RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");
            LinkButton cmdMove = (LinkButton)e.Item.FindControl("cmdMove");
            if (cmdMove != null)
            {
                cmdMove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to move the company experience to other experience?')");
                cmdMove.Visible = SessionUtil.CanAccess(this.ViewState, cmdMove.CommandName);
            }
            if (string.IsNullOrEmpty(lblVessel.Text))
            {
                if (cmdMove != null) cmdMove.Visible = true;
            }
            else
            {
                if (cmdMove != null) cmdMove.Visible = false;
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
            RadCheckBox cb = ((RadCheckBox)e.Item.FindControl("chkVerified"));

            if (cb != null)
            {
                if (cb.Checked == true)
                {
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");                  
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                }
                else
                {
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicenseUnVerified");              
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                }
            }


            RadLabel lblManningCompanyName = (RadLabel)e.Item.FindControl("lblManningCompanyName");            

            if (lblManningCompanyName != null)
            {
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucManningCompanyTT");
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lblManningCompanyName.ClientID;             
            }
        }

        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem header = (GridHeaderItem)e.Item;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                header["FromDate"].Text = "Sign On Date";
                header["ToDate"].Text = "Sign Off Date";
            }

        }

    }

    protected void gvCrewCompanyExperience_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string CompanyExpid = ((RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId")).Text;

        if (General.GetNullableInteger(CompanyExpid) != null)
        {
            PhoenixCrewCompanyExperience.DeleteCrewCompanyExperience(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
           , Int32.Parse(CompanyExpid), Int32.Parse(Filter.CurrentCrewSelection.ToString()));

            BindData();
            gvCrewCompanyExperience.Rebind();
        }

    }

    protected void gvCrewCompanyExperience_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvCrewCompanyExperience_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT")) return;

        if (e.CommandName.ToUpper().Equals("MOVETOOTHEREXPERIENCE"))
        {
            PhoenixCrewOffshoreEmployee.MoveCompanyExperience(General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId")).Text));
            BindData();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewCompanyExperience_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string CompanyExpid = ((RadLabel)e.Item.FindControl("lblCrewCompanyExperienceIdEdit")).Text;
            
            string chk = "0";

            if (((RadCheckBox)e.Item.FindControl("chkVerifiedEdit")).Checked == true)
                chk = "1";

            PhoenixCrewCompanyExperience.UpdateCrewCompanyExperienceThreeYN(
                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , Convert.ToInt32(CompanyExpid)
                                                , byte.Parse(chk));

            BindData();
            gvCrewCompanyExperience.Rebind();
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
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvCrewCompanyExperience_SortCommand(object sender, GridSortCommandEventArgs e)
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
}

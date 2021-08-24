using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOtherExperience : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewOtherExperience.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewOtherExperience')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" + Filter.CurrentCrewSelection + "&type=p" + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWOTHEREXPERIENCE");
            }
            else
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" + Filter.CurrentCrewSelection + "&type=p" + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWOTHEREXPERIENCE");
            }
            MenuCrewOtherExperience.AccessRights = this.ViewState;
            MenuCrewOtherExperience.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarMenu = new PhoenixToolbar();
            MenuCompanyExp.AccessRights = this.ViewState;
            MenuCompanyExp.MenuList = toolbarMenu.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                DataSet ds = PhoenixRegistersThirdPartyLinks.EditThirdPartyLinks(3);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    equasis.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;             
                ViewState["EMPLOYEEOTHEREXPERIENCEID"] = null;
                SetEmployeePrimaryDetails();

                gvCrewOtherExperience.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        BindData();
    }
   
    protected void MenuCrewOtherExperience_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSEL", "FLDTYPEDESCRIPTION", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT", "FLDVESSELTEU", "FLDVESSELBHP", "FLDENGINETYPEMODEL", "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDSIGNOFFREASONNAME" };
                string[] alCaptions = { "Vessel", "Vessel Type", "Rank", "DWT", "GRT", "TEU", "BHP", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration", "Gap", "Employer", "SignOff Reason" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


                DataSet ds = PhoenixCrewOtherExperience.SearchEmployeeOtherExperience(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                                 sortexpression, sortdirection,
                                                                                       1,
                                                                                       iRowCount,
                                                                                       ref iRowCount,
                                                                                       ref iTotalPageCount);

                General.ShowExcel("Crew Other Experience", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindData()
    {

        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSEL", "FLDTYPEDESCRIPTION", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT", "FLDVESSELTEU", "FLDVESSELBHP", "FLDENGINETYPEMODEL", "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDSIGNOFFREASONNAME" };
            string[] alCaptions = { "Vessel", "Vessel Type", "Rank", "DWT", "GRT", "TEU", "BHP", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration", "Gap", "Employer", "SignOff Reason" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewOtherExperience.SearchEmployeeOtherExperience(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                             sortexpression, sortdirection,
                                                                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                             gvCrewOtherExperience.PageSize,
                                                                             ref iRowCount,
                                                                             ref iTotalPageCount);

            General.SetPrintOptions("gvCrewOtherExperience", "Crew Other Experience", alCaptions, alColumns, ds);


            gvCrewOtherExperience.DataSource = ds;            
            gvCrewOtherExperience.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    

    protected void gvCrewOtherExperience_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewOtherExperience.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvCrewOtherExperience_ItemCommand(object sender, GridCommandEventArgs e)
    {
       if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewOtherExperience_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel ll = (RadLabel)e.Item.FindControl("lblGrtNumber");
            if (ll != null && !string.IsNullOrEmpty(ll.Text))
                ll.Text = ll.Text.Substring(0, ll.Text.IndexOf('.'));

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }


            RadLabel l = (RadLabel)e.Item.FindControl("lblEmployeeExpid");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkRank");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null) if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                        Filter.CurrentCrewSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                    Filter.CurrentCrewSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
            }
            else
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" +
            Filter.CurrentCrewSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" +
                    Filter.CurrentCrewSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
            }
            
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblManningCompanyName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucManningCompanyTT");
            if (lbtn != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

        }

        
    }

    
    protected void gvCrewOtherExperience_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string employeeexperienceid = ((RadLabel)e.Item.FindControl("lblEmployeeExpid")).Text;
            ViewState["EMPLOYEEOTHEREXPERIENCEID"] = null;
            PhoenixCrewOtherExperience.DeleteEmployeeOtherExperience(Convert.ToInt32(Filter.CurrentCrewSelection), Convert.ToInt32(employeeexperienceid));
            ResetFormControlValues(this);
            SetEmployeePrimaryDetails();

            BindData();
            gvCrewOtherExperience.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    
    private void ResetFormControlValues(Control parent)
    {
        try
        {
            ViewState["EMPLOYEEOTHEREXPERIENCEID"] = null;
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvCrewOtherExperience_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    

    protected void gvCrewOtherExperience_SortCommand(object sender, GridSortCommandEventArgs e)
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

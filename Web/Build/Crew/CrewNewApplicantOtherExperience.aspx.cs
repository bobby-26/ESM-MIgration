using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewNewApplicantOtherExperience : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantOtherExperience.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewOtherExp')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=n" + "'); return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADDCREWOTHEREXPERIENCE");
            }
            else
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=p" + "'); return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADDCREWOTHEREXPERIENCE");
            }

            MenuCrewOtherExperience.AccessRights = this.ViewState;
            MenuCrewOtherExperience.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbargrid1 = new PhoenixToolbar();
            TabStrip1.AccessRights = this.ViewState;
            TabStrip1.MenuList = toolbargrid1.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                DataSet ds = PhoenixRegistersThirdPartyLinks.EditThirdPartyLinks(3);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    equasis.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["EMPLOYEEOTHEREXPERIENCEID"] = null;
                SetEmployeePrimaryDetails();
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
            string[] alColumns = { "FLDVESSEL", "FLDTYPEDESCRIPTION", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT", "FLDVESSELTEU", "FLDENGINETYPEMODEL", "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDSIGNOFFREASONNAME" };
            string[] alCaptions = { "Vessel", "Vessel Type", "Rank", "DWT", "GRT", "TEU", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration", "Gap", "Employer", "SignOff Reason" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = new DataTable();

            dt = PhoenixNewApplicantOtherExperience.SearchEmployeeOtherExperience(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                    , sortexpression, sortdirection, Convert.ToInt32(ViewState["PAGENUMBER"].ToString()), gvCrewOtherExp.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewOtherExp", "Other Experience", alCaptions, alColumns, ds);

            gvCrewOtherExp.DataSource = dt;
            gvCrewOtherExp.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        gvCrewOtherExp.Rebind();
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
                string[] alColumns = { "FLDVESSEL", "FLDTYPEDESCRIPTION", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT", "FLDVESSELTEU", "FLDENGINETYPEMODEL", "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDSIGNOFFREASONNAME" };
                string[] alCaptions = { "Vessel", "Vessel Type", "Rank", "DWT", "GRT", "TEU", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration", "Gap", "Employer", "SignOff Reason" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataTable dt = PhoenixNewApplicantOtherExperience.SearchEmployeeOtherExperience(Convert.ToInt32(Filter.CurrentNewApplicantSelection),
                                                                                 sortexpression, sortdirection,
                                                                                       1,
                                                                                       General.ShowRecords(null),
                                                                                       ref iRowCount,
                                                                                       ref iTotalPageCount);
                General.ShowExcel("Crew Other Experience", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvCrewOtherExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label ll = (Label)e.Row.FindControl("lblGrtNumber");
            if (ll != null && !string.IsNullOrEmpty(ll.Text))
                ll.Text = ll.Text.Substring(0, ll.Text.IndexOf('.'));
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                Label l = (Label)e.Row.FindControl("lblEmployeeExpid");

                LinkButton lb = (LinkButton)e.Row.FindControl("lnkRank");
                //lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewOtherExperienceList.aspx?empid=" +
                //    Filter.CurrentNewApplicantSelection +"&type=n"+ "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
                {
                    lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '',  '" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                    db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                        Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                }
                else
                {
                    lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" +
                Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                    db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" +
                        Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                }
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                    Filter.CurrentNewApplicantSelection + "&type=n" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");

                if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);

                Label lbtn = (Label)e.Row.FindControl("lblManningCompanyName");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucManningCompanyTT");

                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;

                //if (lbtn != null)
                //{
                //    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                //}
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;
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

    protected void gvCrewOtherExp_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.NewEditIndex;
            _gridView.SelectedIndex = nCurrentRow;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewOtherExp_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }



    private void DeleteEmployeeOtheExperience(string employeeid, string employeeexperienceid)
    {
        try
        {
            PhoenixNewApplicantOtherExperience.DeleteEmployeeOtherExperience(Convert.ToInt32(employeeid), Convert.ToInt32(employeeexperienceid));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewOtherExp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string employeeexperienceid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeExpid")).Text;
            DeleteEmployeeOtheExperience(Filter.CurrentNewApplicantSelection, employeeexperienceid);
            ViewState["EMPLOYEEOTHEREXPERIENCEID"] = null;
            SetEmployeePrimaryDetails();
            _gridView.SelectedIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewOtherExp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewOtherExp.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewOtherExp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel ll = (RadLabel)e.Item.FindControl("lblGrtNumber");
            if (ll != null && !string.IsNullOrEmpty(ll.Text))
                ll.Text = ll.Text.Substring(0, ll.Text.IndexOf('.'));
        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            RadLabel l = (RadLabel)e.Item.FindControl("lblEmployeeExpid");

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkRank");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
            {
                lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
            Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                db1.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                    Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
            }
            else
            {
                lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" +
            Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
                db1.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?empid=" +
                    Filter.CurrentNewApplicantSelection + "&type=p" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");
            }
            db1.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?empid=" +
                Filter.CurrentNewApplicantSelection + "&type=n" + "&CREWOTHEREXPERIENCEID=" + l.Text + "');return false;");

            if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblManningCompanyName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucManningCompanyTT");

            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;
            //if (lbtn != null)
            //{
            //    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            //    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            //}
        }
    }

    protected void gvCrewOtherExp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "DELETE")
        {
            string employeeexperienceid = ((RadLabel)e.Item.FindControl("lblEmployeeExpid")).Text;
            DeleteEmployeeOtheExperience(Filter.CurrentNewApplicantSelection, employeeexperienceid);
            ViewState["EMPLOYEEOTHEREXPERIENCEID"] = null;
            SetEmployeePrimaryDetails();
            gvCrewOtherExp.EditIndexes.Clear();
            BindData();
            gvCrewOtherExp.Rebind();
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}

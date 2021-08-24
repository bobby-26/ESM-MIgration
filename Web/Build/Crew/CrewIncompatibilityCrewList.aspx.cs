using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class CrewIncompatibilityCrewList : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageButton("", "Filter", "search.png", "SEARCH");
            CrewQueryMenu.MenuList = toolbarsub.Show();
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            if (!IsPostBack)
            {

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Compatibility", "COMPATIBILITY");
                toolbarmain.AddButton("Employee List ", "EMPLOYEE");
                CrewIncidents.MenuList = toolbarmain.Show();
                CrewIncidents.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                SetEmployeePrimaryDetails();

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewIncidents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("COMPATIBILITY"))
        {
            Response.Redirect("CrewIncompatibilityList.aspx?empid=" + Request.QueryString["empid"], false);
        }
        else if (CommandName.ToUpper().Equals("EMPLOYEE"))
        {
            Response.Redirect("CrewIncompatibilityCrewList.aspx?empid=" + Request.QueryString["empid"], false);
        }
    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ResetFormControlValues(this);
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            string empid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (strEmployeeId == empid)
            {              
                db.Visible = false;
            }
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            GridDataItem Item = e.Item as GridDataItem;
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "ADD")
            {
                string empid = ((RadLabel)Item.FindControl("lblEmployeeid")).Text;
                PhoenixCrewIncompatibility.InsertIncompatibility(int.Parse(strEmployeeId), int.Parse(empid));
                ucStatus.Text = "Added to Incompatibility List";

                BindData();
                gvCrewSearch.Rebind();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "ADD")
            {
                string empid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
                PhoenixCrewIncompatibility.InsertIncompatibility(int.Parse(strEmployeeId), int.Parse(empid));
                ucStatus.Text = "Added to Incompatibility List";
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

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCommonCrew.QueryActivity(string.Empty, string.Empty, string.Empty, string.Empty
                                                                   , ddlRank.SelectedRank
                                                                   , General.GetNullableInteger(string.Empty)
                                                                   , General.GetNullableInteger(string.Empty)
                                                                   , string.Empty
                                                                   , General.GetNullableInteger(string.Empty)
                                                                   , General.GetNullableInteger(string.Empty)
                                                                   , General.GetNullableInteger(string.Empty)
                                                                   , General.GetNullableInteger(string.Empty)
                                                                   , General.GetNullableDateTime(string.Empty)
                                                                   , General.GetNullableDateTime(string.Empty)
                                                                   , General.GetNullableDateTime(string.Empty)
                                                                   , General.GetNullableDateTime(string.Empty), null, null
                                                                   , string.Empty, txtName.Text
                                                                   , null
                                                                   , string.Empty
                                                                   , null
                                                                   , null, null, null
                                                                   , sortexpression, sortdirection
                                                                   , gvCrewSearch.CurrentPageIndex + 1
                                                                   , gvCrewSearch.PageSize
                                                                   , ref iRowCount, ref iTotalPageCount
                                                                   , string.Empty, null, null);




            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
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

}

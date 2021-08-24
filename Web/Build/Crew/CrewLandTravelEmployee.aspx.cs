using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLandTravelEmployee : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Add ", "ADD", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuEmployeeList.AccessRights = this.ViewState;
            MenuEmployeeList.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvEmployee.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBERCF"] = 1;
                ViewState["SORTEXPRESSIONCF"] = null;
                ViewState["SORTDIRECTIONCF"] = null;

                gvEmployeeFamily.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBERU"] = 1;
                ViewState["SORTEXPRESSIONU"] = null;
                ViewState["SORTDIRECTIONU"] = null;

                gvUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["REQUESTID"] = null;

                if (Request.QueryString["requestid"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
                }
            }
            setVisibility();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void rblPassengerType_Changed(object sender, EventArgs e)
    {
        setVisibility();
    }


    protected void setVisibility()
    {
        if (rblPassengerType.SelectedValue == "1")
        {
            EmployeeTable.Visible = true;
            gvEmployee.Visible = true;

            EmployeeFamily.Visible = false;
            gvEmployeeFamily.Visible = false;

            tblConfigureUser.Visible = false;
            gvUser.Visible = false;

            otheruser.Visible = false;
        }
        else if (rblPassengerType.SelectedValue == "2")
        {
            EmployeeTable.Visible = false;
            gvEmployee.Visible = false;

            EmployeeFamily.Visible = true;
            gvEmployeeFamily.Visible = true;

            tblConfigureUser.Visible = false;
            gvUser.Visible = false;

            otheruser.Visible = false;
        }
        else if (rblPassengerType.SelectedValue == "3")
        {
            EmployeeTable.Visible = false;
            gvEmployee.Visible = false;

            EmployeeFamily.Visible = false;
            gvEmployeeFamily.Visible = false;

            tblConfigureUser.Visible = true;
            gvUser.Visible = true;

            otheruser.Visible = false;
        }
        else if (rblPassengerType.SelectedValue == "4")
        {
            EmployeeTable.Visible = false;
            gvEmployee.Visible = false;

            EmployeeFamily.Visible = false;
            gvEmployeeFamily.Visible = false;

            tblConfigureUser.Visible = false;
            gvUser.Visible = false;

            otheruser.Visible = true;
        }
    }

    protected void MenuEmployeeList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "SEARCH")
            {
                BindData();
                gvEmployee.Rebind();
            }
            if (CommandName.ToUpper() == "ADD")
            {
                if (rblPassengerType.SelectedValue == "1")
                    AddEmployee();
                else if (rblPassengerType.SelectedValue == "2")
                    AddEmployeeFamily();
                else if (rblPassengerType.SelectedValue == "3")
                    AddOfficeUser();
                else if (rblPassengerType.SelectedValue == "4")
                    AddPassenger();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddEmployee()
    {
        StringBuilder strEmployeeIdList = new StringBuilder();

        foreach (GridDataItem gvr in gvEmployee.Items)
        {
            RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheck");

            if (chkCheck != null)
            {
                if (chkCheck.Checked == true && chkCheck.Enabled == true)
                {
                    RadLabel lblEmployeeId = (RadLabel)gvr.FindControl("lblEmployeeId");

                    strEmployeeIdList.Append(lblEmployeeId.Text);
                    strEmployeeIdList.Append(",");
                }
            }
        }
        if (strEmployeeIdList.Length > 1)
        {
            strEmployeeIdList.Remove(strEmployeeIdList.Length - 1, 1);
        }

        if (!IsValidPassenger(strEmployeeIdList.ToString()))
        {
            ucError.Visible = true;
            BindData();
            return;
        }
        else
        {

            if (ViewState["REQUESTID"] != null)
            {
                PhoenixCrewLandTravelPassengers.InsertLandTravelPassengerEmployee(
                    General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                    , strEmployeeIdList.ToString()
                    , int.Parse(rblPassengerType.SelectedValue));
            }
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " fnReloadList();";
            Script += "</script>" + "\n";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
        }
    }

    protected void AddEmployeeFamily()
    {
        StringBuilder strFamilyIdList = new StringBuilder();

        foreach (GridDataItem gvr in gvEmployeeFamily.Items)
        {
            RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheck");

            if (chkCheck != null)
            {
                if (chkCheck.Checked == true && chkCheck.Enabled == true)
                {
                    RadLabel lblFamilyId = (RadLabel)gvr.FindControl("lblFamilyId");

                    strFamilyIdList.Append(lblFamilyId.Text);
                    strFamilyIdList.Append(",");
                }
            }
        }
        if (strFamilyIdList.Length > 1)
        {
            strFamilyIdList.Remove(strFamilyIdList.Length - 1, 1);
        }

        if (!IsValidPassenger(strFamilyIdList.ToString()))
        {
            ucError.Visible = true;
            BindData();
            return;
        }
        else
        {
            if (ViewState["REQUESTID"] != null)
            {
                PhoenixCrewLandTravelPassengers.InsertLandTravelPassengerEmployeeFamily(
                    General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                    , strFamilyIdList.ToString()
                    , int.Parse(rblPassengerType.SelectedValue));
            }
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " fnReloadList();";
            Script += "</script>" + "\n";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
        }
    }

    protected void AddOfficeUser()
    {
        StringBuilder strUserIdList = new StringBuilder();

        foreach (GridDataItem gvr in gvUser.Items)
        {
            RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheckUser");

            if (chkCheck != null)
            {
                if (chkCheck.Checked == true && chkCheck.Enabled == true)
                {
                    RadLabel lblUserCode = (RadLabel)gvr.FindControl("lblUser");

                    strUserIdList.Append(lblUserCode.Text);
                    strUserIdList.Append(",");
                }
            }
        }
        if (strUserIdList.Length > 1)
        {
            strUserIdList.Remove(strUserIdList.Length - 1, 1);
        }

        if (!IsValidPassenger(strUserIdList.ToString()))
        {
            ucError.Visible = true;
            BindData();
            return;
        }
        else
        {
            if (ViewState["REQUESTID"] != null)
            {
                PhoenixCrewLandTravelPassengers.InsertLandTravelPassengerOfficeUser(
                    General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                    , strUserIdList.ToString()
                    , int.Parse(rblPassengerType.SelectedValue));
            }
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " fnReloadList();";
            Script += "</script>" + "\n";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
        }
    }

    protected void AddPassenger()
    {
        if (!IsValidPassenger(txtPassengerName.Text, txtDesignation.Text))
        {
            ucError.Visible = true;
            return;
        }
        if (ViewState["REQUESTID"] != null)
        {
            PhoenixCrewLandTravelPassengers.InsertLandTravelPassenger(
                    General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                    , General.GetNullableString(txtPassengerName.Text)
                    , General.GetNullableString(txtDesignation.Text)
                    , int.Parse(rblPassengerType.SelectedValue));
        }
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += " fnReloadList();";
        Script += "</script>" + "\n";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewLandTravel.SearchLandTravelEmployees(General.GetNullableGuid(ViewState["REQUESTID"] == null ? "" : ViewState["REQUESTID"].ToString()),
                General.GetNullableString(txtName.Text)
                , General.GetNullableString(txtFileNo.Text)
                , General.GetNullableString(lstRank.selectedlist)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvEmployee.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvEmployee.DataSource = ds;
            gvEmployee.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvEmployee_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmployee.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvEmployee_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    
    protected void gvEmployee_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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

    protected void gvEmployee_SortCommand(object sender, GridSortCommandEventArgs e)
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


    private bool IsValidPassenger(string EmployeeIdList)
    {
        if (EmployeeIdList.Trim() == "")
        {
            ucError.ErrorMessage = "Please Select Atleast One Passenger.";
        }
        return (!ucError.IsError);
    }

    private bool IsValidPassenger(string name, string designation)
    {
        if (name.Trim() == "")
        {
            ucError.ErrorMessage = "Passenger Name is required.";
        }
        if (designation.Trim() == "")
        {
            ucError.ErrorMessage = "Designation is required.";
        }
        return (!ucError.IsError);
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }




    // Crew Family 

    private void BindDataCF()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONCF"] == null) ? null : (ViewState["SORTEXPRESSIONCF"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTIONCF"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCF"].ToString());

            if (ViewState["ROWCOUNTCF"] == null || Int32.Parse(ViewState["ROWCOUNTCF"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewLandTravel.SearchLandTravelEmployeeFamily(General.GetNullableGuid(ViewState["REQUESTID"] == null ? "" : ViewState["REQUESTID"].ToString()),
                General.GetNullableString(txtEmpFamilyName.Text)
                , General.GetNullableString(txtEmpFileNo.Text)
                , General.GetNullableString(lstEmpRank.selectedlist)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBERCF"]
                , gvEmployeeFamily.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvEmployeeFamily.DataSource = ds;
            gvEmployeeFamily.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTCF"] = iRowCount;
            ViewState["TOTALPAGECOUNTCF"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmployeeFamily_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERCF"] = ViewState["PAGENUMBERCF"] != null ? ViewState["PAGENUMBERCF"] : gvEmployeeFamily.CurrentPageIndex + 1;
        BindDataCF();
    }

    protected void gvEmployeeFamily_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvEmployeeFamily_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERCF"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvEmployeeFamily_SortCommand(object sender, GridSortCommandEventArgs e)
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

    // Office user

    private void BindDataUser()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string strdept = null;
        string sortexpression = (ViewState["SORTEXPRESSIONU"] == null) ? null : (ViewState["SORTEXPRESSIONU"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONU"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONU"].ToString());
        DataSet ds = new DataSet();

        strdept = General.GetNullableString(ucDepartment.SelectedDepartment);

        ds = PhoenixCrewLandTravel.SearchLandTravelOfficeUser(General.GetNullableGuid(ViewState["REQUESTID"] == null ? "" : ViewState["REQUESTID"].ToString()),
                               txtSearch.Text, null, null,
                               strdept,
                               "", txtFullName.Text, "", null,
                               sortexpression, sortdirection,
                               Int32.Parse(ViewState["PAGENUMBERU"].ToString()),
                               gvUser.PageSize,
                               ref iRowCount,
                               ref iTotalPageCount);

        gvUser.DataSource = ds;
        gvUser.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNTU"] = iRowCount;
        ViewState["TOTALPAGECOUNTU"] = iTotalPageCount;
    }

    protected void gvUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERU"] = ViewState["PAGENUMBERU"] != null ? ViewState["PAGENUMBERU"] : gvUser.CurrentPageIndex + 1;
        BindDataUser();
    }

    protected void gvUser_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERU"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvUser_SortCommand(object sender, GridSortCommandEventArgs e)
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

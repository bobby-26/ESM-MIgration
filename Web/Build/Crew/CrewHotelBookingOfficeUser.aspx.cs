using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;


public partial class CrewHotelBookingOfficeUser : PhoenixBasePage
{
    public PhoenixModule mod;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Add", "ADD", ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuUser.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["BOOKINGID"] = null;

            if (Request.QueryString["bookingid"] != null)
                ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();

            if (Request.QueryString["mod"] != null && Request.QueryString["mod"].ToString() != string.Empty)
                mod = (PhoenixModule)Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]);
            if (mod == PhoenixModule.QUALITY)
            {
                if (Request.QueryString["departmentlist"] != null && Request.QueryString["departmentlist"].ToString() != string.Empty)
                    ucDepartment.DepartmentFilter = Request.QueryString["departmentlist"].ToString();
            }

            gvUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvUser.Rebind();
            }
            if (CommandName.ToUpper() == "ADD")
            {
                StringBuilder strUserCodeList = new StringBuilder();

                foreach (GridDataItem gvr in gvUser.Items)
                {
                    RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheck");

                    if (chkCheck != null)
                    {
                        if (chkCheck.Checked == true && chkCheck.Enabled == true)
                        {
                            RadLabel lblUserCode = (RadLabel)gvr.FindControl("lblUserCode");

                            strUserCodeList.Append(lblUserCode.Text);
                            strUserCodeList.Append(",");
                        }
                    }
                }
                if (strUserCodeList.Length > 1)
                {
                    strUserCodeList.Remove(strUserCodeList.Length - 1, 1);
                }
                if (!IsValidHotelGuestRequest(strUserCodeList.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {

                    if (ViewState["BOOKINGID"] != null)
                    {
                        PhoenixCrewHotelBookingGuests.InsertHotelBookingGuestsOffice(
                           General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                         , strUserCodeList.ToString()
                        );
                    }
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " fnReloadList();";
                    Script += "</script>" + "\n";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidHotelGuestRequest(string UserCodeList)
    {
        if (UserCodeList.Trim() == "")
        {
            ucError.ErrorMessage = "Please select atleast one person";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvUser.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string strdept = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        if (Request.QueryString["mod"] != null && Request.QueryString["mod"].ToString() != string.Empty)
            mod = (PhoenixModule)Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]);
        if (mod == PhoenixModule.QUALITY && Request.QueryString["departmentlist"] != null && Request.QueryString["departmentlist"].ToString() != string.Empty)
        {
            if (General.GetNullableString(ucDepartment.SelectedDepartment) == null)
                strdept = Request.QueryString["departmentlist"].ToString();
            else
                strdept = General.GetNullableString(ucDepartment.SelectedDepartment);
        }
        else
        {
            strdept = General.GetNullableString(ucDepartment.SelectedDepartment);
        }

        ds = PhoenixCrewHotelBookingGuests.SearchHotelBookingRequestOfficeUser(General.GetNullableGuid(ViewState["BOOKINGID"] == null ? "" : ViewState["BOOKINGID"].ToString()),
                               txtSearch.Text, null, null,
                               strdept,
                               "", txtFullName.Text, "", null,
                               sortexpression, sortdirection,
                               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                               gvUser.PageSize,
                               ref iRowCount,
                               ref iTotalPageCount);


        gvUser.DataSource = ds;
        gvUser.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = new NameValueCollection();

                RadLabel lblFirstName = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblFirstName");
                RadLabel lblLastName = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblLastName");
                nvc.Add(lblFirstName.ID, lblFirstName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDesignation");
                nvc.Add(lblLastName.ID, lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblUserCode");
                nvc.Add(lblUserCode.ID, lblUserCode.Text);
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblFirstName = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblFirstName");
                RadLabel lblLastName = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblLastName");
                nvc.Set(nvc.GetKey(1), lblFirstName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDesignation");
                nvc.Set(nvc.GetKey(2), lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblUserCode");
                nvc.Set(nvc.GetKey(3), lblUserCode.Text);
                RadLabel lblEmail = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblEmail");
                nvc.Set(nvc.GetKey(4), lblEmail.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUser.CurrentPageIndex + 1;
            BindData();
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvUser.Rebind();
    }


}

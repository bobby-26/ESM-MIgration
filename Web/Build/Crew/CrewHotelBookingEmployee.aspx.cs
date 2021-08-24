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
public partial class CrewHotelBookingEmployee : PhoenixBasePage
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
                ViewState["BOOKINGID"] = null;

                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();
                }

                gvEmployee.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

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
                ViewState["PAGENUMBER"] = 1;
                gvEmployee.CurrentPageIndex = 0;
                BindData();
                gvEmployee.Rebind();
            }
            if (CommandName.ToUpper() == "ADD")
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

                if (!IsValidHotelGuestRequest(strEmployeeIdList.ToString()))
                {
                    ucError.Visible = true;
                    BindData();
                    return;
                }
                else
                {

                    if (ViewState["BOOKINGID"] != null)
                    {
                        PhoenixCrewHotelBookingGuests.InsertHotelBookingEmployee(
                           General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                         , strEmployeeIdList.ToString()
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


            ds = PhoenixCrewHotelBookingGuests.SearchHotelBookingRequestEmployees(General.GetNullableGuid(ViewState["BOOKINGID"] == null ? "" : ViewState["BOOKINGID"].ToString()),
                General.GetNullableString(txtName.Text)
                , General.GetNullableString(txtFileNo.Text)
                , General.GetNullableString(lstRank.selectedlist)
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvEmployee.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


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

    private bool IsValidHotelGuestRequest(string EmployeeIdList)
    {
        if (EmployeeIdList.Trim() == "")
        {
            ucError.ErrorMessage = "Please Select Atleast One Employee";
        }
        return (!ucError.IsError);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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

    protected void gvEmployee_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmployee.CurrentPageIndex + 1;
            BindData();
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
}

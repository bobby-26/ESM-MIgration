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
public partial class CrewTravelVisaLineItemAdd : PhoenixBasePage
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
                ViewState["TRAVELVISAID"] = null;
                if (Request.QueryString["travelvisaid"] != null)
                {
                    ViewState["TRAVELVISAID"] = Request.QueryString["travelvisaid"].ToString();
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
                gvEmployee.Rebind();
            }
            if (CommandName.ToUpper() == "ADD")
            {
                StringBuilder strEmployeeIdList = new StringBuilder();

                if (General.GetNullableInteger(ucVisaProcess.SelectedHard) == null)
                {
                    ucError.ErrorMessage = "Visa Process required";
                    ucError.Visible = true;
                    return;
                }
                else
                {
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

                        if (ViewState["TRAVELVISAID"] != null)
                        {
                            PhoenixCrewTravelVisa.InsertTravelVisaEmployee(
                               General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString())
                             , strEmployeeIdList.ToString()
                             , General.GetNullableInteger(ucVisaProcess.SelectedHard)
                            );

                            DataTable dt = PhoenixCrewTravelVisa.CrewTravelVisaRequestEmployeeList(General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString()));

                            if (dt.Rows.Count > 0)
                            {
                                string expensetype = PhoenixCommonRegisters.GetHardCode(1, 232, "VSA");
                                string paymentmode = PhoenixCommonRegisters.GetHardCode(1, 185, "CMP");
                                if (General.GetNullableInteger(expensetype) != null && General.GetNullableInteger(paymentmode) != null)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string employeeid = dr["FLDEMPLOYEEID"].ToString();
                                        string requestno = dr["FLDREFERENCENO"].ToString();
                                        string vesselid = dr["FLDVESSELID"].ToString();
                                        string dtkey = dr["FLDDTKEY"].ToString();

                                        PhoenixCrewUnallocatedVesselExpenses.InsertCrewUnallocatedVesselExpenses(General.GetNullableInteger(employeeid)
                                            , General.GetNullableInteger(expensetype)
                                            , General.GetNullableString(requestno)
                                            , General.GetNullableInteger(vesselid)
                                            , General.GetNullableInteger(paymentmode)
                                            , General.GetNullableGuid(dtkey));
                                    }
                                }
                            }
                        }
                        string Script = "";
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += " fnReloadList();";
                        Script += "</script>" + "\n";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
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
            
            ds = PhoenixCrewTravelVisa.SearchTravelVisaEmployees(
                General.GetNullableGuid(ViewState["TRAVELVISAID"] == null ? "" : ViewState["TRAVELVISAID"].ToString())
                , General.GetNullableString(txtName.Text)
                , General.GetNullableString(txtFileNo.Text)
                , General.GetNullableString(lstRank.selectedlist)
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvEmployee.PageSize,
                ref iRowCount,
                ref iTotalPageCount
                , General.GetNullableInteger(rblApplicant.SelectedValue));

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
        gvEmployee.Rebind();
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
}

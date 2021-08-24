using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Crew_CrewTravelRequestEmployeeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        CrewList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {        
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVEYN"] = "1";
            ViewState["TRAVELID"] = null;
            gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        BindData();
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
            DataTable dt = new DataTable();
            if (rblCrewFrom.SelectedValue == "0")
            {
                ddlRank.Visible = true;
                ucDesignation.Visible = false;

                dt = PhoenixCommonCrew.QueryActivity(null
                                                    , null
                                                   , null
                                                   , txtFileNo.Text
                                                   , ddlRank.SelectedRank
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , txtName.Text
                                                   , null
                                                   , null
                                                   , General.GetNullableByte(ViewState["ACTIVEYN"].ToString())
                                                   , null
                                                   , null
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                   , gvCrewList.PageSize
                                                   , ref iRowCount, ref iTotalPageCount
                                                   , null, null, null);
                if (dt.Rows.Count > 0)
                {
                    gvCrewList.DataSource = dt;
                    gvCrewList.VirtualItemCount = iRowCount;
                }
                else
                {
                    gvCrewList.DataSource = "";
                }

            }
            else if (rblCrewFrom.SelectedValue == "1")
            {
                ddlRank.Visible = true;
                ucDesignation.Visible = false;

                dt = PhoenixNewApplicantManagement.NewApplicantQueryActivity(null
                                                                   , null
                                                                   , null
                                                                   , txtFileNo.Text
                                                                   , ddlRank.SelectedRank
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , txtName.Text
                                                                   , null
                                                                   , null
                                                                   , General.GetNullableByte(ViewState["ACTIVEYN"].ToString())
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , sortexpression, sortdirection
                                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                   , gvCrewList.PageSize
                                                                   , ref iRowCount, ref iTotalPageCount
                                                                   , null);


                if (dt.Rows.Count > 0)
                {
                    gvCrewList.DataSource = dt;
                    gvCrewList.VirtualItemCount = iRowCount;
                }
                else
                {
                    gvCrewList.DataSource = "";
                }

            }

            else
            {
                ddlRank.Visible = false;
                ucDesignation.Visible = true;

                DataSet ds = PhoenixRegistersOfficeStaff.OfficeStaffSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                 General.GetNullableString(txtFileNo.Text),
                                 txtName.Text,
                                 General.GetNullableInteger(ucDesignation.SelectedDesignation),
                                 sortexpression, sortdirection,
                                 int.Parse(ViewState["PAGENUMBER"].ToString()),
                                 gvCrewList.PageSize,
                                 ref iRowCount,
                                 ref iTotalPageCount);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvCrewList.DataSource = ds;
                    gvCrewList.VirtualItemCount = iRowCount;
                }
                else
                {
                    gvCrewList.DataSource = "";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;
        string Script = "";
        NameValueCollection nvc = Filter.CurrentPickListSelection;
        string id = _gridView.DataKeys[nCurrentRow].Value.ToString();
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
        Script += "</script>" + "\n";

        LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkEployeeName");
        nvc.Set(nvc.GetKey(1), lb.Text.ToString());
        nvc.Set(nvc.GetKey(2), id);

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            else if (e.CommandName.ToString().ToUpper() == "ADDTRAVELREQUEST")
            {
                Guid? travelid = null;
                RadLabel lblEmpId = ((RadLabel)e.Item.FindControl("lblEmpId"));
                string empid = lblEmpId.Text;

                NameValueCollection nvc = Filter.CurrentTravelRequestSelection;
                if (nvc != null && !string.IsNullOrEmpty(nvc.Get("travelid")))
                {
                    ViewState["TRAVELID"] = nvc.Get("travelid").ToString();
                }

                string passengerfrom = null;

                if (rblCrewFrom.SelectedValue == "2")
                {
                    passengerfrom = "0";
                }
                else
                {
                    passengerfrom = "1";
                }

                if (ViewState["TRAVELID"] == null || ViewState["TRAVELID"].ToString() == "")
                {
                    PhoenixCrewTravelRequest.TravelNewRequesInsert
                                (General.GetNullableInteger(nvc != null ? nvc["ucpurpose"] : string.Empty),
                                General.GetNullableDateTime(DateTime.Now.ToLongDateString()),
                                int.Parse(nvc != null ? nvc["ucvessel"] : "0"),
                                General.GetNullableInteger(passengerfrom)  // passenger from
                                , ref travelid
                                , General.GetNullableString("")
                                , General.GetNullableInteger(nvc != null ? nvc["ddlAccountDetails"] : string.Empty));

                    ViewState["TRAVELID"] = travelid;

                    nvc.Add("travelid", ViewState["TRAVELID"].ToString());

                    Filter.CurrentTravelRequestSelection = nvc;

                }
                if (ViewState["TRAVELID"] != null)
                {
                    if (rblCrewFrom.SelectedValue == "2")
                    {
                        PhoenixCrewTravelRequest.InsertPassengersOfficeStaff
                            (General.GetNullableInteger(empid)
                               , General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                               , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                               , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtorigindate") : string.Empty)
                               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlampmOriginDate") : string.Empty)
                               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDestinationdate") : string.Empty)
                               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlampmarrival") : string.Empty)
                              , General.GetNullableInteger(nvc != null ? nvc.Get("Payment") : string.Empty)
                              , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                              , General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                              , General.GetNullableInteger(nvc != null ? nvc.Get("ddlAccountDetails") : string.Empty)
                              );
                    }
                    else
                    {
                        PhoenixCrewTravelRequest.InsertTravelRequest(General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtorigindate") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlampmOriginDate") : string.Empty)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDestinationdate") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlampmarrival") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("Payment") : string.Empty)
                                                                    , General.GetNullableInteger(empid)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlAccountDetails") : string.Empty)
                                                                    );

                    }
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);

                BindData();
                gvCrewList.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "SELECT")
            {

            }
            else if (e.CommandName == "Page")
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

    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblAppliedRank");
            RadLabel lblFileNo = (RadLabel)e.Item.FindControl("lblFileNo");

            if (lblRank != null)
            {
                if (rblCrewFrom.SelectedValue == "0")
                {
                    lblRank.Text = drv["FLDRANKPOSTEDNAME"].ToString();
                }
                if (rblCrewFrom.SelectedValue == "1")
                {
                    lblRank.Text = drv["FLDRANKNAME"].ToString();
                }
                if (rblCrewFrom.SelectedValue == "2")
                {
                    lblRank.Text = drv["FLDDESIGNATIONNAME"].ToString();
                }
            }
            if (lblFileNo != null)
            {
                if (rblCrewFrom.SelectedValue == "0")
                {
                    lblFileNo.Text = drv["FLDEMPLOYEECODE"].ToString();
                }
                if (rblCrewFrom.SelectedValue == "1")
                {
                    lblFileNo.Text = drv["FLDFILENO"].ToString();
                }
                if (rblCrewFrom.SelectedValue == "2")
                {
                    lblFileNo.Text = drv["FLDEMPLOYEENUMBER"].ToString();
                }
            }
        }
    }
           
}
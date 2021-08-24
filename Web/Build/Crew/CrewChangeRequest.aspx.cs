using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CrewChangeRequest : PhoenixBasePage
{
    string strVesselId = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "FILTER", ToolBarDirection.Right);

            if (Request.QueryString["from"] != null)
            {
                if (Request.QueryString["from"].ToString() == "crewcost") //Request came from crew cost Evaluation
                {
                    toolbar.AddButton("Port Cost Detail", "DETAIL");
                    string id = Request.QueryString["REQUESTID"].ToString();
                    ViewState["COSTREQUESTID"] = id;
                }
            }

            CCPMenu.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Next", "NEXT", ToolBarDirection.Right);
            CrewChangeRequestMenu.AccessRights = this.ViewState;
            CrewChangeRequestMenu.MenuList = toolbar.Show();

            strVesselId = Filter.CurrentCrewChangePlanFilterSelection["ucVessel"];
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                BindVesselAccount();
                if (Request.QueryString["hidemenu"] != null && Request.QueryString["hidemenu"].ToString() != "")
                {
                    CrewChangeRequestMenu.Title = "";
                }

                if (Request.QueryString["from"] != null)
                {
                    if (Request.QueryString["from"].ToString() == "crewcost")
                    {
                        SetCostEvalInfo();
                    }
                }
                else
                {
                    SetInformation();
                }
                gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //  BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetCostEvalInfo()
    {
        try
        {
            DataTable dt = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationRequest(new Guid(ViewState["COSTREQUESTID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                ucport.SelectedSeaport = dr["FLDSEAPORTID"].ToString();
                txtDateOfCrewChange.Text = dr["FLDCREWCHANGEDATE"].ToString();
                txtcityid.Text = dr["FLDCITYID"].ToString();
                txtcityname.Text = dr["FLDCITYNAME"].ToString();
                ucCrewChangeReason.SelectedReason = dr["FLDTRAVELREASONID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CCPMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FILTER"))
            {
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
                else
                    Response.Redirect("CrewChangePlanFilter.aspx", false);
            }
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("CrewCostEvaluationRequestGeneral.aspx?REQUESTID=" + ViewState["COSTREQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewChangeRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("NEXT"))
            {

                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strOnSignerYN = new StringBuilder();
                StringBuilder strCrewplanid = new StringBuilder();

                foreach (GridDataItem gvr in gvCCPlan.Items)
                {
                    CheckBox chkOnSigner = (CheckBox)gvr.FindControl("chkOnSigner");
                    CheckBox chkOffSigner = (CheckBox)gvr.FindControl("chkOffSigner");

                    if (chkOnSigner.Checked)
                    {
                        RadLabel lblOnSigner = (RadLabel)gvr.FindControl("lblEmployeeId");
                        RadLabel lblCrewPlanIdOnSigner = (RadLabel)gvr.FindControl("lblCrewPlanId");

                        stremployeelist.Append(lblOnSigner.Text);
                        stremployeelist.Append(",");

                        strCrewplanid.Append(lblCrewPlanIdOnSigner.Text);
                        strCrewplanid.Append(",");

                        strOnSignerYN.Append("1");
                        strOnSignerYN.Append(",");
                    }

                    if (chkOffSigner.Checked)
                    {
                        RadLabel lblOffSigner = (RadLabel)gvr.FindControl("lblOffSignerId");
                        RadLabel lblCrewPlanIdOffSigner = (RadLabel)gvr.FindControl("lblCrewPlanId");

                        stremployeelist.Append(lblOffSigner.Text);
                        stremployeelist.Append(",");

                        strOnSignerYN.Append("0");
                        strOnSignerYN.Append(",");
                    }
                }
                if (stremployeelist.Length > 1)
                {
                    stremployeelist.Remove(stremployeelist.Length - 1, 1);
                }

                if (strOnSignerYN.Length > 1)
                {
                    strOnSignerYN.Remove(strOnSignerYN.Length - 1, 1);
                }

                if (strCrewplanid.Length > 1)
                {
                    strCrewplanid.Remove(strCrewplanid.Length - 1, 1);
                }

                if (!IsValidTrvelRequest(ucport.SelectedSeaport, txtDateOfCrewChange.Text, stremployeelist.ToString(), ucCrewChangeReason.SelectedReason))
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                {
                    int? proceedyn = null;
                    string errormsg = null;

                    PhoenixCrewTravelRequest.ValidateOffshoreTravel(strCrewplanid.ToString(), ref proceedyn, ref errormsg);

                    if (proceedyn == 0)
                    {
                        ucError.ErrorMessage = errormsg;
                        ucError.Visible = true;
                        return;
                    }
                }

                foreach (GridDataItem gvRow in gvCCPlan.Items)
                {
                    StringBuilder stremployeelistInsert = new StringBuilder();
                    StringBuilder strOnSignerYNInsert = new StringBuilder();


                    if (stremployeelist.ToString().Trim() != "")
                    {
                        CheckBox chkOnSignerInsert = (CheckBox)gvRow.FindControl("chkOnSigner");
                        CheckBox chkOffSignerInsert = (CheckBox)gvRow.FindControl("chkOffSigner");

                        RadLabel lblCrewPlanIdOnSignerInsert = (RadLabel)gvRow.FindControl("lblCrewPlanId");

                        if (chkOnSignerInsert.Checked)
                        {
                            RadLabel lblOnSignerInsert = (RadLabel)gvRow.FindControl("lblEmployeeId");

                            stremployeelistInsert.Append(lblOnSignerInsert.Text);
                            stremployeelistInsert.Append(",");

                            strOnSignerYNInsert.Append("1");
                            strOnSignerYNInsert.Append(",");
                        }

                        if (chkOffSignerInsert.Checked)
                        {
                            RadLabel lblOffSignerInsert = (RadLabel)gvRow.FindControl("lblOffSignerId");
                            RadLabel lblCrewPlanIdOffSignerInsert = (RadLabel)gvRow.FindControl("lblCrewPlanId");

                            stremployeelistInsert.Append(lblOffSignerInsert.Text);
                            stremployeelistInsert.Append(",");

                            strOnSignerYNInsert.Append("0");
                            strOnSignerYNInsert.Append(",");
                        }

                        if (stremployeelistInsert.Length > 1)
                        {
                            stremployeelistInsert.Remove(stremployeelistInsert.Length - 1, 1);
                        }
                        if (strOnSignerYNInsert.Length > 1)
                        {
                            strOnSignerYNInsert.Remove(strOnSignerYNInsert.Length - 1, 1);
                        }

                        if (stremployeelistInsert.Length > 1 || strOnSignerYNInsert.Length > 1)
                        {
                            PhoenixCrewTravelRequest.InsertTravelRequest(
                                stremployeelistInsert.ToString()
                                , General.GetNullableGuid(lblCrewPlanIdOnSignerInsert.Text.ToString())
                                , strOnSignerYNInsert.ToString()
                                , int.Parse(Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString())
                                , int.Parse(ucport.SelectedSeaport)
                                , DateTime.Parse(txtDateOfCrewChange.Text)
                                , int.Parse(ucCrewChangeReason.SelectedReason)
                                , General.GetNullableInteger(txtcityid.Text)
                                , General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                        }
                    }
                }

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                {
                    Response.Redirect("../Crew/CrewChangeTravel.aspx?employeelist=" + stremployeelist.ToString()
                    + "&port=" + ucport.SelectedSeaport
                    + "&date=" + txtDateOfCrewChange.Text
                    + "&vessel=" + Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString()
                    + "&launchedfrom=Offshore"
                    + "&from=crewchange", false);
                }
                else
                {
                    Response.Redirect("../Crew/CrewChangeTravel.aspx?employeelist=" + stremployeelist.ToString()
                        + "&port=" + ucport.SelectedSeaport
                        + "&date=" + txtDateOfCrewChange.Text
                        + "&vessel=" + Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString()
                        + "&from=crewchange", false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTrvelRequest(string strPortId, string dateofcrewchange, string strEmployeelist, string crewchangereason)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(dateofcrewchange) == null)
            ucError.ErrorMessage = "Date of Crew Change is required.";

        else if (!DateTime.TryParse(dateofcrewchange.ToString(), out resultDate))
        {
            ucError.ErrorMessage = "Date of Crew Change is not Valid.";
        }

        if (General.GetNullableInteger(strPortId) == null)
            ucError.ErrorMessage = "Crew Change Port is required";

        if (General.GetNullableInteger(crewchangereason) == null)
            ucError.ErrorMessage = "Crew Change Reason is required";

        if (General.GetNullableInteger(txtcityid.Text) == null)
            ucError.ErrorMessage = "City is required";

        if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel Account is required";

        if (strEmployeelist.Trim() == "")
        {
            if (General.GetNullableDateTime(dateofcrewchange) != null && General.GetNullableInteger(strPortId) != null)
            {
                DataSet ds = PhoenixCrewTravelRequest.SearchTravelRequest(
                    int.Parse(Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString())
                    , int.Parse(strPortId)
                    , General.GetNullableDateTime(dateofcrewchange),
                    General.GetNullableGuid(""),
                    null, 0,
                    1,                      //  Page number
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    ucError.ErrorMessage = "Please Select Atleast One Employee";
                }
            }
        }

        return (!ucError.IsError);
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

            NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;

            DataSet ds = PhoenixCrewChangePlan.SearchCrewChangePlan(
                int.Parse(strVesselId), byte.Parse(Request.QueryString["access"] != null ? "1" : "0")
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvCCPlan.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvCCPlan.DataSource = ds;
            gvCCPlan.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCPlan_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCCPlan.Rebind();

    }

    private void SetInformation()
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(strVesselId));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            UnRequestPlan();
        }
    }

    private void UnRequestPlan()
    {
        try
        {
            DataSet ds = PhoenixCrewChangePlan.GetUnProccesCrewChangePlan(int.Parse(strVesselId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDateOfCrewChange.Text = ds.Tables[0].Rows[0]["FLDDATEOFCREWCHANGE"].ToString();
                txtcityid.Text = ds.Tables[0].Rows[0]["FLDCITYOFCREWCHANGE"].ToString();
                txtcityname.Text = ds.Tables[0].Rows[0]["FLDCITYNAME"].ToString();
                ucCrewChangeReason.SelectedReason = ds.Tables[0].Rows[0]["FLDCREWCHANGEREASON"].ToString();
                ucport.SelectedSeaport = ds.Tables[0].Rows[0]["FLDPORTOFCREWCHANGE"].ToString();
                ddlAccountDetails.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public void BindVesselAccount()
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(strVesselId) == 0 ? null : General.GetNullableInteger(strVesselId), 1);
        ddlAccountDetails.DataBind();
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void gvCCPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SELECT")
            {

                LinkButton lnkEmp = (LinkButton)e.Item.FindControl("lnkEmployee");
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");

                string script = "openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewPersonalAddress.aspx?empid=" + empid.Text + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
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

    protected void gvCCPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
                    RadLabel rankid = (RadLabel)e.Item.FindControl("lblRankId");
                    RadLabel vesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                    RadLabel lb = (RadLabel)e.Item.FindControl("lblEmployee");
                    RadLabel lblOffSigner = (RadLabel)e.Item.FindControl("lblOffSignerId");
                    CheckBox chkOffSigner = (CheckBox)e.Item.FindControl("chkOffSigner");
                    RadLabel lblOnSigner = (RadLabel)e.Item.FindControl("lblEmployeeId");
                    CheckBox chkOnSigner = (CheckBox)e.Item.FindControl("chkOnSigner");
                    RadLabel lblOnSignerCrewChange = (RadLabel)e.Item.FindControl("lblOnSignerCrewChange");
                    RadLabel lblOffSignerCrewChange = (RadLabel)e.Item.FindControl("lblOffSignerCrewChange");
                    RadLabel lblOnSignerCrewChangenotreq = (RadLabel)e.Item.FindControl("lblOnSignerCrewChangeNotReq");
                    RadLabel lblOffSignerCrewChangenotreq = (RadLabel)e.Item.FindControl("lblOffSignerCrewChangeNotReq");

                    if (lblOffSigner.Text.Trim() == "")
                    {
                        chkOffSigner.Enabled = false;
                    }
                    if (lblOnSigner.Text.Trim() == "")
                    {
                        chkOnSigner.Enabled = false;
                    }

                    if (lblOffSignerCrewChange.Text.Trim() == "1")
                    {
                        chkOffSigner.Enabled = false;
                    }
                    if (lblOnSignerCrewChange.Text.Trim() == "1")
                    {
                        chkOnSigner.Enabled = false;
                    }
                    if (lblOnSignerCrewChangenotreq.Text.Trim() == "1")
                    {
                        chkOnSigner.Enabled = true;
                        chkOnSigner.Checked = true;
                    }
                    if (lblOffSignerCrewChangenotreq.Text.Trim() == "1")
                    {
                        chkOffSigner.Enabled = true;
                        chkOffSigner.Checked = true;
                    }
                    LinkButton lnkoffsigner = (LinkButton)e.Item.FindControl("lnkOffSigner");
                    if (drv["FLDOFFSIGNERID"].ToString() != string.Empty && lnkoffsigner != null)
                    {
                        lnkoffsigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
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

    protected void gvCCPlan_SortCommand(object sender, GridSortCommandEventArgs e)
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

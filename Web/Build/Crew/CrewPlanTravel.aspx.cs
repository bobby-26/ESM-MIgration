using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class Crew_CrewPlanTravel : PhoenixBasePage
{
    string strVesselId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK",ToolBarDirection.Right);
            CrewChangeRequestMenu.AccessRights = this.ViewState;
            CrewChangeRequestMenu.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanTravel.aspx", "Off-Signer Request", "<i class=\"fas fa-plane-arrival\"></i>", "OFFSIGNER");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanTravel.aspx", "On-Signer Request", "<i class=\"fas fa-plane-departure-On-Signer-Request\"></i>", "ONSIGNER");
            MenuCrewChangePlan.AccessRights = this.ViewState;
            MenuCrewChangePlan.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["Vesselid"] = null;
                if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"].ToString() != "")
                {

                    ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
                    SetInformation();
                    BindVesselAccount();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewPlanList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ChangePlan_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ONSIGNER"))
            {
                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strCrewplanid = new StringBuilder();

                string onsigneryn = "1";

                foreach (GridDataItem gvr in gvCCPlan.Items)
                {
                    RadCheckBox chkOnSigner = (RadCheckBox)gvr.FindControl("chkOnSigner");
                    if (chkOnSigner.Checked==true)
                    {
                        RadLabel lblOnSigner = (RadLabel)gvr.FindControl("lblEmployeeId");
                        RadLabel lblCrewPlanIdOnSigner = (RadLabel)gvr.FindControl("lblCrewPlanId");

                        stremployeelist.Append(lblOnSigner.Text);
                        stremployeelist.Append(",");

                        strCrewplanid.Append(lblCrewPlanIdOnSigner.Text);
                        strCrewplanid.Append(",");
                    }
                }

                if (stremployeelist.Length > 1)
                {
                    stremployeelist.Remove(stremployeelist.Length - 1, 1);
                }
                if (strCrewplanid.Length > 1)
                {
                    strCrewplanid.Remove(strCrewplanid.Length - 1, 1);
                }

                if (stremployeelist.Length > 1)
                {
                    PhoenixCrewTravelRequest.InsertSignOnOffTravelRequest(
                        stremployeelist.ToString()
                        , strCrewplanid.ToString()
                        , General.GetNullableInteger(onsigneryn)
                        , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                        , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                        , General.GetNullableDateTime(txtorigindate.Text)
                        , General.GetNullableInteger(ddlampmOriginDate.SelectedValue)
                        , General.GetNullableDateTime(txtDestinationdate.Text)
                        , General.GetNullableInteger(ddlampmarrival.SelectedValue)
                        , General.GetNullableInteger(txtOrigin.SelectedValue)
                        , General.GetNullableInteger(txtDestination.SelectedValue)
                        , General.GetNullableInteger(Payment.SelectedHard)
                        , General.GetNullableInteger(ucpurpose.SelectedReason));

                    BindData();
                    gvCCPlan.Rebind();
                    ucStatus.Text = "Travel Requested Successfully.";
                }
            }
            if (CommandName.ToUpper().Equals("OFFSIGNER"))
            {
                StringBuilder stroffsignerlist = new StringBuilder();
                StringBuilder strCrewplanid = new StringBuilder();

                string onsigneryn = "0";

                if (!IsValidTravelRequest())
                {
                    ucError.Visible = true;
                    return;
                }

                foreach (GridDataItem gvr in gvCCPlan.Items)
                {
                    RadCheckBox chkoffSigner = (RadCheckBox)gvr.FindControl("chkOffSigner");

                    if (chkoffSigner.Checked==true)
                    {
                        RadLabel lblOffSignerId = (RadLabel)gvr.FindControl("lblOffSignerId");
                        RadLabel lblCrewPlanIdOnSigner = (RadLabel)gvr.FindControl("lblCrewPlanId");

                        stroffsignerlist.Append(lblOffSignerId.Text);
                        stroffsignerlist.Append(",");

                        strCrewplanid.Append(lblCrewPlanIdOnSigner.Text);
                        strCrewplanid.Append(",");
                    }

                }
                if (stroffsignerlist.Length > 1)
                {
                    stroffsignerlist.Remove(stroffsignerlist.Length - 1, 1);
                }
                if (strCrewplanid.Length > 1)
                {
                    strCrewplanid.Remove(strCrewplanid.Length - 1, 1);
                }

                if (stroffsignerlist.Length > 1)
                {
                    PhoenixCrewTravelRequest.InsertSignOnOffTravelRequest(
                        stroffsignerlist.ToString()
                        , strCrewplanid.ToString()
                        , General.GetNullableInteger(onsigneryn)
                        , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                        , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                        , General.GetNullableDateTime(txtorigindate.Text)
                        , General.GetNullableInteger(ddlampmOriginDate.SelectedValue)
                        , General.GetNullableDateTime(txtDestinationdate.Text)
                        , General.GetNullableInteger(ddlampmarrival.SelectedValue)
                        , General.GetNullableInteger(txtOrigin.SelectedValue)
                        , General.GetNullableInteger(txtDestination.SelectedValue)
                        , General.GetNullableInteger(Payment.SelectedHard)
                        , General.GetNullableInteger(ucpurpose.SelectedReason));

                    BindData();
                    gvCCPlan.Rebind();
                    ucStatus.Text = "Travel Requested Successfully.";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultDepDate;
        //DateTime resultArrivalDate;

        if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel Account is required.";

        if (txtOrigin.SelectedValue.ToUpper() == "DUMMY" || General.GetNullableInteger(txtOrigin.SelectedValue) == null)
            ucError.ErrorMessage = "Departure is required.";

        if (General.GetNullableDateTime(txtorigindate.Text) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableInteger(ddlampmOriginDate.SelectedValue) == null)
            ucError.ErrorMessage = "Departure Time is required.";

        if (General.GetNullableDateTime(txtDestinationdate.Text) != null)
        {
            if (DateTime.TryParse(txtorigindate.Text, out resultDepDate) && DateTime.Compare(resultDepDate, DateTime.Parse(txtDestinationdate.Text)) > 0)
            {
                ucError.ErrorMessage = "Arrival Date should be greater than Departure date";
            }
        }

        if (txtDestination.SelectedValue.ToUpper() == "DUMMY" || General.GetNullableInteger(txtDestination.SelectedValue) == null)
            ucError.ErrorMessage = "Destination is required.";

        if (ucpurpose.SelectedReason.ToUpper() == "DUMMY" || General.GetNullableInteger(ucpurpose.SelectedReason) == null)
            ucError.ErrorMessage = "Purpose is required.";

        if (Payment.SelectedHard.ToUpper() == "DUMMY" || General.GetNullableInteger(Payment.SelectedHard) == null)
            ucError.ErrorMessage = "Payment Mode is required.";

        return (!ucError.IsError);
    }

    private void SetInformation()
    {
        try
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Request.QueryString["Vesselid"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
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
            string Vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.SearchCrewPlanTravel(General.GetNullableInteger(Vesselid)
                                                                         , sortexpression, sortdirection
                                                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                         , gvCCPlan.PageSize
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCPlan.DataSource = ds;
                gvCCPlan.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCCPlan.DataSource = "";
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
        gvCCPlan.Rebind();
    }
    
    public void BindVesselAccount()
    {

        string strVesselId = null;

        if (ViewState["Vesselid"] != null && ViewState["Vesselid"] != null)
            strVesselId = ViewState["Vesselid"].ToString();

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
            }
            else if (e.CommandName == "SETDESTINATION")
            {
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
                RadLabel lblOffSignerId = (RadLabel)e.Item.FindControl("lblOffSignerId");
                RadCheckBox chkOffSigner = (RadCheckBox)e.Item.FindControl("chkOffSigner");
                RadCheckBox chkOnSigner = (RadCheckBox)e.Item.FindControl("chkOnSigner");
                if (General.GetNullableInteger(lblOffSignerId.Text) != null)
                {
                    DataTable dt = PhoenixCrewTravelRequest.CrewTravelPlanMoreInfoList(General.GetNullableInteger(lblOffSignerId.Text), null);

                    if (dt.Rows.Count > 0)
                    {
                        if (chkOffSigner.Enabled == true)
                        {
                            chkOffSigner.Checked = true;
                            txtDestination.Text = dt.Rows[0]["FLDAIRPORTCITY"].ToString();
                            txtDestination.SelectedValue = dt.Rows[0]["FLDCITYID"].ToString();
                        }
                    }
                }
            }
            else if (e.CommandName == "SETORIGIN")
            {
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
                RadLabel lblOffSignerId = (RadLabel)e.Item.FindControl("lblOffSignerId");
                RadCheckBox chkOffSigner = (RadCheckBox)e.Item.FindControl("chkOffSigner");
                RadCheckBox chkOnSigner = (RadCheckBox)e.Item.FindControl("chkOnSigner");
                if (General.GetNullableInteger(empid.Text) != null)
                {
                    DataTable dt = PhoenixCrewTravelRequest.CrewTravelPlanMoreInfoList(General.GetNullableInteger(empid.Text), null);

                    if (dt.Rows.Count > 0)
                    {
                        if (chkOnSigner.Enabled == true)
                        {
                            chkOnSigner.Checked = true;
                            txtOrigin.Text = dt.Rows[0]["FLDAIRPORTCITY"].ToString();
                            txtOrigin.SelectedValue = dt.Rows[0]["FLDCITYID"].ToString();
                        }
                    }
                }
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
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel rankid = (RadLabel)e.Item.FindControl("lblRankId");
                RadLabel vesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblOffSigner = (RadLabel)e.Item.FindControl("lblOffSignerId");
                RadCheckBox chkOffSigner = (RadCheckBox)e.Item.FindControl("chkOffSigner");
                RadLabel lblOnSigner = (RadLabel)e.Item.FindControl("lblEmployeeId");
                RadLabel lbloffsignercon = (RadLabel)e.Item.FindControl("lbloffsignercon");
                RadLabel lblonsignercon = (RadLabel)e.Item.FindControl("lblonsignercon");
                RadCheckBox chkOnSigner = (RadCheckBox)e.Item.FindControl("chkOnSigner");
                RadLabel lblOnSignerCrewChangenotreq = (RadLabel)e.Item.FindControl("lblOnSignerCrewChangeNotReq");
                RadLabel lblOffSignerCrewChangenotreq = (RadLabel)e.Item.FindControl("lblOffSignerCrewChangeNotReq");
                LinkButton cmdSetDestination = (LinkButton)e.Item.FindControl("cmdSetDestination");
                LinkButton cmdSetOrigin = (LinkButton)e.Item.FindControl("cmdSetOrigin");
                UserControlCommonToolTip ucCommonToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
                UserControlCommonToolTip ucCommonToolTiponsigner = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTiponsigner");

                if (lblOffSigner.Text.Trim() == "")
                {
                    chkOffSigner.Visible = false;
                    lbloffsignercon.Visible = false;
                    ucCommonToolTip.Visible = false;
                    cmdSetDestination.Visible = false;
                }
                if (lblOnSigner.Text.Trim() == "")
                {
                    chkOnSigner.Visible = false;
                    lblonsignercon.Visible = false;
                    ucCommonToolTiponsigner.Visible = false;
                    cmdSetOrigin.Visible = false;
                }

                if (lblOnSignerCrewChangenotreq.Text.Trim() == "1")
                {
                    chkOnSigner.Enabled = false;
                }
                if (lblOffSignerCrewChangenotreq.Text.Trim() == "1")
                {
                    chkOffSigner.Enabled = false;
                }

                LinkButton lnkoffsigner = (LinkButton)e.Item.FindControl("lnkOffSigner");
                if (drv["FLDOFFSIGNERID"].ToString() != string.Empty && lnkoffsigner != null)
                {
                    lnkoffsigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
                }

                LinkButton lnkOnSigner = (LinkButton)e.Item.FindControl("lnkOnSigner");

                if (drv["FLDEMPLOYEEID"].ToString() != string.Empty && lnkoffsigner != null)
                {
                    if (drv["FLDNEWAPP"].ToString() == "1")
                    {                        
                        lnkOnSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
                    }
                    else
                    {
                        lnkOnSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
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
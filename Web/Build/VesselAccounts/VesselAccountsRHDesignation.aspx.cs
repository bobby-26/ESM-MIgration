using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;
public partial class VesselAccountsRHDesignation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
        MenuRHGeneral.AccessRights = this.ViewState;
        MenuRHGeneral.MenuList = toolbarmain.Show();
        MenuRHGeneral.SelectedMenuIndex = 0;
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (Request.QueryString["EmployeeId"] != null)
            {
                ViewState["EmployeeId"] = Request.QueryString["EmployeeId"];
            }
            if (Request.QueryString["RankId"] != null)
            {
                ViewState["RankId"] = Request.QueryString["RankId"];
            }
            if (Request.QueryString["EmployeeName"] != null)
            {
                ViewState["EmployeeName"] = Request.QueryString["EmployeeName"];
                lblEmployee.Text = ViewState["EmployeeName"].ToString();
            }
            if (Request.QueryString["RankName"] != null)
            {
                ViewState["RankName"] = Request.QueryString["RankName"];
                lblRank.Text = ViewState["RankName"].ToString();
            }
            if (Request.QueryString["RestHourEmployeeId"] != null)
            {
                ViewState["RestHourEmployeeId"] = Request.QueryString["RestHourEmployeeId"];
            }
            if (Request.QueryString["CalenderId"] != null)
            {
                ViewState["CALENDERID"] = Request.QueryString["CalenderId"].ToString();
            }
            BindDetails();
        }
    }
    protected void BindDataSea()
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsRH.DefaultWorkHoursSearch(int.Parse(ViewState["EmployeeId"].ToString())
                     , General.GetNullableGuid(ViewState["RestHourEmployeeId"].ToString())
                     , 1
                     , int.Parse(ViewState["RankId"].ToString()));

            gvWorkHours.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindDataPort()
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsRH.DefaultWorkHoursSearch(int.Parse(ViewState["EmployeeId"].ToString())
                     , General.GetNullableGuid(ViewState["RestHourEmployeeId"].ToString())
                     , 0
                     , int.Parse(ViewState["RankId"].ToString()));

            gvWorkHoursPort.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string[] BindDuration()
    {
        return new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                if (Request.QueryString["MODELYN"] != null && Request.QueryString["MODELYN"].ToString() == "Y")
                {
                    string script = "parent.CloseModelWindow();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
                else
                {
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DesignationWSheetAtSea_OnTimeStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ViewState["RankId"].ToString()) != null && General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != 0)
            {
                int workatsea = 0;
                PhoenixVesselAccountsRH.RestHourDesignationUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                           int.Parse(ViewState["RankId"].ToString()),
                                                            new Guid(DesignationWSheetAtSea.Id),
                                                            1,
                                                            DesignationWSheetAtSea.WorkHours,
                                                            workatsea,
                                                            General.GetNullableInteger(ViewState["EmployeeId"].ToString()),
                                                            General.GetNullableGuid(ViewState["RestHourEmployeeId"].ToString())
                                                           );
                BindDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DesignationWSheetAtPort_OnTimeStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ViewState["RankId"].ToString()) != null && General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != 0)
            {
                int workatport = 1;
                PhoenixVesselAccountsRH.RestHourDesignationUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                           int.Parse(ViewState["RankId"].ToString()),
                                                            new Guid(DesignationWSheetAtPort.Id),
                                                            1,
                                                            DesignationWSheetAtPort.WorkHours,
                                                            workatport,
                                                            General.GetNullableInteger(ViewState["EmployeeId"].ToString()),
                                                            General.GetNullableGuid(ViewState["RestHourEmployeeId"].ToString())
                                                            );
                BindDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDetails()
    {
        try
        {
            DataSet ds = new DataSet();
            if (General.GetNullableInteger(ViewState["RankId"].ToString()) != null && General.GetNullableInteger(ViewState["EmployeeId"].ToString()) != null && General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) != 0)
            {
                ds = PhoenixVesselAccountsRH.RestHourDesignationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                      int.Parse(ViewState["RankId"].ToString()),
                                                      1,
                                                      General.GetNullableInteger(ViewState["EmployeeId"].ToString()),
                                                      new Guid(ViewState["RestHourEmployeeId"].ToString())
                                                      );
                DesignationWSheetAtSea.FieldToBind = "FLDREPORTINGHOUR";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DesignationWSheetAtSea.FieldToBind = "FLDHOURS";
                    DesignationWSheetAtSea.FieldValue = "FLDRESTHOURDESIGNATIONID";
                    DesignationWSheetAtSea.SetTimeList((DataTable)ds.Tables[0]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DesignationWSheetAtPort.FieldToBind = "FLDHOURSATPORT";
                    DesignationWSheetAtPort.FieldValue = "FLDRESTHOURDESIGNATIONID";
                    DesignationWSheetAtPort.SetTimeList((DataTable)ds.Tables[1]);
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                ucError.ErrorMessage = "Choose Vessel";
                ucError.Visible = true;
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
        BindDetails();
    }
    protected void ddlRank_TextChanged(object sender, EventArgs e)
    {
        DesignationWSheetAtSea.clear();
        DesignationWSheetAtPort.clear();
    }
    protected void ddlEmployeeList_TextChanged(object sender, EventArgs e)
    {
        if (!IsValidRank())
        {
            ucError.Visible = true;
            return;
        }
        BindDetails();
    }
    private bool IsValidRank()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ViewState["RankId"].ToString()) == null)
            ucError.ErrorMessage = "Rank is required.";

        return (!ucError.IsError);
    }
    private bool IsValidWorkHours(string wkfrom1Add, string wkto1Add, string wkfrom2Add, string wkto2Add
        , string nwkfrom1Add, string nwkto1Add, string nwkfrom2Add, string nwkto2Add)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if To is lesser than From

        if (General.GetNullableInteger(wkfrom1Add) > General.GetNullableInteger(wkto1Add))
            ucError.ErrorMessage = "Enter valid WatchKeeping At sea for Period 1.";

        if (General.GetNullableInteger(wkfrom2Add) > General.GetNullableInteger(wkto2Add))
            ucError.ErrorMessage = "Enter valid WatchKeeping At sea for Period 2.";

        if (General.GetNullableInteger(nwkfrom1Add) > General.GetNullableInteger(nwkto1Add))
            ucError.ErrorMessage = "Enter valid Non WatchKeeping At sea for Period 1.";

        if (General.GetNullableInteger(nwkfrom2Add) > General.GetNullableInteger(nwkto2Add))
            ucError.ErrorMessage = "Enter valid Non WatchKeeping At sea for Period 2.";

        return (!ucError.IsError);
    }
    private bool IsValidWorkHoursInPort(string wkfrom1Add, string wkto1Add, string wkfrom2Add, string wkto2Add
        , string nwkfrom1Add, string nwkto1Add, string nwkfrom2Add, string nwkto2Add)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        // From is greater than to
        if (General.GetNullableInteger(wkfrom1Add) > General.GetNullableInteger(wkto1Add))
            ucError.ErrorMessage = "Enter valid WatchKeeping in Port for Period 1.";

        if (General.GetNullableInteger(wkfrom2Add) > General.GetNullableInteger(wkto2Add))
            ucError.ErrorMessage = "Enter valid WatchKeeping in Port for Period 2.";

        if (General.GetNullableInteger(nwkfrom1Add) > General.GetNullableInteger(nwkto1Add))
            ucError.ErrorMessage = "Enter valid Non WatchKeeping in Port for Period 1.";

        if (General.GetNullableInteger(nwkfrom2Add) > General.GetNullableInteger(nwkto2Add))
            ucError.ErrorMessage = "Enter valid Non WatchKeeping in Port for Period 2.";

        return (!ucError.IsError);
    }

    protected void gvWorkHours_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataSea();
        BindDetails();
    }
    protected void Rebind()
    {
        gvWorkHours.SelectedIndexes.Clear();
        gvWorkHours.EditIndexes.Clear();
        gvWorkHours.DataSource = null;
        gvWorkHours.Rebind();
    }

    protected void gvWorkHours_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadComboBox ddlWKPeriod1From = (RadComboBox)e.Item.FindControl("ddlWKPeriod1From");
                if (ddlWKPeriod1From != null)
                {
                    ddlWKPeriod1From.DataSource = BindDuration();
                    ddlWKPeriod1From.DataBind();
                    ddlWKPeriod1From.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKPeriod1From.SelectedValue = drv["FLDWATCHKEEPINGFROM1"].ToString();
                }
                RadComboBox ddlWKPeriod1To = (RadComboBox)e.Item.FindControl("ddlWKPeriod1To");
                if (ddlWKPeriod1To != null)
                {
                    ddlWKPeriod1To.DataSource = BindDuration();
                    ddlWKPeriod1To.DataBind();
                    ddlWKPeriod1To.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKPeriod1To.SelectedValue = drv["FLDWATCHKEEPINGTO1"].ToString();
                }
                RadComboBox ddlWKPeriod2From = (RadComboBox)e.Item.FindControl("ddlWKPeriod2From");
                if (ddlWKPeriod2From != null)
                {
                    ddlWKPeriod2From.DataSource = BindDuration();
                    ddlWKPeriod2From.DataBind();
                    ddlWKPeriod2From.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKPeriod2From.SelectedValue = drv["FLDWATCHKEEPINGFROM2"].ToString();
                }
                RadComboBox ddlWKPeriod2To = (RadComboBox)e.Item.FindControl("ddlWKPeriod2To");
                if (ddlWKPeriod2To != null)
                {
                    ddlWKPeriod2To.DataSource = BindDuration();
                    ddlWKPeriod2To.DataBind();
                    ddlWKPeriod2To.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKPeriod2To.SelectedValue = drv["FLDWATCHKEEPINGTO2"].ToString();
                }
                RadComboBox ddlNWKPeriod1From = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1From");
                if (ddlNWKPeriod1From != null)
                {
                    ddlNWKPeriod1From.DataSource = BindDuration();
                    ddlNWKPeriod1From.DataBind();
                    ddlNWKPeriod1From.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKPeriod1From.SelectedValue = drv["FLDNONWATCHKEEPINGFROM3"].ToString();
                }
                RadComboBox ddlNWKPeriod1To = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1To");
                if (ddlNWKPeriod1To != null)
                {
                    ddlNWKPeriod1To.DataSource = BindDuration();
                    ddlNWKPeriod1To.DataBind();
                    ddlNWKPeriod1To.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKPeriod1To.SelectedValue = drv["FLDNONWATCHKEEPINGTO3"].ToString();
                }
                RadComboBox ddlNWKPeriod2From = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2From");
                if (ddlNWKPeriod2From != null)
                {
                    ddlNWKPeriod2From.DataSource = BindDuration();
                    ddlNWKPeriod2From.DataBind();
                    ddlNWKPeriod2From.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKPeriod2From.SelectedValue = drv["FLDNONWATCHKEEPINGFROM4"].ToString();
                }
                RadComboBox ddlNWKPeriod2To = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2To");
                if (ddlNWKPeriod2To != null)
                {
                    ddlNWKPeriod2To.DataSource = BindDuration();
                    ddlNWKPeriod2To.DataBind();
                    ddlNWKPeriod2To.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKPeriod2To.SelectedValue = drv["FLDNONWATCHKEEPINGTO4"].ToString();
                }
            }
            if (e.Item is GridFooterItem)
            {
                RadComboBox ddlWKPeriod1FromAdd = (RadComboBox)e.Item.FindControl("ddlWKPeriod1FromAdd");
                if (ddlWKPeriod1FromAdd != null)
                {
                    ddlWKPeriod1FromAdd.DataSource = BindDuration();
                    ddlWKPeriod1FromAdd.DataBind();
                    ddlWKPeriod1FromAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlWKPeriod1ToAdd = (RadComboBox)e.Item.FindControl("ddlWKPeriod1ToAdd");
                if (ddlWKPeriod1ToAdd != null)
                {
                    ddlWKPeriod1ToAdd.DataSource = BindDuration();
                    ddlWKPeriod1ToAdd.DataBind();
                    ddlWKPeriod1ToAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlWKPeriod2FromAdd = (RadComboBox)e.Item.FindControl("ddlWKPeriod2FromAdd");
                if (ddlWKPeriod2FromAdd != null)
                {
                    ddlWKPeriod2FromAdd.DataSource = BindDuration();
                    ddlWKPeriod2FromAdd.DataBind();
                    ddlWKPeriod2FromAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlWKPeriod2ToAdd = (RadComboBox)e.Item.FindControl("ddlWKPeriod2ToAdd");
                if (ddlWKPeriod2ToAdd != null)
                {
                    ddlWKPeriod2ToAdd.DataSource = BindDuration();
                    ddlWKPeriod2ToAdd.DataBind();
                    ddlWKPeriod2ToAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKPeriod1FromAdd = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1FromAdd");
                if (ddlNWKPeriod1FromAdd != null)
                {
                    ddlNWKPeriod1FromAdd.DataSource = BindDuration();
                    ddlNWKPeriod1FromAdd.DataBind();
                    ddlNWKPeriod1FromAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKPeriod1ToAdd = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1ToAdd");
                if (ddlNWKPeriod1ToAdd != null)
                {
                    ddlNWKPeriod1ToAdd.DataSource = BindDuration();
                    ddlNWKPeriod1ToAdd.DataBind();
                    ddlNWKPeriod1ToAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKPeriod2FromAdd = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2FromAdd");
                if (ddlNWKPeriod2FromAdd != null)
                {
                    ddlNWKPeriod2FromAdd.DataSource = BindDuration();
                    ddlNWKPeriod2FromAdd.DataBind();
                    ddlNWKPeriod2FromAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKPeriod2ToAdd = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2ToAdd");
                if (ddlNWKPeriod2ToAdd != null)
                {
                    ddlNWKPeriod2ToAdd.DataSource = BindDuration();
                    ddlNWKPeriod2ToAdd.DataBind();
                    ddlNWKPeriod2ToAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkHours_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                {
                    string ResthoursDefaultWorkHours = ((RadLabel)e.Item.FindControl("lblresthrdefaultworkhrs")).Text;
                    string wkfrom1 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod1From")).Text;
                    string wkto1 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod1To")).Text;
                    string wkfrom2 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod2From")).Text;
                    string wkto2 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod2To")).Text;
                    string nwkfrom1 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod1From")).Text;
                    string nwkto1 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod1To")).Text;
                    string nwkfrom2 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod2From")).Text;
                    string nwkto2 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod2To")).Text;

                    if (!IsValidWorkHours(wkfrom1, wkto1, wkfrom2, wkto2, nwkfrom1, nwkto1, nwkfrom2, nwkto2))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixVesselAccountsRH.DefaultWorkHoursUpdate(
                        General.GetNullableGuid(ResthoursDefaultWorkHours)
                            , int.Parse(ViewState["EmployeeId"].ToString())
                            , General.GetNullableGuid(ViewState["RestHourEmployeeId"].ToString())
                            , 1
                            , General.GetNullableInteger(wkfrom1)
                            , General.GetNullableInteger(wkto1), General.GetNullableInteger(wkfrom2), General.GetNullableInteger(wkto2)
                            , General.GetNullableInteger(nwkfrom1), General.GetNullableInteger(nwkto1), General.GetNullableInteger(nwkfrom2)
                            , General.GetNullableInteger(nwkto2)
                        );
                    Rebind();
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    ucError.ErrorMessage = "You cannot configure default work hours in office, Please configure only from ship.";
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkHoursPort_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataPort();
        BindDetails();
    }
    protected void RebindPort()
    {
        gvWorkHoursPort.SelectedIndexes.Clear();
        gvWorkHoursPort.EditIndexes.Clear();
        gvWorkHoursPort.DataSource = null;
        gvWorkHoursPort.Rebind();
    }
    protected void gvWorkHoursPort_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadComboBox ddlWKP1Fromport = (RadComboBox)e.Item.FindControl("ddlWKPeriod1Fromport");
                if (ddlWKP1Fromport != null)
                {
                    ddlWKP1Fromport.DataSource = BindDuration();
                    ddlWKP1Fromport.DataBind();
                    ddlWKP1Fromport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKP1Fromport.SelectedValue = drv["FLDWATCHKEEPINGFROM1"].ToString();
                }
                RadComboBox ddlWKP1Toport = (RadComboBox)e.Item.FindControl("ddlWKPeriod1Toport");
                if (ddlWKP1Toport != null)
                {
                    ddlWKP1Toport.DataSource = BindDuration();
                    ddlWKP1Toport.DataBind();
                    ddlWKP1Toport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKP1Toport.SelectedValue = drv["FLDWATCHKEEPINGTO1"].ToString();
                }
                RadComboBox ddlWKP2Fromport = (RadComboBox)e.Item.FindControl("ddlWKPeriod2Fromport");
                if (ddlWKP2Fromport != null)
                {
                    ddlWKP2Fromport.DataSource = BindDuration();
                    ddlWKP2Fromport.DataBind();
                    ddlWKP2Fromport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKP2Fromport.SelectedValue = drv["FLDWATCHKEEPINGFROM2"].ToString();
                }
                RadComboBox ddlWKP2Toport = (RadComboBox)e.Item.FindControl("ddlWKPeriod2Toport");
                if (ddlWKP2Toport != null)
                {
                    ddlWKP2Toport.DataSource = BindDuration();
                    ddlWKP2Toport.DataBind();
                    ddlWKP2Toport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlWKP2Toport.SelectedValue = drv["FLDWATCHKEEPINGTO2"].ToString();
                }
                RadComboBox ddlNWKP1Fromport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1Fromport");
                if (ddlNWKP1Fromport != null)
                {
                    ddlNWKP1Fromport.DataSource = BindDuration();
                    ddlNWKP1Fromport.DataBind();
                    ddlNWKP1Fromport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKP1Fromport.SelectedValue = drv["FLDNONWATCHKEEPINGFROM3"].ToString();
                }
                RadComboBox ddlNWKP1Toport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1Toport");
                if (ddlNWKP1Toport != null)
                {
                    ddlNWKP1Toport.DataSource = BindDuration();
                    ddlNWKP1Toport.DataBind();
                    ddlNWKP1Toport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKP1Toport.SelectedValue = drv["FLDNONWATCHKEEPINGTO3"].ToString();
                }
                RadComboBox ddlNWKP2Fromport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2Fromport");
                if (ddlNWKP2Fromport != null)
                {
                    ddlNWKP2Fromport.DataSource = BindDuration();
                    ddlNWKP2Fromport.DataBind();
                    ddlNWKP2Fromport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKP2Fromport.SelectedValue = drv["FLDNONWATCHKEEPINGFROM4"].ToString();
                }
                RadComboBox ddlNWKP2Toport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2Toport");
                if (ddlNWKP2Toport != null)
                {
                    ddlNWKP2Toport.DataSource = BindDuration();
                    ddlNWKP2Toport.DataBind();
                    ddlNWKP2Toport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlNWKP2Toport.SelectedValue = drv["FLDNONWATCHKEEPINGTO4"].ToString();
                }
            }
            if (e.Item is GridFooterItem)
            {
                RadComboBox ddlWKP1FromAddport = (RadComboBox)e.Item.FindControl("ddlWKPeriod1FromAddport");
                if (ddlWKP1FromAddport != null)
                {
                    ddlWKP1FromAddport.DataSource = BindDuration();
                    ddlWKP1FromAddport.DataBind();
                    ddlWKP1FromAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlWKP1ToAddport = (RadComboBox)e.Item.FindControl("ddlWKPeriod1ToAddport");
                if (ddlWKP1ToAddport != null)
                {
                    ddlWKP1ToAddport.DataSource = BindDuration();
                    ddlWKP1ToAddport.DataBind();
                    ddlWKP1ToAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlWKP2FromAddport = (RadComboBox)e.Item.FindControl("ddlWKPeriod2FromAddport");
                if (ddlWKP2FromAddport != null)
                {
                    ddlWKP2FromAddport.DataSource = BindDuration();
                    ddlWKP2FromAddport.DataBind();
                    ddlWKP2FromAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlWKP2ToAddport = (RadComboBox)e.Item.FindControl("ddlWKPeriod2ToAddport");
                if (ddlWKP2ToAddport != null)
                {
                    ddlWKP2ToAddport.DataSource = BindDuration();
                    ddlWKP2ToAddport.DataBind();
                    ddlWKP2ToAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKP1FromAddport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1FromAddport");
                if (ddlNWKP1FromAddport != null)
                {
                    ddlNWKP1FromAddport.DataSource = BindDuration();
                    ddlNWKP1FromAddport.DataBind();
                    ddlNWKP1FromAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKP1ToAddport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod1ToAddport");
                if (ddlNWKP1ToAddport != null)
                {
                    ddlNWKP1ToAddport.DataSource = BindDuration();
                    ddlNWKP1ToAddport.DataBind();
                    ddlNWKP1ToAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKP2FromAddport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2FromAddport");
                if (ddlNWKP2FromAddport != null)
                {
                    ddlNWKP2FromAddport.DataSource = BindDuration();
                    ddlNWKP2FromAddport.DataBind();
                    ddlNWKP2FromAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
                RadComboBox ddlNWKP2ToAddport = (RadComboBox)e.Item.FindControl("ddlNWKPeriod2ToAddport");
                if (ddlNWKP2ToAddport != null)
                {
                    ddlNWKP2ToAddport.DataSource = BindDuration();
                    ddlNWKP2ToAddport.DataBind();
                    ddlNWKP2ToAddport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkHoursPort_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string ResthoursDefaultWorkHours = ((RadLabel)e.Item.FindControl("lblresthrdefaultworkhrsport")).Text;
                string wkfrom1 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod1Fromport")).Text;
                string wkto1 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod1Toport")).Text;
                string wkfrom2 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod2Fromport")).Text;
                string wkto2 = ((RadComboBox)e.Item.FindControl("ddlWKPeriod2Toport")).Text;
                string nwkfrom1 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod1Fromport")).Text;
                string nwkto1 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod1Toport")).Text;
                string nwkfrom2 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod2Fromport")).Text;
                string nwkto2 = ((RadComboBox)e.Item.FindControl("ddlNWKPeriod2Toport")).Text;

                if (!IsValidWorkHoursInPort(wkfrom1, wkto1, wkfrom2, wkto2, nwkfrom1, nwkto1, nwkfrom2, nwkto2))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsRH.DefaultWorkHoursUpdate(
                    General.GetNullableGuid(ResthoursDefaultWorkHours)
                        , int.Parse(ViewState["EmployeeId"].ToString())
                        , General.GetNullableGuid(ViewState["RestHourEmployeeId"].ToString())
                        , 0
                        , General.GetNullableInteger(wkfrom1)
                        , General.GetNullableInteger(wkto1), General.GetNullableInteger(wkfrom2), General.GetNullableInteger(wkto2)
                        , General.GetNullableInteger(nwkfrom1), General.GetNullableInteger(nwkto1), General.GetNullableInteger(nwkfrom2)
                        , General.GetNullableInteger(nwkto2)
                    );
                RebindPort();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

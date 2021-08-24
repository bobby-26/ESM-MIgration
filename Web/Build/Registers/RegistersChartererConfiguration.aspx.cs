using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class RegistersChartererConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucChartererStanard.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersChartererConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>","Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvChartererConfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                                                                
           // MenuChartererConfiguration.AccessRights = this.ViewState;
            //MenuChartererConfiguration.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            CharterConfigMenu.AccessRights = this.ViewState;
            CharterConfigMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["addresscode"] = "";

                if (Request.QueryString["addresscode"] != null && Request.QueryString["addresscode"].ToString() != "")
                    ViewState["addresscode"] = Request.QueryString["addresscode"].ToString();
                gvChartererConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                EditChartererConfig();

            }            
            //BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void EditChartererConfig()
    {
        if (ViewState["addresscode"] != null && ViewState["addresscode"].ToString() != "")
        {
            DataSet ds = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["addresscode"].ToString()));
            if(ds.Tables[0].Rows.Count > 0)
                txtCharterer.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();

            DataTable dt = PhoenixRegistersChartererConfiguration.EditChartererConfigInfo(int.Parse(ViewState["addresscode"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtShortName.Text = dt.Rows[0]["FLDCHARTERERSHORTNAME"].ToString();
                ucChartererStanard.SelectedAddress = dt.Rows[0]["FLDMATRIXSTANDARD"].ToString();
                if (dt.Rows[0]["FLDOWNSTANDARDYN"].ToString() == "1")
                    ckbownstandard.Checked = true;
            }
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDGROUPNAME", "FLDCONTRACTPERIOD" };
        string[] alCaptions = { "Rank Group", "Contract Period (in days)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersChartererConfiguration.ChartererConfigurationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["addresscode"].ToString()),
                sortexpression, sortdirection,
                1,
               iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        General.ShowExcel("Charterer Configuration", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void CharterConfigMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!isValidCharterer())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersChartererConfiguration.InsertChartererConfigInfo(int.Parse(ViewState["addresscode"].ToString())
                        , General.GetNullableString(txtShortName.Text)
                        , General.GetNullableInteger(ucChartererStanard.SelectedAddress)
                        , ckbownstandard.Checked == true ? 1 : 0);
                ucStatus.Text = "Charterer information updated.";
                EditChartererConfig();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool isValidCharterer()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtShortName.Text) == null)
            ucError.ErrorMessage = "Charterer short name is required.";
       //if(General.GetNullableInteger(ucChartererStanard.SelectedAddress) == null)
       //    ucError.ErrorMessage = "Please select the Training Matrix Standard";
        return (!ucError.IsError);
    }

    protected void MenuChartererConfiguration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDGROUPNAME", "FLDCONTRACTPERIOD" };
        string[] alCaptions = { "Rank Group", "Contract Period (in days)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                        
        DataTable dt = PhoenixRegistersChartererConfiguration.ChartererConfigurationCertvalititySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["addresscode"].ToString()), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvChartererConfiguration.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvChartererConfiguration", "Charterer Configuration", alCaptions, alColumns, ds);

        gvChartererConfiguration.DataSource = ds;
        gvChartererConfiguration.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
    }

    protected void gvChartererConfiguration_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            //DropDownList ddlRankGroupEdit = (DropDownList)e.Row.FindControl("ddlRankGroupEdit");
            //if (ddlRankGroupEdit != null)
            //{
            //    ddlRankGroupEdit.DataSource = PhoenixRegistersRank.ListRankGroupMapping();
            //    ddlRankGroupEdit.DataTextField = "FLDGROUPNAME";
            //    ddlRankGroupEdit.DataValueField = "FLDMAPPINGID";
            //    ddlRankGroupEdit.DataBind();
            //    ddlRankGroupEdit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            //    ddlRankGroupEdit.SelectedValue = dr["FLDRANKGROUPMAPPINGID"].ToString();
            //}
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            //DropDownList ddlRankGroupAdd = (DropDownList)e.Row.FindControl("ddlRankGroupAdd");
            //if (ddlRankGroupAdd != null)
            //{
            //    ddlRankGroupAdd.DataSource = PhoenixRegistersRank.ListRankGroupMapping();
            //    ddlRankGroupAdd.DataTextField = "FLDGROUPNAME";
            //    ddlRankGroupAdd.DataValueField = "FLDMAPPINGID";
            //    ddlRankGroupAdd.DataBind();
            //    ddlRankGroupAdd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            //}
        }
    }

    protected void gvChartererConfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((UserControlMaskNumber)e.Item.FindControl("ucmaxtourdutyAdd")).Text,
                                 ((UserControlMaskNumber)e.Item.FindControl("ucCertPeriodAdd")).Text)
                                 )
                {
                    ucError.Visible = true;
                    return;
                }

                //PhoenixRegistersChartererConfiguration.InsertChartererConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //    , int.Parse(ViewState["addresscode"].ToString())
                //    , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlRankGroupAdd")).SelectedValue)
                //    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucContractPeriodAdd")).Text)
                //    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucRangeAdd")).Text)
                //    );
                PhoenixRegistersChartererConfiguration.InsertChartererCertValidityConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , int.Parse(ViewState["addresscode"].ToString())
                   , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucmaxtourdutyAdd")).Text)
                   , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucCertPeriodAdd")).Text)
                   );

                BindData();
                gvChartererConfiguration.Rebind();
                // ((DropDownList)_gridView.FooterRow.FindControl("ddlRankGroupAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string configurationid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCONFIGURATIONID"].ToString();
                PhoenixRegistersChartererConfiguration.DeleteChartererCertValidityConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(configurationid));
                BindData();
                gvChartererConfiguration.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlRankGroupEdit")).SelectedValue,
                //        ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucContractPeriodEdit")).Text,
                //        ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucRangeEdit")).Text))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                if (!IsValidData(((UserControlMaskNumber)e.Item.FindControl("ucmaxtourdutyEdit")).Text,
                                     ((UserControlMaskNumber)e.Item.FindControl("ucCertPeriodEdit")).Text)
                                     )

                {
                    ucError.Visible = true;
                    return;
                }
                string configurationid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCONFIGURATIONID"].ToString();

                //PhoenixRegistersChartererConfiguration.UpdateChartererConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //    , int.Parse(configurationid)
                //    , int.Parse(ViewState["addresscode"].ToString())
                //    , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlRankGroupEdit")).SelectedValue)
                //    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucContractPeriodEdit")).Text)
                //    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucRangeEdit")).Text)
                //    );
                //21606
                PhoenixRegistersChartererConfiguration.InsertChartererCertValidityConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                      , int.Parse(ViewState["addresscode"].ToString())
                      , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucmaxtourdutyEdit")).Text)
                      , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucCertPeriodEdit")).Text)
                      );

                BindData();
                gvChartererConfiguration.Rebind();
            }
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string rankgroupmappingid, string contractperiod, string range)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(rankgroupmappingid) == null)
            ucError.ErrorMessage = "Rank Group is required.";

        if (General.GetNullableInteger(contractperiod) == null)
            ucError.ErrorMessage = "Contract Period is required.";

        return (!ucError.IsError);
    }
    //21606
    private bool IsValidData(string maxtour, string certvalidity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(maxtour) == null)
            ucError.ErrorMessage = "Max Tour of Duty  is required.";

        if (General.GetNullableInteger(certvalidity) == null)
            ucError.ErrorMessage = "Certificate validity  is required.";

        return (!ucError.IsError);
    }

    protected void gvChartererConfiguration_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvChartererConfiguration.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

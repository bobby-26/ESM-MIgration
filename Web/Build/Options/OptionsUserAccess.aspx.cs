using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Options_OptionsUserAccess : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Defaults", "USERACCESS");
            if (Request.QueryString["usercode"] != null)
                toolbar.AddButton("Approvals", "APPROVALLIMITS");
            toolbar.AddButton("Zone", "USERZONE");
            toolbar.AddButton("Rank", "USERRANK");
            toolbar.AddButton("Vessel", "USERVESSEL");
            toolbar.AddButton("Vessel Type", "USERVESSELTYPE");
            toolbar.AddButton("Fleet", "USERFLEET");
            toolbar.AddButton("Pool", "USERPOOL");
            toolbar.AddButton("Alert", "USERALERT");
            toolbar.AddButton("Company", "USERCOMPANY");
            toolbar.AddButton("Department", "USERDEPARTMENT");
            toolbar.AddButton("Sub Department", "USERSUBDEPARTMENT");
            if ((Request.QueryString["accessid"] != null) && (Request.QueryString["GroupId"] != null))
                toolbar.AddButton("HSEQA", "HSEQAACCESS");

            MenuUserAccessList.MenuList = toolbar.Show();
            MenuUserAccessList.SelectedMenuIndex = 0;

            toolbar = new PhoenixToolbar();

            if (Request.QueryString["usercode"] != null)
                ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
            if (ViewState["USERCODE"] != null)
                toolbar.AddButton("Cancel", "USER", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuUserAccess.MenuList = toolbar.Show();

            if (Request.QueryString["GroupId"] != null)
                ViewState["GroupId"] = Request.QueryString["GroupId"].ToString();

            ViewState["ACCESSID"] = Request.QueryString["accessid"].ToString();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ShowDataAccess(int.Parse(ViewState["ACCESSID"].ToString()));
                gvCompanyConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void Rebind()
    {
        gvCompanyConfiguration.SelectedIndexes.Clear();
        gvCompanyConfiguration.EditIndexes.Clear();
        gvCompanyConfiguration.DataSource = null;
        gvCompanyConfiguration.Rebind();
    }


    private void ShowDataAccess(int accessid)
    {
        DataSet ds = SessionUtil.AccessEdit(accessid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucDefaultCompany.SelectedCompany = dr["FLDDEFAULTCOMPANYID"].ToString();
            ucDefaultVessel.SelectedVessel = dr["FLDDEFAULTVESSELID"].ToString();
            ucRegisteredCompany.SelectedCompany = dr["FLDREGISTEREDCOMPANYID"].ToString();
            ucRegisteredZone.SelectedZone = dr["FLDREGISTEREDZONE"].ToString();
            txtPOApprovalLimit.Text = dr["FLDPOAPPROVALLIMIT"].ToString();
        }
    }

    private bool IsValidSave(string ucDefaultCompany, string ucDefaultVessel, string ucRegisteredCompany, string ucRegisteredZone, string txtPOApprovalLimit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucDefaultCompany.Equals(""))
            ucError.ErrorMessage = "Default company is required.";

        if (ucDefaultVessel.Equals(""))
            ucError.ErrorMessage = "Default vessel is required.";
        if (ucRegisteredCompany.Equals(""))
            ucError.ErrorMessage = "Registered company is required.";
        if (ucRegisteredZone.Equals(""))
            ucError.ErrorMessage = "Registered zone is required.";
        if (txtPOApprovalLimit.Equals(""))
            ucError.ErrorMessage = "POApprovalLimit is required.";

   
        return (!ucError.IsError);
    }

    protected void UserAccess_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToString().ToUpper().Equals("SAVE"))
            {
                if (!IsValidSave(ucDefaultCompany.SelectedCompany.ToString(),ucDefaultVessel.SelectedVessel,
                    ucRegisteredCompany.SelectedCompany,ucRegisteredZone.SelectedZone,((RadTextBox)FindControl("txtPOApprovalLimit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateUserAccess(int.Parse(ViewState["ACCESSID"].ToString()),int.Parse(ViewState["USERCODE"].ToString()),
                    ucDefaultCompany.SelectedCompany,ucDefaultVessel.SelectedVessel,ucRegisteredCompany.SelectedCompany,
                    ucRegisteredZone.SelectedZone,txtPOApprovalLimit.Text);
                Rebind();               
            }
            if (CommandName.ToUpper().Equals("USER"))
            {
                Response.Redirect("OptionsUser.aspx?usercode=" + ViewState["USERCODE"].ToString(), false);
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void UpdateUserAccess(int accessid, int usercode, string defaultcompany, string defaultvessel, string registeredcompany, string registeredzone, string poapprovallimit)
    {
        SessionUtil.UpdateAccess(accessid,General.GetNullableInteger(defaultcompany),General.GetNullableInteger(defaultvessel),
            General.GetNullableInteger(registeredcompany),General.GetNullableInteger(registeredzone),General.GetNullableDecimal(poapprovallimit));

        ucStatus.Text = "User defaults saved.";
    }

    protected void UserAccessList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("USERACCESS"))
            {
                if (ViewState["ACCESSID"] != null)
                {
                    if (ViewState["USERCODE"] != null)
                        Response.Redirect("OptionsUserAccess.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
                    else
                        Response.Redirect("OptionsUserAccess.aspx?accessid=" + ViewState["ACCESSID"].ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("APPROVALLIMITS"))
            {
                if (ViewState["ACCESSID"] != null)
                {
                    if (ViewState["USERCODE"] != null)
                        Response.Redirect("OptionUserAccessPOApprovalLimit.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
                    else
                        Response.Redirect("OptionUserAccessPOApprovalLimit.aspx?accessid=" + ViewState["ACCESSID"].ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("HSEQAACCESS"))
            {
                if (ViewState["ACCESSID"] != null)
                {
                    if ((Request.QueryString["usercode"] == null) && (Request.QueryString["GroupId"] != null))
                        Response.Redirect("OptionsHSEQAUserAccess.aspx?GroupId=" + Request.QueryString["GroupId"].ToString() + "&accessid=" + Request.QueryString["accessid"].ToString());
                }
            }
            else
            {
                if (ViewState["ACCESSID"] != null)
                {
                    if (ViewState["USERCODE"] != null)
                        Response.Redirect("OptionsUserAccessList.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString() + "&menuindex=" + dce.Item + "&commandname=" + CommandName);

                    else if ((Request.QueryString["usercode"] == null) && (Request.QueryString["GroupId"] != null))
                        Response.Redirect("OptionsUserAccessList.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString() + "&menuindex=" + dce.Item + "&commandname=" + CommandName);

                    else
                        Response.Redirect("OptionsUserAccessList.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&menuindex=" + dce.Item + "&commandname=" + CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    // default company configuration

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
            DataSet ds = SessionUtil.SearchCompanyConfiguration(int.Parse(ViewState["ACCESSID"].ToString())
                , null, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvCompanyConfiguration.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvCompanyConfiguration.DataSource = ds;
            gvCompanyConfiguration.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidCompany(string accessshortcode, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(accessshortcode) == null)
            ucError.ErrorMessage = "Access Short Code is required.";

        if (General.GetNullableInteger(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }


    protected void InsertCompanyConfiguration(string accessshortcode, string companyid)
    {
        SessionUtil.InsertCompanyConfiguration(int.Parse(ViewState["ACCESSID"].ToString()), General.GetNullableString(accessshortcode), int.Parse(companyid));
    }

    protected void DeleteCompanyConfiguration(string dtkey)
    {
        SessionUtil.DeleteCompanyConfiguration(new Guid(dtkey));
    }

    protected void UpdateCompanyConfiguration(string dtkey, string accessshortcode, string companyid)
    {
        SessionUtil.UpdateCompanyConfiguration(new Guid(dtkey), General.GetNullableString(accessshortcode), int.Parse(companyid));
    }

    protected void gvCompanyConfiguration_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {           
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCompany(((RadTextBox)e.Item.FindControl("txtAccessShortCode")).Text
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertCompanyConfiguration(((RadTextBox)e.Item.FindControl("txtAccessShortCode")).Text
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany);
                ((RadTextBox)e.Item.FindControl("txtAccessShortCode")).Focus();
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCompany(
                   ((RadTextBox)e.Item.FindControl("txtAccessShortCodeEdit")).Text
                  , ((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateCompanyConfiguration(((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text
                        , ((RadTextBox)e.Item.FindControl("txtAccessShortCodeEdit")).Text
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCompanyConfiguration(((RadLabel)e.Item.FindControl("lblDTKey")).Text);
                Rebind();
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

    protected void gvCompanyConfiguration_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

          
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }

        if (e.Item.IsInEditMode)
        {

            RadTextBox ShortCode = (RadTextBox)e.Item.FindControl("txtAccessShortCodeEdit");
            UserControlCompany ddlCompany = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            DataRowView drvDeptartmentType = (DataRowView)e.Item.DataItem;
            if (ShortCode != null) ShortCode.Text = DataBinder.Eval(e.Item.DataItem, "FLDACCESSSHORTCODE").ToString();
            if (ddlCompany != null) ddlCompany.SelectedCompany = DataBinder.Eval(e.Item.DataItem, "FLDCOMPANYID").ToString();
        }
    }

    protected void gvCompanyConfiguration_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCompanyConfiguration.CurrentPageIndex + 1;
        BindData();
    }
}

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
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class OptionsUserAccessList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Sub Department", "USERSUBDEPARTMENT", ToolBarDirection.Right);
        toolbar.AddButton("Department", "USERDEPARTMENT", ToolBarDirection.Right);
        toolbar.AddButton("Company", "USERCOMPANY", ToolBarDirection.Right);
        toolbar.AddButton("Alert", "USERALERT", ToolBarDirection.Right);
        toolbar.AddButton("Pool", "USERPOOL", ToolBarDirection.Right);
        toolbar.AddButton("Fleet", "USERFLEET", ToolBarDirection.Right);
        toolbar.AddButton("Vessel Type", "USERVESSELTYPE", ToolBarDirection.Right);
        toolbar.AddButton("Rank", "USERRANK", ToolBarDirection.Right);
        toolbar.AddButton("Vessel", "USERVESSEL", ToolBarDirection.Right);
        toolbar.AddButton("Zone", "USERZONE", ToolBarDirection.Right);
        if (Request.QueryString["usercode"] != null)
            toolbar.AddButton("Approvals", "APPROVALLIMITS", ToolBarDirection.Right);
        toolbar.AddButton("Defaults", "USERACCESS", ToolBarDirection.Right);

        if ((Request.QueryString["accessid"] != null) && (Request.QueryString["GroupId"] != null))
            toolbar.AddButton("HSEQA", "HSEQAACCESS");

        MenuUserAccessList.MenuList = toolbar.Show();
        // MenuUserAccessList.SelectedMenuIndex = int.Parse(Request.QueryString["menuindex"].ToString());

        if (Request.QueryString["accessid"] != null)
            ViewState["ACCESSID"] = Request.QueryString["accessid"].ToString();
        else
            Response.Redirect("OptionsUserAccess.aspx");

        if (Request.QueryString["usercode"] != null)
            ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();

        if (Request.QueryString["GroupId"] != null)
            ViewState["GroupId"] = Request.QueryString["GroupId"].ToString();

        if (!IsPostBack)
        {

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvUserAccessList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void UserAccessList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("USERACCESS"))
        {
            if (ViewState["ACCESSID"] != null)
            {
                if (ViewState["USERCODE"] != null)
                    Response.Redirect("OptionsUserAccess.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());

                else if (Request.QueryString["GroupId"] != null)
                    Response.Redirect("OptionsUserAccess.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString());
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
            if ((ViewState["ACCESSID"] != null) && (Request.QueryString["usercode"] == null))
            {
                Response.Redirect("OptionsHSEQAUserAccess.aspx?GroupId=" + Request.QueryString["GroupId"].ToString() + "&accessid=" + Request.QueryString["accessid"].ToString());
            }
        }
        else
        {
            if (ViewState["ACCESSID"] != null)
            {
                if (ViewState["USERCODE"] != null)
                    Response.Redirect("OptionsUserAccessList.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString() + "&menuindex=" + dce.Item + "&commandname=" + CommandName);

                else if ((ViewState["ACCESSID"] != null) && (Request.QueryString["GroupId"] != null))
                    Response.Redirect("OptionsUserAccessList.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString() + "&menuindex=" + dce.Item + "&commandname=" + CommandName);

                else
                    Response.Redirect("OptionsUserAccessList.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&menuindex=" + dce.Item.TabIndex + "&commandname=" + CommandName);
            }
        }
    }

    private void BindData()
    {
        DataSet ds = BindAccessList(Request.QueryString["commandname"].ToString());
        gvUserAccessList.DataSource = ds;
        gvUserAccessList.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    private DataSet BindAccessList(string commandname)
    {
        DataSet ds = new DataSet();

        if (commandname.ToUpper().Equals("USERCOMPANY"))
            ds = SessionUtil.Access2CompanyList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERZONE"))
            ds = SessionUtil.Access2ZoneList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERPOOL"))
            ds = SessionUtil.Access2PoolList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERRANK"))
            ds = SessionUtil.Access2RankList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERFLEET"))
            ds = SessionUtil.Access2FleetList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERVESSEL"))
            ds = SessionUtil.Access2VesselList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERVESSELTYPE"))
            ds = SessionUtil.Access2VesselTypeList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERALERT"))
            ds = SessionUtil.Access2AlertList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERDEPARTMENT"))
            ds = SessionUtil.Access2DepartmentList(int.Parse(ViewState["ACCESSID"].ToString()));
        if (commandname.ToUpper().Equals("USERSUBDEPARTMENT"))
            ds = SessionUtil.Access2SubDepartmentList(int.Parse(ViewState["ACCESSID"].ToString()));

        return ds;
    }
    private void SaveAccessList(string recordid, string commandname)
    {
        int accessid = int.Parse(ViewState["ACCESSID"].ToString());
        int id = int.Parse(recordid);

        if (commandname.ToUpper().Equals("USERCOMPANY"))
            SessionUtil.MapAccess2CompanyList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERZONE"))
            SessionUtil.MapAccess2ZoneList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERPOOL"))
            SessionUtil.MapAccess2PoolList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERRANK"))
            SessionUtil.MapAccess2RankList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERFLEET"))
            SessionUtil.MapAccess2FleetList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERVESSEL"))
            SessionUtil.MapAccess2VesselList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERVESSELTYPE"))
            SessionUtil.MapAccess2VesselTypeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERALERT"))
            SessionUtil.MapAccess2AlertList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERDEPARTMENT"))
            SessionUtil.MapAccess2DepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        if (commandname.ToUpper().Equals("USERSUBDEPARTMENT"))
            SessionUtil.MapAccess2SubDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid, id);
        Rebind();
    }
    protected void gvUserAccessList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        string commandname = Request.QueryString["commandname"].ToString();
        if (e.Item is GridHeaderItem)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblCaption");
            if (lbl != null)
            {

                if (commandname.ToUpper().Equals("USERCOMPANY"))
                    lbl.Text = "Company";
                if (commandname.ToUpper().Equals("USERZONE"))
                    lbl.Text = "Zone";
                if (commandname.ToUpper().Equals("USERPOOL"))
                    lbl.Text = "Pool";
                if (commandname.ToUpper().Equals("USERRANK"))
                    lbl.Text = "Rank";
                if (commandname.ToUpper().Equals("USERFLEET"))
                    lbl.Text = "Fleet";
                if (commandname.ToUpper().Equals("USERVESSEL"))
                    lbl.Text = "Vessel";
                if (commandname.ToUpper().Equals("USERVESSELTYPE"))
                    lbl.Text = "Vessel Type";
                if (commandname.ToUpper().Equals("USERALERT"))
                    lbl.Text = "Alert";
                if (commandname.ToUpper().Equals("USERDEPARTMENT"))
                    lbl.Text = "Department";
                if (commandname.ToUpper().Equals("USERSUBDEPARTMENT"))
                    lbl.Text = "Sub Department";
                lbl.ForeColor = System.Drawing.Color.White;
            }

        }
        if (e.Item is GridDataItem)
        {
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkUserRights");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (cb != null)
                cb.Checked = drv["FLDRIGHTS"].ToString().Equals("1") ? true : false;
        }

    }
    protected void Rebind()
    {
        gvUserAccessList.SelectedIndexes.Clear();
        gvUserAccessList.EditIndexes.Clear();
        gvUserAccessList.DataSource = null;
        gvUserAccessList.Rebind();
    }
    protected void gvUserAccessList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUserAccessList.CurrentPageIndex + 1;
        BindData();
    }
    protected void gvUserAccessList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string recordid = ((RadLabel)e.Item.FindControl("lblID")).Text;
                int accessid = int.Parse(ViewState["ACCESSID"].ToString());

                SaveAccessList(recordid, Request.QueryString["commandname"].ToString());
                Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }
}

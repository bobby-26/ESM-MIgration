using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class OptionUserAccessPOApprovalLimit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Options/OptionUserAccessPOApprovalLimit.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Options/OptionUserAccessPOApprovalLimit.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuDivPO.AccessRights = this.ViewState;
            MenuDivPO.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Sub Department", "USERSUBDEPARTMENT", ToolBarDirection.Right);
            toolbar.AddButton("Department", "USERDEPARTMENT", ToolBarDirection.Right);
            toolbar.AddButton("Company", "USERCOMPANY", ToolBarDirection.Right);
            toolbar.AddButton("Alert", "USERALERT", ToolBarDirection.Right);
            toolbar.AddButton("Pool", "USERPOOL", ToolBarDirection.Right);
            toolbar.AddButton("Fleet", "USERFLEET", ToolBarDirection.Right);
            toolbar.AddButton("Vessel Type", "USERVESSELTYPE", ToolBarDirection.Right);
            toolbar.AddButton("Vessel", "USERVESSEL", ToolBarDirection.Right);
            toolbar.AddButton("Rank", "USERRANK", ToolBarDirection.Right);
            toolbar.AddButton("Zone", "USERZONE", ToolBarDirection.Right);
            toolbar.AddButton("Approvals", "APPROVALLIMITS", ToolBarDirection.Right);
            toolbar.AddButton("Defaults", "USERACCESS", ToolBarDirection.Right);

            MenuUserAccessList.MenuList = toolbar.Show();
            MenuUserAccessList.SelectedMenuIndex = 10;        

            if (!IsPostBack)
            {
                if (Request.QueryString["usercode"] != null)
                    ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
                if (Request.QueryString["accessid"] != null)
                    ViewState["ACCESSID"] = Request.QueryString["accessid"].ToString();
                else
                    Response.Redirect("OptionsUserAccess.aspx");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvPOApprovalLimitsList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //ucVesselName.AssignedVessels = true;
                //ucVesselName.VesselList  = PhoenixRegistersVessel.ListAssignedVessel(int.Parse(ViewState["USERCODE"].ToString()), null);
                //ucVesselName.DataBind();

                ddlVesselName.DataTextField = "FLDVESSELNAME";
                ddlVesselName.DataValueField = "FLDVESSELID";
                ddlVesselName.DataSource = SessionUtil.Access2POVesselList(int.Parse(ViewState["USERCODE"].ToString()));
                ddlVesselName.DataBind();

                ddlVesselName.Items.Insert(0, new DropDownListItem("--Select--", "--Dummy--"));
                ddlVesselName.Items.Insert(1, new DropDownListItem("--Office--", "0"));
    
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
        gvPOApprovalLimitsList.SelectedIndexes.Clear();
        gvPOApprovalLimitsList.EditIndexes.Clear();
        gvPOApprovalLimitsList.DataSource = null;
        gvPOApprovalLimitsList.Rebind();
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
        else
        {
            if (ViewState["ACCESSID"] != null)
            {
                if (ViewState["USERCODE"] != null)
                    Response.Redirect("OptionsUserAccessList.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString() + "&menuindex=" + dce.Item + "&commandname=" + CommandName);
                else
                    Response.Redirect("OptionsUserAccessList.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&menuindex=" + dce.Item.Index + "&commandname=" + CommandName);
            }
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

            DataSet ds = SessionUtil.Access2POApprovalLimit(int.Parse(ViewState["USERCODE"].ToString()),
                General.GetNullableInteger(ucPOType.SelectedValue),
                General.GetNullableInteger(ddlVesselName.SelectedValue.ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvPOApprovalLimitsList.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvPOApprovalLimitsList.DataSource = ds;
            gvPOApprovalLimitsList.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidFormInsert(string vesselid, string POType, string primarylimit, string secondarylimit)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (vesselid.Equals("Dummy"))
            ucError.ErrorMessage = "Select a Vessel";
        if (POType.Equals("Dummy"))
            ucError.ErrorMessage = "Select PO Approval Type";
        if (string.IsNullOrEmpty(primarylimit))
            ucError.ErrorMessage = "primarylimit is required";
        if (string.IsNullOrEmpty(secondarylimit))
            ucError.ErrorMessage = "secondarylimit is required";
        return (!ucError.IsError);
    }

    private bool IsValidFormUpdate(string primarylimit, string secondarylimit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(primarylimit))
            ucError.ErrorMessage = "primarylimit is required";
        if (string.IsNullOrEmpty(secondarylimit))
            ucError.ErrorMessage = "secondarylimit is required";
        return (!ucError.IsError);
    }

    protected void gvPOApprovalLimitsList_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    protected void MenuDivPO_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVesselName.SelectedValue = "";
                ucPOType.SelectedValue = string.Empty; 
                ViewState["PAGENUMBER"] = 1;
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPOApprovalLimitsList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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


            {
                LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                    cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadDropDownList ddlVesselAdd = (RadDropDownList)e.Item.FindControl("ddlVesselAdd");
            if (ddlVesselAdd != null)
            {

                ddlVesselAdd.DataSource = SessionUtil.Access2POVesselList(int.Parse(ViewState["USERCODE"].ToString()));
                ddlVesselAdd.DataBind();

                ddlVesselAdd.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                ddlVesselAdd.Items.Insert(1, new DropDownListItem("--Office--", "0"));
            }

            UserControlQuick ucPOType = (UserControlQuick)e.Item.FindControl("ucPOApprovalCategoryAdd");
            if (ucPOType != null)
            {
                ucPOType.QuickList = PhoenixRegistersQuick.ListQuick(1, 152);
                ucPOType.DataBind();
            }

        }

    }

    protected void gvPOApprovalLimitsList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            //    if (e.CommandName.ToUpper().Equals("EDIT"))
            //{
            //    e.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            //}

            else if (e.CommandName.ToUpper().Equals("ADD"))

            {
                if (!IsValidFormInsert(((RadDropDownList)e.Item.FindControl("ddlVesselAdd")).SelectedValue.ToString().Trim(),
                    ((UserControlQuick)e.Item.FindControl("ucPOApprovalCategoryAdd")).SelectedQuick.ToString().Trim(),
                    ((UserControlMaskNumber)e.Item.FindControl("ucPrimaryLimitAdd")).ToString().Trim(),
                    ((UserControlMaskNumber)e.Item.FindControl("ucSecondaryLimitAdd")).ToString().Trim()))
                {
                    ucError.Visible = true;
                    return;
                }

                SessionUtil.Access2POApprovalLimitInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , int.Parse(ViewState["USERCODE"].ToString())
                                                        , General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlvesselAdd")).SelectedValue.ToString().Trim())
                                                        , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucPOApprovalCategoryAdd")).SelectedValue.ToString().Trim())
                                                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucPrimaryLimitAdd")).Text.ToString().Trim())
                                                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucSecondaryLimitAdd")).Text.ToString().Trim()));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                // if (!IsValidForm(((RadDropDownList)e.Item.FindControl("ddlVesselAdd")).SelectedValue.ToString().Trim(), ((UserControlQuick)e.Item.FindControl("ucPOApprovalCategoryAdd")).SelectedQuick.ToString().Trim()))
                if (!IsValidFormUpdate(((UserControlMaskNumber)e.Item.FindControl("ucPrimaryLimitEdit")).ToString().Trim(), ((UserControlMaskNumber)e.Item.FindControl("ucSecondaryLimitEdit")).ToString().Trim()))
                {
                    ucError.Visible = true;
                    return;
                }

                string limitid = ((RadLabel)e.Item.FindControl("lblLimitId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string type = ((RadLabel)e.Item.FindControl("lblPOApprovalCat")).Text.ToString().Trim();
                string primary = ((UserControlMaskNumber)e.Item.FindControl("ucPrimaryLimitEdit")).Text.ToString().Trim();
                string secondary = ((UserControlMaskNumber)e.Item.FindControl("ucSecondaryLimitEdit")).Text.ToString().Trim();

                SessionUtil.Access2POApprovalLimitUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , int.Parse(ViewState["USERCODE"].ToString())
                                                , new Guid(limitid)
                                                , Convert.ToInt32(vesselid)
                                                , General.GetNullableInteger(type)
                                                , General.GetNullableDecimal(primary)
                                                , General.GetNullableDecimal(secondary));


                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string DocumentFieldsId = (e.Item as GridDataItem).GetDataKeyValue("FLDUSERLIMITID").ToString();
                SessionUtil.Access2POApprovalLimitDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(DocumentFieldsId).Value);


                Rebind();
                //Access2POApprovalLimitDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDocumentFieldsId"))));

                //Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    //private void Access2POApprovalLimitDelete(Guid LimitID)
    //{
    //    SessionUtil.Access2POApprovalLimitDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, LimitID);
    //}


    protected void gvPOApprovalLimitsList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPOApprovalLimitsList.CurrentPageIndex + 1;
        BindData();
    }
}

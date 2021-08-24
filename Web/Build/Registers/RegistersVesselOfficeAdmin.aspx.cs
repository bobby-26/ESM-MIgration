using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersVesselOfficeAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuVesselOfficeAdmin.AccessRights = this.ViewState;
            MenuVesselOfficeAdmin.MenuList = toolbar1.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddButton("Vessel Search", "VESSELSEARCH", ToolBarDirection.Right);
            toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

            toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
            toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
            toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
            toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
            //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
            toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
            toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);

            MenuVesselList.AccessRights = this.ViewState;
            MenuVesselList.MenuList = toolbar.Show();
            MenuVesselList.SelectedMenuIndex = 4;
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                EditVessel();
                EditVesselOfficeAdmin();
                gvVesselAdminUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void EditVessel()
    {
        if (Filter.CurrentVesselMasterFilter != null && !string.IsNullOrEmpty(Filter.CurrentVesselMasterFilter.ToString()))
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Filter.CurrentVesselMasterFilter.ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                MenuVesselOfficeAdmin.Title = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            }
        }
    }
    protected void EditVesselOfficeAdmin()
    {
        if (Filter.CurrentVesselMasterFilter != null && !string.IsNullOrEmpty(Filter.CurrentVesselMasterFilter.ToString()))
        {
            DataSet ds = PhoenixRegistersVesselOfficeAdmin.OfficeAdminDetailsEdit(int.Parse(Filter.CurrentVesselMasterFilter.ToString()));
          
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["vesselid"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

                ucAccountsExecutive.SelectedValue = dr["FLDACCOUNTSEXECUTIVE"].ToString();
                ucAccountsExecutive.Text = dr["FLDACCOUNTSEXECUTIVENAME"].ToString() + (dr["FLDACCOUNTSEXECUTIVEDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDACCOUNTSEXECUTIVEDESIGNATION"].ToString() : "") + (dr["FLDACCOUNTSEXECUTIVEEMAIL"].ToString() != string.Empty ? " / " + dr["FLDACCOUNTSEXECUTIVEEMAIL"].ToString() : "");

                ucPurchaseSupdt.SelectedValue = dr["FLDPURCHASESUPDT"].ToString();
                ucPurchaseSupdt.Text = dr["FLDPURCHASESUPDTNAME"].ToString() + (dr["FLDPURCHASESUPDTDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDPURCHASESUPDTDESIGNATION"].ToString() : "") + (dr["FLDPURCHASESUPDTEMAIL"].ToString() != string.Empty ? " / " + dr["FLDPURCHASESUPDTEMAIL"].ToString() : "");

                ucQualitySupdtDesignation.SelectedValue = dr["FLDQUALITYSUPDT"].ToString();
                ucQualitySupdtDesignation.Text = dr["FLDQUALITYSUPDTNAME"].ToString() + (dr["FLDQUALITYSUPDTDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQUALITYSUPDTDESIGNATION"].ToString() : "") + (dr["FLDQUALITYSUPDTEMAIL"].ToString() != string.Empty ? " / " + dr["FLDQUALITYSUPDTEMAIL"].ToString() : "");

                ucQualityDirector.SelectedValue = dr["FLDQUALITYDIRECTOR"].ToString();
                ucQualityDirector.Text = dr["FLDQUALITYDIRECTORNAME"].ToString() + (dr["FLDQUALITYDIRECTORDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQUALITYDIRECTORDESIGNATION"].ToString() : "") + (dr["FLDQUALITYDIRECTOREMAIL"].ToString() != string.Empty ? " / " + dr["FLDQUALITYDIRECTOREMAIL"].ToString() : "");

                ucTravelPIC2.SelectedValue = dr["FLDTRAVELPIC2"].ToString();
                ucTravelPIC2.Text = dr["FLDTRAVELPIC2NAME"].ToString() + (dr["FLDTRAVELPIC2DESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPIC2DESIGNATION"].ToString() : "") + (dr["FLDTRAVELPIC2EMAIL"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPIC2EMAIL"].ToString() : "");

                ucTravelPIC3.SelectedValue = dr["FLDTRAVELPIC3"].ToString();
                ucTravelPIC3.Text = dr["FLDTRAVELPIC3NAME"].ToString() + (dr["FLDTRAVELPIC3DESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPIC3DESIGNATION"].ToString() : "") + (dr["FLDTRAVELPIC3"].ToString() != string.Empty ? " / " + dr["FLDTRAVELPIC3"].ToString() : "");

                ucQualityGM.SelectedValue = dr["FLDQUALITYGENERALMANAGER"].ToString();
                ucQualityGM.Text = dr["FLDQUALITYGENERALMANAGERNAME"].ToString() + (dr["FLDQUALITYGENERALMANAGERDESIGNATION"].ToString() != string.Empty ? " / " + dr["FLDQUALITYGENERALMANAGERDESIGNATION"].ToString() : "") + (dr["FLDQUALITYGENERALMANAGEREMAIL"].ToString() != string.Empty ? " / " + dr["FLDQUALITYGENERALMANAGEREMAIL"].ToString() : "");

                if (ds.Tables[0].Rows[0]["FLDUSESUPPLIERCONFIGYN"].ToString() == "0")
                    chkSupplierConfig.Checked = false;
                else
                    chkSupplierConfig.Checked = true;
            }
        }
    }
    protected void MenuVesselOfficeAdmin_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["vesselid"] != null && !string.IsNullOrEmpty(ViewState["vesselid"].ToString()))
                {
                    PhoenixRegistersVesselOfficeAdmin.OfficeAdminDetailsUpdate(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , int.Parse(ViewState["vesselid"].ToString())
                                    , General.GetNullableInteger(ucAccountsExecutive.SelectedValue)
                                    , General.GetNullableString("")
                                    , chkSupplierConfig.Checked == true ? 1 : 0
                                   , General.GetNullableInteger(ucPurchaseSupdt.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucQualitySupdtDesignation.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucQualityDirector.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucTravelPIC2.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucTravelPIC3.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucQualityGM.SelectedValue)
                                    , General.GetNullableString("")
                                    );

                    ucStatus.Text = "Office Admin details updated.";
                    ucStatus.Visible = true;
                }
                else
                {
                    PhoenixRegistersVesselOfficeAdmin.OfficeAdminDetailsInsert(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , int.Parse(Filter.CurrentVesselMasterFilter.ToString())
                                       , General.GetNullableInteger(ucAccountsExecutive.SelectedValue)
                                    , General.GetNullableString("")
                                    , chkSupplierConfig.Checked == true ? 1 : 0
                                    , General.GetNullableInteger(ucPurchaseSupdt.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucQualitySupdtDesignation.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucQualityDirector.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucTravelPIC2.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucTravelPIC3.SelectedValue)
                                    , General.GetNullableString("")
                                    , General.GetNullableInteger(ucQualityGM.SelectedValue)
                                    , General.GetNullableString("")
                                    );

                    ucStatus.Text = "Office Admin details updated.";
                    ucStatus.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDSUPPLIERCODE", "FLDNAME", "FLDVESSELDISCOUNTPERCENTAGE" };
        string[] alCaptions = { "Vessel Name", "SupplierCode", "Supplier Name", "%Return" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (Filter.CurrentVesselMasterFilter != null && !string.IsNullOrEmpty(Filter.CurrentVesselMasterFilter.ToString()))
        {
            DataSet ds = PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapSearch(General.GetNullableInteger(Filter.CurrentVesselMasterFilter.ToString())
                , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvVesselAdminUser.PageSize, ref iRowCount, ref iTotalPageCount, 1);

            General.SetPrintOptions("gvVesselAdminUser", "Discount", alCaptions, alColumns, ds);
            gvVesselAdminUser.DataSource = ds;
            gvVesselAdminUser.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
    }
    protected void Rebind()
    {
        gvVesselAdminUser.SelectedIndexes.Clear();
        gvVesselAdminUser.EditIndexes.Clear();
        gvVesselAdminUser.DataSource = null;
        gvVesselAdminUser.Rebind();
    }

    protected void gvVesselAdminUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidData((((UserControlDesignation)e.Item.FindControl("ucDesignation")).SelectedDesignation),
                    ((UserControlUserName)e.Item.FindControl("ucPIC")).SelectedUser))
                {
                    ucError.Visible = true;
                    return;
                }
                int UserDesignation = int.Parse(((UserControlDesignation)e.Item.FindControl("ucDesignation")).SelectedDesignation);
                int PICUserId = int.Parse(((UserControlMultiColumnUser)e.Item.FindControl("ucPIC")).SelectedValue);
                ucStatus.Text = "Vessel Admin Uesr Information added.";
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(((UserControlDesignation)e.Item.FindControl("ucDesignationEdit")).SelectedDesignation,
                                    ((UserControlMultiColumnUser)e.Item.FindControl("UcPersonOfficeId")).SelectedValue))
                {

                    ucError.Visible = true;
                    return;
                }
                int UserDesignation = int.Parse(((UserControlDesignation)e.Item.FindControl("ucDesignationEdit")).SelectedDesignation);
                int PICUserId = int.Parse(((UserControlMultiColumnUser)e.Item.FindControl("UcPersonOfficeId")).SelectedValue);
                Guid designationinvoiceid = new Guid(((RadLabel)e.Item.FindControl("lblDesignationInvoiceId")).Text);

                if (((RadLabel)e.Item.FindControl("lblVesselAdminUserMapCodeEdit")).Text == "")
                {
                    InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Filter.CurrentVesselMasterFilter.ToString()), UserDesignation, PICUserId, designationinvoiceid);
                }
                else
                {
                    Guid VesselAdminUserMapCode = new Guid(((RadLabel)e.Item.FindControl("lblVesselAdminUserMapCodeEdit")).Text);
                    UpdateVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode, UserDesignation, PICUserId);
                }
                ucStatus.Text = "Vessel Admin Uesr Information updated.";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid VesselAdminUserMapCode = new Guid(((RadLabel)e.Item.FindControl("lblVesselAdminUserMapCode")).Text);
                PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode);
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
    protected void gvVesselAdminUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselAdminUser.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselAdminUser_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            UserControlDesignation ucDesignationEdit = (UserControlDesignation)e.Item.FindControl("ucDesignationEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucDesignationEdit != null) ucDesignationEdit.SelectedDesignation = drv["FLDDESIGNATIONID"].ToString();

            UserControlMultiColumnUser officestaffid = (UserControlMultiColumnUser)e.Item.FindControl("UcPersonOfficeId");
            if (officestaffid != null)
            {
                officestaffid.Text= drv["FLDPICUSERNAME"].ToString();
                officestaffid.SelectedValue = drv["FLDPICUSERID"].ToString();
            }
        }


    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

    }

    private void InsertVesselAdminUser(int rowusercode, int? Vesselid, int DesignationId, int PICUserId, Guid designationinvoiceid)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapInsert(rowusercode, Vesselid, null, DesignationId, PICUserId, designationinvoiceid);
    }
    private void UpdateVesselAdminUser(int rowusercode, Guid VesselAdminUserMapCode, int DesignationId, int PICUserId)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapUpdate(rowusercode, VesselAdminUserMapCode, DesignationId, PICUserId);
    }
    private bool IsValidData(string designationid, string userid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (designationid.Trim().Equals("") || designationid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Designation is required.";

        if (userid.Trim().Equals("") || userid.Trim().Equals("Dummy") || General.GetNullableInteger(userid) == null)
            ucError.ErrorMessage = "PIC User is required.";
        return (!ucError.IsError);
    }
    private void DeleteVesselAdminUser(int rowusercode, Guid VesselAdminUserMapCode)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(rowusercode, VesselAdminUserMapCode);
    }
    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }

}

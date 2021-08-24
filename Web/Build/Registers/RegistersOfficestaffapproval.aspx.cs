using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOfficestaffapproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);          
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "BACK");
            toolbarmain.AddButton("Office Staff", "OFFICESTAFF");
            toolbarmain.AddButton("Family", "FAMILY");
            toolbarmain.AddButton("Approver", "APPROVERLIST");
            MenuOfficestaff.AccessRights = this.ViewState;
            MenuOfficestaff.MenuList = toolbarmain.Show();
            MenuOfficestaff.SelectedMenuIndex = 3;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENO"] = Request.QueryString["page"].ToString();
                ViewState["OFFICESTAFFID"] = Request.QueryString["OFFICESTAFFID"].ToString();
                OfficeStaffEdit(int.Parse(ViewState["OFFICESTAFFID"].ToString()));
                ddlAddType.Items.Clear();
                ddlAddType.DataSource = PhoenixRegistersOfficeStaff.ListOfficeStaffApprovalType(1);
                ddlAddType.DataTextField = "FLDNAME";
                ddlAddType.DataValueField = "FLDTYPEID";
                ddlAddType.DataBind();
                ddlAddType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    private void OfficeStaffEdit(int Staffid)
    {
        try
        {
            DataSet ds = PhoenixRegistersOfficeStaff.EditOfficeStaff(Staffid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtEmployeeNo.Text = dr["FLDEMPLOYEENUMBER"].ToString();
                txtLastname.Text = dr["FLDOFFICESURNAME"].ToString();
                txtfirstname.Text = dr["FLDOFFICEFIRSTNAME"].ToString();
                txtmiddlename.Text = dr["FLDMIDDLENAME"].ToString();
                txtUsername.Text = dr["FLDUSERNAME"].ToString();
                txtSalutation.Text = dr["FLDSALUTATION"].ToString();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOfficestaff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Registers/RegistersOfficeStaff.aspx?page=" + ViewState["PAGENO"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("OFFICESTAFF"))
            {
                Response.Redirect("../Registers/RegistersOfficeStaffList.aspx?OFFICESTAFFID=" + ViewState["OFFICESTAFFID"].ToString() + "&page=" + ViewState["PAGENO"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("APPROVERLIST"))
            {
                Response.Redirect("../Registers/RegistersOfficestaffapproval.aspx?OFFICESTAFFID=" + ViewState["OFFICESTAFFID"].ToString() + "&page=" + ViewState["PAGENO"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("FAMILY"))
            {
                Response.Redirect("../Registers/RegistersOfficeStaffFamily.aspx?OFFICESTAFFID=" + ViewState["OFFICESTAFFID"].ToString() + "&page=" + ViewState["PAGENO"].ToString(), false);
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
        int addType = 0;
        if (ddlAddType.SelectedIndex > 0)
            addType = General.GetNullableInteger(ddlAddType.SelectedValue).Value;
        DataSet ds = PhoenixRegistersOfficeStaff.ListOfficeStaffApproval(int.Parse(ViewState["OFFICESTAFFID"].ToString()),addType);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewComponentTrack.DataSource = ds.Tables[0];           
        }
        else
        {
            gvCrewComponentTrack.DataSource = "";
        }
    }    

    private bool IsValidComponent(string approverid,string Level,string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (approverid.Trim().Equals(""))
            ucError.ErrorMessage = " Approver is required.";

        if (Level.Trim().Equals("0"))
            ucError.ErrorMessage = "Level is required.";

        if (type.Trim().Equals("0"))
            ucError.ErrorMessage = "Type is required.";
        return (!ucError.IsError);
    }
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCrewComponentTrack_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {             
                string approverid = ((RadTextBox)e.Item.FindControl("txtAddStaffId")).Text;
                string level = ((RadComboBox)e.Item.FindControl("ddlAddLevel")).SelectedValue.ToString();                
                if (!IsValidComponent(approverid, level, ddlAddType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersOfficeStaff.InsertOfficeStaffApproval(int.Parse(approverid), int.Parse(level), int.Parse(ddlAddType.SelectedValue), int.Parse(ViewState["OFFICESTAFFID"].ToString()));
                BindData();
                gvCrewComponentTrack.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = General.GetNullableGuid(((GridEditableItem)e.Item).GetDataKeyValue("FLDCONFIGURATIONID").ToString()).Value ;
                PhoenixRegistersOfficeStaff.DeleteOfficeStaffApprovalConfiguration(id);
                BindData();
                gvCrewComponentTrack.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {                
                string approverid = ((RadTextBox)e.Item.FindControl("txtEditStaffId")).Text;
                Guid id = General.GetNullableGuid(((GridEditableItem)e.Item).GetDataKeyValue("FLDCONFIGURATIONID").ToString()).Value;
                string level = ((RadComboBox)e.Item.FindControl("ddleditlevel")).SelectedValue.ToString();                
                if (!IsValidComponent(approverid, level, ddlAddType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersOfficeStaff.UpdateOfficeStaffApproval(int.Parse(approverid), int.Parse(level), int.Parse(ddlAddType.SelectedValue), int.Parse(ViewState["OFFICESTAFFID"].ToString()), id);                
                BindData();
                gvCrewComponentTrack.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewComponentTrack_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewComponentTrack_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
            RadComboBox ddleditlevel = (RadComboBox)e.Item.FindControl("ddleditlevel");
            if (ddleditlevel != null)
            {
                ddleditlevel.SelectedValue = drv["FLDLEVEL"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            // DropDownList ddlAddType = (DropDownList)e.Row.FindControl("ddlAddType");
            //ddlAddType.Items.Clear();
            //ddlAddType.DataSource = PhoenixRegistersOfficeStaff.ListOfficeStaffApprovalType(1);
            //ddlAddType.DataTextField = "FLDNAME";
            //ddlAddType.DataValueField = "FLDTYPEID";
            //ddlAddType.DataBind();
            //ddlAddType.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }
}

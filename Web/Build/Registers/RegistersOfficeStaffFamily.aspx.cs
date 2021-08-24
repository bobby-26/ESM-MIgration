using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOfficeStaffFamily : PhoenixBasePage
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
            MenuOfficestaff.SelectedMenuIndex = 2;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENO"] = Request.QueryString["page"].ToString();
                ViewState["OFFICESTAFFID"] = Request.QueryString["OFFICESTAFFID"].ToString();
                OfficeStaffEdit(int.Parse(ViewState["OFFICESTAFFID"].ToString()));
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
        DataSet ds = PhoenixRegistersOfficeStaff.ListOfficeStaffFamliy(int.Parse(ViewState["OFFICESTAFFID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewComponentTrack.DataSource = ds.Tables[0];          
        }
        else
        {
            gvCrewComponentTrack.DataSource = "";
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewComponentTrack.Rebind();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCrewComponentTrack_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

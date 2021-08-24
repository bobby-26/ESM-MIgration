using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersTravelClaimapprovalPIC : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Approver", "APPROVERLIST", ToolBarDirection.Right);
            toolbarmain.AddButton("Back", "BACK",ToolBarDirection.Right);
            MenuOfficestaff.Title = "Approver List";
            MenuOfficestaff.AccessRights = this.ViewState;
            MenuOfficestaff.MenuList = toolbarmain.Show();
            MenuOfficestaff.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ViewState["PAGENO"] = Request.QueryString["page"].ToString();
                ViewState["OFFICESTAFFID"] = Request.QueryString["OFFICESTAFFID"].ToString();
              
                OfficeStaffEdit(int.Parse(ViewState["OFFICESTAFFID"].ToString()));
                ddlAddType.Items.Clear();
                ddlAddType.DataSource = PhoenixRegistersOfficeStaff.ListOfficeStaffApprovalType(1);
                ddlAddType.DataTextField = "FLDNAME";
                ddlAddType.DataValueField = "FLDTYPEID";
                ddlAddType.DataBind();
                ddlAddType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
                ddlAddType.SelectedValue = "2";
                ddlAddType.Enabled = false;
            }
         
         //  BindData();
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
                Response.Redirect("../Registers/RegistersPICtravelclaim.aspx?page=" + ViewState["PAGENO"].ToString(), false);
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
        DataSet ds = PhoenixRegistersOfficeStaff.ListOfficeStaffApproval(int.Parse(ViewState["OFFICESTAFFID"].ToString()),int.Parse(ddlAddType.SelectedValue));

        // if (ds.Tables[0].Rows.Count > 0)
        //{
              gvCrewComponentTrack.DataSource = ds.Tables[0];
           // gvCrewComponentTrack.DataBind();
       // }

    }
    //protected void gvCrewComponentTrack_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    //protected void gvCrewComponentTrack_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string approverid= ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEditStaffId")).Text;
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        string level = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddleditlevel")).SelectedValue.ToString();
    //       // string Type= ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddleditType")).SelectedValue.ToString();
    //        if (!IsValidComponent(approverid, level, ddlAddType.SelectedValue))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixRegistersOfficeStaff.UpdateOfficeStaffApproval(int.Parse(approverid), int.Parse(level), int.Parse(ddlAddType.SelectedValue), int.Parse(ViewState["OFFICESTAFFID"].ToString()), id);

    //        _gridView.EditIndex = -1;
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvCrewComponentTrack_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;
    //    BindData();
    //}
    protected void Rebind()
    {
        gvCrewComponentTrack.SelectedIndexes.Clear();
        gvCrewComponentTrack.EditIndexes.Clear();
        gvCrewComponentTrack.DataSource = null;
        gvCrewComponentTrack.Rebind();
    }
    protected void gvCrewComponentTrack_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                string approverid = ((RadTextBox)e.Item.FindControl("txtAddStaffId")).Text;
                string level = ((RadComboBox)e.Item.FindControl("ddlAddLevel")).SelectedValue.ToString();

                if (!IsValidComponent(approverid, level, ddlAddType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersOfficeStaff.InsertOfficeStaffApproval(int.Parse(approverid), int.Parse(level), int.Parse(ddlAddType.SelectedValue), int.Parse(ViewState["OFFICESTAFFID"].ToString()));
                Rebind();


            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                string approverid = ((RadTextBox)e.Item.FindControl("txtEditStaffId")).Text;             
                Guid id =   Guid.Parse(((RadLabel)e.Item.FindControl("lblconfigurationidedit")).Text);
                string level = ((RadComboBox)e.Item.FindControl("ddleditlevel")).SelectedValue.ToString();
                // string Type= ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddleditType")).SelectedValue.ToString();
                if (!IsValidComponent(approverid, level, ddlAddType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersOfficeStaff.UpdateOfficeStaffApproval(int.Parse(approverid), int.Parse(level), int.Parse(ddlAddType.SelectedValue), int.Parse(ViewState["OFFICESTAFFID"].ToString()),id);

               
                Rebind();


            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblconfigurationid")).Text);
                PhoenixRegistersOfficeStaff.DeleteOfficeStaffApprovalConfiguration(id);
                Rebind();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvCrewComponentTrack_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
            
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            GridView _gridView = (GridView)sender;
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //            string approverid = ((TextBox)_gridView.FooterRow.FindControl("txtAddStaffId")).Text;
    //            string level = ((DropDownList)_gridView.FooterRow.FindControl("ddlAddLevel")).SelectedValue.ToString();
                
    //            if (!IsValidComponent(approverid, level, ddlAddType.SelectedValue))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            PhoenixRegistersOfficeStaff.InsertOfficeStaffApproval(int.Parse(approverid), int.Parse(level), int.Parse(ddlAddType.SelectedValue), int.Parse(ViewState["OFFICESTAFFID"].ToString()));
    //            BindData();
    //        }        
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvCrewComponentTrack_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = de.RowIndex;
    //    Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //    PhoenixRegistersOfficeStaff.DeleteOfficeStaffApprovalConfiguration(id);
    //    BindData();
       
    //}

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
    protected void gvCrewComponentTrack_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewComponentTrack_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;         
            RadComboBox ddleditlevel = (RadComboBox)e.Item.FindControl("ddleditlevel");
            if (ddleditlevel != null)
            {
                ddleditlevel.SelectedValue = drv["FLDLEVEL"].ToString();
            }
            //
            ImageButton imgEdituser = (ImageButton)e.Item.FindControl("imgEdituser");
            if (imgEdituser != null)
                imgEdituser.Attributes.Add("onclick", "return showPickList('spnPickListOfficeStaffedit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOfficeStaff.aspx', true); ");
            
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton imgadduser = (ImageButton)e.Item.FindControl("imgadduser");
            if (imgadduser != null)
                imgadduser.Attributes.Add("onclick", "return showPickList('spnPickListOfficeStaff', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOfficeStaff.aspx', true); ");
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
    }

}

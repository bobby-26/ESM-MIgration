using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Telerik.Web.UI;


public partial class DryDockProjectAdd : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // onclick = "return showPickList('spnPickListMakerEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132', true);"

            SessionUtil.PageAccessRights(this.ViewState);
           
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuProject.AccessRights = this.ViewState;
            MenuProject.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["VSLID"] = Request.QueryString["vslid"];
                ViewState["ProjectID"] = null;
                if (Request.QueryString["ProjectID"] != null)
                {
                    ViewState["ProjectID"] = Request.QueryString["ProjectID"];
                    EditProject(new Guid(ViewState["ProjectID"].ToString()));
                }

                if (Request.QueryString["ProjectID"] == null)
                {
                    NextProject();
                }

                LoadStatus();
                LoadCategory();
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    private void NextProject()
    {
        try
        {
            DataTable dt = PhoenixDryDockSchedule.GetNextDryDockSchedule(int.Parse(ViewState["VSLID"].ToString()));
            DataRow dr = dt.Rows[0];

            ucFromDate.Text = dr["FLDFROMDATE"].ToString();
            ucToDate.Text = dr["FLDTODATE"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            ucOrderStatus.SelectedValue = dr["FLDSTATUS"].ToString();
            ViewState["OPERATIONMODE"] = "EDIT";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void EditProject(Guid Projectid)
    {
        DataSet ds = PhoenixDryDockOrder.EditDryDockOrder(int.Parse(ViewState["VSLID"].ToString()), Projectid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtTitle.Text = dr["FLDTITLE"].ToString();
            txtRefNo.Text = dr["FLDNUMBER"].ToString();
            ucCreatedDate.Text = dr["FLDCREATEDDATE"].ToString();
            ucFromDate.Text = dr["FLDFROMDATE"].ToString();
            ucToDate.Text = dr["FLDTODATE"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            ucOrderStatus.SelectedValue = dr["FLDSTATUS"].ToString();
            ucWindowPeriod.Text = dr["FLDWINDOWPERIOD"].ToString();
            txtStartDate.Text = dr["FLDACTUALSTARTDATE"].ToString();
            txtFinishDate.Text = dr["FLDACTUALFINISHDATE"].ToString();
            if (dr["FLDCATEGORYID"].ToString() != string.Empty)
                ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString();
            txtHFOCost.Text = FormatDecimal2Places(dr["FLDHFOCOST"].ToString());
            txtHFOConsumption.Text = FormatDecimal2Places(dr["FLDHFOCONSUMPTION"].ToString());
            txtMDOCost.Text = FormatDecimal2Places(dr["FLDMDOCOST"].ToString());
            txtMDOConsumption.Text = FormatDecimal2Places(dr["FLDMDOCONSUMPTION"].ToString());
        }
    }
    private string FormatDecimal2Places(string value)
    {
        decimal? d = General.GetNullableDecimal(value);
        if (d == null)
            return "0.00";

        return String.Format("{0:f2}", d);
    }
    private void LoadStatus()
    {
        try
        {
            DataSet ds = PhoenixDryDockSchedule.ListDryDockStatus();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ucOrderStatus.DataSource = ds.Tables[0];
                ucOrderStatus.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void LoadCategory()
    {
        try
        {
            DataTable dt = PhoenixDryDockCategory.ListDryDockCategory(1);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new DropDownListItem("--Select--", "Dummy"));
            if (dt.Rows.Count > 0)
            {
                ddlCategory.DataSource = dt;
                ddlCategory.DataBind();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Project_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidProject())
                {
                    ucError.Visible = true;
                    return;
                }


                if (ViewState["ProjectID"] == null)
                {
                    PhoenixDryDockOrder.InsertDryDockOrder(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , int.Parse(ViewState["VSLID"].ToString())
                                                        , txtTitle.Text
                                                        , General.GetNullableDateTime(ucFromDate.Text)
                                                        , General.GetNullableDateTime(ucToDate.Text)
                                                        , General.GetNullableInteger(ucOrderStatus.SelectedValue)
                                                        , General.GetNullableInteger(ucWindowPeriod.Text)
                                                        , General.GetNullableString(txtRemarks.Text)
                                                        , General.GetNullableDateTime(txtStartDate.Text).HasValue ? General.GetNullableDateTime(txtStartDate.Text).Value : General.GetNullableDateTime(ucFromDate.Text).Value
                                                        , General.GetNullableDateTime(txtFinishDate.Text)
                                                        , int.Parse(ddlCategory.SelectedValue)
                                                        , General.GetNullableDecimal(txtHFOCost.Text)
                                                        , General.GetNullableDecimal(txtHFOConsumption.Text)
                                                        , General.GetNullableDecimal(txtMDOCost.Text)
                                                        , General.GetNullableDecimal(txtMDOConsumption.Text));

                }
                else
                {

                    PhoenixDryDockOrder.UpdateDryDockOrder(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , new Guid(ViewState["ProjectID"].ToString())
                                                        , int.Parse(ViewState["VSLID"].ToString())
                                                        , txtTitle.Text
                                                        , General.GetNullableDateTime(ucFromDate.Text)
                                                        , General.GetNullableDateTime(ucToDate.Text)
                                                        , General.GetNullableInteger(ucOrderStatus.SelectedValue)
                                                        , General.GetNullableInteger(ucWindowPeriod.Text)
                                                        , General.GetNullableString(txtRemarks.Text)
                                                        , General.GetNullableDateTime(txtStartDate.Text)
                                                        , General.GetNullableDateTime(txtFinishDate.Text)
                                                        , int.Parse(ddlCategory.SelectedValue)
                                                        , General.GetNullableDecimal(txtHFOCost.Text)
                                                        , General.GetNullableDecimal(txtHFOConsumption.Text)
                                                        , General.GetNullableDecimal(txtMDOCost.Text)
                                                        , General.GetNullableDecimal(txtMDOConsumption.Text));
                    ucStatus.Text = "Details Updated.";
                }

               

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

    }
    private bool IsValidProject()
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required.";

        if (General.GetNullableDateTime(ucFromDate.Text) == null)
            ucError.ErrorMessage = "From Date cannot be blank.";

        if (General.GetNullableDateTime(ucToDate.Text) == null)
            ucError.ErrorMessage = "To Date cannot be blank.";

        if (ucOrderStatus.SelectedValue == "0" && !General.GetNullableDateTime(txtFinishDate.Text).HasValue)
        {
            ucError.ErrorMessage = "Finish Date cannot be blank when closing a project.";
        }

        if (!General.GetNullableInteger(ddlCategory.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Docking Category is required.";
        }
        if (General.GetNullableDateTime(txtStartDate.Text) != null & General.GetNullableDateTime(txtFinishDate.Text) != null)
        {
            if (General.GetNullableDateTime(txtStartDate.Text) > General.GetNullableDateTime(txtFinishDate.Text))
            {
                ucError.ErrorMessage = "Enter Valid start and finish dates.";
            }
        }
        if (ucWindowPeriod.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Window Period is required.";

        return (!ucError.IsError);
    }

   
}

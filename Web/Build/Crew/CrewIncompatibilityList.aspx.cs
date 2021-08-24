using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;


public partial class CrewIncompatibilityList : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Compatibility", "COMPATIBILITY");
            toolbarmain.AddButton("Employee List ", "EMPLOYEE");
            CrewIncidents.MenuList = toolbarmain.Show();
            CrewIncidents.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuIncompatibilty.AccessRights = this.ViewState;
            MenuIncompatibilty.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["INCOMPID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["CREWLIST"] = string.Empty;

                cblVesselType.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);


                //cblVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(0);
                //cblVesselType.DataTextField = "FLDTYPEDESCRIPTION";
                //cblVesselType.DataValueField = "FLDVESSELTYPEID";
                //cblVesselType.DataBind();

                SetEmployeePrimaryDetails();

                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewIncidents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("COMPATIBILITY"))
        {
            Response.Redirect("CrewIncompatibilityList.aspx?empid=" + Request.QueryString["empid"], false);            
        }
        else if (CommandName.ToUpper().Equals("EMPLOYEE"))
        {
            Response.Redirect("CrewIncompatibilityCrewList.aspx?empid=" + Request.QueryString["empid"], false);            
        }
    }

    protected void CrewIncompatibilty_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //StringBuilder strvesseltype = new StringBuilder();
                //foreach (ListItem item in cblVesselType.Items)
                //{
                //    if (item.Selected == true)
                //    {
                //        strvesseltype.Append(item.Value.ToString());
                //        strvesseltype.Append(",");
                //    }
                //}

                //if (strvesseltype.Length > 1)
                //{
                //    strvesseltype.Remove(strvesseltype.Length - 1, 1);
                //}
                if (!IsValidCrewIncompatibilty(cblVesselType.SelectedVesseltype))
                //if (!IsValidCrewIncompatibilty(strvesseltype.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["INCOMPID"].ToString() == "")
                {
                    PhoenixCrewIncompatibility.InsertEmployeeIncompatibility(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            Convert.ToInt32(strEmployeeId), cblVesselType.SelectedVesseltype, General.GetNullableString(txtRemarks.Text), 1);

                }
                else
                {
                    PhoenixCrewIncompatibility.UpdateEmployeeIncompatibility(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            int.Parse(ViewState["INCOMPID"].ToString()), Convert.ToInt32(strEmployeeId), cblVesselType.SelectedVesseltype,
                            General.GetNullableString(txtRemarks.Text), 1);
                }
                txtRemarks.Text = "";
               // cblVesselType.SelectedIndex = -1;
                cblVesselType.SelectedVesseltype = string.Empty;
                ViewState["INCOMPID"] = "";
                BindIncompatibiltyData();
                gvIncompatibility.Rebind();
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewIncompatibility.ListIncompatibility(int.Parse(strEmployeeId));
        
        gvCrewList.DataSource = dt;
        gvCrewList.VirtualItemCount = iRowCount;   

        iRowCount = dt.Rows.Count;
        iTotalPageCount = 1;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;        
    }

    private bool IsValidCrewIncompatibilty(string vesseltype)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (vesseltype.Trim().Length == 0)
            ucError.ErrorMessage = "Select Atleast one vessel type.";

        if (txtRemarks.Text.Equals(""))
            ucError.ErrorMessage = "Remarks is required.";


        return (!ucError.IsError);
    }

      
    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }
    }
    
    protected void gvCrewList_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem Item = e.Item as GridDataItem;

            string empid = ((RadLabel)Item.FindControl("lblEmployeeid")).Text;

            PhoenixCrewIncompatibility.DeleteIncompatibility(int.Parse(strEmployeeId), int.Parse(empid));
            BindData();
            gvCrewList.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    
    private void BindIncompatibiltyData()
    {

        DataSet ds = PhoenixCrewIncompatibility.ListEmployeeIncompatibility(int.Parse(strEmployeeId), null);

        gvIncompatibility.DataSource = ds;

    }
    protected void gvIncompatibility_EditCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem Item = e.Item as GridDataItem;
        ViewState["INCOMPID"] = ((RadLabel)Item.FindControl("lblIncompid")).Text;
        CrewIncompEdit(int.Parse(ViewState["INCOMPID"].ToString()));
        BindIncompatibiltyData();
    }

    private void CrewIncompEdit(int incompid)
    {
        //if (cblVesselType.SelectedIndex != "")
        //{
        //    cblVesselType.SelectedIndex = -1;
        //}

        DataSet ds = PhoenixCrewIncompatibility.EditEmployeeIncompatibility(incompid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            string[] vsltype = dr["FLDVESSELTYPEID"].ToString().Split(',');

            cblVesselType.SelectedVesseltype = (dr["FLDVESSELTYPEID"].ToString() == null) ? "" : dr["FLDVESSELTYPEID"].ToString();
            //foreach (string item in vsltype)
            //{
            //    if (item.Trim() != "")
            //    {
            //        //if (cblVesselType.Items.FindByValue(item) != null)
            //        //{
            //        //    cblVesselType.Items.FindByValue(item).Selected = true;
            //        //}
            //    }
            //}
        }
    }

    protected void gvIncompatibility_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }
    }


    protected void gvIncompatibility_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem Item = e.Item as GridDataItem;

            string lblIncompid = ((RadLabel)Item.FindControl("lblIncompid")).Text;

            PhoenixCrewIncompatibility.DeleteEmployeeIncompatibility(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(lblIncompid));
            BindIncompatibiltyData();
            gvIncompatibility.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvIncompatibility_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindIncompatibiltyData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }


}
